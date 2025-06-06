﻿#region Copyright 2022 Simon Vonhoff & Contributors

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
using Serilog.Sinks.RichTextBoxForms.Themes;
using System;
using System.IO;
using System.Windows.Forms;

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

        public void Render(LogEvent logEvent, RichTextBox richTextBox)
        {
            var scalarValue = new ScalarValue(logEvent.Timestamp);
            var writer = new StringWriter();
            scalarValue.Render(writer, _token.Format, _formatProvider);
            _theme.Render(richTextBox, StyleToken.SecondaryText, writer.ToString());
        }
    }
}