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

using Serilog.Sinks.RichTextBoxForms.Rtf;
using System.Collections.Generic;

namespace Serilog.Sinks.RichTextBoxForms.Themes
{
    public class Theme
    {
        private readonly IReadOnlyDictionary<StyleToken, Style> _styles;

        public Theme(Style defaultStyle, IReadOnlyDictionary<StyleToken, Style> styles)
        {
            DefaultStyle = defaultStyle;
            _styles = styles;
        }

        public Style DefaultStyle { get; set; }

        public void Render(IRtfCanvas canvas, StyleToken styleToken, string value)
        {
            if (!_styles.TryGetValue(styleToken, out var themeStyle))
            {
                return;
            }

            canvas.SelectionStart = canvas.TextLength;
            canvas.SelectionLength = 0;
            canvas.SelectionColor = themeStyle.Foreground;
            canvas.SelectionBackColor = themeStyle.Background;
            canvas.AppendText(value);

            canvas.SelectionColor = DefaultStyle.Foreground;
            canvas.SelectionBackColor = DefaultStyle.Background;
        }
    }
}