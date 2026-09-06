using ExpandedGalaxy.SensorDish;
using HarmonyLib;
using System.Collections.Generic;
using UnityEngine;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLTurret), "ServerFireMissile")]
    internal class AncientSensorDishMissileRebateP1
    {
        internal static List<int> TurretMissileTick = new List<int>();
        private static void Postfix(PLTurret __instance, PLTrackerMissile inMissile, int inTargetShipID)
        {
            if (!((UnityEngine.Object)__instance.TurretInstance != (UnityEngine.Object)null) || inMissile.SubTypeData <= (short)0)
                return;
            PLShipInfoBase pLShipInfoBase = PLEncounterManager.Instance.GetShipFromID(inTargetShipID);
            if (pLShipInfoBase == null)
                return;
            if (SensorDishModManager.IsSensorWeaknessActiveModded(pLShipInfoBase, 5, 1))
            {
                __instance.LastFireMissileTime = Time.time - (__instance.TrackerMissileReloadTime * 100000f);
                TurretMissileTick.Add(__instance.NetID);
            }
        }
    }
}
