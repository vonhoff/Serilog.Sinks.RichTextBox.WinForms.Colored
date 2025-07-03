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

using Serilog.Data;
using Serilog.Events;
using Serilog.Sinks.RichTextBoxForms.Rtf;
using Serilog.Sinks.RichTextBoxForms.Themes;
using System;
using System.Text;

namespace Serilog.Sinks.RichTextBoxForms.Formatting
{
    public abstract class ValueFormatter : LogEventPropertyValueVisitor<ValueFormatterState, bool>
    {
        private readonly StringBuilder _formatBuilder = new();
        private readonly IFormatProvider? _formatProvider;

        protected ValueFormatter(Theme theme, IFormatProvider? formatProvider = null)
        {
            Theme = theme;
            _formatProvider = formatProvider;
        }

        protected Theme Theme { get; }

        public void Format(LogEventPropertyValue value, IRtfCanvas canvas, string format, bool isLiteral)
        {
            Visit(new ValueFormatterState(canvas, format, isLiteral), value);
        }

        /// <summary>
        /// Determines the appropriate StyleToken for a given type.
        /// </summary>
        /// <param name="type">The type to analyze</param>
        /// <returns>StyleToken.Number for numeric types, StyleToken.Scalar for others</returns>
        protected static StyleToken GetStyleTokenForType(Type type)
        {
            return type == typeof(float) || type == typeof(double) || type == typeof(decimal) ||
                   type == typeof(int) || type == typeof(uint) || type == typeof(long) || type == typeof(ulong) ||
                   type == typeof(byte) || type == typeof(sbyte) || type == typeof(short) || type == typeof(ushort)
                ? StyleToken.Number : StyleToken.Scalar;
        }

        protected string? ProcessFormat(string? format, Type type)
        {
            var effectiveFormat = format;

            // Remove sink-specific format specifiers (e.g. "l" for literal, "j" for JSON) that are not
            // recognised by the underlying IFormattable implementation and would otherwise trigger
            // a FormatException.
            if (!string.IsNullOrEmpty(effectiveFormat))
            {
                _formatBuilder.Clear();
                _formatBuilder.Append(effectiveFormat);
                _formatBuilder.Replace("l", string.Empty);
                _formatBuilder.Replace("j", string.Empty);
                effectiveFormat = _formatBuilder.ToString();

                if (string.IsNullOrWhiteSpace(effectiveFormat))
                {
                    effectiveFormat = null;
                }
            }

            // Apply default formats for specific types
            if (type == typeof(TimeSpan) && string.IsNullOrEmpty(effectiveFormat))
            {
                effectiveFormat = "c";
            }

            if (type == typeof(Guid) && string.IsNullOrEmpty(effectiveFormat))
            {
                effectiveFormat = "D";
            }

            return effectiveFormat;
        }

        protected void RenderFormattable(IRtfCanvas canvas, IFormattable formattable, string? format)
        {
            var type = formattable.GetType();
            var token = GetStyleTokenForType(type);
            var effectiveFormat = ProcessFormat(format, type);

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
    }
}