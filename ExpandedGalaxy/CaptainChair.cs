using HarmonyLib;
using PulsarModLoader;
using PulsarModLoader.Content.Components.CaptainsChair;
using System.Collections.Generic;

namespace ExpandedGalaxy
{
    internal class CaptainChair
    {
        public class RelicChair : CaptainsChairMod
        {
            public override string Name => "Seat of the Surveyor";

            public override string Description => "This chair was aboard the ship captained by Jana Cron - the first human vessel to enter this galaxy. It has its own integrated piloting system which can control a small drone. Drone is repaired automatically when visiting a repair depot.";

            public override int MarketPrice => 30000;

            public override string GetStatLineRight(PLShipComponent InComp)
            {
                PLCaptainsChair chair = InComp as PLCaptainsChair;
                if (!chair.IsEquipped)
                    return "INACTIVE\n";
                string value;
                switch (chair.SubTypeData)
                {
                    case 0:
                        value = "READY";
                        break;
                    case -1:
                        value = "DESTROYED";
                        break;
                    default:
                        value = "ACTIVE";
                        break;
                }
                return value + "\n";
            }
            public override string GetStatLineLeft(PLShipComponent InComp) => "Drone Status\n";

            public override void OnWarp(PLShipComponent InComp)
            {
                if (!PhotonNetwork.isMasterClient)
                    return;
                PLCaptainsChair chair = InComp as PLCaptainsChair;
                if (chair.SubType != CaptainsChairModManager.Instance.GetCaptainsChairIDFromName("Seat of the Surveyor"))
                    return;
                if (chair.SubTypeData > 0)
                {
                    if (PLEncounterManager.Instance.GetShipFromID(chair.SubTypeData) != null)
                    {
                        PLEncounterManager.Instance.GetShipFromID(chair.SubTypeData).DestroySelf(null);
                    }
                    if (Puppet.shipDatas.ContainsKey(chair.SubTypeData))
                        Puppet.shipDatas.Remove(chair.SubTypeData);
                    chair.SubTypeData = -1;

                }
            }

            public override void Tick(PLShipComponent InComp)
            {
                base.Tick(InComp);
                if (InComp.SubTypeData == 0)
                {
                    if (InComp.IsEquipped && PLNetworkManager.Instance.LocalPlayer.IsSittingInCaptainsChair() && PLInput.Instance.GetButtonUp(PLInputBase.EInputActionName.pilot_ability))
                    {
                        ModMessage.SendRPC("sugarbuzz1.ExpandedGalaxy", "ExpandedGalaxy.ControlDrone", PhotonTargets.MasterClient, new object[1]
                        {
                            (object) InComp.ShipStats.Ship.ShipID,
                        });
                    }
                }
                else if (InComp.SubTypeData > 0)
                {
                    if (PhotonNetwork.isMasterClient)
                    {
                        if (PLEncounterManager.Instance.GetShipFromID(InComp.SubTypeData) != null)
                        {
                            if (PLEncounterManager.Instance.GetShipFromID(InComp.SubTypeData).HasBeenDestroyed)
                            {
                                if (Puppet.shipDatas.ContainsKey(InComp.SubTypeData))
                                    Puppet.shipDatas.Remove(InComp.SubTypeData);
                                InComp.SubTypeData = -1;
                                ModMessage.SendRPC("sugarbuzz1.ExpandedGalaxy", "ExpandedGalaxy.UpdateSubTypeData", PhotonTargets.Others, new object[3]
                                {
                                (object) InComp.ShipStats.Ship.ShipID,
                                (object) InComp.NetID,
                                (object) InComp.SubTypeData,
                                });
                            }
                            if (!InComp.IsEquipped)
                            {
                                PLEncounterManager.Instance.GetShipFromID(InComp.SubTypeData).DestroySelf(null);
                                if (Puppet.shipDatas.ContainsKey(InComp.SubTypeData))
                                    Puppet.shipDatas.Remove(InComp.SubTypeData);
                                InComp.SubTypeData = -1;
                                ModMessage.SendRPC("sugarbuzz1.ExpandedGalaxy", "ExpandedGalaxy.UpdateSubTypeData", PhotonTargets.Others, new object[3]
                                {
                                (object) InComp.ShipStats.Ship.ShipID,
                                (object) InComp.NetID,
                                (object) InComp.SubTypeData,
                                });
                            }
                        }
                        else
                        {
                            if (Puppet.shipDatas.ContainsKey(InComp.SubTypeData))
                                Puppet.shipDatas.Remove(InComp.SubTypeData);
                            InComp.SubTypeData = -1;
                            ModMessage.SendRPC("sugarbuzz1.ExpandedGalaxy", "ExpandedGalaxy.UpdateSubTypeData", PhotonTargets.Others, new object[3]
                            {
                            (object) InComp.ShipStats.Ship.ShipID,
                            (object) InComp.NetID,
                            (object) InComp.SubTypeData,
                            });
                        }
                    }
                    if (InComp.IsEquipped && PLNetworkManager.Instance.LocalPlayer.IsSittingInCaptainsChair() && PLInput.Instance.GetButtonUp(PLInputBase.EInputActionName.pilot_ability))
                    {
                        ModMessage.SendRPC("sugarbuzz1.ExpandedGalaxy", "ExpandedGalaxy.ControlDrone", PhotonTargets.MasterClient, new object[1]
                        {
                                (object) InComp.ShipStats.Ship.ShipID,
                        });
                    }
                }
                else
                {
                    if (PhotonNetwork.isMasterClient)
                    {
                        if (InComp.IsEquipped)
                        {
                            foreach (PLSensorObject allSensorObject in InComp.ShipStats.Ship.GetAllSensorObjects())
                            {
                                if (allSensorObject != null)
                                {
                                    PLRepairDepot depot = allSensorObject.GetComponentInChildren<PLRepairDepot>();
                                    if (depot != null && InComp.SubTypeData == -1 && !InComp.ShipStats.Ship.InWarp)
                                    {
                                        InComp.SubTypeData = 0;
                                        ModMessage.SendRPC("sugarbuzz1.ExpandedGalaxy", "ExpandedGalaxy.UpdateSubTypeData", PhotonTargets.Others, new object[3]
                                        {
                                        (object) InComp.ShipStats.Ship.ShipID,
                                        (object) InComp.NetID,
                                        (object) InComp.SubTypeData,
                                        });
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        public class ControlDrone : ModMessage
        {
            public override void HandleRPC(object[] arguments, PhotonMessageInfo sender)
            {
                PLCaptainsChair chair = PLEncounterManager.Instance.GetShipFromID((int)arguments[0]).MyStats.GetShipComponent<PLCaptainsChair>(ESlotType.E_COMP_CAPTAINS_CHAIR);
                if (chair.SubTypeData == 0)
                {
                    PLPersistantShipInfo droneInfo = new PLPersistantShipInfo(EShipType.E_WDDRONE1, PLNetworkManager.Instance.LocalPlayer.StartingShip.FactionID, PLServer.GetCurrentSector(), isFlagged: false)
                    {
                        ShipName = "Surveyor Drone",
                        HullPercent = 1f,
                        ShldPercent = 1f,
                    };
                    droneInfo.CompOverrides.AddRange((IEnumerable<ComponentOverrideData>)CaptainChair.DroneData(PLEncounterManager.Instance.GetShipFromID((int)arguments[0])));
                    PLServer.Instance.AllPSIs.Add(droneInfo);

                    PLShipInfoBase info = PLEncounterManager.Instance.GetCurrentPersistantEncounterInstance().SpawnEnemyShip(droneInfo.Type, droneInfo, spawnPos: PLEncounterManager.Instance.GetShipFromID((int)arguments[0]).transform.position + PLEncounterManager.Instance.GetShipFromID((int)arguments[0]).transform.forward * 200f);
                    info.DropScrap = false;
                    info.CreditsLeftBehind = 0;
                    info.NoRepLossOnKilled = true;
                    if ((UnityEngine.Object)info.PilotingSystem == (UnityEngine.Object)null)
                    {
                        info.PilotingSystem = info.gameObject.AddComponent<PLPilotingSystem>();
                        info.PilotingSystem.MyShipInfo = info;
                    }
                    if ((UnityEngine.Object)info.PilotingHUD == (UnityEngine.Object)null)
                    {
                        info.PilotingHUD = info.gameObject.AddComponent<PLPilotingHUD>();
                        info.PilotingHUD.MyShipInfo = info;
                    }
                    info.OrbitCameraMaxDistance = 40f;
                    info.OrbitCameraMinDistance = 7f;
                    info.photonView.RPC("Captain_NameShip", PhotonTargets.All, (object)"Surveyer Drone");
                    chair.SubTypeData = (short)info.ShipID;
                    ModMessage.SendRPC("sugarbuzz1.ExpandedGalaxy", "ExpandedGalaxy.UpdateSubTypeData", PhotonTargets.Others, new object[3]
                    {
                        (object) chair.ShipStats.Ship.ShipID,
                        (object) chair.NetID,
                        (object) chair.SubTypeData,
                    });
                    Puppet.shipDatas.Add(info.ShipID, (int)arguments[0]);
                    info.photonView.RPC("NewShipController", PhotonTargets.All, -1);
                    if (sender.sender.IsMasterClient)
                    {
                        info.photonView.RPC("NewShipController", PhotonTargets.All, PLNetworkManager.Instance.LocalPlayer.GetPlayerID());
                    }
                    else
                    {
                        ModMessage.SendRPC("sugarbuzz1.ExpandedGalaxy", "ExpandedGalaxy.PilotDrone", sender.sender, new object[1]
                        {
                            info.ShipID
                        });
                    }
                }
                else if (chair.SubTypeData > 0)
                {
                    if (PLEncounterManager.Instance.GetShipFromID(chair.SubTypeData) != null)
                    {
                        PLShipInfoBase info = PLEncounterManager.Instance.GetShipFromID(chair.SubTypeData);
                        if ((UnityEngine.Object)info.PilotingSystem == (UnityEngine.Object)null)
                        {
                            info.PilotingSystem = info.gameObject.AddComponent<PLPilotingSystem>();
                            info.PilotingSystem.MyShipInfo = info;
                        }
                        if ((UnityEngine.Object)info.PilotingHUD == (UnityEngine.Object)null)
                        {
                            info.PilotingHUD = info.gameObject.AddComponent<PLPilotingHUD>();
                            info.PilotingHUD.MyShipInfo = info;
                        }
                        info.OrbitCameraMaxDistance = 40f;
                        info.OrbitCameraMinDistance = 7f;
                        info.photonView.RPC("NewShipController", PhotonTargets.All, -1);
                        if (sender.sender.IsMasterClient)
                        {
                            info.photonView.RPC("NewShipController", PhotonTargets.All, PLNetworkManager.Instance.LocalPlayer.GetPlayerID());
                        }
                        else
                        {
                            ModMessage.SendRPC("sugarbuzz1.ExpandedGalaxy", "ExpandedGalaxy.PilotDrone", sender.sender, new object[1]
                            {
                                info.ShipID
                            });
                        }
                    }
                }
            }
        }

        private static List<ComponentOverrideData> DroneData(PLShipInfoBase ship)
        {
            List<ComponentOverrideData> droneParts = new List<ComponentOverrideData>();
            int compLevel = 0;
            if (ship.MyShieldGenerator != null)
                compLevel = ship.MyShieldGenerator.Level / 2;
            droneParts.Add
                    (
                        new ComponentOverrideData()
                        {
                            CompType = (int)ESlotType.E_COMP_SHLD,
                            CompSubType = (int)EShieldGeneratorType.E_SG_HEAVY_TACTICAL_HOLOSCREEN,
                            ReplaceExistingComp = true,
                            CompLevel = compLevel,
                            IsCargo = false,
                            CompTypeToReplace = (int)ESlotType.E_COMP_SHLD,
                            SlotNumberToReplace = 0
                        }
                    );
            compLevel = 0;
            if (ship.MyReactor != null)
                compLevel = ship.MyReactor.Level / 2;
            droneParts.Add
                (
                new ComponentOverrideData()
                {
                    CompType = (int)ESlotType.E_COMP_REACTOR,
                    CompSubType = (int)EReactorType.E_REAC_CU_FUSION_REACTOR_MK3,
                    ReplaceExistingComp = true,
                    CompLevel = 2 + compLevel,
                    IsCargo = false,
                    CompTypeToReplace = (int)ESlotType.E_COMP_REACTOR,
                    SlotNumberToReplace = 0
                }
                );
            compLevel = 0;
            if (ship.MyHull != null)
                compLevel = ship.MyHull.Level / 2;
            droneParts.Add
                (
                new ComponentOverrideData()
                {
                    CompType = (int)ESlotType.E_COMP_HULL,
                    CompSubType = (int)EHullType.E_CCG_LIGHT_HULL,
                    ReplaceExistingComp = true,
                    CompLevel = compLevel,
                    IsCargo = false,
                    CompTypeToReplace = (int)ESlotType.E_COMP_HULL,
                    SlotNumberToReplace = 0
                }
                );
            droneParts.Add
                (
                new ComponentOverrideData()
                {
                    CompType = (int)ESlotType.E_COMP_CPU,
                    CompSubType = (int)ECPUClass.E_CPUTYPE_CYBER_DEF,
                    ReplaceExistingComp = true,
                    CompLevel = 4,
                    IsCargo = false,
                    CompTypeToReplace = (int)ESlotType.E_COMP_CPU,
                    SlotNumberToReplace = 0
                }
                );
            droneParts.Add
                (
                new ComponentOverrideData()
                {
                    CompType = (int)ESlotType.E_COMP_CPU,
                    CompSubType = (int)ECPUClass.E_CPUTYPE_SHIELD_COPROCESSOR,
                    ReplaceExistingComp = true,
                    CompLevel = 0,
                    IsCargo = false,
                    CompTypeToReplace = (int)ESlotType.E_COMP_CPU,
                    SlotNumberToReplace = 1
                }
                );
            droneParts.Add
                (
                new ComponentOverrideData()
                {
                    CompType = (int)ESlotType.E_COMP_CPU,
                    CompSubType = (int)ECPUClass.E_CPUTYPE_SHIELD_COPROCESSOR,
                    ReplaceExistingComp = true,
                    CompLevel = 0,
                    IsCargo = false,
                    CompTypeToReplace = (int)ESlotType.E_COMP_CPU,
                    SlotNumberToReplace = 2
                }
                );
            compLevel = 0;
            if (ship.MyStats.GetShipComponent<PLTurret>(ESlotType.E_COMP_MAINTURRET) != null)
                compLevel = ship.MyStats.GetShipComponent<PLTurret>(ESlotType.E_COMP_MAINTURRET).Level / 2;
            droneParts.Add
                (
                new ComponentOverrideData()
                {
                    CompType = (int)ESlotType.E_COMP_TURRET,
                    CompSubType = (int)ETurretType.LASER,
                    ReplaceExistingComp = true,
                    CompLevel = compLevel,
                    IsCargo = false,
                    CompTypeToReplace = (int)ESlotType.E_COMP_TURRET,
                    SlotNumberToReplace = 0
                }
                );
            droneParts.Add
                (
                new ComponentOverrideData()
                {
                    CompType = (int)ESlotType.E_COMP_TURRET,
                    CompSubType = (int)ETurretType.LASER,
                    ReplaceExistingComp = true,
                    CompLevel = compLevel,
                    IsCargo = false,
                    CompTypeToReplace = (int)ESlotType.E_COMP_TURRET,
                    SlotNumberToReplace = 1
                }
                );

            droneParts.Add
                (
                new ComponentOverrideData()
                {
                    CompType = (int)ESlotType.E_COMP_SENS,
                    CompSubType = (int)0,
                    ReplaceExistingComp = true,
                    CompLevel = 9,
                    IsCargo = false,
                    CompTypeToReplace = (int)ESlotType.E_COMP_SENS,
                    SlotNumberToReplace = 0
                }
                );
            droneParts.Add
                (
                new ComponentOverrideData()
                {
                    CompType = (int)ESlotType.E_COMP_THRUSTER,
                    CompSubType = (int)EThrusterType.E_THRUSTER_PERF,
                    ReplaceExistingComp = true,
                    CompLevel = 2,
                    IsCargo = false,
                    CompTypeToReplace = (int)ESlotType.E_COMP_THRUSTER,
                    SlotNumberToReplace = 0
                }
                );
            droneParts.Add
                (
                new ComponentOverrideData()
                {
                    CompType = (int)ESlotType.E_COMP_THRUSTER,
                    CompSubType = (int)EThrusterType.E_THRUSTER_PERF,
                    ReplaceExistingComp = true,
                    CompLevel = 2,
                    IsCargo = false,
                    CompTypeToReplace = (int)ESlotType.E_COMP_THRUSTER,
                    SlotNumberToReplace = 1
                }
                );
            droneParts.Add
                (
                new ComponentOverrideData()
                {
                    CompType = (int)ESlotType.E_COMP_INERTIA_THRUSTER,
                    CompSubType = (int)EInertiaThrusterType.E_NORMAL,
                    ReplaceExistingComp = true,
                    CompLevel = 2,
                    IsCargo = false,
                    CompTypeToReplace = (int)ESlotType.E_COMP_INERTIA_THRUSTER,
                    SlotNumberToReplace = 0
                }
                );
            droneParts.Add
                (
                new ComponentOverrideData()
                {
                    CompType = (int)ESlotType.E_COMP_MANEUVER_THRUSTER,
                    CompSubType = (int)EManeuverThrusterType.E_NORMAL,
                    ReplaceExistingComp = true,
                    CompLevel = 2,
                    IsCargo = false,
                    CompTypeToReplace = (int)ESlotType.E_COMP_MANEUVER_THRUSTER,
                    SlotNumberToReplace = 0
                }
                );

            return droneParts;
        }

        [HarmonyPatch(typeof(PLShipInfo), "AttemptToSitInCaptainsChair")]
        internal class DontKickFromChairWhilePiloting
        {
            private static bool Prefix(PLShipInfo __instance, int playerID)
            {
                PLPlayer playerFromPlayerId = PLServer.Instance.GetPlayerFromPlayerID(playerID);
                if (playerID == -1 && !Systems.IsPlayerPiloting(__instance.CaptainsChairPlayerID))
                {
                    __instance.CaptainsChairPlayerID = -1;
                }
                else
                {
                    if (__instance.CaptainsChairPlayerID != -1 && (!((UnityEngine.Object)playerFromPlayerId != (UnityEngine.Object)null) || playerFromPlayerId.GetClassID() != 0))
                        return false;
                    if (!Systems.IsPlayerPiloting(__instance.CaptainsChairPlayerID))
                        __instance.CaptainsChairPlayerID = playerID;
                }
                return false;
            }
        }
    }
}
