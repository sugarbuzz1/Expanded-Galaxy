using HarmonyLib;
using System.Collections.Generic;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLPersistantEncounterInstance), "PlayerEnter")]
    internal class SpawnFullFleet
    {
        private static void Postfix(PLPersistantEncounterInstance __instance, int inHubID)
        {
            if (!PhotonNetwork.isMasterClient)
                return;
            if (PLServer.Instance != null && PLServer.Instance.HasActiveMissionWithID(8000008))
            {
                if (PLServer.Instance.GetActiveMissionWithID(8000008).MyMissionData.Objectives[0].Data.ContainsKey("ExGal_NPC_SectorCurrent"))
                {
                    int currentSectorID = -1;
                    try
                    {
                        currentSectorID = int.Parse(PLServer.Instance.GetActiveMissionWithID(8000008).MyMissionData.Objectives[0].Data["ExGal_NPC_SectorCurrent"]);
                    }
                    catch { }
                    if (currentSectorID == -1 || currentSectorID != inHubID)
                        return;
                    PLSectorInfo sectorWithId = PLServer.GetSectorWithID(inHubID);
                    if (sectorWithId == null)
                        return;
                    List<PLPersistantShipInfo> pLPersistantShipInfos = new List<PLPersistantShipInfo>();
                    foreach (PLPersistantShipInfo allPsI in PLServer.Instance.AllPSIs)
                    {
                        if (allPsI != null && allPsI.MyCurrentSector == sectorWithId && (allPsI.FactionID == 2 || allPsI.SelectedActorID == "ExGal_TreasureFleet_Friend"))
                            pLPersistantShipInfos.Add(allPsI);
                    }
                    PLPersistantEncounterInstance.ClearPSIs(sectorWithId);
                    foreach (PLPersistantShipInfo PsI in pLPersistantShipInfos)
                        PLServer.Instance.AllPSIs.Add(PsI);
                }
            }
        }
    }
}

