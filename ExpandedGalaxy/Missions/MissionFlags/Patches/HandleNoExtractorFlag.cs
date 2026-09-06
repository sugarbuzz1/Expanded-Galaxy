using HarmonyLib;
using PulsarModLoader.Content.Components.WarpDrive;
using System;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLShipComponent), "GetSalvageSuccessRate")]
    internal class HandleNoExtractorFlag
    {
        private static Exception Finalizer(Exception __exception, PLShipComponent __instance, float successRateFromExtractor, ref float __result)
        {
            if (__instance != null && __instance is PLWarpDrive && __instance.SubType == WarpDriveModManager.Instance.GetWarpDriveIDFromName("Broken Warp Drive"))
                return __exception;
            if (__instance.ShipStats != null)
            {
                bool flag = false;
                foreach (PLShipComponent component in __instance.ShipStats.GetComponentsOfType(ESlotType.E_COMP_REAC_COOLING))
                {
                    if (component is MissionNoExtractorFlag)
                    {
                        flag = true;
                        break;
                    }
                }
                if (flag)
                    __result = 0f;
            }
            return __exception;
        }
    }

}

