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
using Serilog.Sinks.RichTextBoxForms.Collections;
using Serilog.Sinks.RichTextBoxForms.Extensions;
using Serilog.Sinks.RichTextBoxForms.Rendering;
using Serilog.Sinks.RichTextBoxForms.Rtf;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Serilog.Sinks.RichTextBoxForms
{
    public class RichTextBoxSink : ILogEventSink, IDisposable
    {
        private const double FlushIntervalMs = 1000.0 / 12.0;
        private readonly ConcurrentCircularBuffer<LogEvent> _buffer;
        private readonly AutoResetEvent _signal;
        private readonly RichTextBoxSinkOptions _options;
        private readonly ITokenRenderer _renderer;
        private readonly RichTextBox _richTextBox;
        private readonly CancellationTokenSource _tokenSource;
        private readonly Task _processingTask;
        private bool _disposed;
        private int _hasNewMessages;

        public RichTextBoxSink(RichTextBox richTextBox, RichTextBoxSinkOptions options, ITokenRenderer? renderer = null)
        {
            _options = options;
            _richTextBox = richTextBox;
            _renderer = renderer ?? new TemplateRenderer(options.Theme, options.OutputTemplate, options.FormatProvider);
            _tokenSource = new CancellationTokenSource();

            _buffer = new ConcurrentCircularBuffer<LogEvent>(options.MaxLogLines);
            _signal = new AutoResetEvent(false);
            _hasNewMessages = 0;

            richTextBox.Clear();
            richTextBox.ReadOnly = true;
            richTextBox.DetectUrls = false;
            richTextBox.ForeColor = options.Theme.DefaultStyle.Foreground;
            richTextBox.BackColor = options.Theme.DefaultStyle.Background;

            _processingTask = Task.Run(() => ProcessMessages(_tokenSource.Token));
        }

        public void Dispose()
        {
            if (_disposed) return;

            _disposed = true;
            _tokenSource.Cancel();
            _signal.Set();
            _processingTask.Wait();
            _signal.Dispose();
            _tokenSource.Dispose();
            GC.SuppressFinalize(this);
        }

        public void Emit(LogEvent logEvent)
        {
            _buffer.Add(logEvent);
            Interlocked.Exchange(ref _hasNewMessages, 1);
            _signal.Set();
        }

        public void Clear()
        {
            _buffer.Clear();
            Interlocked.Exchange(ref _hasNewMessages, 1);
            _signal.Set();
        }

        public void Restore()
        {
            _buffer.Restore();
            Interlocked.Exchange(ref _hasNewMessages, 1);
            _signal.Set();
        }

        private void ProcessMessages(CancellationToken token)
        {
            var builder = new RtfBuilder(_options.Theme);
            var snapshot = new System.Collections.Generic.List<LogEvent>(_options.MaxLogLines);
            var flushInterval = TimeSpan.FromMilliseconds(FlushIntervalMs);
            var lastFlush = DateTime.UtcNow;

            while (!token.IsCancellationRequested)
            {
                _signal.WaitOne();

                if (Interlocked.CompareExchange(ref _hasNewMessages, 0, 1) == 1)
                {
                    var now = DateTime.UtcNow;
                    var elapsed = now - lastFlush;
                    if (elapsed < flushInterval)
                    {
                        var remaining = flushInterval - elapsed;
                        if (remaining > TimeSpan.Zero)
                        {
                            Thread.Sleep(remaining);
                        }
                    }

                    Interlocked.Exchange(ref _hasNewMessages, 0);

                    _buffer.TakeSnapshot(snapshot);
                    builder.Clear();
                    foreach (var evt in snapshot)
                    {
                        _renderer.Render(evt, builder);
                    }
                    _richTextBox.SetRtf(builder.Rtf, _options.AutoScroll);
                    lastFlush = DateTime.UtcNow;
                }
            }
        }
    }
}