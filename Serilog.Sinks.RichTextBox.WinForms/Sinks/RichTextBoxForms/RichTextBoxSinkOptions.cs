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
using Serilog.Core;
using Serilog.Events;
using Serilog.Sinks.RichTextBoxForms.Themes;

namespace Serilog.Sinks.RichTextBoxForms
{
    public class RichTextBoxSinkOptions
    {
        private int _messageBatchSize;
        private int _messagePendingInterval;
        private int _maxLogLines;
        private const string DefaultOutputTemplate = "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}";

        /// <summary>
        /// Settings for the RichTextBoxSink
        /// </summary>
        /// <param name="appliedTheme">The theme to apply.</param>
        /// <param name="messageBatchSize">Max messages per batch for printing.</param>
        /// <param name="messagePendingInterval">Duration to hold incoming messages.</param>
        /// <param name="autoScroll">Auto-scroll to bottom for new messages.</param>
        /// <param name="maxLogLines">Maximum number of lines to keep.</param>
        /// <param name="outputTemplate">Message format (default: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}").</param>
        /// <param name="formatProvider">Culture-specific formatting (null for default).</param>
        public RichTextBoxSinkOptions(
            Theme appliedTheme,
            int messageBatchSize,
            int messagePendingInterval,
            bool autoScroll,
            int maxLogLines,
            string outputTemplate = DefaultOutputTemplate,
            IFormatProvider? formatProvider = null)
        {
            MessageBatchSize = messageBatchSize;
            MessagePendingInterval = messagePendingInterval;
            AutoScroll = autoScroll;
            AppliedTheme = appliedTheme;
            MaxLogLines = maxLogLines;
            OutputTemplate = outputTemplate;
            FormatProvider = formatProvider;
        }

        public bool AutoScroll { get; set; }

        public Theme AppliedTheme { get; set; }

        public int MessageBatchSize
        {
            get => _messageBatchSize;
            set => _messageBatchSize = value > 3 ? value : 3;
        }

        public int MessagePendingInterval
        {
            get => _messagePendingInterval;
            set => _messagePendingInterval = value > 0 ? value : 1;
        }

        public int MaxLogLines
        {
            get => _maxLogLines;
            set => _maxLogLines = value > 0 ? value : int.MaxValue;
        }

        public string OutputTemplate { get; set; }

        public IFormatProvider? FormatProvider { get; set; }
    }
}
