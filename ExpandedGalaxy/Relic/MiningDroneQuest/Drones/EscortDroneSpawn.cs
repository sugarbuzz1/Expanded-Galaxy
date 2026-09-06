using HarmonyLib;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLShipInfoBase), "Update")]
    internal class EscortDroneSpawn
    {
        private static IEnumerator TimedShipWarpInDirectional(PLPersistantShipInfo newPSI, PLPersistantEncounterInstance pei, Vector3 spawnPos, Quaternion spawnRot)
        {
            PLServer.Instance.photonView.RPC("WarpInEffect", PhotonTargets.All, (object)spawnPos, (object)spawnRot);
            PLMusic.PostEvent("play_sx_ship_civiliantransport_warpin", PLEncounterManager.Instance.PlayerShip.Exterior.gameObject);
            PLServer.Instance.AllPSIs.Add(newPSI);
            yield return new WaitForSeconds(0.5f);
            newPSI.CreateShipInstance(pei);
            if (newPSI.ShipInstance != null)
            {
                newPSI.ShipInstance.Exterior.transform.position = spawnPos;
                newPSI.ShipInstance.Exterior.transform.rotation = spawnRot;
            }
            
        }

        private static Vector3 GetEmptyLocationForEscortDrone(PLPersistantEncounterInstance inPEI, PLPersistantShipInfo inPSI, out Quaternion outEntryDir)
        {
            if (PLServer.Instance != null)
            {
                if (PLGlobal.Instance.Galaxy != null)
                {
                    Vector3 hubLoc = Vector3.zero;
                    foreach (PLSectorInfo info in PLGlobal.Instance.Galaxy.AllSectorInfos.Values)
                    {
                        if (info.VisualIndication == ESectorVisualIndication.LAVA2)
                        {
                            hubLoc = info.Position;
                            break;
                        }
                    }
                    if (hubLoc != Vector3.zero)
                    {
                        Vector3 entryDir = (hubLoc - PLServer.GetCurrentSector().Position).normalized;
                        for (int i = 0; i < 18; i++)
                        {
                            Vector3 playerPos = PLEncounterManager.Instance.PlayerShip.GetCurrentSensorPosition();
                            Vector3 pos = entryDir * (700f + (i * 100f) + UnityEngine.Random.Range(0f, 500f));
                            pos.z = pos.y;
                            pos.x += playerPos.x;
                            pos.y = playerPos.y;
                            pos.z += playerPos.z;
                            inPEI.Check_IsPositionSafeForShipCheck(pos);
                            if (inPEI.Check_IsPositionSafeForShipCheck(pos))
                            {
                                outEntryDir = Quaternion.LookRotation((playerPos - pos).normalized);
                                return pos;
                            }
                        }
                    }
                }
            }
            outEntryDir = Quaternion.Euler(Vector3.zero);
            return Vector3.zero;
        }

        private static void Postfix(PLShipInfoBase __instance)
        {
            if (!MiningDroneQuest.dronesActive || !PhotonNetwork.isMasterClient)
                return;
            if (PLEncounterManager.Instance == null || PLEncounterManager.Instance.GetCPEI() == null)
                return;
            if (__instance.DistressSignalActive)
            {
                bool flag = false;
                if (__instance.GetIsPlayerShip())
                {
                    if (PhotonNetwork.isMasterClient && PLServer.Instance != null && !__instance.InWarp && PLServer.GetCurrentSector() != null && PLServer.Instance.GetCurrentHubID() > 0 && PLServer.GetCurrentSector().VisualIndication != ESectorVisualIndication.LCWBATTLE && PLServer.GetCurrentSector().VisualIndication != ESectorVisualIndication.TOPSEC && PLServer.GetCurrentSector().VisualIndication != ESectorVisualIndication.LAVA2)
                    {
                        PLDistressSignal component = __instance.MyStats.GetComponentFromNetID<PLDistressSignal>(__instance.SelectedDistressSignalNetID);
                        if (component != null && (component is MiningDroneSignal))
                        {
                            flag = true;
                        }
                    }
                }
                else if (__instance.FactionID == 6 && __instance.ShipTypeID == EShipType.E_WDDRONE1 || __instance.ShipTypeID == EShipType.E_WDDRONE2)
                {
                    foreach (PLShipComponent component in __instance.MyStats.GetComponentsOfType(ESlotType.E_COMP_REAC_COOLING))
                    {
                        if (component is MiningDroneFlag)
                        {
                            if (component.Level < 4)
                            {
                                flag = true;
                                break;
                            }
                        }
                    }
                }
                if (flag)
                {
                    int num = 0;
                    foreach (PLPersistantShipInfo allPsI in PLServer.Instance.AllPSIs)
                    {
                        if (allPsI != null && allPsI.MyCurrentSector == PLServer.GetCurrentSector())
                            ++num;
                    }
                    if (num < 3 || PLServer.GetCurrentSector().VisualIndication == ESectorVisualIndication.NONE && num < 6)
                    {
                        if (UnityEngine.Random.Range(0, 1000 + num * 5000) == 15)
                        {
                            PLRand rand = new PLRand(PLServer.GetCurrentSector().ID * (1 + num));
                            PLPersistantShipInfo pLPersistantShipInfo = new PLPersistantShipInfo(EShipType.E_WDDRONE2, 6, PLServer.GetCurrentSector())
                            {
                                ShipName = "Escort Drone",
                                HullPercent = 1f,
                                ShldPercent = 1f,
                                IsFlagged = true,
                                ForcedHostile = true,

                            };
                            pLPersistantShipInfo.CompOverrides.AddRange((IEnumerable<ComponentOverrideData>)MiningDroneQuest.GetComponentsFromDroneType(1, rand));
                            Vector3 pos = GetEmptyLocationForEscortDrone(PLEncounterManager.Instance.GetCPEI(), pLPersistantShipInfo, out Quaternion entryDir);
                            if (pos != Vector3.zero)
                            {
                                PLServer.Instance.StartCoroutine(TimedShipWarpInDirectional(pLPersistantShipInfo, PLEncounterManager.Instance.GetCPEI(), pos, entryDir));
                            }
                        }
                    }
                }
            }
            if (__instance.IsDrone && __instance.CaptainTargetedSpaceTargetID != -1)
            {
                __instance.TargetSpaceTarget = PLEncounterManager.Instance.GetSpaceTargetFromID(__instance.CaptainTargetedSpaceTargetID);
            }
        }
    }
}
