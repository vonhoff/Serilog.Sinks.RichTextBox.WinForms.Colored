﻿#region Copyright 2025 Simon Vonhoff & Contributors

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
using Serilog.Sinks.RichTextBoxForms.Formatting;
using Serilog.Sinks.RichTextBoxForms.Rtf;
using Serilog.Sinks.RichTextBoxForms.Themes;
using System;

namespace Serilog.Sinks.RichTextBoxForms.Rendering
{
    public class MessageTemplateTokenRenderer : ITokenRenderer
    {
        private readonly MessageTemplateRenderer _renderer;

        public MessageTemplateTokenRenderer(Theme theme, PropertyToken token, IFormatProvider? formatProvider)
        {
            var isLiteral = token.Format?.Contains("l") == true;
            var isJson = token.Format?.Contains("j") == true;

            ValueFormatter valueFormatter = isJson
                ? new JsonValueFormatter(theme, formatProvider)
                : new DisplayValueFormatter(theme, formatProvider);

            _renderer = new MessageTemplateRenderer(theme, valueFormatter, isLiteral);
        }

        public void Render(LogEvent logEvent, IRtfCanvas canvas)
        {
            _renderer.Render(logEvent.MessageTemplate, logEvent.Properties, canvas);
        }
    }
}