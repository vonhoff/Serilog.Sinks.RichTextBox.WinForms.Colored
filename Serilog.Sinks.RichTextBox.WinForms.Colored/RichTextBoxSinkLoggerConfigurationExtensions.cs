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

using Serilog.Configuration;
using Serilog.Core;
using Serilog.Events;
using Serilog.Sinks.RichTextBoxForms;
using Serilog.Sinks.RichTextBoxForms.Rendering;
using Serilog.Sinks.RichTextBoxForms.Themes;
using System;
using System.Windows.Forms;

namespace Serilog
{
    public static class RichTextBoxSinkLoggerConfigurationExtensions
    {
        private const string OutputTemplate = "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}";

        /// <summary>
        ///     Writes log events to a RichTextBox control.
        /// </summary>
        /// <param name="sinkConfiguration">Logger sink config.</param>
        /// <param name="richTextBoxControl">The RichTextBox to write to.</param>
        /// <param name="minimumLogEventLevel">Minimum event level (ignored if levelSwitch is set).</param>
        /// <param name="outputTemplate">
        ///     Message format (default: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}").
        /// </param>
        /// <param name="formatProvider">Culture-specific formatting (null for default).</param>
        /// <param name="levelSwitch">Allows runtime level change.</param>
        /// <param name="theme">Output theme (default: Dark).</param>
        /// <param name="autoScroll">Auto-scroll to bottom for new messages.</param>
        /// <param name="maxLogLines">Maximum number of lines to keep.</param>
        /// <param name="batchSize">Number of log events to process in a batch (default: 500).</param>
        /// <param name="flushInterval">Interval (ms) to flush logs (default: 16).</param>
        /// <param name="queueCapacity">Maximum number of log events in the queue (default: 1000).</param>
        /// <returns>Config object for chaining.</returns>
        public static LoggerConfiguration RichTextBox(
            this LoggerSinkConfiguration sinkConfiguration,
            RichTextBox richTextBoxControl,
            Theme? theme = null,
            bool autoScroll = true,
            int maxLogLines = 250,
            int batchSize = 500,
            int flushInterval = 16,
            int queueCapacity = 1000,
            string outputTemplate = OutputTemplate,
            IFormatProvider? formatProvider = null,
            LogEventLevel minimumLogEventLevel = LogEventLevel.Verbose,
            LoggingLevelSwitch? levelSwitch = null)
        {
            var appliedTheme = theme ?? ThemePresets.Literate;
            var renderer = new TemplateRenderer(appliedTheme, outputTemplate, formatProvider);
            var options = new RichTextBoxSinkOptions(
                appliedTheme,
                autoScroll,
                maxLogLines,
                batchSize,
                flushInterval,
                queueCapacity,
                outputTemplate,
                formatProvider);
            var sink = new RichTextBoxSink(richTextBoxControl, options, renderer);
            return sinkConfiguration.Sink(sink, minimumLogEventLevel, levelSwitch);
        }
    }
}