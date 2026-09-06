using HarmonyLib;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLTurret), "GetDamageTypeString")]
    internal class ExplosionDamageString
    {
        private static void Postfix(PLTurret __instance, ref string __result)
        {
            if (__instance.SubType == (int)ETurretType.PLASMA || __instance.SubType == (int)ETurretType.BURST || __instance.SubType == (int)ETurretType.DEFENDER)
                __result = "PHYS (EXPLD)";
        }
    }
}
