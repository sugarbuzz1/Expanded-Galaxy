using HarmonyLib;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLServer), "ClaimShip")]
    internal class TreasureFleetClaim
    {
        private static bool Prefix(PLServer __instance, int inShipID)
        {
            PLShipInfo shipFromId = PLEncounterManager.Instance.GetShipFromID(inShipID) as PLShipInfo;
            if (shipFromId != null)
            {
                if (shipFromId.SelectedActorID == "ExGal_TreasureFleet_Friend")
                    PLMissionObjective_KillShipOfName.OnShipDeath(shipFromId);
                if (shipFromId.TeamID == 1 && shipFromId.PersistantShipInfo != null)
                    shipFromId.PersistantShipInfo.IsShipDestroyed = true;
            }
            return true;
        }
    }
}

