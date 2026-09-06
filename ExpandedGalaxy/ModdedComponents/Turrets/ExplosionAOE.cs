using HarmonyLib;
using UnityEngine;
using static ExpandedGalaxy.Turrets;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLProjectile), "Explode")]
    internal class ExplosionAOE
    {
        private static bool Prefix(PLProjectile __instance, PLSpaceTarget target, Vector3 pos, Vector3 norm)
        {
            if (__instance.MyDamageType == (EDamageType)16)
            {
                ServerExplosiveProjExplode(__instance, target);
            }
            return true;
        }
    }

}
