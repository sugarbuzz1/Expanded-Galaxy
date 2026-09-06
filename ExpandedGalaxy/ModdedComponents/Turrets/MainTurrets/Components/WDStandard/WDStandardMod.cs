using PulsarModLoader.Content.Components.MegaTurret;
using static ExpandedGalaxy.Turrets;

namespace ExpandedGalaxy
{
    public class WDStandardMod : MegaTurretMod
    {
        public override string Name => "WD Standard";

        public override PLShipComponent PLMegaTurret => (PLShipComponent)new WDStandard();
    }
}
