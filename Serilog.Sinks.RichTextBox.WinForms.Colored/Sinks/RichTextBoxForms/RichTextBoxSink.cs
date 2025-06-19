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
using Serilog.Sinks.RichTextBoxForms.Rtf;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;

namespace Serilog.Sinks.RichTextBoxForms
{
    public class RichTextBoxSink : ILogEventSink, IDisposable
    {
        // Internal tuning constants – change here for future optimisation.
        private const int BatchSize = 250; // maximum events per flush
        private const int FlushIntervalMs = 50; // maximum time between flushes
        private const int QueueCapacity = 10_000; // maximum events waiting to be rendered
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
            // Bounded queue – producers will block when capacity is reached, preventing memory growth and UI hangs.
            _messageQueue = new BlockingCollection<LogEvent>(QueueCapacity);

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
            // Block when the queue is saturated so that no messages are lost.
            // Honour cancellation so Dispose() can interrupt blocked producers.
            _messageQueue.Add(logEvent, _tokenSource.Token);
        }

        private void ProcessMessages(object? obj)
        {
            var token = (CancellationToken)(obj ?? throw new ArgumentNullException(nameof(obj)));
            var logEvents = new List<LogEvent>(BatchSize);
            var builder = new RtfBuilder(_richTextBox.ForeColor, _richTextBox.BackColor);
            var lastFlushTick = Environment.TickCount;

            try
            {
                while (true)
                {
                    // Always wait for at least one event so we block when idle.
                    var logEvent = _messageQueue.Take(token);
                    logEvents.Add(logEvent);

                    // Drain immediately available events up to BatchSize.
                    while (logEvents.Count < BatchSize && _messageQueue.TryTake(out logEvent))
                    {
                        logEvents.Add(logEvent);
                    }

                    var now = Environment.TickCount;
                    var elapsed = unchecked(now - lastFlushTick);

                    if (logEvents.Count >= BatchSize || elapsed >= FlushIntervalMs)
                    {
                        foreach (var @event in logEvents)
                        {
                            _renderer.Render(@event, builder);
                        }

                        _richTextBox.AppendRtf(builder.Rtf, _options.AutoScroll, _options.MaxLogLines);
                        builder.Clear();
                        logEvents.Clear();
                        lastFlushTick = now;
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