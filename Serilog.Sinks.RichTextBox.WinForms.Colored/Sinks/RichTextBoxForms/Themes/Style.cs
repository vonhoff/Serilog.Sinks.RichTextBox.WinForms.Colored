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

using System.Drawing;

namespace Serilog.Sinks.RichTextBoxForms.Themes
{
    /// <summary>
    /// Styling applied for Foreground and Background colors.
    /// </summary>
    public readonly struct Style
    {
        public Style(Color foreground)
        {
            Background = default;
            Foreground = foreground;
        }

        public Style(Color foreground, Color background)
        {
            Background = background;
            Foreground = foreground;
        }

        /// <summary>
        /// The background color to apply
        /// </summary>
        public Color Background { get; }

        /// <summary>
        /// The foreground color to apply.
        /// </summary>
        public Color Foreground { get; }
    }
}