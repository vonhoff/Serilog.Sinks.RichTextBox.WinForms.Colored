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
using System.IO;
using System.Text;

namespace Serilog.Sinks.RichTextBoxForms.Formatting
{
    public class DisplayValueFormatter : ValueFormatter
    {
        private readonly StringBuilder _scalarBuilder = new();
        private readonly StringBuilder _literalBuilder = new(64);
        private JsonValueFormatter? _jsonValueFormatter;

        public DisplayValueFormatter(RichTextBoxSinkOptions options) : base(options)
        {
        }

        private void FormatLiteralValue(ScalarValue scalar, IRtfCanvas canvas, string? format, bool isLiteral)
        {
            var value = scalar.Value;

            switch (value)
            {
                case null:
                    Options.Theme.Render(canvas, StyleToken.Null, "null");
                    return;

                case string text:
                    RenderString(text, canvas, format, isLiteral);
                    return;

                case byte[] bytes:
                    _literalBuilder.Clear();
                    _literalBuilder.Append('"').Append(Convert.ToBase64String(bytes)).Append('"');
                    Options.Theme.Render(canvas, StyleToken.String, _literalBuilder.ToString());
                    return;

                case bool b:
                    Options.Theme.Render(canvas, StyleToken.Boolean, b.ToString());
                    return;

                case Uri uri:
                    Options.Theme.Render(canvas, StyleToken.Scalar, uri.ToString());
                    return;

                case IFormattable formattable:
                    RenderFormattable(canvas, formattable, format);
                    return;
            }

            _scalarBuilder.Clear();

            using (var writer = new StringWriter(_scalarBuilder))
            {
                scalar.Render(writer, null, Options.FormatProvider);
            }

            Options.Theme.Render(canvas, StyleToken.Scalar, _scalarBuilder.ToString());
        }

        private void RenderString(string text, IRtfCanvas canvas, string? format, bool isLiteral)
        {
            var effectivelyLiteral = isLiteral || format != null && format.Contains("l");
            if (effectivelyLiteral)
            {
                Options.Theme.Render(canvas, StyleToken.String, text);
            }
            else
            {
                _literalBuilder.Clear();
                _literalBuilder.Append('"').Append(text.Replace("\"", "\\\"")).Append('"');
                Options.Theme.Render(canvas, StyleToken.String, _literalBuilder.ToString());
            }
        }

        protected override bool VisitDictionaryValue(ValueFormatterState state, DictionaryValue dictionary)
        {
            if (state.Format.Contains("j"))
            {
                _jsonValueFormatter ??= new JsonValueFormatter(Options);
                _jsonValueFormatter.Format(dictionary, state.Canvas, state.Format, state.IsLiteral);
                return true;
            }

            Options.Theme.Render(state.Canvas, StyleToken.TertiaryText, "{");

            var delimiter = string.Empty;
            foreach (var (scalarValue, propertyValue) in dictionary.Elements)
            {
                if (!string.IsNullOrEmpty(delimiter))
                {
                    Options.Theme.Render(state.Canvas, StyleToken.TertiaryText, delimiter);
                }

                delimiter = ", ";
                Options.Theme.Render(state.Canvas, StyleToken.TertiaryText, "[");
                Visit(state.Next(), scalarValue);
                Options.Theme.Render(state.Canvas, StyleToken.TertiaryText, "]=");
                Visit(state.Next(), propertyValue);
            }

            Options.Theme.Render(state.Canvas, StyleToken.TertiaryText, "}");
            return true;
        }

        protected override bool VisitScalarValue(ValueFormatterState state, ScalarValue scalar)
        {
            FormatLiteralValue(scalar, state.Canvas, state.Format, state.IsLiteral);
            return true;
        }

        protected override bool VisitSequenceValue(ValueFormatterState state, SequenceValue sequence)
        {
            if (state.Format.Contains("j"))
            {
                _jsonValueFormatter ??= new JsonValueFormatter(Options);
                _jsonValueFormatter.Format(sequence, state.Canvas, state.Format, state.IsLiteral);
                return true;
            }

            Options.Theme.Render(state.Canvas, StyleToken.TertiaryText, "[");

            var delimiter = string.Empty;
            foreach (var propertyValue in sequence.Elements)
            {
                if (!string.IsNullOrEmpty(delimiter))
                {
                    Options.Theme.Render(state.Canvas, StyleToken.TertiaryText, delimiter);
                }

                delimiter = ", ";
                Visit(state.Next(), propertyValue);
            }

            Options.Theme.Render(state.Canvas, StyleToken.TertiaryText, "]");
            return true;
        }

        protected override bool VisitStructureValue(ValueFormatterState state, StructureValue structure)
        {
            if (state.Format.Contains("j"))
            {
                _jsonValueFormatter ??= new JsonValueFormatter(Options);
                _jsonValueFormatter.Format(structure, state.Canvas, state.Format, state.IsLiteral);
                return true;
            }

            if (structure.TypeTag != null)
            {
                Options.Theme.Render(state.Canvas, StyleToken.Name, structure.TypeTag + " ");
            }

            Options.Theme.Render(state.Canvas, StyleToken.TertiaryText, "{");

            var delimiter = string.Empty;
            foreach (var eventProperty in structure.Properties)
            {
                if (!string.IsNullOrEmpty(delimiter))
                {
                    Options.Theme.Render(state.Canvas, StyleToken.TertiaryText, delimiter);
                }

                delimiter = ", ";
                Options.Theme.Render(state.Canvas, StyleToken.Name, eventProperty.Name);
                Options.Theme.Render(state.Canvas, StyleToken.TertiaryText, "=");
                Visit(state.Next(), eventProperty.Value);
            }

            Options.Theme.Render(state.Canvas, StyleToken.TertiaryText, "}");
            return true;
        }
    }
}