using HarmonyLib;
using PulsarModLoader.Content.Components.Turret;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLShipStats), "TakeShieldDamage")]
    internal class SylvassiTurretDamage
    {
        private static bool Prefix(PLShipStats __instance, float inDmg, EDamageType dmgType, float DT_ShieldBoost, float shieldDamageMod, PLTurret turret, ref float __result)
        {
            if (turret != null && turret.SubType == TurretModManager.Instance.GetTurretIDFromName("Sylvassi Turret") && __instance.Ship != null && !__instance.Ship.IsSupershieldActive())
            {
                __result = inDmg;
                return false;
            }
            return true;
        }
    }
}
