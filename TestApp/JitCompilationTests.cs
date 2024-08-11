using System.Runtime.CompilerServices;

namespace TestApp;

internal class JitCompilationTests
{
    public static void Run()
    {
        _ = PrivateMethod();

        var logs = Logs.Fetch().ToList();

        Logs.AssertContains(logs, "JITCompilationStarted - TestApp.JitCompilationTests.PrivateMethod");
        Logs.AssertContains(logs, "JITCompilationFinished - TestApp.JitCompilationTests.PrivateMethod");
        Logs.AssertContains(logs, "JITInlining - TestApp.JitCompilationTests.InnerMethod -> TestApp.JitCompilationTests.PrivateMethod");
    }

    [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.AggressiveOptimization)]
    public static int PrivateMethod()
    {
        if (InnerMethod() == 1)
        {
            return 2;
        }

        return 0;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    private static int InnerMethod() => 1;
}
