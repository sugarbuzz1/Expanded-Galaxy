using PulsarModLoader.Content.Components.HullPlating;

namespace ExpandedGalaxy
{
    public class BonePlatingMod : HullPlatingMod
    {
        public override string Name => "Bone Plating";

        public override PLShipComponent PLHullPlating => new BonePlating(EHullPlatingType.E_HULLPLATING_CCGE, 0);
    }
}
