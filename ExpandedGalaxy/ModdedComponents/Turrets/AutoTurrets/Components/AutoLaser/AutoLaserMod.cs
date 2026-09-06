using PulsarModLoader.Content.Components.AutoTurret;

namespace ExpandedGalaxy
{
    public class AutoLaserMod : AutoTurretMod
    {
        public override string Name => "Auto Laser Turret";

        public override PLShipComponent PLAutoTurret => (PLShipComponent)new AutoLaser();
    }
}
