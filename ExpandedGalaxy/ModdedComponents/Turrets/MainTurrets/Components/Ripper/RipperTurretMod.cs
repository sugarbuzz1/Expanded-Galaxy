using PulsarModLoader.Content.Components.MegaTurret;
using static ExpandedGalaxy.Turrets;

namespace ExpandedGalaxy
{
    public class RipperTurretMod : MegaTurretMod
    {
        public override string Name => "Ripper Turret";

        public override PLShipComponent PLMegaTurret => new RipperTurrret();
    }
}
