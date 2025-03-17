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
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Serilog.Sinks.RichTextBoxForms.Extensions
{
    internal static class RichTextBoxExtensions
    {
        private const int WmUser = 0x400;
        private const int EmGetScrollPos = WmUser + 221;
        private const int EmSetScrollPos = WmUser + 222;
        private const string NullCharacter = "\0";

        [DllImport("user32.dll")]
        private static extern IntPtr SendMessage(IntPtr hWnd, int wMsg, int wParam, ref Point lParam);

        /// <summary>
        ///     Updates the content of a <see cref="RichTextBox" />
        ///     with the specified content in rich text format (RTF).
        /// </summary>
        /// <param name="richTextBox">The <see cref="RichTextBox" /> to update.</param>
        /// <param name="rtf">The content in rich text format (RTF).</param>
        /// <param name="autoScroll">Automatically scroll on update.</param>
        /// <param name="maxLogLines">Maximum number of lines to keep.</param>
        public static void AppendRtf(this RichTextBox richTextBox, string rtf, bool autoScroll, int maxLogLines)
        {
            if (richTextBox.InvokeRequired)
            {
                richTextBox.Invoke(new Action(() => AppendRtf(richTextBox, rtf, autoScroll, maxLogLines)));
                return;
            }

            richTextBox.Suspend();
            var scrollPoint = Point.Empty;
            var previousSelection = richTextBox.SelectionStart;
            var previousLength = richTextBox.SelectionLength;

            if (autoScroll == false)
            {
                SendMessage(richTextBox.Handle, EmGetScrollPos, 0, ref scrollPoint);
            }

            richTextBox.SelectionStart = richTextBox.TextLength;

            if (richTextBox.TextLength > 0)
            {
                richTextBox.SelectedText = Environment.NewLine;
            }

            richTextBox.SelectedRtf = rtf;

            var newLineLength = Environment.NewLine.Length;
            var selectionStart = richTextBox.TextLength - newLineLength;
            richTextBox.SelectionStart = selectionStart >= 0 ? selectionStart : 0;
            richTextBox.SelectionLength = newLineLength;
            richTextBox.SelectedText = NullCharacter;

            if (richTextBox.Lines.Length > maxLogLines)
            {
                var linesToRemove = richTextBox.Lines.Length - maxLogLines;
                var charsToRemove = 0;

                for (var i = 0; i < linesToRemove; i++)
                {
                    charsToRemove += richTextBox.Lines[i].Length + 1;
                }

                richTextBox.SelectionStart = 0;
                richTextBox.SelectionLength = charsToRemove;
                richTextBox.SelectedText = NullCharacter;
                previousSelection = 0;
                previousLength = 0;
            }

            if (autoScroll == false)
            {
                richTextBox.SelectionStart = previousSelection;
                richTextBox.SelectionLength = previousLength;
                SendMessage(richTextBox.Handle, EmSetScrollPos, 0, ref scrollPoint);
            }
            else
            {
                richTextBox.SelectionStart = richTextBox.TextLength;
                richTextBox.ScrollToCaret();
            }

            richTextBox.Resume();
        }
    }
}