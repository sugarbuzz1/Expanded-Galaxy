using PulsarModLoader.Content.Components.InertiaThruster;

namespace ExpandedGalaxy
{
    public class StabilizerMod : InertiaThrusterMod
    {
        public override string Name => "Integrated Stabilizer Thruster";

        public override string Description => "Inertia thruster with an automatic rotation control system that eliminates turret and missile knockback";

        public override float MaxOutput => 0.37f;

        public override float MaxPowerUsage_Watts => 4000f;

        public override int MarketPrice => 8000;
    }
}
