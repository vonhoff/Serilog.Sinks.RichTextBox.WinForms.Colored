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
        ///     A theme with a light background and contrasting colors.
        /// </summary>
        public static Theme Light { get; } = new(
            new Style(ThemeColors.ThunderGray, ThemeColors.IronGray),
            new Dictionary<StyleToken, Style>
            {
                [StyleToken.Text] = new(ThemeColors.ThunderGray),
                [StyleToken.SecondaryText] = new(ThemeColors.ShipGray),
                [StyleToken.TertiaryText] = new(ThemeColors.AbbeyGray),
                [StyleToken.Invalid] = new(ThemeColors.CardinalRed),
                [StyleToken.Null] = new(ThemeColors.FunBlue),
                [StyleToken.Name] = new(ThemeColors.AbbeyGray),
                [StyleToken.String] = new(ThemeColors.FunBlue),
                [StyleToken.Number] = new(ThemeColors.EminencePurple),
                [StyleToken.Boolean] = new(ThemeColors.FunBlue),
                [StyleToken.Scalar] = new(ThemeColors.ForestGreen),
                [StyleToken.LevelVerbose] = new(ThemeColors.AbbeyGray),
                [StyleToken.LevelDebug] = new(ThemeColors.ShipGray),
                [StyleToken.LevelInformation] = new(ThemeColors.ThunderGray),
                [StyleToken.LevelWarning] = new(ThemeColors.TuscanyOrange),
                [StyleToken.LevelError] = new(ThemeColors.IronGray, ThemeColors.CardinalRed),
                [StyleToken.LevelFatal] = new(ThemeColors.IronGray, ThemeColors.CrimsonRed)
            });

        /// <summary>
        ///     Styled to replicate the default theme of Serilog.Sinks.Console; This is the default when no theme is specified.
        /// </summary>
        public static Theme Dark { get; } = new(
            new Style(ThemeColors.IronGray, ThemeColors.ThunderGray),
            new Dictionary<StyleToken, Style>
            {
                [StyleToken.Text] = new(ThemeColors.IronGray),
                [StyleToken.SecondaryText] = new(ThemeColors.SilverSand),
                [StyleToken.TertiaryText] = new(ThemeColors.OsloGray),
                [StyleToken.Invalid] = new(ThemeColors.HollywoodPink),
                [StyleToken.Null] = new(ThemeColors.ForestGreen),
                [StyleToken.Name] = new(ThemeColors.SilverSand),
                [StyleToken.String] = new(ThemeColors.AppleGreen),
                [StyleToken.Number] = new(ThemeColors.ForestGreen),
                [StyleToken.Boolean] = new(ThemeColors.EminencePurple),
                [StyleToken.Scalar] = new(ThemeColors.CeruleanBlue),
                [StyleToken.LevelVerbose] = new(ThemeColors.SilverSand),
                [StyleToken.LevelDebug] = new(ThemeColors.SilverSand),
                [StyleToken.LevelInformation] = new(ThemeColors.IronGray),
                [StyleToken.LevelWarning] = new(ThemeColors.ButtercupYellow),
                [StyleToken.LevelError] = new(ThemeColors.IronGray, ThemeColors.CardinalRed),
                [StyleToken.LevelFatal] = new(ThemeColors.IronGray, ThemeColors.CrimsonRed)
            });
    }
}