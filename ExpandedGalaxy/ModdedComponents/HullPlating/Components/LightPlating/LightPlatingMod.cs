using PulsarModLoader.Content.Components.HullPlating;

namespace ExpandedGalaxy
{
    public class LightPlatingMod : HullPlatingMod
    {
        public override string Name => "LightHullPlating";

        public override PLShipComponent PLHullPlating => (PLShipComponent)new LightPlating(EHullPlatingType.E_HULLPLATING_CCGE, 0);
    }
}
