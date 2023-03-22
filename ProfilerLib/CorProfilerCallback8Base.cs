namespace ProfilerLib
{
    public class CorProfilerCallback8Base : CorProfilerCallback7Base, ICorProfilerCallback8
    {
        private readonly NativeObjects.ICorProfilerCallback8 _corProfilerCallback8;

        public CorProfilerCallback8Base()
        {
            _corProfilerCallback8 = NativeObjects.ICorProfilerCallback8.Wrap(this);
        }

        public override HResult QueryInterface(in Guid guid, out nint ptr)
        {
            if (guid == ICorProfilerCallback8.Guid)
            {
                ptr = _corProfilerCallback8;
                return HResult.S_OK;
            }

            return base.QueryInterface(guid, out ptr);
        }

        public virtual unsafe HResult DynamicMethodJITCompilationStarted(FunctionId functionId, bool fIsSafeToBlock, byte* pILHeader, uint cbILHeader)
        {
            return default;
        }

        public virtual HResult DynamicMethodJITCompilationFinished(FunctionId functionId, HResult hrStatus, bool fIsSafeToBlock)
        {
            return default;
        }
    }
}
