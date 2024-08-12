namespace TestApp;

internal class PInvokeTests
{
    public static void Run()
    {
        _ = PInvokes.Win32.GetCurrentThreadId();

        var logs = Logs.Fetch().ToList();
        Logs.AssertContains(logs, "ManagedToUnmanagedTransition - Win32.GetCurrentThreadId - COR_PRF_TRANSITION_CALL");
    }
}