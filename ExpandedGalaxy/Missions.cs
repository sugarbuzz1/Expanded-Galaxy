using CodeStage.AntiCheat.ObscuredTypes;
using HarmonyLib;
using PulsarModLoader.Content.Components.Missile;
using PulsarModLoader.Content.Components.MissionShipComponent;
using PulsarModLoader.Content.Components.Turret;
using PulsarModLoader.Content.Items;
using System;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;

namespace ExpandedGalaxy
{
    internal class Missions
    {
        public class JunkCubeStart
        {
            public static PickupMissionData MissionData => Missions.JunkCubeStart.CreateData();
            private static PickupMissionData CreateData()
            {
                PickupMissionData pickupMissionData = new PickupMissionData();
                pickupMissionData.Name = "\"Special\" Offer";
                pickupMissionData.Desc = "I'm looking for something. On the outside it looks like a worthless hunk of garbage, but it's actully considered a high deity to some denizens of the galaxy. If you can find it and bring it to me, I will make it worth your while.";
                pickupMissionData.CanBeAbandonedByPlayers = false;
                pickupMissionData.MissionID = 8000000;
                pickupMissionData.CanBeBlockedByOtherPickupMissions = false;
                pickupMissionData.LongRangeDialogueActorID = "";
                pickupMissionData.LongRangeDialogueDisplayName = "";
                pickupMissionData.LongRangeDialogueDisplayNameOriginal = "";
                List<ObjectiveData> objectiveDatas = new List<ObjectiveData>()
                {
                    new ObjectiveData
                    {
                        ObjType = 2,
                        Data = new Dictionary<string, string>()
                        {
                            {
                                "ScriptName",
                                "ExGal_GotJunkCube1"
                            },
                            {
                                "CustomText",
                                "Find the Junk Cube"
                            },
                            {
                                "PC_ComponentType",
                                "E_COMP_MISSION_COMPONENT"
                            },
                            {
                                "PC_SubType",
                                "8"
                            },
                            {
                                "PC_CompName",
                                "Junk Cube"
                            },
                            {
                                "PC_AmountNeeded",
                                "1"
                            },
                            {
                                "PC_RemoveComponents",
                                "true"
                            }
                        }
                    },
                    new ObjectiveData
                    {
                        ObjType = 0,
                        Data = new Dictionary<string, string>()
                        {
                            {
                                "ScriptName",
                                "ExGal_DeliverJunkCube1"
                            },
                            {
                                "CustomText",
                                "Deliver the Junk Cube to the Caravan"
                            }
                        }
                    },
                };
                pickupMissionData.Objectives.AddRange(objectiveDatas);
                return pickupMissionData;
            }
        }

        public class JunkCubeWait1
        {
            public static PickupMissionData MissionData => Missions.JunkCubeWait1.CreateData();
            private static PickupMissionData CreateData()
            {
                PickupMissionData pickupMissionData = new PickupMissionData();
                pickupMissionData.Name = "Junk Processing";
                pickupMissionData.Desc = "Incredible! I'm suprised you were able to find it in a galaxy so vast... assuming what you've brought me is authentic that is! I'll need some time to make sure what you brought me isn't actually junk. Come find me later and I'll give you your reward.";
                pickupMissionData.CanBeAbandonedByPlayers = false;
                pickupMissionData.MissionID = 8000001;
                pickupMissionData.CanBeBlockedByOtherPickupMissions = false;
                pickupMissionData.LongRangeDialogueActorID = "";
                pickupMissionData.LongRangeDialogueDisplayName = "";
                pickupMissionData.LongRangeDialogueDisplayNameOriginal = "";
                List<ObjectiveData> objectiveDatas = new List<ObjectiveData>()
                {
                    new ObjectiveData
                    {
                        ObjType = 21,
                        Data = new Dictionary<string, string>()
                        {
                            {
                                "ScriptName",
                                "ExGal_WaitJunkCube1"
                            },
                            {
                                "CMAJC_Value",
                                "15"
                            }
                        }
                    },
                    new ObjectiveData
                    {
                        ObjType = 0,
                        Data = new Dictionary<string, string>()
                        {
                            {
                                "ScriptName",
                                "ExGal_WaitJunkCubeReturn1"
                            },
                            {
                                "CustomText",
                                "Return to the Caravan"
                            }
                        }
                    },
                };
                pickupMissionData.Objectives.AddRange(objectiveDatas);
                return pickupMissionData;
            }
        }

        public class JunkCubeRetrieval
        {
            public static PickupMissionData MissionData => Missions.JunkCubeRetrieval.CreateData();
            private static PickupMissionData CreateData()
            {
                PickupMissionData pickupMissionData = new PickupMissionData();
                pickupMissionData.Name = "Recover Junk Cube";
                pickupMissionData.Desc = "I was wondering when you'd return for that. Unfortunatly some bandits took off with it while we were stopped at Cornelia. They kept shouting \"Praise be the Cube,\" whatever that means. I've tracked them to this sector, bring it back to me so I can finish looking it over and get you your reward.";
                pickupMissionData.CanBeAbandonedByPlayers = false;
                pickupMissionData.MissionID = 8000002;
                pickupMissionData.CanBeBlockedByOtherPickupMissions = false;
                pickupMissionData.LongRangeDialogueActorID = "";
                pickupMissionData.LongRangeDialogueDisplayName = "";
                pickupMissionData.LongRangeDialogueDisplayNameOriginal = "";
                List<ObjectiveData> objectiveDatas = new List<ObjectiveData>()
                {
                    new ObjectiveData
                    {
                        ObjType = 1,
                        Data = new Dictionary<string, string>()
                        {
                            {
                                "CustomText",
                                "Reach the marked sector"
                            },
                            {
                                "RST_SectorTypeValue",
                                "16"
                            },
                            {
                                "RST_MustKillAll",
                                "0"
                            }
                        }
                    },
                    new ObjectiveData
                    {
                        ObjType = 2,
                        Data = new Dictionary<string, string>()
                        {
                            {
                                "ScriptName",
                                "ExGal_GotJunkCube2"
                            },
                            {
                                "CustomText",
                                "Retrieve the Junk Cube"
                            },
                            {
                                "PC_ComponentType",
                                "E_COMP_MISSION_COMPONENT"
                            },
                            {
                                "PC_SubType",
                                "8"
                            },
                            {
                                "PC_CompName",
                                "Junk Cube"
                            },
                            {
                                "PC_AmountNeeded",
                                "1"
                            },
                            {
                                "PC_RemoveComponents",
                                "true"
                            }
                        }
                    },
                    new ObjectiveData
                    {
                        ObjType = 0,
                        Data = new Dictionary<string, string>()
                        {
                            {
                                "ScriptName",
                                "ExGal_DeliverJunkCube2"
                            },
                            {
                                "CustomText",
                                "Deliver the Junk Cube back to the Caravan"
                            }
                        }
                    },
                };
                PickupSectorData sectorData = new PickupSectorData();
                sectorData.SectorType = (int)ESectorVisualIndication.CANYON;
                sectorData.Distance = 36f;
                sectorData.SpawnRegularShipsToo = false;
                sectorData.UniqueType = false;
                pickupMissionData.Sectors.Add(sectorData);
                pickupMissionData.Objectives.AddRange(objectiveDatas);
                return pickupMissionData;
            }
        }

        public class JunkCubeWait2
        {
            public static PickupMissionData MissionData => Missions.JunkCubeWait2.CreateData();
            private static PickupMissionData CreateData()
            {
                PickupMissionData pickupMissionData = new PickupMissionData();
                pickupMissionData.Name = "Junk Reprocessing";
                pickupMissionData.Desc = "Nice work. I won't lose it this time I promise! Once I finish examining this come find me and I'll give you your reward.";
                pickupMissionData.CanBeAbandonedByPlayers = false;
                pickupMissionData.MissionID = 8000003;
                pickupMissionData.CanBeBlockedByOtherPickupMissions = false;
                pickupMissionData.LongRangeDialogueActorID = "";
                pickupMissionData.LongRangeDialogueDisplayName = "";
                pickupMissionData.LongRangeDialogueDisplayNameOriginal = "";
                List<ObjectiveData> objectiveDatas = new List<ObjectiveData>()
                {
                    new ObjectiveData
                    {
                        ObjType = 21,
                        Data = new Dictionary<string, string>()
                        {
                            {
                                "ScriptName",
                                "ExGal_WaitJunkCube2"
                            },
                            {
                                "CMAJC_Value",
                                "15"
                            }
                        }
                    },
                    new ObjectiveData
                    {
                        ObjType = 0,
                        Data = new Dictionary<string, string>()
                        {
                            {
                                "ScriptName",
                                "ExGal_WaitJunkCubeReturn2"
                            },
                            {
                                "CustomText",
                                "Return to the Caravan"
                            }
                        }
                    },
                };
                List<RewardData> rewardDatas = new List<RewardData>()
                {
                    new RewardData
                    {
                        RwdType = 3,
                        RewardDataA = (int)ESlotType.E_COMP_MISSION_COMPONENT,
                        RewardDataB = (int)MissionShipComponentModManager.Instance.GetMissionShipComponentIDFromName("Reward"),
                        RewardAmount = 1

                    }
                };
                pickupMissionData.SuccessRewards.AddRange(rewardDatas);
                pickupMissionData.Objectives.AddRange(objectiveDatas);
                return pickupMissionData;
            }
        }

        public class WDHuntProjVulcanus
        {
            public static PickupMissionData MissionData => CreateData();

            public static PickupMissionData CreateData()
            {
                PickupMissionData pickupMissionData = new PickupMissionData();
                pickupMissionData.Name = "Hunt Project Vulcanus";
                pickupMissionData.Desc = "I've tracked the crew to this sector. Take over the ship - DON'T DESTROY IT - and bring it to Maes Argale at Dutain's Garage. I'll have another crew handle your ship.";
                pickupMissionData.CanBeAbandonedByPlayers = true;
                pickupMissionData.MissionID = 8000004;
                pickupMissionData.CanBeBlockedByOtherPickupMissions = false;
                pickupMissionData.LongRangeDialogueActorID = "";
                pickupMissionData.LongRangeDialogueDisplayName = "";
                pickupMissionData.LongRangeDialogueDisplayNameOriginal = "";

                PickupSectorData pickupSectorData = new PickupSectorData();
                pickupSectorData.SectorType = 145;
                pickupSectorData.UniqueType = true;
                pickupSectorData.Distance = 32f;
                pickupSectorData.SpawnRegularShipsToo = false;
                pickupSectorData.FactionID = 2;
                
                PickupShipData pickupShipData = new PickupShipData();
                pickupShipData.Name = "Project Vulcanus";
                pickupShipData.RandomizeName = false;
                pickupShipData.DropDefaultCredits = false;
                pickupShipData.CreditsToDrop = 0;
                pickupShipData.DialogueActorID = "";
                pickupShipData.Flagged = true;
                pickupShipData.ForceHostileAgainstPlayerShip = true;
                pickupShipData.ShipType = (int)EShipType.E_ANNIHILATOR;
                pickupShipData.AllComponentOverrides = new List<ComponentOverrideData>()
                {
                    new ComponentOverrideData()
                    {
                        CompType = (int)ESlotType.E_COMP_SHLD,
                        CompSubType = (int)EShieldGeneratorType.E_XC7_SHIELDS,
                        CompLevel = 3,
                        IsCargo = false,
                        CompTypeToReplace = (int)ESlotType.E_COMP_SHLD,
                        SlotNumberToReplace = 0,
                        ReplaceExistingComp = true,
                    },
                    new ComponentOverrideData()
                    {
                        CompType = (int)ESlotType.E_COMP_REACTOR,
                        CompSubType = (int)EReactorType.E_REAC_STRONGPOINT,
                        CompLevel = 2,
                        IsCargo = false,
                        CompTypeToReplace = (int)ESlotType.E_COMP_REACTOR,
                        SlotNumberToReplace = 0,
                        ReplaceExistingComp = true,
                    },
                    new ComponentOverrideData()
                    {
                        CompType = (int)ESlotType.E_COMP_HULL,
                        CompSubType = (int)EHullType.E_LAYERED_HULL,
                        CompLevel = 3,
                        IsCargo = false,
                        CompTypeToReplace = (int)ESlotType.E_COMP_HULL,
                        SlotNumberToReplace = 0,
                        ReplaceExistingComp = true,
                    },
                    new ComponentOverrideData()
                    {
                        CompType = (int)ESlotType.E_COMP_MAINTURRET,
                        CompSubType = (int)4,
                        CompLevel = 2,
                        IsCargo = false,
                        CompTypeToReplace = (int)ESlotType.E_COMP_MAINTURRET,
                        SlotNumberToReplace = 0,
                        ReplaceExistingComp = true,
                    },
                    new ComponentOverrideData()
                    {
                        CompType = (int)ESlotType.E_COMP_TURRET,
                        CompSubType = TurretModManager.Instance.GetTurretIDFromName("Missile Turret Mk. II"),
                        CompLevel = 2,
                        IsCargo = false,
                        CompTypeToReplace = (int)ESlotType.E_COMP_TURRET,
                        SlotNumberToReplace = 0,
                        ReplaceExistingComp = true,
                    },
                    new ComponentOverrideData()
                    {
                        CompType = (int)ESlotType.E_COMP_TURRET,
                        CompSubType = (int)ETurretType.FOCUS_LASER,
                        CompLevel = 2,
                        IsCargo = false,
                        CompTypeToReplace = (int)ESlotType.E_COMP_TURRET,
                        SlotNumberToReplace = 1,
                        ReplaceExistingComp = true,
                    },
                    new ComponentOverrideData()
                    {
                        CompType = (int)ESlotType.E_COMP_TRACKERMISSILE,
                        CompSubType = MissileModManager.Instance.GetMissileIDFromName("Thermobaric Missile"),
                        CompLevel = 0,
                        IsCargo = false,
                        CompTypeToReplace = (int)ESlotType.E_COMP_TRACKERMISSILE,
                        SlotNumberToReplace = 0,
                        ReplaceExistingComp = true,
                    },
                    new ComponentOverrideData()
                    {
                        CompType = (int)ESlotType.E_COMP_TRACKERMISSILE,
                        CompSubType = MissileModManager.Instance.GetMissileIDFromName("Thermobaric Missile"),
                        CompLevel = 0,
                        IsCargo = false,
                        CompTypeToReplace = (int)ESlotType.E_COMP_TRACKERMISSILE,
                        SlotNumberToReplace = 1,
                        ReplaceExistingComp = true,
                    },
                    new ComponentOverrideData()
                    {
                        CompType = (int)ESlotType.E_COMP_TRACKERMISSILE,
                        CompSubType = (int)ETrackerMissileType.SYSTEM_DAMAGE,
                        CompLevel = 0,
                        IsCargo = false,
                        CompTypeToReplace = (int)ESlotType.E_COMP_TRACKERMISSILE,
                        SlotNumberToReplace = 2,
                        ReplaceExistingComp = true,
                    },
                    new ComponentOverrideData()
                    {
                        CompType = (int)ESlotType.E_COMP_PROGRAM,
                        CompSubType = (int)EWarpDriveProgramType.SUPER_SHIELD_BOOSTER,
                        CompLevel = 0,
                        IsCargo = false,
                        CompTypeToReplace = (int)ESlotType.E_COMP_PROGRAM,
                        SlotNumberToReplace = 0,
                        ReplaceExistingComp = true,
                    },
                    new ComponentOverrideData()
                    {
                        CompType = (int)ESlotType.E_COMP_PROGRAM,
                        CompSubType = (int)EWarpDriveProgramType.SUPER_SHIELD_BOOSTER,
                        CompLevel = 0,
                        IsCargo = false,
                        CompTypeToReplace = (int)ESlotType.E_COMP_PROGRAM,
                        SlotNumberToReplace = 1,
                        ReplaceExistingComp = true,
                    },
                    new ComponentOverrideData()
                    {
                        CompType = (int)ESlotType.E_COMP_PROGRAM,
                        CompSubType = (int)EWarpDriveProgramType.BURST_ANTIVIRUS,
                        CompLevel = 0,
                        IsCargo = false,
                        CompTypeToReplace = (int)ESlotType.E_COMP_PROGRAM,
                        SlotNumberToReplace = 2,
                        ReplaceExistingComp = true,
                    },
                    new ComponentOverrideData()
                    {
                        CompType = (int)ESlotType.E_COMP_PROGRAM,
                        CompSubType = (int)EWarpDriveProgramType.QUANTUM_DEFENSES,
                        CompLevel = 0,
                        IsCargo = false,
                        CompTypeToReplace = (int)ESlotType.E_COMP_PROGRAM,
                        SlotNumberToReplace = 3,
                        ReplaceExistingComp = true,
                    },
                    new ComponentOverrideData()
                    {
                        CompType = (int)ESlotType.E_COMP_PROGRAM,
                        CompSubType = (int)EWarpDriveProgramType.DIG_COOLANT,
                        CompLevel = 0,
                        IsCargo = false,
                        CompTypeToReplace = (int)ESlotType.E_COMP_PROGRAM,
                        SlotNumberToReplace = 4,
                        ReplaceExistingComp = true,
                    },
                    new ComponentOverrideData()
                    {
                        CompType = (int)ESlotType.E_COMP_PROGRAM,
                        CompSubType = (int)EWarpDriveProgramType.SYBER_THREAT,
                        CompLevel = 0,
                        IsCargo = false,
                        CompTypeToReplace = (int)ESlotType.E_COMP_PROGRAM,
                        SlotNumberToReplace = 5,
                        ReplaceExistingComp = true,
                    },
                    new ComponentOverrideData()
                    {
                        CompType = (int)ESlotType.E_COMP_PROGRAM,
                        CompSubType = (int)EWarpDriveProgramType.BARRAGE,
                        CompLevel = 0,
                        IsCargo = false,
                        CompTypeToReplace = (int)ESlotType.E_COMP_PROGRAM,
                        SlotNumberToReplace = 6,
                        ReplaceExistingComp = true,
                    },
                    new ComponentOverrideData()
                    {
                        CompType = (int)ESlotType.E_COMP_PROGRAM,
                        CompSubType = (int)EWarpDriveProgramType.FULL_HEAL_ALL_SYSTEMS,
                        CompLevel = 0,
                        IsCargo = false,
                        CompTypeToReplace = (int)ESlotType.E_COMP_PROGRAM,
                        SlotNumberToReplace = 7,
                        ReplaceExistingComp = true,
                    },
                    new ComponentOverrideData()
                    {
                        CompType = (int)ESlotType.E_COMP_PROGRAM,
                        CompSubType = (int)EWarpDriveProgramType.THRUSTER_BOOST,
                        IsCargo = false,
                        CompTypeToReplace = (int)ESlotType.E_COMP_PROGRAM,
                        SlotNumberToReplace = 8,
                        ReplaceExistingComp = true,
                    },
                    new ComponentOverrideData()
                    {
                        CompType = (int)ESlotType.E_COMP_PROGRAM,
                        CompSubType = (int)EWarpDriveProgramType.DETECTOR,
                        CompLevel = 0,
                        IsCargo = false,
                        CompTypeToReplace = (int)ESlotType.E_COMP_PROGRAM,
                        SlotNumberToReplace = 9,
                        ReplaceExistingComp = true,
                    },
                    new ComponentOverrideData()
                    {
                        CompType = (int)ESlotType.E_COMP_CPU,
                        CompSubType = (int)ECPUClass.E_CPUTYPE_JUMP_PROCESSOR,
                        CompLevel = 0,
                        IsCargo = false,
                        CompTypeToReplace = (int)ESlotType.E_COMP_CPU,
                        SlotNumberToReplace = 0,
                        ReplaceExistingComp = true,
                    },
                    new ComponentOverrideData()
                    {
                        CompType = (int)ESlotType.E_COMP_CPU,
                        CompSubType = (int)ECPUClass.E_CPUTYPE_CYBER_DEF,
                        CompLevel = 2,
                        IsCargo = false,
                        CompTypeToReplace = (int)ESlotType.E_COMP_CPU,
                        SlotNumberToReplace = 1,
                        ReplaceExistingComp = true,
                    },
                    new ComponentOverrideData()
                    {
                        CompType = (int)ESlotType.E_COMP_CPU,
                        CompSubType = (int)ECPUClass.E_CPUTYPE_CYBER_DEF,
                        CompLevel = 3,
                        IsCargo = false,
                        CompTypeToReplace = (int)ESlotType.E_COMP_CPU,
                        SlotNumberToReplace = 2,
                        ReplaceExistingComp = true,
                    },
                    new ComponentOverrideData()
                    {
                        CompType = (int)ESlotType.E_COMP_CPU,
                        CompSubType = (int)ECPUClass.E_CPUTYPE_SHIELD_COPROCESSOR,
                        CompLevel = 2,
                        IsCargo = false,
                        CompTypeToReplace = (int)ESlotType.E_COMP_CPU,
                        SlotNumberToReplace = 3,
                        ReplaceExistingComp = true,
                    },
                    new ComponentOverrideData()
                    {
                        CompType = (int)ESlotType.E_COMP_CPU,
                        CompSubType = (int)ECPUClass.E_CPUTYPE_SHIELD_COPROCESSOR,
                        CompLevel = 2,
                        IsCargo = false,
                        CompTypeToReplace = (int)ESlotType.E_COMP_CPU,
                        SlotNumberToReplace = 4,
                        ReplaceExistingComp = true,
                    },
                    new ComponentOverrideData()
                    {
                        CompType = (int)ESlotType.E_COMP_CPU,
                        CompSubType = (int)ECPUClass.IMPROVED_DEFENSES,
                        CompLevel = 3,
                        IsCargo = false,
                        CompTypeToReplace = (int)ESlotType.E_COMP_CPU,
                        SlotNumberToReplace = 5,
                        ReplaceExistingComp = true,
                    },
                    new ComponentOverrideData()
                    {
                        CompType = (int)ESlotType.E_COMP_THRUSTER,
                        CompSubType = (int)EThrusterType.E_THRUSTER_PERF,
                        CompLevel = 2,
                        IsCargo = false,
                        CompTypeToReplace = (int)ESlotType.E_COMP_THRUSTER,
                        SlotNumberToReplace = 0,
                        ReplaceExistingComp = true,
                    },
                    new ComponentOverrideData()
                    {
                        CompType = (int)ESlotType.E_COMP_THRUSTER,
                        CompSubType = (int)EThrusterType.E_THRUSTER_PERF,
                        CompLevel = 2,
                        IsCargo = false,
                        CompTypeToReplace = (int)ESlotType.E_COMP_THRUSTER,
                        SlotNumberToReplace = 1,
                        ReplaceExistingComp = true,
                    },
                    new ComponentOverrideData()
                    {
                        CompType = (int)ESlotType.E_COMP_DISTRESS_SIGNAL,
                        CompSubType = 5,
                        CompLevel = 0,
                        IsCargo = true,
                    }
                };
                pickupShipData.ImmediatelyStartDialogue = false;
                pickupShipData.ForceHostileAgainstAll = true;
                pickupShipData.ForceHostileAgainstName = "";

                pickupSectorData.Ships.Add(pickupShipData);

                pickupMissionData.Sectors.Add(pickupSectorData);

                List<ObjectiveData> objectiveDatas = new List<ObjectiveData>()
                {
                    new ObjectiveData
                    {
                        ObjType = 1,
                        Data = new Dictionary<string, string>()
                        {
                            {
                                "RST_SectorTypeValue",
                                "145"
                            },
                            {
                                "RST_DestNameValue",
                                ""
                            },
                            {
                                "RST_MustKillAll",
                                "0"
                            },
                            {
                                "CustomText",
                                "Reach the marked sector"
                            }
                        }
                    },
                    new ObjectiveData
                    {
                        ObjType = 0,
                        Data = new Dictionary<string, string>()
                        {
                            {
                                "ScriptName",
                                "ExGal_ClaimVulcanus"
                            },
                            {
                                "CustomText",
                                "Hijack Project Vulcanus"
                            }
                        }
                    },
                    new ObjectiveData
                    {
                        ObjType = 0,
                        Data = new Dictionary<string, string>()
                        {
                            {
                                "ScriptName",
                                "ExGal_DeliverVulcanus"
                            },
                            {
                                "CustomText",
                                "Deliver Project Vulcanus to Dutain's Garage"
                            }
                        }
                    },
                    new ObjectiveData
                    {
                        ObjType = 0,
                        Data = new Dictionary<string, string>()
                        {
                            {
                                "ScriptName",
                                "ExGal_TalkVulcanus"
                            },
                            {
                                "CustomText",
                                "Talk to Maes Argale"
                            }
                        }
                    },
                };
                List<RewardData> rewardDatas = new List<RewardData>()
                {
                    new RewardData
                    {
                        RwdType = 1,
                        RewardAmount = 7000
                    },
                    new RewardData
                    {
                        RwdType = 4,
                        RewardAmount = 2
                    },
                    new RewardData
                    {
                        RwdType = 6,
                        RewardDataA = 2,
                        RewardAmount = 2
                    },
                };

                pickupMissionData.Objectives.AddRange(objectiveDatas);
                pickupMissionData.SuccessRewards.AddRange(rewardDatas);

                return pickupMissionData;
            }
        }

        public class CUFriendlyFavorA
        {
            public static PickupMissionData MissionData => CreateData();

            public static PickupMissionData CreateData()
            {
                PickupMissionData pickupMissionData = new PickupMissionData();
                pickupMissionData.Name = "Friendly Favor";
                pickupMissionData.Desc = "I've tracked the crew to this sector. Take over the ship - DON'T DESTROY IT - and bring it to Maes Argale at Dutain's Garage. I'll have another crew handle your ship.";
                pickupMissionData.CanBeAbandonedByPlayers = true;
                pickupMissionData.MissionID = 8000005;
                pickupMissionData.CanBeBlockedByOtherPickupMissions = false;
                pickupMissionData.LongRangeDialogueActorID = "";
                pickupMissionData.LongRangeDialogueDisplayName = "";
                pickupMissionData.LongRangeDialogueDisplayNameOriginal = "";

                PickupSectorData pickupSectorData = new PickupSectorData();
                pickupSectorData.SectorType = 146;
                pickupSectorData.UniqueType = true;
                pickupSectorData.Distance = 16f;
                pickupSectorData.SpawnRegularShipsToo = false;
                pickupSectorData.FactionID = 1;

                PickupShipData pickupShipData = new PickupShipData();
                pickupShipData.Name = "The Milano";
                pickupShipData.RandomizeName = true;
                pickupShipData.DropDefaultCredits = false;
                pickupShipData.CreditsToDrop = 0;
                pickupShipData.DialogueActorID = "ExGal_ContactFF";
                pickupShipData.Flagged = false;
                pickupShipData.ForceHostileAgainstPlayerShip = false;
                pickupShipData.ShipType = (int)EShipType.E_STARGAZER;
                pickupShipData.ImmediatelyStartDialogue = true;
                pickupShipData.ForceHostileAgainstAll = false;
                pickupShipData.ForceHostileAgainstName = "";

                pickupSectorData.Ships.Add(pickupShipData);

                pickupMissionData.Sectors.Add(pickupSectorData);

                List<ObjectiveData> objectiveDatas = new List<ObjectiveData>()
                {
                    new ObjectiveData
                    {
                        ObjType = 1,
                        Data = new Dictionary<string, string>()
                        {
                            {
                                "RST_SectorTypeValue",
                                "146"
                            },
                            {
                                "RST_DestNameValue",
                                ""
                            },
                            {
                                "RST_MustKillAll",
                                "0"
                            },
                            {
                                "CustomText",
                                "Reach the marked sector"
                            }
                        }
                    },
                    new ObjectiveData
                    {
                        ObjType = 2,
                        Data = new Dictionary<string, string>()
                        {
                            {
                                "ScriptName",
                                "ExGal_MeetContactFF"
                            },
                            {
                                "PC_ComponentType",
                                "E_COMP_MISSION_COMPONENT"
                            },
                            {
                                "PC_SubType",
                                MissionShipComponentModManager.Instance.GetMissionShipComponentIDFromName("Frequency Scanner").ToString()
                            },
                            {
                                "PC_AmountNeeded",
                                "1"
                            },
                            {
                                "PC_RemoveComponents",
                                "True"
                            },
                            {
                                "CustomText",
                                "Meet with the contact"
                            }
                        }
                    },
                    new ObjectiveData
                    {
                        ObjType = 0,
                        Data = new Dictionary<string, string>()
                        {
                            {
                                "ScriptName",
                                "ExGal_ReturnFF"
                            },
                            {
                                "CustomText",
                                "Return to Ena Sekra in the apartments at Outpost 448"
                            }
                        }
                    }
                };
                ItemModManager.Instance.GetItemIDsFromName("Auto Rifle", out int MainType, out int SubType);
                List<RewardData> rewardDatas = new List<RewardData>()
                {
                    new RewardData
                    {
                        RwdType = 2,
                        RewardDataA = MainType,
                        RewardDataB = SubType,
                        RewardAmount = 6,
                    },
                    new RewardData
                    {
                        RwdType = 2,
                        RewardDataA = MainType,
                        RewardDataB = SubType,
                        RewardAmount = 6,
                    },
                    new RewardData
                    {
                        RwdType = 4,
                        RewardAmount = 2
                    },
                };

                pickupMissionData.Objectives.AddRange(objectiveDatas);
                pickupMissionData.SuccessRewards.AddRange(rewardDatas);

                return pickupMissionData;
            }
        }

        public class CUFriendlyFavorB
        {
            public static PickupMissionData MissionData => CreateData();

            public static PickupMissionData CreateData()
            {
                PickupMissionData pickupMissionData = new PickupMissionData();
                pickupMissionData.Name = "Friendly Favor";
                pickupMissionData.Desc = "I've tracked the crew to this sector. Take over the ship - DON'T DESTROY IT - and bring it to Maes Argale at Dutain's Garage. I'll have another crew handle your ship.";
                pickupMissionData.CanBeAbandonedByPlayers = true;
                pickupMissionData.MissionID = 8000006;
                pickupMissionData.CanBeBlockedByOtherPickupMissions = false;
                pickupMissionData.LongRangeDialogueActorID = "";
                pickupMissionData.LongRangeDialogueDisplayName = "";
                pickupMissionData.LongRangeDialogueDisplayNameOriginal = "";

                PickupSectorData pickupSectorData = new PickupSectorData();
                pickupSectorData.SectorType = 146;
                pickupSectorData.UniqueType = true;
                pickupSectorData.Distance = 16f;
                pickupSectorData.SpawnRegularShipsToo = false;
                pickupSectorData.FactionID = 1;

                PickupShipData pickupShipData = new PickupShipData();
                pickupShipData.Name = "The Milano";
                pickupShipData.RandomizeName = true;
                pickupShipData.DropDefaultCredits = false;
                pickupShipData.CreditsToDrop = 0;
                pickupShipData.DialogueActorID = "ExGal_ContactFF";
                pickupShipData.Flagged = false;
                pickupShipData.ForceHostileAgainstPlayerShip = false;
                pickupShipData.ShipType = (int)EShipType.E_STARGAZER;
                pickupShipData.ImmediatelyStartDialogue = true;
                pickupShipData.ForceHostileAgainstAll = false;
                pickupShipData.ForceHostileAgainstName = "";

                pickupSectorData.Ships.Add(pickupShipData);

                pickupMissionData.Sectors.Add(pickupSectorData);

                List<ObjectiveData> objectiveDatas = new List<ObjectiveData>()
                {
                    new ObjectiveData
                    {
                        ObjType = 1,
                        Data = new Dictionary<string, string>()
                        {
                            {
                                "RST_SectorTypeValue",
                                "146"
                            },
                            {
                                "RST_DestNameValue",
                                ""
                            },
                            {
                                "RST_MustKillAll",
                                "0"
                            },
                            {
                                "CustomText",
                                "Reach the marked sector"
                            }
                        }
                    },
                    new ObjectiveData
                    {
                        ObjType = 0,
                        Data = new Dictionary<string, string>()
                        {
                            {
                                "ScriptName",
                                "ExGal_MeetContactFF"
                            },
                            {
                                "CustomText",
                                "Meet with the contact"
                            }
                        }
                    },
                    new ObjectiveData
                    {
                        ObjType = 0,
                        Data = new Dictionary<string, string>()
                        {
                            {
                                "ScriptName",
                                "ExGal_ReturnFF"
                            },
                            {
                                "CustomText",
                                "Return to Ena Sekra in the apartments at Outpost 448"
                            }
                        }
                    }
                };
                ItemModManager.Instance.GetItemIDsFromName("Auto Rifle", out int MainType, out int SubType);
                List<RewardData> rewardDatas = new List<RewardData>()
                {
                    new RewardData
                    {
                        RwdType = 2,
                        RewardDataA = MainType,
                        RewardDataB = SubType,
                        RewardAmount = 6,
                    },
                    new RewardData
                    {
                        RwdType = 2,
                        RewardDataA = MainType,
                        RewardDataB = SubType,
                        RewardAmount = 6,
                    },
                    new RewardData
                    {
                        RwdType = 4,
                        RewardAmount = 2
                    },
                };

                pickupMissionData.Objectives.AddRange(objectiveDatas);
                pickupMissionData.SuccessRewards.AddRange(rewardDatas);

                return pickupMissionData;
            }
        }

        public class CargoInspectionRepeatable
        {
            public static PickupMissionData MissionData => CreateData();

            public static PickupMissionData CreateData()
            {
                PickupMissionData pickupMissionData = new PickupMissionData();
                pickupMissionData.Name = "Cargo Inspection";
                pickupMissionData.Desc = "You have been selected for a cargo inspection. Please redirect your course to the nearest inspection station, and you will be compensated for your time. Noncompliance is considered to be a criminal offense and will be reported to the Outpost 448 Command Center. We apologize for the inconvenience and thank you for your cooperation.";
                pickupMissionData.CanBeAbandonedByPlayers = true;
                pickupMissionData.MissionID = 8000009;
                pickupMissionData.CanBeBlockedByOtherPickupMissions = false;
                pickupMissionData.LongRangeDialogueActorID = "ExGal_Inspection_Comms";
                pickupMissionData.LongRangeDialogueDisplayName = "Inspection Station";
                pickupMissionData.LongRangeDialogueDisplayNameOriginal = "Inspection Station";
                pickupMissionData.CallOnStart = true;
                pickupMissionData.BlocksOtherPickupMissionStarts = true;
                pickupMissionData.FailureRequirements_ALLMUSTMATCH = true;
                pickupMissionData.IsRepeatable = true;

                PickupSectorData pickupSectorData = new PickupSectorData();
                pickupSectorData.SectorType = 78;
                pickupSectorData.UniqueType = false;
                pickupSectorData.Distance = 1f;
                pickupSectorData.SpawnRegularShipsToo = false;
                pickupSectorData.FactionID = 0;
                pickupSectorData.Name = "";

                pickupMissionData.Sectors.Add(pickupSectorData);

                RequirementData requirementData = new RequirementData();
                requirementData.ReqType = 16;
                requirementData.Data = new Dictionary<string, string>()
                {
                    {
                        "PercentSectorsFaction_ID",
                        "0"
                    },
                    {
                        "PercentSectorsFaction_Percent",
                        "50"
                    },
                    {
                        "PercentSectorsFaction_Range",
                        "5"
                    }
                };
                pickupMissionData.StartingRequirements.Add(requirementData);
                
                List<ObjectiveData> objectiveDatas = new List<ObjectiveData>()
                {
                    new ObjectiveData
                    {
                        ObjType = 1,
                        Data = new Dictionary<string, string>()
                        {
                            {
                                "RST_SectorTypeValue",
                                "CONTRABAND_STATION"
                            },
                            {
                                "RST_DestNameValue",
                                ""
                            },
                            {
                                "RST_MustKillAll",
                                "0"
                            },
                            {
                                "CustomText",
                                "Go to the inspection station"
                            }
                        }
                    },
                    new ObjectiveData
                    {
                        ObjType = 10,
                        Data = new Dictionary<string, string>()
                        {
                            {
                                "CMIJC_Value",
                                "2"
                            },
                        }
                    },
                };
                pickupMissionData.Objectives.AddRange(objectiveDatas);

                List<RewardData> rewardDatasSucceed = new List<RewardData>()
                {
                    new RewardData
                    {
                        RwdType = 1,
                        RewardAmount = 1000,
                    },
                };
                pickupMissionData.SuccessRewards.AddRange(rewardDatasSucceed);

                List<RewardData> rewardDatasFailure = new List<RewardData>()
                {
                    new RewardData
                    {
                        RwdType = 6,
                        RewardAmount = -3,
                        RewardDataA = 0,
                    },
                    new RewardData
                    {
                        RwdType = 5,
                        RewardAmount = 1,
                        RewardDataD = 0.1f
                    },
                    new RewardData
                    {
                        RwdType = 7,
                        RewardAmount = 1
                    }
                };
                pickupMissionData.FailureRewards.AddRange(rewardDatasFailure);

                return pickupMissionData;
            }
        }

        [HarmonyPatch(typeof(PLShipInfo), "Update")]
        internal class VulcanusShipUpdate
        {
            private static void Postfix(PLShipInfo __instance)
            {
                if (PLServer.Instance == null)
                    return;
                if (__instance == null)
                    return;
                if (PLServer.Instance.HasActiveMissionWithID(8000004))
                {
                    bool flag = false;
                    if (__instance.ShipTypeID == EShipType.E_ANNIHILATOR)
                    {
                        foreach (PLShipComponent pLShipComponent in __instance.MyStats.AllComponents)
                        {
                            if (pLShipComponent is MissionShipFlagComp)
                            {
                                flag = true;
                                break;
                            }
                        }
                    }
                    if (flag && !__instance.GetIsPlayerShip() && __instance.TeamID == -1)
                    {
                        if ((PLServer.Instance.GetActiveMissionWithID(8000004).MyMissionData as PickupMissionData).Sectors[0].Ships.Count < 2 && PLEncounterManager.Instance.PlayerShip != null)
                        {
                            PickupShipData playerShipData = new PickupShipData();
                            playerShipData.Name = PLEncounterManager.Instance.PlayerShip.ShipNameValue;
                            playerShipData.ShipType = (int)PLEncounterManager.Instance.PlayerShip.ShipTypeID;
                            foreach (PLShipComponent pLShipComponent in PLEncounterManager.Instance.PlayerShip.MyStats.AllComponents)
                            {
                                ComponentOverrideData componentOverrideData = new ComponentOverrideData();
                                componentOverrideData.ReplaceExistingComp = true;
                                componentOverrideData.CompTypeToReplace = (int)pLShipComponent.ActualSlotType;
                                componentOverrideData.CompType = (int)pLShipComponent.ActualSlotType;
                                componentOverrideData.CompSubType = pLShipComponent.SubType;
                                componentOverrideData.CompLevel = pLShipComponent.Level;
                                componentOverrideData.IsCargo = pLShipComponent.VisualSlotType == ESlotType.E_COMP_CARGO || pLShipComponent.VisualSlotType == ESlotType.E_COMP_HIDDENCARGO;
                                componentOverrideData.SlotNumberToReplace = pLShipComponent.SortID;
                                playerShipData.AllComponentOverrides.Add(componentOverrideData);
                            }
                            playerShipData.AllComponentOverrides.Add(new ComponentOverrideData()
                            {
                                ReplaceExistingComp = true,
                                CompTypeToReplace = (int)ESlotType.E_COMP_AIRLOCK,
                                CompType = (int)ESlotType.E_COMP_DISTRESS_SIGNAL,
                                CompSubType = 6,
                                CompLevel = 0,
                                IsCargo = false,
                                SlotNumberToReplace = 0,
                            });
                            playerShipData.AllComponentOverrides.Add(new ComponentOverrideData()
                            {
                                ReplaceExistingComp = true,
                                CompTypeToReplace = (int)ESlotType.E_COMP_REAC_COOLING,
                                CompType = (int)ESlotType.E_COMP_DISTRESS_SIGNAL,
                                CompSubType = 7,
                                CompLevel = 0,
                                IsCargo = false,
                                SlotNumberToReplace = 0,
                            });
                            playerShipData.FactionID = PLEncounterManager.Instance.PlayerShip.FactionID;
                            playerShipData.DialogueActorID = "";
                            playerShipData.Flagged = PLEncounterManager.Instance.PlayerShip.IsFlagged;
                            (PLServer.Instance.GetActiveMissionWithID(8000004).MyMissionData as PickupMissionData).Sectors[0].Ships.Add(playerShipData);
                            PLEncounterManager.Instance.PlayerShip.MyStats.AddShipComponent(new MissionNoExtractorFlag((int)ESlotType.E_COMP_DISTRESS_SIGNAL), visualSlot: ESlotType.E_COMP_AIRLOCK);
                            PLEncounterManager.Instance.PlayerShip.DropScrap = false;
                        }                        
                    }
                    if (!__instance.GetIsPlayerShip())
                        return;
                    if (flag && !PLMissionObjective.IDIsCompleted("ExGal_ClaimVulcanus"))
                        PLMissionObjective_Custom.OnCustomObjEvent("ExGal_ClaimVulcanus");
                    else if (!flag && PLMissionObjective.IDIsCompleted("ExGal_ClaimVulcanus"))
                    {
                        foreach (PLMissionObjective missionObjective in Traverse.Create(typeof(PLMissionObjective)).Field("AllMissionObjectives").GetValue<List<PLMissionObjective>>())
                        {
                            if (missionObjective != null && !missionObjective.IsCompleted && missionObjective is PLMissionObjective_Custom missionObjectiveCustom && missionObjectiveCustom.ScriptName == "ExGal_ClaimVulcanus")
                            {
                                --missionObjectiveCustom.AmountCompleted;
                                break;
                            }
                        }
                    }
                }
            }
        }

        [HarmonyPatch(typeof(PLWDAnnihilatorInfo), "OnEndWarp")]
        internal class VulcanusShipDeliver
        {
            private static void Postfix(PLWDAnnihilatorInfo __instance)
            {
                if (__instance == null || !__instance.GetIsPlayerShip())
                    return;
                if (PLServer.Instance.HasActiveMissionWithID(8000004))
                {
                    if (PLServer.GetCurrentSector() == null)
                        return;
                    if (PLServer.GetCurrentSector().VisualIndication != ESectorVisualIndication.RACING_SECTOR) {
                        if (PLMissionObjective.IDIsCompleted("ExGal_DeliverVulcanus"))
                        {
                            foreach (PLMissionObjective missionObjective in Traverse.Create(typeof(PLMissionObjective)).Field("AllMissionObjectives").GetValue<List<PLMissionObjective>>())
                            {
                                if (missionObjective != null && !missionObjective.IsCompleted && missionObjective is PLMissionObjective_Custom missionObjectiveCustom && missionObjectiveCustom.ScriptName == "ExGal_DeliverVulcanus")
                                {
                                    --missionObjectiveCustom.AmountCompleted;
                                    break;
                                }
                            }
                            return;
                        }
                    }
                    else {
                        bool flag = false;
                        foreach (PLShipComponent pLShipComponent in __instance.MyStats.AllComponents)
                        {
                            if (pLShipComponent is MissionShipFlagComp)
                            {
                                flag = true;
                                break;
                            }
                        }
                        if (flag)
                        {
                            PLMissionObjective_Custom.OnCustomObjEvent("ExGal_DeliverVulcanus");
                            if ((PLServer.Instance.GetActiveMissionWithID(8000004).MyMissionData as PickupMissionData).Sectors[0].Ships.Count < 2)
                                return;
                            PickupShipData shipData = (PLServer.Instance.GetActiveMissionWithID(8000004).MyMissionData as PickupMissionData).Sectors[0].Ships[1];
                            PLPersistantShipInfo shipInfo = new PLPersistantShipInfo((EShipType)shipData.ShipType, shipData.FactionID, PLServer.GetCurrentSector(), isFlagged: shipData.Flagged, ensureNoCrew: true);
                            shipInfo.CompOverrides.AddRange(shipData.AllComponentOverrides);
                            shipInfo.ShipName = shipData.Name;
                            shipInfo.SelectedActorID = shipData.DialogueActorID;
                            PLShipInfo info = (PLShipInfo)PLEncounterManager.Instance.GetCPEI().SpawnEnemyShip(shipInfo.Type, shipInfo, spawnPos: new UnityEngine.Vector3(-2351f, 414f, -94f));
                            info.CreditsLeftBehind = 0;
                            info.SetAbandoned(true);
                        }
                    }
                }
            }
        }

        public class PLMissionObjective_CompleteAfterJumpCount : PLMissionObjective
        {
            public PLMissionObjective_CompleteAfterJumpCount(int inJumpCount)
            {
                this.AmountNeeded = inJumpCount;
                this.AmountCompleted = 0;
                if (this.AmountNeeded <= 1)
                    return;
                this.m_ObjectiveText = PLLocalize.Localize("Complete mission after ") + this.AmountNeeded.ToString() + " " + PLLocalize.Localize("jumps.");
            }

            public override void CheckIfCompleted() => this.IsCompleted = this.AmountCompleted >= this.AmountNeeded;

            protected override string CustomPostString() => " (" + this.AmountCompleted.ToString() + "/" + this.AmountNeeded.ToString() + ")";

            public static void OnShipWarp()
            {
                foreach (PLMissionObjective missionObjective in PLMissionObjective.AllMissionObjectives)
                {
                    if (missionObjective != null && !missionObjective.MyMissionIsCompleted && missionObjective is PLMissionObjective_CompleteAfterJumpCount completeWithinJumpCount)
                        ++completeWithinJumpCount.AmountCompleted;
                }
            }
        }

        [HarmonyPatch(typeof(PLMissionObjective_CompleteWithinJumpCount), "OnShipWarp")]
        internal class CompleteAfterJumpWarp
        {
            private static void Postfix()
            {
                PLMissionObjective_CompleteAfterJumpCount.OnShipWarp();
            }
        }

        [HarmonyPatch(typeof(PLMissionBase), "CreateObjectiveFromData")]
        internal class AddCompleteAfterJump
        {
            private static bool Prefix(PLMissionBase __instance, PLMissionBase inMission, ObjectiveData inObjData)
            {
                PLMissionObjective missionObjective = null;
                if (inObjData.ObjType == 21)
                {
                    missionObjective = (PLMissionObjective)new PLMissionObjective_CompleteAfterJumpCount(int.Parse(inObjData.GetValueFromKey("CMAJC_Value")));
                }
                else
                    return true;
                if (missionObjective == null)
                    return true;
                missionObjective.Init();
                missionObjective.RawCustomText = inObjData.GetValueFromKey("CustomText");
                missionObjective.ScriptName = inObjData.GetValueFromKey("ScriptName");
                inMission.Objectives.Add(missionObjective);
                return false;
            }
        }

        [HarmonyPatch(typeof(PLLocalize), "Localize", new System.Type[] { typeof(string), typeof(string), typeof(bool) })]
        private class SkipLocalization
        {
            private static void Postfix(ref string __result, string value)
            {
                if (!(__result == string.Empty))
                    return;
                __result = value;
            }
        }

        [HarmonyPatch(typeof(PLCampaignIO), "GetMissionOfTypeID")]
        internal class CreateMissions
        {
            private static void Postfix(PLCampaignIO __instance, int inID, ref MissionData __result)
            {
                if (__result != null)
                    return;
                switch (inID)
                {
                    case 8000000:
                        PickupMissionData missiondata1 = Missions.JunkCubeStart.MissionData;
                        PLCampaignIO.Instance.GetAllPickupMissionData().Add(missiondata1);
                        __result = (MissionData)missiondata1;
                        break;
                    case 8000001:
                        PickupMissionData missiondata2 = Missions.JunkCubeWait1.MissionData;
                        PLCampaignIO.Instance.GetAllPickupMissionData().Add(missiondata2);
                        __result = (MissionData)missiondata2;
                        break;
                    case 8000002:
                        PickupMissionData missiondata3 = Missions.JunkCubeRetrieval.MissionData;
                        PLCampaignIO.Instance.GetAllPickupMissionData().Add(missiondata3);
                        __result = (MissionData)missiondata3;
                        break;
                    case 8000003:
                        PickupMissionData missiondata4 = Missions.JunkCubeWait2.MissionData;
                        PLCampaignIO.Instance.GetAllPickupMissionData().Add(missiondata4);
                        __result = (MissionData)missiondata4;
                        break;
                    case 8000004:
                        PickupMissionData missiondata5 = Missions.WDHuntProjVulcanus.MissionData;
                        PLCampaignIO.Instance.GetAllPickupMissionData().Add(missiondata5);
                        __result = (MissionData)missiondata5;
                        break;
                    case 8000005:
                        PickupMissionData missiondata6 = Missions.CUFriendlyFavorA.MissionData;
                        PLCampaignIO.Instance.GetAllPickupMissionData().Add(missiondata6);
                        __result = (MissionData)missiondata6;
                        break;
                    case 8000006:
                        PickupMissionData missiondata7 = Missions.CUFriendlyFavorA.MissionData;
                        PLCampaignIO.Instance.GetAllPickupMissionData().Add(missiondata7);
                        __result = (MissionData)missiondata7;
                        break;
                    case 8000009:
                        PickupMissionData missiondata10 = Missions.CargoInspectionRepeatable.MissionData;
                        PLCampaignIO.Instance.GetAllPickupMissionData().Add(missiondata10);
                        __result = (MissionData)missiondata10;
                        break;
                }
            }
        }

        public class MissionShipFlagComp : PLDistressSignal
        {
            public MissionShipFlagComp(int inType, int inLevel = 0) : base(EDistressSignalType.UNION, inLevel)
            {
                this.SubType = 5;
                this.Name = "ExGal_ProjVulcanus_Flag";
                this.Desc = "If you're reading this, I fucked up :(";
                this.CanBeDroppedOnShipDeath = false;
                this.Level = inLevel;
                this.SlotType = ESlotType.E_COMP_AIRLOCK;
                this.SubTypeData = 0;
            }

            public override void Tick()
            {
                base.Tick();
                if (!this.IsEquipped && this.ShipStats != null)
                {
                    this.Equip();
                    PLServer.Instance.CaptainChangeItemVisualSlot(this.ShipStats.Ship.ShipID, this.NetID, (int)ESlotType.E_COMP_AIRLOCK);
                }
                if (PLServer.Instance.HasCompletedMissionWithID(8000004) && PLEncounterManager.Instance.GetCPEI() != null && !this.ShipStats.Ship.IsAbandoned())
                {
                    foreach (PLShipInfoBase pLShipInfoBase in PLEncounterManager.Instance.GetCPEI().MyCreatedShipInfos)
                    {
                        bool flag = false;
                        foreach (PLShipComponent pLShipComponent in pLShipInfoBase.MyStats.AllComponents)
                        {
                            if (pLShipComponent is MissionAddOneCPUSlotComp)
                            {
                                flag = true;
                                break;
                            }
                        }
                        if (flag)
                        {
                            this.ShipStats.Ship.SetAbandoned(true);
                            pLShipInfoBase.SetAbandoned(false);
                            if (pLShipInfoBase.MyStats.GetShipComponent<PLShipComponent>(ESlotType.E_COMP_REAC_COOLING) != null)
                                pLShipInfoBase.MyStats.RemoveShipComponent(pLShipInfoBase.MyStats.GetShipComponent<PLShipComponent>(ESlotType.E_COMP_REAC_COOLING));
                            PLServer.Instance.photonView.RPC("ClaimShip", PhotonTargets.All, pLShipInfoBase.ShipID);
                            PhotonNetwork.Destroy(this.ShipStats.Ship.ShipRoot);
                        }
                    }
                }
            }

            public override void FinalLateAddStats(PLShipStats inStats)
            {
                inStats.TurretDamageFactor *= Mathf.Clamp(1.2f - 0.05f * this.SubTypeData, 0.8f, 1.2f);
                inStats.QuantumShieldDefensesActive = inStats.ShieldsCurrent / inStats.ShieldsMax > Mathf.Clamp01(0.1f + 0.2f * this.SubTypeData);
                inStats.ReactorOutputFactor *= Mathf.Clamp(1.2f - 0.05f * this.SubTypeData, 0.8f, 1.2f);
                inStats.HullArmor += Mathf.Clamp(100f - 20f * this.SubTypeData, 0f, 100f) / 250f;
            }

            public override void OnWarp()
            {
                base.OnWarp();
                this.SubTypeData++;
            }
        }

        public class MissionAddOneCPUSlotComp : PLDistressSignal
        {
            public MissionAddOneCPUSlotComp(int inType, int inLevel = 0) : base(EDistressSignalType.UNION, inLevel)
            {
                this.SubType = 6;
                this.Name = "ExGal_AddOneCPUSlot";
                this.Desc = "If you're reading this, I fucked up :(";
                this.CanBeDroppedOnShipDeath = false;
                this.Level = 0;
                this.SlotType = ESlotType.E_COMP_AIRLOCK;
                this.ActualSlotType = ESlotType.E_COMP_AIRLOCK;
            }
        }

        [HarmonyPatch(typeof(PLShipInfoBase), "GetChaosBoost", new Type[2] { typeof(PLPersistantShipInfo), typeof(int) })]
        internal class MissionNoScaling
        {
            private static Exception Finalizer(Exception __exception, PLShipInfoBase __instance, PLPersistantShipInfo inPersistantShipInfo, int offset, ref int __result)
            {
                if (!((UnityEngine.Object)PLServer.Instance != (UnityEngine.Object)null) || inPersistantShipInfo == null)
                    return __exception;
                bool flag = false;
                foreach (ComponentOverrideData data in inPersistantShipInfo.CompOverrides)
                {
                    if (data.CompType == (int)ESlotType.E_COMP_DISTRESS_SIGNAL && data.CompSubType == 6)
                    {
                        flag = true;
                        break;
                    }
                }
                if (flag)
                    __result = 0;
                return __exception;
            }
        }

        public class MissionNoExtractorFlag : PLDistressSignal
        {
            public MissionNoExtractorFlag(int inType, int inLevel = 0) : base(EDistressSignalType.UNION, inLevel)
            {
                this.SubType = 7;
                this.Name = "ExGal_NoExtractor_Flag";
                this.Desc = "If you're reading this, I fucked up :(";
                this.CanBeDroppedOnShipDeath = false;
                this.Level = 0;
                this.SlotType = ESlotType.E_COMP_REAC_COOLING;
                this.ActualSlotType = ESlotType.E_COMP_REAC_COOLING;
            }
        }

        [HarmonyPatch(typeof(PLShipComponent), "GetSalvageSuccessRate")]
        internal class HandleNoExtractorFlag
        {
            private static Exception Finalizer(Exception __exception, PLShipComponent __instance, float successRateFromExtractor, ref float __result)
            {
                if (__instance.ShipStats != null)
                {
                    bool flag = false;
                    foreach (PLShipComponent component in __instance.ShipStats.GetComponentsOfType(ESlotType.E_COMP_AIRLOCK))
                    {
                        if (component is MissionNoExtractorFlag)
                        {
                            flag = true;
                            break;
                        }
                    }
                    if (flag)
                        __result = 0f;
                }
                return __exception;
            }
        }

        [HarmonyPatch(typeof(PLShipStats), "AddShipComponent")]
        internal class HandleSlotBonuses
        {
            private static bool Prefix(PLShipStats __instance, PLShipComponent inComponent, int inNetID, ESlotType visualSlot)
            {
                if (inComponent is MissionAddOneCPUSlotComp && __instance.GetSlot(ESlotType.E_COMP_CPU).MaxItems < 8)
                    ++__instance.GetSlot(ESlotType.E_COMP_CPU).MaxItems;
                return true;
            }

            private static void Postfix(PLShipStats __instance, PLShipComponent inComponent, int inNetID, ESlotType visualSlot)
            {
                if (!__instance.AllComponents.Contains(inComponent))
                    return;
                __instance.AllComponents.Remove(inComponent);
                __instance.AllComponents.Insert(0, inComponent);
            }
        }

        internal class SignalScanner : MissionShipComponentMod
        {
            public override string Name => "Frequency Scanner";

            public override string Description => "A device used to intercept and decrypt signals used for long range communications. It is illegal to have this equipment in your posession.";

            public override int MarketPrice => 3500;

            public override int CargoVisualID => 28;

            public override float Price_LevelMultiplierExponent => 1.2f;

            public override bool Contraband => true;
        }

        [HarmonyPatch(typeof(PLShipInfo), "CreateDefaultItemsForEnemyBotPlayer")]
        internal class MissionShipCrewSpawn
        {
            private static Exception Finalizer(Exception __exception, PLPlayer inPlayer)
            {
                if (!(inPlayer != null) || !(inPlayer.StartingShip != null) || inPlayer.StartingShip.IsRelicHunter || inPlayer.StartingShip.IsBountyHunter)
                    return __exception;
                bool flag = false;
                foreach (PLShipComponent component in inPlayer.StartingShip.MyStats.AllComponents)
                {
                    if (component is MissionShipFlagComp)
                    {
                        flag = true;
                        break;
                    }
                }
                if (!flag)
                    return __exception;
                inPlayer.MyInventory.Clear();
                switch (inPlayer.GetClassID())
                {
                    case 0:
                        inPlayer.MyInventory.UpdateItem(PLServer.Instance.PawnInvItemIDCounter++, 30, 0, 6, 1);
                        break;
                    case 1:
                        inPlayer.MyInventory.UpdateItem(PLServer.Instance.PawnInvItemIDCounter++, 9, 0, 6, 1);
                        break;
                    case 2:
                        inPlayer.MyInventory.UpdateItem(PLServer.Instance.PawnInvItemIDCounter++, 29, 0, 6, 1);
                        inPlayer.MyInventory.UpdateItem(PLServer.Instance.PawnInvItemIDCounter++, 26, 0, 6, 4);
                        break;
                    case 3:
                        inPlayer.MyInventory.UpdateItem(PLServer.Instance.PawnInvItemIDCounter++, 30, 0, 6, 1);
                        inPlayer.MyInventory.UpdateItem(PLServer.Instance.PawnInvItemIDCounter++, 23, 0, 6, 4);
                        break;
                    case 4:
                        inPlayer.MyInventory.UpdateItem(PLServer.Instance.PawnInvItemIDCounter++, 28, 0, 6, 1);
                        break;
                }
                inPlayer.MyInventory.UpdateItem(PLServer.Instance.PawnInvItemIDCounter++, 3, 0, 6, 2);
                inPlayer.MyInventory.UpdateItem(PLServer.Instance.PawnInvItemIDCounter++, 4, 0, 6, 3);
                inPlayer.gameObject.name += "Vulc";
                return __exception;
            }
        }
    }
}

