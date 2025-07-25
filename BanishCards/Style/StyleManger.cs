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
        }

        internal static PanelVisualStyle GetPanelStyle(StyleType type)
        {
            return new PanelVisualStyle
            {
                Slice = SliceColor.GetPanelSlice(type)
            };
        }
    }
}
