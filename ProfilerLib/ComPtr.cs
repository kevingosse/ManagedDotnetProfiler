﻿namespace ProfilerLib;

public static class ComPtr
{
    public static ComPtr<T> Create<T>(T value)
        where T : Interfaces.IUnknown
    {
        return new ComPtr<T>(value);
    }

    public static ComPtr<T> Wrap<T>(this T value)
        where T : Interfaces.IUnknown
    {
        return new ComPtr<T>(value);
    }
}

public class ComPtr<T> : IDisposable
    where T : Interfaces.IUnknown
{
    public ComPtr(T value)
    {
        Value = value;
    }

    public T Value { get; }

    public ComPtr<T> Copy()
    {
        Value?.AddRef();
        return new(Value);
    }

    public void Dispose()
    {
        Value?.Release();
    }
}
