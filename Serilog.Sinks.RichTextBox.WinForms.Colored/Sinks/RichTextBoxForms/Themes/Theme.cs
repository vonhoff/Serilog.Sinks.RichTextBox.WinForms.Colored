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

using System.Collections.Generic;
using System.Windows.Forms;

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

        public void Render(RichTextBox richTextBox, StyleToken styleToken, string value)
        {
            if (!_styles.TryGetValue(styleToken, out var themeStyle))
            {
                return;
            }

            richTextBox.SelectionStart = richTextBox.TextLength;
            richTextBox.SelectionLength = 0;
            richTextBox.SelectionColor = themeStyle.Foreground;
            richTextBox.SelectionBackColor = themeStyle.Background;
            richTextBox.AppendText(value);

            richTextBox.SelectionColor = DefaultStyle.Foreground;
            richTextBox.SelectionBackColor = DefaultStyle.Background;
        }
    }
}