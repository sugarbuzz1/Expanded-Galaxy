using PulsarModLoader.Content.Components.AutoTurret;
using static ExpandedGalaxy.Turrets;

namespace ExpandedGalaxy
{
    public class AncientAutoLaserMod : AutoTurretMod
    {
        public override string Name => "Ancient Auto Laser Turret";

        public override PLShipComponent PLAutoTurret => (PLShipComponent)new AncientAutoLaser();
    }
}
