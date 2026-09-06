using HarmonyLib;
using UnityEngine;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLWarpDriveScreen), "OnButtonMouseAway")]
    internal class OnButtonMouseAwayBrokenDriveScreen
    {
        private static bool Prefix(
          PLWarpDriveScreen __instance,
          UIWidget inButton,
          ref UIWidget ___LastHoverButton)
        {
            if (inButton.name == "Jump")
                inButton.color = Color.white;
            else if (inButton.name == "BlindJump")
                inButton.color = __instance.MyScreenHubBase.OptionalShipInfo.BlindJumpUnlocked ? Color.gray : Color.white;
            else
                inButton.alpha = 0.6f;
            if (!((UnityEngine.Object)___LastHoverButton == (UnityEngine.Object)inButton))
                return false;
            PLInGameUI.HidePersistantMiniTooltip();
            ___LastHoverButton = (UIWidget)null;
            return false;
        }
    }
}