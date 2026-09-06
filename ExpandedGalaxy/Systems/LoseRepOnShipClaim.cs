using CodeStage.AntiCheat.ObscuredTypes;
using HarmonyLib;
using UnityEngine;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLServer), "ClaimShip")]
    internal class LoseRepOnShipClaim
    {
        private static bool Prefix(PLServer __instance, int inShipID)
        {
            PLShipInfo shipFromId = PLEncounterManager.Instance.GetShipFromID(inShipID) as PLShipInfo;
            if (shipFromId == null || shipFromId == PLEncounterManager.Instance.PlayerShip)
                return true;
            if (!shipFromId.Abandoned && !shipFromId.IsFlagged && !shipFromId.NoRepLossOnKilled && shipFromId.TeamID == 1 && shipFromId.FactionID != -1 && shipFromId.FactionID < 6)
            {
                if (!PLServer.Instance.LongRangeCommsDisabled)
                {
                    ref ObscuredInt local = ref PLServer.Instance.RepLevels[shipFromId.FactionID];
                    local = (ObscuredInt)((int)local - 2);
                    PLServer.Instance.AddToShipLog_OneStringLocalized("REP", "-2 Rep for [STR0] (due to removing claim)", Color.white, inString0: PLGlobal.GetFactionTextForFactionID(shipFromId.FactionID));
                }
                else
                    PLServer.Instance.AddToShipLog("REP", "Long range transmission blocked (prevented Reputation loss)", Color.white);
            }
            return true;
        }
    }
}

