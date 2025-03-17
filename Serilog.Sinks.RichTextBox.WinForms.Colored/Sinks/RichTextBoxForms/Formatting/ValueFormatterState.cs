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

using System.Windows.Forms;

namespace Serilog.Sinks.RichTextBoxForms.Formatting
{
    public readonly struct ValueFormatterState
    {
        public ValueFormatterState(RichTextBox richTextBox)
        {
            RichTextBox = richTextBox;
            Format = string.Empty;
            IsTopLevel = false;
        }

        public ValueFormatterState(RichTextBox richTextBox, string format, bool isTopLevel)
        {
            RichTextBox = richTextBox;
            Format = format;
            IsTopLevel = isTopLevel;
        }

        public string Format { get; }
        public bool IsTopLevel { get; }
        public RichTextBox RichTextBox { get; }

        public ValueFormatterState Next()
        {
            return new ValueFormatterState(RichTextBox);
        }
    }
}