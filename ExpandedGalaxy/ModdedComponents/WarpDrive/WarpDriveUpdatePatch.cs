using HarmonyLib;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLWarpDrive), "Update")]
    internal class WarpDriveUpdatePatch
    {
        private static void Postfix(PLWarpDrive __instance)
        {
            if (__instance.SysInstConduit != -1 && __instance.IsPowerActive)
            {
                if (__instance.ShipStats != null && __instance.ShipStats.Ship.EngineeringSystem != null)
                    __instance.RequestPowerUsage_Percent = __instance.ShipStats.Ship.EngineeringSystem.GetHealthRatio();
            }
        }
    }
}
