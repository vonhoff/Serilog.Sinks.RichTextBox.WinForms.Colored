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
    /// <summary>
    /// Control extensions to suspend or resume painting methods.
    /// Based on: https://stackoverflow.com/a/6302008
    /// </summary>
    internal static class ControlExtensions
    {
        private const int WM_SETREDRAW = 0x000B;

        /// <summary>
        /// Resumes painting for a specified control.
        /// </summary>
        /// <param name="control">The control to resume.</param>
        public static void Resume(this Control control)
        {
            var resumeUpdateMessage = Message.Create(control.Handle, WM_SETREDRAW, new IntPtr(1),
                IntPtr.Zero);

            InvokeWindowProcedure(control, ref resumeUpdateMessage);
            control.Refresh();
        }

        /// <summary>
        /// Suspends painting for a specified control.
        /// </summary>
        /// <param name="control">The control to suspend.</param>
        public static void Suspend(this Control control)
        {
            var suspendUpdateMessage = Message.Create(control.Handle, WM_SETREDRAW, IntPtr.Zero,
                IntPtr.Zero);

            InvokeWindowProcedure(control, ref suspendUpdateMessage);
        }

        /// <summary>
        /// Invokes the default window procedure associated with the provided control.
        /// </summary>
        /// <param name="control">The control.</param>
        /// <param name="message">Reference to a Windows message.</param>
        private static void InvokeWindowProcedure(in Control control, ref Message message)
        {
            var window = NativeWindow.FromHandle(control.Handle);
            window.DefWndProc(ref message);
        }
    }
}