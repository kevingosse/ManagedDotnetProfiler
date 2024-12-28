namespace Silhouette.Interfaces;

[NativeObject]
public interface IUnknown
{
    HResult QueryInterface(in Guid guid, out nint ptr);
    int AddRef();
    int Release();
}