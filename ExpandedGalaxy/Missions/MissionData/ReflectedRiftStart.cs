using System.Collections.Generic;

namespace ExpandedGalaxy
{
    public class ReflectedRiftStart
    {
        public static PickupMissionData MissionData => CreateData();

        public static PickupMissionData CreateData()
        {
            PickupMissionData pickupMissionData = new PickupMissionData();
            pickupMissionData.Name = "A Rift in Space";
            pickupMissionData.Desc = "Urgent message from the Outpost 448 Command Center: [PLAYERSHIP_NAME], your crew has been deemed appropriate for a unique assingment. Please make your way to the provided coordinates as fast as you please. A Union vessel will be waiting there to brief you on the assignment.";
            pickupMissionData.CanBeAbandonedByPlayers = false;
            pickupMissionData.MissionID = 8000014;
            pickupMissionData.CanBeBlockedByOtherPickupMissions = false;
            pickupMissionData.LongRangeDialogueActorID = "ExGal_ReflectedRift_Start";
            pickupMissionData.LongRangeDialogueDisplayName = "CU Information Desk";
            pickupMissionData.LongRangeDialogueDisplayNameOriginal = "CU Information Desk";
            pickupMissionData.CallOnStart = true;
            pickupMissionData.BlocksOtherPickupMissionStarts = false;
            pickupMissionData.IsRepeatable = false;
            pickupMissionData.RecCrewLevel = 0;
            pickupMissionData.Rarity = 1f;

            pickupMissionData.OriginalName = "A Rift in Space";
            pickupMissionData.OriginalDesc = "Urgent message from the Outpost 448 Command Center: [PLAYERSHIP_NAME], your crew has been deemed appropriate for a unique assingment. Please make your way to the provided coordinates as fast as you please. A Union vessel will be waiting there to brief you on the assignment.";

            PickupSectorData pickupSectorData = new PickupSectorData();
            pickupSectorData.SectorType = 93;
            pickupSectorData.UniqueType = false;
            pickupSectorData.Distance = 60f;
            pickupSectorData.SpawnRegularShipsToo = false;
            pickupSectorData.FactionID = 6;
            pickupSectorData.Name = "Spacial Rift";

            PickupShipData pickupShipData = new PickupShipData();
            pickupShipData.Name = "";
            pickupShipData.DialogueActorID = "ExGal_ReflectedRift_NPC";
            pickupShipData.ImmediatelyStartDialogue = true;
            pickupShipData.ForceHostileAgainstName = "";
            pickupShipData.ShipType = 23;
            pickupSectorData.Ships.Add(pickupShipData);

            pickupMissionData.Sectors.Add(pickupSectorData);

            pickupMissionData.StartingRequirements = new List<RequirementData>()
                {
                    new RequirementData()
                    {
                        ReqType = 3,
                        Data = new Dictionary<string, string>()
                        {
                            {
                                "ChaosLevelMin_Level",
                                "2"
                            }
                        }
                    }
                };
            pickupMissionData.StartingRequirements_ALLMUSTMATCH = true;

            List<ObjectiveData> objectiveDatas = new List<ObjectiveData>()
                {
                    new ObjectiveData
                    {
                        ObjType = 1,
                        Data = new Dictionary<string, string>()
                        {
                            {
                                "RST_SectorTypeValue",
                                "DIMENSION_STATION"
                            },
                            {
                                "RST_DestNameValue",
                                "Spacial Rift"
                            },
                            {
                                "RST_MustKillAll",
                                "0"
                            },
                            {
                                "CustomText",
                                "Go to the marked sector"
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
                                "ExGal_ReflectedRift_Enter"
                            },
                            {
                                "CustomText",
                                "Enter the rift"
                            }
                        }
                    },
                    new ObjectiveData
                    {
                        ObjType = 13,
                        Data = new Dictionary<string, string>()
                        {
                            {
                                "ScriptName",
                                "ExGal_ReflectedRift_Finsh"
                            },
                            {
                                "DPO_Name",
                                ""
                            },
                            {
                                "DPO_AmountNeeded",
                                "1"
                            },
                            {
                                "CustomText",
                                "Unravel the Anomaly"
                            }
                        }
                    },
                };
            pickupMissionData.Objectives.AddRange(objectiveDatas);

            List<RewardData> rewards = new List<RewardData>()
                {
                    new RewardData()
                    {
                        RwdType = 10,
                        RewardDataA = 3,
                    }
                };
            pickupMissionData.SuccessRewards.AddRange(rewards);

            return pickupMissionData;
        }
    }
}

