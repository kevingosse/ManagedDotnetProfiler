namespace ProfilerLib
{
    public abstract class CorProfilerCallback9Base : CorProfilerCallback8Base, ICorProfilerCallback9
    {
        private readonly NativeObjects.ICorProfilerCallback9 _corProfilerCallback9;

        protected CorProfilerCallback9Base()
        {
            _corProfilerCallback9 = NativeObjects.ICorProfilerCallback9.Wrap(this);
        }

        public override HResult QueryInterface(in Guid guid, out nint ptr)
        {
            if (guid == ICorProfilerCallback9.Guid)
            {
                ptr = _corProfilerCallback9;
                return HResult.S_OK;
            }

            return base.QueryInterface(guid, out ptr);
        }

        public virtual HResult DynamicMethodUnloaded(FunctionId functionId)
        {
            return default;
        }
    }
}
