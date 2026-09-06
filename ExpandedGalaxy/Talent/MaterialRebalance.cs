using HarmonyLib;
using System.Collections.Generic;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLGlobal), "GetTalentInfoForTalentType")]
    internal class MaterialRebalance
    {
        private static void Postfix(PLGlobal __instance, ETalents inTalent, ref Dictionary<int, TalentInfo> ___CachedTalentInfos, ref TalentInfo __result)
        {
            bool flag = false;
            switch (inTalent)
            {
                case ETalents.INC_JETPACK:
                case ETalents.PIL_REDUCE_SYS_DAMAGE:
                case ETalents.ANTI_RAD_INJECTION:
                case ETalents.CAP_SCREEN_SAFETY:
                case ETalents.SCI_PROBE_COOLDOWN:
                    flag = true;
                    break;
            }
            if (!flag)
                return;
            if (___CachedTalentInfos.ContainsKey((int)inTalent))
                ___CachedTalentInfos.Remove((int)inTalent);
            switch (inTalent)
            {
                case ETalents.INC_JETPACK:
                    __result.ResearchCost[0] = 2;
                    break;
                case ETalents.PIL_REDUCE_SYS_DAMAGE:
                    __result.ResearchCost[1] = 3;
                    __result.ResearchCost[4] = 2;
                    __result.ResearchCost[5] = 0;
                    break;
                case ETalents.ANTI_RAD_INJECTION:
                    __result.ResearchCost[1] = 2;
                    break;
                case ETalents.CAP_SCREEN_SAFETY:
                    __result.ResearchCost[0] = 2;
                    __result.ResearchCost[5] = 1;
                    break;
                case ETalents.SCI_PROBE_COOLDOWN:
                    __result.ResearchCost[3] = 1;
                    break;
            }
            ___CachedTalentInfos[(int)inTalent] = __result;
        }
    }
}
