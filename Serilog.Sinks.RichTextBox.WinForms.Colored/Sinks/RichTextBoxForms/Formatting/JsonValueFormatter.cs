#region Copyright 2025 Simon Vonhoff & Contributors

//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//

#endregion

using Serilog.Events;
using Serilog.Sinks.RichTextBoxForms.Extensions;
using Serilog.Sinks.RichTextBoxForms.Rtf;
using Serilog.Sinks.RichTextBoxForms.Themes;
using System;
using System.Globalization;
using System.IO;
using System.Text;

namespace Serilog.Sinks.RichTextBoxForms.Formatting
{
    public class JsonValueFormatter : ValueFormatter
    {
        private readonly IFormatProvider? _formatProvider;

        public JsonValueFormatter(Theme theme, IFormatProvider? formatProvider) : base(theme)
        {
            _formatProvider = formatProvider;
        }

        protected override bool VisitScalarValue(ValueFormatterState state, ScalarValue scalar)
        {
            FormatLiteralValue(scalar, state.Canvas);
            return true;
        }

        protected override bool VisitSequenceValue(ValueFormatterState state, SequenceValue sequence)
        {
            Theme.Render(state.Canvas, StyleToken.TertiaryText, "[");

            var delimiter = string.Empty;
            foreach (var propertyValue in sequence.Elements)
            {
                if (!string.IsNullOrEmpty(delimiter))
                {
                    Theme.Render(state.Canvas, StyleToken.TertiaryText, delimiter);
                }

                delimiter = ", ";
                Visit(state, propertyValue);
            }

            Theme.Render(state.Canvas, StyleToken.TertiaryText, "]");
            return true;
        }

        protected override bool VisitStructureValue(ValueFormatterState state, StructureValue structure)
        {
            Theme.Render(state.Canvas, StyleToken.TertiaryText, "{");

            var delimiter = string.Empty;
            foreach (var eventProperty in structure.Properties)
            {
                if (!string.IsNullOrEmpty(delimiter))
                {
                    Theme.Render(state.Canvas, StyleToken.TertiaryText, delimiter);
                }

                delimiter = ", ";

                Theme.Render(state.Canvas, StyleToken.Name, GetQuotedJsonString(eventProperty.Name));
                Theme.Render(state.Canvas, StyleToken.TertiaryText, ": ");
                Visit(state.Next(), eventProperty.Value);
            }

            if (structure.TypeTag != null)
            {
                Theme.Render(state.Canvas, StyleToken.TertiaryText, delimiter);
                Theme.Render(state.Canvas, StyleToken.Name, GetQuotedJsonString("$type"));
                Theme.Render(state.Canvas, StyleToken.TertiaryText, ": ");
                Theme.Render(state.Canvas, StyleToken.String, GetQuotedJsonString(structure.TypeTag));
            }

            Theme.Render(state.Canvas, StyleToken.TertiaryText, "}");
            return true;
        }

        protected override bool VisitDictionaryValue(ValueFormatterState state, DictionaryValue dictionary)
        {
            Theme.Render(state.Canvas, StyleToken.TertiaryText, "{");

            var delimiter = string.Empty;
            foreach (var (scalar, propertyValue) in dictionary.Elements)
            {
                if (!string.IsNullOrEmpty(delimiter))
                {
                    Theme.Render(state.Canvas, StyleToken.TertiaryText, delimiter);
                }

                delimiter = ", ";

                var style = scalar.Value switch
                {
                    null => StyleToken.Null,
                    string => StyleToken.String,
                    _ => StyleToken.Scalar
                };

                Theme.Render(state.Canvas, style, GetQuotedJsonString(scalar.Value?.ToString() ?? "null"));
                Theme.Render(state.Canvas, StyleToken.TertiaryText, ": ");

                Visit(state.Next(), propertyValue);
            }

            Theme.Render(state.Canvas, StyleToken.TertiaryText, "}");
            return true;
        }

        private void FormatLiteralValue(ScalarValue scalar, IRtfCanvas canvas)
        {
            var value = scalar.Value;

            switch (value)
            {
                case null:
                    Theme.Render(canvas, StyleToken.Null, "null");
                    return;
                case string str:
                    Theme.Render(canvas, StyleToken.String, GetQuotedJsonString(str));
                    return;
                case byte[] bytes:
                    Theme.Render(canvas, StyleToken.String, GetQuotedJsonString(Convert.ToBase64String(bytes)));
                    return;
                case bool b:
                    Theme.Render(canvas, StyleToken.Boolean, b ? "true" : "false");
                    return;
                case double d:
                    if (double.IsNaN(d) || double.IsInfinity(d))
                    {
                        Theme.Render(canvas, StyleToken.Number, GetQuotedJsonString(d.ToString(CultureInfo.InvariantCulture)));
                    }
                    else
                    {
                        Theme.Render(canvas, StyleToken.Number, d.ToString("R", CultureInfo.InvariantCulture));
                    }
                    return;
                case float f:
                    if (float.IsNaN(f) || float.IsInfinity(f))
                    {
                        Theme.Render(canvas, StyleToken.Number, GetQuotedJsonString(f.ToString(CultureInfo.InvariantCulture)));
                    }
                    else
                    {
                        Theme.Render(canvas, StyleToken.Number, f.ToString("R", CultureInfo.InvariantCulture));
                    }
                    return;
                case char:
                case DateTime:
                case DateTimeOffset:
                case TimeSpan:
                case Guid:
                case Uri:
                    {
                        var sb = new StringBuilder(64);

                        using (var writer = new StringWriter(sb))
                        {
                            switch (value)
                            {
                                // For dates in JSON, always use ISO 8601 format (O)
                                case DateTime dt:
                                    writer.Write(dt.ToString("O", CultureInfo.InvariantCulture));
                                    break;
                                case DateTimeOffset dto:
                                    writer.Write(dto.ToString("O", CultureInfo.InvariantCulture));
                                    break;
                                default:
                                    scalar.Render(writer, null, _formatProvider);
                                    break;
                            }
                        }

                        Theme.Render(canvas, StyleToken.Scalar, GetQuotedJsonString(sb.ToString()));
                        return;
                    }
                default:
                    {
                        if (value is IFormattable formattable)
                        {
                            RenderFormattable(canvas, formattable, null);
                            return;
                        }

                        var sb = new StringBuilder();

                        using (var writer = new StringWriter(sb))
                        {
                            scalar.Render(writer, null, _formatProvider);
                        }

                        Theme.Render(canvas, StyleToken.Scalar, sb.ToString());
                        return;
                    }
            }
        }

        private void RenderFormattable(IRtfCanvas canvas, IFormattable formattable, string? format)
        {
            // Use Number style for numbers, Scalar for others (DateTime, Guid, etc.)
            var type = formattable.GetType();
            var token =
                type == typeof(float) || type == typeof(double) || type == typeof(decimal) ||
                type == typeof(int) || type == typeof(uint) || type == typeof(long) || type == typeof(ulong) ||
                type == typeof(byte) || type == typeof(sbyte) || type == typeof(short) || type == typeof(ushort)
                ? StyleToken.Number : StyleToken.Scalar;

            var effectiveFormat = format;

            // Remove sink-specific format specifiers (e.g. "l" for literal, "j" for JSON) that are not
            // recognised by the underlying IFormattable implementation and would otherwise trigger
            // a FormatException.
            if (!string.IsNullOrEmpty(effectiveFormat))
            {
                effectiveFormat = effectiveFormat!.Replace("l", string.Empty).Replace("j", string.Empty);

                // If all characters were removed we end up with an empty string – treat this as no format.
                if (string.IsNullOrWhiteSpace(effectiveFormat))
                {
                    effectiveFormat = null;
                }
            }

            if (type == typeof(TimeSpan) && string.IsNullOrEmpty(effectiveFormat))
                effectiveFormat = "c";
            if (type == typeof(Guid) && string.IsNullOrEmpty(effectiveFormat))
                effectiveFormat = "D";

            string renderedValue;
            try
            {
                renderedValue = formattable.ToString(effectiveFormat, _formatProvider);
            }
            catch (FormatException)
            {
                // Fall back to the default formatting if the specified format is not supported by the value.
                renderedValue = formattable.ToString(null, _formatProvider);
            }

            Theme.Render(canvas, token, renderedValue);
        }

        private static void WriteQuotedJsonString(string str, TextWriter output)
        {
            output.Write('\"');

            foreach (var c in str)
            {
                switch (c)
                {
                    case '"':
                        output.Write("\\\"");
                        break;
                    case '\\':
                        output.Write(@"\\");
                        break;
                    case '\n':
                        output.Write("\\n");
                        break;
                    case '\r':
                        output.Write("\\r");
                        break;
                    case '\f':
                        output.Write("\\f");
                        break;
                    case '\t':
                        output.Write("\\t");
                        break;
                    case < (char)32:
                        output.Write("\\u");
                        output.Write(((int)c).ToString("X4"));
                        break;
                    default:
                        output.Write(c);
                        break;
                }
            }
            output.Write('\"');
        }

        private static string GetQuotedJsonString(string str)
        {
            var sb = new StringBuilder(str.Length + 2);
            using (var writer = new StringWriter(sb))
            {
                WriteQuotedJsonString(str, writer);
            }

            return sb.ToString();
        }
    }
}