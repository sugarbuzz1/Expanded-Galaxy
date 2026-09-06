using CodeStage.AntiCheat.ObscuredTypes;
using HarmonyLib;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLShipComponent), "AddStats")]
    internal class StatsForBaseHullPlating
    {
        private static bool Prefix(PLShipComponent __instance, PLShipStats inStats)
        {
            float mass = 0f;
            PLHullPlating hullPlating = __instance as PLHullPlating;
            if (hullPlating != null && hullPlating.SubType == (int)EHullPlatingType.E_HULLPLATING_CCGE)
            {
                mass = (5f * hullPlating.Level);
                inStats.Mass += (ObscuredFloat)mass;
                inStats.HullArmor += (55f + (12f * hullPlating.Level)) / 250f;
            }
            return true;
        }
    }
}
