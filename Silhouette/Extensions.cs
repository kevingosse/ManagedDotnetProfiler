namespace Silhouette;

public static class Extensions
{
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
