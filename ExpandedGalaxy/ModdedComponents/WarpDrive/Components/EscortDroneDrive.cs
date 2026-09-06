using PulsarModLoader.Content.Components.WarpDrive;

namespace ExpandedGalaxy
{
    public class EscortDroneDrive : WarpDriveMod
    {
        public override string Name => "Asset Protection Drive";

        public override string Description => "A custom warp drive for the Escort Drones. It's large range allows for a direct route from their hub system to quickly reinforce a drone in distress.";

        public override bool CanBeDroppedOnShipDeath => false;

        public override float ChargeSpeed => 1f;

        public override float WarpRange => 0.36f;

        public override float EnergySignature => 18f;

        public override int NumberOfChargesPerFuel => 3;

        public override float MaxPowerUsage_Watts => 3000f;
    }
}
