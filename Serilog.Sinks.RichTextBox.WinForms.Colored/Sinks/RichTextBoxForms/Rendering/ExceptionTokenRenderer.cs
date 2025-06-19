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
using Serilog.Sinks.RichTextBoxForms.Rtf;
using Serilog.Sinks.RichTextBoxForms.Themes;
using System;
using System.IO;

namespace Serilog.Sinks.RichTextBoxForms.Rendering
{
    public class ExceptionTokenRenderer : ITokenRenderer
    {
        private const string StackFrameLinePrefix = "   ";
        private readonly Theme _theme;

        public ExceptionTokenRenderer(Theme theme)
        {
            _theme = theme;
        }

        public void Render(LogEvent logEvent, IRtfCanvas canvas)
        {
            if (logEvent.Exception is null)
            {
                return;
            }

            var lines = new StringReader(logEvent.Exception.ToString());

            while (lines.ReadLine() is { } nextLine)
            {
                var style = nextLine.StartsWith(StackFrameLinePrefix) ? StyleToken.SecondaryText : StyleToken.Text;
                _theme.Render(canvas, style, nextLine);
                canvas.AppendText(Environment.NewLine);
            }
        }
    }
}