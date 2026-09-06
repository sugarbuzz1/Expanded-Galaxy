using HarmonyLib;
using PulsarModLoader.Content.Components.WarpDrive;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLWarpDriveScreen), "Update")]
    internal class UpdateBrokenDriveScreen
    {
        private static UISprite cachedPanel;

        private static void Postfix(
          PLWarpDriveScreen __instance,
          ref List<UILabel> ___AllLabels,
          ref UISprite ___WarpDrivePanel,
          ref UISprite ___JumpComputerPanel,
          ref UISprite ___m_BlockingTargetOnboardPanel,
          ref List<UISprite> ___AllStylizedElements,
          ref List<UILabel> ___AllSprites,
          ref List<UIWidget> ___AllButtons)
        {
            if (!__instance.UIIsSetup())
                return;
            if ((UnityEngine.Object)__instance.MyScreenHubBase != (UnityEngine.Object)null && (UnityEngine.Object)__instance.MyScreenHubBase.OptionalShipInfo != (UnityEngine.Object)null && __instance.MyScreenHubBase.OptionalShipInfo.MyWarpDrive != null && __instance.MyScreenHubBase.OptionalShipInfo.MyWarpDrive.SubType == WarpDriveModManager.Instance.GetWarpDriveIDFromName("Broken Warp Drive"))
            {
                if ((UnityEngine.Object)UpdateBrokenDriveScreen.cachedPanel == (UnityEngine.Object)null)
                {
                    foreach (UISprite uiSprite in ___AllStylizedElements)
                    {
                        if ((UnityEngine.Object)uiSprite != (UnityEngine.Object)null && uiSprite.name == "Warp Drive Controls: FTL-X899")
                        {
                            UpdateBrokenDriveScreen.cachedPanel = uiSprite;
                            break;
                        }
                    }
                    if ((UnityEngine.Object)UpdateBrokenDriveScreen.cachedPanel == (UnityEngine.Object)null)
                    {
                        SetupBrokenDriveScreen.Setup(__instance, out UpdateBrokenDriveScreen.cachedPanel);
                        UpdateBrokenDriveScreen.cachedPanel.gameObject.transform.position = ___JumpComputerPanel.gameObject.transform.position;
                    }
                }
                if (___m_BlockingTargetOnboardPanel.gameObject.activeSelf)
                {
                    UpdateBrokenDriveScreen.cachedPanel.gameObject.SetActive(false);
                }
                else
                {
                    ___WarpDrivePanel.gameObject.SetActive(false);
                    ___JumpComputerPanel.gameObject.SetActive(false);
                    UpdateBrokenDriveScreen.cachedPanel.gameObject.SetActive(true);
                    foreach (UIWidget uiWidget in ___AllButtons)
                    {
                        if (!((UnityEngine.Object)uiWidget == (UnityEngine.Object)null) && (uiWidget.name.Contains("inputOption") || uiWidget.name == "backspace" || uiWidget.name == "enter" || uiWidget.name == "reverse"))
                            uiWidget.gameObject.SetActive(__instance.MyScreenHubBase.OptionalShipInfo.MyWarpDrive.Level != 0);
                    }
                    if (___AllLabels.Count < SetupBrokenDriveScreen.statusLabel)
                        return;
                    ___AllLabels[SetupBrokenDriveScreen.targetLabel].color = Color.Lerp(___AllLabels[SetupBrokenDriveScreen.targetLabel].color, Color.white, Time.deltaTime);
                    ___AllLabels[SetupBrokenDriveScreen.statusLabel].color = Color.Lerp(___AllLabels[SetupBrokenDriveScreen.statusLabel].color, Color.white, Time.deltaTime);
                }
            }
            else
            {
                if ((UnityEngine.Object)UpdateBrokenDriveScreen.cachedPanel != (UnityEngine.Object)null)
                {
                    UnityEngine.Object.Destroy((UnityEngine.Object)UpdateBrokenDriveScreen.cachedPanel.gameObject);
                    UpdateBrokenDriveScreen.cachedPanel = (UISprite)null;
                }
                ___AllStylizedElements.RemoveAll((Predicate<UISprite>)(item => (UnityEngine.Object)item == (UnityEngine.Object)null));
                ___AllLabels.RemoveAll((Predicate<UILabel>)(item => (UnityEngine.Object)item == (UnityEngine.Object)null));
                ___AllSprites.RemoveAll((Predicate<UILabel>)(item => (UnityEngine.Object)item == (UnityEngine.Object)null));
                ___AllButtons.RemoveAll((Predicate<UIWidget>)(item => (UnityEngine.Object)item == (UnityEngine.Object)null));
            }
        }
    }
}