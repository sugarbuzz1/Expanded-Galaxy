using HarmonyLib;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLDefenderTurret), "UpdateMaxPowerUsageWatts")]
    internal class DefenderPowerFix
    {
        private static bool Prefix(PLDefenderTurret __instance)
        {
            __instance.CalculatedMaxPowerUsage_Watts = 7600f;
            return false;
        }
    }
}
