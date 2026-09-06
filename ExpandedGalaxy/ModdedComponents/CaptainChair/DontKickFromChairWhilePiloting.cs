using HarmonyLib;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLShipInfo), "AttemptToSitInCaptainsChair")]
    internal class DontKickFromChairWhilePiloting
    {
        private static bool Prefix(PLShipInfo __instance, int playerID)
        {
            PLPlayer playerFromPlayerId = PLServer.Instance.GetPlayerFromPlayerID(playerID);
            if (playerID == -1 && !Systems.IsPlayerPiloting(__instance.CaptainsChairPlayerID))
            {
                __instance.CaptainsChairPlayerID = -1;
            }
            else
            {
                if (__instance.CaptainsChairPlayerID != -1 && (!((UnityEngine.Object)playerFromPlayerId != (UnityEngine.Object)null) || playerFromPlayerId.GetClassID() != 0))
                    return false;
                if (!Systems.IsPlayerPiloting(__instance.CaptainsChairPlayerID))
                    __instance.CaptainsChairPlayerID = playerID;
            }
            return false;
        }
    }
}
