namespace Serilog.Sinks.RichTextBoxForms.Formatting
{
    public static class TextFormatter
    {
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