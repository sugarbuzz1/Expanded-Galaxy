using HarmonyLib;
using System.Linq;
using UnityEngine;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLTurret), "Tick")]
    internal class AncientSensorDishMissileRebateP2
    {
        private static void Postfix(PLTurret __instance)
        {
            if (AncientSensorDishMissileRebateP1.TurretMissileTick.Contains(__instance.NetID))
            {
                __instance.LastFireMissileTime = Time.time - (__instance.TrackerMissileReloadTime * 0.5f);
                AncientSensorDishMissileRebateP1.TurretMissileTick.Remove(__instance.NetID);
            }
        }
    }
}
