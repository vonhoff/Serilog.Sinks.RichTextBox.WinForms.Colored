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
using Serilog.Configuration;
using Serilog.Core;
using Serilog.Events;
using Serilog.Sinks.RichTextBoxForms;
using Serilog.Sinks.RichTextBoxForms.Rendering;
using Serilog.Sinks.RichTextBoxForms.Themes;

namespace Serilog
{
    public static class RichTextBoxFormsSinkLoggerConfigurationExtensions
    {
        private const string OutputTemplate = "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}";

        /// <summary>
        /// Writes log events to a RichTextBox control.
        /// </summary>
        /// <param name="sinkConfiguration">Logger sink config.</param>
        /// <param name="richTextBoxControl">The RichTextBox to write to.</param>
        /// <param name="minimumLogEventLevel">Minimum event level (ignored if levelSwitch is set).</param>
        /// <param name="outputTemplate">Message format (default: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}").</param>
        /// <param name="formatProvider">Culture-specific formatting (null for default).</param>
        /// <param name="levelSwitch">Allows runtime level change.</param>
        /// <param name="theme">Output theme (default: Dark).</param>
        /// <param name="messageBatchSize">Max messages per batch for printing.</param>
        /// <param name="messageDequeueInterval">Deprecated as of v1.1.1, kept for backwards compatibility.</param>
        /// <param name="messagePendingInterval">Duration to hold incoming messages.</param>
        /// <param name="autoScroll">Auto-scroll to bottom for new messages.</param>
        /// <param name="maxLogLines">Maximum number of lines to keep.</param>
        /// <returns>Config object for chaining.</returns>
        public static LoggerConfiguration RichTextBox(
            this LoggerSinkConfiguration sinkConfiguration,
            RichTextBox richTextBoxControl,
            LogEventLevel minimumLogEventLevel = LogEventLevel.Verbose,
            string outputTemplate = OutputTemplate,
            IFormatProvider? formatProvider = null,
            LoggingLevelSwitch? levelSwitch = null,
            Theme? theme = null,
            int messageBatchSize = 200,
            int messageDequeueInterval = 0,
            int messagePendingInterval = 5,
            bool autoScroll = true,
            int maxLogLines = 0)
        {
            var appliedTheme = theme ?? ThemePresets.Dark;
            var renderer = new TemplateRenderer(appliedTheme, outputTemplate, formatProvider);
            var options = new RichTextBoxSinkOptions(appliedTheme, messageBatchSize, messagePendingInterval, autoScroll, maxLogLines);
            var sink = new RichTextBoxSink(richTextBoxControl, options, renderer);
            return sinkConfiguration.Sink(sink, minimumLogEventLevel, levelSwitch);
        }
    }
}