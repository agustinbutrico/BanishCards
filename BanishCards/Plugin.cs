using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;
using UnityEngine;

namespace BanishCards
{
    [BepInPlugin("AgusBut.BanishCards", "BanishCards", "1.0.0")]
    public class Plugin : BaseUnityPlugin
    {
        public static Plugin Instance { get; private set; }
        public static BepInEx.Logging.ManualLogSource Log { get; private set; }

        // Máximo configurable de cartas a desaparecer
        private ConfigEntry<KeyCode> banishCardsMax;
        public ConfigEntry<KeyCode> BanishCardsMax => banishCardsMax;
        private void Awake()
        {
            Instance = this;
            Log = base.Logger;

            Logger.LogInfo("Loading [BanishCards 1.0.0]");

            // Crear e instalar los parches Harmony
            var harmony = new Harmony("AgusBut.BanishCards");
            harmony.PatchAll();
        }
    }
}
