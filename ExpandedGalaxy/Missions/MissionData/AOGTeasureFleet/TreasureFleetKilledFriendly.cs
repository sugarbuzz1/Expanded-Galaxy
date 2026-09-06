using System.Collections.Generic;

namespace ExpandedGalaxy
{
    public class TreasureFleetKilledFriendly
    {
        public static PickupMissionData MissionData => CreateData();

        public static PickupMissionData CreateData()
        {
            PickupMissionData pickupMissionData = new PickupMissionData();
            pickupMissionData.Name = "ExGal_TreasureFleet_Hidden_KilledFriend";
            pickupMissionData.Desc = "";
            pickupMissionData.CanBeAbandonedByPlayers = false;
            pickupMissionData.MissionID = 8000011;
            pickupMissionData.CanBeBlockedByOtherPickupMissions = false;
            pickupMissionData.LongRangeDialogueActorID = "";
            pickupMissionData.LongRangeDialogueDisplayName = "";
            pickupMissionData.LongRangeDialogueDisplayNameOriginal = "";
            pickupMissionData.FailureRequirements_ALLMUSTMATCH = false;
            pickupMissionData.Hidden = true;
            pickupMissionData.Rarity = -1f;

            List<ObjectiveData> objectives = new List<ObjectiveData>()
                {
                    new ObjectiveData()
                    {
                        ObjType = 8,
                        Data = new Dictionary<string, string>()
                        {
                            {
                                "KSN_Name",
                                "The Milano"
                            },
                            {
                                "KSN_AmountNeeded",
                                "1"
                            },
                            {
                                "ScriptName",
                                "ExGal_TreasureFleet_Hidden_KilledFriend"
                            }
                        }
                    },
                    new ObjectiveData()
                    {
                        ObjType = 0,
                        Data = new Dictionary<string, string>()
                        {
                            {
                                "ScriptName",
                                "ExGal_TreasureFleet_Hidden_KilledFriend_Finish"
                            }
                        }
                    }
                };

            List<RequirementData> failureRequirements = new List<RequirementData>()
                {
                    new RequirementData()
                    {
                        ReqType = 13,
                        Data = new Dictionary<string, string>()
                        {
                            {
                                "MissionIDCompleted_ID",
                                "8000008"
                            }
                        }
                    }
                };

            List<RewardData> rewardDatas = new List<RewardData>()
                {
                    new RewardData
                    {
                        RwdType = 1,
                        RewardAmount = 10000,
                    },
                    new RewardData
                    {
                        RwdType = 6,
                        RewardDataA = 1,
                        RewardAmount = -2,
                    }
                };



            pickupMissionData.Objectives.AddRange(objectives);
            pickupMissionData.SuccessRewards.AddRange(rewardDatas);
            pickupMissionData.FailureRequirements.AddRange(failureRequirements);

            return pickupMissionData;
        }
    }
}

