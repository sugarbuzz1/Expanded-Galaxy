using HarmonyLib;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLRolandInfo), "ShipFinalCalculateStats")]
    internal class RolandCD
    {
        private static void Postfix(PLStarGazerInfo __instance, ref PLShipStats inStats)
        {
            inStats.CyberDefenseRating += 1.0f;
        }
    }
}
