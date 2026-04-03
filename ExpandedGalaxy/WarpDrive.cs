using CodeStage.AntiCheat.ObscuredTypes;
using HarmonyLib;
using PulsarModLoader;
using PulsarModLoader.Content.Components.WarpDrive;
using PulsarModLoader.Utilities;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace ExpandedGalaxy
{
    internal class WarpDrive
    {
        [HarmonyPatch(typeof(PLWarpDrive), MethodType.Constructor, new Type[3] {typeof(EWarpDriveType), typeof(int), typeof(short) })]
        internal class WarpDriveProgramChargeRebalance
        {
            private static void Postfix(PLWarpDrive __instance)
            {
                switch(__instance.SubType)
                {
                    case (int)EWarpDriveType.E_WARPDR_CU_LONGRANGE_JUMP_MODULE:
                        __instance.NumberOfChargingNodes = 3;
                        break;
                    case (int)EWarpDriveType.E_WARPDR_GTC_SNAPPY_CRICKET:
                    case (int)EWarpDriveType.E_WARPDR_CU_STANDARD_JUMP_MODULE:
                        __instance.NumberOfChargingNodes = 4;
                        break;
                    case (int)EWarpDriveType.E_WARPDR_WDMILITARYJUMP:
                        __instance.NumberOfChargingNodes = 5;
                        break;
                    case (int)EWarpDriveType.E_WARPDR_EXPLORERS:
                        __instance.NumberOfChargingNodes = 3;
                        break;
                }
            }
        }

        [HarmonyPatch(typeof(PLWarpDrive), "Update")]
        internal class WarpDriveUpdatePatch
        {
            private static void Postfix(PLWarpDrive __instance)
            {
                if (__instance.SysInstConduit != -1 && __instance.IsPowerActive)
                {
                    if (__instance.ShipStats != null && __instance.ShipStats.Ship.EngineeringSystem != null)
                        __instance.RequestPowerUsage_Percent = __instance.ShipStats.Ship.EngineeringSystem.GetHealthRatio();
                }
            }
        }

        [HarmonyPatch(typeof(PLWarpDrive), "ChargePrograms")]
        internal class ChargePrograms
        {
            private static bool Prefix(PLWarpDrive __instance, ref bool chargeToFull, int overrideChargeCount)
            {
                if (!__instance.ShipStats.Ship.GetIsPlayerShip() && overrideChargeCount == -1) // Full charge programs
                    chargeToFull = true;
                if ((overrideChargeCount != -1 || chargeToFull) && (int)PLServer.Instance.JumpsNeededToResearchTalent > 0) // Capacitor doesn't advance research
                    ++PLServer.Instance.JumpsNeededToResearchTalent;
                return true;
            }
        }

        private class DestroyedDrive : WarpDriveMod
        {
            public override string Name => "Broken Warp Drive";

            public override string Description => "A warp drive hailing from the Old Wars era that has seemingly been modified to have intergalactic warp capabilities using a method long lost to the sands of time. Unfortunatly, it is damaged beyond repair.";

            public override bool CanBeDroppedOnShipDeath => false;

            public override float ChargeSpeed => 0f;

            public override float WarpRange => 0.01f;

            public override float EnergySignature => 2f;

            public override int NumberOfChargesPerFuel => 1;

            public override float MaxPowerUsage_Watts => 8550f;

            public override void Tick(PLShipComponent InComp)
            {
                PLWarpDrive pLWarpDrive = InComp as PLWarpDrive;
                if (!pLWarpDrive.ImportantItem)
                    pLWarpDrive.ImportantItem = true;
                if (pLWarpDrive.ShipStats == null || pLWarpDrive.ShipStats.Ship == null || (PLShipInfo)pLWarpDrive.ShipStats.Ship == null || pLWarpDrive.ShipStats.Ship.GetIsPlayerShip())
                    pLWarpDrive.FlagForSelfDestruction();

                if (!((PLShipInfo)pLWarpDrive.ShipStats.Ship).IsGodModeActive)
                    ((PLShipInfo)pLWarpDrive.ShipStats.Ship).IsGodModeActive = true;

                if (PhotonNetwork.isMasterClient && pLWarpDrive.SubTypeData == 0)
                {
                    pLWarpDrive.ShipStats.Ship.SetAbandoned(true);
                    pLWarpDrive.ShipStats.Ship.AuxConfig = 0;
                    pLWarpDrive.ShipStats.Ship.EngineeringSystem.Health = (ObscuredFloat)0f;
                    pLWarpDrive.ShipStats.Ship.WeaponsSystem.Health = (ObscuredFloat)0f;
                    pLWarpDrive.ShipStats.Ship.ComputerSystem.Health = (ObscuredFloat)0f;
                    pLWarpDrive.ShipStats.Ship.LifeSupportSystem.Health = (ObscuredFloat)0f;
                    pLWarpDrive.ShipStats.HullCurrent = pLWarpDrive.ShipStats.HullMax * 0.13f;
                    ((PLShipInfo)pLWarpDrive.ShipStats.Ship).CrewControlEnabled = false;
                    pLWarpDrive.ShipStats.Ship.MyStats.OxygenLevel = 0f;
                    pLWarpDrive.ShipStats.Ship.MyStats.RemoveShipComponentByNetID(pLWarpDrive.ShipStats.Ship.MyReactor.NetID);
                    ((PLShipInfo)pLWarpDrive.ShipStats.Ship).StartupSwitchBoard.SetStatus(0, false);
                    ++pLWarpDrive.SubTypeData;
                }

                if (PhotonNetwork.isMasterClient && Relic.ReflectedRift.inRift)
                {
                    if (!Relic.ReflectedRift.GetRiftData(6) && pLWarpDrive.ShipStats != null && pLWarpDrive.ShipStats.Ship != null && ((PLShipInfo)pLWarpDrive.ShipStats.Ship).StartupSwitchBoard.GetLateStatus(2) && ((PLShipInfo)pLWarpDrive.ShipStats.Ship).CrewControlEnabled)
                    {
                        Relic.ReflectedRift.SetRiftData(6, true);
                        CrewLogData data = new CrewLogData
                        {
                            optionalSectorID = -1,
                            timeStamp = (float)PLServer.Instance.Playtime,
                            optionalColor = new Color(UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f)),
                            Text = string.Empty,
                            specialData = 2
                        };
                        CrewLogManager.Instance.AddLog(data);
                        List<object> sendArgumentList = new List<object>();
                        int logCount = CrewLogManager.Instance.GetLogs().Count;
                        sendArgumentList.Add(logCount);
                        foreach (CrewLogData sendLogData in CrewLogManager.Instance.GetLogs())
                        {
                            sendArgumentList.Add(sendLogData.Text);
                            sendArgumentList.Add(sendLogData.timeStamp);
                            sendArgumentList.Add(sendLogData.optionalSectorID);
                            sendArgumentList.Add(sendLogData.optionalColor.r);
                            sendArgumentList.Add(sendLogData.optionalColor.g);
                            sendArgumentList.Add(sendLogData.optionalColor.b);
                            sendArgumentList.Add(sendLogData.optionalColor.a);
                            sendArgumentList.Add(sendLogData.specialData);
                        }
                        ModMessage.SendRPC("sugarbuzz1.ExpandedGalaxy", "ExpandedGalaxy.ServerSendLog", PhotonTargets.Others, sendArgumentList.ToArray());
                        Messaging.Notification("Crew log added", PhotonTargets.All, durationMs: 10000);
                    }
                }
            }
        }
    }
}
