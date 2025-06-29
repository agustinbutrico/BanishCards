using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;

namespace BanishCards
{
    [BepInPlugin("AgusBut.BanishCards", "BanishCards", "1.0.0")]
    public class Plugin : BaseUnityPlugin
    {
        public static Plugin Instance { get; private set; }
        public static BepInEx.Logging.ManualLogSource Log { get; private set; }

        public ConfigEntry<int> MaxBanishesConfig { get; private set; }

        internal int BanishesThisRun = 0;

        private void Awake()
        {
            Instance = this;
            Log = base.Logger;

            Logger.LogInfo("Loading [BanishCards 1.0.0]");

            // Create config on first run
            MaxBanishesConfig = Config.Bind(
                "General",
                "MaxBanishes",
                3,
                "Maximum number of cards you can banish per run."
            );

            var harmony = new Harmony("AgusBut.BanishCards");
            harmony.PatchAll();
        }
    }
}
