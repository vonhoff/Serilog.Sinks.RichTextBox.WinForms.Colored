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
        private const string DefaultRichTextBoxOutputTemplate = "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}";

        /// <summary>
        /// Writes log events to a <see cref="RichTextBox"/> control.
        /// </summary>
        /// <param name="sinkConfiguration">Logger sink configuration.</param>
        /// <param name="richTextBoxControl">The RichTextBox control to write to.</param>
        /// <param name="minimumLogEventLevel">The minimum level for
        /// events passed through the sink. Ignored when <paramref name="levelSwitch"/> is specified.</param>
        /// <param name="outputTemplate">A message template describing the format used to write to the sink.
        /// The default is <c>"[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}"</c>.</param>
        /// <param name="formatProvider">Supplies culture-specific formatting information, or null.</param>
        /// <param name="levelSwitch">A switch allowing the pass-through minimum level to be changed at runtime.</param>
        /// <param name="theme">The theme to apply to the styled output. If not specified,
        /// uses <see cref="ThemePresets.Light"/>.</param>
        /// <returns>Configuration object allowing method chaining.</returns>
        public static LoggerConfiguration RichTextBox(
            this LoggerSinkConfiguration sinkConfiguration,
            RichTextBox richTextBoxControl,
            LogEventLevel minimumLogEventLevel = LogEventLevel.Verbose,
            string outputTemplate = DefaultRichTextBoxOutputTemplate,
            IFormatProvider? formatProvider = null,
            LoggingLevelSwitch? levelSwitch = null,
            Theme? theme = null)
        {
            var appliedTheme = theme ?? ThemePresets.Dark;
            richTextBoxControl.Clear();
            richTextBoxControl.ReadOnly = true;
            richTextBoxControl.ForeColor = appliedTheme.DefaultStyle.Foreground;
            richTextBoxControl.BackColor = appliedTheme.DefaultStyle.Background;

            var renderer = new TemplateRenderer(appliedTheme, outputTemplate, formatProvider);
            var sink = new RichTextBoxSink(richTextBoxControl, renderer);
            return sinkConfiguration.Sink(sink, minimumLogEventLevel, levelSwitch);
        }
    }
}