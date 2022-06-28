using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace ManagedDotnetProfiler
{
    public unsafe class CorProfilerCallback2 : ICorProfilerCallback2
    {
        private static readonly Guid ICorProfilerCallback2Guid = Guid.Parse("8a8cc829-ccf2-49fe-bbae-0f022228071a");

        private readonly NativeStubs.ICorProfilerCallback2Stub _corProfilerCallback2;
        private ICorProfilerInfo4 _corProfilerInfo;
        private ManagedThreadList _managedThreadList;
        private StackSamplerLoopManager _stackSamplerLoopManager;
        private ThreadsCpuManager _threadsCpuManager;
        private StackFramesCollector _stackFramesCollector;
        private FrameStore _frameStore;
        private WallTimeProvider _wallTimeProvider;
        private PprofExporter _exporter;
        private SamplesAggregator _samplesAggregator;

        public CorProfilerCallback2()
        {
            _corProfilerCallback2 = NativeStubs.ICorProfilerCallback2Stub.Wrap(this);
        }

        public IntPtr ICorProfilerCallback2Object => _corProfilerCallback2;

        public int AddRef()
        {
            return 1;
        }

        public HResult Initialize(IntPtr pICorProfilerInfoUnk)
        {
            var impl = NativeStubs.IUnknownStub.Wrap(pICorProfilerInfoUnk);

            var result = impl.QueryInterface(KnownGuids.ICorProfilerInfo4, out IntPtr ptr);

            Console.WriteLine("[Profiler] Fetched ICorProfilerInfo4: " + result);

            _corProfilerInfo = NativeStubs.ICorProfilerInfo4Stub.Wrap(ptr);

            var eventMask = CorPrfMonitor.COR_PRF_MONITOR_EXCEPTIONS | CorPrfMonitor.COR_PRF_MONITOR_THREADS | CorPrfMonitor.COR_PRF_ENABLE_STACK_SNAPSHOT;

            result = _corProfilerInfo.SetEventMask(eventMask);

            Console.WriteLine("[Profiler] Setting event mask to " + eventMask);

            _managedThreadList = new ManagedThreadList();
            _threadsCpuManager = new ThreadsCpuManager();
            _stackFramesCollector = new StackFramesCollector(_corProfilerInfo);
            _frameStore = new FrameStore(_corProfilerInfo);
            _wallTimeProvider = new WallTimeProvider(_frameStore);
            _stackSamplerLoopManager = new StackSamplerLoopManager(_corProfilerInfo, _threadsCpuManager, _managedThreadList, _stackFramesCollector, _wallTimeProvider);
            _exporter = new PprofExporter();
            _samplesAggregator = new SamplesAggregator(_exporter);
            _samplesAggregator.Register(_wallTimeProvider);

            _stackSamplerLoopManager.Start();
            _wallTimeProvider.Start();
            _samplesAggregator.Start();

            return HResult.S_OK;
        }

        public HResult QueryInterface(in Guid guid, out IntPtr ptr)
        {
            if (guid == ICorProfilerCallback2Guid)
            {
                Console.WriteLine("[Profiler] Returning instance of ICorProfilerCallback2");
                ptr = _corProfilerCallback2;
                return HResult.S_OK;
            }

            ptr = IntPtr.Zero;
            return HResult.E_NOTIMPL;
        }

        public int Release()
        {
            return 0;
        }

        public HResult Shutdown()
        {
            return HResult.S_OK;
        }

        public HResult AppDomainCreationStarted(AppDomainId appDomainId)
        {
            throw new NotImplementedException();
        }

        public HResult AppDomainCreationFinished(AppDomainId appDomainId, HResult hrStatus)
        {
            throw new NotImplementedException();
        }

        public HResult AppDomainShutdownStarted(AppDomainId appDomainId)
        {
            throw new NotImplementedException();
        }

        public HResult AppDomainShutdownFinished(AppDomainId appDomainId, HResult hrStatus)
        {
            throw new NotImplementedException();
        }

        public HResult AssemblyLoadStarted(AssemblyId assemblyId)
        {
            throw new NotImplementedException();
        }

        public HResult AssemblyLoadFinished(AssemblyId assemblyId, HResult hrStatus)
        {
            throw new NotImplementedException();
        }

        public HResult AssemblyUnloadStarted(AssemblyId assemblyId)
        {
            throw new NotImplementedException();
        }

        public HResult AssemblyUnloadFinished(AssemblyId assemblyId, HResult hrStatus)
        {
            throw new NotImplementedException();
        }

        public HResult ModuleLoadStarted(ModuleId moduleId)
        {
            throw new NotImplementedException();
        }

        public HResult ModuleLoadFinished(ModuleId moduleId, HResult hrStatus)
        {
            throw new NotImplementedException();
        }

        public HResult ModuleUnloadStarted(ModuleId moduleId)
        {
            throw new NotImplementedException();
        }

        public HResult ModuleUnloadFinished(ModuleId moduleId, HResult hrStatus)
        {
            throw new NotImplementedException();
        }

        public HResult ModuleAttachedToAssembly(ModuleId moduleId, AssemblyId assemblyId)
        {
            throw new NotImplementedException();
        }

        public HResult ClassLoadStarted(ClassId classId)
        {
            throw new NotImplementedException();
        }

        public HResult ClassLoadFinished(ClassId classId, HResult hrStatus)
        {
            throw new NotImplementedException();
        }

        public HResult ClassUnloadStarted(ClassId classId)
        {
            throw new NotImplementedException();
        }

        public HResult ClassUnloadFinished(ClassId classId, HResult hrStatus)
        {
            throw new NotImplementedException();
        }

        public HResult FunctionUnloadStarted(FunctionId functionId)
        {
            throw new NotImplementedException();
        }

        public HResult JITCompilationStarted(FunctionId functionId, bool fIsSafeToBlock)
        {
            _corProfilerInfo.GetFunctionInfo(functionId, out var classId, out var moduleId, out var mdToken);

            _corProfilerInfo.GetModuleMetaData(moduleId, CorOpenFlags.ofRead, KnownGuids.IMetaDataImport, out var ppOut);

            IMetaDataImport metaDataImport = NativeStubs.IMetaDataImportStub.Wrap((IntPtr)ppOut);

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

        public HResult JITCompilationFinished(FunctionId functionId, HResult hrStatus, bool fIsSafeToBlock)
        {
            return HResult.S_OK;
        }

        public HResult JITCachedFunctionSearchStarted(FunctionId functionId, out bool pbUseCachedFunction)
        {
            pbUseCachedFunction = true;
            return HResult.S_OK;
        }

        public HResult JITCachedFunctionSearchFinished(FunctionId functionId, COR_PRF_JIT_CACHE result)
        {
            return HResult.S_OK;
        }

        public HResult JITFunctionPitched(FunctionId functionId)
        {
            return HResult.S_OK;
        }

        public HResult JITInlining(FunctionId callerId, FunctionId calleeId, out bool pfShouldInline)
        {
            pfShouldInline = true;
            return HResult.S_OK;
        }

        public HResult ThreadCreated(ThreadId threadId)
        {
            _managedThreadList.GetOrCreate(threadId);
            return HResult.S_OK;
        }

        public HResult ThreadDestroyed(ThreadId threadId)
        {
            if (_managedThreadList.UnregisterThread(threadId, out var threadInfo))
            {
                threadInfo.IsDestroyed = true;
            }

            return HResult.S_OK;
        }

        public HResult ThreadAssignedToOSThread(ThreadId managedThreadId, int osThreadId)
        {
            var hr = _corProfilerInfo.GetHandleFromThread(managedThreadId, out var handle);

            if (!hr.IsOK)
            {
                Console.WriteLine("GetHandleFromThread failed: " + hr);
                return hr;
            }

            var process = Process.GetCurrentProcess().Handle;

            const uint THREAD_ALL_ACCESS = 0x000F0000 | 0x00100000 | 0xFFFF;

            var success = OpSysTools.DuplicateHandle(process, handle, process, out var dupHandle, THREAD_ALL_ACCESS, false, 0);

            if (!success)
            {
                Console.WriteLine("DuplicateHandle failed");
                return HResult.E_FAIL;
            }

            _managedThreadList.SetThreadOsInfo(managedThreadId, osThreadId, dupHandle);

            return HResult.S_OK;
        }

        public HResult RemotingClientInvocationStarted()
        {
            throw new NotImplementedException();
        }

        public HResult RemotingClientSendingMessage(in Guid pCookie, bool fIsAsync)
        {
            throw new NotImplementedException();
        }

        public HResult RemotingClientReceivingReply(in Guid pCookie, bool fIsAsync)
        {
            throw new NotImplementedException();
        }

        public HResult RemotingClientInvocationFinished()
        {
            throw new NotImplementedException();
        }

        public HResult RemotingServerReceivingMessage(in Guid pCookie, bool fIsAsync)
        {
            throw new NotImplementedException();
        }

        public HResult RemotingServerInvocationStarted()
        {
            throw new NotImplementedException();
        }

        public HResult RemotingServerInvocationReturned()
        {
            throw new NotImplementedException();
        }

        public HResult RemotingServerSendingReply(in Guid pCookie, bool fIsAsync)
        {
            throw new NotImplementedException();
        }

        public HResult UnmanagedToManagedTransition(FunctionId functionId, COR_PRF_TRANSITION_REASON reason)
        {
            throw new NotImplementedException();
        }

        public HResult ManagedToUnmanagedTransition(FunctionId functionId, COR_PRF_TRANSITION_REASON reason)
        {
            throw new NotImplementedException();
        }

        public HResult RuntimeSuspendStarted(COR_PRF_SUSPEND_REASON suspendReason)
        {
            throw new NotImplementedException();
        }

        public HResult RuntimeSuspendFinished()
        {
            throw new NotImplementedException();
        }

        public HResult RuntimeSuspendAborted()
        {
            throw new NotImplementedException();
        }

        public HResult RuntimeResumeStarted()
        {
            throw new NotImplementedException();
        }

        public HResult RuntimeResumeFinished()
        {
            throw new NotImplementedException();
        }

        public HResult RuntimeThreadSuspended(ThreadId threadId)
        {
            return HResult.S_OK;
        }

        public HResult RuntimeThreadResumed(ThreadId threadId)
        {
            return HResult.S_OK;
        }

        public HResult MovedReferences(uint cMovedObjectIDRanges, ObjectId* oldObjectIDRangeStart, ObjectId* newObjectIDRangeStart, uint* cObjectIDRangeLength)
        {
            throw new NotImplementedException();
        }

        public HResult ObjectAllocated(ObjectId objectId, ClassId classId)
        {
            throw new NotImplementedException();
        }

        public HResult ObjectsAllocatedByClass(uint cClassCount, ClassId* classIds, uint* cObjects)
        {
            throw new NotImplementedException();
        }

        public HResult ObjectReferences(ObjectId objectId, ClassId classId, uint cObjectRefs, ObjectId* objectRefIds)
        {
            throw new NotImplementedException();
        }

        public HResult RootReferences(uint cRootRefs, ObjectId* rootRefIds)
        {
            throw new NotImplementedException();
        }

        public HResult ExceptionThrown(ObjectId thrownObjectId)
        {
            Console.WriteLine("Enumerating modules");

            _corProfilerInfo.EnumModules(out void* enumerator);

            ICorProfilerModuleEnum moduleEnumerator = NativeStubs.ICorProfilerModuleEnumStub.Wrap((IntPtr)enumerator);

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

            var metaDataImport = NativeStubs.IMetaDataImportStub.Wrap((IntPtr)ppOut);

            metaDataImport.GetTypeDefProps(typeDef, null, 0, out var nameCharCount, out _, out _);

            Span<char> buffer = stackalloc char[(int)nameCharCount];

            fixed (char* p = buffer)
            {
                metaDataImport.GetTypeDefProps(typeDef, p, nameCharCount, out _, out _, out _);
            }

            Console.WriteLine("[Profiler] An exception was thrown: " + new string(buffer));

            WalkStack();

            return HResult.S_OK;
        }

        private void WalkStack()
        {
            _corProfilerInfo.GetCurrentThreadId(out var threadId);

            var buffer = new StackSnapshotBuffer();

            var result = _corProfilerInfo.DoStackSnapshot(threadId, &StackSnapshotCallback, COR_PRF_SNAPSHOT_INFO.COR_PRF_SNAPSHOT_DEFAULT, Unsafe.AsPointer(ref buffer), null, 0);

            Console.WriteLine("WalkStack result: " + result);
            Console.WriteLine(buffer);
        }

        [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvStdcall) })]
        private static HResult StackSnapshotCallback(FunctionId functionId, nint ip, COR_PRF_FRAME_INFO frameInfo, uint contextSize, byte* context, void* clientData)
        {
            ref var buffer = ref Unsafe.AsRef<StackSnapshotBuffer>(clientData);

            return buffer.Add(ip) ? HResult.S_OK : HResult.E_FAIL;
        }

        public HResult ExceptionSearchFunctionEnter(FunctionId functionId)
        {
            Debug.WriteLine("ExceptionSearchFunctionEnter");
            return HResult.S_OK;
        }

        public HResult ExceptionSearchFunctionLeave()
        {
            Debug.WriteLine("ExceptionSearchFunctionLeave");
            return HResult.S_OK;
        }

        public HResult ExceptionSearchFilterEnter(FunctionId functionId)
        {
            Debug.WriteLine("ExceptionSearchFilterEnter");
            return HResult.S_OK;
        }

        public HResult ExceptionSearchFilterLeave()
        {
            Debug.WriteLine("ExceptionSearchFilterLeave");
            return HResult.S_OK;
        }

        public HResult ExceptionSearchCatcherFound(FunctionId functionId)
        {
            _corProfilerInfo.GetFunctionInfo(functionId, out var classId, out var moduleId, out var mdToken);

            _corProfilerInfo.GetModuleMetaData(moduleId, CorOpenFlags.ofRead, KnownGuids.IMetaDataImport, out var ppOut);

            IMetaDataImport metaDataImport = NativeStubs.IMetaDataImportStub.Wrap((IntPtr)ppOut);

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

        public HResult ExceptionOSHandlerEnter(nint* __unused)
        {
            Debug.WriteLine("ExceptionOSHandlerEnter");
            return HResult.S_OK;
        }

        public HResult ExceptionOSHandlerLeave(nint* __unused)
        {
            Debug.WriteLine("ExceptionOSHandlerLeave");
            return HResult.S_OK;
        }

        public HResult ExceptionUnwindFunctionEnter(FunctionId functionId)
        {
            Debug.WriteLine("ExceptionUnwindFunctionEnter");
            return HResult.S_OK;
        }

        public HResult ExceptionUnwindFunctionLeave()
        {
            Debug.WriteLine("ExceptionUnwindFunctionLeave");
            return HResult.S_OK;
        }

        public HResult ExceptionUnwindFinallyEnter(FunctionId functionId)
        {
            Debug.WriteLine("ExceptionUnwindFinallyEnter");
            return HResult.S_OK;
        }

        public HResult ExceptionUnwindFinallyLeave()
        {
            Debug.WriteLine("ExceptionUnwindFinallyLeave");
            return HResult.S_OK;
        }

        public HResult ExceptionCatcherEnter(FunctionId functionId, ObjectId objectId)
        {
            Debug.WriteLine("ExceptionCatcherEnter");
            return HResult.S_OK;
        }

        public HResult ExceptionCatcherLeave()
        {
            Debug.WriteLine("ExceptionCatcherLeave");
            return HResult.S_OK;
        }

        public HResult COMClassicVTableCreated(ClassId wrappedClassId, in Guid implementedIID, void* pVTable, uint cSlots)
        {
            throw new NotImplementedException();
        }

        public HResult COMClassicVTableDestroyed(ClassId wrappedClassId, in Guid implementedIID, void* pVTable)
        {
            throw new NotImplementedException();
        }

        public HResult ExceptionCLRCatcherFound()
        {
            throw new NotImplementedException();
        }

        public HResult ExceptionCLRCatcherExecute()
        {
            throw new NotImplementedException();
        }

        public HResult ThreadNameChanged(ThreadId threadId, uint cchName, char* name)
        {
            var threadName = new string(name, 0, (int)cchName);

            _corProfilerInfo.GetThreadInfo(threadId, out var osThreadId);
            _managedThreadList.SetThreadName(threadId, threadName);

            return HResult.S_OK;
        }

        public HResult GarbageCollectionStarted(int cGenerations, bool* generationCollected, COR_PRF_GC_REASON reason)
        {
            throw new NotImplementedException();
        }

        public HResult SurvivingReferences(uint cSurvivingObjectIDRanges, ObjectId* objectIDRangeStart, uint* cObjectIDRangeLength)
        {
            throw new NotImplementedException();
        }

        public HResult GarbageCollectionFinished()
        {
            throw new NotImplementedException();
        }

        public HResult FinalizeableObjectQueued(int finalizerFlags, ObjectId objectID)
        {
            throw new NotImplementedException();
        }

        public HResult RootReferences2(uint cRootRefs, ObjectId* rootRefIds, COR_PRF_GC_ROOT_KIND* rootKinds, COR_PRF_GC_ROOT_FLAGS* rootFlags, uint* rootIds)
        {
            throw new NotImplementedException();
        }

        public HResult HandleCreated(GCHandleID handleId, ObjectId initialObjectId)
        {
            throw new NotImplementedException();
        }

        public HResult HandleDestroyed(GCHandleID handleId)
        {
            throw new NotImplementedException();
        }
    }
}
