using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;
using UnityEngine.SceneManagement;

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

            SceneManager.sceneLoaded += OnSceneLoaded;

            var harmony = new Harmony("AgusBut.BanishCards");
            harmony.PatchAll();
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (scene.name == "GameScene")
            {
                Logger.LogDebug("New run detected. Resetting banish counter.");
                Logger.LogDebug($"[BanishCards] Loaded MaxBanishes from config: {MaxBanishesConfig.Value}");
                BanishesThisRun = 0;
            }
        }
    }
}
