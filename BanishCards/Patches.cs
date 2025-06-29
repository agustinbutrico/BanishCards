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

                    // Clone button object
                    var banishButtonObj = GameObject.Instantiate(selectButton.gameObject, highlight);
                    banishButtonObj.name = "BanishButton";
                    banishButtonObj.transform.localScale = Vector3.one;
                    banishButtonObj.transform.localPosition = new Vector3(0f, 135f, 0f);

                    // Set button text
                    var text = banishButtonObj.GetComponentInChildren<Text>();
                    if (text != null) text.text = "Banish";

                    // Remove old Button component
                    var oldButton = banishButtonObj.GetComponent<Button>();
                    if (oldButton != null)
                        UnityEngine.Object.Destroy(oldButton);

                    // Add fresh Button component
                    var banishButton = banishButtonObj.AddComponent<Button>();

                    // Transfer visual styling
                    var image = banishButtonObj.GetComponent<Image>();
                    banishButton.targetGraphic = image;

                    // Assign BanishCard action
                    int capturedIndex = i;
                    banishButton.onClick.AddListener(() =>
                    {
                        BanishCard(__instance, capturedIndex);
                    });

                    // Disable navigation conflicts
                    banishButton.navigation = new Navigation { mode = Navigation.Mode.None };

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
            // Reflection: Access necessary private fields
            var cardsField = AccessTools.Field(typeof(CardManager), "cards");
            UpgradeCard[] cards = (UpgradeCard[])cardsField.GetValue(cardManager);

            var availableCardsField = AccessTools.Field(typeof(CardManager), "availableCards");
            var availableCards = (List<UpgradeCard>)availableCardsField.GetValue(cardManager);

            var uiField = AccessTools.Field(typeof(CardManager), "ui");
            GameObject ui = (GameObject)uiField.GetValue(cardManager);

            var drawingCardsField = AccessTools.Field(typeof(CardManager), "<drawingCards>k__BackingField");

            if (cards == null || index >= cards.Length) return;

            var card = cards[index];
            if (card == null) return;

            if (availableCards.Contains(card))
                availableCards.Remove(card);

            drawingCardsField.SetValue(cardManager, false);
            ui.SetActive(false);
            SpawnManager.instance.ShowSpawnUIs(true);
        }
    }
}
