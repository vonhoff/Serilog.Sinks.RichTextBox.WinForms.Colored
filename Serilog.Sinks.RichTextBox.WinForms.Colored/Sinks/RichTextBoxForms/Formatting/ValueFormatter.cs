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

using Serilog.Data;
using Serilog.Events;
using Serilog.Sinks.RichTextBoxForms.Themes;
using System.Windows.Forms;

namespace Serilog.Sinks.RichTextBoxForms.Formatting
{
    public abstract class ValueFormatter : LogEventPropertyValueVisitor<ValueFormatterState, bool>
    {
        protected ValueFormatter(Theme theme)
        {
            Theme = theme;
        }

        public Theme Theme { get; set; }

        public void Format(LogEventPropertyValue value, RichTextBox richTextBox, string format, bool isLiteral)
        {
            Visit(new ValueFormatterState(richTextBox, format, true, isLiteral), value);
        }
    }
}