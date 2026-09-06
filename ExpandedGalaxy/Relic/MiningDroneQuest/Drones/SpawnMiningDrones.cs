using HarmonyLib;
using System.Collections.Generic;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLGalaxy), "CreateStartingShips")]
    internal class SpawnMiningDrones
    {
        private static bool Prefix(PLGalaxy __instance, ref Dictionary<int, PLSectorInfo> ___m_AllSectorInfos, out Dictionary<int, PLSectorInfo> __state)
        {
            __state = new Dictionary<int, PLSectorInfo>();
            if (PLServer.Instance == null)
                return true;
            PLRand rand = new PLRand(__instance.Seed);
            foreach (int sectorNum in ___m_AllSectorInfos.Keys)
            {
                if (___m_AllSectorInfos[sectorNum].VisualIndication == ESectorVisualIndication.NONE && ___m_AllSectorInfos[sectorNum].MySPI != null && ___m_AllSectorInfos[sectorNum].MySPI.Faction != 4 && rand.Next(100) > 80)
                {
                    __state.Add(sectorNum, ___m_AllSectorInfos[sectorNum]);
                }
                if (___m_AllSectorInfos[sectorNum].VisualIndication == ESectorVisualIndication.LAVA2)
                {
                    ___m_AllSectorInfos[sectorNum].Discovered = false;
                    ___m_AllSectorInfos[sectorNum].Name = sectorNum.ToString();
                    ___m_AllSectorInfos[sectorNum].MySPI.Faction = 6;
                    for (int i = 0; i < 3; i++)
                    {
                        PLPersistantShipInfo pLPersistantShipInfo = new PLPersistantShipInfo(EShipType.E_WDDRONE2, 6, ___m_AllSectorInfos[sectorNum])
                        {
                            ShipName = "Guardian Drone",
                            HullPercent = 1f,
                            ShldPercent = 1f,
                            IsFlagged = true,
                            ForcedHostile = true,
                        };
                        pLPersistantShipInfo.CompOverrides.AddRange((IEnumerable<ComponentOverrideData>)MiningDroneQuest.GetComponentsFromDroneType(2, rand));
                        PLServer.Instance.AllPSIs.Add(pLPersistantShipInfo);
                    }
                }
            }
            foreach (int sectorNum in __state.Keys)
            {
                if (___m_AllSectorInfos.ContainsKey(sectorNum))
                    ___m_AllSectorInfos.Remove(sectorNum);
            }
            return true;
        }

        private static void Postfix(PLGalaxy __instance, ref Dictionary<int, PLSectorInfo> ___m_AllSectorInfos, Dictionary<int, PLSectorInfo> __state)
        {
            if (PLServer.Instance == null)
                return;
            PLRand rand = new PLRand(__instance.Seed);
            foreach (int sectorNum in __state.Keys)
            {
                PLSectorInfo sectorInfo = __state[sectorNum];
                for (int i = 0; i < 1 + rand.Next(3); i++)
                {
                    PLPersistantShipInfo pLPersistantShipInfo = new PLPersistantShipInfo(EShipType.E_WDDRONE1, 6, sectorInfo)
                    {
                        ShipName = "Mining Drone",
                        HullPercent = rand.Next(0.85f, 1f),
                        ShldPercent = 1f,
                        IsFlagged = true
                    };
                    pLPersistantShipInfo.CompOverrides.AddRange((IEnumerable<ComponentOverrideData>)MiningDroneQuest.GetComponentsFromDroneType(0, rand));
                    PLServer.Instance.AllPSIs.Add(pLPersistantShipInfo);
                }
                for (int i = 1; i < rand.Next(4); i++)
                {
                    PLPersistantShipInfo pLPersistantShipInfo = new PLPersistantShipInfo(EShipType.E_WDDRONE2, 6, sectorInfo)
                    {
                        ShipName = "Escort Drone",
                        HullPercent = rand.Next(0.8f, 1f),
                        ShldPercent = 1f,
                        IsFlagged = true,

                    };
                    pLPersistantShipInfo.CompOverrides.AddRange((IEnumerable<ComponentOverrideData>)MiningDroneQuest.GetComponentsFromDroneType(1, rand));
                    PLServer.Instance.AllPSIs.Add(pLPersistantShipInfo);
                }
                ___m_AllSectorInfos.Add(sectorNum, sectorInfo);
            }
        }
    }
}
