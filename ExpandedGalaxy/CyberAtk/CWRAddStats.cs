using HarmonyLib;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLCPU), "AddStats")]
    internal class CWRAddStats
    {
        private static void Postfix(PLCPU __instance, PLShipStats inStats)
        {
            if (__instance.CPUClass == ECPUClass.CYBERWARFARE_MODULE)
            {
                inStats.CyberAttackRating -= (float)(5.0 * (double)__instance.LevelMultiplier(0.5f) * 0.0099999997764825821);
                inStats.CyberAttackRating += (0.225f * __instance.LevelMultiplier(0.75f) * __instance.GetPowerPercentInput());
            }
        }
    }
}
