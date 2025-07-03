using HarmonyLib;
using PulsarModLoader;
using PulsarModLoader.Content.Components.Hull;
using PulsarModLoader.Content.Components.WarpDriveProgram;
using System;
using System.Collections.Generic;
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
            private static void Postfix(PLWarpDriveProgram __instance, EWarpDriveProgramType inType, int inLevel = 0, short inSubData = 0)
            {
                if (inType == EWarpDriveProgramType.SHIELD_BOOSTER)
                    __instance.Desc = "Increases the charge rate of shields by 25% for 15 seconds.";
                if (inType == EWarpDriveProgramType.SUPER_SHIELD_BOOSTER)
                    __instance.Desc = "Increases the charge rate of shields by 50% for 15 seconds.";
                if (inType == EWarpDriveProgramType.OVERCHARGE)
                {
                    __instance.Desc = "Increases reactor output by 50% and prevents meltdown for 20 seconds.";
                    Traverse.Create(__instance).Field("Overcharge_ActiveTime").SetValue(15f);
                }
                if (inType == EWarpDriveProgramType.EXTENDED_SHIELDS)
                {
                    __instance.Desc = "Increases max shields by +100 for 60 seconds.";
                    Traverse.Create(__instance).Field("ExShields_BoostAmount").SetValue(100f);
                }
                if (inType == EWarpDriveProgramType.VIRUS_BOOSTER)
                    __instance.Desc = "+0.5 CyberAttack for 30 seconds";
                if (inType == EWarpDriveProgramType.BARRAGE)
                    __instance.Desc += " This effect does not stack.";
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

        [HarmonyPatch(typeof(PLShop_Exotic1), "CreateInitialWares")]
        internal class Shop_Exotic1Heat
        {
            private static void Postfix(PLShop_Processors __instance, TraderPersistantDataEntry inPDE)
            {
                if (inPDE == null)
                    return;
                PLShipComponent componentFromHash = PLShipComponent.CreateShipComponentFromHash((int)PLShipComponent.createHashFromInfo((int)ESlotType.E_COMP_PROGRAM, WarpDriveProgramModManager.Instance.GetWarpDriveProgramIDFromName("H.E.A.T."), 0, 0, 12));
                componentFromHash.NetID = inPDE.ServerWareIDCounter;
                inPDE.Wares.Add(inPDE.ServerWareIDCounter, (PLWare)componentFromHash);
                ++inPDE.ServerWareIDCounter;
            }
        }
    }
}
