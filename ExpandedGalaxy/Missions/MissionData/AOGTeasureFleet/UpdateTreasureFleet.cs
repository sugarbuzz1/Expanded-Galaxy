using HarmonyLib;
using PulsarModLoader;
using System.Collections.Generic;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLServer), "Update")]
    internal class UpdateTreasureFleet
    {
        internal static int fleetUpdateTime = 0;
        private static void Postfix(PLServer __instance)
        {
            if (!PhotonNetwork.isMasterClient)
                return;
            if (!__instance.HasActiveMissionWithID(8000008))
                return;
            if (PLServer.Instance.GetActiveMissionWithID(8000008).MyMissionData.Objectives[0].Data.ContainsKey("ExGal_NPC_SectorCurrent"))
            {
                if (PLServer.Instance.GetActiveMissionWithID(8000008).MyMissionData.Objectives[0].Data["ExGal_NPC_SectorCurrent"] == "")
                {
                    foreach (PLPersistantShipInfo pLPersistantShipInfo in __instance.AllPSIs)
                    {
                        if (pLPersistantShipInfo.Type == EShipType.E_WDCRUISER && pLPersistantShipInfo.SelectedActorID == "ExGal_TreasureFleet_Cruiser")
                        {
                            PLServer.Instance.GetActiveMissionWithID(8000008).MyMissionData.Objectives[0].Data["ExGal_NPC_SectorCurrent"] = pLPersistantShipInfo.MyCurrentSector.ID.ToString();
                            ModMessage.SendRPC("sugarbuzz1.ExpandedGalaxy", "ExpandedGalaxy.ServerSendNPCSector", PhotonTargets.Others, new object[3] { 8000008, 0, pLPersistantShipInfo.MyCurrentSector.ID });
                            break;
                        }
                    }
                }
                else if (!PLMissionObjective_Custom.IDIsCompleted("ExGal_TreasureFleet_Intercept"))
                {
                    int currentSector = -1;
                    try
                    {
                        currentSector = int.Parse(PLServer.Instance.GetActiveMissionWithID(8000008).MyMissionData.Objectives[0].Data["ExGal_NPC_SectorCurrent"]);
                    }
                    catch { }
                    if (PLServer.GetCurrentSector() != null && PLServer.GetCurrentSector().ID == currentSector)
                        PLMissionObjective_Custom.OnCustomObjEvent("ExGal_TreasureFleet_Intercept");
                }
            }
            if (__instance.GetEstimatedServerMs() - fleetUpdateTime > 0)
            {
                fleetUpdateTime = __instance.GetEstimatedServerMs() + 120000;
                PLPersistantShipInfo cruiserInfo = null;
                PLPersistantShipInfo destroyerInfo = null;
                PLPersistantShipInfo droneInfo = null;
                PLPersistantShipInfo friendInfo = null;
                foreach (PLPersistantShipInfo pLPersistantShipInfo in __instance.AllPSIs)
                {
                    if (pLPersistantShipInfo.SelectedActorID == "ExGal_TreasureFleet_Cruiser")
                    {
                        if (pLPersistantShipInfo.Type == EShipType.E_WDCRUISER)
                            cruiserInfo = pLPersistantShipInfo;
                        else if (pLPersistantShipInfo.Type == EShipType.E_DESTROYER)
                            destroyerInfo = pLPersistantShipInfo;
                        else if (pLPersistantShipInfo.Type == EShipType.E_WDDRONE2)
                            droneInfo = pLPersistantShipInfo;
                    }
                    else if (pLPersistantShipInfo.SelectedActorID == "ExGal_TreasureFleet_Friend" && pLPersistantShipInfo.Type == EShipType.E_STARGAZER)
                    {
                        friendInfo = pLPersistantShipInfo;
                    }
                    if (cruiserInfo != null && destroyerInfo != null && droneInfo != null && friendInfo != null)
                        break;
                }
                if (cruiserInfo == null)
                {
                    __instance.GetActiveMissionWithID(8000008).FailMission();
                    ModMessage.SendRPC("sugarbuzz1.ExpandedGalaxy", "ExpandedGalaxy.ServerSendNPCSector", PhotonTargets.Others, new object[3] { 8000008, 0, -1 });
                    return;
                }
                if (cruiserInfo.IsShipDestroyed)
                {
                    if (PLServer.Instance.GetActiveMissionWithID(8000008).MyMissionData.Objectives[0].Data.ContainsKey("ExGal_NPC_SectorCurrent") && PLServer.Instance.GetActiveMissionWithID(8000008).MyMissionData.Objectives[0].Data["ExGal_NPC_SectorCurrent"] != "-1")
                    {
                        PLServer.Instance.GetActiveMissionWithID(8000008).MyMissionData.Objectives[0].Data["ExGal_NPC_SectorCurrent"] = "-1";
                        ModMessage.SendRPC("sugarbuzz1.ExpandedGalaxy", "ExpandedGalaxy.ServerSendNPCSector", PhotonTargets.Others, new object[3] { 8000008, 0, -1 });
                    }
                    if (destroyerInfo != null)
                        PLServer.Instance.AllPSIs.Remove(destroyerInfo);
                    if (droneInfo != null)
                        PLServer.Instance.AllPSIs.Remove(droneInfo);
                    if (friendInfo != null)
                        PLServer.Instance.AllPSIs.Remove(friendInfo);
                    return;
                }

                if (PLServer.GetCurrentSector() != null && PLServer.GetCurrentSector() == cruiserInfo.MyCurrentSector)
                {
                    fleetUpdateTime = __instance.GetEstimatedServerMs() + 30000;
                    return;
                }

                PLSectorInfo wdHubSector = PLGlobal.Instance.Galaxy.GetSectorOfVisualIndication(ESectorVisualIndication.WD_START);
                if (wdHubSector == null)
                {
                    __instance.GetActiveMissionWithID(8000008).FailMission();
                    ModMessage.SendRPC("sugarbuzz1.ExpandedGalaxy", "ExpandedGalaxy.ServerSendNPCSector", PhotonTargets.Others, new object[3] { 8000008, 0, -1 });
                    PLServer.Instance.AllPSIs.Remove(cruiserInfo);
                    if (destroyerInfo != null)
                        PLServer.Instance.AllPSIs.Remove(destroyerInfo);
                    if (droneInfo != null)
                        PLServer.Instance.AllPSIs.Remove(droneInfo);
                    if (friendInfo != null)
                        PLServer.Instance.AllPSIs.Remove(friendInfo);
                    return;
                }
                List<PLSectorInfo> fleetPath = RelicCaravan.GetPathToSector_NPC(cruiserInfo.MyCurrentSector, wdHubSector, 0.12f, 2);
                if (fleetPath.Count < 2)
                {
                    PulsarModLoader.Utilities.Logger.Info("Could not properly find a path for Treasure Fleet!");
                    ModMessage.SendRPC("sugarbuzz1.ExpandedGalaxy", "ExpandedGalaxy.ServerSendNPCSector", PhotonTargets.Others, new object[3] { 8000008, 0, -1 });
                    __instance.GetActiveMissionWithID(8000008).FailMission();
                    PLServer.Instance.AllPSIs.Remove(cruiserInfo);
                    if (destroyerInfo != null)
                        PLServer.Instance.AllPSIs.Remove(destroyerInfo);
                    if (droneInfo != null)
                        PLServer.Instance.AllPSIs.Remove(droneInfo);
                    if (friendInfo != null)
                        PLServer.Instance.AllPSIs.Remove(friendInfo);
                    return;
                }
                else if (fleetPath[1].VisualIndication == ESectorVisualIndication.WD_START)
                {
                    cruiserInfo.IsShipDestroyed = true;
                    __instance.GetActiveMissionWithID(8000008).FailMission();
                    ModMessage.SendRPC("sugarbuzz1.ExpandedGalaxy", "ExpandedGalaxy.ServerSendNPCSector", PhotonTargets.Others, new object[3] { 8000008, 0, -1 });
                    PLServer.Instance.AllPSIs.Remove(cruiserInfo);
                    if (destroyerInfo != null)
                        PLServer.Instance.AllPSIs.Remove(destroyerInfo);
                    if (droneInfo != null)
                        PLServer.Instance.AllPSIs.Remove(droneInfo);
                    if (friendInfo != null)
                        PLServer.Instance.AllPSIs.Remove(friendInfo);
                    return;
                }
                else
                {
                    cruiserInfo.MyCurrentSector = fleetPath[1];
                    cruiserInfo.ShldPercent = 1f;
                    bool flag = fleetPath[1] == PLServer.GetCurrentSector();
                    if (flag)
                        cruiserInfo.CreateShipInstance(PLEncounterManager.Instance.GetCPEI());
                    if (destroyerInfo != null)
                    {
                        destroyerInfo.MyCurrentSector = fleetPath[1];
                        destroyerInfo.ShldPercent = 1f;
                        if (flag)
                            destroyerInfo.CreateShipInstance(PLEncounterManager.Instance.GetCPEI());
                    }
                    if (droneInfo != null)
                    {
                        droneInfo.MyCurrentSector = fleetPath[1];
                        droneInfo.ShldPercent = 1f;
                        if (flag)
                            droneInfo.CreateShipInstance(PLEncounterManager.Instance.GetCPEI());
                    }
                    if (friendInfo != null)
                    {
                        friendInfo.MyCurrentSector = fleetPath[1];
                        friendInfo.ShldPercent = 1f;
                        if (flag)
                            friendInfo.CreateShipInstance(PLEncounterManager.Instance.GetCPEI());
                    }
                    if (PLServer.Instance.GetActiveMissionWithID(8000008).MyMissionData.Objectives[0].Data.ContainsKey("ExGal_NPC_SectorCurrent"))
                    {
                        PLServer.Instance.GetActiveMissionWithID(8000008).MyMissionData.Objectives[0].Data["ExGal_NPC_SectorCurrent"] = fleetPath[1].ID.ToString();
                        int id;
                        CrewLogManager.Instance.GetPinOfName("W.D. FLEET", out id);
                        if (id != -1)
                            CrewLogManager.Instance.MovePin("W.D. FLEET", id, fleetPath[1].ID);
                        else
                            CrewLogManager.Instance.AddPin("W.D. FLEET", fleetPath[1].ID, PLGlobal.Instance.Galaxy.FactionColors[2], 4);

                        ModMessage.SendRPC("sugarbuzz1.ExpandedGalaxy", "ExpandedGalaxy.ServerSendNPCSector", PhotonTargets.Others, new object[3] { 8000008, 0, fleetPath[1].ID });
                    }
                }
            }
        }
    }
}

