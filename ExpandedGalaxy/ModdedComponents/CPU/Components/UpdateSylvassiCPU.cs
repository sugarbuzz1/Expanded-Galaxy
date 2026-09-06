using PulsarModLoader;
using PulsarModLoader.Content.Components.CPU;

namespace ExpandedGalaxy
{
    internal class UpdateSylvassiCPU : ModMessage
    {
        public override void HandleRPC(object[] arguments, PhotonMessageInfo sender)
        {
            PLShipInfoBase ship = PLEncounterManager.Instance.GetShipFromID((int)arguments[0]);
            if (ship == null)
                return;
            PLCPU plcpu = null;
            foreach (PLShipComponent component in ship.MyStats.GetComponentsOfType(ESlotType.E_COMP_CPU))
            {
                plcpu = component as PLCPU;
                if (plcpu != null && plcpu.SubType == CPUModManager.Instance.GetCPUIDFromName("Sylvassi Shield Charger"))
                    break;
            }
            if (plcpu == null)
                return;
            if (plcpu.SubTypeData == 0)
            {
                plcpu.SubTypeData = 1;
            }
            else
            {
                plcpu.SubTypeData = 0;
            }
        }
    }
}
