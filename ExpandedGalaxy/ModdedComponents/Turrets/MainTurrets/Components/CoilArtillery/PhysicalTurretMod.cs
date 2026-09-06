using PulsarModLoader.Content.Components.MegaTurret;

namespace ExpandedGalaxy
{
    public class PhysicalTurretMod : MegaTurretMod
    {
        public override string Name => "Coil Artillery";

        public override PLShipComponent PLMegaTurret => new PhysicalTurret();
    }
}
