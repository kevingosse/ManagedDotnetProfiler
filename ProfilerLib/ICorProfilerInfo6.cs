namespace ProfilerLib;

public class ICorProfilerInfo6 : ICorProfilerInfo5
{
    private NativeObjects.ICorProfilerInfo6Invoker _impl;

    public ICorProfilerInfo6(IntPtr ptr) : base(ptr)
    {
        _impl = new(ptr);
    }

    public HResult EnumNgenModuleMethodsInliningThisMethod(ModuleId inlinersModuleId, ModuleId inlineeModuleId, MdMethodDef inlineeMethodId, out int incompleteData, out nint ppEnum)
    {
        return _impl.EnumNgenModuleMethodsInliningThisMethod(inlinersModuleId, inlineeModuleId, inlineeMethodId, out incompleteData, out ppEnum);
    }
}