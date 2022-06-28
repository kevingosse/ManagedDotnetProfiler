namespace ManagedDotnetProfiler;

internal record struct StackFrame(string Module, string Ns, string Type, string Function)
{
}