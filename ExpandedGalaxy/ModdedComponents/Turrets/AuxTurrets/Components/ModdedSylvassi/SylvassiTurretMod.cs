using PulsarModLoader.Content.Components.Turret;
using static ExpandedGalaxy.Turrets;

namespace ExpandedGalaxy
{
    public class SylvassiTurretMod : TurretMod
    {
        public override string Name => "Sylvassi Turret";

        public override PLShipComponent PLTurret => (PLShipComponent)new SylvassiTurret();
    }
}
