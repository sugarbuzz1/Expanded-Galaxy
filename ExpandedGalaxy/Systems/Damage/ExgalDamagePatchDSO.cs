using HarmonyLib;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLDamageableSpaceObject), "TakeDamage")]
    internal class ExgalDamagePatchDSO
    {
        private static bool Prefix(PLDamageableSpaceObject __instance, ref float damage)
        {
            if (__instance != null && Nukes.AllStatusDatas.ContainsKey(__instance.SpaceTargetID))
            {
                ShipStatusEffectData data = Nukes.AllStatusDatas[__instance.SpaceTargetID];
                if (data.EffectId == 0)
                {
                    damage *= data.EffectStrength;
                }
            }
            return true;
        }
    }
}

