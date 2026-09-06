using System.Collections.Generic;

namespace ExpandedGalaxy
{
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
}
