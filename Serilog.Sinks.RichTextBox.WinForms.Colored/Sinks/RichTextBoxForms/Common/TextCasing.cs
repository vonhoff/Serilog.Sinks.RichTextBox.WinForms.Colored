namespace Serilog.Sinks.RichTextBoxForms.Common
{
    public static class TextCasing
    {
        /// <summary>
        /// Applies simple casing rules recognised by the sink (u = upper, w = lower, t = title).
        /// For any unknown or empty format this simply returns the input.
        /// </summary>
        public static string Format(string value, string? format)
        {
            if (string.IsNullOrEmpty(format) || string.IsNullOrEmpty(value))
            {
                return value;
            }

            var first = format![0];
            return first switch
            {
                'u' => value.ToUpperInvariant(),
                'w' => value.ToLowerInvariant(),
                't' => char.ToUpperInvariant(value[0]) + value.Substring(1).ToLowerInvariant(),
                _ => value
            };
        }
    }
}