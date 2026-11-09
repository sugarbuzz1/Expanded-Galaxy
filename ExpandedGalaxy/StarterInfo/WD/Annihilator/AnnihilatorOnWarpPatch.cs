using HarmonyLib;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLWDAnnihilatorInfo), "OnEndWarp")]
    internal class AnnihilatorOnWarpPatch
    {
        private static bool Prefix(PLWDAnnihilatorInfo __instance)
        {
            if (!(__instance.MyFlightAI != null))
                return false;
            __instance.MyFlightAI.cachedRepairDepotList.Clear();
            return false;
        }
    }
}
