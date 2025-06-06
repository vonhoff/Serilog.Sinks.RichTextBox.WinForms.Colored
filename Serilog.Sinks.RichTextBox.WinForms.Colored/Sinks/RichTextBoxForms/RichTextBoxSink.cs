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

using Serilog.Core;
using Serilog.Events;
using Serilog.Sinks.RichTextBoxForms.Extensions;
using Serilog.Sinks.RichTextBoxForms.Rendering;
using System;
using System.Collections.Concurrent;
using System.Threading;
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

        public RichTextBoxSink(RichTextBox richTextBox, RichTextBoxSinkOptions options, ITokenRenderer? renderer = null)
        {
            _options = options;
            _richTextBox = richTextBox;
            _renderer = renderer ??
                        new TemplateRenderer(options.AppliedTheme, options.OutputTemplate, options.FormatProvider);
            _tokenSource = new CancellationTokenSource();
            _messageQueue = new BlockingCollection<LogEvent>();

            richTextBox.Clear();
            richTextBox.ReadOnly = true;
            richTextBox.DetectUrls = false;
            richTextBox.ForeColor = options.AppliedTheme.DefaultStyle.Foreground;
            richTextBox.BackColor = options.AppliedTheme.DefaultStyle.Background;

            ThreadPool.QueueUserWorkItem(ProcessMessages, _tokenSource.Token);
        }

        public void Dispose()
        {
            _messageQueue.Dispose();
            _tokenSource.Cancel();
            _tokenSource.Dispose();
            GC.SuppressFinalize(this);
        }

        public void Emit(LogEvent logEvent)
        {
            _messageQueue.Add(logEvent);
        }

        private void ProcessMessages(object? obj)
        {
            var token = (CancellationToken)(obj ?? throw new ArgumentNullException(nameof(obj)));
            var messageBatch = 1;
            var buffer = new RichTextBox 
            { 
                Font = _richTextBox.Font,
                DetectUrls = false,
                ForeColor = _richTextBox.ForeColor,
                BackColor = _richTextBox.BackColor
            };
            try
            {
                while (true)
                {
                    if (_messageQueue.TryTake(out var logEvent, _options.MessagePendingInterval, token))
                    {
                        _renderer.Render(logEvent, buffer);
                        while (messageBatch < _options.MessageBatchSize && _messageQueue.TryTake(out logEvent, 0, token))
                        {
                            _renderer.Render(logEvent, buffer);
                            messageBatch++;
                        }

                        _richTextBox.AppendRtf(buffer.Rtf!, _options.AutoScroll, _options.MaxLogLines);
                        buffer.Clear();
                        messageBatch = 1;
                    }
                }
            }
            catch (ObjectDisposedException)
            {
            }
            catch (OperationCanceledException)
            {
            }
        }
    }
}