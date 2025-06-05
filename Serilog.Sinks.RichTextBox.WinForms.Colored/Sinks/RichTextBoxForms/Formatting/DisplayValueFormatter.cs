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
using Serilog.Sinks.RichTextBoxForms.Themes;
using System;
using System.IO;
using System.Windows.Forms;

namespace Serilog.Sinks.RichTextBoxForms.Formatting
{
    internal class DisplayValueFormatter : ValueFormatter
    {
        private readonly IFormatProvider? _formatProvider;

        public DisplayValueFormatter(Theme theme, IFormatProvider? formatProvider) : base(theme)
        {
            _formatProvider = formatProvider;
        }

        public void FormatLiteralValue(ScalarValue scalar, RichTextBox richTextBox, string format, bool isLiteral)
        {
            var value = scalar.Value;

            switch (value)
            {
                case null:
                    Theme.Render(richTextBox, StyleToken.Null, "null");
                    return;
                case string text:
                    bool effectivelyLiteral = isLiteral || (format != null && format.Contains("l"));
                    if (effectivelyLiteral)
                    {
                        Theme.Render(richTextBox, StyleToken.String, text);
                    }
                    else
                    {
                        Theme.Render(richTextBox, StyleToken.String, $"\"{text.Replace("\"", "\\\"")}\"");
                    }
                    return;
                case byte[] bytes:
                    Theme.Render(richTextBox, StyleToken.String, $"\"{Convert.ToBase64String(bytes)}\"");
                    return;
                case bool b:
                    Theme.Render(richTextBox, StyleToken.Boolean, b.ToString());
                    return;
                case char ch:
                    Theme.Render(richTextBox, StyleToken.Scalar, ch.ToString());
                    return;
                case int i:
                    Theme.Render(richTextBox, StyleToken.Number, i.ToString(_formatProvider));
                    return;
                case uint ui:
                    Theme.Render(richTextBox, StyleToken.Number, ui.ToString(_formatProvider));
                    return;
                case long l:
                    Theme.Render(richTextBox, StyleToken.Number, l.ToString(_formatProvider));
                    return;
                case ulong ul:
                    Theme.Render(richTextBox, StyleToken.Number, ul.ToString(_formatProvider));
                    return;
                case decimal dec:
                    Theme.Render(richTextBox, StyleToken.Number, dec.ToString(_formatProvider));
                    return;
                case byte bValue:
                    Theme.Render(richTextBox, StyleToken.Number, bValue.ToString(_formatProvider));
                    return;
                case sbyte sb:
                    Theme.Render(richTextBox, StyleToken.Number, sb.ToString(_formatProvider));
                    return;
                case short s:
                    Theme.Render(richTextBox, StyleToken.Number, s.ToString(_formatProvider));
                    return;
                case ushort us:
                    Theme.Render(richTextBox, StyleToken.Number, us.ToString(_formatProvider));
                    return;
                case float f:
                    Theme.Render(richTextBox, StyleToken.Number, f.ToString(_formatProvider));
                    return;
                case double d:
                    Theme.Render(richTextBox, StyleToken.Number, d.ToString(_formatProvider));
                    return;
                case DateTime dt:
                    Theme.Render(richTextBox, StyleToken.Scalar, dt.ToString("O", _formatProvider));
                    return;
                case DateTimeOffset dto:
                    Theme.Render(richTextBox, StyleToken.Scalar, dto.ToString("O", _formatProvider));
                    return;
                case TimeSpan ts:
                    Theme.Render(richTextBox, StyleToken.Scalar, ts.ToString("c", _formatProvider));
                    return;
                case Guid guid:
                    Theme.Render(richTextBox, StyleToken.Scalar, guid.ToString("D", _formatProvider));
                    return;
                case Uri uri:
                    Theme.Render(richTextBox, StyleToken.Scalar, uri.ToString());
                    return;
            }

            var writer = new StringWriter();
            scalar.Render(writer, null, _formatProvider);
            Theme.Render(richTextBox, StyleToken.Scalar, writer.ToString());
        }

        protected override bool VisitDictionaryValue(ValueFormatterState state, DictionaryValue dictionary)
        {
            if (state.Format != null && state.Format.Contains("j"))
            {
                var jsonFormatter = new JsonValueFormatter(Theme, _formatProvider);
                jsonFormatter.Format(dictionary, state.RichTextBox, state.Format, state.IsLiteral);
                return true;
            }

            Theme.Render(state.RichTextBox, StyleToken.TertiaryText, "{");

            var delimiter = string.Empty;
            foreach (var (scalarValue, propertyValue) in dictionary.Elements)
            {
                if (!string.IsNullOrEmpty(delimiter))
                {
                    Theme.Render(state.RichTextBox, StyleToken.TertiaryText, delimiter);
                }

                delimiter = ", ";

                Theme.Render(state.RichTextBox, StyleToken.TertiaryText, "[");
                Visit(state.Next(), scalarValue);
                Theme.Render(state.RichTextBox, StyleToken.TertiaryText, "]=");
                Visit(state.Next(), propertyValue);
            }

            Theme.Render(state.RichTextBox, StyleToken.TertiaryText, "}");
            return true;
        }

        protected override bool VisitScalarValue(ValueFormatterState state, ScalarValue scalar)
        {
            if (scalar is null)
            {
                throw new ArgumentNullException(nameof(scalar));
            }

            FormatLiteralValue(scalar, state.RichTextBox, state.Format, state.IsLiteral);
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
                jsonFormatter.Format(sequence, state.RichTextBox, state.Format, state.IsLiteral);
                return true;
            }

            Theme.Render(state.RichTextBox, StyleToken.TertiaryText, "[");

            var delimiter = string.Empty;
            foreach (var propertyValue in sequence.Elements)
            {
                if (!string.IsNullOrEmpty(delimiter))
                {
                    Theme.Render(state.RichTextBox, StyleToken.TertiaryText, delimiter);
                }

                delimiter = ", ";
                Visit(state.Next(), propertyValue);
            }

            Theme.Render(state.RichTextBox, StyleToken.TertiaryText, "]");
            return true;
        }

        protected override bool VisitStructureValue(ValueFormatterState state, StructureValue structure)
        {
            if (state.Format != null && state.Format.Contains("j"))
            {
                var jsonFormatter = new JsonValueFormatter(Theme, _formatProvider);
                jsonFormatter.Format(structure, state.RichTextBox, state.Format, state.IsLiteral);
                return true;
            }

            if (structure.TypeTag != null)
            {
                Theme.Render(state.RichTextBox, StyleToken.Name, structure.TypeTag + " ");
            }

            Theme.Render(state.RichTextBox, StyleToken.TertiaryText, "{");

            var delimiter = string.Empty;
            foreach (var eventProperty in structure.Properties)
            {
                if (!string.IsNullOrEmpty(delimiter))
                {
                    Theme.Render(state.RichTextBox, StyleToken.TertiaryText, delimiter);
                }

                delimiter = ", ";

                Theme.Render(state.RichTextBox, StyleToken.Name, eventProperty.Name);
                Theme.Render(state.RichTextBox, StyleToken.TertiaryText, "=");
                Visit(state.Next(), eventProperty.Value);
            }

            Theme.Render(state.RichTextBox, StyleToken.TertiaryText, "}");
            return true;
        }
    }
}