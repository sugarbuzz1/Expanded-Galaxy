using HarmonyLib;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLShipInfo), "OnCalculateStatsFinal")]
    internal class ChargeShieldFix
    {
        private static bool Prefix(PLShipInfo __instance, PLShipStats inStats)
        {
            if (__instance.MyShieldGenerator != null && !__instance.MyShieldGenerator.IsPowerActive)
                inStats.ShieldsChargeRate = 0f;
            if (!__instance.InWarp)
                return false;
            inStats.ShieldsChargeRate *= 10f;
            inStats.ShieldsChargeRateMax *= 10f;
            return false;
        }
    }
}
