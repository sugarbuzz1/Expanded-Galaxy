using CodeStage.AntiCheat.ObscuredTypes;
using HarmonyLib;
using System;
using System.Collections.Generic;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLPlayer), "Start")]
    internal class TalentsFixFix
    {
        private static bool Prefix(PLPlayer __instance, out Dictionary<int, int> __state)
        {
            __state = new Dictionary<int, int>();
            for (int i = 0; i < __instance.Talents.Length; i++)
            {
                if ((int)__instance.Talents[i] != 0)
                    __state.Add(i, (int)__instance.Talents[i]);
            }
            return true;
        }

        private static Exception Finalizer(Exception __exception, PLPlayer __instance, Dictionary<int, int> __state)
        {
            foreach (int talentID in __state.Keys)
            {
                __instance.Talents[talentID] = (ObscuredInt)__state[talentID];
            }
            return __exception;
        }
    }
}
