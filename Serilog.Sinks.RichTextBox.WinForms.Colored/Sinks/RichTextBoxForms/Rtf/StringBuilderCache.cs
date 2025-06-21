using System;
using System.Text;

namespace Serilog.Sinks.RichTextBoxForms.Rtf
{
    internal static class StringBuilderCache
    {
        private const int MaxBuilderSize = 32 * 1024;

        [ThreadStatic]
        private static StringBuilder? _cachedInstance;

        public static StringBuilder Acquire(int capacity = 256)
        {
            if (capacity <= MaxBuilderSize)
            {
                var sb = _cachedInstance;
                if (sb != null && sb.Capacity >= capacity)
                {
                    sb.Clear();
                    return sb;
                }
            }

            return new StringBuilder(capacity);
        }

        public static void Release(StringBuilder sb)
        {
            if (sb.Capacity > MaxBuilderSize)
            {
                return;
            }

            _cachedInstance = sb;
        }

        public static string GetStringAndRelease(StringBuilder sb)
        {
            var result = sb.ToString();
            Release(sb);
            return result;
        }
    }
}