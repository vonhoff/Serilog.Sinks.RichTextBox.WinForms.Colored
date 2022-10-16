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

using System;
using System.IO;
using System.Windows.Forms;
using Serilog.Events;
using Serilog.Sinks.RichTextBoxForms.Themes;

namespace Serilog.Sinks.RichTextBoxForms.Formatting
{
    internal class DisplayValueFormatter : ValueFormatter
    {
        private readonly IFormatProvider? _formatProvider;

        public DisplayValueFormatter(Theme theme, IFormatProvider? formatProvider) : base(theme)
        {
            _formatProvider = formatProvider;
        }

        public void FormatLiteralValue(ScalarValue scalar, RichTextBox richTextBox, string format)
        {
            var value = scalar.Value;

            switch (value)
            {
                case null:
                {
                    Theme.Render(richTextBox, StyleToken.Null, "null");
                    break;
                }

                case string text:
                {
                    Theme.Render(richTextBox, StyleToken.String, text);
                    return;
                }
            }

            var writer = new StringWriter();
            scalar.Render(writer, format, _formatProvider);

            if (value is ValueType)
            {
                switch (value)
                {
                    case int:
                    case uint:
                    case long:
                    case ulong:
                    case decimal:
                    case byte:
                    case sbyte:
                    case short:
                    case ushort:
                    case float:
                    case double:
                    {
                        Theme.Render(richTextBox, StyleToken.Number, writer.ToString());
                        return;
                    }

                    case bool b:
                    {
                        Theme.Render(richTextBox, StyleToken.Boolean, b.ToString());
                        return;
                    }

                    case char ch:
                    {
                        Theme.Render(richTextBox, StyleToken.Scalar, ch.ToString());
                        return;
                    }
                }
            }

            Theme.Render(richTextBox, StyleToken.Scalar, writer.ToString());
        }

        protected override bool VisitDictionaryValue(ValueFormatterState state, DictionaryValue dictionary)
        {
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

            FormatLiteralValue(scalar, state.RichTextBox, state.Format);
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