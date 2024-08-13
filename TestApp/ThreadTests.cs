namespace TestApp;

internal class ThreadTests
{
    public static void Run()
    {
        var threadId = CreateAndDestroyThread();

        // Make sure the thread is finalized
        GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced, true);
        GC.WaitForPendingFinalizers();

        var logs = Logs.Fetch().ToList();

        Logs.AssertContains(logs, $"ThreadCreated - {threadId}");
        Logs.AssertContains(logs, $"ThreadDestroyed - {threadId}");
        Logs.AssertContains(logs, $"ThreadAssignedToOSThread - {threadId}");
        Logs.AssertContains(logs, "ThreadNameChanged - Test");

    }

    private static uint CreateAndDestroyThread()
    {
        uint id = 0;

        var thread = new Thread(() => 
        { 
            id = PInvokes.Win32.GetCurrentThreadId();
            Thread.CurrentThread.Name = "Test";
        });

        thread.Start();
        thread.Join();

        return id;
    }
}