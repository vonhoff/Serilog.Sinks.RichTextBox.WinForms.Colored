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

using System.Collections.Generic;
using System.Drawing;

namespace Serilog.Sinks.RichTextBoxForms.Themes
{
    public static class ThemePresets
    {
        public static Theme Light { get; } = new(
            new Style { Foreground = Color.Black, Background = Color.White },
            new Dictionary<StyleToken, Style>
            {
                [StyleToken.Text] = new() { Foreground = Color.Black },
                [StyleToken.SecondaryText] = new() { Foreground = Color.Gray },
                [StyleToken.TertiaryText] = new() { Foreground = Color.DimGray },
                [StyleToken.Invalid] = new() { Foreground = Color.DarkGoldenrod },
                [StyleToken.Null] = new() { Foreground = Color.DarkBlue },
                [StyleToken.Name] = new() { Foreground = Color.DimGray },
                [StyleToken.String] = new() { Foreground = Color.DarkCyan },
                [StyleToken.Number] = new() { Foreground = Color.DarkMagenta },
                [StyleToken.Boolean] = new() { Foreground = Color.DarkBlue },
                [StyleToken.Scalar] = new() { Foreground = Color.DarkGreen },
                [StyleToken.LevelVerbose] = new() { Foreground = Color.DarkGray },
                [StyleToken.LevelDebug] = new() { Foreground = Color.DimGray },
                [StyleToken.LevelInformation] = new() { Foreground = Color.Black },
                [StyleToken.LevelWarning] = new() { Foreground = Color.DarkGoldenrod },
                [StyleToken.LevelError] = new() { Foreground = Color.White, Background = Color.Red },
                [StyleToken.LevelFatal] = new() { Foreground = Color.White, Background = Color.Red },
            });

        public static Theme Dark { get; } = new(
            new Style { Foreground = Color.White, Background = Color.Black },
            new Dictionary<StyleToken, Style>
            {
                [StyleToken.Text] = new() { Foreground = Color.White },
                [StyleToken.SecondaryText] = new() { Foreground = Color.Gray },
                [StyleToken.TertiaryText] = new() { Foreground = Color.DarkGray },
                [StyleToken.Invalid] = new() { Foreground = Color.Yellow },
                [StyleToken.Null] = new() { Foreground = Color.Blue },
                [StyleToken.Name] = new() { Foreground = Color.Gray },
                [StyleToken.String] = new() { Foreground = Color.Cyan },
                [StyleToken.Number] = new() { Foreground = Color.Magenta },
                [StyleToken.Boolean] = new() { Foreground = Color.Blue },
                [StyleToken.Scalar] = new() { Foreground = Color.Lime },
                [StyleToken.LevelVerbose] = new() { Foreground = Color.Gray },
                [StyleToken.LevelDebug] = new() { Foreground = Color.Gray },
                [StyleToken.LevelInformation] = new() { Foreground = Color.White },
                [StyleToken.LevelWarning] = new() { Foreground = Color.Yellow },
                [StyleToken.LevelError] = new() { Foreground = Color.White, Background = Color.Red },
                [StyleToken.LevelFatal] = new() { Foreground = Color.White, Background = Color.Red },
            });
    }
}