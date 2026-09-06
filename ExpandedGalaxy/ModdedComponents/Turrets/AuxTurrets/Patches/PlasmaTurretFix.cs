using HarmonyLib;
using System;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLBasicTurret), MethodType.Constructor, new Type[2] { typeof(int), typeof(int) })]
    internal class PlasmaTurretFix
    {
        private static void Postfix(PLRailgunTurret __instance, int inLevel, int inSubTypeData)
        {
            __instance.m_Damage = 150f;
            Traverse traverse = Traverse.Create(__instance);
            traverse.Field("FireDelay").SetValue(5f);
            traverse.Field("TurretRange").SetValue(4500f);
            traverse.Field("HeatGeneratedOnFire").SetValue(0.45f);
            __instance.Desc = "A standard armament that can be found on many ships. It fires bolts of volatile plasma that detonates on contact or when it reaches max range.";
        }
    }
}
