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
using Serilog.Parsing;
using Serilog.Sinks.RichTextBoxForms.Rtf;
using Serilog.Sinks.RichTextBoxForms.Themes;
using System;

namespace Serilog.Sinks.RichTextBoxForms.Rendering
{
    public class TimestampTokenRenderer : ITokenRenderer
    {
        private readonly IFormatProvider? _formatProvider;
        private readonly Theme _theme;
        private readonly PropertyToken _token;

        public TimestampTokenRenderer(Theme theme, PropertyToken token, IFormatProvider? formatProvider)
        {
            _theme = theme;
            _token = token;
            _formatProvider = formatProvider;
        }

        public void Render(LogEvent logEvent, IRtfCanvas canvas)
        {
            var s = logEvent.Timestamp.ToString(_token.Format, _formatProvider);
            _theme.Render(canvas, StyleToken.SecondaryText, s);
        }
    }
}