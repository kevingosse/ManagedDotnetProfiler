using ProfilerLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using NativeObjects;
using System.Threading;
using System.Collections.Concurrent;
using System.Runtime.CompilerServices;

namespace ManagedDotnetProfiler
{
    internal unsafe partial class CorProfiler : CorProfilerCallback10Base
    {
        private ConcurrentDictionary<AssemblyId, bool> _assemblyLoads = new();
        private ConcurrentDictionary<ClassId, bool> _classLoads = new();

        public static CorProfiler Instance { get; private set; }

        public static ConcurrentQueue<string> Logs { get; } = new();

        public bool GetThreadId(ulong expectedThreadId, int expectedOsId)
        {
            var (result, threadId) = ICorProfilerInfo.GetCurrentThreadId();

            if (!result.IsOK)
            {
                LogHResult(nameof(ICorProfilerInfo.GetCurrentThreadId), result);
                return false;
            }

            Log($"GetThreadId - expected: {expectedThreadId} - actual: {threadId.Value}");

            if (expectedThreadId != threadId.Value)
            {
                return false;
            }

            // Can't call GetThreadInfo in the CLR thread
            int osId = 0;

            Task.Run(() =>
            {
                (result, osId) = ICorProfilerInfo.GetThreadInfo(threadId);
            }).Wait();

            if (!result.IsOK)
            {
                LogHResult(nameof(ICorProfilerInfo.GetThreadInfo), result);
                return false;
            }

            Log($"GetThreadInfo - expected: {expectedOsId} - actual: {osId}");

            return true;
        }

        protected override HResult Initialize(int iCorProfilerInfoVersion)
        {
            if (iCorProfilerInfoVersion < 11)
            {
                return HResult.E_FAIL;
            }

            Instance = this;

            var eventMask = CorPrfMonitor.COR_PRF_MONITOR_ALL;
            var highEventMask = CorPrfHighMonitor.COR_PRF_HIGH_MONITOR_DYNAMIC_FUNCTION_UNLOADS;

            Log($"Setting event mask to {eventMask}");
            Log($"Setting high event mask to {highEventMask}");

            return ICorProfilerInfo11.SetEventMask2(eventMask, CorPrfHighMonitor.COR_PRF_HIGH_MONITOR_DYNAMIC_FUNCTION_UNLOADS);
        }

        protected override HResult JITCompilationStarted(FunctionId functionId, bool fIsSafeToBlock)
        {
            var (_, _, moduleId, mdToken) = ICorProfilerInfo2.GetFunctionInfo(functionId);
            var (_, metaDataImport) = ICorProfilerInfo2.GetModuleMetaData(moduleId, CorOpenFlags.ofRead, KnownGuids.IMetaDataImport);
            var (_, methodProperties) = metaDataImport.GetMethodProps(new MdMethodDef(mdToken));
            var (_, typeName, _, _) = metaDataImport.GetTypeDefProps(methodProperties.Class);

            Log($"JITCompilationStarted: {typeName}.{methodProperties.Name}");

            return HResult.S_OK;
        }

        protected override HResult ExceptionThrown(ObjectId thrownObjectId)
        {
            Log("Enumerating modules");

            ICorProfilerInfo3.EnumModules(out void* enumerator);

            var moduleEnumerator = new ICorProfilerModuleEnumInvoker((IntPtr)enumerator);

            moduleEnumerator.GetCount(out var modulesCount);

            Log($"Fetching {modulesCount} modules");

            var modules = new ModuleId[modulesCount];

            fixed (ModuleId* p = modules)
            {
                moduleEnumerator.Next(modulesCount, p, out modulesCount);
            }

            Log($"Fetched {modulesCount} modules");

            foreach (var module in modules)
            {
                var (_, moduleName, baseAddress, _) = ICorProfilerInfo.GetModuleInfo(module);

                Log($"Module: {moduleName} loaded at address {baseAddress:x2}");
            }

            ICorProfilerInfo2.GetClassFromObject(thrownObjectId, out var classId);
            var (_, moduleId, typeDef) = ICorProfilerInfo2.GetClassIdInfo(classId);
            var (_, metaDataImport) = ICorProfilerInfo2.GetModuleMetaData(moduleId, CorOpenFlags.ofRead, KnownGuids.IMetaDataImport);

            metaDataImport.GetTypeDefProps(typeDef, null, out var nameCharCount, out _, out _);

            Span<char> buffer = stackalloc char[(int)nameCharCount];

            metaDataImport.GetTypeDefProps(typeDef, buffer, out _, out _, out _);

            Log("An exception was thrown: " + new string(buffer));

            return HResult.S_OK;
        }

        protected override HResult ExceptionSearchCatcherFound(FunctionId functionId)
        {
            var (_, _, moduleId, mdToken) = ICorProfilerInfo2.GetFunctionInfo(functionId);
            var (_, metaDataImport) = ICorProfilerInfo2.GetModuleMetaData(moduleId, CorOpenFlags.ofRead, KnownGuids.IMetaDataImport);
            var (_, methodProperties) = metaDataImport.GetMethodProps(new MdMethodDef(mdToken));
            var (_, typeName, _, _) = metaDataImport.GetTypeDefProps(methodProperties.Class);
            
            Log($"Exception was caught in {typeName}.{methodProperties.Name}");
            return HResult.S_OK;
        }

        protected override HResult AppDomainCreationStarted(AppDomainId appDomainId)
        {
            var (_, appDomainName, processId) = ICorProfilerInfo.GetAppDomainInfo(appDomainId);

            Log($"AppDomainCreationStarted - {appDomainName} - Process Id {processId.Value}");

            return HResult.S_OK;
        }

        protected override HResult AppDomainCreationFinished(AppDomainId appDomainId, HResult hrStatus)
        {
            var (_, appDomainName, _) = ICorProfilerInfo.GetAppDomainInfo(appDomainId);

            Log($"AppDomainCreationFinished - {appDomainName} - HResult {hrStatus}");

            return HResult.S_OK;
        }

        protected override HResult AppDomainShutdownStarted(AppDomainId appDomainId)
        {
            // TODO: Test on .NET Framework
            var (_, appDomainName, _) = ICorProfilerInfo.GetAppDomainInfo(appDomainId);

            Log($"AppDomainShutdownStarted - {appDomainName}");

            return HResult.S_OK;
        }

        protected override HResult AppDomainShutdownFinished(AppDomainId appDomainId, HResult hrStatus)
        {
            // TODO: Test on .NET Framework
            var (_, appDomainName, _) = ICorProfilerInfo.GetAppDomainInfo(appDomainId);

            Log($"AppDomainShutdownFinished - {appDomainName} - HResult {hrStatus}");

            return HResult.S_OK;
        }

        protected override HResult AssemblyLoadStarted(AssemblyId assemblyId)
        {
            if (!_assemblyLoads.TryAdd(assemblyId, true))
            {
                Error($"Assembly {assemblyId} already loading");
            }

            return HResult.S_OK;
        }

        protected override HResult AssemblyLoadFinished(AssemblyId assemblyId, HResult hrStatus)
        {
            var (_, assemblyName, appDomainId, moduleId) = ICorProfilerInfo.GetAssemblyInfo(assemblyId);
            var (_, appDomainName, _) = ICorProfilerInfo.GetAppDomainInfo(appDomainId);
            var (_, moduleName, _, _) = ICorProfilerInfo.GetModuleInfo(moduleId);

            Log($"AssemblyLoadFinished - {assemblyName} - AppDomain {appDomainName} - Module {moduleName}");

            if (!_assemblyLoads.TryRemove(assemblyId, out _))
            {
                Error($"Saw no AssemblyLoadStarted event for {assemblyId.Value}");
            }

            return HResult.S_OK;
        }

        protected override HResult AssemblyUnloadStarted(AssemblyId assemblyId)
        {
            // TODO: Test on .NET Framework or after the ALC bug is fixed
            Log("AssemblyUnloadStarted");

            return HResult.S_OK;
        }

        protected override HResult AssemblyUnloadFinished(AssemblyId assemblyId, HResult hrStatus)
        {
            var (_, assemblyName, appDomainId, moduleId) = ICorProfilerInfo.GetAssemblyInfo(assemblyId);
            var (_, appDomainName, _) = ICorProfilerInfo.GetAppDomainInfo(appDomainId);
            var (_, moduleName, _, _) = ICorProfilerInfo.GetModuleInfo(moduleId);

            Log($"AssemblyUnloadFinished - {assemblyName} - AppDomain {appDomainName} - Module {moduleName}");

            return HResult.S_OK;
        }

        protected override HResult ClassLoadStarted(ClassId classId)
        {
            if (!_classLoads.TryAdd(classId, true))
            {
                Error($"Class {classId.Value} already loading");
            }

            return HResult.S_OK;
        }

        protected override HResult ClassLoadFinished(ClassId classId, HResult hrStatus)
        {
            var (_, moduleId, typeDef) = ICorProfilerInfo.GetClassIdInfo(classId);
            var (_, metaDataImport) = ICorProfilerInfo.GetModuleMetaData(moduleId, CorOpenFlags.ofRead, KnownGuids.IMetaDataImport);
            var (_, typeName, _, _) = metaDataImport.GetTypeDefProps(typeDef);

            Log($"ClassLoadFinished - {typeName}");

            if (!_classLoads.TryRemove(classId, out _))
            {
                Error($"Saw no ClassLoadStarted event for {classId.Value}");
            }

            return HResult.S_OK;
        }

        protected override HResult ClassUnloadStarted(ClassId classId)
        {
            var (moduleId, typeDef) = ICorProfilerInfo.GetClassIdInfo(classId).ThrowIfFailed();
            var metaDataImport = ICorProfilerInfo.GetModuleMetaData(moduleId, CorOpenFlags.ofRead, KnownGuids.IMetaDataImport).ThrowIfFailed();
            var (typeName, _, _) = metaDataImport.GetTypeDefProps(typeDef).ThrowIfFailed();

            Log($"ClassUnloadStarted - {typeName}");

            return HResult.S_OK;
        }

        protected override HResult ClassUnloadFinished(ClassId classId, HResult hrStatus)
        {
            var (_, moduleId, typeDef) = ICorProfilerInfo.GetClassIdInfo(classId);
            var (_, metaDataImport) = ICorProfilerInfo.GetModuleMetaData(moduleId, CorOpenFlags.ofRead, KnownGuids.IMetaDataImport);
            var (_, typeName, _, _) = metaDataImport.GetTypeDefProps(typeDef);

            Log($"ClassUnloadFinished - {typeName}");

            return HResult.S_OK;
        }

        protected override unsafe HResult COMClassicVTableCreated(ClassId wrappedClassId, in Guid implementedIID, void* pVTable, uint cSlots)
        {
            var (moduleId, typeDef) = ICorProfilerInfo.GetClassIdInfo(wrappedClassId).ThrowIfFailed();
            var metaDataImport = ICorProfilerInfo.GetModuleMetaData(moduleId, CorOpenFlags.ofRead, KnownGuids.IMetaDataImport).ThrowIfFailed();
            var (typeName, _, _) = metaDataImport.GetTypeDefProps(typeDef).ThrowIfFailed();

            Log($"COMClassicVTableCreated - {typeName} - {implementedIID} - {cSlots}");
            return HResult.S_OK;
        }

        protected override unsafe HResult COMClassicVTableDestroyed(ClassId wrappedClassId, in Guid implementedIID, void* pVTable)
        {
            Log("Error: the profiling API never raises this event");
            return HResult.S_OK;
        }

        protected override unsafe HResult ConditionalWeakTableElementReferences(uint cRootRefs, ObjectId* keyRefIds, ObjectId* valueRefIds, GCHandleId* rootIds)
        {
            var (_, stringLengthOffset, bufferOffset) = ICorProfilerInfo2.GetStringLayout().ThrowIfFailed();

            var stringPtr1 = (byte*)(*keyRefIds).Value;
            var str1 = new ReadOnlySpan<char>(stringPtr1 + bufferOffset, Unsafe.Read<int>(stringPtr1 + stringLengthOffset));

            var stringPtr2 = (byte*)(*valueRefIds).Value;
            var str2 = new ReadOnlySpan<char>(stringPtr2 + bufferOffset, Unsafe.Read<int>(stringPtr2 + stringLengthOffset));

            Log($"ConditionalWeakTableElementReferences - {str1} -> {str2}");

            return HResult.S_OK;
        }

        protected override unsafe HResult DynamicMethodJITCompilationStarted(FunctionId functionId, bool fIsSafeToBlock, byte* pILHeader, uint cbILHeader)
        {
            Log($"DynamicMethodJITCompilationStarted - {functionId.Value:x2}");
            return HResult.S_OK;
        }

        protected override HResult DynamicMethodJITCompilationFinished(FunctionId functionId, HResult hrStatus, bool fIsSafeToBlock)
        {
            Log($"DynamicMethodJITCompilationFinished - {functionId.Value:x2}");
            return HResult.S_OK;
        }

        protected override HResult DynamicMethodUnloaded(FunctionId functionId)
        {
            Log($"DynamicMethodUnloaded - {functionId.Value:x2}");
            return HResult.S_OK;
        }

        protected override HResult Shutdown()
        {
            Console.WriteLine("[Profiler] *** Shutting down, dumping remaining logs ***");

            while (Logs.TryDequeue(out var log))
            {
                Console.WriteLine($"[Profiler] {log}");
            }

            return HResult.S_OK;
        }

        private static void LogHResult(string function, HResult hresult)
        {
            Log($"Call to {function} failed with code {hresult}");
        }

        private static void Error(string explanation)
        {
            Log($"Error: {explanation}");
        }

        private static void Log(string line)
        {
            Logs.Enqueue(line);

            // Console.WriteLine($"[Profiler] {line}");
        }
    }
}
