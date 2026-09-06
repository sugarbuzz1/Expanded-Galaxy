using CodeStage.AntiCheat.ObscuredTypes;
using HarmonyLib;
using PulsarModLoader;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLSpecialEncounterNetObject), "OnNewEncounter")]
    internal class DelayedSetupPersistantScrapCall
    {
        private static IEnumerator DelayedSetupPersistantScrap()
        {
            yield return new WaitForSeconds(4);
            if (PLEncounterManager.Instance.GetCPEI() != null && PLServer.GetCurrentSector() != null)
            {
                PLPersistantEncounterInstance pLPersistantEncounterInstance = PLEncounterManager.Instance.GetCPEI();
                Dictionary<int, ExtraScrapData> scrapDatas = PersistantScrapManager.Instance.GetScrapDataForSector(PLServer.GetCurrentSector().ID);
                int max = 0;
                foreach (ObscuredInt objectID in pLPersistantEncounterInstance.MyPersistantData.SpecialNetObjectPersistantData.Keys)
                {
                    if (!(bool)pLPersistantEncounterInstance.MyPersistantData.SpecialNetObjectPersistantData[objectID].PersistantState)
                    {
                        if (PLSpecialEncounterNetObject.GetObjectAtID((int)objectID) == null)
                        {
                            if (scrapDatas != null && scrapDatas.ContainsKey((int)objectID))
                            {
                                ModMessage.SendRPC("sugarbuzz1.ExpandedGalaxy", "ExpandedGalaxy.SendScrapRPC", PhotonTargets.All, new object[5]
                                {
                                    (int)objectID,
                                    scrapDatas[(int)objectID].location,
                                    scrapDatas[(int)objectID].rotation,
                                    scrapDatas[(int)objectID].forceComp,
                                    scrapDatas[(int)objectID].compHash
                                });
                                yield return new WaitForSeconds(0.2f);
                            }
                        }
                    }
                    if ((int)objectID > max)
                        max = (int)objectID;
                }
                PLSpecialEncounterNetObject.m_IDCounter = max + 1;
            }
        }

        private static void Postfix()
        {
            PLEncounterManager.Instance.StartCoroutine(DelayedSetupPersistantScrap());
        }
    }
}
