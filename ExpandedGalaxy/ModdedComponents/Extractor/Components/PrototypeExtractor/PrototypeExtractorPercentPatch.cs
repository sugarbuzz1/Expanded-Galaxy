using HarmonyLib;
using PulsarModLoader.Content.Components.Extractor;

namespace ExpandedGalaxy
{
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
}
