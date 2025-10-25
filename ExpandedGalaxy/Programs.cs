using CodeStage.AntiCheat.ObscuredTypes;
using HarmonyLib;
using PulsarModLoader;
using PulsarModLoader.Content.Components.Hull;
using PulsarModLoader.Content.Components.Virus;
using PulsarModLoader.Content.Components.WarpDriveProgram;
using PulsarModLoader.Patches;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using UnityEngine;

namespace ExpandedGalaxy
{
    internal class Programs
    {
        private class EMPModded : WarpDriveProgramMod
        {
            public override string Name => "EMP";

            public override string Description => "Shuts down EVERY ship not equipped to handle EMP attacks within 3.5km";

            public override int MarketPrice => 2500;

            public override string ShortName => "EM";

            public override float ActiveTime => 0.1f;

            public override int MaxLevelCharges => 5;

            public override void Execute(PLWarpDriveProgram InWarpDriveProgram)
            {
                if (PLServer.GetCurrentSector() == null || !PhotonNetwork.isMasterClient)
                    return;
                ModMessage.SendRPC("sugarbuzz1.ExpandedGalaxy", "ExpandedGalaxy.EMPPulse", PhotonTargets.All, new object[2]
                        {
                            (object) InWarpDriveProgram.ShipStats.Ship.ShipID,
                            (object) 3500f
                        });
                if (InWarpDriveProgram.ShipStats.Ship.ShipTypeID == EShipType.E_FLUFFY_DELIVERY || InWarpDriveProgram.ShipStats.Ship.ShipTypeID == EShipType.E_FLUFFY_TWO || InWarpDriveProgram.ShipStats.Ship.ShipTypeID == EShipType.E_ABYSS_PLAYERSHIP)
                    return;
                InWarpDriveProgram.ShipStats.Ship.photonView.RPC("Overcharged", PhotonTargets.All);
            }
        }

        private class HeatMod : WarpDriveProgramMod
        {
            public override string Name => "H.E.A.T.";

            public override string Description => "Highly Erosive Ammunition for Turrets. Causes all physical damage turrets to deal acid damage for 60 seconds. Affected turrets deal 60% less damage.";

            public override int MarketPrice => 6000;

            public override string ShortName => "HE";

            public override float ActiveTime => 60f;

            public override int MaxLevelCharges => 3;

            public override bool Experimental => true;
        }

        [HarmonyPatch(typeof(PLShipInfoBase), "TakeDamage")]
        internal class HeatDamage
        {
            private static bool Prefix(PLShipInfoBase __instance, ref float dmg, ref EDamageType dmgType, float randomNum, int SystemTargetID, PLShipInfoBase attackingShip, int turretID)
            {
                bool flag = false;
                if (attackingShip != null)
                {
                    List<PLShipComponent> programs = attackingShip.MyStats.GetComponentsOfType(ESlotType.E_COMP_PROGRAM);
                    foreach (PLShipComponent p in programs)
                    {
                        PLWarpDriveProgram program = p as PLWarpDriveProgram;
                        if (program != null && program.SubType == WarpDriveProgramModManager.Instance.GetWarpDriveProgramIDFromName("H.E.A.T."))
                        {
                            if (program.GetActiveTimerAlpha() < 1f)
                            {
                                flag = true;
                            }
                        }
                    }
                    if (!flag)
                        return true;

                    PLTurret turret = attackingShip.GetTurretAtID(turretID);
                    if (turret != null && dmgType == EDamageType.E_PHYSICAL)
                    {
                        dmg = dmg * 0.4f;
                        dmgType = EDamageType.E_BIOHAZARD;
                    }
                }
                return true;
            }
        }

        private class DigQuake : WarpDriveProgramMod
        {
            public override string Name => "Digital Earthquake";

            public override string Description => "An ancient code that causes all ships infected with a virus in the system to disintegrate. Its power scales with the number of unique viruses";

            public override int MarketPrice => 12000;

            public override string ShortName => "DQ";

            public override float ActiveTime => 0.1f;

            public override int MaxLevelCharges => 5;

            public override void Execute(PLWarpDriveProgram InWarpDriveProgram)
            {
                if (PLServer.GetCurrentSector() == null || !PhotonNetwork.isMasterClient)
                    return;
                List<int> virusIDs = new List<int>();
                foreach (PLShipInfoBase plShipInfoBase in UnityEngine.Object.FindObjectsOfType(typeof(PLShipInfoBase)))
                {
                    virusIDs.Clear();
                    foreach (PLVirus virus in plShipInfoBase.MyStats.GetComponentsOfType(ESlotType.E_COMP_VIRUS))
                    {
                        if (!virusIDs.Contains(virus.SubType))
                            virusIDs.Add(virus.SubType);
                    }
                    if (virusIDs.Count > 0)
                    {
                        plShipInfoBase.MyStats.TakeHullDamage((float)(200.0 * Math.Pow(1.5, virusIDs.Count - 1)), EDamageType.E_ARMOR_PIERCE_PHYS, InWarpDriveProgram.ShipStats.Ship, null);
                        plShipInfoBase.RemoveHostileViruses();
                        plShipInfoBase.DestroySelfIfDead(InWarpDriveProgram.ShipStats.Ship);
                    }
                }
            }
        }

        private class CorruptionProgramMod : WarpDriveProgramMod
        {
            public override string Name => "Corruption [VIRUS]";

            public override string Description => "Broadcasts [Corruption] virus to nearby ships on activation.\n\nCorruption: Causes the ship systems to malfunction for 5 minutes";

            public override int MarketPrice => 20000;

            public override string ShortName => "CR";

            public override bool IsVirus => true;

            public override int VirusSubtype => (int)EVirusType.CORRUPTION;

            public override int MaxLevelCharges => 4;

            public override float ActiveTime => 30f;

            public override Texture2D IconTexture => PLGlobal.Instance.VirusBGTexture;
        }

        [HarmonyPatch(typeof(PLVirus), "CorruptionUpdate")]
        internal class CorruptionDamage
        {
            private static void Postfix(PLVirus __instance, PLShipStats stats, ref float ___LastSiphenAttemptTime)
            {
                if (!PhotonNetwork.isMasterClient)
                    return;
                if ((double)Time.time - (double)___LastSiphenAttemptTime <= 6.0)
                    return;
                ___LastSiphenAttemptTime = Time.time;
                if (__instance.ShipStats.Ship != null)
                {
                    PLMainSystem system = __instance.ShipStats.Ship.GetSystemFromID(UnityEngine.Random.Range(0, 4));
                    if (system != null)
                    {
                        system.TakeDamage((float)system.MaxHealth * UnityEngine.Random.Range(0.05f, 0.2f));
                    }
                }
            }
        }

        [HarmonyPatch(typeof(PLShipInfoBase), "Update")]
        internal class CorruptionControls
        {
            private static void Postfix(PLShipInfoBase __instance)
            {
                if (!(PLServer.Instance != null))
                    return;
                if (PLNetworkManager.Instance.LocalPlayer == null || PLNetworkManager.Instance.MyLocalPawn == null)
                    return;
                if (!(__instance != null && __instance.MyStats != null))
                    return;
                bool flag = false;
                if (__instance.GetCurrentShipControllerPlayerID() == PLNetworkManager.Instance.LocalPlayer.GetPlayerID())
                {
                    List<PLShipComponent> components = __instance.MyStats.GetComponentsOfType(ESlotType.E_COMP_VIRUS);
                    foreach (PLShipComponent component in components)
                    {
                        PLVirus virus = component as PLVirus;
                        if (virus != null && virus.SubType == (int)EVirusType.CORRUPTION)
                        {
                            flag = true;
                            break;
                        }
                    }
                    if (flag)
                    {
                        PLInput.Instance.EInputActionNameToString[54] = "flight_left";
                        PLInput.Instance.EInputActionNameToString[53] = "flight_right";
                        PLInput.Instance.EInputActionNameToString[56] = "flight_forward";
                        PLInput.Instance.EInputActionNameToString[55] = "flight_back";
                        PLInput.Instance.EInputActionNameToString[59] = "flight_roll_right";
                        PLInput.Instance.EInputActionNameToString[60] = "flight_roll_left";
                        PLInput.Instance.EInputActionNameToString[63] = "full_throttle";
                        PLInput.Instance.EInputActionNameToString[62] = "full_reverse_throttle";
                    }
                    else
                    {
                        PLInput.Instance.EInputActionNameToString[54] = "flight_back";
                        PLInput.Instance.EInputActionNameToString[53] = "flight_forward";
                        PLInput.Instance.EInputActionNameToString[56] = "flight_left";
                        PLInput.Instance.EInputActionNameToString[55] = "flight_right";
                        PLInput.Instance.EInputActionNameToString[59] = "flight_roll_left";
                        PLInput.Instance.EInputActionNameToString[60] = "flight_roll_right";
                        PLInput.Instance.EInputActionNameToString[63] = "full_reverse_throttle";
                        PLInput.Instance.EInputActionNameToString[62] = "full_throttle";
                    }
                }
            }
        }

        private class SpecialTrainingMod : WarpDriveProgramMod
        {
            public override string Name => "Special Training [VIRUS]";

            public override string Description => "Broadcasts [Special Training] virus to nearby ships on activation.\n\nSpecial Training: Slowly fills ship with acidic gas for 30 seconds.";

            public override int MarketPrice => 7600;

            public override string ShortName => "ST";

            public override bool IsVirus => true;

            public override int VirusSubtype => (int)VirusModManager.Instance.GetVirusIDFromName("Special Training");

            public override int MaxLevelCharges => 5;

            public override float ActiveTime => 30f;

            public override Texture2D IconTexture => PLGlobal.Instance.VirusBGTexture;

            public override bool Contraband => true;
        }

        private class SpecialTrainingVirusMod : VirusMod
        {
            public override string Name => "Special Training";

            public override string Description => "Slowly fills ship with acidic gas";

            public override int InfectionTimeLimitMs => 30000;

            public override void FinalLateAddStats(PLShipComponent InComp)
            {
                PLShipInfoBase pLShipInfoBase = InComp.ShipStats.Ship;
                if (pLShipInfoBase != null)
                {
                    pLShipInfoBase.AcidicAtmoBoostAlpha += 0.2f * Time.deltaTime;
                    pLShipInfoBase.AuxConfig &= (byte) 251U;
                }
            }
        }

        [HarmonyPatch(typeof(PLWarpDriveProgram), "Tick")]
        internal class ShieldBoostNumbers
        {
            private static void Postfix(PLWarpDriveProgram __instance, ref float ___ShieldBooster_BoostAmount, ref float ___SuperShieldBooster_BoostAmount)
            {
                if (__instance.ShipStats != null)
                {
                    if (__instance.ShipStats.Ship != null)
                    {
                        if (__instance.ShipStats.Ship.MyShieldGenerator != null)
                        {
                            ___ShieldBooster_BoostAmount = __instance.ShipStats.Ship.MyShieldGenerator.ChargeRateCurrent * 0.25f;
                            ___SuperShieldBooster_BoostAmount = __instance.ShipStats.Ship.MyShieldGenerator.ChargeRateCurrent * 0.5f;
                        }
                    }
                }
            }
        }

        [HarmonyPatch(typeof(PLWarpDriveProgram), MethodType.Constructor, new Type[3] { typeof(EWarpDriveProgramType), typeof(int), typeof(short) })]
        internal class ProgramText
        {
            private static void Postfix(PLWarpDriveProgram __instance, EWarpDriveProgramType inType, int inLevel, short inSubData, ref float ___Detector_BoostAmount, ref float ___Detector_ActiveTime, ref float ___ShieldBooster_ActiveTime, ref ObscuredInt ___m_MarketPrice)
            {
                if (inType == EWarpDriveProgramType.SHIELD_BOOSTER)
                    __instance.Desc = "Increases the charge rate of shields by 25% for 15 seconds.";
                else if (inType == EWarpDriveProgramType.SUPER_SHIELD_BOOSTER)
                    __instance.Desc = "Increases the charge rate of shields by 50% for 15 seconds.";
                else if (inType == EWarpDriveProgramType.OVERCHARGE)
                {
                    __instance.Desc = "Increases reactor output by 50% and prevents meltdown for 15 seconds.";
                    Traverse.Create(__instance).Field("Overcharge_ActiveTime").SetValue(15f);
                }
                else if (inType == EWarpDriveProgramType.EXTENDED_SHIELDS)
                {
                    __instance.Desc = "Increases max shields by +100 for 60 seconds.";
                    Traverse.Create(__instance).Field("ExShields_BoostAmount").SetValue(100f);
                }
                else if (inType == EWarpDriveProgramType.VIRUS_BOOSTER)
                    __instance.Desc = "+0.5 CyberAttack for 30 seconds";
                else if (inType == EWarpDriveProgramType.BARRAGE)
                    __instance.Desc += " This effect does not stack.";
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
                    __instance.Desc = "Reveals all ships in the sector for 30 seconds.";
                    ___Detector_BoostAmount = 1f;
                    ___Detector_ActiveTime = 30f;

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

        [HarmonyPatch(typeof(PLWarpDriveProgram), "ExecuteBasedOnType")]
        internal class ExShieldsBuff
        {
            private static bool Prefix(PLWarpDriveProgram __instance)
            {
                if (__instance.SubType != (int)EWarpDriveProgramType.EXTENDED_SHIELDS)
                    return true;
                __instance.Level = 0;
                if (__instance.GetActiveTimerAlpha() > 0f && __instance.GetActiveTimerAlpha() < 1f)
                    return true;
                if (__instance.ShipStats.Ship.MyShieldGenerator != null)
                {
                    if (__instance.ShipStats.Ship.MyHull != null && __instance.ShipStats.Ship.MyHull.SubType == HullModManager.Instance.GetHullIDFromName("Juggernaut Hull"))
                        return true;
                    __instance.ShipStats.Ship.MyShieldGenerator.Current += 100f;
                    __instance.ShipStats.Ship.MyShieldGenerator.Current = Mathf.Clamp(__instance.ShipStats.Ship.MyShieldGenerator.Current, 0f, __instance.ShipStats.ShieldsMax + 100f);
                }
                return true;
            }
        }

        [HarmonyPatch(typeof(PLWarpDriveProgram), "FinalLateAddStats")]
        internal class OverdriveBuff
        {
            private static void Postfix(PLWarpDriveProgram __instance, PLShipStats inStats, ref float ___Overcharge_LastActivationTime, ref float ___Overcharge_ActiveTime)
            {
                if ((double)Time.time - (double)___Overcharge_LastActivationTime < (double)___Overcharge_ActiveTime)
                {
                    if (inStats.Ship.CoreInstability > 1.0f)
                        inStats.Ship.CoreInstability = 1.0f;
                }
            }
        }

        [HarmonyPatch(typeof(PLWarpDriveProgram), "FinalLateAddStats")]
        internal class BarrageNerf
        {
            private static bool Prefix(PLWarpDriveProgram __instance, PLShipStats inStats)
            {
                if (__instance.SubType == (int)EWarpDriveProgramType.BARRAGE)
                {
                    foreach (PLWarpDriveProgram program in inStats.GetComponentsOfType(ESlotType.E_COMP_PROGRAM))
                    {
                        if (__instance == program)
                            return true;
                        else
                        {
                            if (program.SubType == (int)EWarpDriveProgramType.BARRAGE && (double)program.GetActiveTimerAlpha() > 0.0 && (double)program.GetActiveTimerAlpha() < 1.0)
                                return false;
                        }
                    }
                }
                return true;
            }
        }

        [HarmonyPatch(typeof(PLSensorObject), "InternalDetectionSignatureIsDetected")]
        internal class DetectorRework
        {
            private static void Postfix(PLSensorObject __instance, ref PLSensorObjectCacheData socd, float sqDist, float signature, float detection, PLShipInfoBase detectorShip)
            {
                if (!(__instance is PLSensorObjectShip))
                    return;
                socd.IsDetected |= HasActiveProgramOfType(detectorShip, 15);
            }
        }

        [HarmonyPatch(typeof(PLVirus), MethodType.Constructor, new Type[3] { typeof(EVirusType), typeof(int), typeof(short) })]
        internal class BetterRand
        {
            private static void Postfix(PLVirus __instance, EVirusType inType, int inLevel, short inSubTypeData)
            {
                if (__instance.SubType != (int)EVirusType.RAND_SMALL && __instance.SubType != (int)EVirusType.RAND_LARGE)
                    return;
                if (PhotonNetwork.isMasterClient)
                {
                    PLRand rand = new PLRand((int)PLServer.Instance.GalaxySeed + PLServer.Instance.GetEstimatedServerMs());
                    __instance.SubTypeData = (short)(rand.Next() % 4);
                }
                if (__instance.SubType == (int )EVirusType.RAND_LARGE)
                {
                    __instance.Desc = "Unknown/random function";
                }
            }
        }

        [HarmonyPatch(typeof(PLVirus), "ShouldApplyRandSmallEffectID")]
        internal class RandSmallFix
        {
            private static bool Prefix(PLVirus __instance, int inID, ref bool __result)
            {
                if (__instance.SubType != 14 || __instance.NetID == -1)
                {
                    __result = false;
                    return false;
                }
                __result = (int)__instance.SubTypeData == inID;
                return false;
            }
        }

        [HarmonyPatch(typeof(PLScientistVirusScreen), "Update")]
        internal class RandText
        {
            private static void Postfix(PLScientistVirusScreen __instance)
            {
                foreach (PLVirusDrawInfo drawInfo in __instance.VirusDrawInfos)
                {
                    if (drawInfo.MyVirus == null)
                        continue;
                    if (drawInfo.MyVirus.SubType == (int)EVirusType.RAND_SMALL || drawInfo.MyVirus.SubType == (int)EVirusType.RAND_LARGE)
                    {
                        string Desc = "";
                        if (drawInfo.MyVirus.SubType == (int)EVirusType.RAND_SMALL)
                        {
                            switch (drawInfo.MyVirus.SubTypeData)
                            {
                                case 0:
                                    Desc = "Backdoor/Phalanx";
                                    break;
                                case 1:
                                    Desc = "Sitting Duck/Warp Disable";
                                    break;
                                case 2:
                                    Desc = "Blindfold/Expose";
                                    break;
                                case 3:
                                    Desc = "Sitting Duck/Phalanx";
                                    break;
                            }
                        }
                        else
                        {
                            switch (drawInfo.MyVirus.SubTypeData)
                            {
                                case 0:
                                    Desc = "Syber's Shield/Phalanx";
                                    break;
                                case 1:
                                    Desc = "Armor Flaw/Breathless";
                                    break;
                                case 2:
                                    Desc = "Backdoor/Lazy Guns";
                                    break;
                                case 3:
                                    Desc = "Blindfold/Warp Disable";
                                    break;
                            }
                        }
                        drawInfo.MyDescLabel.text = Desc;
                    }
                }
            }
        }

        [HarmonyPatch(typeof(PLVirus), "FinalLateAddStats")]
        internal class VirusEffect
        {
            private static bool Prefix(PLVirus __instance, ref PLShipStats inStats)
            {
                if (__instance.SubType == (int)EVirusType.RAND_SMALL || __instance.SubType == (int)EVirusType.RAND_LARGE)
                    /*ModMessage.SendRPC("sugarbuzz1.ExpandedGalaxy", "ExpandedGalaxy.UpdateSubTypeData", PhotonTargets.Others, new object[3]
                    {
                        (object) __instance.ShipStats.Ship.ShipID,
                        (object) __instance.NetID,
                        (object) __instance.SubTypeData,
                    });*/
                if (__instance.SubType != (int)EVirusType.RAND_LARGE)
                    return true;
                switch ((int)__instance.SubTypeData)
                {
                    case 0:
                        inStats.TurretDamageFactor *= 0.7f;
                        inStats.ShieldsChargeRate = 0.0f;
                        break;
                    case 1:
                        inStats.HullArmor *= 0.5f;
                        inStats.OxygenRefillRate = 0.0f;
                        break;
                    case 2:
                        inStats.CyberDefenseRating = 0.0f;
                        inStats.TurretChargeFactor *= 0.7f;
                        break;
                    case 3:
                        inStats.EMDetection = 0.0f;
                        inStats.WarpChargeRate = 0.0f;
                        break;
                }
                return true;
            }
        }

        [HarmonyPatch(typeof(PLShop_Exotic1), "CreateInitialWares")]
        internal class Shop_Exotic1Heat
        {
            private static void Postfix(PLShop_Exotic1 __instance, TraderPersistantDataEntry inPDE)
            {
                if (inPDE == null)
                    return;
                PLShipComponent componentFromHash = PLShipComponent.CreateShipComponentFromHash((int)PLShipComponent.createHashFromInfo((int)ESlotType.E_COMP_PROGRAM, WarpDriveProgramModManager.Instance.GetWarpDriveProgramIDFromName("H.E.A.T."), 0, 0, 12));
                componentFromHash.NetID = inPDE.ServerWareIDCounter;
                inPDE.Wares.Add(inPDE.ServerWareIDCounter, (PLWare)componentFromHash);
                ++inPDE.ServerWareIDCounter;
            }
        }

        [HarmonyPatch(typeof(PLShop_Exotic2), "CreateInitialWares")]
        internal class Shop_Exotic2RandLarge
        {
            private static void Postfix(PLShop_Exotic2 __instance, TraderPersistantDataEntry inPDE)
            {
                if (inPDE == null)
                    return;
                PLShipComponent componentFromHash = PLShipComponent.CreateShipComponentFromHash((int)PLShipComponent.createHashFromInfo((int)ESlotType.E_COMP_PROGRAM, (int)EWarpDriveProgramType.RAND_LARGE, 0, 0, 12));
                componentFromHash.NetID = inPDE.ServerWareIDCounter;
                inPDE.Wares.Add(inPDE.ServerWareIDCounter, (PLWare)componentFromHash);
                ++inPDE.ServerWareIDCounter;
            }
        }

        public static bool HasActiveProgramOfType(PLShipInfoBase shipInfo, int programType)
        {
            foreach (PLWarpDriveProgram program in shipInfo.MyStats.GetComponentsOfType(ESlotType.E_COMP_PROGRAM))
            {
                if (program.SubType == programType)
                    if ((double)program.GetActiveTimerAlpha() > 0.0 && (double)program.GetActiveTimerAlpha() < 1.0)
                        return true;
            }
            return false;
        }

        [HarmonyPatch(typeof(PLWarpDrive), "ChargePrograms")]
        internal class EnemyFullChargePrograms
        {
            private static bool Prefix(PLWarpDrive __instance, ref bool chargeToFull, int overrideChargeCount)
            {
                if (!__instance.ShipStats.Ship.GetIsPlayerShip())
                    chargeToFull = true;
                return true;
            }
        }

        [HarmonyPatch(typeof(PLScientistComputerScreen), "Update")]
        internal class ProgramFGColor
        {
            internal static Color GetFGColorForProgram(PLWarpDriveProgram program)
            {
                if (program.Experimental)
                    return new Color(0.7f, 0.7f, 0.1f, 1f);
                else if (program.Contraband)
                    return new Color(0.8f, 0f, 0f, 1f);
                else if (Relic.getIsRelic(program))
                    return Relic.getRelicColor();
                return new Color(0.65f, 0.65f, 0.65f, 1f);
            }
            private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
            {
                List<CodeInstruction> list = instructions.ToList();
                LocalBuilder index1 = null;
                List<Label> label = new List<Label>();

                for (int i = 0; i < list.Count; i++)
                {
                    CodeInstruction codeInstruction = list[i];
                    if (codeInstruction.opcode == OpCodes.Ldloc_S && codeInstruction.operand is LocalBuilder lb1 && lb1.LocalIndex == 40)
                    {
                        index1 = lb1;
                        if (codeInstruction.labels.Count > 0 && i > 1200)
                            label.AddRange(codeInstruction.labels);
                    }
                    if (index1 != null && label.Count > 0)
                        break;
                }

                List<CodeInstruction> targetSequence = new List<CodeInstruction>() {
                    new CodeInstruction(OpCodes.Ldloc_S, index1),
                    new CodeInstruction(OpCodes.Ldarg_0),
                    new CodeInstruction(OpCodes.Ldfld, AccessTools.Field(typeof(PLUIScreen), "UI_White")),
                };
                List<CodeInstruction> patchSequence = new List<CodeInstruction>()
                {
                    new CodeInstruction(OpCodes.Ldloc_S, (byte)40),
                    new CodeInstruction(OpCodes.Ldloc_S, (byte)43),
                    new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(ProgramFGColor), "GetFGColorForProgram", new Type[1] {typeof(PLWarpDriveProgram)})),
                };

                List<CodeInstruction> list2 = HarmonyHelpers.PatchBySequence(list.AsEnumerable<CodeInstruction>(), targetSequence, patchSequence, HarmonyHelpers.PatchMode.REPLACE, HarmonyHelpers.CheckMode.NONNULL, true).ToList<CodeInstruction>();
                patchSequence[0].labels.AddRange(label);
                return HarmonyHelpers.PatchBySequence(list2.AsEnumerable<CodeInstruction>(), targetSequence, patchSequence, HarmonyHelpers.PatchMode.REPLACE, HarmonyHelpers.CheckMode.NONNULL, true);
            }
        }
    }
}

