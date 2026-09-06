using PulsarModLoader.Content.Components.MegaTurret;
using static ExpandedGalaxy.Turrets;

namespace ExpandedGalaxy
{
    public class GuardianMainTurretMod : MegaTurretMod
    {
        public override string Name => "GuardianMainTurret";

        public override PLShipComponent PLMegaTurret => (PLShipComponent)new GuardianMainTurret();
    }
}
