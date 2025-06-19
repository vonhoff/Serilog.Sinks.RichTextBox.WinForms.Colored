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

        public RtfBuilder(Color defaultForeground, Color? defaultBackground = null)
        {
            _selectionColor = defaultForeground;
            _selectionBackColor = defaultBackground ?? Color.Transparent;
            _currentFgIndex = RegisterColor(_selectionColor);
            _currentBgIndex = RegisterColor(_selectionBackColor);
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

            idx = _colorTable.Count + 1; // RTF colour indexes start at 1.
            _colorTable.Add(color, idx);
            return idx;
        }

        private void EscapeAndAppend(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return;
            }

            foreach (var ch in value)
            {
                switch (ch)
                {
                    case '\\':
                        _body.Append("\\\\");
                        break;
                    case '{':
                        _body.Append("\\{");
                        break;
                    case '}':
                        _body.Append("\\}");
                        break;
                    case '\n':
                        _body.Append("\\par\r\n");
                        break;
                    case '\r':
                        // Skip – handled by \n branch.
                        break;
                    default:
                        if (ch <= 0x7f)
                        {
                            _body.Append(ch);
                        }
                        else
                        {
                            // Unicode escape.
                            _body.Append(@"\\u").Append(Convert.ToInt32(ch)).Append('?');
                        }
                        break;
                }
            }
        }

        private string BuildDocument()
        {
            var document = new StringBuilder(_body.Length + 256);
            document.Append(@"{\rtf1\ansi\deff0");
            document.Append("{\\colortbl ;");

            var ordered = new Color[_colorTable.Count + 1];
            foreach (var kvp in _colorTable)
            {
                ordered[kvp.Value] = kvp.Key;
            }

            for (var i = 1; i < ordered.Length; i++)
            {
                var c = ordered[i];
                document.Append("\\red").Append(c.R)
                         .Append("\\green").Append(c.G)
                         .Append("\\blue").Append(c.B).Append(';');
            }

            document.Append('}');
            document.Append(_body);
            document.Append('}');
            return document.ToString();
        }

        public void Dispose()
        {
            _body.Clear();
            _colorTable.Clear();
        }

        public void Clear()
        {
            _body.Clear();

            // Trim the internal buffer so we don't retain very large char arrays
            // on the Large Object Heap after a heavy burst of logging.
            const int MaxRetainedCapacity = 4096; // 8 KB
            if (_body.Capacity > MaxRetainedCapacity)
            {
                _body.Capacity = MaxRetainedCapacity;
            }
            _colorTable.Clear();

            // Re-register default colours so their indexes are 1 and 2 again.
            _currentFgIndex = RegisterColor(_selectionColor);
            _currentBgIndex = RegisterColor(_selectionBackColor);
            _textLength = 0;
        }
    }
}