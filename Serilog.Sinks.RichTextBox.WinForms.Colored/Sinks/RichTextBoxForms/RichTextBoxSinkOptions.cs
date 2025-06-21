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
        private int _queueCapacity;

        public RichTextBoxSinkOptions(
            Theme theme,
            bool autoScroll = true,
            int maxLogLines = 512,
            int batchSize = 256,
            int queueCapacity = 5000,
            string outputTemplate = DefaultOutputTemplate,
            IFormatProvider? formatProvider = null)
        {
            AutoScroll = autoScroll;
            Theme = theme;
            MaxLogLines = maxLogLines;
            OutputTemplate = outputTemplate;
            FormatProvider = formatProvider;
            BatchSize = batchSize;
            QueueCapacity = queueCapacity;
        }

        public bool AutoScroll { get; set; }

        public Theme Theme { get; set; }

        public int MaxLogLines
        {
            get => _maxLogLines;
            set => _maxLogLines = value > 1 ? value : 512;
        }

        public int BatchSize
        {
            get => _batchSize;
            set => _batchSize = value > 50 ? value : 256;
        }

        public int QueueCapacity
        {
            get => _queueCapacity;
            set => _queueCapacity = value > 50 ? value : 1000;
        }

        public string OutputTemplate { get; set; }

        public IFormatProvider? FormatProvider { get; set; }
    }
}