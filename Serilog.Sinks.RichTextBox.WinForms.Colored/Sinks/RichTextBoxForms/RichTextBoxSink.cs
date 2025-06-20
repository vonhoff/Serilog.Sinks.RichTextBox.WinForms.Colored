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

using Serilog.Core;
using Serilog.Events;
using Serilog.Sinks.RichTextBoxForms.Extensions;
using Serilog.Sinks.RichTextBoxForms.Rendering;
using Serilog.Sinks.RichTextBoxForms.Rtf;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Serilog.Sinks.RichTextBoxForms
{
    public class RichTextBoxSink : ILogEventSink, IDisposable
    {
        private readonly BlockingCollection<LogEvent> _messageQueue;
        private readonly RichTextBoxSinkOptions _options;
        private readonly ITokenRenderer _renderer;
        private readonly RichTextBox _richTextBox;
        private readonly CancellationTokenSource _tokenSource;
        private readonly Task _processingTask;

        public RichTextBoxSink(RichTextBox richTextBox, RichTextBoxSinkOptions options, ITokenRenderer? renderer = null)
        {
            _options = options;
            _richTextBox = richTextBox;
            _renderer = renderer ?? new TemplateRenderer(options.Theme, options.OutputTemplate, options.FormatProvider);
            _tokenSource = new CancellationTokenSource();
            _messageQueue = new BlockingCollection<LogEvent>(_options.QueueCapacity);

            richTextBox.Clear();
            richTextBox.ReadOnly = true;
            richTextBox.DetectUrls = false;
            richTextBox.ForeColor = options.Theme.DefaultStyle.Foreground;
            richTextBox.BackColor = options.Theme.DefaultStyle.Background;

            _processingTask = Task.Run(() => ProcessMessages(_tokenSource.Token), _tokenSource.Token);
        }

        public void Dispose()
        {
            _messageQueue.CompleteAdding();
            _processingTask.Wait();
            _tokenSource.Cancel();
            _tokenSource.Dispose();
            GC.SuppressFinalize(this);
        }

        public void Emit(LogEvent logEvent)
        {
            _messageQueue.Add(logEvent, _tokenSource.Token);
        }

        private void ProcessMessages(CancellationToken token)
        {
            var logEvents = new List<LogEvent>(_options.BatchSize);
            var builder = new RtfBuilder(_richTextBox.ForeColor, _richTextBox.BackColor);

            while (true)
            {
                LogEvent? nextEvent;

                if (_messageQueue.TryTake(out nextEvent!, _options.FlushInterval, token))
                {
                    logEvents.Add(nextEvent);
                    while (logEvents.Count < _options.BatchSize && _messageQueue.TryTake(out nextEvent))
                    {
                        logEvents.Add(nextEvent);
                    }
                }

                if (logEvents.Count == 0)
                {
                    if (_messageQueue.IsCompleted)
                    {
                        break;
                    }

                    continue;
                }

                foreach (var @event in logEvents)
                {
                    _renderer.Render(@event, builder);
                }

                _richTextBox.AppendRtf(builder.Rtf, _options.AutoScroll, _options.MaxLogLines);
                builder.Clear();
                logEvents.Clear();
            }
        }
    }
}