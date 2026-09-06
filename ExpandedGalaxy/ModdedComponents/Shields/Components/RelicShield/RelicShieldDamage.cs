using HarmonyLib;
using PulsarModLoader.Content.Components.Shield;
using UnityEngine;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLShipStats), "TakeShieldDamage")]
    internal class RelicShieldDamage
    {
        private static bool Prefix(PLShipStats __instance, ref float inDmg, EDamageType dmgType, float DT_ShieldBoost, float shieldDamageMod, PLTurret turret)
        {
            if (__instance.GetShipComponent<PLShieldGenerator>(ESlotType.E_COMP_SHLD) != null && __instance.GetShipComponent<PLShieldGenerator>(ESlotType.E_COMP_SHLD).SubType == ShieldModManager.Instance.GetShieldIDFromName("Anti-Matter Suspension Field"))
            {
                PLShieldGenerator shipComponent = __instance.GetShipComponent<PLShieldGenerator>(ESlotType.E_COMP_SHLD);
                if (shipComponent.Current < shipComponent.ShipStats.ShieldsMax * 0.101f)
                    return true;
                float num1 = 1f;
                float num2 = num1 * (float)(1.0 / (double)DT_ShieldBoost * (1.0 / (double)shieldDamageMod));
                float num3 = Mathf.Min(inDmg, shipComponent.Current * num2);
                float num4 = num3 / num2;
                SuspensionField.cachedDamage += num4;
                inDmg = 0f;
            }
            return true;
        }
    }
}
