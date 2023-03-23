using System;
using System.Diagnostics;
using System.Text;
using ProfilerLib;

namespace ManagedDotnetProfiler
{
    public unsafe class CorProfilerCallback : CorProfilerCallback2Base
    {
        private ICorProfilerInfo3 _corProfilerInfo;

        public override HResult Initialize(IntPtr pICorProfilerInfoUnk)
        {
            Console.WriteLine($"CorProfilerCallback - Initialize");


            var impl = NativeObjects.IUnknown.Wrap(pICorProfilerInfoUnk);

            var result = impl.QueryInterface(ICorProfilerInfo3.Guid, out IntPtr ptr);

            Console.WriteLine("[Profiler] Fetched ICorProfilerInfo3: " + result);

            _corProfilerInfo = NativeObjects.ICorProfilerInfo3.Wrap(ptr);

            var eventMask = CorPrfMonitor.COR_PRF_MONITOR_EXCEPTIONS | CorPrfMonitor.COR_PRF_MONITOR_JIT_COMPILATION;

            result = _corProfilerInfo.SetEventMask(eventMask);

            Console.WriteLine("[Profiler] Setting event mask to " + eventMask);

            return HResult.S_OK;
        }

        public override HResult JITCompilationStarted(FunctionId functionId, bool fIsSafeToBlock)
        {
            _corProfilerInfo.GetFunctionInfo(functionId, out var classId, out var moduleId, out var mdToken);

            _corProfilerInfo.GetModuleMetaData(moduleId, CorOpenFlags.ofRead, KnownGuids.IMetaDataImport, out var ppOut);

            IMetaDataImport metaDataImport = NativeObjects.IMetaDataImport.Wrap((IntPtr)ppOut);

            metaDataImport.GetMethodProps(new MdMethodDef(mdToken), out var typeDef, null, 0, out var size, out _, out _, out _, out _, out _);

            var buffer = new char[size];

            fixed (char* p = buffer)
            {
                metaDataImport.GetMethodProps(new MdMethodDef(mdToken), out _, p, size, out _, out _, out _, out _, out _, out _);
            }

            var methodName = new string(buffer);

            metaDataImport.GetTypeDefProps(typeDef, null, 0, out size, out _, out _);

            buffer = new char[size];

            fixed (char* p = buffer)
            {
                metaDataImport.GetTypeDefProps(typeDef, p, size, out _, out _, out _);
            }

            var typeName = new string(buffer);

            Console.WriteLine($"[Profiler] JITCompilationStarted: {typeName}.{methodName}");

            return HResult.S_OK;
        }

        public override HResult ExceptionThrown(ObjectId thrownObjectId)
        {
            Console.WriteLine("Enumerating modules");

            _corProfilerInfo.EnumModules(out void* enumerator);

            ICorProfilerModuleEnum moduleEnumerator = NativeObjects.ICorProfilerModuleEnum.Wrap((IntPtr)enumerator);

            moduleEnumerator.GetCount(out var modulesCount);

            Console.WriteLine($"Fetching {modulesCount} modules");

            var modules = new ModuleId[modulesCount];

            fixed (ModuleId* p = modules)
            {
                moduleEnumerator.Next(modulesCount, p, out modulesCount);
            }

            Console.WriteLine($"Fetched {modulesCount} modules");

            foreach (var module in modules)
            {
                _corProfilerInfo.GetModuleInfo(module, out _, 0, out uint moduleSize, null, out _);

                Span<char> moduleBuffer = stackalloc char[(int)moduleSize];

                nint baseAddress;

                fixed (char* p = moduleBuffer)
                {
                    _corProfilerInfo.GetModuleInfo(module, out baseAddress, moduleSize, out _, p, out _);
                }

                Console.WriteLine($"Module: {new string(moduleBuffer)} loaded at address {baseAddress:x2}");
            }

            _corProfilerInfo.GetClassFromObject(thrownObjectId, out var classId);
            _corProfilerInfo.GetClassIdInfo(classId, out var moduleId, out var typeDef);
            _corProfilerInfo.GetModuleMetaData(moduleId, CorOpenFlags.ofRead, KnownGuids.IMetaDataImport, out void* ppOut);

            var metaDataImport = NativeObjects.IMetaDataImport.Wrap((IntPtr)ppOut);

            metaDataImport.GetTypeDefProps(typeDef, null, 0, out var nameCharCount, out _, out _);

            Span<char> buffer = stackalloc char[(int)nameCharCount];

            fixed (char* p = buffer)
            {
                metaDataImport.GetTypeDefProps(typeDef, p, nameCharCount, out _, out _, out _);
            }

            Console.WriteLine("[Profiler] An exception was thrown: " + new string(buffer));

            return HResult.S_OK;
        }

        public override HResult ExceptionSearchCatcherFound(FunctionId functionId)
        {
            _corProfilerInfo.GetFunctionInfo(functionId, out var classId, out var moduleId, out var mdToken);

            _corProfilerInfo.GetModuleMetaData(moduleId, CorOpenFlags.ofRead, KnownGuids.IMetaDataImport, out var ppOut);

            IMetaDataImport metaDataImport = NativeObjects.IMetaDataImport.Wrap((IntPtr)ppOut);

            metaDataImport.GetMethodProps(new MdMethodDef(mdToken), out _, null, 0, out var size, out _, out _, out _, out _, out _);

            var buffer = new char[size];

            MdTypeDef typeDef;

            fixed (char* p = buffer)
            {
                metaDataImport.GetMethodProps(new MdMethodDef(mdToken), out typeDef, p, size, out _, out _, out _, out _, out _, out _);
            }

            metaDataImport.GetTypeDefProps(typeDef, null, 0, out size, out _, out _);

            var methodName = new string(buffer);

            buffer = new char[size];

            fixed (char* p = buffer)
            {
                metaDataImport.GetTypeDefProps(typeDef, p, size, out _, out _, out _);
            }

            var typeName = new string(buffer);

            Console.WriteLine($"[Profiler] Exception was caught in {typeName}.{methodName}");
            return HResult.S_OK;
        }
    }
}
