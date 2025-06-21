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

namespace Serilog.Sinks.RichTextBoxForms.Formatting
{
    public class DisplayValueFormatter : ValueFormatter
    {
        private readonly IFormatProvider? _formatProvider;
        private JsonValueFormatter? _jsonValueFormatter;

        public DisplayValueFormatter(Theme theme, IFormatProvider? formatProvider) : base(theme)
        {
            _formatProvider = formatProvider;
        }

        public void FormatLiteralValue(ScalarValue scalar, IRtfCanvas canvas, string? format, bool isLiteral)
        {
            var value = scalar.Value;

            if (value is null)
            {
                Theme.Render(canvas, StyleToken.Null, "null");
                return;
            }
            if (value is string text)
            {
                RenderString(text, canvas, format, isLiteral);
                return;
            }
            if (value is byte[] bytes)
            {
                Theme.Render(canvas, StyleToken.String, $"\"{Convert.ToBase64String(bytes)}\"");
                return;
            }
            if (value is bool b)
            {
                Theme.Render(canvas, StyleToken.Boolean, b.ToString());
                return;
            }
            if (value is Uri uri)
            {
                Theme.Render(canvas, StyleToken.Scalar, uri.ToString());
                return;
            }
            if (value is IFormattable formattable)
            {
                RenderFormattable(formattable, format, canvas);
                return;
            }

            var sb = StringBuilderCache.Acquire(256);
            try
            {
                using (var writer = new StringWriter(sb))
                {
                    scalar.Render(writer, null, _formatProvider);
                }
                Theme.Render(canvas, StyleToken.Scalar, sb.ToString());
            }
            finally
            {
                StringBuilderCache.Release(sb);
            }
        }

        private void RenderString(string text, IRtfCanvas canvas, string? format, bool isLiteral)
        {
            bool effectivelyLiteral = isLiteral || (format != null && format.Contains("l"));
            if (effectivelyLiteral)
            {
                Theme.Render(canvas, StyleToken.String, text);
            }
            else
            {
                Theme.Render(canvas, StyleToken.String, $"\"{text.Replace("\"", "\\\"")}\"");
            }
        }

        private void RenderFormattable(IFormattable formattable, string? format, IRtfCanvas canvas)
        {
            // Use Number style for numbers, Scalar for others (DateTime, Guid, etc.)
            var type = formattable.GetType();
            StyleToken token =
                type == typeof(float) || type == typeof(double) || type == typeof(decimal) ||
                type == typeof(int) || type == typeof(uint) || type == typeof(long) || type == typeof(ulong) ||
                type == typeof(byte) || type == typeof(sbyte) || type == typeof(short) || type == typeof(ushort)
                ? StyleToken.Number : StyleToken.Scalar;

            string? effectiveFormat = format;

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

        protected override bool VisitDictionaryValue(ValueFormatterState state, DictionaryValue dictionary)
        {
            if (state.Format != null && state.Format.Contains("j"))
            {
                _jsonValueFormatter ??= new JsonValueFormatter(Theme, _formatProvider);
                _jsonValueFormatter.Format(dictionary, state.Canvas, state.Format, state.IsLiteral);
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
            FormatLiteralValue(scalar, state.Canvas, state.Format, state.IsLiteral);
            return true;
        }

        protected override bool VisitSequenceValue(ValueFormatterState state, SequenceValue sequence)
        {
            if (state.Format != null && state.Format.Contains("j"))
            {
                _jsonValueFormatter ??= new JsonValueFormatter(Theme, _formatProvider);
                _jsonValueFormatter.Format(sequence, state.Canvas, state.Format, state.IsLiteral);
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
                _jsonValueFormatter ??= new JsonValueFormatter(Theme, _formatProvider);
                _jsonValueFormatter.Format(structure, state.Canvas, state.Format, state.IsLiteral);
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