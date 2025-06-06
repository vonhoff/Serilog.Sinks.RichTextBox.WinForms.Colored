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
using System.Globalization;
using System.IO;
using System.Windows.Forms;

namespace Serilog.Sinks.RichTextBoxForms.Formatting
{
    internal class JsonValueFormatter : ValueFormatter
    {
        private readonly DisplayValueFormatter _displayFormatter;

        public JsonValueFormatter(Theme theme, IFormatProvider? formatProvider) : base(theme)
        {
            _displayFormatter = new DisplayValueFormatter(theme, formatProvider);
        }

        protected override bool VisitScalarValue(ValueFormatterState state, ScalarValue scalar)
        {
            if (scalar is null)
            {
                throw new ArgumentNullException(nameof(scalar));
            }

            FormatLiteralValue(scalar, state.RichTextBox);
            return true;
        }

        protected override bool VisitSequenceValue(ValueFormatterState state, SequenceValue sequence)
        {
            if (sequence is null)
            {
                throw new ArgumentNullException(nameof(sequence));
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
                Visit(state, propertyValue);
            }

            Theme.Render(state.RichTextBox, StyleToken.TertiaryText, "]");
            return true;
        }

        protected override bool VisitStructureValue(ValueFormatterState state, StructureValue structure)
        {
            Theme.Render(state.RichTextBox, StyleToken.TertiaryText, "{");

            var delimiter = string.Empty;
            foreach (var eventProperty in structure.Properties)
            {
                if (!string.IsNullOrEmpty(delimiter))
                {
                    Theme.Render(state.RichTextBox, StyleToken.TertiaryText, delimiter);
                }

                delimiter = ", ";

                Theme.Render(state.RichTextBox, StyleToken.Name, GetQuotedJsonString(eventProperty.Name));
                Theme.Render(state.RichTextBox, StyleToken.TertiaryText, ": ");
                Visit(state.Next(), eventProperty.Value);
            }

            if (structure.TypeTag != null)
            {
                Theme.Render(state.RichTextBox, StyleToken.TertiaryText, delimiter);
                Theme.Render(state.RichTextBox, StyleToken.Name, GetQuotedJsonString("$type"));
                Theme.Render(state.RichTextBox, StyleToken.TertiaryText, ": ");
                Theme.Render(state.RichTextBox, StyleToken.String, GetQuotedJsonString(structure.TypeTag));
            }

            Theme.Render(state.RichTextBox, StyleToken.TertiaryText, "}");
            return true;
        }

        protected override bool VisitDictionaryValue(ValueFormatterState state, DictionaryValue dictionary)
        {
            Theme.Render(state.RichTextBox, StyleToken.TertiaryText, "{");

            var delimiter = string.Empty;
            foreach (var (scalar, propertyValue) in dictionary.Elements)
            {
                if (!string.IsNullOrEmpty(delimiter))
                {
                    Theme.Render(state.RichTextBox, StyleToken.TertiaryText, delimiter);
                }

                delimiter = ", ";

                var style = scalar.Value switch
                {
                    null => StyleToken.Null,
                    string => StyleToken.String,
                    _ => StyleToken.Scalar
                };

                Theme.Render(state.RichTextBox, style, GetQuotedJsonString(scalar.Value?.ToString() ?? "null"));
                Theme.Render(state.RichTextBox, StyleToken.TertiaryText, ": ");

                Visit(state.Next(), propertyValue);
            }

            Theme.Render(state.RichTextBox, StyleToken.TertiaryText, "}");
            return true;
        }

        private void FormatLiteralValue(ScalarValue scalar, RichTextBox richTextBox)
        {
            var value = scalar.Value;

            switch (value)
            {
                case null:
                    {
                        Theme.Render(richTextBox, StyleToken.Null, "null");
                        return;
                    }
                case string str:
                    {
                        Theme.Render(richTextBox, StyleToken.String, GetQuotedJsonString(str));
                        return;
                    }
                case byte[] bytes:
                    {
                        Theme.Render(richTextBox, StyleToken.String, GetQuotedJsonString(Convert.ToBase64String(bytes)));
                        return;
                    }
                case bool b:
                    {
                        Theme.Render(richTextBox, StyleToken.Boolean, b ? "true" : "false");
                        return;
                    }
                case ValueType and (DateTime or DateTimeOffset):
                    {
                        _displayFormatter.FormatLiteralValue(scalar, richTextBox, "O", false);
                        return;
                    }
                default:
                    {
                        _displayFormatter.FormatLiteralValue(scalar, richTextBox, null, false);
                        return;
                    }
            }
        }

        /// <summary>
        ///     Write a valid JSON string literal, escaping as necessary.
        /// </summary>
        /// <param name="str">The string value to write.</param>
        public static string GetQuotedJsonString(string str)
        {
            var output = new StringWriter();
            output.Write('\"');

            for (var i = 0; i < str.Length; ++i)
            {
                var c = str[i];
                switch (c)
                {
                    case '"':
                        output.Write("\\\"");
                        break;
                    case '\\':
                        output.Write("\\\\");
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
            return output.ToString();
        }
    }
}