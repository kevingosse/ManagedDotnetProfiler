namespace ProfilerLib
{
    public abstract class CorProfilerCallbackBase : Unknown, Interfaces.ICorProfilerCallback
    {
        private readonly NativeObjects.ICorProfilerCallback _corProfilerCallback;

        protected CorProfilerCallbackBase()
        {
            _corProfilerCallback = NativeObjects.ICorProfilerCallback.Wrap(this);
        }

        public IntPtr ICorProfilerCallback => _corProfilerCallback;
        public ICorProfilerInfo ICorProfilerInfo;
        public ICorProfilerInfo2 ICorProfilerInfo2;
        public ICorProfilerInfo3 ICorProfilerInfo3;
        public ICorProfilerInfo4 ICorProfilerInfo4;
        public ICorProfilerInfo5 ICorProfilerInfo5;
        public ICorProfilerInfo6 ICorProfilerInfo6;
        public ICorProfilerInfo7 ICorProfilerInfo7;
        public ICorProfilerInfo8 ICorProfilerInfo8;
        public ICorProfilerInfo9 ICorProfilerInfo9;
        public ICorProfilerInfo10 ICorProfilerInfo10;
        public ICorProfilerInfo11 ICorProfilerInfo11;

        protected override HResult QueryInterface(in Guid guid, out nint ptr)
        {
            if (guid == Interfaces.ICorProfilerCallback.Guid)
            {
                ptr = _corProfilerCallback;
                return HResult.S_OK;
            }

            ptr = default;
            return HResult.E_NOINTERFACE;
        }

        protected abstract HResult Initialize(int iCorProfilerInfoVersion);

        private int GetICorProfilerInfo(nint pICorProfilerInfoUnk)
        {
            int supportedInterface = 0;

            var impl = new NativeObjects.IUnknownInvoker(pICorProfilerInfoUnk);

            HResult result;
            IntPtr ptr;

            result = impl.QueryInterface(Interfaces.ICorProfilerInfo.Guid, out ptr);

            if (!result.IsOK)
            {
                return supportedInterface;
            }

            supportedInterface++;

            ICorProfilerInfo = new ICorProfilerInfo(ptr);

            result = impl.QueryInterface(Interfaces.ICorProfilerInfo2.Guid, out ptr);

            if (!result.IsOK)
            {
                return supportedInterface;
            }

            supportedInterface++;

            ICorProfilerInfo2 = new ICorProfilerInfo2(ptr);

            result = impl.QueryInterface(Interfaces.ICorProfilerInfo3.Guid, out ptr);

            if (!result.IsOK)
            {
                return supportedInterface;
            }

            supportedInterface++;

            ICorProfilerInfo3 = new ICorProfilerInfo3(ptr);

            result = impl.QueryInterface(Interfaces.ICorProfilerInfo4.Guid, out ptr);

            if (!result.IsOK)
            {
                return supportedInterface;
            }

            supportedInterface++;

            ICorProfilerInfo4 = new ICorProfilerInfo4(ptr);

            result = impl.QueryInterface(Interfaces.ICorProfilerInfo5.Guid, out ptr);

            if (!result.IsOK)
            {
                return supportedInterface;
            }

            supportedInterface++;

            ICorProfilerInfo5 = new ICorProfilerInfo5(ptr);

            result = impl.QueryInterface(Interfaces.ICorProfilerInfo6.Guid, out ptr);

            if (!result.IsOK)
            {
                return supportedInterface;
            }

            supportedInterface++;

            ICorProfilerInfo6 = new ICorProfilerInfo6(ptr);

            result = impl.QueryInterface(Interfaces.ICorProfilerInfo7.Guid, out ptr);

            if (!result.IsOK)
            {
                return supportedInterface;
            }

            supportedInterface++;

            ICorProfilerInfo7 = new ICorProfilerInfo7(ptr);

            result = impl.QueryInterface(Interfaces.ICorProfilerInfo8.Guid, out ptr);

            if (!result.IsOK)
            {
                return supportedInterface;
            }

            supportedInterface++;

            ICorProfilerInfo8 = new ICorProfilerInfo8(ptr);

            result = impl.QueryInterface(Interfaces.ICorProfilerInfo9.Guid, out ptr);

            if (!result.IsOK)
            {
                return supportedInterface;
            }

            supportedInterface++;

            ICorProfilerInfo9 = new ICorProfilerInfo9(ptr);

            result = impl.QueryInterface(Interfaces.ICorProfilerInfo10.Guid, out ptr);

            if (!result.IsOK)
            {
                return supportedInterface;
            }

            supportedInterface++;

            ICorProfilerInfo10 = new ICorProfilerInfo10(ptr);

            result = impl.QueryInterface(Interfaces.ICorProfilerInfo11.Guid, out ptr);

            if (!result.IsOK)
            {
                return supportedInterface;
            }

            supportedInterface++;

            ICorProfilerInfo11 = new ICorProfilerInfo11(ptr);

            return supportedInterface;
        }

        #region ICorProfilerCallback

        HResult Interfaces.ICorProfilerCallback.Initialize(nint pICorProfilerInfoUnk)
        {
            int version = GetICorProfilerInfo(pICorProfilerInfoUnk);

            return Initialize(version);
        }

        HResult Interfaces.ICorProfilerCallback.Shutdown()
        {
            return Shutdown();
        }

        HResult Interfaces.ICorProfilerCallback.AppDomainCreationStarted(AppDomainId appDomainId)
        {
            return AppDomainCreationStarted(appDomainId);
        }

        HResult Interfaces.ICorProfilerCallback.AppDomainCreationFinished(AppDomainId appDomainId, HResult hrStatus)
        {
            return AppDomainCreationFinished(appDomainId, hrStatus);
        }

        HResult Interfaces.ICorProfilerCallback.AppDomainShutdownStarted(AppDomainId appDomainId)
        {
            return AppDomainShutdownStarted(appDomainId);
        }

        HResult Interfaces.ICorProfilerCallback.AppDomainShutdownFinished(AppDomainId appDomainId, HResult hrStatus)
        {
            return AppDomainShutdownFinished(appDomainId, hrStatus);
        }

        HResult Interfaces.ICorProfilerCallback.AssemblyLoadStarted(AssemblyId assemblyId)
        {
            return AssemblyLoadStarted(assemblyId);
        }

        HResult Interfaces.ICorProfilerCallback.AssemblyLoadFinished(AssemblyId assemblyId, HResult hrStatus)
        {
            return AssemblyLoadFinished(assemblyId, hrStatus);
        }

        HResult Interfaces.ICorProfilerCallback.AssemblyUnloadStarted(AssemblyId assemblyId)
        {
            return AssemblyUnloadStarted(assemblyId);
        }

        HResult Interfaces.ICorProfilerCallback.AssemblyUnloadFinished(AssemblyId assemblyId, HResult hrStatus)
        {
            return AssemblyUnloadFinished(assemblyId, hrStatus);
        }

        HResult Interfaces.ICorProfilerCallback.ModuleLoadStarted(ModuleId moduleId)
        {
            return ModuleLoadStarted(moduleId);
        }

        HResult Interfaces.ICorProfilerCallback.ModuleLoadFinished(ModuleId moduleId, HResult hrStatus)
        {
            return ModuleLoadFinished(moduleId, hrStatus);
        }

        HResult Interfaces.ICorProfilerCallback.ModuleUnloadStarted(ModuleId moduleId)
        {
            return ModuleUnloadStarted(moduleId);
        }

        HResult Interfaces.ICorProfilerCallback.ModuleUnloadFinished(ModuleId moduleId, HResult hrStatus)
        {
            return ModuleUnloadFinished(moduleId, hrStatus);
        }

        HResult Interfaces.ICorProfilerCallback.ModuleAttachedToAssembly(ModuleId moduleId, AssemblyId assemblyId)
        {
            return ModuleAttachedToAssembly(moduleId, assemblyId);
        }

        HResult Interfaces.ICorProfilerCallback.ClassLoadStarted(ClassId classId)
        {
            return ClassLoadStarted(classId);
        }

        HResult Interfaces.ICorProfilerCallback.ClassLoadFinished(ClassId classId, HResult hrStatus)
        {
            return ClassLoadFinished(classId, hrStatus);
        }

        HResult Interfaces.ICorProfilerCallback.ClassUnloadStarted(ClassId classId)
        {
            return ClassUnloadStarted(classId);
        }

        HResult Interfaces.ICorProfilerCallback.ClassUnloadFinished(ClassId classId, HResult hrStatus)
        {
            return ClassUnloadFinished(classId, hrStatus);
        }

        HResult Interfaces.ICorProfilerCallback.FunctionUnloadStarted(FunctionId functionId)
        {
            return FunctionUnloadStarted(functionId);
        }

        HResult Interfaces.ICorProfilerCallback.JITCompilationStarted(FunctionId functionId, int fIsSafeToBlock)
        {
            return JITCompilationStarted(functionId, fIsSafeToBlock != 0);
        }

        HResult Interfaces.ICorProfilerCallback.JITCompilationFinished(FunctionId functionId, HResult hrStatus, int fIsSafeToBlock)
        {
            return JITCompilationFinished(functionId, hrStatus, fIsSafeToBlock != 0);
        }

        HResult Interfaces.ICorProfilerCallback.JITCachedFunctionSearchStarted(FunctionId functionId, out int pbUseCachedFunction)
        {
            var result = JITCachedFunctionSearchStarted(functionId, out var useCachedFunction);

            pbUseCachedFunction = useCachedFunction ? 1 : 0;

            return result;
        }

        HResult Interfaces.ICorProfilerCallback.JITCachedFunctionSearchFinished(FunctionId functionId, COR_PRF_JIT_CACHE result)
        {
            return JITCachedFunctionSearchFinished(functionId, result);
        }

        HResult Interfaces.ICorProfilerCallback.JITFunctionPitched(FunctionId functionId)
        {
            return JITFunctionPitched(functionId);
        }

        HResult Interfaces.ICorProfilerCallback.JITInlining(FunctionId callerId, FunctionId calleeId, out int pfShouldInline)
        {
            var result = JITInlining(callerId, calleeId, out var shouldInline);

            pfShouldInline = shouldInline ? 1 : 0;

            return result;
        }

        HResult Interfaces.ICorProfilerCallback.ThreadCreated(ThreadId threadId)
        {
            return ThreadCreated(threadId);
        }

        HResult Interfaces.ICorProfilerCallback.ThreadDestroyed(ThreadId threadId)
        {
            return ThreadDestroyed(threadId);
        }

        HResult Interfaces.ICorProfilerCallback.ThreadAssignedToOSThread(ThreadId managedThreadId, int osThreadId)
        {
            return ThreadAssignedToOSThread(managedThreadId, osThreadId);
        }

        HResult Interfaces.ICorProfilerCallback.RemotingClientInvocationStarted()
        {
            return RemotingClientInvocationStarted();
        }

        HResult Interfaces.ICorProfilerCallback.RemotingClientSendingMessage(in Guid pCookie, int fIsAsync)
        {
            return RemotingClientSendingMessage(in pCookie, fIsAsync != 0);
        }

        HResult Interfaces.ICorProfilerCallback.RemotingClientReceivingReply(in Guid pCookie, int fIsAsync)
        {
            return RemotingClientReceivingReply(in pCookie, fIsAsync != 0);
        }

        HResult Interfaces.ICorProfilerCallback.RemotingClientInvocationFinished()
        {
            return RemotingClientInvocationFinished();
        }

        HResult Interfaces.ICorProfilerCallback.RemotingServerReceivingMessage(in Guid pCookie, int fIsAsync)
        {
            return RemotingServerReceivingMessage(in pCookie, fIsAsync != 0);
        }

        HResult Interfaces.ICorProfilerCallback.RemotingServerInvocationStarted()
        {
            return RemotingServerInvocationStarted();
        }

        HResult Interfaces.ICorProfilerCallback.RemotingServerInvocationReturned()
        {
            return RemotingServerInvocationReturned();
        }

        HResult Interfaces.ICorProfilerCallback.RemotingServerSendingReply(in Guid pCookie, int fIsAsync)
        {
            return RemotingServerSendingReply(in pCookie, fIsAsync != 0);
        }

        HResult Interfaces.ICorProfilerCallback.UnmanagedToManagedTransition(FunctionId functionId, COR_PRF_TRANSITION_REASON reason)
        {
            return UnmanagedToManagedTransition(functionId, reason);
        }

        HResult Interfaces.ICorProfilerCallback.ManagedToUnmanagedTransition(FunctionId functionId, COR_PRF_TRANSITION_REASON reason)
        {
            return ManagedToUnmanagedTransition(functionId, reason);
        }

        HResult Interfaces.ICorProfilerCallback.RuntimeSuspendStarted(COR_PRF_SUSPEND_REASON suspendReason)
        {
            return RuntimeSuspendStarted(suspendReason);
        }

        HResult Interfaces.ICorProfilerCallback.RuntimeSuspendFinished()
        {
            return RuntimeSuspendFinished();
        }

        HResult Interfaces.ICorProfilerCallback.RuntimeSuspendAborted()
        {
            return RuntimeSuspendAborted();
        }

        HResult Interfaces.ICorProfilerCallback.RuntimeResumeStarted()
        {
            return RuntimeResumeStarted();
        }

        HResult Interfaces.ICorProfilerCallback.RuntimeResumeFinished()
        {
            return RuntimeResumeFinished();
        }

        HResult Interfaces.ICorProfilerCallback.RuntimeThreadSuspended(ThreadId threadId)
        {
            return RuntimeThreadSuspended(threadId);
        }

        HResult Interfaces.ICorProfilerCallback.RuntimeThreadResumed(ThreadId threadId)
        {
            return RuntimeThreadResumed(threadId);
        }

        unsafe HResult Interfaces.ICorProfilerCallback.MovedReferences(uint cMovedObjectIDRanges, ObjectId* oldObjectIDRangeStart, ObjectId* newObjectIDRangeStart, uint* cObjectIDRangeLength)
        {
            return MovedReferences(cMovedObjectIDRanges, oldObjectIDRangeStart, newObjectIDRangeStart, cObjectIDRangeLength);
        }

        HResult Interfaces.ICorProfilerCallback.ObjectAllocated(ObjectId objectId, ClassId classId)
        {
            return ObjectAllocated(objectId, classId);
        }

        unsafe HResult Interfaces.ICorProfilerCallback.ObjectsAllocatedByClass(uint cClassCount, ClassId* classIds, uint* cObjects)
        {
            return ObjectsAllocatedByClass(cClassCount, classIds, cObjects);
        }

        unsafe HResult Interfaces.ICorProfilerCallback.ObjectReferences(ObjectId objectId, ClassId classId, uint cObjectRefs, ObjectId* objectRefIds)
        {
            return ObjectReferences(objectId, classId, cObjectRefs, objectRefIds);
        }

        unsafe HResult Interfaces.ICorProfilerCallback.RootReferences(uint cRootRefs, ObjectId* rootRefIds)
        {
            return RootReferences(cRootRefs, rootRefIds);
        }

        HResult Interfaces.ICorProfilerCallback.ExceptionThrown(ObjectId thrownObjectId)
        {
            return ExceptionThrown(thrownObjectId);
        }

        HResult Interfaces.ICorProfilerCallback.ExceptionSearchFunctionEnter(FunctionId functionId)
        {
            return ExceptionSearchFunctionEnter(functionId);
        }

        HResult Interfaces.ICorProfilerCallback.ExceptionSearchFunctionLeave()
        {
            return ExceptionSearchFunctionLeave();
        }

        HResult Interfaces.ICorProfilerCallback.ExceptionSearchFilterEnter(FunctionId functionId)
        {
            return ExceptionSearchFilterEnter(functionId);
        }

        HResult Interfaces.ICorProfilerCallback.ExceptionSearchFilterLeave()
        {
            return ExceptionSearchFilterLeave();
        }

        HResult Interfaces.ICorProfilerCallback.ExceptionSearchCatcherFound(FunctionId functionId)
        {
            return ExceptionSearchCatcherFound(functionId);
        }

        unsafe HResult Interfaces.ICorProfilerCallback.ExceptionOSHandlerEnter(nint* __unused)
        {
            return ExceptionOSHandlerEnter(__unused);
        }

        unsafe HResult Interfaces.ICorProfilerCallback.ExceptionOSHandlerLeave(nint* __unused)
        {
            return ExceptionOSHandlerLeave(__unused);
        }

        HResult Interfaces.ICorProfilerCallback.ExceptionUnwindFunctionEnter(FunctionId functionId)
        {
            return ExceptionUnwindFunctionEnter(functionId);
        }

        HResult Interfaces.ICorProfilerCallback.ExceptionUnwindFunctionLeave()
        {
            return ExceptionUnwindFunctionLeave();
        }

        HResult Interfaces.ICorProfilerCallback.ExceptionUnwindFinallyEnter(FunctionId functionId)
        {
            return ExceptionUnwindFinallyEnter(functionId);
        }

        HResult Interfaces.ICorProfilerCallback.ExceptionUnwindFinallyLeave()
        {
            return ExceptionUnwindFinallyLeave();
        }

        HResult Interfaces.ICorProfilerCallback.ExceptionCatcherEnter(FunctionId functionId, ObjectId objectId)
        {
            return ExceptionCatcherEnter(functionId, objectId);
        }

        HResult Interfaces.ICorProfilerCallback.ExceptionCatcherLeave()
        {
            return ExceptionCatcherLeave();
        }

        unsafe HResult Interfaces.ICorProfilerCallback.COMClassicVTableCreated(ClassId wrappedClassId, in Guid implementedIID, void* pVTable, uint cSlots)
        {
            return COMClassicVTableCreated(wrappedClassId, in implementedIID, pVTable, cSlots);
        }

        unsafe HResult Interfaces.ICorProfilerCallback.COMClassicVTableDestroyed(ClassId wrappedClassId, in Guid implementedIID, void* pVTable)
        {
            return COMClassicVTableDestroyed(wrappedClassId, in implementedIID, pVTable);
        }

        HResult Interfaces.ICorProfilerCallback.ExceptionCLRCatcherFound()
        {
            return ExceptionCLRCatcherFound();
        }

        HResult Interfaces.ICorProfilerCallback.ExceptionCLRCatcherExecute()
        {
            return ExceptionCLRCatcherExecute();
        }

        #endregion

        protected virtual HResult Shutdown()
        {
            return HResult.E_NOTIMPL;
        }

        protected virtual HResult AppDomainCreationStarted(AppDomainId appDomainId)
        {
            return HResult.E_NOTIMPL;
        }

        protected virtual HResult AppDomainCreationFinished(AppDomainId appDomainId, HResult hrStatus)
        {
            return HResult.E_NOTIMPL;
        }

        protected virtual HResult AppDomainShutdownStarted(AppDomainId appDomainId)
        {
            return HResult.E_NOTIMPL;
        }

        protected virtual HResult AppDomainShutdownFinished(AppDomainId appDomainId, HResult hrStatus)
        {
            return HResult.E_NOTIMPL;
        }

        protected virtual HResult AssemblyLoadStarted(AssemblyId assemblyId)
        {
            return HResult.E_NOTIMPL;
        }

        protected virtual HResult AssemblyLoadFinished(AssemblyId assemblyId, HResult hrStatus)
        {
            return HResult.E_NOTIMPL;
        }

        protected virtual HResult AssemblyUnloadStarted(AssemblyId assemblyId)
        {
            return HResult.E_NOTIMPL;
        }

        protected virtual HResult AssemblyUnloadFinished(AssemblyId assemblyId, HResult hrStatus)
        {
            return HResult.E_NOTIMPL;
        }

        protected virtual HResult ModuleLoadStarted(ModuleId moduleId)
        {
            return HResult.E_NOTIMPL;
        }

        protected virtual HResult ModuleLoadFinished(ModuleId moduleId, HResult hrStatus)
        {
            return HResult.E_NOTIMPL;
        }

        protected virtual HResult ModuleUnloadStarted(ModuleId moduleId)
        {
            return HResult.E_NOTIMPL;
        }

        protected virtual HResult ModuleUnloadFinished(ModuleId moduleId, HResult hrStatus)
        {
            return HResult.E_NOTIMPL;
        }

        protected virtual HResult ModuleAttachedToAssembly(ModuleId moduleId, AssemblyId assemblyId)
        {
            return HResult.E_NOTIMPL;
        }

        protected virtual HResult ClassLoadStarted(ClassId classId)
        {
            return HResult.E_NOTIMPL;
        }

        protected virtual HResult ClassLoadFinished(ClassId classId, HResult hrStatus)
        {
            return HResult.E_NOTIMPL;
        }

        protected virtual HResult ClassUnloadStarted(ClassId classId)
        {
            return HResult.E_NOTIMPL;
        }

        protected virtual HResult ClassUnloadFinished(ClassId classId, HResult hrStatus)
        {
            return HResult.E_NOTIMPL;
        }

        protected virtual HResult FunctionUnloadStarted(FunctionId functionId)
        {
            return HResult.E_NOTIMPL;
        }

        protected virtual HResult JITCompilationStarted(FunctionId functionId, bool fIsSafeToBlock)
        {
            return HResult.E_NOTIMPL;
        }

        protected virtual HResult JITCompilationFinished(FunctionId functionId, HResult hrStatus, bool fIsSafeToBlock)
        {
            return HResult.E_NOTIMPL;
        }

        protected virtual HResult JITCachedFunctionSearchStarted(FunctionId functionId, out bool pbUseCachedFunction)
        {
            pbUseCachedFunction = false;

            return HResult.E_NOTIMPL;
        }

        protected virtual HResult JITCachedFunctionSearchFinished(FunctionId functionId, COR_PRF_JIT_CACHE result)
        {
            return HResult.E_NOTIMPL;
        }

        protected virtual HResult JITFunctionPitched(FunctionId functionId)
        {
            return HResult.E_NOTIMPL;
        }

        protected virtual HResult JITInlining(FunctionId callerId, FunctionId calleeId, out bool pfShouldInline)
        {
            pfShouldInline = false;

            return HResult.E_NOTIMPL;
        }

        protected virtual HResult ThreadCreated(ThreadId threadId)
        {
            return HResult.E_NOTIMPL;
        }

        protected virtual HResult ThreadDestroyed(ThreadId threadId)
        {
            return HResult.E_NOTIMPL;
        }

        protected virtual HResult ThreadAssignedToOSThread(ThreadId managedThreadId, int osThreadId)
        {
            return HResult.E_NOTIMPL;
        }

        protected virtual HResult RemotingClientInvocationStarted()
        {
            return HResult.E_NOTIMPL;
        }

        protected virtual HResult RemotingClientSendingMessage(in Guid pCookie, bool fIsAsync)
        {
            return HResult.E_NOTIMPL;
        }

        protected virtual HResult RemotingClientReceivingReply(in Guid pCookie, bool fIsAsync)
        {
            return HResult.E_NOTIMPL;
        }

        protected virtual HResult RemotingClientInvocationFinished()
        {
            return HResult.E_NOTIMPL;
        }

        protected virtual HResult RemotingServerReceivingMessage(in Guid pCookie, bool fIsAsync)
        {
            return HResult.E_NOTIMPL;
        }

        protected virtual HResult RemotingServerInvocationStarted()
        {
            return HResult.E_NOTIMPL;
        }

        protected virtual HResult RemotingServerInvocationReturned()
        {
            return HResult.E_NOTIMPL;
        }

        protected virtual HResult RemotingServerSendingReply(in Guid pCookie, bool fIsAsync)
        {
            return HResult.E_NOTIMPL;
        }

        protected virtual HResult UnmanagedToManagedTransition(FunctionId functionId, COR_PRF_TRANSITION_REASON reason)
        {
            return HResult.E_NOTIMPL;
        }

        protected virtual HResult ManagedToUnmanagedTransition(FunctionId functionId, COR_PRF_TRANSITION_REASON reason)
        {
            return HResult.E_NOTIMPL;
        }

        protected virtual HResult RuntimeSuspendStarted(COR_PRF_SUSPEND_REASON suspendReason)
        {
            return HResult.E_NOTIMPL;
        }

        protected virtual HResult RuntimeSuspendFinished()
        {
            return HResult.E_NOTIMPL;
        }

        protected virtual HResult RuntimeSuspendAborted()
        {
            return HResult.E_NOTIMPL;
        }

        protected virtual HResult RuntimeResumeStarted()
        {
            return HResult.E_NOTIMPL;
        }

        protected virtual HResult RuntimeResumeFinished()
        {
            return HResult.E_NOTIMPL;
        }

        protected virtual HResult RuntimeThreadSuspended(ThreadId threadId)
        {
            return HResult.E_NOTIMPL;
        }

        protected virtual HResult RuntimeThreadResumed(ThreadId threadId)
        {
            return HResult.E_NOTIMPL;
        }

        protected virtual unsafe HResult MovedReferences(uint cMovedObjectIDRanges, ObjectId* oldObjectIDRangeStart, ObjectId* newObjectIDRangeStart, uint* cObjectIDRangeLength)
        {
            return HResult.E_NOTIMPL;
        }

        protected virtual HResult ObjectAllocated(ObjectId objectId, ClassId classId)
        {
            return HResult.E_NOTIMPL;
        }

        protected virtual unsafe HResult ObjectsAllocatedByClass(uint cClassCount, ClassId* classIds, uint* cObjects)
        {
            return HResult.E_NOTIMPL;
        }

        protected virtual unsafe HResult ObjectReferences(ObjectId objectId, ClassId classId, uint cObjectRefs, ObjectId* objectRefIds)
        {
            return HResult.E_NOTIMPL;
        }

        protected virtual unsafe HResult RootReferences(uint cRootRefs, ObjectId* rootRefIds)
        {
            return HResult.E_NOTIMPL;
        }

        protected virtual HResult ExceptionThrown(ObjectId thrownObjectId)
        {
            return HResult.E_NOTIMPL;
        }

        protected virtual HResult ExceptionSearchFunctionEnter(FunctionId functionId)
        {
            return HResult.E_NOTIMPL;
        }

        protected virtual HResult ExceptionSearchFunctionLeave()
        {
            return HResult.E_NOTIMPL;
        }

        protected virtual HResult ExceptionSearchFilterEnter(FunctionId functionId)
        {
            return HResult.E_NOTIMPL;
        }

        protected virtual HResult ExceptionSearchFilterLeave()
        {
            return HResult.E_NOTIMPL;
        }

        protected virtual HResult ExceptionSearchCatcherFound(FunctionId functionId)
        {
            return HResult.E_NOTIMPL;
        }

        protected virtual unsafe HResult ExceptionOSHandlerEnter(nint* __unused)
        {
            return HResult.E_NOTIMPL;
        }

        protected virtual unsafe HResult ExceptionOSHandlerLeave(nint* __unused)
        {
            return HResult.E_NOTIMPL;
        }

        protected virtual HResult ExceptionUnwindFunctionEnter(FunctionId functionId)
        {
            return HResult.E_NOTIMPL;
        }

        protected virtual HResult ExceptionUnwindFunctionLeave()
        {
            return HResult.E_NOTIMPL;
        }

        protected virtual HResult ExceptionUnwindFinallyEnter(FunctionId functionId)
        {
            return HResult.E_NOTIMPL;
        }

        protected virtual HResult ExceptionUnwindFinallyLeave()
        {
            return HResult.E_NOTIMPL;
        }

        protected virtual HResult ExceptionCatcherEnter(FunctionId functionId, ObjectId objectId)
        {
            return HResult.E_NOTIMPL;
        }

        protected virtual HResult ExceptionCatcherLeave()
        {
            return HResult.E_NOTIMPL;
        }

        protected virtual unsafe HResult COMClassicVTableCreated(ClassId wrappedClassId, in Guid implementedIID, void* pVTable, uint cSlots)
        {
            return HResult.E_NOTIMPL;
        }

        protected virtual unsafe HResult COMClassicVTableDestroyed(ClassId wrappedClassId, in Guid implementedIID, void* pVTable)
        {
            return HResult.E_NOTIMPL;
        }

        protected virtual HResult ExceptionCLRCatcherFound()
        {
            return HResult.E_NOTIMPL;
        }

        protected virtual HResult ExceptionCLRCatcherExecute()
        {
            return HResult.E_NOTIMPL;
        }
    }
}
