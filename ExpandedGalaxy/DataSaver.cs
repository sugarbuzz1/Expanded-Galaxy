using CodeStage.AntiCheat.ObscuredTypes;
using HarmonyLib;
using PulsarModLoader;
using PulsarModLoader.Content.Components.CPU;
using PulsarModLoader.Content.Components.HullPlating;
using PulsarModLoader.Content.Components.MegaTurret;
using PulsarModLoader.Content.Components.PolytechModule;
using PulsarModLoader.SaveData;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Talents.Framework;
using UnityEngine;

namespace ExpandedGalaxy
{
    internal class DataSaver : PMLSaveData
    {
        public override uint VersionID => 2;

        public override string Identifier() => "sugarbuzz1.ExpandedGalaxy";

        public override void LoadData(byte[] Data, uint VersionID)
        {
            using (MemoryStream input = new MemoryStream(Data))
            {
                using (BinaryReader binaryReader = new BinaryReader((Stream)input))
                {
                    PFSectorCommander.bossFlag = binaryReader.ReadInt32();
                    Relic.RelicData = binaryReader.ReadUInt64();
                    Relic.MiningDroneQuest.dronesActive = binaryReader.ReadBoolean();
                    Relic.MiningDroneQuest.GXData = binaryReader.ReadInt32();
                    Ammunition.DynamicAmmunition = binaryReader.ReadBoolean();
                    Relic.RelicCaravan.CaravanCurrentSector = binaryReader.ReadInt32();
                    Relic.RelicCaravan.CaravanTargetSector = binaryReader.ReadInt32();
                    Relic.RelicCaravan.CaravanSpecialsData = binaryReader.ReadInt32();
                    Relic.RelicCaravan.ClearCaravanPath();

                    TraderPersistantDataEntry dataEntry = new TraderPersistantDataEntry();
                    dataEntry.ServerWareIDCounter = binaryReader.ReadInt32();
                    int wareCount = binaryReader.ReadInt32();
                    for (int i = 0; i < wareCount; i++)
                    {
                        PLWare fromHash = PLWare.CreateFromHash(binaryReader.ReadInt32(), (int)binaryReader.ReadUInt32());
                        if (fromHash != null)
                            dataEntry.ServerAddWare(fromHash);
                    }
                    Relic.RelicCaravan.CaravanTraderData = dataEntry;

                    if (VersionID < 2)
                        return;
                    List<object> data = new List<object>();

                    int ammoBoxCount = binaryReader.ReadInt32();
                    data.Add(ammoBoxCount);
                    for (int i = 0; i < ammoBoxCount; i++)
                        data.Add(binaryReader.ReadSingle());

                    DelayedLoadData(data);

                    if (VersionID < 3)
                        return;

                    Relic.PickupQueue.Clear();
                    int pickupQueueCount = binaryReader.ReadInt32();
                    for (int i = 0; i < pickupQueueCount; i++)
                    {
                        int sectorID = binaryReader.ReadInt32();
                        int hashCount = binaryReader.ReadInt32();
                        List<int> hashes = new List<int>();
                        for (int j = 0; j < hashCount; j++)
                            hashes.Add(binaryReader.ReadInt32());
                        Relic.PickupQueue.Add(sectorID, hashes);
                    }
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
                    binaryWriter.Write(Relic.MiningDroneQuest.dronesActive);
                    binaryWriter.Write(Relic.MiningDroneQuest.GXData);
                    binaryWriter.Write(Ammunition.DynamicAmmunition);
                    binaryWriter.Write(Relic.RelicCaravan.CaravanCurrentSector);
                    binaryWriter.Write(Relic.RelicCaravan.CaravanTargetSector);
                    binaryWriter.Write(Relic.RelicCaravan.CaravanSpecialsData);

                    binaryWriter.Write(Relic.RelicCaravan.CaravanTraderData.ServerWareIDCounter);
                    int num = 0;
                    foreach (int key in Relic.RelicCaravan.CaravanTraderData.Wares.Keys)
                    {
                        if (Relic.RelicCaravan.CaravanTraderData.Wares[key] != null)
                            ++num;
                    }
                    binaryWriter.Write(num);
                    foreach (int key1 in Relic.RelicCaravan.CaravanTraderData.Wares.Keys)
                    {
                        PLWare ware = Relic.RelicCaravan.CaravanTraderData.Wares[key1];
                        if (ware != null)
                        {
                            binaryWriter.Write((int)ware.WareType);
                            binaryWriter.Write(ware.getHash());
                        }
                    }
                    binaryWriter.Write(PLEncounterManager.Instance.PlayerShip.MyAmmoRefills.Length);
                    foreach (PLAmmoRefill refill in PLEncounterManager.Instance.PlayerShip.MyAmmoRefills)
                        binaryWriter.Write(refill.SupplyAmount);

                    binaryWriter.Write(Relic.PickupQueue.Count);
                    foreach(int sectorID in Relic.PickupQueue.Keys)
                    {
                        binaryWriter.Write(sectorID);
                        binaryWriter.Write(Relic.PickupQueue[sectorID].Count);
                        foreach (int hash in Relic.PickupQueue[sectorID])
                            binaryWriter.Write(hash);
                    }
                }
                return output.ToArray();
            }
        }

        private async void DelayedLoadData(List<object> data)
        {
            await Task.Delay(5000);
            while (true)
            {
                if (PLServer.Instance == null)
                    return;
                if (PLEncounterManager.Instance.PlayerShip == null)
                    await Task.Delay(1000);
                else
                    break;
            }
            await Task.Delay(1000);
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
        }
    }

    internal class DataManager
    {
        [HarmonyPatch(typeof(PLUICreateGameMenu), "ClickEngage")]
        internal class ResetFlags
        {
            private static void Postfix(PLUICreateGameMenu __instance)
            {
                PFSectorCommander.bossFlag = 0;
                Relic.RelicData = 0U;
                Relic.PickupQueue.Clear();
                Relic.MiningDroneQuest.dronesActive = true;
                Relic.MiningDroneQuest.GXData = 0;
                SyncCheck.checkedPlayers.Clear();
                Relic.RelicCaravan.CaravanCurrentSector = -1;
                Relic.RelicCaravan.CaravanTargetSector = -1;
                Relic.RelicCaravan.CaravanUpdateTime = 60000;
                Relic.RelicCaravan.ClearCaravanPath();
            }
        }


        internal class RecieveData : ModMessage
        {
            public override void HandleRPC(object[] arguments, PhotonMessageInfo sender)
            {
                if (sender.sender.IsMasterClient)
                {
                    PFSectorCommander.bossFlag = (int)arguments[0];
                    Relic.MiningDroneQuest.dronesActive = (bool)arguments[1];
                    Relic.MiningDroneQuest.GXData = (int)arguments[2];
                    Jetpack.AdvancedJetPack = (bool)arguments[3];
                    Ammunition.DynamicAmmunition = (bool)arguments[4];
                    Relic.RelicCaravan.CaravanCurrentSector = (int)arguments[5];
                    Relic.RelicCaravan.CaravanTargetSector = (int)arguments[6];
                }

            }
        }

        [HarmonyPatch(typeof(PLShipInfoBase), "Update")]
        internal class ShipUpdatePatch
        {
            private static void Postfix(PLShipInfoBase __instance)
            {
                if (__instance == null || !__instance.GetIsPlayerShip())
                    return;
                if (Jetpack.AdvancedJetPack)
                {
                    TalentModManager.Instance.HideTalent((int)ETalents.INC_JETPACK);
                    TalentModManager.Instance.UnHideTalent((int)TalentModManager.Instance.GetTalentIDFromName("Jetpack Fuel Reserve"));
                }
                else
                {
                    TalentModManager.Instance.UnHideTalent((int)ETalents.INC_JETPACK);
                    TalentModManager.Instance.HideTalent((int)TalentModManager.Instance.GetTalentIDFromName("Jetpack Fuel Reserve"));
                }
                if (Ammunition.DynamicAmmunition)
                {
                    TalentModManager.Instance.HideTalent((int)ETalents.WPN_AMMO_BOOST);
                    TalentModManager.Instance.UnHideTalent((int)TalentModManager.Instance.GetTalentIDFromName("Reloader"));
                }
                else
                {
                    TalentModManager.Instance.UnHideTalent((int)ETalents.WPN_AMMO_BOOST);
                    TalentModManager.Instance.HideTalent((int)TalentModManager.Instance.GetTalentIDFromName("Reloader"));
                }
                TalentModManager.Instance.HideTalent((int)ETalents.SCI_RESEARCH_SPECIALTY);
                if (PLNetworkManager.Instance.LocalPlayer != null)
                {
                    foreach (PLPlayer player in PLServer.Instance.AllPlayers)
                    {
                        if (player != null && player.TeamID == PLNetworkManager.Instance.LocalPlayer.TeamID)
                        {
                            if (Jetpack.AdvancedJetPack && player.Talents[(int)ETalents.INC_JETPACK] > 0)
                            {
                                int num = (int)player.Talents[(int)ETalents.INC_JETPACK];
                                player.Talents[TalentModManager.Instance.GetTalentIDFromName("Jetpack Fuel Reserve")] = (ObscuredInt)num;
                                player.Talents[(int)ETalents.INC_JETPACK] = (ObscuredInt)0;
                            }
                            else if (!Jetpack.AdvancedJetPack && player.Talents[TalentModManager.Instance.GetTalentIDFromName("Jetpack Fuel Reserve")] > 0)
                            {
                                int num = (int)player.Talents[TalentModManager.Instance.GetTalentIDFromName("Jetpack Fuel Reserve")];
                                player.Talents[(int)ETalents.INC_JETPACK] = (ObscuredInt)num;
                                player.Talents[(int)TalentModManager.Instance.GetTalentIDFromName("Jetpack Fuel Reserve")] = (ObscuredInt)0;
                            }
                        }
                    }
                }
                if (!PhotonNetwork.isMasterClient)
                    return;
                /*ModMessage.SendRPC("sugarbuzz1.ExpandedGalaxy", "ExpandedGalaxy.RecieveData", PhotonTargets.Others, new object[7]
                {
                        (object) PFSectorCommander.bossFlag,
                        (object) Relic.MiningDroneQuest.dronesActive,
                        (object) Relic.MiningDroneQuest.GXData,
                        (object) Jetpack.AdvancedJetPack,
                        (object) Ammunition.DynamicAmmunition,
                        (object) Relic.RelicCaravan.CaravanCurrentSector,
                        (object) Relic.RelicCaravan.CaravanTargetSector
                });*/
                if (PLServer.Instance == null)
                    return;
                foreach (PLPlayer player in PLServer.Instance.AllPlayers)
                {
                    if (player != null && !player.IsBot && player.GetPhotonPlayer() != null && !SyncCheck.checkedPlayers.Contains(player.GetPhotonPlayer()))
                    {
                        ModMessage.SendRPC("sugarbuzz1.ExpandedGalaxy", "ExpandedGalaxy.RequestSyncCheck", player.GetPhotonPlayer(), new object[0]);
                        SyncCheck.checkedPlayers.Add(player.GetPhotonPlayer());
                    }
                }
            }
        }

        [HarmonyPatch(typeof(PLServer), "OnPhotonSerializeView")]
        internal class SendData
        {
            private static void Postfix(PLServer __instance, ref PhotonStream stream, PhotonMessageInfo info)
            {
                if (stream.isWriting)
                {
                    stream.SendNext(PFSectorCommander.bossFlag);
                    stream.SendNext(Relic.MiningDroneQuest.dronesActive);
                    stream.SendNext(Relic.MiningDroneQuest.GXData);
                    stream.SendNext(Jetpack.AdvancedJetPack);
                    stream.SendNext(Ammunition.DynamicAmmunition);
                    stream.SendNext(Relic.RelicCaravan.CaravanCurrentSector);
                    stream.SendNext(Relic.RelicCaravan.CaravanTargetSector);
                }
                else
                {
                    PFSectorCommander.bossFlag = (int)stream.ReceiveNext();
                    Relic.MiningDroneQuest.dronesActive = (bool)stream.ReceiveNext();
                    Relic.MiningDroneQuest.GXData = (int)stream.ReceiveNext();
                    Relic.RelicCaravan.CaravanCurrentSector = (int)stream.ReceiveNext();
                    Relic.RelicCaravan.CaravanTargetSector = (int)stream.ReceiveNext();
                    Jetpack.AdvancedJetPack = (bool)stream.ReceiveNext();
                    Ammunition.DynamicAmmunition = (bool)stream.ReceiveNext();
                }
            }
        }

        private static bool ShouldUpdateSubTypeData(PLShipComponent shipComponent)
        {
            switch (shipComponent.ActualSlotType)
            {
                case ESlotType.E_COMP_CPU:
                    if (shipComponent.SubType == CPUModManager.Instance.GetCPUIDFromName("Super Shield"))
                        return true;
                    break;
                case ESlotType.E_COMP_HULLPLATING:
                    if (shipComponent.SubType == HullPlatingModManager.Instance.GetHullPlatingIDFromName("Bone Plating"))
                        return true;
                    break;
                case ESlotType.E_COMP_POLYTECH_MODULE:
                    if (shipComponent.SubType == PolytechModuleModManager.Instance.GetPolytechModuleIDFromName("P.T. Module: Recompiler 1") || shipComponent.SubType == PolytechModuleModManager.Instance.GetPolytechModuleIDFromName("P.T. Module: Recompiler 4") || shipComponent.SubType == PolytechModuleModManager.Instance.GetPolytechModuleIDFromName("P.T. Module: Recompiler 5"))
                        return true;
                    break;
                case ESlotType.E_COMP_CLOAKING_SYS:
                    if (shipComponent.SubType == (int)ECloakingSystemType.E_NORMAL || shipComponent.SubType == (int)ECloakingSystemType.E_SYVASSI)
                        return true;
                    break;
                case ESlotType.E_COMP_VIRUS:
                    if (shipComponent.SubType == (int)EVirusType.RAND_SMALL || shipComponent.SubType == (int)EVirusType.RAND_LARGE)
                        return true;
                    break;
                case ESlotType.E_COMP_MAINTURRET:
                    if (shipComponent.SubType == MegaTurretModManager.Instance.GetMegaTurretIDFromName("WD Standard"))
                        return true;
                    break;
            }
            return false;
        }

        [HarmonyPatch(typeof(PLShipInfoBase), "OnPhotonSerializeView")]
        internal class SendSubTypeData
        {
            private static void Postfix(PLShipInfoBase __instance, ref PhotonStream stream, PhotonMessageInfo info)
            {
                if (stream.isWriting)
                {
                    Dictionary<int, short> data = new Dictionary<int, short>();
                    if (__instance.MyStats != null)
                    {
                        foreach (PLShipComponent component in __instance.MyStats.AllComponents)
                            if (ShouldUpdateSubTypeData(component) && component.NetID != -1)
                            {
                                data.Add(component.NetID, component.SubTypeData);
                            }
                    }
                    stream.SendNext(data.Count);
                    if (data.Count > 0)
                    {
                        stream.SendNext(data.Keys.ToArray());
                        stream.SendNext(data.Values.ToArray());
                    }
                }
                else
                {
                    int count = (int)stream.ReceiveNext();
                    if (count > 0)
                    {
                        int[] netIDs = (int[])stream.ReceiveNext();
                        short[] subTypeDatas = (short[])stream.ReceiveNext();

                        if (__instance.MyStats != null)
                        {
                            for (int i = 0; i < netIDs.Length; i++)
                            {
                                if (__instance.MyStats.GetComponentFromNetID(netIDs[i]) != null && i < subTypeDatas.Length)
                                    __instance.MyStats.GetComponentFromNetID(netIDs[i]).SubTypeData = subTypeDatas[i];
                            }
                        }
                    }
                }
            }
        }
    }
}
