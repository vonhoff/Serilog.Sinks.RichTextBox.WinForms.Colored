using System;
using System.Text;

namespace Serilog.Sinks.RichTextBoxForms.Rtf
{
    internal static class StringBuilderCache
    {
        // Max char capacity we are willing to cache (32 KB = 64 kB on the LOH threshold).
        private const int MaxBuilderSize = 32 * 1024;

        [ThreadStatic]
        private static StringBuilder? _cachedInstance;

        /// <summary>
        /// Retrieves a cached <see cref="StringBuilder"/> if one is available and large enough;
        /// otherwise allocates a new instance.
        /// </summary>
        public static StringBuilder Acquire(int capacity)
        {
            if (capacity <= MaxBuilderSize)
            {
                var sb = _cachedInstance;
                if (sb != null && sb.Capacity >= capacity)
                {
                    _cachedInstance = null;
                    sb.Clear();
                    return sb;
                }
            }

            return new StringBuilder(capacity);
        }

        /// <summary>
        /// Returns a <see cref="StringBuilder"/> to the cache. Call this only with builders
        /// obtained from <see cref="Acquire"/>.
        /// </summary>
        public static void Release(StringBuilder sb)
        {
            if (sb.Capacity > MaxBuilderSize)
            {
                // Too big – don't cache.
                return;
            }

            _cachedInstance = sb;
        }
    }
}