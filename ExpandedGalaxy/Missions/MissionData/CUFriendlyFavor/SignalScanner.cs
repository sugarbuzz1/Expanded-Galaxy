using PulsarModLoader.Content.Components.MissionShipComponent;

namespace ExpandedGalaxy
{
    internal class SignalScanner : MissionShipComponentMod
    {
        public override string Name => "Frequency Scanner";

        public override string Description => "A device used to intercept and decrypt signals used for long range communications. It is illegal to have this equipment in your posession.";

        public override int MarketPrice => 9000;

        public override int CargoVisualID => 28;

        public override float Price_LevelMultiplierExponent => 1.2f;

        public override bool Contraband => true;
    }

}

