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

namespace ManagedDotnetProfiler
{
    internal unsafe partial class CorProfiler : CorProfilerCallback10Base
    {
        private ConcurrentDictionary<AssemblyId, bool> _assemblyLoads = new();

        public static CorProfiler Instance { get; private set; }

        public static ConcurrentQueue<string> Logs { get; } = new();

        public bool GetThreadId(ulong expectedThreadId, int expectedOsId)
        {
            var result = ICorProfilerInfo.GetCurrentThreadId(out var threadId);

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
                result = ICorProfilerInfo.GetThreadInfo(threadId, out osId);
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

            Log($"Setting event mask to {eventMask}");

            return ICorProfilerInfo11.SetEventMask(eventMask);
        }

        protected override HResult JITCompilationStarted(FunctionId functionId, bool fIsSafeToBlock)
        {
            ICorProfilerInfo2.GetFunctionInfo(functionId, out var classId, out var moduleId, out var mdToken);

            ICorProfilerInfo2.GetModuleMetaData(moduleId, CorOpenFlags.ofRead, KnownGuids.IMetaDataImport, out var metaDataImport);

            metaDataImport.GetMethodProps(new MdMethodDef(mdToken), out var typeDef, null, 0, out var size, out _, out _, out _, out _, out _);

            var buffer = new char[size];

            fixed (char* p = buffer)
            {
                metaDataImport.GetMethodProps(new MdMethodDef(mdToken), out _, p, size, out _, out _, out _, out _, out _, out _);
            }

            var methodName = new string(buffer);

            metaDataImport.GetTypeDefProps(typeDef, null, out size, out _, out _);

            buffer = new char[size];

            metaDataImport.GetTypeDefProps(typeDef, buffer, out _, out _, out _);

            var typeName = new string(buffer);

            Log($"JITCompilationStarted: {typeName}.{methodName}");

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
            ICorProfilerInfo2.GetClassIdInfo(classId, out var moduleId, out var typeDef);
            ICorProfilerInfo2.GetModuleMetaData(moduleId, CorOpenFlags.ofRead, KnownGuids.IMetaDataImport, out var metaDataImport);

            metaDataImport.GetTypeDefProps(typeDef, null, out var nameCharCount, out _, out _);

            Span<char> buffer = stackalloc char[(int)nameCharCount];

            metaDataImport.GetTypeDefProps(typeDef, buffer, out _, out _, out _);

            Log("An exception was thrown: " + new string(buffer));

            return HResult.S_OK;
        }

        protected override HResult ExceptionSearchCatcherFound(FunctionId functionId)
        {
            ICorProfilerInfo2.GetFunctionInfo(functionId, out var classId, out var moduleId, out var mdToken);

            ICorProfilerInfo2.GetModuleMetaData(moduleId, CorOpenFlags.ofRead, KnownGuids.IMetaDataImport, out var metaDataImport);

            metaDataImport.GetMethodProps(new MdMethodDef(mdToken), out _, null, 0, out var size, out _, out _, out _, out _, out _);

            var buffer = new char[size];

            MdTypeDef typeDef;

            fixed (char* p = buffer)
            {
                metaDataImport.GetMethodProps(new MdMethodDef(mdToken), out typeDef, p, size, out _, out _, out _, out _, out _, out _);
            }

            metaDataImport.GetTypeDefProps(typeDef, null, out size, out _, out _);

            var methodName = new string(buffer);

            buffer = new char[size];

            metaDataImport.GetTypeDefProps(typeDef, buffer, out _, out _, out _);

            var typeName = new string(buffer);

            Log($"Exception was caught in {typeName}.{methodName}");
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
                Error($"{assemblyId} already loading");
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
