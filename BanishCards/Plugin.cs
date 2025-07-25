﻿using BanishCards.Runtime;
using BanishCards.Utility;
using BepInEx;
using HarmonyLib;
using UnityEngine.SceneManagement;

namespace BanishCards
{
    [BepInPlugin("AgusBut.BanishCards", "BanishCards", "1.1.0")]
    [BepInDependency("AgusBut.TexturesLib.Shared")]
    [BepInDependency("AgusBut.TexturesLib.UI")]
    public class Plugin : BaseUnityPlugin
    {
        public static Plugin Instance { get; private set; }
        public static BepInEx.Logging.ManualLogSource Log { get; private set; }

        internal int BanishesThisRun = 0;

        private void Awake()
        {
            Instance = this;
            Log = base.Logger;

            ConfigManager.Initialize(this);

            SceneManager.activeSceneChanged += OnActiveSceneChanged;

            var harmony = new Harmony("AgusBut.BanishCards");
            harmony.PatchAll();

            Logger.LogInfo("BanishCards loaded successfully.");
        }

        private void OnActiveSceneChanged(Scene oldScene, Scene newScene)
        {
            if (newScene.name == "GameScene")
            {
                BanishesThisRun = 0;
                BanishedCards.BanishedUnlockNames.Clear();
            }
        }
    }
}
