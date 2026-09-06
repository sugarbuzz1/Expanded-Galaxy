using HarmonyLib;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLRolandInfo), "GetShipAttributes")]
    internal class RolandCDText
    {
        private static void Postfix(PLStarGazerInfo __instance, ref string __result)
        {
            __result = PLLocalize.Localize("+100% EM SIG") + "\n" + PLLocalize.Localize("+25% Shield Recharge Rate") + "\n" + PLLocalize.Localize("+1.0 Cyber-Defense");
        }
    }
}
