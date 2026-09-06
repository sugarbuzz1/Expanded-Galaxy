using PulsarModLoader.Content.Components.HullPlating;

namespace ExpandedGalaxy
{
    public class HeavyDutyPlatingMod : HullPlatingMod
    {
        public override string Name => "HeavyDutyPlating";

        public override PLShipComponent PLHullPlating => (PLShipComponent)new HeavyDutyPlating(EHullPlatingType.E_HULLPLATING_CCGE, 0);
    }
}
