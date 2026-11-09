using HarmonyLib;
using System;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLBurstTurret), MethodType.Constructor, new Type[2] { typeof(int), typeof(int) })]
    internal class BurstTurretFix
    {
        private static void Postfix(PLBurstTurret __instance, int inLevel, int inSubTypeData)
        {
            __instance.ProjSpeed = 3600f;
            __instance.m_BurstShotTiming = 0.1f;
        }
    }
}
