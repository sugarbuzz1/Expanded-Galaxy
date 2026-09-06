using HarmonyLib;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLWDAnnihilatorInfo), "OnEndWarp")]
    internal class VulcanusShipDeliver
    {
        private static void Postfix(PLWDAnnihilatorInfo __instance)
        {
            if (__instance == null || !__instance.GetIsPlayerShip())
                return;
            if (PLServer.Instance.HasActiveMissionWithID(8000004))
            {
                if (PLServer.GetCurrentSector() == null)
                    return;
                if (PLServer.GetCurrentSector().VisualIndication != ESectorVisualIndication.RACING_SECTOR)
                {
                    if (PLMissionObjective.IDIsCompleted("ExGal_DeliverVulcanus"))
                    {
                        foreach (PLMissionObjective missionObjective in PLMissionObjective.AllMissionObjectives)
                        {
                            if (missionObjective != null && !missionObjective.IsCompleted && missionObjective is PLMissionObjective_Custom missionObjectiveCustom && missionObjectiveCustom.ScriptName == "ExGal_DeliverVulcanus")
                            {
                                --missionObjectiveCustom.AmountCompleted;
                                break;
                            }
                        }
                        return;
                    }
                }
                else
                {
                    bool flag = false;
                    foreach (PLShipComponent pLShipComponent in __instance.MyStats.AllComponents)
                    {
                        if (pLShipComponent is MissionShipFlagComp)
                        {
                            flag = true;
                            break;
                        }
                    }
                    if (flag)
                    {
                        PLMissionObjective_Custom.OnCustomObjEvent("ExGal_DeliverVulcanus");
                        if ((PLServer.Instance.GetActiveMissionWithID(8000004).MyMissionData as PickupMissionData).Sectors[0].Ships.Count < 2)
                            return;
                        PickupShipData shipData = (PLServer.Instance.GetActiveMissionWithID(8000004).MyMissionData as PickupMissionData).Sectors[0].Ships[1];
                        PLPersistantShipInfo shipInfo = new PLPersistantShipInfo((EShipType)shipData.ShipType, shipData.FactionID, PLServer.GetCurrentSector(), isFlagged: shipData.Flagged, ensureNoCrew: true);
                        shipInfo.CompOverrides.AddRange(shipData.AllComponentOverrides);
                        shipInfo.ShipName = shipData.Name;
                        shipInfo.SelectedActorID = shipData.DialogueActorID;
                        shipInfo.CreateShipInstance(PLEncounterManager.Instance.GetCPEI());
                        if (shipInfo.ShipInstance == null)
                            return;
                        PLShipInfo info = (PLShipInfo)shipInfo.ShipInstance;
                        info.Exterior.transform.position = new UnityEngine.Vector3(-2351f, 414f, -94f);
                        info.CreditsLeftBehind = 0;
                        info.SetAbandoned(true);
                    }
                }
            }
        }
    }
}

