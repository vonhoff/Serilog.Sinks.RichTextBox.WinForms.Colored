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