using HarmonyLib;
using UnityEngine;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLPersistantEncounterInstance), "ClearPSIs")]
    internal class MoveCaravan
    {
        private static bool Prefix(PLSectorInfo currentSector)
        {
            if (PLServer.Instance == null || currentSector == null)
                return false;
            foreach (PLPersistantShipInfo pLPersistantShipInfo in PLServer.Instance.AllPSIs)
            {
                if (pLPersistantShipInfo.ShipName == "Wandering Caravan" && pLPersistantShipInfo.Type == EShipType.E_ROLAND && pLPersistantShipInfo.SelectedActorID == "ExGal_RelicCaravan")
                {
                    if (pLPersistantShipInfo.MyCurrentSector != null && pLPersistantShipInfo.MyCurrentSector.ID == currentSector.ID)
                    {
                        float num1 = float.MaxValue;
                        PLSectorInfo plSectorInfo = (PLSectorInfo)null;
                        foreach (PLSectorInfo currentSector1 in PLGlobal.Instance.Galaxy.AllSectorInfos.Values)
                        {
                            if (currentSector1 != null && currentSector1 != currentSector && currentSector1.VisualIndication == ESectorVisualIndication.NONE && currentSector1.MySPI.Faction != 4)
                            {
                                float num2 = Vector3.SqrMagnitude(currentSector1.Position - currentSector.Position);
                                if ((double)num2 < (double)num1)
                                {
                                    num1 = num2;
                                    plSectorInfo = currentSector1;
                                }
                            }
                        }
                        if (plSectorInfo != null)
                        {
                            pLPersistantShipInfo.MyCurrentSector = plSectorInfo;
                            RelicCaravan.CaravanCurrentSector = plSectorInfo.ID;
                            RelicCaravan.CaravanPath.Clear();
                            RelicCaravan.CaravanPathIndex = 0;
                        }
                        else
                        {
                            PLSectorInfo corneliaSector = PLGlobal.Instance.Galaxy.GetSectorOfVisualIndication(ESectorVisualIndication.CORNELIA_HUB);
                            if (corneliaSector != null)
                            {
                                pLPersistantShipInfo.MyCurrentSector = corneliaSector;
                                RelicCaravan.CaravanCurrentSector = corneliaSector.ID;
                                RelicCaravan.CaravanPath.Clear();
                                UpdateCaravan.invalidTargets.Clear();
                                RelicCaravan.CaravanTargetSector = -1;
                                RelicCaravan.CaravanPathIndex = 0;
                            }
                            else
                                PLServer.Instance.AllPSIs.Remove(pLPersistantShipInfo);
                        }
                    }
                    break;
                }
            }
            return true;
        }
    }
}
