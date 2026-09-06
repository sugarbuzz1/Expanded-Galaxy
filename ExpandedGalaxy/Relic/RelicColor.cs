using HarmonyLib;
using PulsarModLoader.Content.Components.MissionShipComponent;
using UnityEngine;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLGlobal), "GetColorBGForWare")]
    internal class RelicColor
    {
        private static void Postfix(PLGlobal __instance, PLWare ware, ref Color __result)
        {
            if (ware != null)
            {
                if (Relic.GetIsRelic(ware))
                {
                    __result = Relic.GetRelicColor();
                }
                else if (ware is PLMissionShipComponent && (ware as PLMissionShipComponent).SubType == MissionShipComponentModManager.Instance.GetMissionShipComponentIDFromName("Ammunition Cache"))
                    __result = PLGlobal.Instance.Galaxy.FactionColors[2];
            }
        }
    }
}
