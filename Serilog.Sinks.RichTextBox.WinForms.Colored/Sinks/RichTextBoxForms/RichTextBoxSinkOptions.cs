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
        ///     Settings for the RichTextBoxSink
        /// </summary>
        /// <param name="appliedTheme">The theme to apply.</param>
        /// <param name="autoScroll">Auto-scroll to bottom for new messages.</param>
        /// <param name="maxLogLines">Maximum number of lines to keep.</param>
        /// <param name="outputTemplate">Message format.</param>
        /// <param name="formatProvider">Culture-specific formatting (null for default).</param>
        public RichTextBoxSinkOptions(
            Theme appliedTheme,
            bool autoScroll = true,
            int maxLogLines = 250,
            string outputTemplate = DefaultOutputTemplate,
            IFormatProvider? formatProvider = null)
        {
            AutoScroll = autoScroll;
            AppliedTheme = appliedTheme;
            MaxLogLines = maxLogLines;
            OutputTemplate = outputTemplate;
            FormatProvider = formatProvider;
        }

        public bool AutoScroll { get; set; }

        public Theme AppliedTheme { get; set; }

        public int MaxLogLines
        {
            get => _maxLogLines;
            set => _maxLogLines = value > 0 ? value : 250;
        }

        public string OutputTemplate { get; set; }

        public IFormatProvider? FormatProvider { get; set; }
    }
}