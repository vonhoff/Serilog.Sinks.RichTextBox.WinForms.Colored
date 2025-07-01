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

using System.Text;

namespace Serilog.Sinks.RichTextBoxForms.Formatting
{
    public static class TextFormatter
    {
        private static readonly StringBuilder FormatterBuilder = new();

        public static string Format(string value, string? format)
        {
            if (string.IsNullOrEmpty(format) || string.IsNullOrEmpty(value))
            {
                return value;
            }

            var first = format![0];
            return first switch
            {
                'u' => value.ToUpperInvariant(),
                'w' => value.ToLowerInvariant(),
                't' => FormatTitleCase(value),
                _ => value
            };
        }

        private static string FormatTitleCase(string value)
        {
            FormatterBuilder.Clear();
            FormatterBuilder.Append(char.ToUpperInvariant(value[0]));

            if (value.Length > 1)
            {
                FormatterBuilder.Append(value.Substring(1).ToLowerInvariant());
            }

            return FormatterBuilder.ToString();
        }
    }
}