using PulsarModLoader.Content.Components.WarpDrive;

namespace ExpandedGalaxy
{
    public class MiningDroneDrive : WarpDriveMod
    {
        public override string Name => "Extration Drone Warp Drive";

        public override string Description => "A warp drive that boasts a fast charge rate for quick getaways when a Mining Drone is provoked.";

        public override bool CanBeDroppedOnShipDeath => false;

        public override float ChargeSpeed => 6f;

        public override float WarpRange => 0.06f;

        public override float EnergySignature => 12f;

        public override int NumberOfChargesPerFuel => 3;

        public override float MaxPowerUsage_Watts => 3000f;
    }
}
