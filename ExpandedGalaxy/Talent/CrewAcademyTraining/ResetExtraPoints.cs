using HarmonyLib;
using Talents.Framework;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLPlayer), "ResetTalentPoints")]
    internal class ResetExtraPoints
    {
        private static bool Prefix(PLPlayer __instance, out bool __state)
        {
            __state = false;
            if (__instance.Talents == null)
                return true;
            if (__instance.GetClassID() != 0)
                return true;
            if ((int)__instance.Talents[TalentModManager.Instance.GetTalentIDFromName("Crew Academy Training")] > 0)
                __state = true;

            return true;
        }
        private static void Postfix(PLPlayer __instance, bool __state)
        {
            if (__instance.GetClassID() != 0)
            {
                PLPlayer player = PLServer.Instance.GetCachedFriendlyPlayerOfClass(0);
                if (player != null)
                    __instance.TalentPointsAvailable += player.Talents[TalentModManager.Instance.GetTalentIDFromName("Crew Academy Training")];
            }
            else
            {
                if (__state)
                {
                    foreach (PLPlayer player in PLServer.Instance.AllPlayers)
                    {
                        if (player.TeamID == __instance.TeamID && player.GetPlayerID() != __instance.GetPlayerID() && player.GetClassID() != 0)
                            PLServer.Instance.photonView.RpcSecure("ResetTalentPointsOfPlayer", PhotonTargets.All, true, (object)player.GetPlayerID());

                    }
                }
            }
        }
    }
}
