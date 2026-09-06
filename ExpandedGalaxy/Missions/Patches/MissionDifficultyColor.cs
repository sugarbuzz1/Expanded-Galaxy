using HarmonyLib;
using UnityEngine;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLTabMenu), "GetColorForDifficulty")]
    internal class MissionDifficultyColor
    {
        private static bool Prefix(PLTabMenu __instance, int difficulty, ref Color __result)
        {
            if (difficulty == 4)
            {
                __result = Relic.GetRelicColor();
                return false;
            }
            return true;
        }
    }

}

