using CodeStage.AntiCheat.ObscuredTypes;
using ExitGames.Client.Photon;
using PulsarModLoader;
using PulsarModLoader.Content.Components.MegaTurret;
using PulsarModLoader.SaveData;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Talents.Framework;
using UnityEngine;

namespace ExpandedGalaxy
{
    internal class DataSaver : PMLSaveData
    {
        public override uint VersionID => 6;

        public override string Identifier() => "sugarbuzz1.ExpandedGalaxy";

        public override void LoadData(byte[] Data, uint VersionID)
        {
            using (MemoryStream input = new MemoryStream(Data))
            {
                using (BinaryReader binaryReader = new BinaryReader((Stream)input))
                {
                    PFSectorCommander.bossFlag = binaryReader.ReadInt32();
                    Relic.RelicData = binaryReader.ReadUInt64();
                    MiningDroneQuest.dronesActive = binaryReader.ReadBoolean();
                    MiningDroneQuest.GXData = binaryReader.ReadInt32();
                    Ammunition.DynamicAmmunition = binaryReader.ReadBoolean();
                    RelicCaravan.CaravanCurrentSector = binaryReader.ReadInt32();
                    RelicCaravan.CaravanTargetSector = binaryReader.ReadInt32();
                    RelicCaravan.CaravanSpecialsData = binaryReader.ReadInt32();
                    RelicCaravan.ClearCaravanPath();
                    UpdateCaravan.invalidTargets.Clear();
                    PlanetObjectSetupManager.Instance.OnNewGame();
                    Debug.Log("[ExGal] Cleared Flags...");

                    TraderPersistantDataEntry dataEntry = new TraderPersistantDataEntry();
                    dataEntry.ServerWareIDCounter = binaryReader.ReadInt32();
                    int wareCount = binaryReader.ReadInt32();
                    for (int i = 0; i < wareCount; i++)
                    {
                        PLWare fromHash = PLWare.CreateFromHash(binaryReader.ReadInt32(), (int)binaryReader.ReadUInt32());
                        if (fromHash != null)
                            dataEntry.ServerAddWare(fromHash);
                    }
                    RelicCaravan.CaravanTraderData = dataEntry;
                    UpdateCaravan.persistantCaravanInfo = null;
                    Debug.Log("[ExGal] Loaded Caravan...");

                    if (VersionID < 2)
                        return;
                    List<object> data = new List<object>();

                    int ammoBoxCount = binaryReader.ReadInt32();
                    data.Add(ammoBoxCount);
                    for (int i = 0; i < ammoBoxCount; i++)
                        data.Add(binaryReader.ReadSingle());

                    Debug.Log("[ExGal] Loaded Ammo...");
                    if (VersionID < 3)
                        return;

                    PlanetCompPickups.PickupQueue.Clear();
                    int pickupQueueCount = binaryReader.ReadInt32();
                    for (int i = 0; i < pickupQueueCount; i++)
                    {
                        int sectorID = binaryReader.ReadInt32();
                        int hashCount = binaryReader.ReadInt32();
                        List<int> hashes = new List<int>();
                        for (int j = 0; j < hashCount; j++)
                            hashes.Add(binaryReader.ReadInt32());
                        PlanetCompPickups.PickupQueue.Add(sectorID, hashes);
                    }
                    Debug.Log("[ExGal] Loaded PickupQueue...");

                    if (VersionID < 4)
                        return;

                    Missions.slowMissionPickups = binaryReader.ReadBoolean();
                    Missions.pickupMissionDelay = 0;

                    PersistantScrapManager.Instance.ClearData();
                    int totalScrapDatas = binaryReader.ReadInt32();
                    for (int i = 0; i < totalScrapDatas; i++)
                    {
                        ExtraScrapData scrapData = new ExtraScrapData();
                        scrapData.ID = binaryReader.ReadInt32();
                        scrapData.sectorID = binaryReader.ReadInt32();
                        scrapData.compHash = binaryReader.ReadInt32();
                        scrapData.forceComp = binaryReader.ReadBoolean();
                        Vector3 vector3 = new Vector3(binaryReader.ReadSingle(), binaryReader.ReadSingle(), binaryReader.ReadSingle());
                        scrapData.location = vector3;
                        Quaternion quaternion = new Quaternion(binaryReader.ReadSingle(), binaryReader.ReadSingle(), binaryReader.ReadSingle(), binaryReader.ReadSingle());
                        scrapData.rotation = quaternion;
                        PersistantScrapManager.Instance.LoadScrap(scrapData);
                    }
                    Debug.Log("[ExGal] Loaded PersistantScrap...");
                    CrewLogManager.Instance.OnNewGame();
                    int totalLogs = binaryReader.ReadInt32();
                    Debug.Log("Loading " + totalLogs.ToString() + " Logs...");
                    for (int i = 0; i < totalLogs; i++)
                    {
                        CrewLogData logData = new CrewLogData();
                        logData.timeStamp = binaryReader.ReadSingle();
                        logData.Text = binaryReader.ReadString();
                        logData.optionalSectorID = binaryReader.ReadInt32();
                        Color color = new Color(binaryReader.ReadSingle(), binaryReader.ReadSingle(), binaryReader.ReadSingle(), binaryReader.ReadSingle());
                        logData.optionalColor = color;
                        if (!(VersionID < 6))
                            logData.specialData = binaryReader.ReadInt32();
                        else
                            logData.specialData = -1;
                            CrewLogManager.Instance.AddLog(logData);
                    }
                    Debug.Log("Loaded All Logs");

                    TraderPersistantDataEntry dataEntry1 = new TraderPersistantDataEntry();
                    dataEntry1.ServerWareIDCounter = binaryReader.ReadInt32();
                    int wareCount1 = binaryReader.ReadInt32();
                    for (int i = 0; i < wareCount1; i++)
                    {
                        PLWare fromHash = PLWare.CreateFromHash(binaryReader.ReadInt32(), (int)binaryReader.ReadUInt32());
                        if (fromHash != null)
                            dataEntry1.ServerAddWare(fromHash);
                    }
                    Debug.Log("[ExGal] Created Carrier Shop with [" + dataEntry1.ServerWareIDCounter.ToString() + "] items");
                    data.Add(dataEntry1);

                    if (VersionID < 5)
                        return;
                    ReflectedRift.riftData = binaryReader.ReadByte();
                    data.Add(binaryReader.ReadInt64());         

                    DelayedLoadData(data);
                }
            }
        }

        public override byte[] SaveData()
        {
            using (MemoryStream output = new MemoryStream())
            {
                using (BinaryWriter binaryWriter = new BinaryWriter((Stream)output))
                {
                    binaryWriter.Write(PFSectorCommander.bossFlag);
                    binaryWriter.Write(Relic.RelicData);
                    binaryWriter.Write(MiningDroneQuest.dronesActive);
                    binaryWriter.Write(MiningDroneQuest.GXData);
                    binaryWriter.Write(Ammunition.DynamicAmmunition);
                    binaryWriter.Write(RelicCaravan.CaravanCurrentSector);
                    binaryWriter.Write(RelicCaravan.CaravanTargetSector);
                    binaryWriter.Write(RelicCaravan.CaravanSpecialsData);

                    binaryWriter.Write(RelicCaravan.CaravanTraderData.ServerWareIDCounter);
                    int num = 0;
                    foreach (int key in RelicCaravan.CaravanTraderData.Wares.Keys)
                    {
                        if (RelicCaravan.CaravanTraderData.Wares[key] != null)
                            ++num;
                    }
                    binaryWriter.Write(num);
                    foreach (int key1 in RelicCaravan.CaravanTraderData.Wares.Keys)
                    {
                        PLWare ware = RelicCaravan.CaravanTraderData.Wares[key1];
                        if (ware != null)
                        {
                            binaryWriter.Write((int)ware.WareType);
                            binaryWriter.Write(ware.getHash());
                        }
                    }
                    binaryWriter.Write(PLEncounterManager.Instance.PlayerShip.MyAmmoRefills.Length);
                    foreach (PLAmmoRefill refill in PLEncounterManager.Instance.PlayerShip.MyAmmoRefills)
                        binaryWriter.Write(refill.SupplyAmount);

                    binaryWriter.Write(PlanetCompPickups.PickupQueue.Count);
                    foreach(int sectorID in PlanetCompPickups.PickupQueue.Keys)
                    {
                        binaryWriter.Write(sectorID);
                        binaryWriter.Write(PlanetCompPickups.PickupQueue[sectorID].Count);
                        foreach (int hash in PlanetCompPickups.PickupQueue[sectorID])
                            binaryWriter.Write(hash);
                    }

                    binaryWriter.Write(Missions.slowMissionPickups);

                    binaryWriter.Write(PersistantScrapManager.Instance.GetTotalScrapDatas());
                    foreach (ExtraScrapData scrapData in PersistantScrapManager.Instance.GetAllScrapDatas())
                    {
                        binaryWriter.Write(scrapData.ID);
                        binaryWriter.Write(scrapData.sectorID);
                        binaryWriter.Write(scrapData.compHash);
                        binaryWriter.Write(scrapData.forceComp);
                        binaryWriter.Write(scrapData.location.x);
                        binaryWriter.Write(scrapData.location.y);
                        binaryWriter.Write(scrapData.location.z);
                        binaryWriter.Write(scrapData.rotation.x);
                        binaryWriter.Write(scrapData.rotation.y);
                        binaryWriter.Write(scrapData.rotation.z);
                        binaryWriter.Write(scrapData.rotation.w);
                    }
                    binaryWriter.Write(CrewLogManager.Instance.GetLogs().Count);
                    foreach (CrewLogData logData in CrewLogManager.Instance.GetLogs())
                    {
                        binaryWriter.Write(logData.timeStamp);
                        binaryWriter.Write(logData.Text);
                        binaryWriter.Write(logData.optionalSectorID);
                        binaryWriter.Write(logData.optionalColor.r);
                        binaryWriter.Write(logData.optionalColor.g);
                        binaryWriter.Write(logData.optionalColor.b);
                        binaryWriter.Write(logData.optionalColor.a);
                        binaryWriter.Write(logData.specialData);
                    }
                    Debug.Log("Saved " + CrewLogManager.Instance.GetLogs().Count.ToString() + " Logs to File");

                    binaryWriter.Write(BadBiscuits.carrierData.ServerWareIDCounter);
                    int num1 = 0;
                    foreach (int key in BadBiscuits.carrierData.Wares.Keys)
                    {
                        if (BadBiscuits.carrierData.Wares[key] != null)
                            ++num1;
                    }
                    binaryWriter.Write(num1);
                    foreach (int key1 in BadBiscuits.carrierData.Wares.Keys)
                    {
                        PLWare ware = BadBiscuits.carrierData.Wares[key1];
                        if (ware != null)
                        {
                            binaryWriter.Write((int)ware.WareType);
                            binaryWriter.Write(ware.getHash());
                        }
                    }

                    binaryWriter.Write(ReflectedRift.riftData);

                    binaryWriter.Write(PLServer.Instance.ActiveChaosEvents);
                }
                return output.ToArray();
            }
        }

        private async void DelayedLoadData(List<object> data)
        {
            Debug.Log("[ExGal] DelayedLoadData");
            await Task.Delay(5000);
            while (true)
            {
                if (PLServer.Instance == null)
                {
                    await Task.Delay(1000);
                    continue;
                }
                if (PLEncounterManager.Instance.PlayerShip == null)
                {
                    await Task.Delay(1000);
                    continue;
                }
                else
                    break;
            }
            await Task.Delay(1000);
            RelicCaravan.CaravanUpdateTime = PLServer.Instance.GetEstimatedServerMs() + 10000;
            int index = 0;
            int ammoBoxCount = (int)data[index];
            index++;
            float[] ammoBoxSupply = new float[ammoBoxCount];
            for (int i = 0; i < ammoBoxCount; i++)
            {
                ammoBoxSupply[i] = (float)data[index + i];
            }
            for (int i = 0; i < PLEncounterManager.Instance.PlayerShip.MyAmmoRefills.Length; i++)
                PLEncounterManager.Instance.PlayerShip.MyAmmoRefills[i].SupplyAmount = ammoBoxSupply[i];
            index += ammoBoxCount;

            TraderPersistantDataEntry dataEntry = (TraderPersistantDataEntry)data[index];
            foreach (PLPersistantShipInfo allPSI in PLServer.Instance.AllPSIs)
            {
                if (allPSI != null && allPSI.SelectedActorID == "ExGal_FBCarrier")
                {
                    allPSI.OptionalTPDE = dataEntry;
                    BadBiscuits.carrierData = dataEntry;
                    Debug.Log("[ExGal] Loaded Carrier Shop");
                    break;
                }
            }
            ++index;
            PLServer.Instance.ActiveChaosEvents = (long)data[index];
            for (int i = 0; i < 10; i++)
            {
                if (PLServer.Instance.IsChaosEventActive((EChaosEvent)i))
                {
                    ChaosEvents.CreateLRDAForChaosEvent((EChaosEvent)i);
                    break;
                }
            }

            List<object> sendArgumentList = new List<object>();
            int num = 0;
            foreach (CrewLogData sendLogData in CrewLogManager.Instance.GetLogs())
            {
                ++num;
                sendArgumentList.Add(sendLogData.Text);
                sendArgumentList.Add(sendLogData.timeStamp);
                sendArgumentList.Add(sendLogData.optionalSectorID);
                sendArgumentList.Add(sendLogData.optionalColor.r);
                sendArgumentList.Add(sendLogData.optionalColor.g);
                sendArgumentList.Add(sendLogData.optionalColor.b);
                sendArgumentList.Add(sendLogData.optionalColor.a);
                sendArgumentList.Add(sendLogData.specialData);
            }
            sendArgumentList.Insert(0, num);
            ModMessage.SendRPC("sugarbuzz1.ExpandedGalaxy", "ExpandedGalaxy.ServerSendLog", PhotonTargets.Others, sendArgumentList.ToArray());
            Debug.Log("[ExGal] Save data loading finished!");
        }

        private static bool IsTalentUnlocked(int talentID)
        {
            int num = talentID / 64;
            int num2 = talentID % 64;
            if (num != 0)
            {
                long num3 = 1L << num2;
                Dictionary<int, ObscuredLong> dictionary = TalentModManager.Instance.extraTalentLockedStatus;
                int key = num;
                return ((long)dictionary[key] & num3) > 0;
            }
            return false;
        }
    }
}
