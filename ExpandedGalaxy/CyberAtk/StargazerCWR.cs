using HarmonyLib;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLStarGazerInfo), "ShipFinalCalculateStats")]
    internal class StargazerCWR
    {
        private static void Postfix(PLStarGazerInfo __instance, ref PLShipStats inStats)
        {
            inStats.CyberAttackRating += 1.0f;
        }
    }
}
