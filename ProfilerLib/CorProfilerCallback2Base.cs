namespace ProfilerLib
{
    public class CorProfilerCallback2Base : CorProfilerCallbackBase, ICorProfilerCallback2
    {
        private readonly NativeObjects.ICorProfilerCallback2 _corProfilerCallback2;

        public CorProfilerCallback2Base()
        {
            _corProfilerCallback2 = NativeObjects.ICorProfilerCallback2.Wrap(this);
        }
        public override HResult QueryInterface(in Guid guid, out IntPtr ptr)
        {
            if (guid == ICorProfilerCallback2.Guid)
            {
                ptr = _corProfilerCallback2;
                return HResult.S_OK;
            }

            return base.QueryInterface(guid, out ptr);
        }

        public virtual unsafe HResult ThreadNameChanged(ThreadId threadId, uint cchName, char* name)
        {
            return default;
        }

        public virtual unsafe HResult GarbageCollectionStarted(int cGenerations, bool* generationCollected, COR_PRF_GC_REASON reason)
        {
            return default;
        }

        public virtual unsafe HResult SurvivingReferences(uint cSurvivingObjectIDRanges, ObjectId* objectIDRangeStart, uint* cObjectIDRangeLength)
        {
            return default;
        }

        public virtual HResult GarbageCollectionFinished()
        {
            return default;
        }

        public virtual HResult FinalizeableObjectQueued(int finalizerFlags, ObjectId objectID)
        {
            return default;
        }

        public virtual unsafe HResult RootReferences2(uint cRootRefs, ObjectId* rootRefIds, COR_PRF_GC_ROOT_KIND* rootKinds, COR_PRF_GC_ROOT_FLAGS* rootFlags, uint* rootIds)
        {
            return default;
        }

        public virtual HResult HandleCreated(GCHandleId handleId, ObjectId initialObjectId)
        {
            return default;
        }

        public virtual HResult HandleDestroyed(GCHandleId handleId)
        {
            return default;
        }
    }
}
