using HarmonyLib;
using System.Collections.Generic;
using UnityEngine;
using static ExpandedGalaxy.Relic;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLWarpStationScreen), "Update")]
    internal class UpdateStargateScreen
    {
        private static void Postfix(PLWarpStationScreen __instance, ref List<UILabel> ___AllLabels, ref UISprite ___WarpPanel, ref List<UISprite> ___AllStylizedElements)
        {
            if (!(__instance.MyWarpStation != null) || !__instance.UIIsSetup())
                return;
            if (PLServer.GetCurrentSector() != null && PLServer.GetCurrentSector().VisualIndication == ESectorVisualIndication.WARP_NETWORK_STATION && PLServer.GetCurrentSector().MySPI != null && PLServer.GetCurrentSector().MySPI.Faction == 6)
            {
                __instance.gameObject.SetActive(true);
                if (___WarpPanel.gameObject.activeSelf)
                {
                    ___WarpPanel.gameObject.SetActive(false);
                    foreach (UISprite element in ___AllStylizedElements)
                    {
                        if (element.name == "Panel_Stargate Controls")
                        {
                            element.gameObject.SetActive(true);
                            StargatePuzzle.GeneratePuzzleVector(out SetupStargateScreen.currentSolution, ((int)PLServer.Instance.GalaxySeed) + (ReflectedRift.inRift ? 0 : 1));
                            break;
                        }
                    }
                }
                if (___AllLabels.Count < SetupStargateScreen.statusLabel)
                    return;
                ___AllLabels[SetupStargateScreen.targetLabel].color = Color.Lerp(___AllLabels[SetupStargateScreen.targetLabel].color, Color.white, Time.deltaTime);
                ___AllLabels[SetupStargateScreen.statusLabel].color = Color.Lerp(___AllLabels[SetupStargateScreen.statusLabel].color, Color.white, Time.deltaTime);
            }
        }
    }
}
