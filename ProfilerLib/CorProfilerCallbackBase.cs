using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProfilerLib
{
    public abstract class CorProfilerCallbackBase : Unknown, ICorProfilerCallback
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
            if (guid == ProfilerLib.ICorProfilerCallback.Guid)
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

            var impl = NativeObjects.IUnknown.Wrap(pICorProfilerInfoUnk);

            HResult result;
            IntPtr ptr;

            result = impl.QueryInterface(ProfilerLib.ICorProfilerInfo.Guid, out ptr);

            if (!result.IsOK)
            {
                return supportedInterface;
            }

            supportedInterface++;

            ICorProfilerInfo = NativeObjects.ICorProfilerInfo.Wrap(ptr);

            result = impl.QueryInterface(ProfilerLib.ICorProfilerInfo2.Guid, out ptr);

            if (!result.IsOK)
            {
                return supportedInterface;
            }

            supportedInterface++;

            ICorProfilerInfo2 = NativeObjects.ICorProfilerInfo2.Wrap(ptr);

            result = impl.QueryInterface(ProfilerLib.ICorProfilerInfo3.Guid, out ptr);

            if (!result.IsOK)
            {
                return supportedInterface;
            }

            supportedInterface++;

            ICorProfilerInfo3 = NativeObjects.ICorProfilerInfo3.Wrap(ptr);

            result = impl.QueryInterface(ProfilerLib.ICorProfilerInfo4.Guid, out ptr);

            if (!result.IsOK)
            {
                return supportedInterface;
            }

            supportedInterface++;

            ICorProfilerInfo4 = NativeObjects.ICorProfilerInfo4.Wrap(ptr);

            result = impl.QueryInterface(ProfilerLib.ICorProfilerInfo5.Guid, out ptr);

            if (!result.IsOK)
            {
                return supportedInterface;
            }

            supportedInterface++;

            ICorProfilerInfo5 = NativeObjects.ICorProfilerInfo5.Wrap(ptr);

            result = impl.QueryInterface(ProfilerLib.ICorProfilerInfo6.Guid, out ptr);

            if (!result.IsOK)
            {
                return supportedInterface;
            }

            supportedInterface++;

            ICorProfilerInfo6 = NativeObjects.ICorProfilerInfo6.Wrap(ptr);

            result = impl.QueryInterface(ProfilerLib.ICorProfilerInfo7.Guid, out ptr);

            if (!result.IsOK)
            {
                return supportedInterface;
            }

            supportedInterface++;

            ICorProfilerInfo7 = NativeObjects.ICorProfilerInfo7.Wrap(ptr);

            result = impl.QueryInterface(ProfilerLib.ICorProfilerInfo8.Guid, out ptr);

            if (!result.IsOK)
            {
                return supportedInterface;
            }

            supportedInterface++;

            ICorProfilerInfo8 = NativeObjects.ICorProfilerInfo8.Wrap(ptr);

            result = impl.QueryInterface(ProfilerLib.ICorProfilerInfo9.Guid, out ptr);

            if (!result.IsOK)
            {
                return supportedInterface;
            }

            supportedInterface++;

            ICorProfilerInfo9 = NativeObjects.ICorProfilerInfo9.Wrap(ptr);

            result = impl.QueryInterface(ProfilerLib.ICorProfilerInfo10.Guid, out ptr);

            if (!result.IsOK)
            {
                return supportedInterface;
            }

            supportedInterface++;

            ICorProfilerInfo10 = NativeObjects.ICorProfilerInfo10.Wrap(ptr);

            result = impl.QueryInterface(ProfilerLib.ICorProfilerInfo11.Guid, out ptr);

            if (!result.IsOK)
            {
                return supportedInterface;
            }

            supportedInterface++;

            ICorProfilerInfo11 = NativeObjects.ICorProfilerInfo11.Wrap(ptr);

            return supportedInterface;
        }

        #region ICorProfilerCallback

        HResult ICorProfilerCallback.Initialize(nint pICorProfilerInfoUnk)
        {
            Console.WriteLine($"CorProfilerCallbackBase - Initialize");

            int version = GetICorProfilerInfo(pICorProfilerInfoUnk);

            Console.WriteLine($"[Profiler] Fetched ICorProfilerInfo{version}");

            return Initialize(version);
        }

        HResult ICorProfilerCallback.Shutdown()
        {
            return Shutdown();
        }

        HResult ICorProfilerCallback.AppDomainCreationStarted(AppDomainId appDomainId)
        {
            return AppDomainCreationStarted(appDomainId);
        }

        HResult ICorProfilerCallback.AppDomainCreationFinished(AppDomainId appDomainId, HResult hrStatus)
        {
            return AppDomainCreationFinished(appDomainId, hrStatus);
        }

        HResult ICorProfilerCallback.AppDomainShutdownStarted(AppDomainId appDomainId)
        {
            return AppDomainShutdownStarted(appDomainId);
        }

        HResult ICorProfilerCallback.AppDomainShutdownFinished(AppDomainId appDomainId, HResult hrStatus)
        {
            return AppDomainShutdownFinished(appDomainId, hrStatus);
        }

        HResult ICorProfilerCallback.AssemblyLoadStarted(AssemblyId assemblyId)
        {
            return AssemblyLoadStarted(assemblyId);
        }

        HResult ICorProfilerCallback.AssemblyLoadFinished(AssemblyId assemblyId, HResult hrStatus)
        {
            return AssemblyLoadFinished(assemblyId, hrStatus);
        }

        HResult ICorProfilerCallback.AssemblyUnloadStarted(AssemblyId assemblyId)
        {
            return AssemblyUnloadStarted(assemblyId);
        }

        HResult ICorProfilerCallback.AssemblyUnloadFinished(AssemblyId assemblyId, HResult hrStatus)
        {
            return AssemblyUnloadFinished(assemblyId, hrStatus);
        }

        HResult ICorProfilerCallback.ModuleLoadStarted(ModuleId moduleId)
        {
            return ModuleLoadStarted(moduleId);
        }

        HResult ICorProfilerCallback.ModuleLoadFinished(ModuleId moduleId, HResult hrStatus)
        {
            return ModuleLoadFinished(moduleId, hrStatus);
        }

        HResult ICorProfilerCallback.ModuleUnloadStarted(ModuleId moduleId)
        {
            return ModuleUnloadStarted(moduleId);
        }

        HResult ICorProfilerCallback.ModuleUnloadFinished(ModuleId moduleId, HResult hrStatus)
        {
            return ModuleUnloadFinished(moduleId, hrStatus);
        }

        HResult ICorProfilerCallback.ModuleAttachedToAssembly(ModuleId moduleId, AssemblyId assemblyId)
        {
            return ModuleAttachedToAssembly(moduleId, assemblyId);
        }

        HResult ICorProfilerCallback.ClassLoadStarted(ClassId classId)
        {
            return ClassLoadStarted(classId);
        }

        HResult ICorProfilerCallback.ClassLoadFinished(ClassId classId, HResult hrStatus)
        {
            return ClassLoadFinished(classId, hrStatus);
        }

        HResult ICorProfilerCallback.ClassUnloadStarted(ClassId classId)
        {
            return ClassUnloadStarted(classId);
        }

        HResult ICorProfilerCallback.ClassUnloadFinished(ClassId classId, HResult hrStatus)
        {
            return ClassUnloadFinished(classId, hrStatus);
        }

        HResult ICorProfilerCallback.FunctionUnloadStarted(FunctionId functionId)
        {
            return FunctionUnloadStarted(functionId);
        }

        HResult ICorProfilerCallback.JITCompilationStarted(FunctionId functionId, bool fIsSafeToBlock)
        {
            return JITCompilationStarted(functionId, fIsSafeToBlock);
        }

        HResult ICorProfilerCallback.JITCompilationFinished(FunctionId functionId, HResult hrStatus, bool fIsSafeToBlock)
        {
            return JITCompilationFinished(functionId, hrStatus, fIsSafeToBlock);
        }

        HResult ICorProfilerCallback.JITCachedFunctionSearchStarted(FunctionId functionId, out bool pbUseCachedFunction)
        {
            return JITCachedFunctionSearchStarted(functionId, out pbUseCachedFunction);
        }

        HResult ICorProfilerCallback.JITCachedFunctionSearchFinished(FunctionId functionId, COR_PRF_JIT_CACHE result)
        {
            return JITCachedFunctionSearchFinished(functionId, result);
        }

        HResult ICorProfilerCallback.JITFunctionPitched(FunctionId functionId)
        {
            return JITFunctionPitched(functionId);
        }

        HResult ICorProfilerCallback.JITInlining(FunctionId callerId, FunctionId calleeId, out bool pfShouldInline)
        {
            return JITInlining(callerId, calleeId, out pfShouldInline);
        }

        HResult ICorProfilerCallback.ThreadCreated(ThreadId threadId)
        {
            return ThreadCreated(threadId);
        }

        HResult ICorProfilerCallback.ThreadDestroyed(ThreadId threadId)
        {
            return ThreadDestroyed(threadId);
        }

        HResult ICorProfilerCallback.ThreadAssignedToOSThread(ThreadId managedThreadId, int osThreadId)
        {
            return ThreadAssignedToOSThread(managedThreadId, osThreadId);
        }

        HResult ICorProfilerCallback.RemotingClientInvocationStarted()
        {
            return RemotingClientInvocationStarted();
        }

        HResult ICorProfilerCallback.RemotingClientSendingMessage(in Guid pCookie, bool fIsAsync)
        {
            return RemotingClientSendingMessage(in pCookie, fIsAsync);
        }

        HResult ICorProfilerCallback.RemotingClientReceivingReply(in Guid pCookie, bool fIsAsync)
        {
            return RemotingClientReceivingReply(in pCookie, fIsAsync);
        }

        HResult ICorProfilerCallback.RemotingClientInvocationFinished()
        {
            return RemotingClientInvocationFinished();
        }

        HResult ICorProfilerCallback.RemotingServerReceivingMessage(in Guid pCookie, bool fIsAsync)
        {
            return RemotingServerReceivingMessage(in pCookie, fIsAsync);
        }

        HResult ICorProfilerCallback.RemotingServerInvocationStarted()
        {
            return RemotingServerInvocationStarted();
        }

        HResult ICorProfilerCallback.RemotingServerInvocationReturned()
        {
            return RemotingServerInvocationReturned();
        }

        HResult ICorProfilerCallback.RemotingServerSendingReply(in Guid pCookie, bool fIsAsync)
        {
            return RemotingServerSendingReply(in pCookie, fIsAsync);
        }

        HResult ICorProfilerCallback.UnmanagedToManagedTransition(FunctionId functionId, COR_PRF_TRANSITION_REASON reason)
        {
            return UnmanagedToManagedTransition(functionId, reason);
        }

        HResult ICorProfilerCallback.ManagedToUnmanagedTransition(FunctionId functionId, COR_PRF_TRANSITION_REASON reason)
        {
            return ManagedToUnmanagedTransition(functionId, reason);
        }

        HResult ICorProfilerCallback.RuntimeSuspendStarted(COR_PRF_SUSPEND_REASON suspendReason)
        {
            return RuntimeSuspendStarted(suspendReason);
        }

        HResult ICorProfilerCallback.RuntimeSuspendFinished()
        {
            return RuntimeSuspendFinished();
        }

        HResult ICorProfilerCallback.RuntimeSuspendAborted()
        {
            return RuntimeSuspendAborted();
        }

        HResult ICorProfilerCallback.RuntimeResumeStarted()
        {
            return RuntimeResumeStarted();
        }

        HResult ICorProfilerCallback.RuntimeResumeFinished()
        {
            return RuntimeResumeFinished();
        }

        HResult ICorProfilerCallback.RuntimeThreadSuspended(ThreadId threadId)
        {
            return RuntimeThreadSuspended(threadId);
        }

        HResult ICorProfilerCallback.RuntimeThreadResumed(ThreadId threadId)
        {
            return RuntimeThreadResumed(threadId);
        }

        unsafe HResult ICorProfilerCallback.MovedReferences(uint cMovedObjectIDRanges, ObjectId* oldObjectIDRangeStart, ObjectId* newObjectIDRangeStart, uint* cObjectIDRangeLength)
        {
            return MovedReferences(cMovedObjectIDRanges, oldObjectIDRangeStart, newObjectIDRangeStart, cObjectIDRangeLength);
        }

        HResult ICorProfilerCallback.ObjectAllocated(ObjectId objectId, ClassId classId)
        {
            return ObjectAllocated(objectId, classId);
        }

        unsafe HResult ICorProfilerCallback.ObjectsAllocatedByClass(uint cClassCount, ClassId* classIds, uint* cObjects)
        {
            return ObjectsAllocatedByClass(cClassCount, classIds, cObjects);
        }

        unsafe HResult ICorProfilerCallback.ObjectReferences(ObjectId objectId, ClassId classId, uint cObjectRefs, ObjectId* objectRefIds)
        {
            return ObjectReferences(objectId, classId, cObjectRefs, objectRefIds);
        }

        unsafe HResult ICorProfilerCallback.RootReferences(uint cRootRefs, ObjectId* rootRefIds)
        {
            return RootReferences(cRootRefs, rootRefIds);
        }

        HResult ICorProfilerCallback.ExceptionThrown(ObjectId thrownObjectId)
        {
            return ExceptionThrown(thrownObjectId);
        }

        HResult ICorProfilerCallback.ExceptionSearchFunctionEnter(FunctionId functionId)
        {
            return ExceptionSearchFunctionEnter(functionId);
        }

        HResult ICorProfilerCallback.ExceptionSearchFunctionLeave()
        {
            return ExceptionSearchFunctionLeave();
        }

        HResult ICorProfilerCallback.ExceptionSearchFilterEnter(FunctionId functionId)
        {
            return ExceptionSearchFilterEnter(functionId);
        }

        HResult ICorProfilerCallback.ExceptionSearchFilterLeave()
        {
            return ExceptionSearchFilterLeave();
        }

        HResult ICorProfilerCallback.ExceptionSearchCatcherFound(FunctionId functionId)
        {
            return ExceptionSearchCatcherFound(functionId);
        }

        unsafe HResult ICorProfilerCallback.ExceptionOSHandlerEnter(nint* __unused)
        {
            return ExceptionOSHandlerEnter(__unused);
        }

        unsafe HResult ICorProfilerCallback.ExceptionOSHandlerLeave(nint* __unused)
        {
            return ExceptionOSHandlerLeave(__unused);
        }

        HResult ICorProfilerCallback.ExceptionUnwindFunctionEnter(FunctionId functionId)
        {
            return ExceptionUnwindFunctionEnter(functionId);
        }

        HResult ICorProfilerCallback.ExceptionUnwindFunctionLeave()
        {
            return ExceptionUnwindFunctionLeave();
        }

        HResult ICorProfilerCallback.ExceptionUnwindFinallyEnter(FunctionId functionId)
        {
            return ExceptionUnwindFinallyEnter(functionId);
        }

        HResult ICorProfilerCallback.ExceptionUnwindFinallyLeave()
        {
            return ExceptionUnwindFinallyLeave();
        }

        HResult ICorProfilerCallback.ExceptionCatcherEnter(FunctionId functionId, ObjectId objectId)
        {
            return ExceptionCatcherEnter(functionId, objectId);
        }

        HResult ICorProfilerCallback.ExceptionCatcherLeave()
        {
            return ExceptionCatcherLeave();
        }

        unsafe HResult ICorProfilerCallback.COMClassicVTableCreated(ClassId wrappedClassId, in Guid implementedIID, void* pVTable, uint cSlots)
        {
            return COMClassicVTableCreated(wrappedClassId, in implementedIID, pVTable, cSlots);
        }

        unsafe HResult ICorProfilerCallback.COMClassicVTableDestroyed(ClassId wrappedClassId, in Guid implementedIID, void* pVTable)
        {
            return COMClassicVTableDestroyed(wrappedClassId, in implementedIID, pVTable);
        }

        HResult ICorProfilerCallback.ExceptionCLRCatcherFound()
        {
            return ExceptionCLRCatcherFound();
        }

        HResult ICorProfilerCallback.ExceptionCLRCatcherExecute()
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
