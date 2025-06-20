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
using Serilog.Sinks.RichTextBoxForms.Formatting;
using Serilog.Sinks.RichTextBoxForms.Rtf;
using Serilog.Sinks.RichTextBoxForms.Themes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Serilog.Sinks.RichTextBoxForms.Rendering
{
    public class PropertiesTokenRenderer : ITokenRenderer
    {
        private readonly MessageTemplate _outputTemplate;
        private readonly ValueFormatter _valueFormatter;
        private readonly HashSet<string> _outputTemplateProperties;

        public PropertiesTokenRenderer(Theme theme, PropertyToken token, MessageTemplate outputTemplate, IFormatProvider? formatProvider)
        {
            _outputTemplate = outputTemplate;
            _valueFormatter = token.Format?.Contains("j") == true
                ? new JsonValueFormatter(theme, formatProvider)
                : new DisplayValueFormatter(theme, formatProvider);

            _outputTemplateProperties = new HashSet<string>(
                outputTemplate.Tokens.OfType<PropertyToken>().Select(p => p.PropertyName));
        }

        public void Render(LogEvent logEvent, IRtfCanvas canvas)
        {
            var included = logEvent.Properties
                .Where(p =>
                    !TemplateContainsPropertyName(logEvent.MessageTemplate, p.Key) &&
                    !_outputTemplateProperties.Contains(p.Key))
                .Select(p => new LogEventProperty(p.Key, p.Value));

            var value = new StructureValue(included);
            _valueFormatter.Format(value, canvas, string.Empty, false);
        }

        private static bool TemplateContainsPropertyName(MessageTemplate template, string propertyName)
        {
            foreach (var token in template.Tokens)
            {
                if (token is PropertyToken namedProperty && namedProperty.PropertyName == propertyName)
                {
                    return true;
                }
            }

            return false;
        }
    }
}