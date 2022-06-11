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
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;
using Serilog.Core;
using Serilog.Events;
using Serilog.Sinks.RichTextBoxForms.Extensions;
using Serilog.Sinks.RichTextBoxForms.Rendering;

namespace Serilog.Sinks.RichTextBoxForms
{
    internal class RichTextBoxSink : ILogEventSink
    {
        private const int MessageBatchSize = 200;
        private const int MessageDequeueInterval = 25;
        private const int MessagePendingInterval = 50;
        private readonly ConcurrentQueue<LogEvent> _messageQueue = new();
        private readonly ITokenRenderer _renderer;
        private readonly RichTextBox _richTextBox;

        public RichTextBoxSink(RichTextBox richTextBox, ITokenRenderer renderer)
        {
            _richTextBox = richTextBox;
            _renderer = renderer;

            var messageWorker = new BackgroundWorker();
            messageWorker.WorkerSupportsCancellation = true;
            messageWorker.DoWork += ProcessMessages;
            messageWorker.RunWorkerAsync();
        }

        private enum ProcessState
        {
            Pending,
            Processing,
            Flushing
        }

        public void Emit(LogEvent logEvent)
        {
            _messageQueue.Enqueue(logEvent);
        }

        /// <summary>
        /// Processes all incoming log events until disposed or cancelled.
        /// </summary>
        /// <param name="sender">The object that raised this event.</param>
        /// <param name="e">Data for the event handler.</param>
        private void ProcessMessages(object? sender, DoWorkEventArgs e)
        {
            if (sender == null)
            {
                throw new ArgumentNullException(nameof(sender));
            }

            var messageBatch = 0;
            var processState = ProcessState.Pending;
            var stopwatch = new Stopwatch();
            var buffer = new RichTextBox { Font = _richTextBox.Font };
            var worker = (BackgroundWorker)sender;
            while (!worker.CancellationPending)
            {
                switch (processState)
                {
                    case ProcessState.Pending:
                    {
                        if (_messageQueue.IsEmpty)
                        {
                            Thread.Sleep(MessagePendingInterval);
                            break;
                        }

                        stopwatch.Restart();
                        processState = ProcessState.Processing;
                        break;
                    }
                    case ProcessState.Processing:
                    {
                        if (stopwatch.ElapsedMilliseconds >= MessageDequeueInterval || messageBatch >= MessageBatchSize)
                        {
                            if (messageBatch > 0)
                            {
                                processState = ProcessState.Flushing;
                                break;
                            }

                            processState = ProcessState.Pending;
                        }

                        if (_messageQueue.TryDequeue(out var logEvent))
                        {
                            _renderer.Render(logEvent, buffer);
                            stopwatch.Restart();
                            messageBatch++;
                        }
                        break;
                    }
                    case ProcessState.Flushing:
                    {
                        _richTextBox.AppendRtf(buffer.Rtf);
                        buffer.Clear();
                        stopwatch.Stop();
                        messageBatch = 0;
                        processState = ProcessState.Pending;
                        break;
                    }
                }
            }
        }
    }
}