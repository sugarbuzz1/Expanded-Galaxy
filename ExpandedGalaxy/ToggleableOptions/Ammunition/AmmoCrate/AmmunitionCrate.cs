using PulsarModLoader.Content.Components.MissionShipComponent;

namespace ExpandedGalaxy
{
    internal class AmmunitionCrate : MissionShipComponentMod
    {
        public override string Name => "Ammunition Cache";

        public override string Description => "A cache filled with various types of weapon ammo. Used to restock ship ammo refills.";

        public override int MarketPrice => 2500;

        public override int CargoVisualID => 49;

        public override float Price_LevelMultiplierExponent => 1.2f;
    }
}
