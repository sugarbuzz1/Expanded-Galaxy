using PulsarModLoader.Content.Components.Turret;
using static ExpandedGalaxy.Turrets;

namespace ExpandedGalaxy
{
    public class MiningLaserMod : TurretMod
    {
        public override string Name => "Mining Laser";

        public override PLShipComponent PLTurret => (PLShipComponent)new MiningLaser();
    }
}
