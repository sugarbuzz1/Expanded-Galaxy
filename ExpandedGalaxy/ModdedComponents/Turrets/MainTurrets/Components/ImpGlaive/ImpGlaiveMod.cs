using PulsarModLoader.Content.Components.MegaTurret;
using static ExpandedGalaxy.Turrets;

namespace ExpandedGalaxy
{
    public class ImpGlaiveMod : MegaTurretMod
    {
        public override string Name => "Imperial Glaive";

        public override PLShipComponent PLMegaTurret => (PLShipComponent)new ImpGlaive();
    }
}
