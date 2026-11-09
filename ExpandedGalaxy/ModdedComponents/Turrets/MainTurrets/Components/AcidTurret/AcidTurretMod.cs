using PulsarModLoader.Content.Components.MegaTurret;

namespace ExpandedGalaxy
{
    internal class AcidTurretMod : MegaTurretMod
    {
        public override string Name => "Caustic Collider";

        public override PLShipComponent PLMegaTurret => new AcidTurret();
    }
}
