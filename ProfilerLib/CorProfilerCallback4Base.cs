namespace ProfilerLib
{
    public abstract class CorProfilerCallback4Base : CorProfilerCallback3Base, ICorProfilerCallback4
    {
        private readonly NativeObjects.ICorProfilerCallback4 _corProfilerCallback4;

        protected CorProfilerCallback4Base()
        {
            _corProfilerCallback4 = NativeObjects.ICorProfilerCallback4.Wrap(this);
        }

        protected override HResult QueryInterface(in Guid guid, out nint ptr)
        {
            if (guid == ICorProfilerCallback4.Guid)
            {
                ptr = _corProfilerCallback4;
                return HResult.S_OK;
            }

            return base.QueryInterface(guid, out ptr);
        }

        #region ICorProfilerCallback4

        HResult ICorProfilerCallback4.GetReJITParameters(ModuleId moduleId, MdMethodDef methodId, nint pFunctionControl)
        {
            return GetReJITParameters(moduleId, methodId, pFunctionControl);
        }

        HResult ICorProfilerCallback4.ReJITCompilationFinished(FunctionId functionId, ReJITId rejitId, HResult hrStatus, bool fIsSafeToBlock)
        {
            return ReJITCompilationFinished(functionId, rejitId, hrStatus, fIsSafeToBlock);
        }

        HResult ICorProfilerCallback4.ReJITError(ModuleId moduleId, MdMethodDef methodId, FunctionId functionId, HResult hrStatus)
        {
            return ReJITError(moduleId, methodId, functionId, hrStatus);
        }

        unsafe HResult ICorProfilerCallback4.MovedReferences2(uint cMovedObjectIDRanges, ObjectId* oldObjectIDRangeStart, ObjectId* newObjectIDRangeStart, nint* cObjectIDRangeLength)
        {
            return MovedReferences2(cMovedObjectIDRanges, oldObjectIDRangeStart, newObjectIDRangeStart, cObjectIDRangeLength);
        }

        unsafe HResult ICorProfilerCallback4.SurvivingReferences2(uint cSurvivingObjectIDRanges, ObjectId* objectIDRangeStart, nint* cObjectIDRangeLength)
        {
            return SurvivingReferences2(cSurvivingObjectIDRanges, objectIDRangeStart, cObjectIDRangeLength);
        }

        HResult ICorProfilerCallback4.ReJITCompilationStarted(FunctionId functionId, ReJITId rejitId, bool fIsSafeToBlock)
        {
            return ReJITCompilationStarted(functionId, rejitId, fIsSafeToBlock);
        }

        #endregion

        protected virtual HResult ReJITCompilationStarted(FunctionId functionId, ReJITId rejitId, bool fIsSafeToBlock)
        {
            return HResult.E_NOTIMPL;
        }

        protected virtual HResult GetReJITParameters(ModuleId moduleId, MdMethodDef methodId, nint pFunctionControl)
        {
            return HResult.E_NOTIMPL;
        }

        protected virtual HResult ReJITCompilationFinished(FunctionId functionId, ReJITId rejitId, HResult hrStatus, bool fIsSafeToBlock)
        {
            return HResult.E_NOTIMPL;
        }

        protected virtual HResult ReJITError(ModuleId moduleId, MdMethodDef methodId, FunctionId functionId, HResult hrStatus)
        {
            return HResult.E_NOTIMPL;
        }

        protected virtual unsafe HResult MovedReferences2(uint cMovedObjectIDRanges, ObjectId* oldObjectIDRangeStart, ObjectId* newObjectIDRangeStart, nint* cObjectIDRangeLength)
        {
            return HResult.E_NOTIMPL;
        }

        protected virtual unsafe HResult SurvivingReferences2(uint cSurvivingObjectIDRanges, ObjectId* objectIDRangeStart, nint* cObjectIDRangeLength)
        {
            return HResult.E_NOTIMPL;
        }
    }
}
