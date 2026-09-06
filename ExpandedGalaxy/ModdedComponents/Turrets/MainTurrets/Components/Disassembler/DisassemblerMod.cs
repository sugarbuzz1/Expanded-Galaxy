using PulsarModLoader.Content.Components.MegaTurret;
using static ExpandedGalaxy.Turrets;

namespace ExpandedGalaxy
{
    public class DisassemblerMod : MegaTurretMod
    {
        public override string Name => "The Disassembler";

        public override PLShipComponent PLMegaTurret => (PLShipComponent)new Disassembler();
    }
}
