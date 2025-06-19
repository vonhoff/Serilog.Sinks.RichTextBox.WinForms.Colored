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
using Serilog.Formatting.Display;
using Serilog.Parsing;
using Serilog.Sinks.RichTextBoxForms.Rtf;
using Serilog.Sinks.RichTextBoxForms.Themes;
using System;
using System.Collections.Generic;

namespace Serilog.Sinks.RichTextBoxForms.Rendering
{
    public class TemplateRenderer : ITokenRenderer
    {
        private const string OutputTemplate = "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}";
        private readonly List<ITokenRenderer> _renderers;

        public TemplateRenderer(Theme theme, string outputTemplate = OutputTemplate, IFormatProvider? formatProvider = null)
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
                        {
                            _renderers.Add(new LevelTokenRenderer(theme, propertyToken));
                            break;
                        }

                    case OutputProperties.NewLinePropertyName:
                        {
                            _renderers.Add(new NewLineTokenRenderer());
                            break;
                        }

                    case OutputProperties.ExceptionPropertyName:
                        {
                            _renderers.Add(new ExceptionTokenRenderer(theme));
                            break;
                        }

                    case OutputProperties.MessagePropertyName:
                        {
                            _renderers.Add(new MessageTemplateTokenRenderer(theme, propertyToken, formatProvider));
                            break;
                        }

                    case OutputProperties.TimestampPropertyName:
                        {
                            _renderers.Add(new TimestampTokenRenderer(theme, propertyToken, formatProvider));
                            break;
                        }

                    case OutputProperties.PropertiesPropertyName:
                        {
                            _renderers.Add(new PropertiesTokenRenderer(theme, propertyToken, template, formatProvider));
                            break;
                        }

                    default:
                        {
                            _renderers.Add(new EventPropertyTokenRenderer(theme, propertyToken, formatProvider));
                            break;
                        }
                }
            }
        }

        public void Render(LogEvent logEvent, IRtfCanvas canvas)
        {
            foreach (var renderer in _renderers)
            {
                renderer.Render(logEvent, canvas);
            }
        }

        /// <summary>
        /// Convenience overload that allows rendering directly into a <see cref="System.Windows.Forms.RichTextBox"/>
        /// without the caller needing to create the adapter manually. This is primarily for legacy tests and
        /// backward-compatibility. New code should prefer passing an <see cref="IRtfCanvas"/> implementation.
        /// </summary>
        /// <param name="logEvent">The log event to render.</param>
        /// <param name="richTextBox">Destination <see cref="System.Windows.Forms.RichTextBox"/>.</param>
        public void Render(LogEvent logEvent, System.Windows.Forms.RichTextBox richTextBox)
        {
            if (richTextBox == null) throw new ArgumentNullException(nameof(richTextBox));
            Render(logEvent, new Serilog.Sinks.RichTextBoxForms.Rtf.RichTextBoxCanvasAdapter(richTextBox));
        }
    }
}