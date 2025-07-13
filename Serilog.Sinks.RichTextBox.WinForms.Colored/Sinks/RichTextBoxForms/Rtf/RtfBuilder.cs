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

using Serilog.Sinks.RichTextBoxForms.Themes;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Serilog.Sinks.RichTextBoxForms.Rtf
{
    public sealed class RtfBuilder : IRtfCanvas, IDisposable
    {
        private readonly StringBuilder _body = new();
        private readonly StringBuilder _documentBuilder = new();
        private readonly Dictionary<Color, int> _colorTable = new();
        private int _currentFgIndex;
        private int _currentBgIndex;

        public RtfBuilder(Theme theme)
        {
            SelectionColor = theme.DefaultStyle.Foreground;
            SelectionBackColor = theme.DefaultStyle.Background;
            _currentFgIndex = RegisterColor(SelectionColor);
            _currentBgIndex = RegisterColor(SelectionBackColor);

            foreach (var color in theme.Colors)
            {
                RegisterColor(color);
            }
        }

        public int TextLength { get; private set; }

        public int SelectionStart { get; set; }

        public int SelectionLength { get; set; }

        public Color SelectionColor { get; set; }

        public Color SelectionBackColor { get; set; }

        public void AppendText(string text)
        {
            EnsureColorSwitch();
            EscapeAndAppend(text);
            TextLength += text.Length;
        }

        public string Rtf
        {
            get => BuildDocument();
        }

        private void EnsureColorSwitch()
        {
            var fgIdx = RegisterColor(SelectionColor);
            var bgIdx = RegisterColor(SelectionBackColor);

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
                if (ch <= 0x7f && ch != '\\' && ch != '{' && ch != '}' && ch != '\n' && ch != '\r')
                {
                    continue;
                }

                if (i > segmentStart)
                {
                    _body.Append(value, segmentStart, i - segmentStart);
                }

                if (ch > 0x7f)
                {
                    _body.Append("\\u").Append((int)ch).Append('?');
                }
                else
                {
                    switch (ch)
                    {
                        case '\\' or '{' or '}':
                            _body.Append('\\').Append(ch);
                            break;

                        case '\n':
                            _body.Append("\\par\r\n");
                            break;
                    }
                }

                segmentStart = i + 1;
            }

            if (segmentStart < value.Length)
            {
                _body.Append(value, segmentStart, value.Length - segmentStart);
            }
        }

        private string BuildDocument()
        {
            _documentBuilder.Clear();
            _documentBuilder.Append(@"{\rtf1\ansi\deff0");
            _documentBuilder.Append("{\\colortbl ;");
            foreach (var key in _colorTable.Keys)
            {
                _documentBuilder.Append("\\red").Append(key.R)
                    .Append("\\green").Append(key.G)
                    .Append("\\blue").Append(key.B).Append(';');
            }

            _documentBuilder.Append('}');
            _documentBuilder.Append(_body);
            _documentBuilder.Append('}');

            return _documentBuilder.ToString();
        }

        public void Dispose()
        {
            _body.Clear();
            _documentBuilder.Clear();
            _colorTable.Clear();
        }

        public void Clear()
        {
            _body.Clear();
            _documentBuilder.Clear();

            const int maxRetainedBuilderSize = 64 * 1024;
            if (_body.Capacity > maxRetainedBuilderSize)
            {
                _body.Capacity = maxRetainedBuilderSize;
            }

            if (_documentBuilder.Capacity > maxRetainedBuilderSize)
            {
                _documentBuilder.Capacity = maxRetainedBuilderSize;
            }

            TextLength = 0;
        }
    }
}