using HarmonyLib;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLScientistVirusScreen), "Update_Part1")]
    internal class VirusScreenCWRText
    {
        private static void Postfix(PLScientistVirusScreen __instance, ref UILabel ___VirusPanel_Title)
        {
            if (__instance.MyScreenHubBase.OptionalShipInfo.SensorDishTargetShipID != -1)
            {
                double modifier = 0.0;
                foreach (PLUIScreen screen in __instance.MyScreenHubBase.AllScreens)
                {
                    if (screen is PLScientistComputerScreen)
                    {
                        PLWarpDriveProgram program = ((PLScientistComputerScreen)screen).CurrentSelectedProgram;
                        if (program != null)
                            modifier = CyberAtk.GetVirusCyberAtkModifier(program.SubType);
                        break;
                    }
                }
                ___VirusPanel_Title.text = PLLocalize.Localize("CYBER-ATK") + ": " + ((double)(1.0 + __instance.MyScreenHubBase.OptionalShipInfo.MyStats.CyberAttackRating + modifier) - PLEncounterManager.Instance.GetShipFromID(__instance.MyScreenHubBase.OptionalShipInfo.SensorDishTargetShipID).MyStats.CyberDefenseRating).ToString("0.0");
            }
            else
            {
                ___VirusPanel_Title.text = PLLocalize.Localize("CYBER-ATK") + ": " + ((double)1.0 + __instance.MyScreenHubBase.OptionalShipInfo.MyStats.CyberAttackRating).ToString("0.0");
            }
        }
    }
}
