using HarmonyLib;
using PulsarModLoader.Content.Components.MegaTurret;
using PulsarModLoader.Content.Components.Shield;
using PulsarModLoader.Content.Components.Turret;
using PulsarModLoader.Patches;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using UnityEngine;

namespace ExpandedGalaxy
{
    internal class StarterInfo
    {
        [HarmonyPatch(typeof(PLCruiserInfo), "SetupShipStats")]
        internal class CruiserInfoMod
        {
            private static void Postfix(PLCruiserInfo __instance, bool previewStats, bool startingPlayerShip)
            {
                if (__instance == null || __instance.MyStats == null)
                    return;
                if (!__instance.ShouldCreateDefaultComponents || !(PhotonNetwork.isMasterClient | previewStats))
                    return;
                __instance.MyStats.RemoveShipComponent((PLShipComponent)__instance.MyStats.GetShipComponent<PLMegaTurret>(ESlotType.E_COMP_MAINTURRET));
                if (startingPlayerShip)
                {
                    __instance.MyStats.AddShipComponent(PLShipComponent.CreateShipComponentFromHash((int)PLShipComponent.createHashFromInfo((int)ESlotType.E_COMP_MAINTURRET, MegaTurretModManager.Instance.GetMegaTurretIDFromName("WD Standard"), 0, 0, 12)), visualSlot: ESlotType.E_COMP_MAINTURRET);
                }
                else
                {
                    PLRand deterministicRand = PLShipInfoBase.GetShipDeterministicRand(__instance.PersistantShipInfo);
                    __instance.MyStats.AddShipComponent(PLShipComponent.CreateShipComponentFromHash((int)PLShipComponent.createHashFromInfo((int)ESlotType.E_COMP_MAINTURRET, MegaTurretModManager.Instance.GetMegaTurretIDFromName("WD Standard"), __instance.GetChaosBoost(deterministicRand.Next() % 50), 0, 12)), visualSlot: ESlotType.E_COMP_MAINTURRET);
                    __instance.MyStats.RemoveShipComponent((PLShipComponent)__instance.MyStats.GetShipComponent<PLHullPlating>(ESlotType.E_COMP_HULLPLATING));
                    __instance.MyStats.AddShipComponent(PLShipComponent.CreateShipComponentFromHash((int)PLShipComponent.createHashFromInfo((int)ESlotType.E_COMP_HULLPLATING, (int)EHullPlatingType.E_HULLPLATING_CCGE, __instance.GetChaosBoost(deterministicRand.Next() % 50), 0, 12)), visualSlot: ESlotType.E_COMP_HULLPLATING);
                    if ((double)(float)PLServer.Instance.ChaosLevel > 1.0)
                    {
                        float num = Mathf.Clamp01((float)((double)deterministicRand.NextFloat() + (double)(float)PLServer.Instance.ChaosLevel * 0.01999999731779099));
                        if (num >= 0.85)
                        {
                            __instance.MyStats.RemoveShipComponent(__instance.MyStats.GetComponentsOfType(ESlotType.E_COMP_TURRET)[deterministicRand.Next() % 2]);
                            bool flag = (double)(float)PLServer.Instance.ChaosLevel > 2.0 && deterministicRand.Next() % 3 == 0;
                            if (flag)
                                __instance.MyStats.AddShipComponent(PLShipComponent.CreateShipComponentFromHash((int)PLShipComponent.createHashFromInfo((int)ESlotType.E_COMP_TURRET, TurretModManager.Instance.GetTurretIDFromName("Particle Lance"), __instance.GetChaosBoost(deterministicRand.Next() % 50), 0, 12)), visualSlot: ESlotType.E_COMP_TURRET);
                            else
                                __instance.MyStats.AddShipComponent(PLShipComponent.CreateShipComponentFromHash((int)PLShipComponent.createHashFromInfo((int)ESlotType.E_COMP_TURRET, TurretModManager.Instance.GetTurretIDFromName("Sylvassi Turret"), __instance.GetChaosBoost(deterministicRand.Next() % 50), 0, 12)), visualSlot: ESlotType.E_COMP_TURRET);
                        }
                    }
                }
            }
        }

        [HarmonyPatch(typeof(PLWDDestroyerInfo), "SetupShipStats")]
        internal class DestroyerInfoMod
        {
            private static void Postfix(PLWDDestroyerInfo __instance, bool previewStats, bool startingPlayerShip)
            {
                if (__instance == null || __instance.MyStats == null)
                    return;
                if (!__instance.ShouldCreateDefaultComponents || !(PhotonNetwork.isMasterClient | previewStats))
                    return;
                __instance.MyStats.RemoveShipComponent((PLShipComponent)__instance.MyStats.GetShipComponent<PLMegaTurret>(ESlotType.E_COMP_MAINTURRET));
                if (startingPlayerShip)
                {
                    __instance.MyStats.AddShipComponent(PLShipComponent.CreateShipComponentFromHash((int)PLShipComponent.createHashFromInfo((int)ESlotType.E_COMP_MAINTURRET, MegaTurretModManager.Instance.GetMegaTurretIDFromName("WD Standard"), 0, 0, 12)), visualSlot: ESlotType.E_COMP_MAINTURRET);
                }
                else
                {
                    PLRand deterministicRand = PLShipInfoBase.GetShipDeterministicRand(__instance.PersistantShipInfo);
                    __instance.MyStats.AddShipComponent(PLShipComponent.CreateShipComponentFromHash((int)PLShipComponent.createHashFromInfo((int)ESlotType.E_COMP_MAINTURRET, MegaTurretModManager.Instance.GetMegaTurretIDFromName("WD Standard"), __instance.GetChaosBoost(deterministicRand.Next() % 50), 0, 12)), visualSlot: ESlotType.E_COMP_MAINTURRET);
                    __instance.MyStats.RemoveShipComponent((PLShipComponent)__instance.MyStats.GetShipComponent<PLHullPlating>(ESlotType.E_COMP_HULLPLATING));
                    __instance.MyStats.AddShipComponent(PLShipComponent.CreateShipComponentFromHash((int)PLShipComponent.createHashFromInfo((int)ESlotType.E_COMP_HULLPLATING, (int)EHullPlatingType.E_HULLPLATING_CCGE, __instance.GetChaosBoost(deterministicRand.Next() % 50), 0, 12)), visualSlot: ESlotType.E_COMP_HULLPLATING);
                    if ((double)(float)PLServer.Instance.ChaosLevel > 1.0)
                    {
                        float num = Mathf.Clamp01((float)((double)deterministicRand.NextFloat() + (double)(float)PLServer.Instance.ChaosLevel * 0.01999999731779099));
                        if (num >= 0.85)
                        {
                            __instance.MyStats.RemoveShipComponent(__instance.MyStats.GetComponentsOfType(ESlotType.E_COMP_TURRET)[deterministicRand.Next() % 1]);
                            bool flag = (double)(float)PLServer.Instance.ChaosLevel > 2.0 && deterministicRand.Next() % 3 == 0;
                            if (flag)
                                __instance.MyStats.AddShipComponent(PLShipComponent.CreateShipComponentFromHash((int)PLShipComponent.createHashFromInfo((int)ESlotType.E_COMP_TURRET, TurretModManager.Instance.GetTurretIDFromName("Particle Lance"), __instance.GetChaosBoost(deterministicRand.Next() % 50), 0, 12)), visualSlot: ESlotType.E_COMP_TURRET);
                            else
                                __instance.MyStats.AddShipComponent(PLShipComponent.CreateShipComponentFromHash((int)PLShipComponent.createHashFromInfo((int)ESlotType.E_COMP_TURRET, TurretModManager.Instance.GetTurretIDFromName("Sylvassi Turret"), __instance.GetChaosBoost(deterministicRand.Next() % 50), 0, 12)), visualSlot: ESlotType.E_COMP_TURRET);
                        }
                    }
                }
            }
        }

        [HarmonyPatch(typeof(PLWDAnnihilatorInfo), "SetupShipStats")]
        internal class AnnihilatorInfoMod
        {
            private static void Postfix(PLWDAnnihilatorInfo __instance, bool previewStats, bool startingPlayerShip)
            {
                if (__instance == null || __instance.MyStats == null)
                    return;
                if (!__instance.ShouldCreateDefaultComponents || !(PhotonNetwork.isMasterClient | previewStats))
                    return;
                if (startingPlayerShip)
                    return;
                PLRand deterministicRand = PLShipInfoBase.GetShipDeterministicRand(__instance.PersistantShipInfo);
                __instance.MyStats.RemoveShipComponent((PLShipComponent)__instance.MyStats.GetShipComponent<PLHullPlating>(ESlotType.E_COMP_HULLPLATING));
                __instance.MyStats.AddShipComponent(PLShipComponent.CreateShipComponentFromHash((int)PLShipComponent.createHashFromInfo((int)ESlotType.E_COMP_HULLPLATING, (int)EHullPlatingType.E_HULLPLATING_CCGE, __instance.GetChaosBoost(deterministicRand.Next() % 50), 0, 12)), visualSlot: ESlotType.E_COMP_HULLPLATING);
                if ((double)(float)PLServer.Instance.ChaosLevel > 1.0)
                {
                    float num = Mathf.Clamp01((float)((double)deterministicRand.NextFloat() + (double)(float)PLServer.Instance.ChaosLevel * 0.01999999731779099));
                    if (num >= 0.85)
                    {
                        __instance.MyStats.RemoveShipComponent(__instance.MyStats.GetComponentsOfType(ESlotType.E_COMP_TURRET)[deterministicRand.Next() % 2]);
                        bool flag = (double)(float)PLServer.Instance.ChaosLevel > 2.0 && deterministicRand.Next() % 3 == 0;
                        if (flag)
                            __instance.MyStats.AddShipComponent(PLShipComponent.CreateShipComponentFromHash((int)PLShipComponent.createHashFromInfo((int)ESlotType.E_COMP_TURRET, TurretModManager.Instance.GetTurretIDFromName("Particle Lance"), __instance.GetChaosBoost(deterministicRand.Next() % 50), 0, 12)), visualSlot: ESlotType.E_COMP_TURRET);
                        else
                            __instance.MyStats.AddShipComponent(PLShipComponent.CreateShipComponentFromHash((int)PLShipComponent.createHashFromInfo((int)ESlotType.E_COMP_TURRET, TurretModManager.Instance.GetTurretIDFromName("Sylvassi Turret"), __instance.GetChaosBoost(deterministicRand.Next() % 50), 0, 12)), visualSlot: ESlotType.E_COMP_TURRET);
                    }
                }
            }
        }

        [HarmonyPatch(typeof(PLIntrepidInfo), "SetupShipStats")]
        internal class IntrepidInfoMod
        {
            private static void Postfix(PLIntrepidInfo __instance, bool previewStats, bool startingPlayerShip)
            {
                if (__instance == null || __instance.MyStats == null)
                    return;
                if (!__instance.ShouldCreateDefaultComponents || !(PhotonNetwork.isMasterClient | previewStats))
                    return;
                if (startingPlayerShip)
                    return;
                PLRand deterministicRand = PLShipInfoBase.GetShipDeterministicRand(__instance.PersistantShipInfo);
                __instance.MyStats.RemoveShipComponent((PLShipComponent)__instance.MyStats.GetShipComponent<PLHullPlating>(ESlotType.E_COMP_HULLPLATING));
                __instance.MyStats.AddShipComponent(PLShipComponent.CreateShipComponentFromHash((int)PLShipComponent.createHashFromInfo((int)ESlotType.E_COMP_HULLPLATING, (int)EHullPlatingType.E_HULLPLATING_CCGE, __instance.GetChaosBoost(deterministicRand.Next() % 50), 0, 12)), visualSlot: ESlotType.E_COMP_HULLPLATING);
                if ((double)(float)PLServer.Instance.ChaosLevel > 1.0)
                {
                    float num = Mathf.Clamp01((float)((double)deterministicRand.NextFloat() + (double)(float)PLServer.Instance.ChaosLevel * 0.01999999731779099));
                    if (num >= 0.95)
                    {
                        __instance.MyStats.RemoveShipComponent(__instance.MyStats.GetComponentsOfType(ESlotType.E_COMP_TURRET)[deterministicRand.Next() % 2]);
                        bool flag = (double)(float)PLServer.Instance.ChaosLevel > 2.0 && deterministicRand.Next() % 3 == 0;
                        if (flag)
                            __instance.MyStats.AddShipComponent(PLShipComponent.CreateShipComponentFromHash((int)PLShipComponent.createHashFromInfo((int)ESlotType.E_COMP_TURRET, TurretModManager.Instance.GetTurretIDFromName("Particle Lance"), __instance.GetChaosBoost(deterministicRand.Next() % 50), 0, 12)), visualSlot: ESlotType.E_COMP_TURRET);
                        else
                            __instance.MyStats.AddShipComponent(PLShipComponent.CreateShipComponentFromHash((int)PLShipComponent.createHashFromInfo((int)ESlotType.E_COMP_TURRET, TurretModManager.Instance.GetTurretIDFromName("Sylvassi Turret"), __instance.GetChaosBoost(deterministicRand.Next() % 50), 0, 12)), visualSlot: ESlotType.E_COMP_TURRET);
                    }
                }
            }
        }

        [HarmonyPatch(typeof(PLRolandInfo), "SetupShipStats")]
        internal class RolandInfoMod
        {
            private static void Postfix(PLRolandInfo __instance, bool previewStats, bool startingPlayerShip)
            {
                if (__instance == null || __instance.MyStats == null)
                    return;
                if (!__instance.ShouldCreateDefaultComponents || !(PhotonNetwork.isMasterClient | previewStats))
                    return;
                if (startingPlayerShip)
                    return;
                PLRand deterministicRand = PLShipInfoBase.GetShipDeterministicRand(__instance.PersistantShipInfo);
                __instance.MyStats.RemoveShipComponent((PLShipComponent)__instance.MyStats.GetShipComponent<PLHullPlating>(ESlotType.E_COMP_HULLPLATING));
                __instance.MyStats.AddShipComponent(PLShipComponent.CreateShipComponentFromHash((int)PLShipComponent.createHashFromInfo((int)ESlotType.E_COMP_HULLPLATING, (int)EHullPlatingType.E_HULLPLATING_CCGE, __instance.GetChaosBoost(deterministicRand.Next() % 50), 0, 12)), visualSlot: ESlotType.E_COMP_HULLPLATING);
                if ((double)(float)PLServer.Instance.ChaosLevel > 1.0)
                {
                    float num = Mathf.Clamp01((float)((double)deterministicRand.NextFloat() + (double)(float)PLServer.Instance.ChaosLevel * 0.01999999731779099));
                    if (num >= 0.95)
                    {
                        __instance.MyStats.RemoveShipComponent(__instance.MyStats.GetComponentsOfType(ESlotType.E_COMP_TURRET)[deterministicRand.Next() % 2]);
                        bool flag = (double)(float)PLServer.Instance.ChaosLevel > 2.0 && deterministicRand.Next() % 3 == 0;
                        if (flag)
                            __instance.MyStats.AddShipComponent(PLShipComponent.CreateShipComponentFromHash((int)PLShipComponent.createHashFromInfo((int)ESlotType.E_COMP_TURRET, TurretModManager.Instance.GetTurretIDFromName("Particle Lance"), __instance.GetChaosBoost(deterministicRand.Next() % 50), 0, 12)), visualSlot: ESlotType.E_COMP_TURRET);
                        else
                            __instance.MyStats.AddShipComponent(PLShipComponent.CreateShipComponentFromHash((int)PLShipComponent.createHashFromInfo((int)ESlotType.E_COMP_TURRET, TurretModManager.Instance.GetTurretIDFromName("Sylvassi Turret"), __instance.GetChaosBoost(deterministicRand.Next() % 50), 0, 12)), visualSlot: ESlotType.E_COMP_TURRET);
                    }
                }
            }
        }

        [HarmonyPatch(typeof(PLOutriderInfo), "SetupShipStats")]
        internal class OutriderInfoMod
        {
            private static void Postfix(PLOutriderInfo __instance, bool previewStats, bool startingPlayerShip)
            {
                if (__instance == null || __instance.MyStats == null)
                    return;
                if (!__instance.ShouldCreateDefaultComponents || !(PhotonNetwork.isMasterClient | previewStats))
                    return;
                if (startingPlayerShip)
                {
                    __instance.MyStats.AddShipComponent(PLWarpDriveProgram.CreateWarpDriveProgramFromHash((int)EWarpDriveProgramType.QUANTUM_TUNNEL, 0, 0), visualSlot: ESlotType.E_COMP_PROGRAM);
                    return;
                }
                PLRand deterministicRand = PLShipInfoBase.GetShipDeterministicRand(__instance.PersistantShipInfo);
                __instance.MyStats.RemoveShipComponent((PLShipComponent)__instance.MyStats.GetShipComponent<PLHullPlating>(ESlotType.E_COMP_HULLPLATING));
                __instance.MyStats.AddShipComponent(PLShipComponent.CreateShipComponentFromHash((int)PLShipComponent.createHashFromInfo((int)ESlotType.E_COMP_HULLPLATING, (int)EHullPlatingType.E_HULLPLATING_CCGE, __instance.GetChaosBoost(deterministicRand.Next() % 50), 0, 12)), visualSlot: ESlotType.E_COMP_HULLPLATING);
                if ((double)(float)PLServer.Instance.ChaosLevel > 1.0)
                {
                    float num = Mathf.Clamp01((float)((double)deterministicRand.NextFloat() + (double)(float)PLServer.Instance.ChaosLevel * 0.01999999731779099));
                    if (num >= 0.95)
                    {
                        __instance.MyStats.RemoveShipComponent(__instance.MyStats.GetComponentsOfType(ESlotType.E_COMP_TURRET)[deterministicRand.Next() % 1]);
                        bool flag = (double)(float)PLServer.Instance.ChaosLevel > 2.0 && deterministicRand.Next() % 3 == 0;
                        if (flag)
                            __instance.MyStats.AddShipComponent(PLShipComponent.CreateShipComponentFromHash((int)PLShipComponent.createHashFromInfo((int)ESlotType.E_COMP_TURRET, TurretModManager.Instance.GetTurretIDFromName("Particle Lance"), __instance.GetChaosBoost(deterministicRand.Next() % 50), 0, 12)), visualSlot: ESlotType.E_COMP_TURRET);
                        else
                            __instance.MyStats.AddShipComponent(PLShipComponent.CreateShipComponentFromHash((int)PLShipComponent.createHashFromInfo((int)ESlotType.E_COMP_TURRET, TurretModManager.Instance.GetTurretIDFromName("Sylvassi Turret"), __instance.GetChaosBoost(deterministicRand.Next() % 50), 0, 12)), visualSlot: ESlotType.E_COMP_TURRET);
                    }
                }
            }
        }

        [HarmonyPatch(typeof(PLStarGazerInfo), "SetupShipStats")]
        internal class StargazerInfoMod
        {
            private static void Postfix(PLStarGazerInfo __instance, bool previewStats, bool startingPlayerShip)
            {
                if (__instance == null || __instance.MyStats == null)
                    return;
                if (!__instance.ShouldCreateDefaultComponents || !(PhotonNetwork.isMasterClient | previewStats))
                    return;
                PLShipComponent[] shipComponents = __instance.MyStats.AllComponents.ToArray();
                int netID = -1;
                foreach (PLShipComponent shipComponent in shipComponents)
                {
                    if (shipComponent is PLWarpDriveProgram && shipComponent.SubType == (int)EWarpDriveProgramType.BLOCK_LONG_RANGE_COMMS)
                    {
                        netID = shipComponent.NetID;
                        break;
                    }
                }
                if (netID != -1)
                    __instance.MyStats.RemoveShipComponentByNetID(netID);
                __instance.MyStats.AddShipComponent(PLShipComponent.CreateShipComponentFromHash((int)PLShipComponent.createHashFromInfo((int)ESlotType.E_COMP_PROGRAM, (int)EWarpDriveProgramType.DETECTOR, 0, 0, (int)ESlotType.E_COMP_PROGRAM)));
                if (startingPlayerShip)
                    return;
                PLRand deterministicRand = PLShipInfoBase.GetShipDeterministicRand(__instance.PersistantShipInfo);
                __instance.MyStats.RemoveShipComponent((PLShipComponent)__instance.MyStats.GetShipComponent<PLHullPlating>(ESlotType.E_COMP_HULLPLATING));
                __instance.MyStats.AddShipComponent(PLShipComponent.CreateShipComponentFromHash((int)PLShipComponent.createHashFromInfo((int)ESlotType.E_COMP_HULLPLATING, (int)EHullPlatingType.E_HULLPLATING_CCGE, __instance.GetChaosBoost(deterministicRand.Next() % 50), 0, 12)), visualSlot: ESlotType.E_COMP_HULLPLATING);
                if ((double)(float)PLServer.Instance.ChaosLevel > 1.0)
                {
                    float num = Mathf.Clamp01((float)((double)deterministicRand.NextFloat() + (double)(float)PLServer.Instance.ChaosLevel * 0.01999999731779099));
                    if (num >= 0.75)
                    {
                        __instance.MyStats.RemoveShipComponent(__instance.MyStats.GetComponentsOfType(ESlotType.E_COMP_TURRET)[deterministicRand.Next() % 2]);
                        bool flag = (double)(float)PLServer.Instance.ChaosLevel > 2.0 && deterministicRand.Next() % 3 == 0;
                        if (flag)
                            __instance.MyStats.AddShipComponent(PLShipComponent.CreateShipComponentFromHash((int)PLShipComponent.createHashFromInfo((int)ESlotType.E_COMP_TURRET, TurretModManager.Instance.GetTurretIDFromName("Particle Lance"), __instance.GetChaosBoost(deterministicRand.Next() % 50), 0, 12)), visualSlot: ESlotType.E_COMP_TURRET);
                        else
                            __instance.MyStats.AddShipComponent(PLShipComponent.CreateShipComponentFromHash((int)PLShipComponent.createHashFromInfo((int)ESlotType.E_COMP_TURRET, TurretModManager.Instance.GetTurretIDFromName("Sylvassi Turret"), __instance.GetChaosBoost(deterministicRand.Next() % 50), 0, 12)), visualSlot: ESlotType.E_COMP_TURRET);
                    }
                    if (num >= 0.5 && (double)(float)PLServer.Instance.ChaosLevel > 2.0)
                    {
                        if (deterministicRand.Next() % 3 == 0)
                        {
                            __instance.MyStats.RemoveShipComponent(__instance.MyStats.GetShipComponent<PLShieldGenerator>(ESlotType.E_COMP_SHLD));
                            __instance.MyStats.AddShipComponent(PLShipComponent.CreateShipComponentFromHash((int)PLShipComponent.createHashFromInfo((int)ESlotType.E_COMP_SHLD, ShieldModManager.Instance.GetShieldIDFromName("Reflector Shield Generator"), __instance.GetChaosBoost(deterministicRand.Next() % 50), 0, 12)), visualSlot: ESlotType.E_COMP_SHLD);
                        }
                    }
                }
            }
        }

        [HarmonyPatch(typeof(PLCarrierInfo), "SetupShipStats")]
        internal class CarrierInfoMod
        {
            private static void Postfix(PLCarrierInfo __instance, bool previewStats, bool startingPlayerShip)
            {
                if (__instance == null || __instance.MyStats == null)
                    return;
                if (!__instance.ShouldCreateDefaultComponents || !(PhotonNetwork.isMasterClient | previewStats))
                    return;
                PLShipComponent[] shipComponents = __instance.MyStats.AllComponents.ToArray();
                int netID = -1;
                foreach (PLShipComponent shipComponent in shipComponents)
                {
                    if (shipComponent is PLWarpDriveProgram && shipComponent.SubType == (int)EWarpDriveProgramType.BLOCK_LONG_RANGE_COMMS)
                    {
                        netID = shipComponent.NetID;
                        break;
                    }
                }
                if (netID != -1)
                    __instance.MyStats.RemoveShipComponentByNetID(netID);
                __instance.MyStats.AddShipComponent(PLShipComponent.CreateShipComponentFromHash((int)PLShipComponent.createHashFromInfo((int)ESlotType.E_COMP_PROGRAM, (int)EWarpDriveProgramType.DETECTOR, 0, 0, (int)ESlotType.E_COMP_PROGRAM)));
                if (startingPlayerShip)
                    return;
                PLRand deterministicRand = PLShipInfoBase.GetShipDeterministicRand(__instance.PersistantShipInfo);
                __instance.MyStats.RemoveShipComponent((PLShipComponent)__instance.MyStats.GetShipComponent<PLHullPlating>(ESlotType.E_COMP_HULLPLATING));
                __instance.MyStats.AddShipComponent(PLShipComponent.CreateShipComponentFromHash((int)PLShipComponent.createHashFromInfo((int)ESlotType.E_COMP_HULLPLATING, (int)EHullPlatingType.E_HULLPLATING_CCGE, __instance.GetChaosBoost(deterministicRand.Next() % 50), 0, 12)), visualSlot: ESlotType.E_COMP_HULLPLATING);
                if ((double)(float)PLServer.Instance.ChaosLevel > 1.0)
                {
                    float num = Mathf.Clamp01((float)((double)deterministicRand.NextFloat() + (double)(float)PLServer.Instance.ChaosLevel * 0.01999999731779099));
                    if (num >= 0.75)
                    {
                        __instance.MyStats.RemoveShipComponent(__instance.MyStats.GetComponentsOfType(ESlotType.E_COMP_TURRET)[deterministicRand.Next() % 2]);
                        bool flag = (double)(float)PLServer.Instance.ChaosLevel > 2.0 && deterministicRand.Next() % 3 == 0;
                        if (flag)
                            __instance.MyStats.AddShipComponent(PLShipComponent.CreateShipComponentFromHash((int)PLShipComponent.createHashFromInfo((int)ESlotType.E_COMP_TURRET, TurretModManager.Instance.GetTurretIDFromName("Particle Lance"), __instance.GetChaosBoost(deterministicRand.Next() % 50), 0, 12)), visualSlot: ESlotType.E_COMP_TURRET);
                        else
                            __instance.MyStats.AddShipComponent(PLShipComponent.CreateShipComponentFromHash((int)PLShipComponent.createHashFromInfo((int)ESlotType.E_COMP_TURRET, TurretModManager.Instance.GetTurretIDFromName("Sylvassi Turret"), __instance.GetChaosBoost(deterministicRand.Next() % 50), 0, 12)), visualSlot: ESlotType.E_COMP_TURRET);
                    }
                    if (num >= 0.5 && (double)(float)PLServer.Instance.ChaosLevel > 2.0)
                    {
                        if (deterministicRand.Next() % 3 == 0)
                        {
                            __instance.MyStats.RemoveShipComponent(__instance.MyStats.GetShipComponent<PLShieldGenerator>(ESlotType.E_COMP_SHLD));
                            __instance.MyStats.AddShipComponent(PLShipComponent.CreateShipComponentFromHash((int)PLShipComponent.createHashFromInfo((int)ESlotType.E_COMP_SHLD, ShieldModManager.Instance.GetShieldIDFromName("Reflector Shield Generator"), __instance.GetChaosBoost(deterministicRand.Next() % 50), 0, 12)), visualSlot: ESlotType.E_COMP_SHLD);
                        }
                    }
                }
            }
        }

        [HarmonyPatch(typeof(PLOldWarsShip_Sylvassi), "SetupShipStats")]
        internal class SwordshipInfoMod
        {
            private static void Postfix(PLOldWarsShip_Sylvassi __instance, bool previewStats, bool startingPlayerShip)
            {
                if (__instance == null || __instance.MyStats == null)
                    return;
                if (!__instance.ShouldCreateDefaultComponents || !(PhotonNetwork.isMasterClient | previewStats))
                    return;
                if (startingPlayerShip)
                    return;
                PLRand deterministicRand = PLShipInfoBase.GetShipDeterministicRand(__instance.PersistantShipInfo);
                __instance.MyStats.RemoveShipComponent((PLShipComponent)__instance.MyStats.GetShipComponent<PLHullPlating>(ESlotType.E_COMP_HULLPLATING));
                __instance.MyStats.AddShipComponent(PLShipComponent.CreateShipComponentFromHash((int)PLShipComponent.createHashFromInfo((int)ESlotType.E_COMP_HULLPLATING, (int)EHullPlatingType.E_HULLPLATING_CCGE, __instance.GetChaosBoost(deterministicRand.Next() % 50), 0, 12)), visualSlot: ESlotType.E_COMP_HULLPLATING);
            }
        }

        [HarmonyPatch(typeof(PLFluffyShipInfo), "SetupShipStats")]
        internal class FluffyOneInfoMod
        {
            private static void Postfix(PLFluffyShipInfo __instance, bool previewStats, bool startingPlayerShip)
            {
                if (__instance == null || __instance.MyStats == null)
                    return;
                if (!__instance.ShouldCreateDefaultComponents || !(PhotonNetwork.isMasterClient | previewStats))
                    return;
                if (startingPlayerShip)
                    return;
                PLRand deterministicRand = PLShipInfoBase.GetShipDeterministicRand(__instance.PersistantShipInfo);
                __instance.MyStats.RemoveShipComponent((PLShipComponent)__instance.MyStats.GetShipComponent<PLHullPlating>(ESlotType.E_COMP_HULLPLATING));
                __instance.MyStats.AddShipComponent(PLShipComponent.CreateShipComponentFromHash((int)PLShipComponent.createHashFromInfo((int)ESlotType.E_COMP_HULLPLATING, (int)EHullPlatingType.E_HULLPLATING_CCGE, __instance.GetChaosBoost(deterministicRand.Next() % 50), 0, 12)), visualSlot: ESlotType.E_COMP_HULLPLATING);
                if ((double)(float)PLServer.Instance.ChaosLevel > 1.0)
                {
                    float num = Mathf.Clamp01((float)((double)deterministicRand.NextFloat() + (double)(float)PLServer.Instance.ChaosLevel * 0.01999999731779099));
                    if (num >= 0.85)
                    {
                        __instance.MyStats.RemoveShipComponent(__instance.MyStats.GetComponentsOfType(ESlotType.E_COMP_TURRET)[deterministicRand.Next() % 2]);
                        bool flag = (double)(float)PLServer.Instance.ChaosLevel > 2.0 && deterministicRand.Next() % 3 == 0;
                        if (flag)
                            __instance.MyStats.AddShipComponent(PLShipComponent.CreateShipComponentFromHash((int)PLShipComponent.createHashFromInfo((int)ESlotType.E_COMP_TURRET, TurretModManager.Instance.GetTurretIDFromName("Particle Lance"), __instance.GetChaosBoost(deterministicRand.Next() % 50), 0, 12)), visualSlot: ESlotType.E_COMP_TURRET);
                        else
                            __instance.MyStats.AddShipComponent(PLShipComponent.CreateShipComponentFromHash((int)PLShipComponent.createHashFromInfo((int)ESlotType.E_COMP_TURRET, TurretModManager.Instance.GetTurretIDFromName("Sylvassi Turret"), __instance.GetChaosBoost(deterministicRand.Next() % 50), 0, 12)), visualSlot: ESlotType.E_COMP_TURRET);
                    }
                }
            }
        }

        [HarmonyPatch(typeof(PLFluffyShipInfo2), "SetupShipStats")]
        internal class FluffyTwoInfoMod
        {
            private static void Postfix(PLFluffyShipInfo2 __instance, bool previewStats, bool startingPlayerShip)
            {
                if (__instance == null || __instance.MyStats == null)
                    return;
                if (!__instance.ShouldCreateDefaultComponents || !(PhotonNetwork.isMasterClient | previewStats))
                    return;
                if (startingPlayerShip)
                    return;
                PLRand deterministicRand = PLShipInfoBase.GetShipDeterministicRand(__instance.PersistantShipInfo);
                __instance.MyStats.RemoveShipComponent((PLShipComponent)__instance.MyStats.GetShipComponent<PLHullPlating>(ESlotType.E_COMP_HULLPLATING));
                __instance.MyStats.AddShipComponent(PLShipComponent.CreateShipComponentFromHash((int)PLShipComponent.createHashFromInfo((int)ESlotType.E_COMP_HULLPLATING, (int)EHullPlatingType.E_HULLPLATING_CCGE, __instance.GetChaosBoost(deterministicRand.Next() % 50), 0, 12)), visualSlot: ESlotType.E_COMP_HULLPLATING);
            }
        }

        [HarmonyPatch(typeof(PLIntrepidCommanderInfo), "SetupShipStats")]
        internal class IntrepidCommanderInfoMod
        {
            private static void Postfix(PLIntrepidCommanderInfo __instance, bool previewStats, bool startingPlayerShip)
            {
                if (__instance == null || __instance.MyStats == null)
                    return;
                if (!__instance.ShouldCreateDefaultComponents || !(PhotonNetwork.isMasterClient | previewStats))
                    return;
                if (startingPlayerShip)
                    return;
                PLRand deterministicRand = PLShipInfoBase.GetShipDeterministicRand(__instance.PersistantShipInfo);
                __instance.MyStats.RemoveShipComponent((PLShipComponent)__instance.MyStats.GetShipComponent<PLHullPlating>(ESlotType.E_COMP_HULLPLATING));
                __instance.MyStats.AddShipComponent(PLShipComponent.CreateShipComponentFromHash((int)PLShipComponent.createHashFromInfo((int)ESlotType.E_COMP_HULLPLATING, (int)EHullPlatingType.E_HULLPLATING_CCGE, 5 + __instance.GetChaosBoost(deterministicRand.Next() % 50), 0, 12)), visualSlot: ESlotType.E_COMP_HULLPLATING);
            }
        }

        [HarmonyPatch(typeof(PLAlchemistShipInfo), "SetupShipStats")]
        internal class AlchemistInfoMod
        {
            private static void Postfix(PLAlchemistShipInfo __instance, bool previewStats, bool startingPlayerShip)
            {
                if (__instance == null || __instance.MyStats == null)
                    return;
                if (!__instance.ShouldCreateDefaultComponents || !(PhotonNetwork.isMasterClient | previewStats))
                    return;
                if (startingPlayerShip)
                    return;
                PLRand deterministicRand = PLShipInfoBase.GetShipDeterministicRand(__instance.PersistantShipInfo);
                __instance.MyStats.RemoveShipComponent((PLShipComponent)__instance.MyStats.GetShipComponent<PLHullPlating>(ESlotType.E_COMP_HULLPLATING));
                __instance.MyStats.AddShipComponent(PLShipComponent.CreateShipComponentFromHash((int)PLShipComponent.createHashFromInfo((int)ESlotType.E_COMP_HULLPLATING, (int)EHullPlatingType.E_HULLPLATING_CCGE, 4 + __instance.GetChaosBoost(deterministicRand.Next() % 50), 0, 12)), visualSlot: ESlotType.E_COMP_HULLPLATING);
            }
        }

        [HarmonyPatch(typeof(PLBeaconInfo), "SetupShipStats")]
        internal class NoBeaconScrap
        {
            private static void Postfix(PLBeaconInfo __instance, bool previewStats, bool startingPlayerShip)
            {
                __instance.DropScrap = false;
            }
        }
    }
}
