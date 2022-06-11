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

using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Serilog.Events;
using Serilog.Formatting.Display;
using Serilog.Parsing;
using Serilog.Sinks.RichTextBoxForms.Themes;

namespace Serilog.Sinks.RichTextBoxForms.Rendering
{
    public class TemplateRenderer : ITokenRenderer
    {
        private readonly List<ITokenRenderer> _renderers;

        public TemplateRenderer(Theme theme, string outputTemplate, IFormatProvider? formatProvider = null)
        {
            if (string.IsNullOrEmpty(outputTemplate))
            {
                throw new ArgumentNullException(nameof(outputTemplate));
            }

            var template = new MessageTemplateParser().Parse(outputTemplate);
            _renderers = new List<ITokenRenderer>();
            foreach (var token in template.Tokens)
            {
                if (token is TextToken textToken)
                {
                    _renderers.Add(new TextTokenRenderer(theme, textToken.Text));
                    continue;
                }

                var propertyToken = (PropertyToken)token;

                switch (propertyToken.PropertyName)
                {
                    case OutputProperties.LevelPropertyName:
                        _renderers.Add(new LevelTokenRenderer(theme, propertyToken));
                        break;

                    case OutputProperties.NewLinePropertyName:
                        _renderers.Add(new NewLineTokenRenderer());
                        break;

                    case OutputProperties.ExceptionPropertyName:
                        _renderers.Add(new ExceptionTokenRenderer(theme));
                        break;

                    case OutputProperties.MessagePropertyName:
                        _renderers.Add(new MessageTemplateTokenRenderer(theme, propertyToken, formatProvider));
                        break;

                    case OutputProperties.TimestampPropertyName:
                        _renderers.Add(new TimestampTokenRenderer(theme, propertyToken, formatProvider));
                        break;

                    case OutputProperties.PropertiesPropertyName:
                        _renderers.Add(new PropertiesTokenRenderer(theme, propertyToken, template, formatProvider));
                        break;

                    default:
                        _renderers.Add(new EventPropertyTokenRenderer(theme, propertyToken, formatProvider));
                        break;
                }
            }
        }

        public void Render(LogEvent logEvent, RichTextBox richTextBox)
        {
            foreach (var renderer in _renderers)
            {
                renderer.Render(logEvent, richTextBox);
            }
        }
    }
}