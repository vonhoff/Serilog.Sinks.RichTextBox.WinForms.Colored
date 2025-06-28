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

using Serilog.Sinks.RichTextBoxForms.Themes;
using System;

namespace Serilog.Sinks.RichTextBoxForms
{
    public class RichTextBoxSinkOptions
    {
        private const string DefaultOutputTemplate = "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}";
        private int _maxLogLines;

        /// <summary>
        /// Creates a new collection of options that control the behaviour and appearance of a
        /// <see cref="RichTextBoxSink"/> instance.
        /// </summary>
        /// <param name="theme">The colour theme applied when rendering individual message tokens.</param>
        /// <param name="autoScroll">When <c>true</c> (default) the target control scrolls automatically to the most recent log line.</param>
        /// <param name="maxLogLines">Maximum number of log events retained in the in-memory circular buffer and rendered in the control.</param>
        /// <param name="outputTemplate">Serilog output template that controls textual formatting of each log event.</param>
        /// <param name="formatProvider">Optional culture-specific or custom formatting provider used when rendering scalar values; <c>null</c> for the invariant culture.</param>
        public RichTextBoxSinkOptions(
            Theme theme,
            bool autoScroll = true,
            int maxLogLines = 256,
            string outputTemplate = DefaultOutputTemplate,
            IFormatProvider? formatProvider = null)
        {
            AutoScroll = autoScroll;
            Theme = theme;
            MaxLogLines = maxLogLines;
            OutputTemplate = outputTemplate;
            FormatProvider = formatProvider;
        }

        public bool AutoScroll { get; set; }

        public Theme Theme { get; }

        public int MaxLogLines
        {
            get => _maxLogLines;
            private set => _maxLogLines = value switch
            {
                < 1 => 1,
                > 2048 => 2048,
                _ => value
            };
        }

        public string OutputTemplate { get; }

        public IFormatProvider? FormatProvider { get; }
    }
}