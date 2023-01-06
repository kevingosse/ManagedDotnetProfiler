using System;

namespace ManagedDotnetProfiler;

public unsafe class ClassFactory : IClassFactory
{
    private NativeObjects.IClassFactory _classFactory;

    private CorProfilerCallback2 _callback = new();

    public ClassFactory()
    {
        _classFactory = NativeObjects.IClassFactory.Wrap(this);
    }

    public IntPtr IClassFactory => _classFactory;

    public HResult CreateInstance(IntPtr outer, in Guid guid, out IntPtr instance)
    {
        instance = _callback.ICorProfilerCallback2Object;
        return HResult.S_OK;

        // return _callback.IUnknown.QueryInterface(_callback.IUnknownObject, guid, out instance);
    }

    public HResult LockServer(bool @lock)
    {
        Console.WriteLine("ClassFactory - LockServer");
        return default;
    }

    public HResult QueryInterface(in Guid guid, out IntPtr ptr)
    {
        Console.WriteLine("ClassFactory - QueryInterface - " + guid);

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