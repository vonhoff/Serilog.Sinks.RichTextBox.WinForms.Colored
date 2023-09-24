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
    internal class RichTextBoxSink : ILogEventSink, IDisposable
    {
        private readonly int _messagePendingInterval;
        private readonly int _messageBatchSize;
        private readonly ConcurrentQueue<LogEvent> _messageQueue = new();
        private readonly ITokenRenderer _renderer;
        private readonly RichTextBox _richTextBox;
        private readonly CancellationTokenSource _tokenSource;
        private readonly bool _autoScroll;

        public RichTextBoxSink(RichTextBox richTextBox, ITokenRenderer renderer, int messageBatchSize, int messagePendingInterval, bool autoScroll)
        {
            _messageBatchSize = messageBatchSize > 3 ? messageBatchSize : 3;
            _messagePendingInterval = messagePendingInterval > 0 ? messagePendingInterval : 1;
            _richTextBox = richTextBox;
            _renderer = renderer;
            _autoScroll = autoScroll;
            _tokenSource = new CancellationTokenSource();
            ThreadPool.QueueUserWorkItem(ProcessMessages, _tokenSource.Token);
        }

        public void Emit(LogEvent logEvent)
        {
            _messageQueue.Enqueue(logEvent);
        }

        /// <summary>
        /// Processes all incoming log events until disposed.
        /// </summary>
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
                        Thread.Sleep(_messagePendingInterval);
                    }

                    while (_messageQueue.TryDequeue(out var logEvent) && messageBatch++ <= _messageBatchSize)
                    {
                        _renderer.Render(logEvent, buffer);
                    }

                    _richTextBox.AppendRtf(buffer.Rtf, _autoScroll);
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

        public void Dispose()
        {
            _tokenSource.Cancel();
            _tokenSource.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}