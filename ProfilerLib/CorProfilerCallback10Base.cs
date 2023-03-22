namespace ProfilerLib
{
    public class CorProfilerCallback10Base : CorProfilerCallback9Base, ICorProfilerCallback10
    {
        private readonly NativeObjects.ICorProfilerCallback10 _corProfilerCallback10;

        public CorProfilerCallback10Base()
        {
            _corProfilerCallback10 = NativeObjects.ICorProfilerCallback10.Wrap(this);
        }

        public override HResult QueryInterface(in Guid guid, out nint ptr)
        {
            if (guid == ICorProfilerCallback10.Guid)
            {
                ptr = _corProfilerCallback10;
                return HResult.S_OK;
            }

            return base.QueryInterface(guid, out ptr);
        }

        public virtual unsafe HResult EventPipeEventDelivered(nint provider, int eventId, int eventVersion, uint cbMetadataBlob, byte* metadataBlob, uint cbEventData, byte* eventData, in Guid pActivityId, in Guid pRelatedActivityId, ThreadId eventThread, uint numStackFrames, nint* stackFrames)
        {
            return default;
        }

        public virtual HResult EventPipeProviderCreated(nint provider)
        {
            return default;
        }
    }
}
