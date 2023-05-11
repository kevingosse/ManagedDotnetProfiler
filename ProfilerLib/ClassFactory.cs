namespace ProfilerLib;

public class ClassFactory : Interfaces.IClassFactory
{
    private readonly NativeObjects.IClassFactory _classFactory;

    private readonly CorProfilerCallbackBase _corProfilerCallback;

    public ClassFactory(CorProfilerCallbackBase corProfilerCallback)
    {
        _classFactory = NativeObjects.IClassFactory.Wrap(this);
        _corProfilerCallback = corProfilerCallback;
    }

    public IntPtr IClassFactory => _classFactory;

    public HResult CreateInstance(IntPtr outer, in Guid guid, out IntPtr instance)
    {
        instance = _corProfilerCallback.ICorProfilerCallback;
        return HResult.S_OK;
    }

    public HResult LockServer(bool @lock)
    {
        return default;
    }

    public HResult QueryInterface(in Guid guid, out IntPtr ptr)
    {
        if (guid == KnownGuids.ClassFactoryGuid)
        {
            ptr = IClassFactory;
            return HResult.S_OK;
        }

        ptr = IntPtr.Zero;
        return HResult.E_NOTIMPL;
    }

    public int AddRef()
    {
        return 1;
    }

    public int Release()
    {
        return 0;
    }
}
