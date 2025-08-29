using HarmonyLib;

namespace ExpandedGalaxy
{
    internal class CyberAtk
    {
        [HarmonyPatch(typeof(PLCPU), "GetStatLineRight")]
        internal class CWRText
        {
            private static void Postfix(PLCPU __instance, ref string __result)
            {
                if (__instance.CPUClass == ECPUClass.CYBERWARFARE_MODULE)
                {
                    float value = 0.225f * __instance.LevelMultiplier(0.75f);
                    __result = __instance.ShipStats == null || (bool)__instance.ShipStats.isPreview || __instance.InCargoSlot() ? value.ToString() : (value * __instance.GetPowerPercentInput()).ToString("0.0") + "/" + value.ToString("0.0") + "\n";
                }
            }
        }

        [HarmonyPatch(typeof(PLCPU), "AddStats")]
        internal class CWRAddStats
        {
            private static void Postfix(PLCPU __instance, PLShipStats inStats)
            {
                if (__instance.CPUClass == ECPUClass.CYBERWARFARE_MODULE)
                {
                    inStats.CyberAttackRating -= (float)(5.0 * (double)__instance.LevelMultiplier(0.5f) * 0.0099999997764825821);
                    inStats.CyberAttackRating += (0.225f * __instance.LevelMultiplier(0.75f) * __instance.GetPowerPercentInput());
                }
            }
        }

        [HarmonyPatch(typeof(PLScientistVirusScreen), "Update_Part1")]
        internal class VirusScreenCWRText
        {
            private static void Postfix(PLScientistVirusScreen __instance, ref UILabel ___VirusPanel_Title)
            {
                if (__instance.MyScreenHubBase.OptionalShipInfo.SensorDishTargetShipID != -1)
                {
                    double modifier = 0.0;
                    foreach(PLUIScreen screen in __instance.MyScreenHubBase.AllScreens)
                    {
                        if (screen is PLScientistComputerScreen)
                        {
                            PLWarpDriveProgram program = Traverse.Create(screen).Field("CurrentSelectedProgram").GetValue<PLWarpDriveProgram>();
                            if (program != null)
                                modifier = GetVirusCyberAtkModifier(program.SubType);
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

        [HarmonyPatch(typeof(PLShipInfoBase), "VirusAttemptSuccessful")]
        internal class NewVirusCalc
        {
            private static void Postfix(PLShipInfoBase __instance, PLVirus inVirus, ref bool __result)
            {
                if (__instance.MyStats != null)
                {
                    if (!inVirus.AttemptCounterMap.ContainsKey(__instance.ShipID) || inVirus.AttemptCounterMap[__instance.ShipID] < 3)
                    {
                        __result = false;
                        return;
                    }
                    if ((double)1.0 + inVirus.Sender.MyStats.CyberAttackRating + GetVirusCyberAtkModifier(inVirus.SubType) > (double)__instance.MyStats.CyberDefenseRating)
                    {
                        __result = true;
                        return;
                    }
                }
                __result = false;
            }
        }

        [HarmonyPatch(typeof(PLShipInfo), "AddCrewShipCompVariety")]
        internal class AddCWR
        {
            private static bool Prefix(PLShipInfo __instance, int offset)
            {
                PLRand plRand = new PLRand(offset);
                __instance.MyStats.AddShipComponent(new PLCPU(ECPUClass.CYBERWARFARE_MODULE, __instance.GetChaosBoost(plRand.Next() % 50)));
                return true;
            }
        }

        [HarmonyPatch(typeof(PLShop_Processors), "CreateInitialWares")]
        internal class Shop_ProcessorsCWR
        {
            private static void Postfix(PLShop_Processors __instance, TraderPersistantDataEntry inPDE)
            {
                if (inPDE == null)
                    return;
                inPDE.ServerAddWare((PLWare)new PLCPU(ECPUClass.CYBERWARFARE_MODULE, 0));
                inPDE.ServerAddWare((PLWare)new PLCPU(ECPUClass.CYBERWARFARE_MODULE, 0));
            }
        }

        [HarmonyPatch(typeof(PLShop_Programs), "CreateInitialWares")]
        internal class Shop_ProgramsSyberWar
        {
            private static void Postfix(PLShop_Programs __instance, TraderPersistantDataEntry inPDE)
            {
                if (inPDE == null)
                    return;
                inPDE.ServerAddWare((PLWare)new PLWarpDriveProgram(EWarpDriveProgramType.VIRUS_BOOSTER));
                inPDE.ServerAddWare((PLWare)new PLWarpDriveProgram(EWarpDriveProgramType.VIRUS_BOOSTER));
            }
        }

        [HarmonyPatch(typeof(PLStarGazerInfo), "ShipFinalCalculateStats")]
        internal class StargazerCWR
        {
            private static void Postfix(PLStarGazerInfo __instance, ref PLShipStats inStats)
            {
                inStats.CyberAttackRating += 1.0f;
            }
        }

        [HarmonyPatch(typeof(PLStarGazerInfo), "GetShipAttributes")]
        internal class StargazerCWRText
        {
            private static void Postfix(PLStarGazerInfo __instance, ref string __result)
            {
                __result = PLLocalize.Localize("-50% EM SIG") + "\n" + PLLocalize.Localize("+1.0 Cyber-Attack");
            }
        }

        [HarmonyPatch(typeof(PLRolandInfo), "ShipFinalCalculateStats")]
        internal class RolandCD
        {
            private static void Postfix(PLStarGazerInfo __instance, ref PLShipStats inStats)
            {
                inStats.CyberDefenseRating += 1.0f;
            }
        }

        [HarmonyPatch(typeof(PLRolandInfo), "GetShipAttributes")]
        internal class RolandCDText
        {
            private static void Postfix(PLStarGazerInfo __instance, ref string __result)
            {
                __result = PLLocalize.Localize("+100% EM SIG") + "\n" + PLLocalize.Localize("+25% Shield Recharge Rate") + "\n" + PLLocalize.Localize("+1.0 Cyber-Defense");
            }
        }

        public static double GetVirusCyberAtkModifier(int inSubType)
        {
            double mod = 0.0;
            switch (inSubType)
            {
                case (int)EVirusType.TROJAN_HORSE:
                    mod = 0.5;
                    break;
                case (int)EVirusType.SITTING_DUCK:
                    mod = -0.25;
                    break;
                case (int)EVirusType.GENTLEMENS_WELCOME:
                    mod = -0.5;
                    break;
                case (int)EVirusType.SHUTDOWN_DEFENSES:
                    mod = -0.25;
                    break;
            }
            return mod;
        }
    }
}
