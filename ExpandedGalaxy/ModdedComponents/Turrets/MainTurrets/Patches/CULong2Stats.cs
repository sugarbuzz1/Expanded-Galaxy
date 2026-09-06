using HarmonyLib;
using System;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLMegaTurretCU_2), MethodType.Constructor, new Type[2] { typeof(int), typeof(int) })]
    internal class CULong2Stats
    {
        private static void Postfix(PLMegaTurretCU_2 __instance, int inLevel, int inSubTypeData, ref float ___m_MaxPowerUsage_Watts, ref float ___TurretRange)
        {
            ___m_MaxPowerUsage_Watts = 11500f;
            ___TurretRange = 11000f;
            __instance.Desc = "An impressive beam weapon that was designed for use in the Colonial Union Fleet. This one has had custom modifications for extra damage. It has a max range of 11 km. ";
        }
    }
}
