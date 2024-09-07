﻿#region Copyright 2022 Simon Vonhoff & Contributors

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
using Serilog.Data;
using Serilog.Events;
using Serilog.Sinks.RichTextBoxForms.Themes;

namespace Serilog.Sinks.RichTextBoxForms.Formatting
{
    public abstract class ValueFormatter : LogEventPropertyValueVisitor<ValueFormatterState, bool>
    {
        public Theme Theme { get; set; }

        protected ValueFormatter(Theme theme)
        {
            Theme = theme;
        }

        public bool Format(LogEventPropertyValue value, RichTextBox richTextBox, string format, bool literalTopLevel = false)
        {
            return Visit(new ValueFormatterState(richTextBox, format, literalTopLevel), value);
        }
    }
}