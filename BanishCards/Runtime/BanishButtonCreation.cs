﻿using BanishCards.Utility;
using HarmonyLib;
using TexturesLib.Shared;
using UnityEngine;
using UnityEngine.UI;
using static BanishCards.Style.StyleManager;

namespace BanishCards.Runtime
{
    public class BanishButtonCreation : MonoBehaviour
    {
        private Button button;
        private Image buttonImage;
        private Text buttonText;

        void Awake()
        {
            button = GetComponent<Button>();
            buttonImage = GetComponent<Image>();
            buttonText = GetComponentInChildren<Text>();

            var buttonStyle = GetPanelStyle(StyleType.Button);

            // Assign base and hover sprites
            if (buttonImage != null)
            {
                buttonImage.sprite = SpriteHelper.FindSpriteByName($"{buttonStyle.Slice}Filled");
            }

            if (button != null)
            {
                SpriteState spriteState = button.spriteState;
                spriteState.highlightedSprite = SpriteHelper.FindSpriteByName(buttonStyle.Slice);
                button.spriteState = spriteState;
            }
        }

        void OnEnable()
        {
            UpdateState();
        }

        public void UpdateState()
        {
            
            int maxBanishes = ConfigManager.MaxBanishesConfig.Value;
            int banishesDone = Plugin.Instance.BanishesThisRun;

            var cardsField = AccessTools.Field(typeof(CardManager), "cards");
            UpgradeCard[] cards = (UpgradeCard[])cardsField.GetValue(CardManager.instance);
            int cardsShown = 0;
            for (int i = 0; i < cards.Length; i++)
                if (cards[i] != null)
                    cardsShown++;

            int perDrawLimit = cardsShown - 1;
            int remainingBanishes = maxBanishes - banishesDone;
            Mathf.Min(remainingBanishes, perDrawLimit);

            bool shouldBeInvisible = banishesDone >= maxBanishes;
            bool shouldBeTranslucent = cardsShown <= 1 && !shouldBeInvisible;

            // Set interactable
            button.interactable = !(shouldBeInvisible || shouldBeTranslucent);

            // Apply color adjustments
            if (buttonImage != null)
            {
                Color c = buttonImage.color;
                c.a = shouldBeInvisible ? 0f : shouldBeTranslucent ? 0.3f : 1f;
                buttonImage.color = c;
            }

            if (buttonText != null)
            {
                var buttonStyle = GetPanelStyle(StyleType.Button);
                Color c = buttonStyle.TextColor;
                c.a = shouldBeInvisible ? 0f : shouldBeTranslucent ? 0.3f : 1f;
                buttonText.color = c;
            }

            // Ensure GameObject visibility aligns with true invisibility
            gameObject.SetActive(!shouldBeInvisible);
        }
    }
}
