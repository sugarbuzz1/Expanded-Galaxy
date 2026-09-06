using PulsarModLoader.Content.Components.Reactor;

namespace ExpandedGalaxy
{
    public class MiningDroneReactor : ReactorMod
    {
        public override string Name => "Extraction Drone Reactor";

        public override string Description => "Reactor custom built for the mining drones.";

        public override int MarketPrice => 50500;

        public override float EnergyOutputMax => 50000f;

        public override float MaxTemp => 4500f;

        public override float EmergencyCooldownTime => 5f;

        public override float EnergySignatureAmount => 24f;

        public override bool CanBeDroppedOnShipDeath => false;
    }
}
