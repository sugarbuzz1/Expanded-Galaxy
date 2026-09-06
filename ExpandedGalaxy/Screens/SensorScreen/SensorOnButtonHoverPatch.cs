
using HarmonyLib;
using UnityEngine;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLScientistSensorScreen), "OnButtonHover")]
    internal class SensorOnButtonHoverPatch
    {
        private static bool Prefix(PLScientistSensorScreen __instance, UIWidget inButton, ref UIWidget ___LastHoverButton, ref Color ___UI_White)
        {
            if (!inButton.name.Contains("MS_ActiveScanButton"))
                return true;
            if (__instance.MyScreenHubBase.OptionalShipInfo.IsActiveScanInProgress())
                return false;
            inButton.color = ___UI_White;
            if ((double)inButton.finalAlpha > 0.01)
            {
                PLInGameUI.SetTooltipLargeText("Sensor Sweep", "Starts the process of scanning for nearby ships and objects with the selected sensor. Your ship will be easier to detect by enemies while your sensors are boosted", false, 0.0f, inIsHelpTip: true);
            }
            if (inButton == null || !inButton.gameObject.activeSelf || !inButton.gameObject.activeInHierarchy || !((double)inButton.finalAlpha > 0.1) || !(___LastHoverButton != inButton))
                return false;
            ___LastHoverButton = inButton;
            __instance.PlaySoundEventOnAllClonedScreens("play_ship_generic_internal_computer_ui_hover");
            return false;
        }
    }
}
