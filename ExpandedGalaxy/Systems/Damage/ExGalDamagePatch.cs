using ExpandedGalaxy.SensorDish;
using HarmonyLib;
using PulsarModLoader.Content.Components.HullPlating;
using UnityEngine;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLShipInfoBase), "TakeDamage")]
    internal class ExGalDamagePatch
    {
        private static bool Prefix(PLShipInfoBase __instance, ref float dmg, ref bool bottomHit, ref EDamageType dmgType, ref float randomNum, ref int SystemTargetID, ref PLShipInfoBase attackingShip, ref int turretID, ref float __result)
        {
            if (__instance != null && Nukes.AllStatusDatas.ContainsKey(__instance.SpaceTargetID))
            {
                ShipStatusEffectData data = Nukes.AllStatusDatas[__instance.SpaceTargetID];
                if (data.EffectId == 0)
                {
                    dmg *= data.EffectStrength;
                }
            }

            if (!(__instance is PLShipInfo) || (__instance is PLShipInfo && ((PLShipInfo)__instance).StartupSwitchBoard.GetLateStatus(2)))
            {
                if (__instance.MyStats.GetShipComponent<PLShieldGenerator>(ESlotType.E_COMP_SHLD) != null)
                {
                    if (__instance.MyStats.ShieldsCurrent > __instance.MyStats.GetShipComponent<PLShieldGenerator>(ESlotType.E_COMP_SHLD).MinIntegrityAfterDamageScaled)
                        return true;
                }
            }

            if (__instance.MyStats.GetShipComponent<PLHullPlating>(ESlotType.E_COMP_HULLPLATING) == null)
                return true;

            if (SensorDishModManager.IsSensorWeaknessActiveModded(__instance, 2, 1))
                return true;

            PLHullPlating hullPlating = __instance.MyStats.GetShipComponent<PLHullPlating>(ESlotType.E_COMP_HULLPLATING);

            if (bottomHit && __instance.MyStats.GetShipComponent<PLHullPlating>(ESlotType.E_COMP_HULLPLATING) != null)
            {
                if (hullPlating.SubType == (int)EHullPlatingType.E_HULLPLATING_CCGE)
                {
                    dmg = Mathf.Clamp(dmg - (65f + (7f * hullPlating.Level)) * (1f - __instance.AcidicAtmoBoostAlpha), dmg * 0.1f, dmg);
                }
                else if (hullPlating.SubType == HullPlatingModManager.Instance.GetHullPlatingIDFromName("HeavyDutyPlating"))
                {
                    dmg = Mathf.Clamp(dmg * (1f - ((32f + (2f * hullPlating.Level)) * (1f - __instance.AcidicAtmoBoostAlpha) / 100f)), dmg * 0.1f, dmg);
                }
                else if (hullPlating.SubType == HullPlatingModManager.Instance.GetHullPlatingIDFromName("LightHullPlating"))
                {
                    dmg = Mathf.Clamp(dmg - (20f + (2f * hullPlating.Level)) * (1f - __instance.AcidicAtmoBoostAlpha), dmg * 0.1f, dmg);
                }
                else if (hullPlating.SubType == HullPlatingModManager.Instance.GetHullPlatingIDFromName("AdaptiveArmorPlating"))
                {
                    if (dmg < 10000f)
                    {
                        dmg = dmg - (dmg * (1 - (1000f / (1000f + dmg))) * (1f - __instance.AcidicAtmoBoostAlpha));
                    }
                }
            }
            return true;
        }
    }
}

