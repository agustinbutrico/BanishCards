using BanishCards.Utility;
using HarmonyLib;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BanishCards.Runtime
{
    internal class CardBanishManager
    {
        internal static void BanishCard(CardManager cardManager, int index)
        {
            // Access cards array
            var cardsField = AccessTools.Field(typeof(CardManager), "cards");
            UpgradeCard[] cards = (UpgradeCard[])cardsField.GetValue(cardManager);

            // Dynamically count cards currently displayed
            int cardsShown = 0;
            for (int i = 0; i < cards.Length; i++)
                if (cards[i] != null)
                    cardsShown++;

            // Determine remaining banishes for this run
            int maxBanishes = ConfigManager.MaxBanishesConfig.Value;
            int remainingBanishesThisRun = maxBanishes - Plugin.Instance.BanishesThisRun;

            // Determine max allowed for this draw (to leave 1 card)
            int perDrawLimit = cardsShown - 1;

            // Final limit: min of remaining banishes and per-draw limit
            int maxAllowedBanishesNow = Mathf.Min(remainingBanishesThisRun, perDrawLimit);

            if (maxAllowedBanishesNow <= 0)
            {
                // Cannot banish anymore this draw or run
                return;
            }

            // Proceed with banish
            var availableCardsField = AccessTools.Field(typeof(CardManager), "availableCards");
            var availableCards = (List<UpgradeCard>)availableCardsField.GetValue(cardManager);

            var cardHoldersField = AccessTools.Field(typeof(CardManager), "cardHolders");
            GameObject[] cardHolders = (GameObject[])cardHoldersField.GetValue(cardManager);

            if (cards == null || index >= cards.Length) return;
            var card = cards[index];
            if (card == null) return;

            // Stores the unlock name if it exists
            string unlockName = (string)AccessTools.Field(typeof(UpgradeCard), "unlockName").GetValue(card);
            if (!string.IsNullOrEmpty(unlockName))
                BanishedCards.BanishedUnlockNames.Add(unlockName);

            // Increment banish counter AFTER passing the check
            Plugin.Instance.BanishesThisRun++;

            if (availableCards.Contains(card))
                availableCards.Remove(card);

            // Adjust cards array to shift down
            int currentCount = 0;
            for (int i = 0; i < cards.Length; i++)
                if (cards[i] != null)
                    currentCount++;

            for (int i = index; i < currentCount - 1; i++)
                cards[i] = cards[i + 1];

            cards[currentCount - 1] = null;
            cardHolders[currentCount - 1].SetActive(false);

            // Update UI text/images
            var titlesField = AccessTools.Field(typeof(CardManager), "titles");
            var imagesField = AccessTools.Field(typeof(CardManager), "images");
            var descriptionsField = AccessTools.Field(typeof(CardManager), "descriptions");

            Text[] titles = (Text[])titlesField.GetValue(cardManager);
            Image[] images = (Image[])imagesField.GetValue(cardManager);
            Text[] descriptions = (Text[])descriptionsField.GetValue(cardManager);

            for (int i = index; i < currentCount - 1; i++)
            {
                if (cards[i] != null)
                {
                    titles[i].text = cards[i].title;
                    images[i].sprite = cards[i].image;
                    descriptions[i].text = cards[i].description;
                }
            }

            titles[currentCount - 1].text = "";
            images[currentCount - 1].sprite = null;
            descriptions[currentCount - 1].text = "";

            // UI remains open for player to select remaining cards

            // Refresh canvas so buttons state reflects inmediately
            UIRefreshHelper.ForceRefreshCanvas();
        }

        public static class UIRefreshHelper
        {
            public static void ForceRefreshCanvas()
            {
                var canvasGo = GameObject.Find("CardUI/Canvas");
                if (canvasGo != null)
                {
                    // Force a refresh by toggling active state
                    canvasGo.SetActive(false);
                    canvasGo.SetActive(true);
                }
            }
        }
    }
}
