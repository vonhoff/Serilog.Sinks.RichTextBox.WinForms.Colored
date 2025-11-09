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

using Serilog.Sinks.RichTextBoxForms.Rtf;

namespace Serilog.Sinks.RichTextBoxForms.Formatting
{
    public readonly struct ValueFormatterState
    {
        public ValueFormatterState(IRtfCanvas canvas, string format, bool isLiteral, int indentLevel = 0, bool useSpacesForIndent = true, int indentSize = 4, bool isTopLevel = true)
        {
            Canvas = canvas;
            Format = format;
            IsLiteral = isLiteral;
            IndentLevel = indentLevel;
            UseSpacesForIndent = useSpacesForIndent;
            IndentSize = indentSize;
            IsTopLevel = isTopLevel;
        }

        public string Format { get; }
        public bool IsLiteral { get; }
        public IRtfCanvas Canvas { get; }
        public int IndentLevel { get; }
        public bool UseSpacesForIndent { get; }
        public int IndentSize { get; }
        public bool IsTopLevel { get; }

        public ValueFormatterState Next(string? format = null)
        {
            return new ValueFormatterState(Canvas, format ?? Format, IsLiteral, IndentLevel, UseSpacesForIndent, IndentSize, false);
        }

        public ValueFormatterState ToIndentUp()
        {
            return new ValueFormatterState(Canvas, Format, IsLiteral, IndentLevel + 1, UseSpacesForIndent, IndentSize, false);
        }

        public ValueFormatterState ToIndentDown()
        {
            return new ValueFormatterState(Canvas, Format, IsLiteral, IndentLevel > 0 ? IndentLevel - 1 : 0, UseSpacesForIndent, IndentSize, false);
        }

        public string GetIndentation()
        {
            if (IndentLevel <= 0)
            {
                return string.Empty;
            }

            var totalIndentUnits = IndentLevel * IndentSize;
            return UseSpacesForIndent
                ? new string(' ', totalIndentUnits)
                : new string('\t', totalIndentUnits);
        }
    }
}