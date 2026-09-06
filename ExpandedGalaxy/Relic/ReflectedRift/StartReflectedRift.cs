using HarmonyLib;
using PulsarModLoader.Content.Components.WarpDrive;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLPickupMissionBase), "Start")]
    internal class StartReflectedRift
    {
        private static IEnumerator SetupReflectedRift()
        {
            yield return new WaitForSeconds(2);
            if (PLGlobal.Instance.Galaxy != null && PLGlobal.Instance.Galaxy.GetSectorOfVisualIndication((ESectorVisualIndication)145) == null)
            {
                List<PLSectorInfo> addedSectors = new List<PLSectorInfo>();
                int freeSectorNum = PLGlobal.Instance.Galaxy.GetMinimumFreeSectorNumber();
                PLRand rand = new PLRand((int)PLServer.Instance.GalaxySeed);
                PLSectorInfo startSector = new PLSectorInfo();
                startSector.ID = freeSectorNum;
                startSector.Discovered = false;
                startSector.Visited = false;
                startSector.MySPI = SectorProceduralInfo.Create(PLGlobal.Instance.Galaxy, ref startSector, startSector.ID);
                startSector.FactionStrength = 0.5f;
                startSector.MySPI.Faction = 6;
                startSector.VisualIndication = (ESectorVisualIndication)145;
                startSector.Position = new Vector3(-20.02f, -20.02f, (float)rand.NextDouble() * 0.0225f);
                PLGlobal.Instance.Galaxy.AllSectorInfos.Add(freeSectorNum, startSector);
                addedSectors.Add(startSector);

                freeSectorNum = PLGlobal.Instance.Galaxy.GetMinimumFreeSectorNumber();
                PLSectorInfo centerSector = new PLSectorInfo();
                centerSector.ID = freeSectorNum;
                centerSector.Discovered = false;
                centerSector.Visited = false;
                centerSector.MySPI = SectorProceduralInfo.Create(PLGlobal.Instance.Galaxy, ref centerSector, centerSector.ID);
                centerSector.FactionStrength = 0.5f;
                centerSector.MySPI.Faction = 6;
                centerSector.VisualIndication = ESectorVisualIndication.NONE;
                centerSector.Position = new Vector3(-20.1f, -20.1f, (float)rand.NextDouble() * 0.0225f);
                PLGlobal.Instance.Galaxy.AllSectorInfos.Add(freeSectorNum, centerSector);
                addedSectors.Add(centerSector);

                freeSectorNum = PLGlobal.Instance.Galaxy.GetMinimumFreeSectorNumber();
                PLSectorInfo endSector = new PLSectorInfo();
                endSector.ID = freeSectorNum;
                endSector.Discovered = false;
                endSector.Visited = false;
                endSector.MySPI = SectorProceduralInfo.Create(PLGlobal.Instance.Galaxy, ref endSector, endSector.ID);
                endSector.FactionStrength = 0.5f;
                endSector.MySPI.Faction = 6;
                endSector.VisualIndication = ESectorVisualIndication.GREY_PLAINS;
                endSector.Position = new Vector3(-40f, -40f, (float)rand.NextDouble() * 0.0225f);
                PLGlobal.Instance.Galaxy.AllSectorInfos.Add(freeSectorNum, endSector);
                addedSectors.Add(endSector);

                List<int> visualsToAdd = new List<int> { 147, 147, 147, 147, 147, 147, 147, 147, 60, 15 };
                List<PLSectorInfo> sectorsToAdd = new List<PLSectorInfo>();

                for (int i = 0; i < 10; i++)
                {
                    freeSectorNum = PLGlobal.Instance.Galaxy.GetMinimumFreeSectorNumber();
                    PLSectorInfo newSector = new PLSectorInfo();
                    newSector.ID = freeSectorNum;
                    newSector.Discovered = false;
                    newSector.Visited = false;
                    newSector.MySPI = SectorProceduralInfo.Create(PLGlobal.Instance.Galaxy, ref newSector, newSector.ID);
                    newSector.FactionStrength = 0.5f;
                    newSector.MySPI.Faction = 6;
                    int visual = visualsToAdd[rand.Next(visualsToAdd.Count)];
                    newSector.VisualIndication = (ESectorVisualIndication)visual;
                    visualsToAdd.Remove(visual);
                    newSector.Position = new Vector3(-20.1f - 0.06f + (float)(rand.NextDouble() * 0.12), -20.1f - 0.06f + (float)(rand.NextDouble() * 0.12), (float)rand.NextDouble() * 0.0225f);
                    if (visual == 0)
                    {
                        sectorsToAdd.Add(newSector);
                    }
                    else if (visual == 15)
                    {
                        PLPersistantShipInfo pLPersistantShipInfo = new PLPersistantShipInfo(EShipType.OLDWARS_HUMAN, 6, newSector, ensureNoCrew: true);
                        pLPersistantShipInfo.CompOverrides.Add(new ComponentOverrideData()
                        {
                            CompType = (int)ESlotType.E_COMP_WARP,
                            CompSubType = (int)WarpDriveModManager.Instance.GetWarpDriveIDFromName("Broken Warp Drive"),
                            ReplaceExistingComp = true,
                            CompLevel = 0,
                            IsCargo = false,
                            CompTypeToReplace = (int)ESlotType.E_COMP_WARP,
                            SlotNumberToReplace = 0
                        });
                        pLPersistantShipInfo.CompOverrides.Add(new ComponentOverrideData()
                        {
                            CompType = (int)ESlotType.E_COMP_REAC_COOLING,
                            CompSubType = 3,
                            CompLevel = 0,
                            IsCargo = false,
                            CompSubTypeToReplace = (int)ESlotType.E_COMP_REAC_COOLING,
                            SlotNumberToReplace = 0,
                            ReplaceExistingComp = false
                        });
                        pLPersistantShipInfo.CompOverrides.Add(new ComponentOverrideData()
                        {
                            CompType = (int)ESlotType.E_COMP_REAC_COOLING,
                            CompSubType = 4,
                            CompLevel = 0,
                            IsCargo = false,
                            CompSubTypeToReplace = (int)ESlotType.E_COMP_REAC_COOLING,
                            SlotNumberToReplace = 1,
                            ReplaceExistingComp = false
                        });
                        pLPersistantShipInfo.CompOverrides.Add(new ComponentOverrideData()
                        {
                            CompType = (int)ESlotType.E_COMP_REAC_COOLING,
                            CompSubType = 5,
                            CompLevel = 0,
                            IsCargo = false,
                            CompSubTypeToReplace = (int)ESlotType.E_COMP_REAC_COOLING,
                            SlotNumberToReplace = 2,
                            ReplaceExistingComp = false
                        });
                        pLPersistantShipInfo.HullPercent = 0.13f;
                        pLPersistantShipInfo.ShipName = "Derilect Ship";
                        PLServer.Instance.AllPSIs.Add(pLPersistantShipInfo);
                    }
                    PLGlobal.Instance.Galaxy.AllSectorInfos.Add(newSector.ID, newSector);
                    addedSectors.Add(newSector);
                }

                for (int i = 0; i < 5; i++)
                {
                    PLSectorInfo sector = sectorsToAdd[rand.Next(sectorsToAdd.Count)];
                    PLPersistantShipInfo pLPersistantShipInfo = new PLPersistantShipInfo(EShipType.E_WDDRONE2, 0, sector)
                    {
                        HullPercent = rand.Next(0.85f, 1f),
                        ShldPercent = 1f,
                        IsFlagged = true,
                        Modifiers = 2048
                    };
                    PLServer.Instance.AllPSIs.Add(pLPersistantShipInfo);
                    sectorsToAdd.Remove(sector);
                }
                yield return new WaitForEndOfFrame();
                for (int i = 0; i < addedSectors.Count; i++)
                {
                    PLServer.Instance.photonView.RPC("ClientInitialGetSectorData", PhotonTargets.Others, (object)PLServer.StarmapDataFromSector(addedSectors[i]), (object)addedSectors[i].Position);
                    PLServer.Instance.photonView.RPC("ClientUpdateSectorName", PhotonTargets.Others, (object)addedSectors[i].ID, (object)addedSectors[i].Name);
                    PLServer.Instance.photonView.RPC("ClientUpdateMissionID", PhotonTargets.Others, (object)addedSectors[i].ID, (object)addedSectors[i].MissionSpecificID);
                    yield return new WaitForEndOfFrame();
                }
            }
        }
        private static void Postfix(PLPickupMissionBase __instance)
        {
            if (!PhotonNetwork.isMasterClient)
                return;
            if (__instance.MissionTypeID == 8000014 && PLServer.Instance != null && PLGlobal.Instance.Galaxy != null && PLGlobal.Instance.Galaxy.GetSectorOfVisualIndication((ESectorVisualIndication)145) == null)
            {
                PLServer.Instance.StartCoroutine(SetupReflectedRift());
            }
        }
    }
}
