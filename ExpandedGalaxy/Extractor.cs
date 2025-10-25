using HarmonyLib;
using PulsarModLoader.Content.Components.Extractor;

namespace ExpandedGalaxy
{
    internal class Extractor
    {
        internal class PrototypeExtractorMod : ExtractorMod
        {
            public override string Name => "P.T. Extractor Prototype";

            public override string Description => "An extractor recovered from the wreckage of a Polytech ship. It guarentees that the first extraction attempt per jump is a success, but otherwise has poor stability.";

            public override float Stability => 0.8f;

            public override bool Experimental => true;
        }

        [HarmonyPatch(typeof(PLShipComponent), "GetSalvageSuccessRate")]
        internal class PrototypeExtractorPercentPatch
        {
            private static void Postfix(PLShipComponent __instance, float successRateFromExtractor, ref float __result)
            {
                if (PLEncounterManager.Instance.PlayerShip == null)
                    return;
                if (__instance.ShipStats != null)
                {
                    bool flag = false;
                    foreach (PLShipComponent component in PLEncounterManager.Instance.PlayerShip.MyStats.GetComponentsOfType(ESlotType.E_COMP_SALVAGE_SYSTEM))
                    {
                        if (component.SubType == ExtractorModManager.Instance.GetExtractorIDFromName("P.T. Extractor Prototype") && component.SubTypeData < 1)
                        {
                            flag = true;
                            break;
                        }
                    }
                    if (flag)
                        __result = 1f;
                }
            }
        }

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
}
