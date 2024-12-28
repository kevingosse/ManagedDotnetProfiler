using System.Runtime.CompilerServices;

namespace TestApp;

internal class ConditionalWeakTableTests
{
    public static void Run()
    {
        _ = new ConditionalWeakTable<string, string> { { "hello", "world" } };
        GC.Collect(2, GCCollectionMode.Forced, true);

        var logs = Logs.Fetch().ToList();
        Logs.AssertContains(logs, "ConditionalWeakTableElementReferences - hello -> world");
    }
}