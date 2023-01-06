using System;

namespace ManagedDotnetProfiler;

[NativeObject]
internal interface IClassFactory : IUnknown
{
    HResult CreateInstance(IntPtr outer, in Guid guid, out IntPtr instance);

    HResult LockServer(bool @lock);

}