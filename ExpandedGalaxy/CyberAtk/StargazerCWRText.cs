using HarmonyLib;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLStarGazerInfo), "GetShipAttributes")]
    internal class StargazerCWRText
    {
        private static void Postfix(PLStarGazerInfo __instance, ref string __result)
        {
            __result = PLLocalize.Localize("-50% EM SIG") + "\n" + PLLocalize.Localize("+1.0 Cyber-Attack");
        }
    }
}
