namespace ProfilerLib
{
    public abstract class CorProfilerCallback2Base : CorProfilerCallbackBase, Interfaces.ICorProfilerCallback2
    {
        private readonly NativeObjects.ICorProfilerCallback2 _corProfilerCallback2;

        protected CorProfilerCallback2Base()
        {
            _corProfilerCallback2 = NativeObjects.ICorProfilerCallback2.Wrap(this);
        }

        protected override HResult QueryInterface(in Guid guid, out IntPtr ptr)
        {
            if (guid == Interfaces.ICorProfilerCallback2.Guid)
            {
                ptr = _corProfilerCallback2;
                return HResult.S_OK;
            }

            return base.QueryInterface(guid, out ptr);
        }

        #region ICorProfilerCallback2

        unsafe HResult Interfaces.ICorProfilerCallback2.GarbageCollectionStarted(int cGenerations, bool* generationCollected, COR_PRF_GC_REASON reason)
        {
            return GarbageCollectionStarted(cGenerations, generationCollected, reason);
        }

        unsafe HResult Interfaces.ICorProfilerCallback2.SurvivingReferences(uint cSurvivingObjectIDRanges, ObjectId* objectIDRangeStart, uint* cObjectIDRangeLength)
        {
            return SurvivingReferences(cSurvivingObjectIDRanges, objectIDRangeStart, cObjectIDRangeLength);
        }

        HResult Interfaces.ICorProfilerCallback2.GarbageCollectionFinished()
        {
            return GarbageCollectionFinished();
        }

        HResult Interfaces.ICorProfilerCallback2.FinalizeableObjectQueued(int finalizerFlags, ObjectId objectID)
        {
            return FinalizeableObjectQueued(finalizerFlags, objectID);
        }

        unsafe HResult Interfaces.ICorProfilerCallback2.RootReferences2(uint cRootRefs, ObjectId* rootRefIds, COR_PRF_GC_ROOT_KIND* rootKinds, COR_PRF_GC_ROOT_FLAGS* rootFlags, uint* rootIds)
        {
            return RootReferences2(cRootRefs, rootRefIds, rootKinds, rootFlags, rootIds);
        }

        HResult Interfaces.ICorProfilerCallback2.HandleCreated(GCHandleId handleId, ObjectId initialObjectId)
        {
            return HandleCreated(handleId, initialObjectId);
        }

        HResult Interfaces.ICorProfilerCallback2.HandleDestroyed(GCHandleId handleId)
        {
            return HandleDestroyed(handleId);
        }

        unsafe HResult Interfaces.ICorProfilerCallback2.ThreadNameChanged(ThreadId threadId, uint cchName, char* name)
        {
            return ThreadNameChanged(threadId, cchName, name);
        }

        #endregion

        protected virtual unsafe HResult ThreadNameChanged(ThreadId threadId, uint cchName, char* name)
        {
            return HResult.E_NOTIMPL;
        }

        protected virtual unsafe HResult GarbageCollectionStarted(int cGenerations, bool* generationCollected, COR_PRF_GC_REASON reason)
        {
            return HResult.E_NOTIMPL;
        }

        protected virtual unsafe HResult SurvivingReferences(uint cSurvivingObjectIDRanges, ObjectId* objectIDRangeStart, uint* cObjectIDRangeLength)
        {
            return HResult.E_NOTIMPL;
        }

        protected virtual HResult GarbageCollectionFinished()
        {
            return HResult.E_NOTIMPL;
        }

        protected virtual HResult FinalizeableObjectQueued(int finalizerFlags, ObjectId objectID)
        {
            return HResult.E_NOTIMPL;
        }

        protected virtual unsafe HResult RootReferences2(uint cRootRefs, ObjectId* rootRefIds, COR_PRF_GC_ROOT_KIND* rootKinds, COR_PRF_GC_ROOT_FLAGS* rootFlags, uint* rootIds)
        {
            return HResult.E_NOTIMPL;
        }

        protected virtual HResult HandleCreated(GCHandleId handleId, ObjectId initialObjectId)
        {
            return HResult.E_NOTIMPL;
        }

        protected virtual HResult HandleDestroyed(GCHandleId handleId)
        {
            return HResult.E_NOTIMPL;
        }
    }
}
