using HarmonyLib;
using PulsarModLoader.Content.Components.WarpDrive;
using UnityEngine;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLWarpDriveScreen), "OnButtonHover")]
    internal class OnHoverBrokenDriveScreen
    {
        private static bool Prefix(
          PLWarpDriveScreen __instance,
          UIWidget inButton,
          ref UIWidget ___LastHoverButton)
        {
            if (inButton.name.Contains("inputOption") && ClickBrokenWarpDriveScreen.currentInput.Count == 0)
            {
                if ((UnityEngine.Object)__instance.MyScreenHubBase != (UnityEngine.Object)null && (UnityEngine.Object)__instance.MyScreenHubBase.OptionalShipInfo != (UnityEngine.Object)null && __instance.MyScreenHubBase.OptionalShipInfo.GetIsPlayerShip() && __instance.MyScreenHubBase.OptionalShipInfo.MyWarpDrive != null && __instance.MyScreenHubBase.OptionalShipInfo.MyWarpDrive.SubType == WarpDriveModManager.Instance.GetWarpDriveIDFromName("Broken Warp Drive"))
                {
                    if (int.Parse(inButton.name.Split('_')[1]) > 19)
                        return false;
                    inButton.alpha = 1f;
                }
                return false;
            }
            if (inButton.name == "Jump")
            {
                inButton.color = Color.blue;
                PLInGameUI.SetTooltipMiniText(PLLocalize.Localize("Toggle warp drive charging. Once charging is complete, press to initiate a warp jump"), true);
            }
            else if (inButton.name == "BlindJump")
            {
                inButton.color = Color.red;
                PLInGameUI.SetTooltipMiniText(PLLocalize.Localize("Blind Jump: Jumps the ship immediately without proper navigation. Can result in ship destruction"), true);
            }
            else
                inButton.alpha = 1f;
            if (!((UnityEngine.Object)___LastHoverButton != (UnityEngine.Object)inButton))
                return false;
            ___LastHoverButton = inButton;
            __instance.PlaySoundEventOnAllClonedScreens("play_ship_generic_internal_computer_ui_hover");
            return false;
        }
    }
}