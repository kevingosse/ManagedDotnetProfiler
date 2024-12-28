using System.Runtime.InteropServices;

namespace TestApp;

internal class PInvokes
{
    public class Win32
    {
        [DllImport("kernel32.dll")]
        public static extern uint GetCurrentThreadId();
    }

    [DllImport("ManagedDotnetProfiler.dll")]
    public static extern unsafe int FetchLastLog(char* buffer, int bufferSize);

    [DllImport("ManagedDotnetProfiler.dll")]
    public static extern bool GetThreadId(ulong expectedThreadId, int expectedOSId);
}