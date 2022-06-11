using System;

namespace ManagedDotnetProfiler;

[GenerateNativeStub]
public interface IUnknown
{
    HResult QueryInterface(in Guid guid, out IntPtr ptr);
    int AddRef();
    int Release();
}