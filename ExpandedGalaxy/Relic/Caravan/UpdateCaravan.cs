using HarmonyLib;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLServer), "Update")]
    internal class UpdateCaravan
    {
        internal static PLPersistantShipInfo persistantCaravanInfo = null;
        internal static List<ESectorVisualIndication> invalidTargets = new List<ESectorVisualIndication>();
        private static void Postfix()
        {
            if (PLServer.Instance == null)
                return;
            if (!PhotonNetwork.isMasterClient)
                return;
            if ((double)PLServer.Instance.lifetime < 10.0)
                return;
            if (persistantCaravanInfo == null)
            {
                foreach (PLPersistantShipInfo pLPersistantShipInfo in PLServer.Instance.AllPSIs)
                {
                    if (pLPersistantShipInfo.ShipName == "Wandering Caravan" && pLPersistantShipInfo.Type == EShipType.E_ROLAND && pLPersistantShipInfo.SelectedActorID == "ExGal_RelicCaravan")
                    {
                        persistantCaravanInfo = pLPersistantShipInfo;
                        break;
                    }
                }
            }
            if (persistantCaravanInfo == null || persistantCaravanInfo.IsShipDestroyed)
            {
                RelicCaravan.CaravanCurrentSector = -1;
                RelicCaravan.CaravanTargetSector = -1;
                return;
            }
            if (persistantCaravanInfo.CompOverrides.Count != 43)
            {
                persistantCaravanInfo.CompOverrides.Clear();
                persistantCaravanInfo.CompOverrides.AddRange(RelicCaravan.CaravanComponents((int)PLServer.Instance.GalaxySeed));
            }
            if (persistantCaravanInfo.OptionalTPDE != null)
                RelicCaravan.CaravanTraderData = persistantCaravanInfo.OptionalTPDE;
            if (persistantCaravanInfo.OptionalTPDE == null && RelicCaravan.CaravanTraderData != null)
                persistantCaravanInfo.OptionalTPDE = RelicCaravan.CaravanTraderData;
            if (persistantCaravanInfo.ShipInstance != null && !persistantCaravanInfo.ShipInstance.ShipNameValue.Contains("Wandering Caravan"))
                persistantCaravanInfo.ShipInstance.ShipNameValue = "Wandering Caravan";
            UpdateCaravanPath();
        }

        private static void UpdateCaravanShop()
        {
            if (persistantCaravanInfo.OptionalTPDE != null)
            {
                foreach (int wareID in persistantCaravanInfo.OptionalTPDE.Wares.Keys.ToList())
                    if (persistantCaravanInfo.OptionalTPDE.Wares[wareID] == null)
                        persistantCaravanInfo.OptionalTPDE.Wares.Remove(wareID);
                while (persistantCaravanInfo.OptionalTPDE.Wares.Count < 30)
                    persistantCaravanInfo.OptionalTPDE.ServerAddWare(PLShipComponent.CreateRandom());
                List<int> keys = persistantCaravanInfo.OptionalTPDE.Wares.Keys.ToList();
                while (persistantCaravanInfo.OptionalTPDE.Wares.Count > 30 && keys.Count > 0)
                {
                    int randomKey = keys[UnityEngine.Random.Range(0, keys.Count)];
                    persistantCaravanInfo.OptionalTPDE.Wares.Remove(randomKey);
                    keys.Remove(randomKey);
                }
                List<int> wareKeys = new List<int>();
                foreach (int wareID in persistantCaravanInfo.OptionalTPDE.Wares.Keys)
                {
                    if (UnityEngine.Random.Range(0f, 1000f) > 750f)
                        wareKeys.Add(wareID);
                }
                int num = 0;
                while (wareKeys.Count > 0)
                {
                    int currentKey = wareKeys[UnityEngine.Random.Range(0, wareKeys.Count)];
                    if (num == 0)
                    {
                        persistantCaravanInfo.OptionalTPDE.Wares.Remove(currentKey);
                        persistantCaravanInfo.OptionalTPDE.ServerAddWare(RelicCaravan.GetSpecialOffer());
                        ++num;
                        wareKeys.Remove(currentKey);
                    }
                    else if (num == 1)
                    {
                        if (UnityEngine.Random.Range(0f, 1000f) > 900f)
                        {
                            persistantCaravanInfo.OptionalTPDE.Wares.Remove(currentKey);
                            persistantCaravanInfo.OptionalTPDE.ServerAddWare(RelicCaravan.GetSpecialOffer());
                            ++num;
                        }
                        else
                        {
                            persistantCaravanInfo.OptionalTPDE.Wares.Remove(currentKey);
                            persistantCaravanInfo.OptionalTPDE.ServerAddWare(PLShipComponent.CreateRandom());
                        }
                        wareKeys.Remove(currentKey);
                    }
                    else
                    {
                        persistantCaravanInfo.OptionalTPDE.Wares.Remove(currentKey);
                        persistantCaravanInfo.OptionalTPDE.ServerAddWare(PLShipComponent.CreateRandom());
                        wareKeys.Remove(currentKey);
                    }
                }
                if (PLServer.DoesPEIOfTypeExist_AndBeenVisited(ESectorVisualIndication.ANCIENT_SENTRY) && RelicCaravan.CaravanSpecialsData % 10 != 1)
                {
                    PLPersistantShipInfo psiWithShipType1 = PLServer.GetPSIWithShipType(EShipType.E_CORRUPTED_DRONE);
                    if (psiWithShipType1 == null || psiWithShipType1.IsShipDestroyed)
                    {
                        persistantCaravanInfo.OptionalTPDE.ServerAddWare(new AncientAutoLaser());
                        persistantCaravanInfo.OptionalTPDE.ServerAddWare(new AncientAutoLaser());
                        RelicCaravan.CaravanSpecialsData = (RelicCaravan.CaravanSpecialsData / 10) + 1;
                    }
                }

            }
        }

        private static void UpdateCaravanPath()
        {
            bool changePath = false;
            bool changeTarget = false;
            if (PLServer.Instance != null && (double)PLServer.Instance.lifetime > 10.0 && persistantCaravanInfo.MyCurrentSector != null && persistantCaravanInfo.MyCurrentSector.ID != RelicCaravan.CaravanCurrentSector)
            {
                if ((bool)PLNetworkManager.Instance.IsInternalBuild)
                    Debug.Log("[ExGal] Caravan in wrong sector! Moving Caravan...");
                RelicCaravan.CaravanCurrentSector = persistantCaravanInfo.MyCurrentSector.ID;
                changePath = true;
                RelicCaravan.CaravanUpdateTime = PLServer.Instance.GetEstimatedServerMs() + 120000;
            }
            if ((RelicCaravan.CaravanCurrentSector != -1 && PLServer.GetCurrentSector() != null))
            {
                if ((PLServer.GetCurrentSector().ID != RelicCaravan.CaravanCurrentSector))
                {
                    if (PLServer.Instance.GetEstimatedServerMs() - RelicCaravan.CaravanUpdateTime > 0 || PLServer.Instance.GetEstimatedServerMs() - RelicCaravan.CaravanUpdateTime < -240000)
                    {
                        RelicCaravan.CaravanUpdateTime = PLServer.Instance.GetEstimatedServerMs() + 120000;
                        if (PLServer.Instance.HasYetToStartMissionWithID(8000017) && PLServer.Instance.DoesRequirementListPass(((PickupMissionData)PLCampaignIO.Instance.GetMissionOfTypeID(8000017)).StartingRequirements))
                            PLServer.Instance.AttemptStartMissionOfTypeID(8000017, true, new PhotonMessageInfo());
                        if (RelicCaravan.CaravanTargetSector != -1)
                        {
                            if (RelicCaravan.CaravanPath.Count > 1 && RelicCaravan.CaravanPath.Count > RelicCaravan.CaravanPathIndex + 1)
                            {
                                if (RelicCaravan.CaravanPath[RelicCaravan.CaravanPathIndex + 1].MySPI.Faction == 4 || RelicCaravan.CaravanPath[RelicCaravan.CaravanPathIndex + 1].DistressSignalActive)
                                {
                                    changePath = true;
                                    changeTarget = true;
                                    if ((bool)PLNetworkManager.Instance.IsInternalBuild)
                                        Debug.Log("[ExGal] Caravan path has dangerous sector! Changing target...");
                                }
                                else
                                {
                                    RelicCaravan.CaravanCurrentSector = RelicCaravan.CaravanPath[RelicCaravan.CaravanPathIndex + 1].ID;
                                    persistantCaravanInfo.MyCurrentSector = RelicCaravan.CaravanPath[RelicCaravan.CaravanPathIndex + 1];
                                    persistantCaravanInfo.ShldPercent = 1f;
                                    RelicCaravan.CaravanPathIndex++;
                                }
                                if (RelicCaravan.CaravanCurrentSector == RelicCaravan.CaravanTargetSector)
                                {
                                    persistantCaravanInfo.HullPercent = 1f;
                                    UpdateCaravanShop();
                                    changePath = true;
                                    changeTarget = true;
                                    if ((bool)PLNetworkManager.Instance.IsInternalBuild)
                                        Debug.Log("[ExGal] Caravan arrived at destination! Changing target...");
                                }
                            }
                            else
                            {
                                changePath = true;
                                if ((bool)PLNetworkManager.Instance.IsInternalBuild)
                                    Debug.Log("[ExGal] No Caravan path! Changing path...");
                            }
                        }
                        else
                        {
                            changePath = true;
                            changeTarget = true;
                            if ((bool)PLNetworkManager.Instance.IsInternalBuild)
                                Debug.Log("[ExGal] No Caravan target! Changing target...");
                        }
                    }
                }
                else
                {
                    if (PLEncounterManager.Instance != null && PLEncounterManager.Instance.GetCPEI() != null && (double)PLEncounterManager.Instance.GetCPEI().GetTimePlayerInEncounterAfterWarp() > 1.0 && persistantCaravanInfo.ShipInstance == null)
                        persistantCaravanInfo.CreateShipInstance(PLEncounterManager.Instance.GetCPEI());
                }
            }
            if (changePath)
            {
                int targetID = RelicCaravan.CaravanTargetSector;
                if (changeTarget)
                {
                    RelicCaravan.CaravanUpdateTime = PLServer.Instance.GetEstimatedServerMs() + 240000;
                    List<ESectorVisualIndication> potentialTargets = new List<ESectorVisualIndication>()
                                        {
                                        ESectorVisualIndication.CORNELIA_HUB,
                                        ESectorVisualIndication.DESERT_HUB,
                                        ESectorVisualIndication.AOG_HUB,
                                        ESectorVisualIndication.GENTLEMEN_START,
                                        ESectorVisualIndication.THE_HARBOR
                                        };
                    if (RelicCaravan.CaravanTargetSector != -1 && PLServer.GetSectorWithID(RelicCaravan.CaravanCurrentSector) != null && !invalidTargets.Contains(PLServer.GetSectorWithID(RelicCaravan.CaravanCurrentSector).VisualIndication))
                        invalidTargets.Add(PLServer.GetSectorWithID(RelicCaravan.CaravanCurrentSector).VisualIndication);
                    if (RelicCaravan.CaravanCurrentSector != -1 && PLServer.GetSectorWithID(RelicCaravan.CaravanCurrentSector) != null && potentialTargets.Contains(PLServer.GetSectorWithID(RelicCaravan.CaravanCurrentSector).VisualIndication) && !invalidTargets.Contains(PLServer.GetSectorWithID(RelicCaravan.CaravanCurrentSector).VisualIndication))
                        invalidTargets.Add(PLServer.GetSectorWithID(RelicCaravan.CaravanCurrentSector).VisualIndication);
                    if (PLServer.Instance.HasCompletedMissionWithID(8000001) && !invalidTargets.Contains(ESectorVisualIndication.DESERT_HUB))
                        invalidTargets.Add(ESectorVisualIndication.DESERT_HUB);
                    potentialTargets.RemoveAll(item => invalidTargets.Contains(item));
                    if (potentialTargets.Count > 0)
                    {
                        ESectorVisualIndication target = potentialTargets[UnityEngine.Random.Range(0, potentialTargets.Count)];
                        if ((bool)PLNetworkManager.Instance.IsInternalBuild)
                            Debug.Log("[ExGal] Caravan selected target: " + target.ToString());
                        if (PLGlobal.Instance.Galaxy.GetSectorOfVisualIndication(target) != null)
                            targetID = PLGlobal.Instance.Galaxy.GetSectorOfVisualIndication(target).ID;
                        else
                        {
                            if ((bool)PLNetworkManager.Instance.IsInternalBuild)
                                Debug.Log("[ExGal] Caravan target was invalid!");
                            invalidTargets.Add(target);
                            targetID = -1;
                        }
                    }
                    else
                    {
                        if ((bool)PLNetworkManager.Instance.IsInternalBuild)
                            Debug.Log("[ExGal] No valid Caravan targets! Moving to Cornelia...");
                        PLSectorInfo corneliaSector = PLGlobal.Instance.Galaxy.GetSectorOfVisualIndication(ESectorVisualIndication.CORNELIA_HUB);
                        if (corneliaSector != null)
                        {
                            if (RelicCaravan.CaravanCurrentSector != corneliaSector.ID)
                            {
                                persistantCaravanInfo.MyCurrentSector = corneliaSector;
                                RelicCaravan.CaravanCurrentSector = corneliaSector.ID;
                                RelicCaravan.CaravanPath.Clear();
                                RelicCaravan.CaravanPathIndex = 0;
                                invalidTargets.Clear();
                            }
                            else
                            {
                                if ((bool)PLNetworkManager.Instance.IsInternalBuild)
                                    Debug.Log("[ExGal] Caravan stuck at Cornelia!");
                            }
                        }
                    }
                }
                RelicCaravan.CaravanTargetSector = targetID;
                RelicCaravan.CaravanPath.Clear();
                if (RelicCaravan.CaravanTargetSector != -1)
                {
                    if ((bool)PLNetworkManager.Instance.IsInternalBuild)
                        Debug.Log("[ExGal] Finding Caravan path...");
                    RelicCaravan.CaravanPath = RelicCaravan.GetPathToSector_NPC(PLServer.GetSectorWithID(RelicCaravan.CaravanCurrentSector), PLServer.GetSectorWithID(RelicCaravan.CaravanTargetSector), 0.12f);
                    RelicCaravan.CaravanPathIndex = 0;
                    if (!(RelicCaravan.CaravanPath.Count > 1))
                    {
                        if ((bool)PLNetworkManager.Instance.IsInternalBuild)
                            Debug.Log("[ExGal] No valid Caravan path for target!");
                        invalidTargets.Add(PLServer.GetSectorWithID(RelicCaravan.CaravanTargetSector).VisualIndication);
                        RelicCaravan.CaravanTargetSector = -1;
                        RelicCaravan.CaravanUpdateTime = RelicCaravan.CaravanUpdateTime = PLServer.Instance.GetEstimatedServerMs() + 4000;
                    }
                    else
                        invalidTargets.Clear();
                }
            }
        }
    }
}
