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
        private int _batchSize;
        private int _flushInterval;
        private int _queueCapacity;

        public RichTextBoxSinkOptions(
            Theme appliedTheme,
            bool autoScroll = true,
            int maxLogLines = 200,
            int batchSize = 100,
            int flushInterval = 50,
            int queueCapacity = 1000,
            string outputTemplate = DefaultOutputTemplate,
            IFormatProvider? formatProvider = null)
        {
            AutoScroll = autoScroll;
            AppliedTheme = appliedTheme;
            MaxLogLines = maxLogLines;
            OutputTemplate = outputTemplate;
            FormatProvider = formatProvider;
            BatchSize = batchSize;
            FlushInterval = flushInterval;
            QueueCapacity = queueCapacity;
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

        public int BatchSize
        {
            get => _batchSize;
            set => _batchSize = value > 0 ? value : 250;
        }

        public int FlushInterval
        {
            get => _flushInterval;
            set => _flushInterval = value > 0 ? value : 10;
        }

        public int QueueCapacity
        {
            get => _queueCapacity;
            set => _queueCapacity = value > 0 ? value : 1000;
        }
    }
}