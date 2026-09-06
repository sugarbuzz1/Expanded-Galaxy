using HarmonyLib;
using UnityEngine;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLTriCorpShockDrone), "SetupShipStats")]
    internal class TriCorpShockDroneMod
    {
        private static void Postfix(PLTriCorpShockDrone __instance, bool previewStats, bool startingPlayerShip)
        {
            if (__instance == null || __instance.MyStats == null)
                return;
            if (__instance.Exterior != null)
                __instance.Exterior.transform.localScale = new Vector3(2, 2, 2);
        }
    }
}
