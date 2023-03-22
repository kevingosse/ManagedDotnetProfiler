namespace ProfilerLib
{
    public class CorProfilerCallback3Base : CorProfilerCallback2Base, ICorProfilerCallback3
    {
        private readonly NativeObjects.ICorProfilerCallback3 _corProfilerCallback3;

        public CorProfilerCallback3Base()
        {
            _corProfilerCallback3 = NativeObjects.ICorProfilerCallback3.Wrap(this);
        }

        public override HResult QueryInterface(in Guid guid, out nint ptr)
        {
            if (guid == ICorProfilerCallback3.Guid)
            {
                ptr = _corProfilerCallback3;
                return HResult.S_OK;
            }

            return base.QueryInterface(guid, out ptr);
        }

        public virtual HResult InitializeForAttach(nint pCorProfilerInfoUnk, nint pvClientData, uint cbClientData)
        {
            return default;
        }

        public virtual HResult ProfilerAttachComplete()
        {
            return default;
        }

        public virtual HResult ProfilerDetachSucceeded()
        {
            return default;
        }
    }
}
