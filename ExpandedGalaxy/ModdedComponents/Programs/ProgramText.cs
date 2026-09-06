using CodeStage.AntiCheat.ObscuredTypes;
using HarmonyLib;
using System;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLWarpDriveProgram), MethodType.Constructor, new Type[3] { typeof(EWarpDriveProgramType), typeof(int), typeof(short) })]
    internal class ProgramText
    {
        private static void Postfix(PLWarpDriveProgram __instance, EWarpDriveProgramType inType, int inLevel, short inSubData, ref float ___Detector_BoostAmount, ref float ___Detector_ActiveTime, ref float ___ShieldBooster_ActiveTime, ref float ___Overcharge_ActiveTime, ref float ___ExShields_BoostAmount, ref ObscuredInt ___m_MarketPrice)
        {
            if (inType == EWarpDriveProgramType.SHIELD_BOOSTER)
                __instance.Desc = "Increases the charge rate of shields by 25% for 15 seconds.";
            else if (inType == EWarpDriveProgramType.SUPER_SHIELD_BOOSTER)
                __instance.Desc = "Increases the charge rate of shields by 50% for 15 seconds.";
            else if (inType == EWarpDriveProgramType.OVERCHARGE)
            {
                __instance.Desc = "Increases reactor output by 50% and prevents meltdown for 15 seconds.";
                ___Overcharge_ActiveTime = 15f;
            }
            else if (inType == EWarpDriveProgramType.EXTENDED_SHIELDS)
            {
                __instance.Desc = "Increases max shields by +100 for 60 seconds.";
                ___ExShields_BoostAmount = 100f;
            }
            else if (inType == EWarpDriveProgramType.VIRUS_BOOSTER)
                __instance.Desc = "+0.5 CyberAttack for 30 seconds";
            else if (inType == EWarpDriveProgramType.BARRAGE)
            {
                __instance.Name = "Flash Coolant";
                __instance.Desc = "Cools all ship turrets by 50% upon activation. Overheated turrets will still need time to cool down.";
                __instance.ShortName = "FC";
            }
            else if (inType == EWarpDriveProgramType.TROJAN_HORSE_VIRUS_PROGRAM)
                __instance.Desc = "Broadcasts [Backdoor] virus to nearby ships on activation. Confers a moderate Cyber-Atk boost when infecting.\n\nBackdoor: Disables firewalls for 3 minutes";
            else if (inType == EWarpDriveProgramType.SITTING_DUCK_VIRUS_PROGRAM)
                __instance.Desc = "Broadcasts [Sitting Duck] virus to nearby ships on activation. Incurs a slight Cyber-Atk penalty when infecting.\n\nSitting Duck: Disables ship thrusters for 60 seconds";
            else if (inType == EWarpDriveProgramType.GENTLEMENS_WELCOME)
            {
                __instance.Desc = "Broadcasts [Gentlemen's Welcome] virus to nearby ships on activation. Incurs a moderate Cyber-Atk penalty when infecting.\n\nGentleman's Welcome: Disables quantum shields for 3 minutes";
                __instance.Contraband = true;
            }
            else if (inType == EWarpDriveProgramType.SHUTDOWN_DEFENSES)
            {
                __instance.Desc = "Broadcasts [Shutdown Defenses] virus to nearby ships on activation. Incurs a slight Cyber-Atk penalty when infecting.\n\nShutdown Defenses: Disables defensive systems for 20 seconds";
                __instance.Contraband = true;
            }
            else if (inType == EWarpDriveProgramType.DETECTOR)
            {
                __instance.Desc = "Reveals all ships in the sector for 10 seconds.";
                ___Detector_BoostAmount = 1f;
                ___Detector_ActiveTime = 10f;

            }
            else if (inType == EWarpDriveProgramType.BLOCK_LONG_RANGE_COMMS)
            {
                ___m_MarketPrice = (ObscuredInt)9000;
                __instance.Contraband = true;
            }
            else if (inType == EWarpDriveProgramType.RAND_LARGE)
            {
                __instance.VirusType = EVirusType.RAND_LARGE;
                __instance.Experimental = true;
            }
            else if (inType == EWarpDriveProgramType.RAND_SMALL)
                __instance.Experimental = true;
            else if (inType == EWarpDriveProgramType.SIPHEN)
                __instance.Contraband = true;
            else if (inType == EWarpDriveProgramType.SHOCK_THE_SYSTEM)
                __instance.Experimental = true;
        }
    }
}

