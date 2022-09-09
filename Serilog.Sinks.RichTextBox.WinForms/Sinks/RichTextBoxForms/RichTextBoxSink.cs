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
        private readonly int _messagePendingInterval;
        private readonly int _messageBatchSize;
        private readonly ConcurrentQueue<LogEvent> _messageQueue = new();
        private readonly ITokenRenderer _renderer;
        private readonly RichTextBox _richTextBox;

        public RichTextBoxSink(RichTextBox richTextBox, ITokenRenderer renderer,
            int messageBatchSize, int messagePendingInterval)
        {
            _messageBatchSize = messageBatchSize > 3 ? messageBatchSize : 3;
            _messagePendingInterval = messagePendingInterval > 0 ? messagePendingInterval : 1;
            _richTextBox = richTextBox;
            _renderer = renderer;

            var messageWorker = new BackgroundWorker();
            messageWorker.DoWork += ProcessMessages;
            messageWorker.RunWorkerAsync();
        }

        public void Emit(LogEvent logEvent)
        {
            _messageQueue.Enqueue(logEvent);
        }

        /// <summary>
        /// Processes all incoming log events until disposed.
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
            var buffer = new RichTextBox { Font = _richTextBox.Font };
            while (true)
            {
                while (_messageQueue.IsEmpty)
                {
                    Thread.Sleep(_messagePendingInterval);
                }

                while (_messageQueue.TryDequeue(out var logEvent) && messageBatch++ <= _messageBatchSize)
                {
                    _renderer.Render(logEvent, buffer);
                }

                _richTextBox.AppendRtf(buffer.Rtf);
                buffer.Clear();
                messageBatch = 0;
            }
        }
    }
}