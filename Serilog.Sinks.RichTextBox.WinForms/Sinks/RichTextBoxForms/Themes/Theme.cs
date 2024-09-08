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

        /// <summary>
        /// Appends text to the provided <see cref="RichTextBox"/> with styling applied.
        /// </summary>
        /// <param name="richTextBox">The RichTextBox control to write to.</param>
        /// <param name="styleToken">The styling to apply.</param>
        /// <param name="value">The value to write.</param>
        public void Render(RichTextBox richTextBox, StyleToken styleToken, string value)
        {
            // Return if the token is not defined in the current theme.
            if (!_styles.TryGetValue(styleToken, out var themeStyle))
            {
                return;
            }

            richTextBox.SelectionStart = richTextBox.TextLength;
            richTextBox.SelectionLength = 0;
            richTextBox.SelectionColor = themeStyle.Foreground;
            richTextBox.SelectionBackColor = themeStyle.Background;

            // Append the provided value.
            richTextBox.AppendText(value);

            // Reset to default colors.
            richTextBox.SelectionColor = DefaultStyle.Foreground;
            richTextBox.SelectionBackColor = DefaultStyle.Background;
        }
    }
}