using Serilog.Sinks.RichTextBoxForms.Themes;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Serilog.Sinks.RichTextBoxForms.Rtf
{
    internal sealed class RtfBuilder : IRtfCanvas, IDisposable
    {
        private readonly StringBuilder _body = new();
        private readonly Dictionary<Color, int> _colorTable = new();
        private int _currentFgIndex;
        private int _currentBgIndex;
        private int _selectionStart;
        private int _selectionLength;
        private int _textLength;
        private Color _selectionColor;
        private Color _selectionBackColor;

        public RtfBuilder(Theme theme)
        {
            _selectionColor = theme.DefaultStyle.Foreground;
            _selectionBackColor = theme.DefaultStyle.Background;
            _currentFgIndex = RegisterColor(_selectionColor);
            _currentBgIndex = RegisterColor(_selectionBackColor);

            foreach (var colour in theme.Colors)
            {
                RegisterColor(colour);
            }
        }

        public int TextLength => _textLength;

        public int SelectionStart
        {
            get => _selectionStart;
            set => _selectionStart = value;
        }

        public int SelectionLength
        {
            get => _selectionLength;
            set => _selectionLength = value;
        }

        public Color SelectionColor
        {
            get => _selectionColor;
            set => _selectionColor = value;
        }

        public Color SelectionBackColor
        {
            get => _selectionBackColor;
            set => _selectionBackColor = value;
        }

        public void AppendText(string text)
        {
            EnsureColourSwitch();
            EscapeAndAppend(text);
            _textLength += text.Length;
        }

        public string Rtf => BuildDocument();

        private void EnsureColourSwitch()
        {
            var fgIdx = RegisterColor(_selectionColor);
            var bgIdx = RegisterColor(_selectionBackColor);

            if (fgIdx == _currentFgIndex && bgIdx == _currentBgIndex)
            {
                return;
            }

            if (fgIdx != _currentFgIndex)
            {
                _currentFgIndex = fgIdx;
                _body.Append(@"\cf").Append(fgIdx).Append(' ');
            }

            if (bgIdx != _currentBgIndex)
            {
                _currentBgIndex = bgIdx;
                _body.Append(@"\highlight").Append(bgIdx).Append(' ');
            }
        }

        private int RegisterColor(Color color)
        {
            if (_colorTable.TryGetValue(color, out var idx))
            {
                return idx;
            }

            idx = _colorTable.Count + 1;
            _colorTable.Add(color, idx);
            return idx;
        }

        private void EscapeAndAppend(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return;
            }

            var segmentStart = 0;
            for (var i = 0; i < value.Length; i++)
            {
                var ch = value[i];
                if (ch > 0x7f || ch == '\\' || ch == '{' || ch == '}' || ch == '\n' || ch == '\r')
                {
                    if (i > segmentStart)
                    {
                        _body.Append(value, segmentStart, i - segmentStart);
                    }

                    if (ch > 0x7f)
                    {
                        _body.Append("\\u").Append((int)ch).Append('?');
                    }
                    else if (ch == '\\' || ch == '{' || ch == '}')
                    {
                        _body.Append('\\').Append(ch);
                    }
                    else if (ch == '\n')
                    {
                        _body.Append("\\par\r\n");
                    }

                    segmentStart = i + 1;
                }
            }

            if (segmentStart < value.Length)
            {
                _body.Append(value, segmentStart, value.Length - segmentStart);
            }
        }

        private string BuildDocument()
        {
            var sb = StringBuilderCache.Acquire(_body.Length + 256);

            sb.Append(@"{\rtf1\ansi\deff0");
            sb.Append("{\\colortbl ;");

            foreach (var key in _colorTable.Keys)
            {
                sb.Append("\\red").Append(key.R)
                  .Append("\\green").Append(key.G)
                  .Append("\\blue").Append(key.B).Append(';');
            }

            sb.Append('}');
            sb.Append(_body);
            sb.Append('}');

            return StringBuilderCache.GetStringAndRelease(sb);
        }

        public void Dispose()
        {
            _body.Clear();
            _colorTable.Clear();
        }

        public void Clear()
        {
            _body.Clear();

            // Prevent unbounded memory retention by the underlying StringBuilder.
            // When very large log batches have been rendered, the StringBuilder can
            // grow to hundreds of kilobytes or even megabytes. Although Clear()
            // resets its length, the backing buffer (Capacity) is retained which
            // keeps the memory alive until the next Gen-2 GC collection, causing
            // memory bloat and noticeable GC pauses.
            const int MaxRetainedBuilderSize = 64 * 1024;
            if (_body.Capacity > MaxRetainedBuilderSize)
            {
                _body.Capacity = MaxRetainedBuilderSize;
            }

            _textLength = 0;
        }
    }
}