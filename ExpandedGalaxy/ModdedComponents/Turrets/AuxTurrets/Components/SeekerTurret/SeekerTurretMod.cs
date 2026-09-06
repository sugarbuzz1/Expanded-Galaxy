using PulsarModLoader.Content.Components.Turret;
using static ExpandedGalaxy.Turrets;

namespace ExpandedGalaxy
{
    public class SeekerTurretMod : TurretMod
    {
        public override string Name => "Seeker Turret";

        public override PLShipComponent PLTurret => new SeekerTurret();
    }
}
