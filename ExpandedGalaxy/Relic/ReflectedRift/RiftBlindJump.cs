using HarmonyLib;
using System.Collections.Generic;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLServer), "Internal_AttemptBlindJump")]
    internal class RiftBlindJump
    {
        private static bool Prefix(PLServer __instance, int inShipID, int playerID)
        {
            if (ReflectedRift.inRift)
            {
                double damage = (double)PLEncounterManager.Instance.PlayerShip.TakeDamage((float)(((double)PLEncounterManager.Instance.PlayerShip.MyStats.HullCurrent + (double)PLEncounterManager.Instance.PlayerShip.MyStats.ShieldsCurrent) * (double)UnityEngine.Random.Range(0.8f, 1.25f) * (double)UnityEngine.Random.Range(0.4f, 1f) * (double)UnityEngine.Random.Range(0.5f, 1f) + 100.0 + (double)PLEncounterManager.Instance.PlayerShip.MyStats.HullArmor * 190.0), false, EDamageType.E_PHYSICAL, UnityEngine.Random.Range(0.0f, 1f), -1, (PLShipInfoBase)null, -1);
                PLShipInfoBase shipFromId = PLEncounterManager.Instance.GetShipFromID(inShipID);
                if ((UnityEngine.Object)shipFromId == (UnityEngine.Object)null || shipFromId.InWarp)
                    return false;
                PLShipInfo plShipInfo1 = shipFromId as PLShipInfo;
                if (!((UnityEngine.Object)plShipInfo1 != (UnityEngine.Object)null) || !plShipInfo1.BlindJumpUnlocked)
                    return false;

                int num = -1;
                for (int i = 0; i < 200; i++)
                {
                    int num1 = UnityEngine.Random.Range(0, PLGlobal.Instance.Galaxy.AllSectorInfos.Keys.Count);
                    if (PLGlobal.Instance.Galaxy.AllSectorInfos.ContainsKey(num1))
                    {
                        PLSectorInfo info = PLGlobal.Instance.Galaxy.AllSectorInfos[num1];
                        if (info != null && info != PLServer.GetCurrentSector() && info.MissionSpecificID == -1 && info.MySPI != null && info.MySPI.Faction != 6 && info.VisualIndication != ESectorVisualIndication.LCWBATTLE && info.VisualIndication != ESectorVisualIndication.TOPSEC)
                        {
                            num = num1;
                            break;
                        }
                    }
                }
                if (num != -1)
                {
                    plShipInfo1.LastBeginBlindWarpServerTime = PLServer.Instance.GetEstimatedServerMs();
                    PLServer.Instance.photonView.RPC("NetworkBeginWarp", PhotonTargets.All, (object)inShipID, (object)num, (object)PLServer.Instance.GetEstimatedServerMs(), (object)-1);
                    ReflectedRift.SetRiftData(0, false);
                    PLServer.Instance.IsReflection = !PLServer.Instance.IsReflection;
                    if (PLServer.Instance.HasCompletedMissionWithID(8000014))
                    {
                        foreach (PLSectorInfo sector in PLGlobal.Instance.Galaxy.AllSectorInfos.Values)
                        {
                            if (sector != null && sector.VisualIndication == ESectorVisualIndication.DIMENSION_STATION && sector.MySPI.Faction == 6)
                            {
                                sector.VisualIndication = ESectorVisualIndication.WARP_NETWORK_STATION;
                                sector.MissionSpecificID = -1;
                                sector.Name = "Ancient Stargate";
                                PLEncounterManager.Instance.AllPersistantEncounterInstances.Remove(sector.ID);
                                if (PhotonNetwork.isMasterClient)
                                {
                                    List<PLPersistantShipInfo> shipInfos = new List<PLPersistantShipInfo>();
                                    foreach (PLPersistantShipInfo allPSI in PLServer.Instance.AllPSIs)
                                    {
                                        if (allPSI.MyCurrentSector != null && allPSI.MyCurrentSector == sector)
                                            shipInfos.Add(allPSI);
                                    }
                                    foreach (PLPersistantShipInfo psi in shipInfos)
                                        PLServer.Instance.AllPSIs.Remove(psi);
                                }

                                PLServer.Instance.photonView.RPC("ClientUpdateSectorData", PhotonTargets.Others, (object)PLServer.StarmapDataFromSector(sector));
                                PLServer.Instance.photonView.RPC("ClientUpdateSectorPosition", PhotonTargets.Others, (object)(short)sector.ID, (object)sector.Position);
                                PLServer.Instance.photonView.RPC("ClientUpdateSectorName", PhotonTargets.Others, (object)sector.ID, (object)sector.Name);
                                PLServer.Instance.photonView.RPC("ClientUpdateMissionID", PhotonTargets.Others, (object)sector.ID, (object)sector.MissionSpecificID);
                                break;
                            }
                        }
                    }
                }
                return false;
            }
            return true;
        }
    }
}
