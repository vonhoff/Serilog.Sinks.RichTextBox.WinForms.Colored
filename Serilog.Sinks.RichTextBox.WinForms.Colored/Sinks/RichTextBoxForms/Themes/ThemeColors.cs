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
    public static class ThemeColors
    {
        // Error and warning colors
        public static readonly Color CrimsonRed = ColorTranslator.FromHtml("#FF1A1A");
        public static readonly Color CardinalRed = ColorTranslator.FromHtml("#FF3333");
        public static readonly Color ButtercupYellow = ColorTranslator.FromHtml("#FFD700");
        public static readonly Color TuscanyOrange = ColorTranslator.FromHtml("#FF9800");

        // Success and info colors
        public static readonly Color ForestGreen = ColorTranslator.FromHtml("#00E676");
        public static readonly Color AppleGreen = ColorTranslator.FromHtml("#B8E986");
        public static readonly Color FunBlue = ColorTranslator.FromHtml("#2196F3");
        public static readonly Color CeruleanBlue = ColorTranslator.FromHtml("#00B0FF");

        // Accent colors
        public static readonly Color EminencePurple = ColorTranslator.FromHtml("#EA88FC");
        public static readonly Color HollywoodPink = ColorTranslator.FromHtml("#FF4081");

        // Grayscale colors
        public static readonly Color ThunderGray = ColorTranslator.FromHtml("#212121");
        public static readonly Color ShipGray = ColorTranslator.FromHtml("#424242");
        public static readonly Color AbbeyGray = ColorTranslator.FromHtml("#616161");
        public static readonly Color OsloGray = ColorTranslator.FromHtml("#9E9E9E");
        public static readonly Color SilverSand = ColorTranslator.FromHtml("#BDBDBD");
        public static readonly Color IronGray = ColorTranslator.FromHtml("#F5F5F5");
    }
}