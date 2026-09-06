using HarmonyLib;
using PulsarModLoader.Content.Components.Shield;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLShipStats), "TakeShieldDamage")]
    internal class ReflectorShieldDamage
    {
        private static bool Prefix(PLShipStats __instance, ref float inDmg, EDamageType dmgType, float DT_ShieldBoost, float shieldDamageMod, PLTurret turret)
        {
            PLShieldGenerator shield = __instance.GetShipComponent<PLShieldGenerator>(ESlotType.E_COMP_SHLD);
            if (shield != null && shield.Current > shield.MinIntegrityAfterDamage && shield.SubType == ShieldModManager.Instance.GetShieldIDFromName("Reflector Shield Generator"))
            {
                if (turret != null && turret.ShipStats != null && turret.ShipStats.Ship != null && turret.ShipStats != __instance)
                {
                    turret.ShipStats.Ship.TakeDamage(inDmg, false, dmgType, 1f, -1, __instance.Ship, -1);
                }
            }
            return true;
        }
    }
}
