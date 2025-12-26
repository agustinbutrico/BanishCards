using HarmonyLib;
using UnityEngine.UI;
using UnityEngine;
using System;
using BanishCards.Runtime;

namespace BanishCards.Patches
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
                    var banishButtonObj = UnityEngine.Object.Instantiate(selectButton.gameObject, highlight);
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
                        CardBanishManager.BanishCard(__instance, capturedIndex);
                    });

                    // Control if the banish button should be active
                    banishButtonObj.AddComponent<BanishButtonCreation>();
                    banishButtonObj.SetActive(true);
                }
                catch (Exception ex)
                {
                    Plugin.Log.LogError($"Error creating banish button for card {i}: {ex}");
                }
            }
        }
    }
}
