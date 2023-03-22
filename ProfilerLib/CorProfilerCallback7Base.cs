namespace ProfilerLib
{
    public class CorProfilerCallback7Base : CorProfilerCallback6Base, ICorProfilerCallback7
    {
        private readonly NativeObjects.ICorProfilerCallback7 _corProfilerCallback7;

        public CorProfilerCallback7Base()
        {
            _corProfilerCallback7 = NativeObjects.ICorProfilerCallback7.Wrap(this);
        }

        public override HResult QueryInterface(in Guid guid, out nint ptr)
        {
            if (guid == ICorProfilerCallback7.Guid)
            {
                ptr = _corProfilerCallback7;
                return HResult.S_OK;
            }

            return base.QueryInterface(guid, out ptr);
        }

        public virtual HResult ModuleInMemorySymbolsUpdated(ModuleId moduleId)
        {
            return default;
        }
    }
}
