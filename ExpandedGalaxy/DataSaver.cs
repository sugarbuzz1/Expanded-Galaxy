using CodeStage.AntiCheat.ObscuredTypes;
using HarmonyLib;
using Pathfinding.Serialization;
using PulsarModLoader;
using PulsarModLoader.SaveData;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Talents.Framework;

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
                Relic.MiningDroneQuest.dronesActive = true;
                Relic.MiningDroneQuest.GXData = 0;
                SyncCheck.checkedPlayers.Clear();
                Relic.RelicCaravan.CaravanCurrentSector = -1;
                Relic.RelicCaravan.CaravanTargetSector = -1;
                Relic.RelicCaravan.CaravanUpdateTime = 60000;
                Relic.RelicCaravan.ClearCaravanPath();
            }
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
    internal class SendData
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
            if (PLNetworkManager.Instance.LocalPlayer != null) {
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
            ModMessage.SendRPC("sugarbuzz1.ExpandedGalaxy", "ExpandedGalaxy.RecieveData", PhotonTargets.Others, new object[7]
            {
                    (object) PFSectorCommander.bossFlag,
                    (object) Relic.MiningDroneQuest.dronesActive,
                    (object) Relic.MiningDroneQuest.GXData,
                    (object) Jetpack.AdvancedJetPack,
                    (object) Ammunition.DynamicAmmunition,
                    (object) Relic.RelicCaravan.CaravanCurrentSector,
                    (object) Relic.RelicCaravan.CaravanTargetSector
            });
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
}
