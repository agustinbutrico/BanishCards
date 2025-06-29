using HarmonyLib;
using UnityEngine.UI;
using UnityEngine;
using System.Collections.Generic;
using System;

namespace BanishCards
{
    [HarmonyPatch(typeof(CardManager), "DisplayCards")]
    internal class Patch_DisplayCards
    {
        static void Postfix(CardManager __instance, int count)
        {
            // Reflection: Access private `cardHolders` field
            var cardHoldersField = AccessTools.Field(typeof(CardManager), "cardHolders");
            GameObject[] cardHolders = (GameObject[])cardHoldersField.GetValue(__instance);

            for (int i = 0; i < count; i++)
            {
                try
                {
                    var holder = cardHolders[i].transform;
                    var highlight = holder.Find($"Highlight{i}") ?? holder.Find("Highlight");
                    if (highlight == null) continue;

                    var selectButtonTransform = highlight.Find("SelectButton");
                    if (selectButtonTransform == null) continue;

                    var selectButton = selectButtonTransform.GetComponent<Button>();
                    if (selectButton == null) continue;

                    // === CLONE SelectButton for BanishButton ===
                    var banishButtonObj = GameObject.Instantiate(selectButton.gameObject, highlight);
                    banishButtonObj.name = "BanishButton";
                    banishButtonObj.transform.localScale = Vector3.one;
                    banishButtonObj.transform.localPosition = new Vector3(0f, 135f, 0f);
                    banishButtonObj.transform.SetAsLastSibling();

                    // Replace Button cleanly while preserving visuals
                    var oldBtn = banishButtonObj.GetComponent<Button>();
                    if (oldBtn != null) UnityEngine.Object.DestroyImmediate(oldBtn);
                    var newBtn = banishButtonObj.AddComponent<Button>();
                    newBtn.transition = selectButton.transition;
                    newBtn.colors = selectButton.colors;
                    newBtn.navigation = new Navigation { mode = Navigation.Mode.None };

                    // Copy Image styling
                    var img = banishButtonObj.GetComponent<Image>();
                    var sampleImg = selectButton.GetComponent<Image>();
                    img.sprite = sampleImg.sprite;
                    img.type = sampleImg.type;
                    img.color = sampleImg.color;
                    img.raycastTarget = true;
                    newBtn.targetGraphic = img;

                    // Configure Text styling
                    var txt = banishButtonObj.GetComponentInChildren<Text>();
                    var sampleTxt = selectButton.GetComponentInChildren<Text>();
                    if (txt != null && sampleTxt != null)
                    {
                        txt.text = "Banish";
                        txt.font = sampleTxt.font;
                        txt.fontSize = sampleTxt.fontSize;
                        txt.color = sampleTxt.color;
                        txt.alignment = sampleTxt.alignment;
                        txt.resizeTextForBestFit = sampleTxt.resizeTextForBestFit;
                        txt.resizeTextMinSize = sampleTxt.resizeTextMinSize;
                        txt.resizeTextMaxSize = sampleTxt.resizeTextMaxSize;
                    }

                    // Hook up banish logic
                    int capturedIndex = i;
                    newBtn.onClick.AddListener(() =>
                    {
                        BanishCard(__instance, capturedIndex);
                    });

                    // Control if the banish button should be active
                    banishButtonObj.AddComponent<BanishButtonControl>();
                    banishButtonObj.SetActive(true);
                }
                catch (Exception ex)
                {
                    Plugin.Log.LogError($"Error creating banish button for card {i}: {ex}");
                }
            }
        }

        static void BanishCard(CardManager cardManager, int index)
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
            int maxBanishes = Plugin.Instance.MaxBanishesConfig.Value;
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
                    canvasGo.SetActive(false);
                    canvasGo.SetActive(true);
                }
            }
        }
    }
}
