using HarmonyLib;
using PulsarModLoader.Content.Components.Shield;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLShipStats), "TakeShieldDamage")]
    internal class MiningDroneShieldDamage
    {
        private static bool Prefix(PLShipStats __instance, ref float inDmg, EDamageType dmgType, float DT_ShieldBoost, float shieldDamageMod, PLTurret turret)
        {
            PLShieldGenerator shield = __instance.GetShipComponent<PLShieldGenerator>(ESlotType.E_COMP_SHLD);
            if (shield != null && shield.Current > shield.MinIntegrityAfterDamage && shield.SubType == ShieldModManager.Instance.GetShieldIDFromName("Layered Surface Projector"))
            {
                inDmg *= 0.9f;
            }
            return true;
        }
    }
}
