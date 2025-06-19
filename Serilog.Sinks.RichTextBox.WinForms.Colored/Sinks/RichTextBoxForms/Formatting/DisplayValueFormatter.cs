#region Copyright 2022 Simon Vonhoff & Contributors

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
using System.IO;

namespace Serilog.Sinks.RichTextBoxForms.Formatting
{
    public class DisplayValueFormatter : ValueFormatter
    {
        private readonly IFormatProvider? _formatProvider;

        public DisplayValueFormatter(Theme theme, IFormatProvider? formatProvider) : base(theme)
        {
            _formatProvider = formatProvider;
        }

        public void FormatLiteralValue(ScalarValue scalar, IRtfCanvas canvas, string? format, bool isLiteral)
        {
            var value = scalar.Value;

            switch (value)
            {
                case null:
                    Theme.Render(canvas, StyleToken.Null, "null");
                    return;
                case string text:
                    bool effectivelyLiteral = isLiteral || (format != null && format.Contains("l"));
                    if (effectivelyLiteral)
                    {
                        Theme.Render(canvas, StyleToken.String, text);
                    }
                    else
                    {
                        Theme.Render(canvas, StyleToken.String, $"\"{text.Replace("\"", "\\\"")}\"");
                    }
                    return;
                case byte[] bytes:
                    Theme.Render(canvas, StyleToken.String, $"\"{Convert.ToBase64String(bytes)}\"");
                    return;
                case bool b:
                    Theme.Render(canvas, StyleToken.Boolean, b.ToString());
                    return;
                case char ch:
                    Theme.Render(canvas, StyleToken.Scalar, ch.ToString());
                    return;
                case int i:
                    Theme.Render(canvas, StyleToken.Number, i.ToString(_formatProvider));
                    return;
                case uint ui:
                    Theme.Render(canvas, StyleToken.Number, ui.ToString(_formatProvider));
                    return;
                case long l:
                    Theme.Render(canvas, StyleToken.Number, l.ToString(_formatProvider));
                    return;
                case ulong ul:
                    Theme.Render(canvas, StyleToken.Number, ul.ToString(_formatProvider));
                    return;
                case decimal dec:
                    Theme.Render(canvas, StyleToken.Number, dec.ToString(_formatProvider));
                    return;
                case byte bValue:
                    Theme.Render(canvas, StyleToken.Number, bValue.ToString(_formatProvider));
                    return;
                case sbyte sb:
                    Theme.Render(canvas, StyleToken.Number, sb.ToString(_formatProvider));
                    return;
                case short s:
                    Theme.Render(canvas, StyleToken.Number, s.ToString(_formatProvider));
                    return;
                case ushort us:
                    Theme.Render(canvas, StyleToken.Number, us.ToString(_formatProvider));
                    return;
                case float f:
                    Theme.Render(canvas, StyleToken.Number, f.ToString(_formatProvider));
                    return;
                case double d:
                    Theme.Render(canvas, StyleToken.Number, d.ToString(_formatProvider));
                    return;
                case DateTime dt:
                    Theme.Render(canvas, StyleToken.Scalar, dt.ToString(format, _formatProvider));
                    return;
                case DateTimeOffset dto:
                    Theme.Render(canvas, StyleToken.Scalar, dto.ToString(format, _formatProvider));
                    return;
                case TimeSpan ts:
                    Theme.Render(canvas, StyleToken.Scalar, ts.ToString(string.IsNullOrEmpty(format) ? "c" : format, _formatProvider));
                    return;
                case Guid guid:
                    Theme.Render(canvas, StyleToken.Scalar, guid.ToString(string.IsNullOrEmpty(format) ? "D" : format, _formatProvider));
                    return;
                case Uri uri:
                    Theme.Render(canvas, StyleToken.Scalar, uri.ToString());
                    return;
            }

            var writer = new StringWriter();
            scalar.Render(writer, null, _formatProvider);
            Theme.Render(canvas, StyleToken.Scalar, writer.ToString());
        }

        // Backwards-compat overload
        public void FormatLiteralValue(ScalarValue scalar, System.Windows.Forms.RichTextBox richTextBox, string? format, bool isLiteral)
        {
            var adapter = new Serilog.Sinks.RichTextBoxForms.Rtf.RichTextBoxCanvasAdapter(richTextBox);
            FormatLiteralValue(scalar, adapter, format, isLiteral);
        }

        protected override bool VisitDictionaryValue(ValueFormatterState state, DictionaryValue dictionary)
        {
            if (state.Format != null && state.Format.Contains("j"))
            {
                var jsonFormatter = new JsonValueFormatter(Theme, _formatProvider);
                jsonFormatter.Format(dictionary, state.Canvas, state.Format, state.IsLiteral);
                return true;
            }

            Theme.Render(state.Canvas, StyleToken.TertiaryText, "{");

            var delimiter = string.Empty;
            foreach (var (scalarValue, propertyValue) in dictionary.Elements)
            {
                if (!string.IsNullOrEmpty(delimiter))
                {
                    Theme.Render(state.Canvas, StyleToken.TertiaryText, delimiter);
                }

                delimiter = ", ";

                Theme.Render(state.Canvas, StyleToken.TertiaryText, "[");
                Visit(state.Next(), scalarValue);
                Theme.Render(state.Canvas, StyleToken.TertiaryText, "]=");
                Visit(state.Next(), propertyValue);
            }

            Theme.Render(state.Canvas, StyleToken.TertiaryText, "}");
            return true;
        }

        protected override bool VisitScalarValue(ValueFormatterState state, ScalarValue scalar)
        {
            if (scalar is null)
            {
                throw new ArgumentNullException(nameof(scalar));
            }

            FormatLiteralValue(scalar, state.Canvas, state.Format, state.IsLiteral);
            return true;
        }

        protected override bool VisitSequenceValue(ValueFormatterState state, SequenceValue sequence)
        {
            if (sequence is null)
            {
                throw new ArgumentNullException(nameof(sequence));
            }

            if (state.Format != null && state.Format.Contains("j"))
            {
                var jsonFormatter = new JsonValueFormatter(Theme, _formatProvider);
                jsonFormatter.Format(sequence, state.Canvas, state.Format, state.IsLiteral);
                return true;
            }

            Theme.Render(state.Canvas, StyleToken.TertiaryText, "[");

            var delimiter = string.Empty;
            foreach (var propertyValue in sequence.Elements)
            {
                if (!string.IsNullOrEmpty(delimiter))
                {
                    Theme.Render(state.Canvas, StyleToken.TertiaryText, delimiter);
                }

                delimiter = ", ";
                Visit(state.Next(), propertyValue);
            }

            Theme.Render(state.Canvas, StyleToken.TertiaryText, "]");
            return true;
        }

        protected override bool VisitStructureValue(ValueFormatterState state, StructureValue structure)
        {
            if (state.Format != null && state.Format.Contains("j"))
            {
                var jsonFormatter = new JsonValueFormatter(Theme, _formatProvider);
                jsonFormatter.Format(structure, state.Canvas, state.Format, state.IsLiteral);
                return true;
            }

            if (structure.TypeTag != null)
            {
                Theme.Render(state.Canvas, StyleToken.Name, structure.TypeTag + " ");
            }

            Theme.Render(state.Canvas, StyleToken.TertiaryText, "{");

            var delimiter = string.Empty;
            foreach (var eventProperty in structure.Properties)
            {
                if (!string.IsNullOrEmpty(delimiter))
                {
                    Theme.Render(state.Canvas, StyleToken.TertiaryText, delimiter);
                }

                delimiter = ", ";

                Theme.Render(state.Canvas, StyleToken.Name, eventProperty.Name);
                Theme.Render(state.Canvas, StyleToken.TertiaryText, "=");
                Visit(state.Next(), eventProperty.Value);
            }

            Theme.Render(state.Canvas, StyleToken.TertiaryText, "}");
            return true;
        }
    }
}