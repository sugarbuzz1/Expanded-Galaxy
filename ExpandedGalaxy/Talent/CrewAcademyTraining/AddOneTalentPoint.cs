using HarmonyLib;
using Talents.Framework;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLPlayer), "ServerRankTalent")]
    internal class AddOneTalentPoint
    {
        private static void Postfix(PLPlayer __instance, int inTalentID)
        {
            if (!PhotonNetwork.isMasterClient)
                return;
            if (inTalentID == TalentModManager.Instance.GetTalentIDFromName("Crew Academy Training"))
            {
                TalentInfo info = PLGlobal.GetTalentInfoForTalentType((ETalents)inTalentID);
                if (info == null || (int)__instance.Talents[inTalentID] > info.MaxRank || (int)__instance.TalentPointsAvailable <= 0)
                    return;
                foreach (PLPlayer player in PLServer.Instance.AllPlayers)
                {
                    if (player.TeamID == __instance.TeamID && player.GetClassID() != 0)
                        ++player.TalentPointsAvailable;
                }

            }
        }
    }
}
