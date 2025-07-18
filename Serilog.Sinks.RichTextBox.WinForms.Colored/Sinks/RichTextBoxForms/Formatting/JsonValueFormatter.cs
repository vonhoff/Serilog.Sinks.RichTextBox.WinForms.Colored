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
        private readonly StringBuilder _literalBuilder = new(64);
        private readonly StringBuilder _scalarBuilder = new();
        private readonly StringBuilder _jsonStringBuilder = new();

        public JsonValueFormatter(Theme theme, IFormatProvider? formatProvider) : base(theme, formatProvider)
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
                    _literalBuilder.Clear();

                    using (var writer = new StringWriter(_literalBuilder))
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

                    Theme.Render(canvas, StyleToken.Scalar, GetQuotedJsonString(_literalBuilder.ToString()));
                    return;

                default:
                    if (value is IFormattable formattable)
                    {
                        RenderFormattable(canvas, formattable, null);
                        return;
                    }

                    _scalarBuilder.Clear();

                    using (var writer = new StringWriter(_scalarBuilder))
                    {
                        scalar.Render(writer, null, _formatProvider);
                    }

                    Theme.Render(canvas, StyleToken.Scalar, _scalarBuilder.ToString());
                    return;
            }
        }

        private static void WriteQuotedJsonString(StringBuilder builder, string str)
        {
            builder.Append('\"');

            foreach (var c in str)
            {
                switch (c)
                {
                    case '"':
                        builder.Append("\\\"");
                        break;

                    case '\\':
                        builder.Append(@"\\");
                        break;

                    case '\n':
                        builder.Append("\\n");
                        break;

                    case '\r':
                        builder.Append("\\r");
                        break;

                    case '\f':
                        builder.Append("\\f");
                        break;

                    case '\t':
                        builder.Append("\\t");
                        break;

                    case < (char)32:
                        builder.Append("\\u");
                        builder.Append(((int)c).ToString("X4"));
                        break;

                    default:
                        builder.Append(c);
                        break;
                }
            }
            builder.Append('\"');
        }

        private string GetQuotedJsonString(string str)
        {
            _jsonStringBuilder.Clear();
            WriteQuotedJsonString(_jsonStringBuilder, str);
            return _jsonStringBuilder.ToString();
        }
    }
}