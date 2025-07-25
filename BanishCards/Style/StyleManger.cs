using UnityEngine;

namespace BanishCards.Style
{
    internal class StyleManager
    {
        public enum StyleType
        {
            Button
        }

        internal struct PanelVisualStyle
        {
            public string Slice;
            public Color TextColor;
        }

        internal static PanelVisualStyle GetPanelStyle(StyleType type)
        {
            return new PanelVisualStyle
            {
                Slice = SliceColor.GetPanelSlice(type),
                TextColor = TextColor.GetCardTextColor(type)
            };
        }
    }
}
