using PulsarModLoader.Content.Components.Missile;
using PulsarModLoader.Content.Components.Turret;
using System.Collections.Generic;

namespace ExpandedGalaxy
{
    public class WDHuntProjVulcanus
    {
        public static PickupMissionData MissionData => CreateData();

        public static PickupMissionData CreateData()
        {
            PickupMissionData pickupMissionData = new PickupMissionData();
            pickupMissionData.Name = "Hunt Project: Vulcanus";
            pickupMissionData.Desc = "I've tracked the crew to this sector. Take over the ship - DON'T DESTROY IT - and bring it to Maes Argale at Dutain's Garage. I'll have another crew handle your ship.";
            pickupMissionData.CanBeAbandonedByPlayers = true;
            pickupMissionData.MissionID = 8000004;
            pickupMissionData.CanBeBlockedByOtherPickupMissions = false;
            pickupMissionData.LongRangeDialogueActorID = "";
            pickupMissionData.LongRangeDialogueDisplayName = "";
            pickupMissionData.LongRangeDialogueDisplayNameOriginal = "";
            pickupMissionData.Rarity = -1f;

            PickupSectorData pickupSectorData = new PickupSectorData();
            pickupSectorData.SectorType = 146;
            pickupSectorData.UniqueType = true;
            pickupSectorData.Distance = 32f;
            pickupSectorData.SpawnRegularShipsToo = false;
            pickupSectorData.FactionID = 2;
            pickupSectorData.Name = "";

            PickupShipData pickupShipData = new PickupShipData();
            pickupShipData.Name = "Project: Vulcanus";
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
                        CompType = (int)ESlotType.E_COMP_REAC_COOLING,
                        CompSubType = 1,
                        CompLevel = 0,
                        IsCargo = false,
                        CompSubTypeToReplace = (int)ESlotType.E_COMP_REAC_COOLING,
                        SlotNumberToReplace = 0,
                        ReplaceExistingComp = false
                    },
                    new ComponentOverrideData()
                    {
                        CompType = (int)ESlotType.E_COMP_REAC_COOLING,
                        CompSubType = 3,
                        CompLevel = 0,
                        IsCargo = false,
                        CompSubTypeToReplace = (int)ESlotType.E_COMP_REAC_COOLING,
                        SlotNumberToReplace = 1,
                        ReplaceExistingComp = false
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
}

