namespace ProfilerLib
{
    public abstract class CorProfilerCallback6Base : CorProfilerCallback5Base, ICorProfilerCallback6
    {
        private readonly NativeObjects.ICorProfilerCallback6 _corProfilerCallback6;

        protected CorProfilerCallback6Base()
        {
            _corProfilerCallback6 = NativeObjects.ICorProfilerCallback6.Wrap(this);
        }

        public override HResult QueryInterface(in Guid guid, out nint ptr)
        {
            if (guid == ICorProfilerCallback6.Guid)
            {
                ptr = _corProfilerCallback6;
                return HResult.S_OK;
            }

            return base.QueryInterface(guid, out ptr);
        }

        public virtual unsafe HResult GetAssemblyReferences(char* wszAssemblyPath, nint pAsmRefProvider)
        {
            return default;
        }
    }
}
