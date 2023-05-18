using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Net.Http.Headers;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
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


foreach (var log in FetchLogs())
{
    Console.WriteLine(log);
}

TestCreateAndUnloadAlc();

TestCreateAndUnloadType();

TestCom();

TestConditionalWeakTable();

TestDynamicMethod();

static void TestDynamicMethod()
{
    //var dynamicMethod = new DynamicMethod("test", null, null);
    //var ilGenerator = dynamicMethod.GetILGenerator();
    //ilGenerator.Emit(OpCodes.Ret);

    //var handle = (RuntimeMethodHandle)typeof(DynamicMethod).GetMethod("GetMethodDescriptor", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(dynamicMethod, null);

    //dynamicMethod.CreateDelegate<Action>().Invoke();

    //dynamicMethod = null;

    var handle = InnerScope();

    GC.Collect(2, GCCollectionMode.Forced, true);
    GC.WaitForPendingFinalizers();
    GC.Collect(2, GCCollectionMode.Forced, true);
    GC.WaitForPendingFinalizers();

    var logs = FetchLogs().ToList();

    AssertContains(logs, $"DynamicMethodJITCompilationStarted - {handle:x2}");
    AssertContains(logs, $"DynamicMethodJITCompilationFinished - {handle:x2}");
    AssertContains(logs, $"DynamicMethodUnloaded - {handle:x2}");
}

static string InnerScope()
{
    var dynamicMethod = new DynamicMethod("test", null, null);
    var ilGenerator = dynamicMethod.GetILGenerator();
    ilGenerator.Emit(OpCodes.Ret);

    var handle = (RuntimeMethodHandle)typeof(DynamicMethod).GetMethod("GetMethodDescriptor", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(dynamicMethod, null);

    dynamicMethod.CreateDelegate<Action>().Invoke();
    
    return handle.Value.ToString("x2");
}

static void TestConditionalWeakTable()
{
    var cwt = new ConditionalWeakTable<string, string>();
    cwt.Add("hello", "world");
    GC.Collect(2, GCCollectionMode.Forced, true);

    var logs = FetchLogs().ToList();
    AssertContains(logs, "ConditionalWeakTableElementReferences - hello -> world");
}

static void TestCom()
{
    _ = Marshal.GetIUnknownForObject(new object());

    var logs = FetchLogs().ToList();
    AssertContains(logs, "COMClassicVTableCreated - System.Object - fbec27f0-fc44-396c-8a8c-4c1993516f2d - 11");
}

static void TestCreateAndUnloadType()
{
    Console.WriteLine("**** Try loading/unloading type");

    CreateAndUnloadType();

    GC.Collect(2, GCCollectionMode.Forced, true);
    GC.WaitForPendingFinalizers();
    GC.Collect(2, GCCollectionMode.Forced, true);

    var logs = FetchLogs().ToList();

    AssertContains(logs, "ClassLoadFinished - DynamicType");
    AssertContains(logs, "ClassUnloadStarted - DynamicType");
    AssertContains(logs, "ClassUnloadFinished - DynamicType");
}

static void CreateAndUnloadType()
{
    var assembly = AssemblyBuilder.DefineDynamicAssembly(new("DynamicAssembly"), AssemblyBuilderAccess.RunAndCollect);
    var module = assembly.DefineDynamicModule("DynamicModule");
    var type = module.DefineType("DynamicType");
    type.CreateType();
}

static void TestCreateAndUnloadAlc()
{
    Console.WriteLine("**** Try loading/unloading assembly");

    CreateAndUnloadAlc();

    GC.Collect(2, GCCollectionMode.Forced, true);
    GC.WaitForPendingFinalizers();
    GC.Collect(2, GCCollectionMode.Forced, true);

    var logs = FetchLogs().ToList();

    foreach (var log in logs)
    {
        Console.WriteLine(log);
    }

    AssertContains(logs, $"AssemblyUnloadFinished - TestApp - AppDomain clrhost - Module {typeof(Program).Assembly.Location}");
}

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

public class TestCom
{
    public int Method() => 42;
}