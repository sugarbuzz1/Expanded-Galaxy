using HarmonyLib;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLWarpDriveProgram), "Tick")]
    internal class ShieldBoostNumbers
    {
        private static void Postfix(PLWarpDriveProgram __instance, ref float ___ShieldBooster_BoostAmount, ref float ___SuperShieldBooster_BoostAmount)
        {
            if (__instance.ShipStats != null)
            {
                if (__instance.ShipStats.Ship != null)
                {
                    if (__instance.ShipStats.Ship.MyShieldGenerator != null)
                    {
                        ___ShieldBooster_BoostAmount = __instance.ShipStats.Ship.MyShieldGenerator.ChargeRateCurrent * 0.25f;
                        ___SuperShieldBooster_BoostAmount = __instance.ShipStats.Ship.MyShieldGenerator.ChargeRateCurrent * 0.5f;
                    }
                }
            }
        }
    }
}

