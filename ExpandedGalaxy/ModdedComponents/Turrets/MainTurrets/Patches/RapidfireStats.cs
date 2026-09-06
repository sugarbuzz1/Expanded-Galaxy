using CodeStage.AntiCheat.ObscuredTypes;
using HarmonyLib;
using System;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLMegaTurret_RapidFire), MethodType.Constructor, new Type[2] { typeof(int), typeof(int) })]
    internal class RapidfireStats
    {
        private static void Postfix(PLMegaTurret_RapidFire __instance, int inLevel, int inSubTypeData, ref ObscuredInt ___m_MarketPrice, ref float ___m_MaxPowerUsage_Watts)
        {
            ___m_MarketPrice = (ObscuredInt)19200;
            ___m_MaxPowerUsage_Watts = 11600f;
        }
    }
}
