using PulsarModLoader.Content.Components.MissionShipComponent;

namespace ExpandedGalaxy
{
    internal class DataCache : MissionShipComponentMod
    {
        public override string Name => "Data Cache";

        public override string Description => "A blast-proof box with a hard drive inside.";

        public override int MarketPrice => 1;

        public override int CargoVisualID => 27;

        public override float Price_LevelMultiplierExponent => 1f;
    }
}
