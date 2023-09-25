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
using System.Collections.Concurrent;
using System.Threading;
using System.Windows.Forms;
using Serilog.Core;
using Serilog.Events;
using Serilog.Sinks.RichTextBoxForms.Extensions;
using Serilog.Sinks.RichTextBoxForms.Rendering;

namespace Serilog.Sinks.RichTextBoxForms
{
    public class RichTextBoxSink : ILogEventSink, IDisposable
    {
        private readonly ConcurrentQueue<LogEvent> _messageQueue = new();
        private readonly RichTextBoxSinkOptions _options;
        private readonly ITokenRenderer _renderer;
        private readonly RichTextBox _richTextBox;
        private readonly CancellationTokenSource _tokenSource;

        public RichTextBoxSink(RichTextBox richTextBox, RichTextBoxSinkOptions options, ITokenRenderer? renderer = null)
        {
            _options = options;
            _richTextBox = richTextBox;
            _renderer = renderer ?? new TemplateRenderer(options.AppliedTheme);
            _tokenSource = new CancellationTokenSource();

            richTextBox.Clear();
            richTextBox.ReadOnly = true;
            richTextBox.ForeColor = options.AppliedTheme.DefaultStyle.Foreground;
            richTextBox.BackColor = options.AppliedTheme.DefaultStyle.Background;

            ThreadPool.QueueUserWorkItem(ProcessMessages, _tokenSource.Token);
        }

        public void Dispose()
        {
            _tokenSource.Cancel();
            _tokenSource.Dispose();
            GC.SuppressFinalize(this);
        }

        public void Emit(LogEvent logEvent)
        {
            _messageQueue.Enqueue(logEvent);
        }

        private void ProcessMessages(object? obj)
        {
            var token = (CancellationToken)(obj ?? throw new ArgumentNullException(nameof(obj)));
            var messageBatch = 0;
            var buffer = new RichTextBox { Font = _richTextBox.Font };
            try
            {
                while (true)
                {
                    while (_messageQueue.IsEmpty)
                    {
                        token.ThrowIfCancellationRequested();
                        Thread.Sleep(_options.MessagePendingInterval);
                    }

                    while (_messageQueue.TryDequeue(out var logEvent) && messageBatch++ <= _options.MessageBatchSize)
                    {
                        _renderer.Render(logEvent, buffer);
                    }

                    _richTextBox.AppendRtf(buffer.Rtf, _options.AutoScroll);
                    buffer.Clear();
                    messageBatch = 0;
                }
            }
            catch (ObjectDisposedException)
            {
                _messageQueue.Clear();
            }
            catch (OperationCanceledException)
            {
            }
        }
    }
}