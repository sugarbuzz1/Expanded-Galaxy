using PulsarModLoader.Content.Components.MegaTurret;
using static ExpandedGalaxy.Turrets;

namespace ExpandedGalaxy
{
    public class WDLongMod : MegaTurretMod
    {
        public override string Name => "WD Long Range";

        public override PLShipComponent PLMegaTurret => new WDLong();
    }
}
