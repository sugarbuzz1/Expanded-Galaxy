using HarmonyLib;
using UnityEngine;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLShipStats), "TakeHullDamage")]
    internal class UpdateArmor
    {
        private static bool Prefix(PLShipStats __instance, float inDmg, EDamageType inDmgType, PLShipInfoBase attackingShip, PLTurret turret, ref float __result)
        {
            if (__instance.GetShipComponent<PLHullPlating>(ESlotType.E_COMP_HULLPLATING) == null)
                return true;
            PLHullPlating hullPlating = __instance.GetShipComponent<PLHullPlating>(ESlotType.E_COMP_HULLPLATING);
            if (hullPlating is BonePlating && hullPlating.SubTypeData == 0)
            {
                __result = 0;
                hullPlating.SubTypeData = (short)Mathf.Clamp(21 - hullPlating.Level, 8f, float.MaxValue);
                return false;
            }
            return true;
        }
        private static void Postfix(PLShipStats __instance, float inDmg, EDamageType inDmgType, PLShipInfoBase attackingShip, PLTurret turret, ref float __result)
        {
            if (__instance.GetShipComponent<PLHullPlating>(ESlotType.E_COMP_HULLPLATING) == null)
                return;

            PLHullPlating hullPlating = __instance.GetShipComponent<PLHullPlating>(ESlotType.E_COMP_HULLPLATING);
            if (hullPlating is NanoActivePlating)
            {
                (hullPlating as NanoActivePlating).armor = 0f;
            }
        }
    }
}
