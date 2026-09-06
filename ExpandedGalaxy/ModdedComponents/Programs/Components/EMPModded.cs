using PulsarModLoader;
using PulsarModLoader.Content.Components.WarpDriveProgram;

namespace ExpandedGalaxy
{
    public class EMPModded : WarpDriveProgramMod
    {
        public override string Name => "EMP";

        public override string Description => "Shuts down EVERY ship not equipped to handle EMP attacks within 3.5km";

        public override int MarketPrice => 2500;

        public override string ShortName => "EM";

        public override float ActiveTime => 0.1f;

        public override int MaxLevelCharges => 5;

        public override void Execute(PLWarpDriveProgram InWarpDriveProgram)
        {
            if (PLServer.GetCurrentSector() == null || !PhotonNetwork.isMasterClient)
                return;
            ModMessage.SendRPC("sugarbuzz1.ExpandedGalaxy", "ExpandedGalaxy.EMPPulse", PhotonTargets.All, new object[2]
                    {
                            (object) InWarpDriveProgram.ShipStats.Ship.ShipID,
                            (object) 3500f
                    });
            if (InWarpDriveProgram.ShipStats.Ship.ShipTypeID == EShipType.E_FLUFFY_DELIVERY || InWarpDriveProgram.ShipStats.Ship.ShipTypeID == EShipType.E_FLUFFY_TWO || InWarpDriveProgram.ShipStats.Ship.ShipTypeID == EShipType.E_ABYSS_PLAYERSHIP)
                return;
            InWarpDriveProgram.ShipStats.Ship.photonView.RPC("Overcharged", PhotonTargets.All);
        }
    }
}

