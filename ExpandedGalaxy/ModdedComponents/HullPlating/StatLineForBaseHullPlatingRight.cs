using HarmonyLib;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLWare), "GetStatLineRight")]
    internal class StatLineForBaseHullPlatingRight
    {
        private static bool Prefix(PLWare __instance, ref string __result)
        {
            PLHullPlating hullPlating = __instance as PLHullPlating;
            if (hullPlating != null && hullPlating.SubType == (int)EHullPlatingType.E_HULLPLATING_CCGE)
            {
                __result = (55f + (12f * hullPlating.Level)).ToString("0") + "\n-" + (65f + (7f * hullPlating.Level)).ToString("0") + "\n" + (50f + (5f * hullPlating.Level)).ToString("0") + "\n";
                return false;
            }
            return true;
        }
    }
}
