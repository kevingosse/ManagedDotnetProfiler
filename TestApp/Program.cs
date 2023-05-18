using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using TestApp;

Console.WriteLine($"PID: {Environment.ProcessId}");

var logs = Logs.Fetch().ToList();

foreach (var log in logs)
{
    Console.WriteLine(log);
}

Logs.AssertContains(logs, $"AssemblyLoadFinished - TestApp - AppDomain clrhost - Module {typeof(Program).Assembly.Location}");
Logs.AssertContains(logs, $"AppDomainCreationStarted - System.Private.CoreLib.dll - Process Id {Environment.ProcessId}");
Logs.AssertContains(logs, $"AppDomainCreationStarted - DefaultDomain - Process Id {Environment.ProcessId}");
Logs.AssertContains(logs, "AppDomainCreationFinished - System.Private.CoreLib.dll - HResult S_OK");
Logs.AssertContains(logs, "AppDomainCreationFinished - DefaultDomain - HResult S_OK");


var threadId = (IntPtr)typeof(Thread).GetField("_DONT_USE_InternalThread", BindingFlags.Instance | BindingFlags.NonPublic)
    .GetValue(Thread.CurrentThread);

var osId = PInvokes.Win32.GetCurrentThreadId();

Logs.Assert(PInvokes.GetThreadId((ulong)threadId, (int)osId));


// Clear the logs before the next tests
Logs.Clear();

TestCreateAndUnloadAlc();

TestCreateAndUnloadType();

TestCom();

TestConditionalWeakTable();

TestDynamicMethod();

ExceptionTests.Run(threadId);


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

    var logs = Logs.Fetch().ToList();

    Logs.AssertContains(logs, $"DynamicMethodJITCompilationStarted - {handle:x2}");
    Logs.AssertContains(logs, $"DynamicMethodJITCompilationFinished - {handle:x2}");
    Logs.AssertContains(logs, $"DynamicMethodUnloaded - {handle:x2}");
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

    var logs = Logs.Fetch().ToList();
    Logs.AssertContains(logs, "ConditionalWeakTableElementReferences - hello -> world");
}

static void TestCom()
{
    _ = Marshal.GetIUnknownForObject(new object());

    var logs = Logs.Fetch().ToList();
    Logs.AssertContains(logs, "COMClassicVTableCreated - System.Object - fbec27f0-fc44-396c-8a8c-4c1993516f2d - 11");
}

static void TestCreateAndUnloadType()
{
    Console.WriteLine("**** Try loading/unloading type");

    CreateAndUnloadType();

    GC.Collect(2, GCCollectionMode.Forced, true);
    GC.WaitForPendingFinalizers();
    GC.Collect(2, GCCollectionMode.Forced, true);

    var logs = Logs.Fetch().ToList();

    Logs.AssertContains(logs, "ClassLoadFinished - DynamicType");
    Logs.AssertContains(logs, "ClassUnloadStarted - DynamicType");
    Logs.AssertContains(logs, "ClassUnloadFinished - DynamicType");
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

    var logs = Logs.Fetch().ToList();

    foreach (var log in logs)
    {
        Console.WriteLine(log);
    }

    Logs.AssertContains(logs, $"AssemblyUnloadFinished - TestApp - AppDomain clrhost - Module {typeof(Program).Assembly.Location}");
}

[MethodImpl(MethodImplOptions.NoInlining)]
static void CreateAndUnloadAlc()
{
    var alc = new TestAssemblyLoadContext();
    _ = alc.LoadFromAssemblyPath(typeof(Program).Assembly.Location);
    alc.Unload();
}

// Dump last logs before exiting
foreach (var log in Logs.Fetch())
{
    Console.WriteLine(log);
}

static void ThrowException()
{
    throw new Exception("Test exception");
}


public class TestCom
{
    public int Method() => 42;
}