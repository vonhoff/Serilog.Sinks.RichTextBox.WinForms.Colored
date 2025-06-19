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
        public ValueFormatterState(IRtfCanvas canvas)
        {
            Canvas = canvas;
            Format = string.Empty;
            IsTopLevel = false;
            IsLiteral = false;
        }

        public ValueFormatterState(IRtfCanvas canvas, string format, bool isTopLevel, bool isLiteral)
        {
            Canvas = canvas;
            Format = format;
            IsTopLevel = isTopLevel;
            IsLiteral = isLiteral;
        }

        public string Format { get; }
        public bool IsTopLevel { get; }
        public bool IsLiteral { get; }
        public IRtfCanvas Canvas { get; }

        public ValueFormatterState Next(string? format = null)
        {
            return new ValueFormatterState(Canvas, format ?? Format, false, IsLiteral);
        }
    }
}