using BanishCards.Utility;
using TexturesLib.Shared;
using UnityEngine;
using static BanishCards.Style.StyleManager;

namespace BanishCards.Style
{
    internal class TextColor
    {
        internal static Color GetCardTextColor(StyleType type)
        {
            string colorKey;

            switch (type)
            {
                case StyleType.Button:
                    colorKey = ConfigManager.ButtonColor.Value;
                    break;
                default:
                    colorKey = "Grey";
                    break;
            };

            return ColorsHelper.GetContrastByName(colorKey);
        }
    }
}
