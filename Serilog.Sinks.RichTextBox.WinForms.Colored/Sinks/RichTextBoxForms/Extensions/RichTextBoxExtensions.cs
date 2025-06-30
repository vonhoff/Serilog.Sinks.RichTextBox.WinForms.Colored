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
        [LibraryImport("user32.dll", EntryPoint = "SendMessageW")]
        private static partial IntPtr SendMessage(IntPtr hWnd, int wMsg, IntPtr wParam, IntPtr lParam);

        [LibraryImport("user32.dll", EntryPoint = "SendMessageW")]
        private static partial IntPtr SendMessage(IntPtr hWnd, int wMsg, int wParam, ref Point lParam);
#else
        // Fallback for older target frameworks that do not support source-generated P/Invokes.
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern IntPtr SendMessage(IntPtr hWnd, int wMsg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern IntPtr SendMessage(IntPtr hWnd, int wMsg, int wParam, ref Point lParam);
#endif

        public static void SetRtf(this RichTextBox richTextBox, string rtf, bool autoScroll)
        {
            if (richTextBox.InvokeRequired)
            {
                try
                {
                    // Use a timeout to prevent deadlocks
                    var asyncResult = richTextBox.BeginInvoke(new Action(() => SetRtfInternal(richTextBox, rtf, autoScroll)));
                    
                    // Wait for the invoke to complete with a timeout
                    if (!asyncResult.AsyncWaitHandle.WaitOne(TimeSpan.FromSeconds(10)))
                    {
                        System.Diagnostics.Debug.WriteLine("Warning: SetRtf BeginInvoke timed out");
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Error in SetRtf BeginInvoke: {ex}");
                }
                return;
            }

            SetRtfInternal(richTextBox, rtf, autoScroll);
        }

        private static void SetRtfInternal(RichTextBox richTextBox, string rtf, bool autoScroll)
        {
            richTextBox.Suspend();
            var originalZoomFactor = richTextBox.ZoomFactor;
            var scrollPoint = new Point();

            if (!autoScroll)
            {
                SendMessage(richTextBox.Handle, EM_GETSCROLLPOS, 0, ref scrollPoint);
            }

            richTextBox.Rtf = rtf;

            // Re-apply the zoom level, as assigning to the Rtf property resets it back to 1.0.
            if (Math.Abs(richTextBox.ZoomFactor - originalZoomFactor) > float.Epsilon)
            {
                richTextBox.ZoomFactor = originalZoomFactor;
            }

            if (!autoScroll)
            {
                SendMessage(richTextBox.Handle, EM_SETSCROLLPOS, 0, ref scrollPoint);
            }
            else
            {
                SendMessage(richTextBox.Handle, WM_VSCROLL, (IntPtr)SB_PAGEBOTTOM, IntPtr.Zero);
            }

            richTextBox.Resume();
        }
    }
}