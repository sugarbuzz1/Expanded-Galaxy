using PulsarModLoader.Content.Components.MissionShipComponent;

namespace ExpandedGalaxy
{
    internal class IrradiatedCargoTreasFleet : MissionShipComponentMod
    {
        public override string Name => "Irradiated Cargo";

        public override string Description => "A crate that is emitting dangerous levels of radiation.";

        public override int MarketPrice => 4000;

        public override int CargoVisualID => 50;

        public override bool Contraband => true;

        public override bool CanBeDroppedOnShipDeath => false;
    }

}

