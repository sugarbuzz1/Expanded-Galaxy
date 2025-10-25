using HarmonyLib;
using UnityEngine;

namespace ExpandedGalaxy
{
    internal class DistressSignal
    {
        public class MiningDroneSignal : PLDistressSignal
        {
            private float LastVirusTime = float.MinValue;
            public MiningDroneSignal(int inType, int inLevel = 0) : base(EDistressSignalType.UNION, inLevel)
            {
                this.SubType = 4;
                this.Name = "368.6 MHz";
                this.Desc = "A distress signal recovered from the wreckage of a mining drone.\n\nPerhaps using it can give insight on where they originated from...";
                this.CanBeDroppedOnShipDeath = false;
                this.Level = inLevel;
            }

            private void UpdateDescription()
            {
                if (this.IsEquipped)
                    this.Desc = "Distress Signal: Mining Drone";
                else
                    this.Desc = "A distress signal recovered from the wreckage of a mining drone.\nPerhaps using it can give insight on where they originated from...";
            }

            public override void Tick()
            {
                base.Tick();
                this.UpdateDescription();
                if (!(this.IsEquipped && this.ShipStats != null && this.ShipStats.Ship != null && this.ShipStats.Ship.IsDrone))
                    return;
                if (this.ShipStats.Ship.MyShieldGenerator != null && this.ShipStats.Ship.MyShieldGenerator.CanBeDroppedOnShipDeath)
                    this.ShipStats.Ship.MyShieldGenerator.CanBeDroppedOnShipDeath = false;
                this.ShipStats.Ship.ClearModifiers();
                if (this.Level == 0)
                {
                    this.ShipStats.Ship.ShipNameValue = "Mining Drone";
                    this.ShipStats.Ship.GX_ID = "Mining Drone";
                    if (PhotonNetwork.isMasterClient && !PLEncounterManager.Instance.PlayerShip.InWarp && Relic.MiningDroneQuest.GXData < 1)
                    {
                        Relic.MiningDroneQuest.GXData = 1;
                        PLServer.Instance.photonView.RPC("AddCrewWarning", PhotonTargets.All, new object[4]
                        {
                            "New GX Entry Added!",
                            Color.white,
                            0,
                            "GX"
                        });


                    }
                }
                else if (this.Level < 4)
                {
                    this.ShipStats.Ship.ShipNameValue = "Escort Drone";
                    this.ShipStats.Ship.GX_ID = "Escort Drone";
                    if (PhotonNetwork.isMasterClient && !PLEncounterManager.Instance.PlayerShip.InWarp && Relic.MiningDroneQuest.GXData < 1)
                    {
                        Relic.MiningDroneQuest.GXData = 1;
                        PLServer.Instance.photonView.RPC("AddCrewWarning", PhotonTargets.All, new object[4]
                        {
                            "New GX Entry Added!",
                            Color.white,
                            0,
                            "GX"
                        });
                    }
                }
                else
                {
                    this.ShipStats.Ship.ShipNameValue = "Guardian Drone";
                    this.ShipStats.Ship.GX_ID = "Guardian Drone";
                    if (PhotonNetwork.isMasterClient && !PLEncounterManager.Instance.PlayerShip.InWarp && Relic.MiningDroneQuest.GXData < 2)
                    {
                        Relic.MiningDroneQuest.GXData = 2;
                        PLServer.Instance.photonView.RPC("AddCrewWarning", PhotonTargets.All, new object[4]
                        {
                            "New GX Entry Added!",
                            Color.white,
                            0,
                            "GX"
                        });
                        
                    }
                }
                Traverse traverse = Traverse.Create(this.ShipStats.Ship);
                if (Relic.MiningDroneQuest.dronesActive)
                {
                    traverse.Field("CanFireProbes").SetValue(true);
                    this.ShipStats.Ship.SetAbandoned(false);
                    if (PhotonNetwork.isMasterClient && this.ShipStats.Ship.LastTookDamageTime() == float.MinValue && !this.ShipStats.Ship.PersistantShipInfo.ForcedHostile)
                    {
                        this.ShipStats.Ship.AlertLevel = 0;
                        this.ShipStats.Ship.Captain_SetTargetShip(-1);
                    }
                    if (PhotonNetwork.isMasterClient && this.ShipStats.Ship.AlertLevel > 0 && PLEncounterManager.Instance.GetCPEI() != null)
                    {
                        this.ShipStats.Ship.PersistantShipInfo.ForcedHostile = true;
                        foreach (PLShipInfoBase plShipInfoBase in PLEncounterManager.Instance.GetCPEI().MyCreatedShipInfos)
                        {
                            if (!plShipInfoBase.PersistantShipInfo.ForcedHostile)
                            {
                                if (plShipInfoBase.ShipTypeID == EShipType.E_WDDRONE2)
                                {
                                    foreach (PLShipComponent component in plShipInfoBase.MyStats.GetComponentsOfType(ESlotType.E_COMP_DISTRESS_SIGNAL))
                                    {
                                        if (component is MiningDroneSignal)
                                        {
                                            plShipInfoBase.PersistantShipInfo.ForcedHostile = true;
                                            if (this.ShipStats.Ship.TargetShip != null)
                                                plShipInfoBase.Captain_SetTargetShip(this.ShipStats.Ship.TargetShip.ShipID);
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    if (!(this.Level > 0 && PhotonNetwork.isMasterClient && this.ShipStats.Ship.AlertLevel > 0 && this.ShipStats.Ship.TargetShip != null))
                        return;
                    double cooldown;
                    EVirusType virusType;
                    if (this.Level < 4)
                    {
                        switch (this.Level)
                        {
                            case 1:
                                virusType = EVirusType.WARP_DISABLE;
                                cooldown = 120.0;
                                break;
                            case 2:
                                virusType = EVirusType.ARMOR_FLAW;
                                cooldown = 60.0;
                                break;
                            default:
                                virusType = EVirusType.PHALANX;
                                cooldown = 150.0;
                                break;
                        }
                    }
                    else
                    {
                        switch (UnityEngine.Random.Range(0, 3))
                        {
                            case 1:
                                virusType = EVirusType.SYBER_SHIELD;
                                cooldown = 100.0;
                                break;
                            case 2:
                                virusType = EVirusType.ARMOR_FLAW;
                                cooldown = 60.0;
                                break;
                            default:
                                virusType = EVirusType.PHALANX;
                                cooldown = 150.0;
                                break;
                        }
                    }
                    if (PLEncounterManager.Instance.PlayerShip != null && !this.ShipStats.Ship.SendQueueContainsVirusOfType(virusType))
                    {
                        if ((double)(Time.time - LastVirusTime) > cooldown)
                        {
                            LastVirusTime = Time.time;
                            PLServer.Instance.photonView.RPC("AddToSendQueue", PhotonTargets.All, new object[4]
                            {
                                this.ShipStats.Ship.ShipID,
                                this.ShipStats.Ship.VirusSendQueueCounter++,
                                (int)virusType,
                                PLServer.Instance.GetEstimatedServerMs()
                            });
                        }
                    }
                }
                else
                {
                    traverse.Field("CanFireProbes").SetValue(false);
                    this.ShipStats.Ship.SetAbandoned(true);
                    this.ShipStats.Ship.AlertLevel = 0;
                    this.ShipStats.Ship.Captain_SetTargetShip(-1);
                    if (PhotonNetwork.isMasterClient)
                        this.ShipStats.Ship.PersistantShipInfo.ForcedHostile = false;
                }
            }

            public override void FinalLateAddStats(PLShipStats inStats)
            {
                base.FinalLateAddStats(inStats);
                if (!(this.IsEquipped && this.ShipStats != null && this.ShipStats.Ship != null && this.ShipStats.Ship.IsDrone))
                    return;
                if (!Relic.MiningDroneQuest.dronesActive)
                {
                    this.ShipStats.ShieldsCurrent = 0f;
                    this.ShipStats.ShieldsMax = 0f;
                    this.ShipStats.ShieldsChargeRate = 0f;
                    this.ShipStats.ThrustOutputMax = 0f;
                    this.ShipStats.ThrustOutputCurrent = 0f;
                    this.ShipStats.ManeuverThrustOutputMax = 0f;
                    this.ShipStats.ManeuverThrustOutputCurrent = 0f;
                    this.ShipStats.InertiaThrustOutputMax = 0f;
                    this.ShipStats.InertiaThrustOutputCurrent = 0f;
                }
            }
        }
        public static PLDistressSignal CreateDistressSignal(int Subtype, int level)
        {

            if (Subtype == 4)
            {
                return new MiningDroneSignal(4, level);
            }
            else if (Subtype == 5)
                return new Missions.MissionShipFlagComp(5, level);
            else if (Subtype == 6)
                return new Missions.MissionAddOneCPUSlotComp(6, level);
            else if (Subtype == 7)
                return new Missions.MissionNoExtractorFlag(7, level);
            return new PLDistressSignal((EDistressSignalType)Subtype, level);
        }

        [HarmonyPatch(typeof(PLDistressSignal), "CreateDistressSignalFromHash")]
        internal class DistressSignalHashFix
        {
            private static bool Prefix(int inSubType, int inLevel, int inSubTypeData, ref PLShipComponent __result)
            {
                __result = (PLShipComponent)CreateDistressSignal(inSubType, inLevel);
                return false;
            }
        }
    }
}
