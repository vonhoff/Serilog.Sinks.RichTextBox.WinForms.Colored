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

using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Serilog.Events;
using Serilog.Parsing;
using Serilog.Sinks.RichTextBoxForms.Formatting;
using Serilog.Sinks.RichTextBoxForms.Themes;

namespace Serilog.Sinks.RichTextBoxForms.Rendering
{
    public class MessageTemplateRenderer
    {
        private readonly bool _isLiteral;
        private readonly Theme _theme;
        private readonly ValueFormatter _valueFormatter;

        public MessageTemplateRenderer(Theme theme, ValueFormatter valueFormatter, bool isLiteral)
        {
            _theme = theme ?? throw new ArgumentNullException(nameof(theme));
            _valueFormatter = valueFormatter;
            _isLiteral = isLiteral;
        }

        public void Render(MessageTemplate template, IReadOnlyDictionary<string, LogEventPropertyValue> properties,
            RichTextBox richTextBox)
        {
            foreach (var token in template.Tokens)
            {
                if (token is TextToken textToken)
                {
                    _theme.Render(richTextBox, StyleToken.Text, textToken.Text);
                }
                else
                {
                    var propertyToken = (PropertyToken)token;
                    RenderPropertyToken(propertyToken, properties, richTextBox);
                }
            }
        }

        private void RenderPropertyToken(PropertyToken propertyToken, IReadOnlyDictionary<string, LogEventPropertyValue> properties,
            RichTextBox richTextBox)
        {
            if (!properties.TryGetValue(propertyToken.PropertyName, out var propertyValue))
            {
                _theme.Render(richTextBox, StyleToken.Invalid, propertyToken.ToString());
                return;
            }

            RenderValue(_valueFormatter, propertyValue, richTextBox, propertyToken.Format);
        }

        private void RenderValue(ValueFormatter valueFormatter,
            LogEventPropertyValue propertyValue, RichTextBox richTextBox, string format)
        {
            if (_isLiteral && propertyValue is ScalarValue { Value: string } scalarValue)
            {
                _theme.Render(richTextBox, StyleToken.String, scalarValue.Value.ToString() ?? string.Empty);
                return;
            }

            valueFormatter.Format(propertyValue, richTextBox, format, _isLiteral);
        }
    }
}