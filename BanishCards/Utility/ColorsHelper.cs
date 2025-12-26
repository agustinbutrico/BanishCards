using UnityEngine;

namespace BanishCards.Utility
{
    internal class ColorsHelper
    {
        public static Color Black = new Color(0.196f, 0.196f, 0.196f, 1f);
        public static Color Grey = new Color(0.514f, 0.502f, 0.514f, 1f);
        public static Color White = new Color(1f, 1f, 1f, 1f);
        public static Color Green = new Color(0.635f, 0.741f, 0.451f, 1f);
        public static Color DeepGreen = new Color(0.259f, 0.403f, 0.155f, 1f);
        public static Color Blue = new Color(0.471f, 0.529f, 0.667f, 1f);
        public static Color DeepBlue = new Color(0f, 0f, 0.286f, 1f);
        public static Color Red = new Color(0.667f, 0.333f, 0.251f, 1f);
        public static Color DeepRed = new Color(0.323f, 0.054f, 0.050f, 1f);
        public static Color Purple = new Color(0.460f, 0.093f, 0.903f, 1f);
        public static Color DeepPurple = new Color(0.369f, 0.157f, 0.569f, 1f);
        public static Color Orange = new Color(0.544f, 0.411f, 0.265f, 1f);
        public static Color DeepOrange = new Color(0.612f, 0.478f, 0.208f, 1f);

        public static Color GetContrastByName(string colorName)
        {
            switch (colorName.ToLower())
            {
                case "blue": return ColorsHelper.DeepBlue;
                case "deepblue": return ColorsHelper.Blue;

                case "red": return ColorsHelper.DeepRed;
                case "deepred": return ColorsHelper.Red;

                case "green": return ColorsHelper.DeepGreen;
                case "deepgreen": return ColorsHelper.Green;

                case "purple": return ColorsHelper.DeepPurple;
                case "deeppurple": return ColorsHelper.Purple;

                case "orange": return ColorsHelper.DeepOrange;
                case "deeporange": return ColorsHelper.Orange;

                case "black": return ColorsHelper.Grey;
                case "grey": return ColorsHelper.Black;
                case "white": return ColorsHelper.Grey;

                default: return ColorsHelper.Grey;
            }
        }
    }
}
