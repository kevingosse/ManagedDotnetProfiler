using System.ComponentModel;

namespace ProfilerLib
{
    public static class Extensions
    {
        public static T ThrowIfFailed<T>(this (HResult, T) value)
        {
            if (!value.Item1.IsOK)
            {
                throw new Win32Exception(value.Item1, $"The HResult is failed: {value.Item1}");
            }

            return value.Item2;
        }

        public static (T1, T2) ThrowIfFailed<T1, T2>(this (HResult, T1, T2) value)
        {
            if (!value.Item1.IsOK)
            {
                throw new Win32Exception(value.Item1, $"The HResult is failed: {value.Item1}");
            }

            return (value.Item2, value.Item3);
        }

        public static (T1, T2, T3) ThrowIfFailed<T1, T2, T3>(this (HResult, T1, T2, T3) value)
        {
            if (!value.Item1.IsOK)
            {
                throw new Win32Exception(value.Item1, $"The HResult is failed: {value.Item1}");
            }

            return (value.Item2, value.Item3, value.Item4);
        }

        internal static string WithoutNullTerminator(this Span<char> buffer)
        {
            if (buffer.Length == 0)
            {
                return string.Empty;
            }

            if (buffer[^1] == '\0')
            {
                return new string(buffer[..^1]);
            }

            return new string(buffer);
        }
    }
}
