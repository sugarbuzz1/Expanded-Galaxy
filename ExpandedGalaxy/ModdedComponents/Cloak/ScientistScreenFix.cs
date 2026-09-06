using HarmonyLib;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLScientistComputerScreen), "Update")]
    internal class ScientistScreenFix
    {
        private static void Postfix(
          PLScientistComputerScreen __instance,
          UILabel ___MainScreen_EMLabel,
          PLCachedFormatString<float> ___cMainScreen_EMLabel)
        {
            PLGlobal.SafeLabelSetText(___MainScreen_EMLabel, ___cMainScreen_EMLabel.ToString(__instance.MyScreenHubBase.OptionalShipInfo.MyStats.EMSignature));
        }
    }
}
