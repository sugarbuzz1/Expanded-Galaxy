using PulsarModLoader.Content.Components.Reactor;

namespace ExpandedGalaxy
{
    public class ModifiedRolandReactor : ReactorMod
    {
        public override string Name => "Modified Roland Reactor";

        public override string Description => "Custom reactor built for a CU Roland-class starship. This one has been modified for larger output and higher temperatures.";

        public override int MarketPrice => 100;

        public override float EnergyOutputMax => 50000f;

        public override float MaxTemp => 4500f;

        public override float EmergencyCooldownTime => 10f;

        public override float EnergySignatureAmount => 16f;

        public override bool CanBeDroppedOnShipDeath => false;
    }
}
