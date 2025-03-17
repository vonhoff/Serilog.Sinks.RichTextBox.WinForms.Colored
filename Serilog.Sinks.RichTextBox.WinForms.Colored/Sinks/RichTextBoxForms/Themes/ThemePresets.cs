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
        /// <summary>
        /// A theme with a light background and contrasting colors.
        /// </summary>
        public static Theme Light { get; } = new(
            new Style(Color.Black, Color.White),
            new Dictionary<StyleToken, Style>
            {
                [StyleToken.Text] = new(Color.Black),
                [StyleToken.SecondaryText] = new(Color.Gray),
                [StyleToken.TertiaryText] = new(Color.DimGray),
                [StyleToken.Invalid] = new(Color.Goldenrod),
                [StyleToken.Null] = new(Color.DarkBlue),
                [StyleToken.Name] = new(Color.DimGray),
                [StyleToken.String] = new(Color.DarkCyan),
                [StyleToken.Number] = new(Color.DarkMagenta),
                [StyleToken.Boolean] = new(Color.DarkBlue),
                [StyleToken.Scalar] = new(Color.DarkGreen),
                [StyleToken.LevelVerbose] = new(Color.DarkGray),
                [StyleToken.LevelDebug] = new(Color.DimGray),
                [StyleToken.LevelInformation] = new(Color.Black),
                [StyleToken.LevelWarning] = new(Color.DarkGoldenrod),
                [StyleToken.LevelError] = new(Color.White, Color.Red),
                [StyleToken.LevelFatal] = new(Color.White, Color.Red)
            });

        /// <summary>
        /// Styled to replicate the default theme of Serilog.Sinks.Console; This is the default when no theme is specified.
        /// </summary>
        public static Theme Dark { get; } = new(
            new Style(Color.White, Color.Black),
            new Dictionary<StyleToken, Style>
            {
                [StyleToken.Text] = new(Color.White),
                [StyleToken.SecondaryText] = new(Color.Gray),
                [StyleToken.TertiaryText] = new(Color.DarkGray),
                [StyleToken.Invalid] = new(Color.Yellow),
                [StyleToken.Null] = new(Color.Blue),
                [StyleToken.Name] = new(Color.Gray),
                [StyleToken.String] = new(Color.Cyan),
                [StyleToken.Number] = new(Color.Magenta),
                [StyleToken.Boolean] = new(Color.Blue),
                [StyleToken.Scalar] = new(Color.Lime),
                [StyleToken.LevelVerbose] = new(Color.Gray),
                [StyleToken.LevelDebug] = new(Color.Gray),
                [StyleToken.LevelInformation] = new(Color.White),
                [StyleToken.LevelWarning] = new(Color.Yellow),
                [StyleToken.LevelError] = new(Color.White, Color.Red),
                [StyleToken.LevelFatal] = new(Color.White, Color.Red)
            });
    }
}