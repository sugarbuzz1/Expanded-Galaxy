using HarmonyLib;
using UnityEngine;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLShieldGenerator), "Tick")]
    internal class ChargeShieldWhenOff
    {
        private static void Postfix(PLShieldGenerator __instance)
        {
            if (!__instance.IsEquipped)
                return;
            PLShipInfo ship = __instance.ShipStats.Ship as PLShipInfo;
            if ((double)__instance.Current < (double)__instance.ShipStats.ShieldsMax && ((Object)ship == (Object)null || ((Object)ship.StartupSwitchBoard != (Object)null && !ship.StartupSwitchBoard.GetLateStatus(2))))
            {
                __instance.RequestPowerUsage_Percent = 1f;
                __instance.IsPowerActive = true;
            }
        }
    }
}
