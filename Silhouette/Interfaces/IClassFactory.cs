using Silhouette;

namespace Silhouette.Interfaces;

[NativeObject]
public interface IClassFactory : IUnknown
{
    HResult CreateInstance(nint outer, in Guid guid, out nint instance);

    HResult LockServer(bool @lock);

}