namespace ProfilerLib
{
    public class CorProfilerCallback5Base : CorProfilerCallback4Base, ICorProfilerCallback5
    {
        private readonly NativeObjects.ICorProfilerCallback5 _corProfilerCallback5;

        public CorProfilerCallback5Base()
        {
            _corProfilerCallback5 = NativeObjects.ICorProfilerCallback5.Wrap(this);
        }

        public override HResult QueryInterface(in Guid guid, out nint ptr)
        {
            if (guid == ICorProfilerCallback5.Guid)
            {
                ptr = _corProfilerCallback5;
                return HResult.S_OK;
            }

            return base.QueryInterface(guid, out ptr);
        }

        public virtual unsafe HResult ConditionalWeakTableElementReferences(uint cRootRefs, ObjectId* keyRefIds, ObjectId* valueRefIds, GCHandleId* rootIds)
        {
            return default;
        }
    }
}
