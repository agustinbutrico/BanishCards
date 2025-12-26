using BepInEx;
using BepInEx.Configuration;

namespace BanishCards.Utility
{
    internal static class ConfigManager
    {
        internal static ConfigEntry<int> MaxBanishesConfig;
        internal static ConfigEntry<string> ButtonColor;

        internal static void Initialize(BaseUnityPlugin plugin)
        {
            string availableColors = "Can be: White, Grey, Black, Blue, Orange, Red, Green, Purple, DeepBlue, DeepBrown, DeepRed, DeepGreen, DeepPurple";

            MaxBanishesConfig = plugin.Config.Bind("General", "MaxBanishes", 3, "Maximum number of cards you can banish per run.");
            ButtonColor = plugin.Config.Bind("Appearance", "ButtonColor", "Grey", availableColors);
        }
    }
}
