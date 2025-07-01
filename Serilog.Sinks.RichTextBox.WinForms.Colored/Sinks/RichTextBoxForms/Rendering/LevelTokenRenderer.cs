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

namespace Serilog.Sinks.RichTextBoxForms.Rendering
{
    public class LevelTokenRenderer : ITokenRenderer
    {
        private static readonly StyleToken[] LevelStyles =
        {
            StyleToken.LevelVerbose,
            StyleToken.LevelDebug,
            StyleToken.LevelInformation,
            StyleToken.LevelWarning,
            StyleToken.LevelError,
            StyleToken.LevelFatal
        };

        private static readonly string[][] LowercaseLevelMap =
        {
            new[] { "v", "vb", "vrb", "verb" },
            new[] { "d", "de", "dbg", "dbug" },
            new[] { "i", "in", "inf", "info" },
            new[] { "w", "wn", "wrn", "warn" },
            new[] { "e", "er", "err", "eror" },
            new[] { "f", "fa", "ftl", "fatl" }
        };

        private static readonly string[][] TitleCaseLevelMap =
        {
            new[] { "V", "Vb", "Vrb", "Verb" },
            new[] { "D", "De", "Dbg", "Dbug" },
            new[] { "I", "In", "Inf", "Info" },
            new[] { "W", "Wn", "Wrn", "Warn" },
            new[] { "E", "Er", "Err", "Eror" },
            new[] { "F", "Fa", "Ftl", "Fatl" }
        };

        private static readonly string[][] UppercaseLevelMap =
        {
            new[] { "V", "VB", "VRB", "VERB" },
            new[] { "D", "DE", "DBG", "DBUG" },
            new[] { "I", "IN", "INF", "INFO" },
            new[] { "W", "WN", "WRN", "WARN" },
            new[] { "E", "ER", "ERR", "EROR" },
            new[] { "F", "FA", "FTL", "FATL" }
        };

        private readonly Theme _theme;
        private readonly string[] _monikers = new string[LevelStyles.Length];

        public LevelTokenRenderer(Theme theme, PropertyToken levelToken)
        {
            _theme = theme;
            var format = levelToken.Format ?? string.Empty;
            for (var i = 0; i < _monikers.Length; i++)
            {
                _monikers[i] = GetLevelMoniker((LogEventLevel)i, format);
            }
        }

        public void Render(LogEvent logEvent, IRtfCanvas canvas)
        {
            var levelIndex = (int)logEvent.Level;
            var moniker = _monikers[levelIndex];
            var levelStyle = LevelStyles[levelIndex];
            _theme.Render(canvas, levelStyle, moniker);
        }

        private static string GetLevelMoniker(LogEventLevel value, string format = "")
        {
            if (format.Length != 2 && format.Length != 3)
            {
                return TextFormatter.Format(value.ToString(), format);
            }

            var width = format[1] - '0';
            if (format.Length == 3)
            {
                width *= 10;
                width += format[2] - '0';
            }

            switch (width)
            {
                case < 1:
                    return string.Empty;
                case > 4:
                    {
                        var stringValue = value.ToString();
                        if (stringValue.Length > width)
                        {
                            stringValue = stringValue.Substring(0, width);
                        }

                        return TextFormatter.Format(stringValue, format);
                    }
            }

            var index = (int)value;
            if (index is < 0 or > (int)LogEventLevel.Fatal)
            {
                return TextFormatter.Format(value.ToString(), format);
            }

            return format[0] switch
            {
                'w' => LowercaseLevelMap[index][width - 1],
                'u' => UppercaseLevelMap[index][width - 1],
                't' => TitleCaseLevelMap[index][width - 1],
                _ => TextFormatter.Format(value.ToString(), format)
            };
        }
    }
}