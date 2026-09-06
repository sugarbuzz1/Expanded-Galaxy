using HarmonyLib;
using PulsarModLoader.Content.Components.Extractor;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLShipComponent), "OnWarp")]
    internal class ExtractorWarpPatch
    {
        private static void Postfix(PLShipComponent __instance)
        {
            PLExtractor extractor = __instance as PLExtractor;
            if (extractor == null)
                return;
            if (extractor.SubType == ExtractorModManager.Instance.GetExtractorIDFromName("P.T. Extractor Prototype"))
                extractor.SubTypeData = 0;
        }
    }
}
