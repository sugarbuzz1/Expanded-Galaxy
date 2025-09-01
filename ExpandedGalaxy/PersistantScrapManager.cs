using CodeStage.AntiCheat.ObscuredTypes;
using HarmonyLib;
using PulsarModLoader;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ExpandedGalaxy
{
    public struct ExtraScrapData
    {
        public int ID;
        public int sectorID;
        public int compHash;
        public bool forceComp;
        public Vector3 location;
        public Quaternion rotation;
    }
    public class PersistantScrapManager
    {
        private static PersistantScrapManager m_instance;
        private Dictionary<int, Dictionary<int, ExtraScrapData>> PersistantScrapData;

        public static PersistantScrapManager Instance
        {
            get
            {
                if (PersistantScrapManager.m_instance == null)
                    PersistantScrapManager.m_instance = new PersistantScrapManager();
                return PersistantScrapManager.m_instance;
            }
        }

        public PersistantScrapManager()
        {
            PersistantScrapData = new Dictionary<int, Dictionary<int, ExtraScrapData>> ();
        }

        public void ClearData()
        {
            foreach (Dictionary<int, ExtraScrapData> dict in PersistantScrapData.Values)
                dict.Clear();
            PersistantScrapData.Clear();
        }

        public void AddScrap(PLSpaceScrap scrap, int sectorID)
        {
            if (!PersistantScrapData.ContainsKey(sectorID))
                PersistantScrapData.Add(sectorID, new Dictionary<int, ExtraScrapData>());
            else if (PersistantScrapData[sectorID].ContainsKey(scrap.EncounterNetID))
                return;
            ExtraScrapData scrapData = new ExtraScrapData();
            scrapData.ID = scrap.EncounterNetID;
            scrapData.sectorID = sectorID;
            scrapData.compHash = scrap.SpecificComponent_CompHash;
            scrapData.forceComp = scrap.IsSpecificComponentScrap;
            scrapData.location = scrap.transform.position;
            scrapData.rotation = scrap.transform.rotation;
            PersistantScrapData[sectorID].Add(scrap.EncounterNetID, scrapData);
        }

        public void RemoveScrap(int sectorID, int inNetID)
        {
            if (!PersistantScrapData.ContainsKey(sectorID) || !PersistantScrapData[sectorID].ContainsKey(inNetID))
                return;
            PersistantScrapData[sectorID].Remove(inNetID);
            if (PersistantScrapData[sectorID].Count == 0)
                PersistantScrapData.Remove(sectorID);
        }

        public Dictionary<int, ExtraScrapData> GetScrapDataForSector(int sectorID)
        {
            if (!PersistantScrapData.ContainsKey(sectorID))
                return null;
            return PersistantScrapData[sectorID];
        }

        public int GetTotalScrapDatas()
        {
            int total = 0;
            foreach (Dictionary<int, ExtraScrapData> dict in PersistantScrapData.Values)
                total += dict.Count;
            return total;
        }

        public List<ExtraScrapData> GetAllScrapDatas()
        {
            List<ExtraScrapData> outScrapDatas = new List<ExtraScrapData>();
            foreach (Dictionary<int, ExtraScrapData> dict in PersistantScrapData.Values)
                foreach (ExtraScrapData scrapData in dict.Values)
                    outScrapDatas.Add(scrapData);
            return outScrapDatas;
        }

        public void LoadScrap(ExtraScrapData scrapData)
        {
            if (!PersistantScrapData.ContainsKey(scrapData.sectorID))
                PersistantScrapData[scrapData.sectorID] = new Dictionary<int, ExtraScrapData>();
            else if (PersistantScrapData[scrapData.sectorID].ContainsKey(scrapData.ID))
                return;
            PersistantScrapData[scrapData.sectorID][scrapData.ID] = scrapData;
        }
    }

    internal class PersistantScrap
    {
        [HarmonyPatch(typeof(PLSpecialEncounterNetObject), "InitNewObject")]
        internal class InitExtraScrapData
        {
            private static void Postfix(PLSpecialEncounterNetObject inObject, PLPersistantEncounterInstance inPersistantEncounterInstance)
            {
                if (!PhotonNetwork.isMasterClient)
                    return;
                if (inObject is PLSpaceScrap)
                {
                    PLSpaceScrap scrap = (PLSpaceScrap)inObject;
                    PersistantScrapManager.Instance.AddScrap(scrap, PLServer.GetCurrentSector().ID);
                }
            }
        }

        [HarmonyPatch(typeof(PLSpaceScrap), "OnCollect")]
        internal class CollectPersistantScrap
        {
            private static bool Prefix(PLSpaceScrap __instance)
            {
                if (!PhotonNetwork.isMasterClient)
                    return true;
                if (__instance.Collected)
                    return false;
                PLShipInfo shipInfo = PLEncounterManager.Instance.PlayerShip;
                if (shipInfo == null || PLServer.Instance == null)
                    return true;
                PLSlot slot = shipInfo.MyStats.GetSlot(ESlotType.E_COMP_CARGO);
                if (slot == null || slot.Count >= slot.MaxItems)
                    return false;
                PersistantScrapManager.Instance.RemoveScrap(PLServer.GetCurrentSector().ID, __instance.EncounterNetID);
                return true;
            }
        }

        [HarmonyPatch(typeof(PLSpecialEncounterNetObject), "OnNewEncounter")]
        internal class DelayedSetupPersistantScrapCall
        {
            private static void Postfix()
            {
                PLEncounterManager.Instance.StartCoroutine(DelayedSetupPersistantScrap());
            }
        }

        private static IEnumerator DelayedSetupPersistantScrap()
        {
            yield return new WaitForSeconds(4);
            if (PLEncounterManager.Instance.GetCPEI() != null && PLServer.GetCurrentSector() != null)
            {
                PLPersistantEncounterInstance pLPersistantEncounterInstance = PLEncounterManager.Instance.GetCPEI();
                Dictionary<int, ExtraScrapData> scrapDatas = PersistantScrapManager.Instance.GetScrapDataForSector(PLServer.GetCurrentSector().ID);
                if (scrapDatas != null && scrapDatas.Count > 0)
                {
                    Traverse traverse = Traverse.Create(typeof(PLSpecialEncounterNetObject));
                    int max = 0;
                    foreach (ObscuredInt objectID in pLPersistantEncounterInstance.MyPersistantData.SpecialNetObjectPersistantData.Keys)
                    {
                        if (!(bool)pLPersistantEncounterInstance.MyPersistantData.SpecialNetObjectPersistantData[objectID].PersistantState)
                        {
                            if (PLSpecialEncounterNetObject.GetObjectAtID((int)objectID) == null)
                            {
                                if (scrapDatas.ContainsKey((int)objectID))
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
                    traverse.Field<int>("m_IDCounter").Value = max;
                }
            }
        }

        public class SendScrapRPC : ModMessage
        {
            public override void HandleRPC(object[] arguments, PhotonMessageInfo sender)
            {
                Traverse traverse = Traverse.Create(typeof(PLSpecialEncounterNetObject));
                traverse.Field<int>("m_IDCounter").Value = (int)arguments[0];
                if (arguments.Length > 1)
                {
                    if (PLEncounterManager.Instance.GetCPEI() == null)
                        return;
                    PLSpaceScrap component = UnityEngine.Object.Instantiate<GameObject>(PLGlobal.Instance.ScrapPrefab, (Vector3)arguments[1], (Quaternion)arguments[2]).GetComponent<PLSpaceScrap>();
                    if (!((UnityEngine.Object)component != (UnityEngine.Object)null))
                        return;
                    component.IsSpecificComponentScrap = (bool)arguments[3];
                    component.SpecificComponent_CompHash = (int)arguments[4];
                    component.CanGiveComponent = (int)arguments[4] != -1;
                    PLSpecialEncounterNetObject.InitNewObject(component, PLEncounterManager.Instance.GetCPEI());
                }
            }
        }
    }
}
