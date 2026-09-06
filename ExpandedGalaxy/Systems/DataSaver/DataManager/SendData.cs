using HarmonyLib;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLServer), "OnPhotonSerializeView")]
    internal class SendData
    {
        private static void Postfix(PLServer __instance, ref PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.isWriting)
            {
                stream.SendNext(PFSectorCommander.bossFlag);
                stream.SendNext(MiningDroneQuest.dronesActive);
                stream.SendNext(MiningDroneQuest.GXData);
                stream.SendNext(RelicCaravan.CaravanCurrentSector);
                stream.SendNext(RelicCaravan.CaravanTargetSector);
                stream.SendNext(ReflectedRift.riftData);
                stream.SendNext(Jetpack.AdvancedJetPack);
                stream.SendNext(Ammunition.DynamicAmmunition);
                stream.SendNext(Exosuit.BetterExosuit);
                stream.SendNext(Missions.slowMissionPickups);

            }
            else
            {
                PFSectorCommander.bossFlag = (int)stream.ReceiveNext();
                MiningDroneQuest.dronesActive = (bool)stream.ReceiveNext();
                MiningDroneQuest.GXData = (int)stream.ReceiveNext();
                RelicCaravan.CaravanCurrentSector = (int)stream.ReceiveNext();
                RelicCaravan.CaravanTargetSector = (int)stream.ReceiveNext();
                ReflectedRift.riftData = (byte)stream.ReceiveNext();
                Jetpack.AdvancedJetPack = (bool)stream.ReceiveNext();
                Ammunition.DynamicAmmunition = (bool)stream.ReceiveNext();
                Exosuit.BetterExosuit = (bool)stream.ReceiveNext();
                Missions.slowMissionPickups = (bool)stream.ReceiveNext();

            }
        }
    }
}
