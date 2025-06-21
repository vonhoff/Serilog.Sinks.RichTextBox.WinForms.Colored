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

            // Register default colours first so that they get low palette indexes (1 and 2).
            _currentFgIndex = RegisterColor(_selectionColor);
            _currentBgIndex = RegisterColor(_selectionBackColor);

            // Pre-register every colour referenced by the theme
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

            int segmentStart = 0;
            for (int i = 0; i < value.Length; i++)
            {
                char ch = value[i];
                string? replacement = ch switch
                {
                    '\\' => "\\\\",
                    '{' => "\\{",
                    '}' => "\\}",
                    '\n' => "\\par\r\n",
                    '\r' => string.Empty,
                    _ => null
                };

                // Non-ASCII characters need to be escaped using their UTF-16 code unit.
                if (replacement is null && ch > 0x7f)
                {
                    if (i > segmentStart)
                    {
                        _body.Append(value, segmentStart, i - segmentStart);
                    }

                    _body.Append("\\u").Append((int)ch).Append('?');
                    segmentStart = i + 1;
                    continue;
                }

                if (replacement is not null)
                {
                    if (i > segmentStart)
                    {
                        _body.Append(value, segmentStart, i - segmentStart);
                    }

                    if (replacement.Length > 0)
                    {
                        _body.Append(replacement);
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
            var document = StringBuilderCache.Acquire(_body.Length + 256);
            document.Append(@"{\rtf1\ansi\deff0");
            document.Append("{\\colortbl ;");

            foreach (var key in _colorTable.Keys)
            {
                document.Append("\\red").Append(key.R)
                         .Append("\\green").Append(key.G)
                         .Append("\\blue").Append(key.B).Append(';');
            }

            document.Append('}');
            document.Append(_body);
            document.Append('}');
            var result = document.ToString();
            StringBuilderCache.Release(document);
            return result;
        }

        public void Dispose()
        {
            _body.Clear();
            _colorTable.Clear();
        }

        public void Clear()
        {
            _body.Clear();
            _textLength = 0;
        }
    }
}