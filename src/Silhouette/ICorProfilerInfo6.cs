namespace Silhouette;

public class ICorProfilerInfo6 : ICorProfilerInfo5
{
    private NativeObjects.ICorProfilerInfo6Invoker _impl;

    public ICorProfilerInfo6(nint ptr) : base(ptr)
    {
        _impl = new(ptr);
    }

    public HResult<NgenModuleMethodsInliningThisMethod> EnumNgenModuleMethodsInliningThisMethod(ModuleId inlinersModuleId, ModuleId inlineeModuleId, MdMethodDef inlineeMethodId)
    {
        var result = _impl.EnumNgenModuleMethodsInliningThisMethod(inlinersModuleId, inlineeModuleId, inlineeMethodId, out var incompleteData, out var pEnum);
        return new(result, new(pEnum, incompleteData != 0));
    }
}