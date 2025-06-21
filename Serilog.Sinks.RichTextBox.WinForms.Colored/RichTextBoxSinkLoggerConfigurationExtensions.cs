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
        /// Adds a sink that writes log events to the specified Windows Forms <see cref="RichTextBox"/>
        /// using colour-coded rich-text formatting.
        /// </summary>
        /// <param name="sinkConfiguration">The logger sink configuration this extension method operates on.</param>
        /// <param name="richTextBoxControl">The target <see cref="RichTextBox"/> instance that will display the log output.</param>
        /// <param name="theme">Optional theme controlling colours of individual message tokens. When <c>null</c>, <see cref="Serilog.Sinks.RichTextBoxForms.Themes.ThemePresets.Literate"/> is used.</param>
        /// <param name="autoScroll">When <c>true</c> (default) the control automatically scrolls to the newest log entry.</param>
        /// <param name="maxLogLines">Maximum number of log events retained in the circular buffer and rendered in the control. Must be between 1 and 1,024 (default: 256).</param>
        /// <param name="outputTemplate">Message template that controls the textual representation of each log event.</param>
        /// <param name="formatProvider">Culture-specific or custom formatting provider, or <c>null</c> to use the invariant culture.</param>
        /// <param name="minimumLogEventLevel">Minimum level below which events are ignored by this sink.</param>
        /// <param name="levelSwitch">Optional switch allowing the minimum log level to be changed at runtime.</param>
        /// <returns>A <see cref="LoggerConfiguration"/> object that can be further configured.</returns>
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
            var appliedTheme = theme ?? ThemePresets.Literate;
            var renderer = new TemplateRenderer(appliedTheme, outputTemplate, formatProvider);
            var options = new RichTextBoxSinkOptions(appliedTheme, autoScroll, maxLogLines, outputTemplate, formatProvider);
            var sink = new RichTextBoxSink(richTextBoxControl, options, renderer);
            return sinkConfiguration.Sink(sink, minimumLogEventLevel, levelSwitch);
        }
    }
}