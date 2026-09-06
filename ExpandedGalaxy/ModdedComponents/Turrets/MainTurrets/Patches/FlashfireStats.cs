using HarmonyLib;
using System;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLMegaTurret_WDExperiment), MethodType.Constructor, new Type[2] { typeof(int), typeof(int) })]
    internal class FlashfireStats
    {
        private static void Postfix(PLMegaTurret_WDExperiment __instance, int inLevel, int inSubTypeData, ref float ___m_MaxPowerUsage_Watts)
        {
            ___m_MaxPowerUsage_Watts = 11600f;
        }
    }
}
