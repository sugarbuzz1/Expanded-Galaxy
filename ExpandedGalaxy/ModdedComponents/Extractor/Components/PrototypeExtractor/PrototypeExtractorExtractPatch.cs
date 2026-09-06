using HarmonyLib;
using PulsarModLoader.Content.Components.Extractor;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLShipInfo), "AttemptExtraction")]
    internal class PrototypeExtractorExtractPatch
    {
        private static void Postfix(PLShipInfo __instance, ref PhotonMessageInfo pmi)
        {
            if (__instance.MyStats != null)
            {
                foreach (PLShipComponent component in __instance.MyStats.GetComponentsOfType(ESlotType.E_COMP_SALVAGE_SYSTEM))
                {
                    if (component.SubType == ExtractorModManager.Instance.GetExtractorIDFromName("P.T. Extractor Prototype") && component.SubTypeData < 1)
                    {
                        ++component.SubTypeData;
                        break;
                    }
                }
            }
        }
    }
}
