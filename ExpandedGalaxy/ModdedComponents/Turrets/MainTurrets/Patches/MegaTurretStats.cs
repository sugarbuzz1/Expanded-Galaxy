using HarmonyLib;
using System;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLMegaTurret), MethodType.Constructor, new Type[1] { typeof(int) })]
    internal class MegaTurretStats
    {
        private static void Postfix(PLMegaTurret __instance, int inLevel, ref float ___m_MaxPowerUsage_Watts)
        {
            ___m_MaxPowerUsage_Watts = 6800f;
        }
    }
}
