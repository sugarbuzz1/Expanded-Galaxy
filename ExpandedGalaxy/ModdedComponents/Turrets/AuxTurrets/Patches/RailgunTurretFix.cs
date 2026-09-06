using HarmonyLib;
using System;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLRailgunTurret), MethodType.Constructor, new Type[2] { typeof(int), typeof(int) })]
    internal class RailgunTurretFix
    {
        private static void Postfix(PLRailgunTurret __instance, int inLevel, int inSubTypeData)
        {
            __instance.ProjSpeed = 3600f;
            __instance.m_Damage = 55f;
        }
    }
}
