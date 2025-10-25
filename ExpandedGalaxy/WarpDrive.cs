using HarmonyLib;
using System;

namespace ExpandedGalaxy
{
    internal class WarpDrive
    {
        [HarmonyPatch(typeof(PLWarpDrive), MethodType.Constructor, new Type[3] {typeof(EWarpDriveType), typeof(int), typeof(short) })]
        internal class WarpDriveProgramChargeRebalance
        {
            private static void Postfix(PLWarpDrive __instance)
            {
                switch(__instance.SubType)
                {
                    case (int)EWarpDriveType.E_WARPDR_CU_LONGRANGE_JUMP_MODULE:
                        __instance.NumberOfChargingNodes = 3;
                        break;
                    case (int)EWarpDriveType.E_WARPDR_GTC_SNAPPY_CRICKET:
                    case (int)EWarpDriveType.E_WARPDR_CU_STANDARD_JUMP_MODULE:
                        __instance.NumberOfChargingNodes = 4;
                        break;
                    case (int)EWarpDriveType.E_WARPDR_WDMILITARYJUMP:
                        __instance.NumberOfChargingNodes = 5;
                        break;
                    case (int)EWarpDriveType.E_WARPDR_EXPLORERS:
                        __instance.NumberOfChargingNodes = 3;
                        break;
                }
            }
        }

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
}
