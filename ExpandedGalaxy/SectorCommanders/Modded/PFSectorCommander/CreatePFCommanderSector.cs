using HarmonyLib;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLGalaxy), "Setup")]
    internal class CreatePFCommanderSector
    {
        private static void Postfix(PLGalaxy __instance, ref PLRand ___m_RandomGenerator)
        {
            if (__instance.AllSectorInfos.Count == 0)
                return;
            for (int index = 0; index < 3000; ++index)
            {
                PLSectorInfo randomSectorInfo = __instance.GetRandomSectorInfo(___m_RandomGenerator.Next());
                if (randomSectorInfo.VisualIndication == ESectorVisualIndication.NONE && randomSectorInfo.MySPI.Faction != 4)
                {
                    randomSectorInfo.VisualIndication = ESectorVisualIndication.ALCHEMIST;
                    randomSectorInfo.MySPI.Faction = 5;
                    break;
                }
            }
        }
    }
}
