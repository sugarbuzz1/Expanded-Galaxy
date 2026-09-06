using PulsarModLoader.Content.Components.MissionShipComponent;

namespace ExpandedGalaxy
{
    public class RelicRewardMod : MissionShipComponentMod
    {
        public override string Name => "Reward";

        public override string Description => "A gift from the Caravan for delivering a prized Junk Cube. Process it to find out what's inside!";

        public override int CargoVisualID => 1;
    }
}
