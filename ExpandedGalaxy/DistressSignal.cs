﻿using HarmonyLib;
using PulsarModLoader.Utilities;
using System.Collections.Generic;
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
                if (this.Level == 0)
                {
                    this.ShipStats.Ship.ShipNameValue = "Mining Drone";
                    this.ShipStats.Ship.GX_ID = "Mining Drone";
                    if (PhotonNetwork.isMasterClient && !PLEncounterManager.Instance.PlayerShip.InWarp && Relic.MiningDroneQuest.GXData < 1)
                    {
                        Relic.MiningDroneQuest.GXData = 1;
                        Messaging.Notification("New GX Entries Added!", PhotonTargets.All);
                        PLGlobal.Instance.AllGeneralInfos.Add(new GeneralInfo
                        {
                            Name = "Mining Drone",
                            Name_lower = "mining drone",
                            ID = 29,
                            Desc = "Drone used for extracting minerals from space asteroids.\n\nThey appear to be communicating with some uncharted hub sector within this galaxy. They are equipped with powerful laser technology.\n\nMining drones are not hostile unless provoked.",
                            MyStats = new List<GeneralInfoStat> { new GeneralInfoStat
                {
                    Left = "Type",
                    Right = "Unmanned Drone"
                },
                new GeneralInfoStat
                {
                    Left = "Faction",
                    Right = "Unknown"
                },
                new GeneralInfoStat
                {
                    Left = "Service",
                    Right = "Resource Extraction"
                }
                }
                        });
                        PLGlobal.Instance.AllGeneralInfos.Add(new GeneralInfo
                        {
                            Name = "Escort Drone",
                            Name_lower = "escort drone",
                            ID = 30,
                            Desc = "Drone used to protect mining drone operations.\n\nThey appear to be communicating with some uncharted hub sector within this galaxy. They reinforce other mining and escort drones in distress.\n\nEscort drones are not hostile unless a drone is provoked.",
                            MyStats = new List<GeneralInfoStat> { new GeneralInfoStat
                {
                    Left = "Type",
                    Right = "Unmanned Drone"
                },
                new GeneralInfoStat
                {
                    Left = "Faction",
                    Right = "Unknown"
                },
                new GeneralInfoStat
                {
                    Left = "Service",
                    Right = "Asset Protection"
                }
                }
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
                        Messaging.Notification("New GX Entries Added!", PhotonTargets.All);
                        PLGlobal.Instance.AllGeneralInfos.Add(new GeneralInfo
                        {
                            Name = "Mining Drone",
                            Name_lower = "mining drone",
                            ID = 29,
                            Desc = "Drone used for extracting minerals from space asteroids.\n\nThey appear to be communicating with some uncharted hub sector within this galaxy. They are equipped with powerful laser technology.\n\nMining drones are not hostile unless provoked.",
                            MyStats = new List<GeneralInfoStat> { new GeneralInfoStat
                {
                    Left = "Type",
                    Right = "Unmanned Drone"
                },
                new GeneralInfoStat
                {
                    Left = "Faction",
                    Right = "Unknown"
                },
                new GeneralInfoStat
                {
                    Left = "Service",
                    Right = "Resource Extraction"
                }
                }
                        });
                        PLGlobal.Instance.AllGeneralInfos.Add(new GeneralInfo
                        {
                            Name = "Escort Drone",
                            Name_lower = "escort drone",
                            ID = 30,
                            Desc = "Drone used to protect mining drone operations.\n\nThey appear to be communicating with some uncharted hub sector within this galaxy. They reinforce other mining and escort drones in distress.\n\nEscort drones are not hostile unless a drone is provoked.",
                            MyStats = new List<GeneralInfoStat> { new GeneralInfoStat
                {
                    Left = "Type",
                    Right = "Unmanned Drone"
                },
                new GeneralInfoStat
                {
                    Left = "Faction",
                    Right = "Unknown"
                },
                new GeneralInfoStat
                {
                    Left = "Service",
                    Right = "Asset Protection"
                }
                }
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
                        Messaging.Notification("New GX Entry Added!", PhotonTargets.All);
                        PLGlobal.Instance.AllGeneralInfos.Add(new GeneralInfo
                        {
                            Name = "Guardian Drone",
                            Name_lower = "guardian drone",
                            ID = 31,
                            Desc = "Drone seen protecting the mining drone hub world in this galaxy.\n\nThey are outfitted with powerful weaponry designed to debilitate attacking ships. Each drone is equipped with a signal jammer that blocks teleportation to thier world's surface.\n\nGuardian drones are always hostile.",
                            MyStats = new List<GeneralInfoStat> { new GeneralInfoStat
                {
                    Left = "Type",
                    Right = "Unmanned Drone"
                },
                new GeneralInfoStat
                {
                    Left = "Faction",
                    Right = "Unknown"
                },
                new GeneralInfoStat
                {
                    Left = "Service",
                    Right = "Hub World Protector"
                }
                }
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
                    if (PhotonNetwork.isMasterClient && this.ShipStats.Ship.AlertLevel > 0)
                    {
                        foreach (PLShipInfoBase plShipInfoBase in UnityEngine.Object.FindObjectsOfType(typeof(PLShipInfoBase)))
                        {
                            if (plShipInfoBase.ShipTypeID == EShipType.E_WDDRONE2 || plShipInfoBase.ShipTypeID == EShipType.E_WDDRONE1)
                            {
                                foreach (PLShipComponent component in plShipInfoBase.MyStats.GetComponentsOfType(ESlotType.E_COMP_DISTRESS_SIGNAL))
                                {
                                    if (component is MiningDroneSignal)
                                    {
                                        this.ShipStats.Ship.PersistantShipInfo.ForcedHostile = true;
                                        break;
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
                    this.ShipStats.ManeuverThrustOutputMax = 0f;
                    this.ShipStats.InertiaThrustOutputMax = 0f;
                }
            }
        }
        public static PLDistressSignal CreateDistressSignal(int Subtype, int level)
        {

            if (Subtype > 3)
            {
                return new MiningDroneSignal(4, level);
            }
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
