﻿#region Copyright 2025 Simon Vonhoff & Contributors

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

namespace Serilog.Sinks.RichTextBoxForms.Themes
{
    public static class ThemePresets
    {
        public static Theme Literate { get; } = new(
            new Style(ThemeColors.White, ThemeColors.Black),
            new Dictionary<StyleToken, Style>
            {
                [StyleToken.Text] = new(ThemeColors.White, ThemeColors.Black),
                [StyleToken.SecondaryText] = new(ThemeColors.Gray, ThemeColors.Black),
                [StyleToken.TertiaryText] = new(ThemeColors.Gray, ThemeColors.Black),
                [StyleToken.Invalid] = new(ThemeColors.Yellow, ThemeColors.Black),
                [StyleToken.Null] = new(ThemeColors.LightBlue, ThemeColors.Black),
                [StyleToken.Name] = new(ThemeColors.Gray, ThemeColors.Black),
                [StyleToken.String] = new(ThemeColors.Cyan, ThemeColors.Black),
                [StyleToken.Number] = new(ThemeColors.Magenta, ThemeColors.Black),
                [StyleToken.Boolean] = new(ThemeColors.LightBlue, ThemeColors.Black),
                [StyleToken.Scalar] = new(ThemeColors.Green, ThemeColors.Black),
                [StyleToken.LevelVerbose] = new(ThemeColors.Gray, ThemeColors.Black),
                [StyleToken.LevelDebug] = new(ThemeColors.Gray, ThemeColors.Black),
                [StyleToken.LevelInformation] = new(ThemeColors.White, ThemeColors.Black),
                [StyleToken.LevelWarning] = new(ThemeColors.Yellow, ThemeColors.Black),
                [StyleToken.LevelError] = new(ThemeColors.White, ThemeColors.Red),
                [StyleToken.LevelFatal] = new(ThemeColors.White, ThemeColors.Red)
            });

        public static Theme Grayscale { get; } = new(
            new Style(ThemeColors.White, ThemeColors.Black),
            new Dictionary<StyleToken, Style>
            {
                [StyleToken.Text] = new(ThemeColors.White, ThemeColors.Black),
                [StyleToken.SecondaryText] = new(ThemeColors.Gray, ThemeColors.Black),
                [StyleToken.TertiaryText] = new(ThemeColors.Gray, ThemeColors.Black),
                [StyleToken.Invalid] = new(ThemeColors.White, ThemeColors.DarkGray),
                [StyleToken.Null] = new(ThemeColors.White, ThemeColors.Black),
                [StyleToken.Name] = new(ThemeColors.Gray, ThemeColors.Black),
                [StyleToken.String] = new(ThemeColors.White, ThemeColors.Black),
                [StyleToken.Number] = new(ThemeColors.White, ThemeColors.Black),
                [StyleToken.Boolean] = new(ThemeColors.White, ThemeColors.Black),
                [StyleToken.Scalar] = new(ThemeColors.White, ThemeColors.Black),
                [StyleToken.LevelVerbose] = new(ThemeColors.Gray, ThemeColors.Black),
                [StyleToken.LevelDebug] = new(ThemeColors.Gray, ThemeColors.Black),
                [StyleToken.LevelInformation] = new(ThemeColors.White, ThemeColors.Black),
                [StyleToken.LevelWarning] = new(ThemeColors.White, ThemeColors.DarkGray),
                [StyleToken.LevelError] = new(ThemeColors.Black, ThemeColors.White),
                [StyleToken.LevelFatal] = new(ThemeColors.Black, ThemeColors.White)
            });

        public static Theme Colored { get; } = new(
            new Style(ThemeColors.Gray, ThemeColors.Black),
            new Dictionary<StyleToken, Style>
            {
                [StyleToken.Text] = new(ThemeColors.Gray, ThemeColors.Black),
                [StyleToken.SecondaryText] = new(ThemeColors.Gray, ThemeColors.Black),
                [StyleToken.TertiaryText] = new(ThemeColors.Gray, ThemeColors.Black),
                [StyleToken.Invalid] = new(ThemeColors.Yellow, ThemeColors.Black),
                [StyleToken.Null] = new(ThemeColors.White, ThemeColors.Black),
                [StyleToken.Name] = new(ThemeColors.White, ThemeColors.Black),
                [StyleToken.String] = new(ThemeColors.White, ThemeColors.Black),
                [StyleToken.Number] = new(ThemeColors.White, ThemeColors.Black),
                [StyleToken.Boolean] = new(ThemeColors.White, ThemeColors.Black),
                [StyleToken.Scalar] = new(ThemeColors.White, ThemeColors.Black),
                [StyleToken.LevelVerbose] = new(ThemeColors.Gray, ThemeColors.DarkGray),
                [StyleToken.LevelDebug] = new(ThemeColors.White, ThemeColors.DarkGray),
                [StyleToken.LevelInformation] = new(ThemeColors.White, ThemeColors.Blue),
                [StyleToken.LevelWarning] = new(ThemeColors.DarkGray, ThemeColors.Yellow),
                [StyleToken.LevelError] = new(ThemeColors.White, ThemeColors.Red),
                [StyleToken.LevelFatal] = new(ThemeColors.White, ThemeColors.Red)
            });

        public static Theme Luminous { get; } = new(
            new Style(ThemeColors.Black, ThemeColors.White),
            new Dictionary<StyleToken, Style>
            {
                [StyleToken.Text] = new(ThemeColors.Black, ThemeColors.White),
                [StyleToken.SecondaryText] = new(ThemeColors.DarkGray, ThemeColors.White),
                [StyleToken.TertiaryText] = new(ThemeColors.DarkGray, ThemeColors.White),
                [StyleToken.Invalid] = new(ThemeColors.White, ThemeColors.Red),
                [StyleToken.Null] = new(ThemeColors.DarkBlue, ThemeColors.White),
                [StyleToken.Name] = new(ThemeColors.DarkMagenta, ThemeColors.White),
                [StyleToken.String] = new(ThemeColors.DarkGreen, ThemeColors.White),
                [StyleToken.Number] = new(ThemeColors.DarkCyan, ThemeColors.White),
                [StyleToken.Boolean] = new(ThemeColors.DarkYellow, ThemeColors.White),
                [StyleToken.Scalar] = new(ThemeColors.DarkCyan, ThemeColors.White),
                [StyleToken.LevelVerbose] = new(ThemeColors.DarkGray, ThemeColors.White),
                [StyleToken.LevelDebug] = new(ThemeColors.DarkBlue, ThemeColors.White),
                [StyleToken.LevelInformation] = new(ThemeColors.DarkGreen, ThemeColors.White),
                [StyleToken.LevelWarning] = new(ThemeColors.Black, ThemeColors.Yellow),
                [StyleToken.LevelError] = new(ThemeColors.White, ThemeColors.Red),
                [StyleToken.LevelFatal] = new(ThemeColors.White, ThemeColors.Red)
            });
    }
}