using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Loader;
using TestApp;


Console.WriteLine($"PID: {Environment.ProcessId}");


var logs = FetchLogs().ToList();

foreach (var log in logs)
{
    Console.WriteLine(log);
}

AssertContains(logs, "ClassLoadFinished - System.Array");
AssertContains(logs, $"AssemblyLoadFinished - TestApp - AppDomain clrhost - Module {typeof(Program).Assembly.Location}");
AssertContains(logs, $"AppDomainCreationStarted - System.Private.CoreLib.dll - Process Id {Environment.ProcessId}");
AssertContains(logs, $"AppDomainCreationStarted - DefaultDomain - Process Id {Environment.ProcessId}");
AssertContains(logs, "AppDomainCreationFinished - System.Private.CoreLib.dll - HResult S_OK");
AssertContains(logs, "AppDomainCreationFinished - DefaultDomain - HResult S_OK");


var threadId = (IntPtr)typeof(Thread).GetField("_DONT_USE_InternalThread", BindingFlags.Instance | BindingFlags.NonPublic)
    .GetValue(Thread.CurrentThread);

var osId = PInvokes.Win32.GetCurrentThreadId();

Assert(PInvokes.GetThreadId((ulong)threadId, (int)osId));


foreach (var log in FetchLogs())
{
    Console.WriteLine(log);
}

Console.WriteLine("**** Try loading/unloading assembly");

CreateAndUnloadAlc();

GC.Collect(2, GCCollectionMode.Forced, true);
GC.WaitForPendingFinalizers();
GC.Collect(2, GCCollectionMode.Forced, true);

logs = FetchLogs().ToList();

AssertContains(logs, $"AssemblyUnloadFinished - TestApp - AppDomain clrhost - Module {typeof(Program).Assembly.Location}");

/*

for (int j = 0; j < 10; j++)
{
    Console.WriteLine("**** Try loading/unloading assembly");

    CreateAndUnloadAlc(out var weakref);

    Console.WriteLine(weakref.IsAlive);

    for (int i = 0; i < 10; i++)
    {
        GC.Collect(2, GCCollectionMode.Forced, true);
        GC.WaitForPendingFinalizers();
        GC.Collect(2, GCCollectionMode.Forced, true);
    }

    Console.WriteLine(weakref.IsAlive);
}

*/

[MethodImpl(MethodImplOptions.NoInlining)]
static void CreateAndUnloadAlc()
{
    var alc = new TestAssemblyLoadContext();
    _ = alc.LoadFromAssemblyPath(typeof(Program).Assembly.Location);
    alc.Unload();
}

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