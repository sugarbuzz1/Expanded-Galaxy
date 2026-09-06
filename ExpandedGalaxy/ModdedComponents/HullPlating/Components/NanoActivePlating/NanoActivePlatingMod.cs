using PulsarModLoader.Content.Components.HullPlating;

namespace ExpandedGalaxy
{
    public class NanoActivePlatingMod : HullPlatingMod
    {
        public override string Name => "NanoActivePlating";

        public override PLShipComponent PLHullPlating => (PLShipComponent)new NanoActivePlating(EHullPlatingType.E_HULLPLATING_CCGE, 0);
    }
}
