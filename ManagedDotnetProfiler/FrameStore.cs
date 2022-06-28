using System;
using System.Collections.Generic;
using System.Text;

namespace ManagedDotnetProfiler
{
    internal unsafe class FrameStore
    {
        private record struct TypeDesc(string Assembly, string Namespace, string Type);

        private readonly ICorProfilerInfo4 _corProfilerInfo;
        private readonly Dictionary<FunctionId, StackFrame> _methods = new();
        private readonly Dictionary<ClassId, TypeDesc> _types = new();

        private static readonly StackFrame UnknownStackFrame = new("Unknown-Assembly", "", "Unknown-Type", "Unknown-Method");

        public FrameStore(ICorProfilerInfo4 corProfilerInfo)
        {
            _corProfilerInfo = corProfilerInfo;
        }

        public (bool success, StackFrame frame) GetFrame(nint instructionPointer)
        {
            var hr = _corProfilerInfo.GetFunctionFromIP(instructionPointer, out var functionId);

            if (hr)
            {
                return (true, GetManagedFrame(functionId));
            }

            return (false, UnknownStackFrame);
        }

        private StackFrame GetManagedFrame(FunctionId functionId)
        {
            lock (_methods)
            {
                if (_methods.TryGetValue(functionId, out var element))
                {
                    return element;
                }
            }

            if (!GetFunctionInfo(
                functionId,
                out var mdTokenFunc,
                out var classId,
                out var moduleId,
                out var genericParametersCount,
                out var genericParameters))
            {
                return UnknownStackFrame;
            }

            if (!GetMetadataApi(moduleId, functionId, out var metaDataImport))
            {
                return UnknownStackFrame;
            }

            try
            {
                var (methodName, mdTokenType) = GetMethodName(metaDataImport, new MdMethodDef(mdTokenFunc), genericParametersCount, genericParameters);

                if (methodName.Length == 0)
                {
                    return UnknownStackFrame;
                }

                TypeDesc typeDesc;
                bool typeInCache;

                lock (_types)
                {
                    typeInCache = _types.TryGetValue(classId, out typeDesc);
                }

                if (!typeInCache)
                {
                    if (!GetTypeDesc(metaDataImport, classId, moduleId, mdTokenType, out typeDesc))
                    {
                        return UnknownStackFrame with { Function = methodName };
                    }

                    if (classId.Value != 0)
                    {
                        lock (_types)
                        {
                            _types[classId] = typeDesc;
                        }
                    }
                }

                var stackFrame = new StackFrame(typeDesc.Assembly, typeDesc.Namespace, typeDesc.Type, methodName);

                lock (_methods)
                {
                    _methods[functionId] = stackFrame;
                }

                return stackFrame;
            }
            finally
            {
                metaDataImport.Release();
            }
        }

        private bool GetTypeDesc(IMetaDataImport2 metadataImport, ClassId classId, ModuleId moduleId, MdTypeDef mdTokenType, out TypeDesc typeDesc)
        {
            typeDesc = default;

            if (!GetAssemblyName(_corProfilerInfo, moduleId, out var assemblyName))
            {
                return false;
            }

            typeDesc.Assembly = assemblyName;

            var (ns, ct) = GetManagedTypeName(_corProfilerInfo, metadataImport, moduleId, classId, mdTokenType);
            typeDesc.Namespace = ns;
            typeDesc.Type = ct;

            return true;
        }

        private static (string ns, string ct) GetManagedTypeName(
            ICorProfilerInfo4 info,
            IMetaDataImport2 metadata,
            ModuleId moduleId,
            ClassId classId,
            MdTypeDef mdTokenType)
        {
            var (ns, typeName) = GetTypeWithNamespace(metadata, mdTokenType);

            if (classId.Value == 0)
            {
                return (ns, typeName + FormatGenericParameters(metadata, mdTokenType));
            }

            var hr = info.GetClassIDInfo2(classId, out _, out var mdType, out var parentClassId, 0, out var numGenericTypeArgs, null);

            if (!hr || numGenericTypeArgs == 0)
            {
                return (ns, typeName);
            }

            var genericTypeArgs = new ClassId[numGenericTypeArgs];

            fixed (ClassId* g = genericTypeArgs)
            {
                hr = info.GetClassIDInfo2(classId, out _, out mdType, out parentClassId, numGenericTypeArgs, out numGenericTypeArgs, g);
            }

            if (!hr)
            {
                return (ns, typeName);
            }

            return (ns, typeName + FormatGenericParameters(info, numGenericTypeArgs, genericTypeArgs));
        }

        private static string FormatGenericParameters(ICorProfilerInfo4 info, uint numGenericTypeArgs, ClassId[] genericTypeArgs)
        {
            var builder = new StringBuilder();
            builder.Append('{');

            for (int currentGenericArg = 0; currentGenericArg < numGenericTypeArgs; currentGenericArg++)
            {
                var argClassId = genericTypeArgs[currentGenericArg];

                info.GetClassIDInfo2(argClassId, out var argModuleId, out var mdType, out _, 0, out _, null);

                var hr = info.GetModuleMetaData(argModuleId, CorOpenFlags.ofRead, KnownGuids.IMetaDataImport2, out var ppOut);

                if (!hr)
                {
                    builder.Append("|ns: |ct:T");
                }
                else
                {
                    var metaData = NativeStubs.IMetaDataImport2Stub.Wrap((IntPtr)ppOut);

                    var (ns, ct) = GetManagedTypeName(info, metaData, argModuleId, argClassId, mdType);
                    builder.Append($"|ns:{ns} |ct:{ct}");

                    metaData.Release();
                }

                if (currentGenericArg < numGenericTypeArgs - 1)
                {
                    builder.Append(", ");
                }
            }

            builder.Append('}');

            return builder.ToString();
        }

        private static string FormatGenericParameters(IMetaDataImport2 metadata, MdTypeDef mdTokenType)
        {
            // TODO
            return string.Empty;
        }

        private static (string ns, string typeName) GetTypeWithNamespace(IMetaDataImport2 metadata, MdTypeDef mdTokenType)
        {
            var hr = metadata.GetNestedClassProps(mdTokenType, out var mdEnclosingType);
            var isNested = hr.IsOK && metadata.IsValidToken(new MdToken(mdEnclosingType.Value));

            var enclosingType = string.Empty;
            var ns = string.Empty;

            if (isNested)
            {
                (ns, enclosingType) = GetTypeWithNamespace(metadata, mdEnclosingType);
            }

            var typeName = GetTypeNameFromMetadata(metadata, mdTokenType);

            if (typeName.Length == 0)
            {
                typeName = "?";
            }

            if (isNested)
            {
                return (ns, $"{enclosingType}.{typeName}");
            }

            var pos = typeName.LastIndexOf('.');

            if (pos == -1)
            {
                return (string.Empty, typeName);
            }

            return (typeName.Substring(0, pos), typeName.Substring(pos + 1));
        }

        private static unsafe bool GetAssemblyName(ICorProfilerInfo4 info, ModuleId moduleId, out string assemblyName)
        {
            assemblyName = string.Empty;

            var hr = info.GetModuleInfo(moduleId, out _, 0, out _, null, out var assemblyId);

            if (!hr)
            {
                return false;
            }

            hr = info.GetAssemblyInfo(assemblyId, 0, out var nameCharCount, null, out _, out _);

            if (!hr)
            {
                return false;
            }

            Span<char> buffer = stackalloc char[(int)nameCharCount];

            fixed (char* b = buffer)
            {
                hr = info.GetAssemblyInfo(assemblyId, nameCharCount, out nameCharCount, b, out _, out _);

                if (!hr)
                {
                    return false;
                }
            }

            assemblyName = new string(buffer);
            return true;
        }

        private (string methodName, MdTypeDef typeDef) GetMethodName(
            IMetaDataImport2 metaDataImport,
            MdMethodDef mdTokenFunc,
            uint genericParametersCount,
            ClassId[] genericParameters)
        {
            var (methodName, mdTokenType) = GetMethodNameFromMetadata(metaDataImport, mdTokenFunc);

            if (methodName.Length == 0 || genericParametersCount == 0)
            {
                return (methodName, mdTokenType);
            }

            var builder = new StringBuilder();

            for (int i = 0; i < genericParametersCount; i++)
            {
                var (ns, typeName) = GetManagedTypeName(_corProfilerInfo, genericParameters[i]);

                // deal with System.__Canon case
                if (typeName == "__Canon")
                {
                    builder.Append("|ns: |ct:T");
                    builder.Append(i);
                }
                else // normal namespace.type case
                {
                    builder.Append("|ns:");

                    if (ns.Length > 0)
                    {
                        builder.Append(ns);
                    }

                    builder.Append(" |ct:");
                    builder.Append(typeName);
                }

                if (i < genericParametersCount - 1)
                {
                    builder.Append(", ");
                }
            }

            builder.Append('}');

            return (methodName + builder, mdTokenType);
        }

        private static unsafe (string ns, string typeName) GetManagedTypeName(ICorProfilerInfo4 info, ClassId classId)
        {
            var hr = info.GetClassIdInfo(classId, out var moduleId, out var mdTypeToken);

            if (!hr)
            {
                return (string.Empty, "T");
            }

            hr = info.GetModuleMetaData(moduleId, CorOpenFlags.ofRead, KnownGuids.IMetaDataImport2, out var ppOut);

            if (!hr)
            {
                return (string.Empty, "T");
            }

            var metaData = NativeStubs.IMetaDataImport2Stub.Wrap((IntPtr)ppOut);

            var typeName = GetTypeNameFromMetadata(metaData, mdTypeToken);
            metaData.Release();

            if (typeName.Length == 0)
            {
                return (string.Empty, "T");
            }

            var pos = typeName.LastIndexOf('.');

            if (pos == -1)
            {
                return (string.Empty, typeName);
            }

            return (typeName.Substring(0, pos), typeName.Substring(pos + 1));
        }

        private static unsafe string GetTypeNameFromMetadata(IMetaDataImport2 metaData, MdTypeDef mdTokenType)
        {
            var hr = metaData.GetTypeDefProps(mdTokenType, null, 0, out var nameCharCount, out _, out _);

            if (!hr)
            {
                return string.Empty;
            }

            Span<char> buffer = stackalloc char[(int)nameCharCount];

            fixed (char* b = buffer)
            {
                hr = metaData.GetTypeDefProps(mdTokenType, b, nameCharCount, out nameCharCount, out _, out _);

                if (!hr)
                {
                    return string.Empty;
                }
            }

            FixTrailingGeneric(buffer);

            return new string(buffer);
        }

        private static void FixTrailingGeneric(Span<char> name)
        {
            var currentCharPos = 0;

            while (name[currentCharPos] != '\0')
            {
                if (name[currentCharPos] == '`')
                {
                    name[currentCharPos] = '\0';
                    return;
                }

                currentCharPos++;
            }
        }

        private unsafe (string methodName, MdTypeDef typeDef) GetMethodNameFromMetadata(IMetaDataImport2 metaDataImport, MdMethodDef mdTokenFunc)
        {
            var hr = metaDataImport.GetMethodProps(mdTokenFunc, out _, null, 0, out var nameCharCount, out _, out _, out _, out _, out _);

            if (!hr.IsOK)
            {
                return (string.Empty, default);
            }

            MdTypeDef mdTokenType;
            Span<char> buffer = stackalloc char[(int)nameCharCount];

            fixed (char* b = buffer)
            {
                hr = metaDataImport.GetMethodProps(mdTokenFunc, out mdTokenType, b, nameCharCount, out nameCharCount, out _, out _, out _, out _, out _);

                if (!hr.IsOK)
                {
                    return (string.Empty, default);
                }
            }

            return (new string(buffer), mdTokenType);
        }

        private unsafe bool GetMetadataApi(ModuleId moduleId, FunctionId functionId, out IMetaDataImport2 metaDataImport)
        {
            var hr = _corProfilerInfo.GetModuleMetaData(moduleId, CorOpenFlags.ofRead, KnownGuids.IMetaDataImport2, out var ppOut);

            if (!hr.IsOK)
            {
                hr = _corProfilerInfo.GetTokenAndMetaDataFromFunction(functionId, KnownGuids.IMetaDataImport2, out ppOut, out _);

                if (!hr.IsOK)
                {
                    Console.WriteLine("Failed to get metadata API");
                    metaDataImport = null;
                    return false;
                }
            }

            metaDataImport = NativeStubs.IMetaDataImport2Stub.Wrap((IntPtr)ppOut);
            return true;
        }

        private unsafe bool GetFunctionInfo(FunctionId functionId,
            out MdToken mdTokenFunc,
            out ClassId classId,
            out ModuleId moduleId,
            out uint genericParametersCount,
            out ClassId[] genericParameters)
        {
            var hr = _corProfilerInfo.GetFunctionInfo2(
                functionId,
                default,
                out classId,
                out moduleId,
                out mdTokenFunc,
                0,
                out genericParametersCount,
                null);

            if (!hr.IsOK)
            {
                genericParameters = null;
                return false;
            }

            if (genericParametersCount > 0)
            {
                genericParameters = new ClassId[genericParametersCount];

                fixed (ClassId* p = genericParameters)
                {
                    hr = _corProfilerInfo.GetFunctionInfo2(
                        functionId,
                        default,
                        out _,
                        out _,
                        out _,
                        genericParametersCount,
                        out genericParametersCount,
                        p);

                    if (!hr.IsOK)
                    {
                        return false;
                    }
                }
            }
            else
            {
                genericParameters = null;
            }

            return true;
        }
    }
}
