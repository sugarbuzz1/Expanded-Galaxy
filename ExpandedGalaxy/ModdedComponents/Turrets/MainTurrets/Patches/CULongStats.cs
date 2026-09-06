using HarmonyLib;
using System;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLMegaTurretCU), MethodType.Constructor, new Type[2] { typeof(int), typeof(int) })]
    internal class CULongStats
    {
        private static void Postfix(PLMegaTurretCU __instance, int inLevel, int inSubTypeData, ref float ___m_MaxPowerUsage_Watts)
        {
            ___m_MaxPowerUsage_Watts = 11500f;
        }
    }
}
