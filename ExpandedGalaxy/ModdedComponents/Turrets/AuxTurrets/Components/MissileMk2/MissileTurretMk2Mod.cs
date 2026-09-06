using PulsarModLoader.Content.Components.Turret;
using static ExpandedGalaxy.Turrets;

namespace ExpandedGalaxy
{
    public class MissileTurretMk2Mod : TurretMod
    {
        public override string Name => "Missile Turret Mk. II";

        public override PLShipComponent PLTurret => new MissileTurretMk2();
    }
}
