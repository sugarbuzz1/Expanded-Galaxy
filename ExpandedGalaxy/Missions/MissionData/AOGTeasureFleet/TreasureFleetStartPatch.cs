using HarmonyLib;
using PulsarModLoader.Content.Components.AutoTurret;
using PulsarModLoader.Content.Components.MissionShipComponent;
using System.Collections.Generic;
using UnityEngine;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLPickupMissionBase), "Start")]
    internal class TreasureFleetStartPatch
    {
        private static void Postfix(PLPickupMissionBase __instance)
        {
            if (!PhotonNetwork.isMasterClient)
                return;
            if (__instance.MissionTypeID == 8000008 && PLServer.Instance != null && PLGlobal.Instance.Galaxy != null)
            {
                bool flag = false;
                foreach (PLPersistantShipInfo persistantShipInfo in PLServer.Instance.AllPSIs)
                {
                    if (persistantShipInfo.Type == EShipType.E_WDCRUISER && persistantShipInfo.SelectedActorID == "ExGal_TreasureFleet_Cruiser")
                    {
                        flag = true;
                        break;
                    }
                }
                if (flag)
                {
                    UpdateTreasureFleet.fleetUpdateTime = PLServer.Instance.GetEstimatedServerMs() + 120000;
                    return;
                }
                List<PLSectorInfo> potentialStartSectors = new List<PLSectorInfo>();
                PLSectorInfo wdHubSector = PLGlobal.Instance.Galaxy.GetSectorOfVisualIndication(ESectorVisualIndication.WD_START);
                if (wdHubSector == null)
                    return;
                float targetDistance = 200f;
                while (true)
                {
                    foreach (PLSectorInfo sectorInfo in PLGlobal.Instance.Galaxy.AllSectorInfos.Values)
                    {
                        if (sectorInfo.VisualIndication == ESectorVisualIndication.GENERAL_STORE && Vector3.Distance(sectorInfo.Position, wdHubSector.Position) * 250f > targetDistance * PLGlobal.Instance.Galaxy.CalculateGenGalaxyScaleFromGenSettings() && !sectorInfo.IsThisSectorWithinPlayerWarpRange() && RelicCaravan.GetPathToSector_NPC(sectorInfo, wdHubSector, 0.12f, 2).Count > 1)
                        {
                            potentialStartSectors.Add(sectorInfo);
                            break;
                        }
                    }
                    if (!(potentialStartSectors.Count > 0))
                        targetDistance -= 25f;
                    else
                        break;
                    if (targetDistance < 150f)
                        break;
                }
                if (!(potentialStartSectors.Count > 0))
                {
                    PulsarModLoader.Utilities.Logger.Info("Could not properly start mission Treasure Fleet! Galaxy was too small or there was too little General Stores!");
                    __instance.FailMission();
                    return;
                }
                PLRand rand = new PLRand(PLServer.Instance.GalaxySeed.GetDecrypted());
                PLSectorInfo startingSector = potentialStartSectors[rand.Next(0, potentialStartSectors.Count)];
                PLPersistantShipInfo persistantShipInfo1 = new PLPersistantShipInfo(EShipType.E_WDCRUISER, 2, startingSector, forcedHostileToFaction: 1);
                persistantShipInfo1.SelectedActorID = "ExGal_TreasureFleet_Cruiser";
                List<ComponentOverrideData> componentOverrideDatas = new List<ComponentOverrideData>();
                for (int i = 0; i < 16; i++)
                {
                    componentOverrideDatas.Add(new ComponentOverrideData()
                    {
                        ReplaceExistingComp = true,
                        CompTypeToReplace = (int)ESlotType.E_COMP_CARGO,
                        CompType = (int)ESlotType.E_COMP_MISSION_COMPONENT,
                        CompSubType = MissionShipComponentModManager.Instance.GetMissionShipComponentIDFromName("Irradiated Cargo"),
                        CompLevel = 0,
                        IsCargo = true,
                        SlotNumberToReplace = i,
                    });
                }
                persistantShipInfo1.CompOverrides.AddRange(componentOverrideDatas);
                PLPersistantShipInfo destroyerInfo = new PLPersistantShipInfo(EShipType.E_DESTROYER, 2, startingSector, forcedHostileToShip: PLEncounterManager.Instance.PlayerShip.ShipID);
                destroyerInfo.SelectedActorID = "ExGal_TreasureFleet_Cruiser";
                destroyerInfo.CompOverrides.AddRange(new List<ComponentOverrideData>()
                            {
                                new ComponentOverrideData()
                                {
                                ReplaceExistingComp = true,
                                CompTypeToReplace = (int)ESlotType.E_COMP_AUTO_TURRET,
                                CompType = (int)ESlotType.E_COMP_AUTO_TURRET,
                                CompSubType = AutoTurretModManager.Instance.GetAutoTurretIDFromName("Auto Laser Turret"),
                                CompLevel = 0,
                                IsCargo = false,
                                SlotNumberToReplace = 0,
                                },
                                new ComponentOverrideData()
                                {
                                ReplaceExistingComp = true,
                                CompTypeToReplace = (int)ESlotType.E_COMP_AUTO_TURRET,
                                CompType = (int)ESlotType.E_COMP_AUTO_TURRET,
                                CompSubType = AutoTurretModManager.Instance.GetAutoTurretIDFromName("Auto Laser Turret"),
                                CompLevel = 0,
                                IsCargo = false,
                                SlotNumberToReplace = 1,
                                }
                            });
                PLPersistantShipInfo droneInfo = new PLPersistantShipInfo(EShipType.E_WDDRONE2, 2, startingSector, forcedHostileToShip: PLEncounterManager.Instance.PlayerShip.ShipID);
                droneInfo.SelectedActorID = "ExGal_TreasureFleet_Cruiser";
                PLPersistantShipInfo friendInfo = new PLPersistantShipInfo(EShipType.E_STARGAZER, 1, startingSector, forcedHostileToFaction: 2);
                friendInfo.SelectedActorID = "ExGal_TreasureFleet_Friend";
                friendInfo.ShipName = "The Milano";
                PLServer.Instance.AllPSIs.Add(persistantShipInfo1);
                PLServer.Instance.AllPSIs.Add(destroyerInfo);
                PLServer.Instance.AllPSIs.Add(droneInfo);
                PLServer.Instance.AllPSIs.Add(friendInfo);
                UpdateTreasureFleet.fleetUpdateTime = PLServer.Instance.GetEstimatedServerMs() + 6000;
                CrewLogManager.Instance.AddPin("W.D. FLEET", startingSector.ID, PLGlobal.Instance.Galaxy.FactionColors[2], 4);
            }
        }
    }
}

