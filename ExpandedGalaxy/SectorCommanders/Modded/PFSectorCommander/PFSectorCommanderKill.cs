using HarmonyLib;
using UnityEngine;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLMissionObjective_DestroySectorCommanders), "CheckIfCompleted")]
    internal class PFSectorCommanderKill
    {
        private static bool Prefix(PLMissionObjective_DestroySectorCommanders __instance)
        {
            if (PhotonNetwork.isMasterClient && (double)UnityEngine.Random.value < 0.0099999997764825821)
            {
                __instance.AmountCompleted = 0;
                if ((UnityEngine.Object)PLServer.Instance != (UnityEngine.Object)null && (double)PLServer.Instance.lifetime > 5.0)
                {
                    PLPersistantShipInfo psiWithShipType1 = PLServer.GetPSIWithShipType(EShipType.E_CORRUPTED_DRONE);
                    PLPersistantShipInfo psiWithShipType2 = PLServer.GetPSIWithShipType(EShipType.E_SWARM_CMDR);
                    PLPersistantShipInfo psiWithShipType3 = PLServer.GetPSIWithShipType(EShipType.E_INTREPID_SC);
                    PLPersistantShipInfo psiWithShipType4 = PLServer.GetPSIWithShipType(EShipType.E_ALCHEMIST);
                    PLPersistantShipInfo psiWithShipType5 = PLServer.GetPSIWithShipType(EShipType.E_DEATHSEEKER_DRONE_SC);
                    if (PLServer.DoesPEIOfTypeExist_AndBeenVisited(ESectorVisualIndication.ANCIENT_SENTRY))
                        __instance.AmountCompleted += psiWithShipType1 == null || psiWithShipType1.IsShipDestroyed ? 1 : 0;
                    if (PLServer.DoesPEIOfTypeExist_AndBeenVisited(ESectorVisualIndication.SWARM_CMDR))
                        __instance.AmountCompleted += psiWithShipType2 == null || psiWithShipType2.IsShipDestroyed ? 1 : 0;
                    if (PLServer.DoesPEIOfTypeExist_AndBeenVisited(ESectorVisualIndication.INTREPID_SECTOR_CMDR))
                        __instance.AmountCompleted += psiWithShipType3 == null || psiWithShipType3.IsShipDestroyed ? 1 : 0;
                    if (PLServer.DoesPEIOfTypeExist_AndBeenVisited(ESectorVisualIndication.ALCHEMIST))
                    {
                        foreach (PLPersistantEncounterInstance encounterInstance in PLEncounterManager.Instance.AllPersistantEncounterInstances.Values)
                        {
                            if (encounterInstance != null)
                            {
                                int sectorId = encounterInstance.GetSectorID();
                                if ((UnityEngine.Object)PLGlobal.Instance.Galaxy != (UnityEngine.Object)null && PLGlobal.Instance.Galaxy.AllSectorInfos.ContainsKey(sectorId))
                                {
                                    PLSectorInfo allSectorInfo = PLGlobal.Instance.Galaxy.AllSectorInfos[sectorId];
                                    if (allSectorInfo != null && allSectorInfo.VisualIndication == ESectorVisualIndication.ALCHEMIST)
                                    {
                                        if (allSectorInfo.MySPI.Faction == 5)
                                            __instance.AmountCompleted += PFSectorCommander.bossFlag == 6 ? 1 : 0;
                                        else
                                            __instance.AmountCompleted += psiWithShipType4 == null || psiWithShipType4.IsShipDestroyed ? 1 : 0;
                                    }
                                }
                            }
                        }
                    }
                    if (PLServer.DoesPEIOfTypeExist_AndBeenVisited(ESectorVisualIndication.DEATHSEEKER_COMMANDER))
                        __instance.AmountCompleted += psiWithShipType5 == null || psiWithShipType5.IsShipDestroyed ? 1 : 0;
                }
                __instance.AmountCompleted = Mathf.Min(__instance.AmountCompleted, __instance.AmountNeeded);
            }
            if (__instance.AmountCompleted < __instance.AmountNeeded)
                return false;
            __instance.IsCompleted = true;
            return false;
        }
    }
}
