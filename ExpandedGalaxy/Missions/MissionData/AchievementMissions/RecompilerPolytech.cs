using System.Collections.Generic;

namespace ExpandedGalaxy
{
    public class RecompilerPolytech
    {
        public static PickupMissionData MissionData => CreateData();

        public static PickupMissionData CreateData()
        {
            PickupMissionData pickupMissionData = new PickupMissionData();
            pickupMissionData.Name = "ExGal_RecompilerPolytech_Hidden";
            pickupMissionData.Desc = "";
            pickupMissionData.Hidden = true;
            pickupMissionData.CanBeAbandonedByPlayers = false;
            pickupMissionData.MissionID = 8000021;
            pickupMissionData.CanBeBlockedByOtherPickupMissions = false;
            pickupMissionData.LongRangeDialogueActorID = "";
            pickupMissionData.CallOnStart = false;
            pickupMissionData.BlocksOtherPickupMissionStarts = false;
            pickupMissionData.IsRepeatable = false;
            pickupMissionData.RecCrewLevel = 0;
            pickupMissionData.Rarity = -1f;

            pickupMissionData.OriginalName = "ExGal_RecompilerPolytech_Hidden";
            pickupMissionData.OriginalDesc = "";

            List<ObjectiveData> objectiveDatas = new List<ObjectiveData>()
                {
                    new ObjectiveData
                    {
                        ObjType = 5,
                        Data = new Dictionary<string, string>()
                        {
                            {
                                "KST_ShipType",
                                "E_POLYTECH_SHIP"
                            },
                            {
                                "KST_AmountNeeded",
                                "5"
                            }
                        }
                    },
                    new ObjectiveData
                    {
                        ObjType = 8,
                        Data = new Dictionary<string, string>()
                        {
                            {
                                "KSN_Name",
                                "The Recompiler: Config. 1"
                            },
                            {
                                "KSN_AmountNeeded",
                                "1"
                            }
                        }
                    },
                    new ObjectiveData
                    {
                        ObjType = 8,
                        Data = new Dictionary<string, string>()
                        {
                            {
                                "KSN_Name",
                                "The Recompiler: Config. 2"
                            },
                            {
                                "KSN_AmountNeeded",
                                "1"
                            }
                        }
                    },
                    new ObjectiveData
                    {
                        ObjType = 8,
                        Data = new Dictionary<string, string>()
                        {
                            {
                                "KSN_Name",
                                "The Recompiler: Config. 3"
                            },
                            {
                                "KSN_AmountNeeded",
                                "1"
                            }
                        }
                    },
                    new ObjectiveData
                    {
                        ObjType = 8,
                        Data = new Dictionary<string, string>()
                        {
                            {
                                "KSN_Name",
                                "The Recompiler: Config. 4"
                            },
                            {
                                "KSN_AmountNeeded",
                                "1"
                            }
                        }
                    },
                    new ObjectiveData
                    {
                        ObjType = 8,
                        Data = new Dictionary<string, string>()
                        {
                            {
                                "KSN_Name",
                                "The Recompiler: Config. 5"
                            },
                            {
                                "KSN_AmountNeeded",
                                "1"
                            }
                        }
                    }
                };
            pickupMissionData.Objectives.AddRange(objectiveDatas);

            List<RewardData> rewards = new List<RewardData>()
                {
                    new RewardData()
                    {
                        RwdType = 10,
                        RewardDataA = 1,
                    }
                };
            pickupMissionData.SuccessRewards.AddRange(rewards);

            return pickupMissionData;
        }
    }
}

