using System;
using System.Text;

namespace Serilog.Sinks.RichTextBoxForms.Rtf
{
    /// <summary>
    /// Minimal clone of the internal BCL StringBuilderCache.
    /// Keeps a single <see cref="StringBuilder"/> per thread under
    /// a configurable size, so we can reuse the buffer between RTF documents
    /// and avoid repeated LOH allocations.
    /// </summary>
    internal static class StringBuilderCache
    {
        // Builders with a capacity above this value are not cached because
        // they are likely to be very large and would bloat the working set.
        private const int MaxBuilderSize = 32 * 1024; // 32 KB => 64 KB char buffer

        [ThreadStatic]
        private static StringBuilder? _cachedInstance;

        /// <summary>
        /// Get a <see cref="StringBuilder"/> from the cache or allocate a new one.
        /// </summary>
        /// <param name="capacity">Minimum required capacity.</param>
        public static StringBuilder Acquire(int capacity = 16)
        {
            // We only cache reasonably-sized builders.
            if (capacity <= MaxBuilderSize)
            {
                var sb = _cachedInstance;
                if (sb != null && capacity <= sb.Capacity)
                {
                    _cachedInstance = null;
                    sb.Clear();
                    return sb;
                }
            }

            return new StringBuilder(capacity);
        }

        /// <summary>
        /// Convert the builder to a string and release it back to the cache.
        /// </summary>
        public static string GetStringAndRelease(StringBuilder sb)
        {
            var result = sb.ToString();

            if (sb.Capacity <= MaxBuilderSize)
            {
                _cachedInstance = sb;
            }

            return result;
        }
    }
}