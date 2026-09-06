using PulsarModLoader;

namespace ExpandedGalaxy
{
    internal class RecieveData : ModMessage
    {
        public override void HandleRPC(object[] arguments, PhotonMessageInfo sender)
        {
            if (sender.sender.IsMasterClient)
            {
                PFSectorCommander.bossFlag = (int)arguments[0];
                MiningDroneQuest.dronesActive = (bool)arguments[1];
                MiningDroneQuest.GXData = (int)arguments[2];
                Jetpack.AdvancedJetPack = (bool)arguments[3];
                Ammunition.DynamicAmmunition = (bool)arguments[4];
                RelicCaravan.CaravanCurrentSector = (int)arguments[5];
                RelicCaravan.CaravanTargetSector = (int)arguments[6];
            }

        }
    }
}
