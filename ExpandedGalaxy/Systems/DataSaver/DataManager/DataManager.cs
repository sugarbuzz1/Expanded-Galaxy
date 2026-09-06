using PulsarModLoader.Content.Components.CaptainsChair;
using PulsarModLoader.Content.Components.CPU;
using PulsarModLoader.Content.Components.Extractor;
using PulsarModLoader.Content.Components.HullPlating;
using PulsarModLoader.Content.Components.PolytechModule;

namespace ExpandedGalaxy
{
    internal class DataManager
    {
        internal static bool ShouldUpdateSubTypeData(PLShipComponent shipComponent)
        {
            switch (shipComponent.ActualSlotType)
            {
                case ESlotType.E_COMP_CPU:
                    if (shipComponent.SubType == CPUModManager.Instance.GetCPUIDFromName("Super Shield") || shipComponent.SubType == CPUModManager.Instance.GetCPUIDFromName("Sylvassi Shield Charger"))
                        return true;
                    break;
                case ESlotType.E_COMP_SENS:
                    return true;
                case ESlotType.E_COMP_CAPTAINS_CHAIR:
                    if (shipComponent.SubType == CaptainsChairModManager.Instance.GetCaptainsChairIDFromName("Seat of the Surveyor"))
                        return true;
                    break;
                case ESlotType.E_COMP_HULLPLATING:
                    if (shipComponent.SubType == HullPlatingModManager.Instance.GetHullPlatingIDFromName("Bone Plating"))
                        return true;
                    break;
                case ESlotType.E_COMP_POLYTECH_MODULE:
                    if (shipComponent.SubType == PolytechModuleModManager.Instance.GetPolytechModuleIDFromName("P.T. Module: Recompiler 1") || shipComponent.SubType == PolytechModuleModManager.Instance.GetPolytechModuleIDFromName("P.T. Module: Recompiler 4") || shipComponent.SubType == PolytechModuleModManager.Instance.GetPolytechModuleIDFromName("P.T. Module: Recompiler 5"))
                        return true;
                    break;
                case ESlotType.E_COMP_CLOAKING_SYS:
                    if (shipComponent.SubType == (int)ECloakingSystemType.E_NORMAL || shipComponent.SubType == (int)ECloakingSystemType.E_SYVASSI)
                        return true;
                    break;
                case ESlotType.E_COMP_VIRUS:
                    if (shipComponent.SubType == (int)EVirusType.RAND_SMALL || shipComponent.SubType == (int)EVirusType.RAND_LARGE)
                        return true;
                    break;
                case ESlotType.E_COMP_SALVAGE_SYSTEM:
                    if (shipComponent.SubType == ExtractorModManager.Instance.GetExtractorIDFromName("P.T. Extractor Prototype"))
                        return true;
                    break;
                case ESlotType.E_COMP_REAC_COOLING:
                    return true;
            }
            return false;
        }
    }
}
