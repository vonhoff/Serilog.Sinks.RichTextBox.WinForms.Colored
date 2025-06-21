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
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Serilog.Sinks.RichTextBoxForms.Extensions
{
    public static partial class RichTextBoxExtensions
    {
        [StructLayout(LayoutKind.Sequential)]
        private struct Point
        {
            public int X;
            public int Y;
        }

        private const int WM_USER = 0x400;
        private const int EM_GETSCROLLPOS = WM_USER + 221;
        private const int EM_SETSCROLLPOS = WM_USER + 222;
        private const int WM_VSCROLL = 277;
        private const int SB_PAGEBOTTOM = 7;

#if NET7_0_OR_GREATER
        // Use source-generated P/Invoke for newer target frameworks to avoid the startup
        // penalty of runtime marshalling stub generation.
        [LibraryImport("user32.dll")]
        private static partial IntPtr SendMessage(IntPtr hWnd, int wMsg, IntPtr wParam, IntPtr lParam);

        [LibraryImport("user32.dll")]
        private static partial IntPtr SendMessage(IntPtr hWnd, int wMsg, int wParam, ref Point lParam);
#else
        // Fallback for older target frameworks that do not support source-generated P/Invokes.
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern IntPtr SendMessage(IntPtr hWnd, int wMsg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern IntPtr SendMessage(IntPtr hWnd, int wMsg, int wParam, ref Point lParam);
#endif

        public static void AppendRtf(this RichTextBox richTextBox, string rtf, bool autoScroll, int maxLogLines)
        {
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
            richTextBox.ReadOnly = false;

            var scrollPoint = new Point();
            var previousStart = richTextBox.SelectionStart;
            var previousLength = richTextBox.SelectionLength;

            if (!autoScroll)
            {
                SendMessage(richTextBox.Handle, EM_GETSCROLLPOS, 0, ref scrollPoint);
            }

            richTextBox.SelectionStart = richTextBox.TextLength;
            richTextBox.SelectedRtf = rtf;

            // Avoid using the richTextBox.Lines property on every append as it allocates
            // a new string array for the entire document and leads to excessive GC pressure
            // under heavy logging. Instead, rely on WinForms helpers that do not allocate.
            var currentLineCount = richTextBox.GetLineFromCharIndex(richTextBox.TextLength) + 1;
            if (currentLineCount > maxLogLines)
            {
                var linesToRemove = currentLineCount - maxLogLines;

                // `GetFirstCharIndexFromLine` returns the character index for the first character
                // in the specified line. By asking for the first line after the removal range
                // we get the exact character offset to trim.
                var charsToRemove = richTextBox.GetFirstCharIndexFromLine(linesToRemove);
                if (charsToRemove > 0)
                {
                    richTextBox.SelectionStart = 0;
                    richTextBox.SelectionLength = charsToRemove;
                    richTextBox.SelectedText = string.Empty;
                    previousStart = 0;
                    previousLength = 0;
                }
            }

            if (!autoScroll)
            {
                richTextBox.SelectionStart = previousStart;
                richTextBox.SelectionLength = previousLength;
                SendMessage(richTextBox.Handle, EM_SETSCROLLPOS, 0, ref scrollPoint);
            }
            else
            {
                SendMessage(richTextBox.Handle, WM_VSCROLL, (IntPtr)SB_PAGEBOTTOM, IntPtr.Zero);
                richTextBox.SelectionStart = richTextBox.Text.Length;
            }

            richTextBox.ReadOnly = true;
            richTextBox.Resume();
        }
    }
}