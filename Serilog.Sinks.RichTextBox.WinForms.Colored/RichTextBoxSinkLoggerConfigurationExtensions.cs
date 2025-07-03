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
using System.Globalization;
using System.Windows.Forms;

namespace Serilog
{
    public static class RichTextBoxSinkLoggerConfigurationExtensions
    {
        private const string OutputTemplate = "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}";

        /// <summary>
        /// Adds a sink that writes log events to a Windows Forms <see cref="System.Windows.Forms.RichTextBox"/> with color-coded formatting.
        /// </summary>
        /// <param name="sinkConfiguration">The logger sink configuration.</param>
        /// <param name="richTextBoxControl">The <see cref="System.Windows.Forms.RichTextBox"/> to display log output.</param>
        /// <param name="richTextBoxSink">The created <see cref="RichTextBoxSink"/> instance.</param>
        /// <param name="theme">Optional theme for message colors. Defaults to <see cref="ThemePresets.Literate"/> if <c>null</c>.</param>
        /// <param name="autoScroll">If <c>true</c> (default), scrolls to the newest log entry automatically.</param>
        /// <param name="maxLogLines">Maximum number of log events retained and displayed.</param>
        /// <param name="outputTemplate">Template for formatting each log event.</param>
        /// <param name="formatProvider">Format provider, or <c>null</c> for invariant culture.</param>
        /// <param name="minimumLogEventLevel">Minimum log level for events to be written.</param>
        /// <param name="levelSwitch">Optional switch to change the minimum log level at runtime.</param>
        /// <returns>The logger configuration, for chaining.</returns>
        public static LoggerConfiguration RichTextBox(
            this LoggerSinkConfiguration sinkConfiguration,
            RichTextBox richTextBoxControl,
            out RichTextBoxSink richTextBoxSink,
            Theme? theme = null,
            bool autoScroll = true,
            int maxLogLines = 256,
            string outputTemplate = OutputTemplate,
            IFormatProvider? formatProvider = null,
            LogEventLevel minimumLogEventLevel = LogEventLevel.Verbose,
            LoggingLevelSwitch? levelSwitch = null)
        {
            var appliedTheme = theme ?? ThemePresets.Literate;
            var appliedFormatProvider = formatProvider ?? CultureInfo.InvariantCulture;
            var renderer = new TemplateRenderer(appliedTheme, outputTemplate, appliedFormatProvider);
            var options = new RichTextBoxSinkOptions(appliedTheme, autoScroll, maxLogLines, outputTemplate, appliedFormatProvider);
            richTextBoxSink = new RichTextBoxSink(richTextBoxControl, options, renderer);
            return sinkConfiguration.Sink(richTextBoxSink, minimumLogEventLevel, levelSwitch);
        }

        /// <summary>
        /// Adds a sink that writes log events to a Windows Forms <see cref="System.Windows.Forms.RichTextBox"/> with color-coded formatting.
        /// </summary>
        /// <param name="sinkConfiguration">The logger sink configuration.</param>
        /// <param name="richTextBoxControl">The <see cref="System.Windows.Forms.RichTextBox"/> to display log output.</param>
        /// <param name="theme">Optional theme for message colors. Defaults to <see cref="ThemePresets.Literate"/> if <c>null</c>.</param>
        /// <param name="autoScroll">If <c>true</c> (default), scrolls to the newest log entry automatically.</param>
        /// <param name="maxLogLines">Maximum number of log events retained and displayed.</param>
        /// <param name="outputTemplate">Template for formatting each log event.</param>
        /// <param name="formatProvider">Format provider, or <c>null</c> for invariant culture.</param>
        /// <param name="minimumLogEventLevel">Minimum log level for events to be written.</param>
        /// <param name="levelSwitch">Optional switch to change the minimum log level at runtime.</param>
        /// <returns>The logger configuration, for chaining.</returns>
        public static LoggerConfiguration RichTextBox(
            this LoggerSinkConfiguration sinkConfiguration,
            RichTextBox richTextBoxControl,
            Theme? theme = null,
            bool autoScroll = true,
            int maxLogLines = 256,
            string outputTemplate = OutputTemplate,
            IFormatProvider? formatProvider = null,
            LogEventLevel minimumLogEventLevel = LogEventLevel.Verbose,
            LoggingLevelSwitch? levelSwitch = null)
        {
            return RichTextBox(
                sinkConfiguration,
                richTextBoxControl,
                out _,
                theme,
                autoScroll,
                maxLogLines,
                outputTemplate,
                formatProvider,
                minimumLogEventLevel,
                levelSwitch);
        }
    }
}