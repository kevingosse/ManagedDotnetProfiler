namespace ProfilerLib
{
    public class CorProfilerCallback4Base : CorProfilerCallback3Base, ICorProfilerCallback4
    {
        private readonly NativeObjects.ICorProfilerCallback4 _corProfilerCallback4;

        public CorProfilerCallback4Base()
        {
            _corProfilerCallback4 = NativeObjects.ICorProfilerCallback4.Wrap(this);
        }

        public override HResult QueryInterface(in Guid guid, out nint ptr)
        {
            if (guid == ICorProfilerCallback4.Guid)
            {
                ptr = _corProfilerCallback4;
                return HResult.S_OK;
            }

            return base.QueryInterface(guid, out ptr);
        }

        public virtual HResult ReJITCompilationStarted(FunctionId functionId, ReJITId rejitId, bool fIsSafeToBlock)
        {
            return default;
        }

        public virtual HResult GetReJITParameters(ModuleId moduleId, MdMethodDef methodId, nint pFunctionControl)
        {
            return default;
        }

        public virtual HResult ReJITCompilationFinished(FunctionId functionId, ReJITId rejitId, HResult hrStatus, bool fIsSafeToBlock)
        {
            return default;
        }

        public virtual HResult ReJITError(ModuleId moduleId, MdMethodDef methodId, FunctionId functionId, HResult hrStatus)
        {
            return default;
        }

        public virtual unsafe HResult MovedReferences2(uint cMovedObjectIDRanges, ObjectId* oldObjectIDRangeStart, ObjectId* newObjectIDRangeStart, nint* cObjectIDRangeLength)
        {
            return default;
        }

        public virtual unsafe HResult SurvivingReferences2(uint cSurvivingObjectIDRanges, ObjectId* objectIDRangeStart, nint* cObjectIDRangeLength)
        {
            return default;
        }
    }
}
