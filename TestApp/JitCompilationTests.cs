using System.Reflection;
using System.Runtime.CompilerServices;

namespace TestApp;

internal class JitCompilationTests
{
    public static void Run()
    {
        // var method = typeof(JitCompilationTests).GetMethod(nameof(PrivateMethod), BindingFlags.Static | BindingFlags.NonPublic);
        // RuntimeHelpers.PrepareMethod(method!.MethodHandle);

        var test = new JitCompilationTests();

        for (int i = 0; i < 50; i++)
        {
            _ = test.PrivateMethod();
        }

        var logs = Logs.Fetch().ToList();

        Logs.AssertContains(logs, "JITCompilationStarted - TestApp.JitCompilationTests.PrivateMethod");
        Logs.AssertContains(logs, "JITCompilationFinished - TestApp.JitCompilationTests.PrivateMethod");
        Logs.AssertContains(logs, "JITInlining - TestApp.JitCompilationTests.InnerMethod -> TestApp.JitCompilationTests.PrivateMethod");
    }

    [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.AggressiveOptimization)]
    public int PrivateMethod()
    {
        if (InnerMethod() == 1)
        {
            return 2;
        }

        return 0;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    private int InnerMethod() => 1;
}
