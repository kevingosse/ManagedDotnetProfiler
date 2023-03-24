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

        public override HResult QueryInterface(in Guid guid, out nint ptr)
        {
            Console.WriteLine($"CorProfilerCallbackBase - QueryInterface - {guid}");

            ptr = 0;

            return HResult.E_NOTIMPL;
        }

        public virtual HResult Initialize(nint pICorProfilerInfoUnk)
        {
            Console.WriteLine($"CorProfilerCallbackBase - Initialize");
            
            int version = GetICorProfilerInfo(pICorProfilerInfoUnk);

            Console.WriteLine($"[Profiler] Fetched ICorProfilerInfo{version}");

            return OnInitialize(version);
        }

        protected abstract HResult OnInitialize(int iCorProfilerInfoVersion);

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

        public virtual HResult Shutdown()
        {
            return default;
        }

        public virtual HResult AppDomainCreationStarted(AppDomainId appDomainId)
        {
            return default;
        }

        public virtual HResult AppDomainCreationFinished(AppDomainId appDomainId, HResult hrStatus)
        {
            return default;
        }

        public virtual HResult AppDomainShutdownStarted(AppDomainId appDomainId)
        {
            return default;
        }

        public virtual HResult AppDomainShutdownFinished(AppDomainId appDomainId, HResult hrStatus)
        {
            return default;
        }

        public virtual HResult AssemblyLoadStarted(AssemblyId assemblyId)
        {
            return default;
        }

        public virtual HResult AssemblyLoadFinished(AssemblyId assemblyId, HResult hrStatus)
        {
            return default;
        }

        public virtual HResult AssemblyUnloadStarted(AssemblyId assemblyId)
        {
            return default;
        }

        public virtual HResult AssemblyUnloadFinished(AssemblyId assemblyId, HResult hrStatus)
        {
            return default;
        }

        public virtual HResult ModuleLoadStarted(ModuleId moduleId)
        {
            return default;
        }

        public virtual HResult ModuleLoadFinished(ModuleId moduleId, HResult hrStatus)
        {
            return default;
        }

        public virtual HResult ModuleUnloadStarted(ModuleId moduleId)
        {
            return default;
        }

        public virtual HResult ModuleUnloadFinished(ModuleId moduleId, HResult hrStatus)
        {
            return default;
        }

        public virtual HResult ModuleAttachedToAssembly(ModuleId moduleId, AssemblyId assemblyId)
        {
            return default;
        }

        public virtual HResult ClassLoadStarted(ClassId classId)
        {
            return default;
        }

        public virtual HResult ClassLoadFinished(ClassId classId, HResult hrStatus)
        {
            return default;
        }

        public virtual HResult ClassUnloadStarted(ClassId classId)
        {
            return default;
        }

        public virtual HResult ClassUnloadFinished(ClassId classId, HResult hrStatus)
        {
            return default;
        }

        public virtual HResult FunctionUnloadStarted(FunctionId functionId)
        {
            return default;
        }

        public virtual HResult JITCompilationStarted(FunctionId functionId, bool fIsSafeToBlock)
        {
            return default;
        }

        public virtual HResult JITCompilationFinished(FunctionId functionId, HResult hrStatus, bool fIsSafeToBlock)
        {
            return default;
        }

        public virtual HResult JITCachedFunctionSearchStarted(FunctionId functionId, out bool pbUseCachedFunction)
        {
            pbUseCachedFunction = false;

            return default;
        }

        public virtual HResult JITCachedFunctionSearchFinished(FunctionId functionId, COR_PRF_JIT_CACHE result)
        {
            return default;
        }

        public virtual HResult JITFunctionPitched(FunctionId functionId)
        {
            return default;
        }

        public virtual HResult JITInlining(FunctionId callerId, FunctionId calleeId, out bool pfShouldInline)
        {
            pfShouldInline = false;

            return default;
        }

        public virtual HResult ThreadCreated(ThreadId threadId)
        {
            return default;
        }

        public virtual HResult ThreadDestroyed(ThreadId threadId)
        {
            return default;
        }

        public virtual HResult ThreadAssignedToOSThread(ThreadId managedThreadId, int osThreadId)
        {
            return default;
        }

        public virtual HResult RemotingClientInvocationStarted()
        {
            return default;
        }

        public virtual HResult RemotingClientSendingMessage(in Guid pCookie, bool fIsAsync)
        {
            return default;
        }

        public virtual HResult RemotingClientReceivingReply(in Guid pCookie, bool fIsAsync)
        {
            return default;
        }

        public virtual HResult RemotingClientInvocationFinished()
        {
            return default;
        }

        public virtual HResult RemotingServerReceivingMessage(in Guid pCookie, bool fIsAsync)
        {
            return default;
        }

        public virtual HResult RemotingServerInvocationStarted()
        {
            return default;
        }

        public virtual HResult RemotingServerInvocationReturned()
        {
            return default;
        }

        public virtual HResult RemotingServerSendingReply(in Guid pCookie, bool fIsAsync)
        {
            return default;
        }

        public virtual HResult UnmanagedToManagedTransition(FunctionId functionId, COR_PRF_TRANSITION_REASON reason)
        {
            return default;
        }

        public virtual HResult ManagedToUnmanagedTransition(FunctionId functionId, COR_PRF_TRANSITION_REASON reason)
        {
            return default;
        }

        public virtual HResult RuntimeSuspendStarted(COR_PRF_SUSPEND_REASON suspendReason)
        {
            return default;
        }

        public virtual HResult RuntimeSuspendFinished()
        {
            return default;
        }

        public virtual HResult RuntimeSuspendAborted()
        {
            return default;
        }

        public virtual HResult RuntimeResumeStarted()
        {
            return default;
        }

        public virtual HResult RuntimeResumeFinished()
        {
            return default;
        }

        public virtual HResult RuntimeThreadSuspended(ThreadId threadId)
        {
            return default;
        }

        public virtual HResult RuntimeThreadResumed(ThreadId threadId)
        {
            return default;
        }

        public virtual unsafe HResult MovedReferences(uint cMovedObjectIDRanges, ObjectId* oldObjectIDRangeStart, ObjectId* newObjectIDRangeStart, uint* cObjectIDRangeLength)
        {
            return default;
        }

        public virtual HResult ObjectAllocated(ObjectId objectId, ClassId classId)
        {
            return default;
        }

        public virtual unsafe HResult ObjectsAllocatedByClass(uint cClassCount, ClassId* classIds, uint* cObjects)
        {
            return default;
        }

        public virtual unsafe HResult ObjectReferences(ObjectId objectId, ClassId classId, uint cObjectRefs, ObjectId* objectRefIds)
        {
            return default;
        }

        public virtual unsafe HResult RootReferences(uint cRootRefs, ObjectId* rootRefIds)
        {
            return default;
        }

        public virtual HResult ExceptionThrown(ObjectId thrownObjectId)
        {
            return default;
        }

        public virtual HResult ExceptionSearchFunctionEnter(FunctionId functionId)
        {
            return default;
        }

        public virtual HResult ExceptionSearchFunctionLeave()
        {
            return default;
        }

        public virtual HResult ExceptionSearchFilterEnter(FunctionId functionId)
        {
            return default;
        }

        public virtual HResult ExceptionSearchFilterLeave()
        {
            return default;
        }

        public virtual HResult ExceptionSearchCatcherFound(FunctionId functionId)
        {
            return default;
        }

        public virtual unsafe HResult ExceptionOSHandlerEnter(nint* __unused)
        {
            return default;
        }

        public virtual unsafe HResult ExceptionOSHandlerLeave(nint* __unused)
        {
            return default;
        }

        public virtual HResult ExceptionUnwindFunctionEnter(FunctionId functionId)
        {
            return default;
        }

        public virtual HResult ExceptionUnwindFunctionLeave()
        {
            return default;
        }

        public virtual HResult ExceptionUnwindFinallyEnter(FunctionId functionId)
        {
            return default;
        }

        public virtual HResult ExceptionUnwindFinallyLeave()
        {
            return default;
        }

        public virtual HResult ExceptionCatcherEnter(FunctionId functionId, ObjectId objectId)
        {
            return default;
        }

        public virtual HResult ExceptionCatcherLeave()
        {
            return default;
        }

        public virtual unsafe HResult COMClassicVTableCreated(ClassId wrappedClassId, in Guid implementedIID, void* pVTable, uint cSlots)
        {
            return default;
        }

        public virtual unsafe HResult COMClassicVTableDestroyed(ClassId wrappedClassId, in Guid implementedIID, void* pVTable)
        {
            return default;
        }

        public virtual HResult ExceptionCLRCatcherFound()
        {
            return default;
        }

        public virtual HResult ExceptionCLRCatcherExecute()
        {
            return default;
        }
    }
}
