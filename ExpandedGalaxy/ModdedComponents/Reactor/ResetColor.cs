using HarmonyLib;
using UnityEngine;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLReactor), "Equip")]
    internal class ResetColor
    {
        private static void Postfix(PLReactor __instance)
        {
            if (__instance.ShipStats == null && __instance.ShipStats.Ship == null)
                return;
            if (__instance.ShipStats.Ship is PLShipInfo)
            {
                if ((__instance.ShipStats.Ship as PLShipInfo).ReactorInstance == null)
                    return;
                foreach (Light componentsInChild in (__instance.ShipStats.Ship as PLShipInfo).ReactorInstance.GetComponentsInChildren<Light>())
                    componentsInChild.color = Color.white;
                foreach (ParticleSystem componentsInChild in (__instance.ShipStats.Ship as PLShipInfo).ReactorInstance.GetComponentsInChildren<ParticleSystem>())
                    componentsInChild.startColor = Color.white;
            }
        }
    }
}
