using System.Reflection;
using TestApp;

Console.WriteLine($"PID: {Environment.ProcessId}");

var logs = FetchLogs().ToList();

foreach (var log in logs)
{
    Console.WriteLine(log);
}

AssertContains(logs, $"AssemblyLoadFinished - TestApp - AppDomain clrhost - Module {typeof(Program).Assembly.Location}");
AssertContains(logs, $"AppDomainCreationStarted - System.Private.CoreLib.dll - Process Id {Environment.ProcessId}");
AssertContains(logs, $"AppDomainCreationStarted - DefaultDomain - Process Id {Environment.ProcessId}");
AssertContains(logs, "AppDomainCreationFinished - System.Private.CoreLib.dll - HResult S_OK");
AssertContains(logs, "AppDomainCreationFinished - DefaultDomain - HResult S_OK");


var threadId = (IntPtr)typeof(Thread).GetField("_DONT_USE_InternalThread", BindingFlags.Instance | BindingFlags.NonPublic)
    .GetValue(Thread.CurrentThread);

var osId = PInvokes.Win32.GetCurrentThreadId();

Assert(PInvokes.GetThreadId((ulong)threadId, (int)osId));

// Dump last logs before exiting
foreach (var log in FetchLogs())
{
    Console.WriteLine(log);
}

//Console.WriteLine("Press return to throw an exception");
//Console.ReadLine();

//try
//{
//    ThrowException();
//}
//catch
//{
//}

static void AssertContains(List<string> logs, string expected)
{
    if (!logs.Contains(expected))
    {
        Console.WriteLine($"Could not find log: '{expected}'");
        Console.WriteLine("********* Assertion failed, dumping logs *********");

        foreach (var log in logs)
        {
            Console.WriteLine(log);
        }

        throw new Exception("Assertion failed");
    }
}

static void Assert(bool value)
{
    if (!value)
    {
        Console.WriteLine("********* Assertion failed, dumping logs *********");

        foreach (var log in FetchLogs())
        {
            Console.WriteLine(log);
        }

        throw new Exception("Assertion failed");
    }
}

static void ThrowException()
{
    throw new Exception("Test exception");
}

static IEnumerable<string> FetchLogs()
{
    while (true)
    {
        var log = FetchNextLog();

        if (log == null)
        {
            yield break;
        }

        if (log.StartsWith("Error:"))
        {
            throw new Exception($"Found error log: {log}");
        }

        yield return log;
    }
}

static unsafe string? FetchNextLog()
{
    const int bufferSize = 1024;
    Span<char> buffer = stackalloc char[bufferSize];

    fixed (char* c = buffer)
    {
        int length = PInvokes.FetchLastLog(c, buffer.Length);

        return length >= 0 ? new string(buffer.Slice(0, length)) : null;
    }
}