using System.Runtime.InteropServices;

namespace ManagedDotnetProfiler;

public class DllMain
{
    private static ClassFactory Instance;

    [UnmanagedCallersOnly(EntryPoint = "DllGetClassObject")]
    public static unsafe int DllGetClassObject(void* rclsid, void* riid, nint* ppv)
    {
        Instance = new ClassFactory();
        *ppv = Instance.IClassFactory;

        return 0;
    }
}