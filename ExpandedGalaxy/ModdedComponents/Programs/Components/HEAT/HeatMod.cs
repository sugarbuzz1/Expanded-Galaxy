using PulsarModLoader.Content.Components.WarpDriveProgram;

namespace ExpandedGalaxy
{
    public class HeatMod : WarpDriveProgramMod
    {
        public override string Name => "H.E.A.T.";

        public override string Description => "Highly Erosive Ammunition for Turrets. Causes all physical damage turrets to deal acid damage for 60 seconds. Affected turrets deal 20% less damage.";

        public override int MarketPrice => 6000;

        public override string ShortName => "HE";

        public override float ActiveTime => 60f;

        public override int MaxLevelCharges => 3;

        public override bool Experimental => true;
    }
}

