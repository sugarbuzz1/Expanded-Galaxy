using PulsarModLoader.Content.Components.Turret;
using static ExpandedGalaxy.Turrets;

namespace ExpandedGalaxy
{
    public class ParticleLanceMod : TurretMod
    {
        public override string Name => "Particle Lance";

        public override PLShipComponent PLTurret => (PLShipComponent)new ParticleLance();
    }
}
