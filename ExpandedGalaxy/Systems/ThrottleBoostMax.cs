using HarmonyLib;
using UnityEngine;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLShipControl), "FixedUpdate")]
    internal class ThrottleBoostMax
    {
        private static void Postfix(PLShipControl __instance)
        {
            if (__instance.ShipInfo != null && __instance.ShipInfo.EngineeringSystem != null)
            {
                __instance.Boost = Mathf.Clamp(__instance.Boost, 0f, __instance.ShipInfo.EngineeringSystem.GetHealthRatio());
            }
        }
    }
}

