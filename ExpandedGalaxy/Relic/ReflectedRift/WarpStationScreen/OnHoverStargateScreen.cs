using HarmonyLib;
using System;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLWarpStationScreen), "OnButtonHover")]
    internal class OnHoverStargateScreen
    {
        private static bool Prefix(PLWarpStationScreen __instance, UIWidget inButton)
        {
            if (PLServer.GetCurrentSector() != null && PLServer.GetCurrentSector().VisualIndication == ESectorVisualIndication.WARP_NETWORK_STATION && PLServer.GetCurrentSector().MySPI != null && PLServer.GetCurrentSector().MySPI.Faction == 6)
            {
                if (ClickStargateScreen.currentInput.Count == 0 && inButton.name.Contains("inputOption"))
                {
                    int index = Int32.Parse(inButton.name.Split('_')[1]);
                    if (index > 19)
                        return false;
                }
            }
            return true;
        }
    }
}
