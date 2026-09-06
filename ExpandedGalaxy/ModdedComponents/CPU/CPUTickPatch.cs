using HarmonyLib;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLCPU), "Tick")]
    internal class CPUTickPatch
    {
        private static void Postfix(PLCPU __instance)
        {
            if (__instance.SubType == (int)ECPUClass.E_CPUTYPE_SHIELD_COPROCESSOR)
            {
                __instance.IsPowerActive = true;
            }
            if (__instance.SysInstConduit != -1 && __instance.IsPowerActive)
            {
                if (__instance.ShipStats != null && __instance.ShipStats.Ship.ComputerSystem != null)
                    __instance.RequestPowerUsage_Percent = __instance.ShipStats.Ship.ComputerSystem.GetHealthRatio();
            }
        }
    }
}
