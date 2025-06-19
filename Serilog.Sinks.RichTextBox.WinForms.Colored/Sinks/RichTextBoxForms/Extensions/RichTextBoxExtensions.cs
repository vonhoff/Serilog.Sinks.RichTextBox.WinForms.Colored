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

using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Serilog.Sinks.RichTextBoxForms.Extensions
{
    public static class RichTextBoxExtensions
    {
        private const int WM_USER = 0x400;
        private const int EM_GETSCROLLPOS = WM_USER + 221;
        private const int EM_SETSCROLLPOS = WM_USER + 222;
        private const int WM_VSCROLL = 277;
        private const int SB_PAGEBOTTOM = 7;
        private const string NullCharacter = "\0";

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern IntPtr SendMessage(IntPtr hWnd, int wMsg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
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
            // Avoid the recursive double-call pattern – schedule the actual work only once.
            if (richTextBox.InvokeRequired)
            {
                richTextBox.BeginInvoke(new Action(() => AppendRtfInternal(richTextBox, rtf, autoScroll, maxLogLines)));
                return;
            }

            AppendRtfInternal(richTextBox, rtf, autoScroll, maxLogLines);
        }

        private static void AppendRtfInternal(RichTextBox richTextBox, string rtf, bool autoScroll, int maxLogLines)
        {
            richTextBox.Suspend();
            var scrollPoint = Point.Empty;
            var previousSelectionStart = richTextBox.SelectionStart;
            var previousSelectionLength = richTextBox.SelectionLength;

            if (!autoScroll)
            {
                SendMessage(richTextBox.Handle, EM_GETSCROLLPOS, 0, ref scrollPoint);
            }

            richTextBox.SelectionStart = richTextBox.TextLength;
            if (richTextBox.TextLength > 0)
            {
                richTextBox.SelectedText = Environment.NewLine;
            }
            richTextBox.SelectedRtf = rtf;

            var selectionStart = richTextBox.TextLength - 1;
            richTextBox.SelectionStart = selectionStart >= 0 ? selectionStart : 0;
            richTextBox.SelectionLength = 1;
            richTextBox.SelectedText = NullCharacter;

            var totalLines = richTextBox.GetLineFromCharIndex(richTextBox.TextLength) + 1;
            if (totalLines > maxLogLines)
            {
                var linesToRemove = totalLines - maxLogLines;

                if (linesToRemove > 0)
                {
                    var charIndex = richTextBox.GetFirstCharIndexFromLine(linesToRemove);
                    if (charIndex > 0)
                    {
                        richTextBox.SelectionStart = 0;
                        richTextBox.SelectionLength = charIndex;
                        richTextBox.SelectedText = NullCharacter;
                        previousSelectionStart = 0;
                        previousSelectionLength = 0;
                    }
                }
            }

            if (!autoScroll)
            {
                richTextBox.SelectionStart = previousSelectionStart;
                richTextBox.SelectionLength = previousSelectionLength;
                SendMessage(richTextBox.Handle, EM_SETSCROLLPOS, 0, ref scrollPoint);
            }
            else
            {
                SendMessage(richTextBox.Handle, WM_VSCROLL, (IntPtr)SB_PAGEBOTTOM, IntPtr.Zero);
                richTextBox.SelectionStart = richTextBox.Text.Length;
            }

            richTextBox.Resume();
        }
    }
}