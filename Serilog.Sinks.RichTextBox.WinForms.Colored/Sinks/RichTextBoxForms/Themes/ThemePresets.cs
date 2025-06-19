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
        public static Theme Literate { get; } = new(
            new Style(ThemeColors.White, ThemeColors.Black),
            new Dictionary<StyleToken, Style>
            {
                [StyleToken.Text] = new(ThemeColors.White),
                [StyleToken.SecondaryText] = new(ThemeColors.Gray),
                [StyleToken.TertiaryText] = new(ThemeColors.Gray),
                [StyleToken.Invalid] = new(ThemeColors.Yellow),
                [StyleToken.Null] = new(ThemeColors.LightBlue),
                [StyleToken.Name] = new(ThemeColors.Gray),
                [StyleToken.String] = new(ThemeColors.Cyan),
                [StyleToken.Number] = new(ThemeColors.Magenta),
                [StyleToken.Boolean] = new(ThemeColors.LightBlue),
                [StyleToken.Scalar] = new(ThemeColors.Green),
                [StyleToken.LevelVerbose] = new(ThemeColors.Gray),
                [StyleToken.LevelDebug] = new(ThemeColors.Gray),
                [StyleToken.LevelInformation] = new(ThemeColors.White),
                [StyleToken.LevelWarning] = new(ThemeColors.Yellow),
                [StyleToken.LevelError] = new(ThemeColors.White, ThemeColors.Red),
                [StyleToken.LevelFatal] = new(ThemeColors.White, ThemeColors.Red),
            });

        public static Theme Grayscale { get; } = new(
            new Style(ThemeColors.White, ThemeColors.Black),
            new Dictionary<StyleToken, Style>
            {
                [StyleToken.Text] = new(ThemeColors.White),
                [StyleToken.SecondaryText] = new(ThemeColors.Gray),
                [StyleToken.TertiaryText] = new(ThemeColors.DarkGray),
                [StyleToken.Invalid] = new(ThemeColors.White, ThemeColors.DarkGray),
                [StyleToken.Null] = new(ThemeColors.White),
                [StyleToken.Name] = new(ThemeColors.Gray),
                [StyleToken.String] = new(ThemeColors.White),
                [StyleToken.Number] = new(ThemeColors.White),
                [StyleToken.Boolean] = new(ThemeColors.White),
                [StyleToken.Scalar] = new(ThemeColors.White),
                [StyleToken.LevelVerbose] = new(ThemeColors.DarkGray),
                [StyleToken.LevelDebug] = new(ThemeColors.DarkGray),
                [StyleToken.LevelInformation] = new(ThemeColors.White),
                [StyleToken.LevelWarning] = new(ThemeColors.White, ThemeColors.DarkGray),
                [StyleToken.LevelError] = new(ThemeColors.Black, ThemeColors.White),
                [StyleToken.LevelFatal] = new(ThemeColors.Black, ThemeColors.White),
            });

        public static Theme Colored { get; } = new(
            new Style(ThemeColors.Gray, ThemeColors.Black),
            new Dictionary<StyleToken, Style>
            {
                [StyleToken.Text] = new(ThemeColors.Gray),
                [StyleToken.SecondaryText] = new(ThemeColors.Gray),
                [StyleToken.TertiaryText] = new(ThemeColors.Gray),
                [StyleToken.Invalid] = new(ThemeColors.Yellow),
                [StyleToken.Null] = new(ThemeColors.White),
                [StyleToken.Name] = new(ThemeColors.White),
                [StyleToken.String] = new(ThemeColors.White),
                [StyleToken.Number] = new(ThemeColors.White),
                [StyleToken.Boolean] = new(ThemeColors.White),
                [StyleToken.Scalar] = new(ThemeColors.White),
                [StyleToken.LevelVerbose] = new(ThemeColors.Gray, ThemeColors.DarkGray),
                [StyleToken.LevelDebug] = new(ThemeColors.White, ThemeColors.DarkGray),
                [StyleToken.LevelInformation] = new(ThemeColors.White, ThemeColors.Blue),
                [StyleToken.LevelWarning] = new(ThemeColors.DarkGray, ThemeColors.Yellow),
                [StyleToken.LevelError] = new(ThemeColors.White, ThemeColors.Red),
                [StyleToken.LevelFatal] = new(ThemeColors.White, ThemeColors.Red),
            });
    }
}