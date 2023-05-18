namespace TestApp;

internal static class ExceptionTests
{
    public static void Run(nint threadId)
    {
        try
        {
            throw new InvalidOperationException("Expected");
        }
        catch (Exception)
        {
            try
            {
                throw new TaskCanceledException("Expected");
            }
            catch (OperationCanceledException) when (ExceptionFilter1())
            {
            }
        }

        var logs = Logs.Fetch().ToList();

        foreach (var log in logs)
        {
            Console.WriteLine(log);
        }

        Logs.AssertContains(logs, "ExceptionCatcherEnter - catch System.InvalidOperationException in TestApp.ExceptionTests.Run");
        Logs.AssertContains(logs, "ExceptionCatcherEnter - catch System.Threading.Tasks.TaskCanceledException in TestApp.ExceptionTests.Run");
        Logs.AssertContains(logs, $"ExceptionCatcherLeave - Thread {threadId:x2} - Nested level 1");
        Logs.AssertContains(logs, $"ExceptionCatcherLeave - Thread {threadId:x2} - Nested level 0");
        Logs.AssertContains(logs, "ExceptionSearchFilterEnter - TestApp.ExceptionTests.Run");
        Logs.AssertContains(logs, "ExceptionSearchFilterEnter - TestApp.ExceptionTests.ExceptionFilter1");
        Logs.AssertContains(logs, $"ExceptionSearchFilterLeave - Thread {threadId:x2} - Nested level 1");
        Logs.AssertContains(logs, $"ExceptionSearchFilterLeave - Thread {threadId:x2} - Nested level 0");
    }
    
    private static bool ExceptionFilter1()
    {
        try
        {
            throw new Exception("Expected");
        }
        catch (Exception) when (ExceptionFilter2())
        {
        }
        
        return true;
    }

    private static bool ExceptionFilter2()
    {
        return true;
    }
}