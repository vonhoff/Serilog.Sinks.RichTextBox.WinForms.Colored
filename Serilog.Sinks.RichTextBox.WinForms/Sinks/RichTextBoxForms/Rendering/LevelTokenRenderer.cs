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

using System.Collections.Generic;
using System.Windows.Forms;
using Serilog.Events;
using Serilog.Parsing;
using Serilog.Sinks.RichTextBoxForms.Formatting;
using Serilog.Sinks.RichTextBoxForms.Themes;

namespace Serilog.Sinks.RichTextBoxForms.Rendering
{
    public class LevelTokenRenderer : ITokenRenderer
    {
        private static readonly IReadOnlyDictionary<LogEventLevel, StyleToken> Levels = new Dictionary<LogEventLevel, StyleToken>
        {
            { LogEventLevel.Verbose, StyleToken.LevelVerbose },
            { LogEventLevel.Debug, StyleToken.LevelDebug },
            { LogEventLevel.Information, StyleToken.LevelInformation },
            { LogEventLevel.Warning, StyleToken.LevelWarning },
            { LogEventLevel.Error, StyleToken.LevelError },
            { LogEventLevel.Fatal, StyleToken.LevelFatal },
        };

        private static readonly string[][] LowercaseLevelMap =
                {
            new[] { "v", "vb", "vrb", "verb" },
            new[] { "d", "de", "dbg", "dbug" },
            new[] { "i", "in", "inf", "info" },
            new[] { "w", "wn", "wrn", "warn" },
            new[] { "e", "er", "err", "eror" },
            new[] { "f", "fa", "ftl", "fatl" },
        };

        private static readonly string[][] TitleCaseLevelMap =
        {
            new[] { "V", "Vb", "Vrb", "Verb" },
            new[] { "D", "De", "Dbg", "Dbug" },
            new[] { "I", "In", "Inf", "Info" },
            new[] { "W", "Wn", "Wrn", "Warn" },
            new[] { "E", "Er", "Err", "Eror" },
            new[] { "F", "Fa", "Ftl", "Fatl" },
        };

        private static readonly string[][] UppercaseLevelMap =
        {
            new[] { "V", "VB", "VRB", "VERB" },
            new[] { "D", "DE", "DBG", "DBUG" },
            new[] { "I", "IN", "INF", "INFO" },
            new[] { "W", "WN", "WRN", "WARN" },
            new[] { "E", "ER", "ERR", "EROR" },
            new[] { "F", "FA", "FTL", "FATL" },
        };

        private readonly PropertyToken _levelToken;
        private readonly Theme _theme;

        public LevelTokenRenderer(Theme theme, PropertyToken levelToken)
        {
            _theme = theme;
            _levelToken = levelToken;
        }

        public static string GetLevelMoniker(LogEventLevel value, string? format = null)
        {
            if (format is null || (format.Length != 2 && format.Length != 3))
            {
                return TextCasing.Format(value.ToString(), format);
            }

            // Using int.Parse() here requires allocating a string to exclude the first character prefix.
            // Junk like "wxy" will be accepted but produce benign results.
            var width = format[1] - '0';
            if (format.Length == 3)
            {
                width *= 10;
                width += format[2] - '0';
            }

            switch (width)
            {
                case < 1:
                {
                    return string.Empty;
                }

                case > 4:
                {
                    var stringValue = value.ToString();
                    if (stringValue.Length > width)
                    {
                        stringValue = stringValue[..width];
                    }

                    return TextCasing.Format(stringValue);
                }
            }

            var index = (int)value;
            if (index is < 0 or > (int)LogEventLevel.Fatal)
            {
                return TextCasing.Format(value.ToString(), format);
            }

            return format[0] switch
            {
                'w' => LowercaseLevelMap[index][width - 1],
                'u' => UppercaseLevelMap[index][width - 1],
                't' => TitleCaseLevelMap[index][width - 1],
                _ => TextCasing.Format(value.ToString(), format)
            };
        }

        public void Render(LogEvent logEvent, RichTextBox richTextBox)
        {
            var moniker = GetLevelMoniker(logEvent.Level, _levelToken.Format);
            if (!Levels.TryGetValue(logEvent.Level, out var levelStyle))
            {
                levelStyle = StyleToken.Invalid;
            }

            _theme.Render(richTextBox, levelStyle, moniker);
        }
    }
}