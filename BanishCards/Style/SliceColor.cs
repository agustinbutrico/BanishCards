using BanishCards.Utility;
using static BanishCards.Style.StyleManager;

namespace BanishCards.Style
{
    internal class SliceColor
    {
        internal static string GetPanelSlice(StyleType type)
        {
            switch (type)
            {
                case StyleType.Button:
                    return $"UI9Slice{ConfigManager.ButtonColor.Value}";
                default:
                    return "UI9SliceRed";
            };
        }
    }
}
