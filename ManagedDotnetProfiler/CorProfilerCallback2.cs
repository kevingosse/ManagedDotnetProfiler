using System;
using System.Diagnostics;
using System.Text;

namespace ManagedDotnetProfiler
{
    public unsafe class CorProfilerCallback2 : ICorProfilerCallback2
    {
        private static readonly Guid ICorProfilerCallback2Guid = Guid.Parse("8a8cc829-ccf2-49fe-bbae-0f022228071a");

        private readonly NativeObjects.ICorProfilerCallback2 _corProfilerCallback2;
        private ICorProfilerInfo3 _corProfilerInfo;

        public CorProfilerCallback2()
        {
            _corProfilerCallback2 = NativeObjects.ICorProfilerCallback2.Wrap(this);
        }

        public IntPtr ICorProfilerCallback2Object => _corProfilerCallback2;

        public int AddRef()
        {
            return 1;
        }

        public HResult Initialize(IntPtr pICorProfilerInfoUnk)
        {
            var impl = NativeObjects.IUnknown.Wrap(pICorProfilerInfoUnk);

            var result = impl.QueryInterface(KnownGuids.ICorProfilerInfo3, out IntPtr ptr);

            Console.WriteLine("[Profiler] Fetched ICorProfilerInfo3: " + result);

            _corProfilerInfo = NativeObjects.ICorProfilerInfo3.Wrap(ptr);

            var eventMask = CorPrfMonitor.COR_PRF_MONITOR_EXCEPTIONS | CorPrfMonitor.COR_PRF_MONITOR_JIT_COMPILATION;

            result = _corProfilerInfo.SetEventMask(eventMask);

            Console.WriteLine("[Profiler] Setting event mask to " + eventMask);

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
            throw new NotImplementedException();
        }

        public HResult ThreadDestroyed(ThreadId threadId)
        {
            throw new NotImplementedException();
        }

        public HResult ThreadAssignedToOSThread(ThreadId managedThreadId, int osThreadId)
        {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }

        public HResult RuntimeThreadResumed(ThreadId threadId)
        {
            throw new NotImplementedException();
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
    }
}
