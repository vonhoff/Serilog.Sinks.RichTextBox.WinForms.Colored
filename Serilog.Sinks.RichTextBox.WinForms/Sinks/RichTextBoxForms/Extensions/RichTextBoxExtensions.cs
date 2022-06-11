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
using System.Windows.Forms;

namespace Serilog.Sinks.RichTextBoxForms.Extensions
{
    internal static class RichTextBoxExtensions
    {
        private const string SpaceCharacter = " ";

        /// <summary>
        /// Updates the content of a <see cref="RichTextBox"/>
        /// with the specified content in rich text format (RTF).
        /// </summary>
        /// <param name="richTextBox">The <see cref="RichTextBox"/> to update.</param>
        /// <param name="rtf">The content in rich text format (RTF).</param>
        public static void AppendRtf(this RichTextBox richTextBox, string rtf)
        {
            if (richTextBox.InvokeRequired)
            {
                richTextBox.Invoke(new Action(() => AppendRtf(richTextBox, rtf)));
                return;
            }

            richTextBox.Suspend();
            richTextBox.SelectionStart = richTextBox.TextLength;

            if (richTextBox.TextLength > 0)
            {
                richTextBox.SelectedText = Environment.NewLine;
            }

            richTextBox.SelectedRtf = rtf;
            richTextBox.SelectionStart = Math.Max(0, richTextBox.TextLength - 2);
            richTextBox.SelectionLength = 2;
            richTextBox.SelectedText = SpaceCharacter;
            richTextBox.ScrollToCaret();
            richTextBox.Resume();
        }
    }
}