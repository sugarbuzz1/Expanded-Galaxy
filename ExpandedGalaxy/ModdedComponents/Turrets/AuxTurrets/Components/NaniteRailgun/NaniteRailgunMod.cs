using PulsarModLoader.Content.Components.Turret;
using static ExpandedGalaxy.Turrets;

namespace ExpandedGalaxy
{
    public class NaniteRailgunMod : TurretMod
    {
        public override string Name => "Nanite Railgun";

        public override PLShipComponent PLTurret => (PLShipComponent)new NaniteRailgun();
    }
}
