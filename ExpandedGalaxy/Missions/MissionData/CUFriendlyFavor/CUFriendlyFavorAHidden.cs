using PulsarModLoader.Content.Components.MissionShipComponent;
using System.Collections.Generic;

namespace ExpandedGalaxy
{
    public class CUFriendlyFavorAHidden
    {
        public static PickupMissionData MissionData => CreateData();

        public static PickupMissionData CreateData()
        {
            PickupMissionData pickupMissionData = new PickupMissionData();
            pickupMissionData.Name = "ExGal_FF_Hidden_A";
            pickupMissionData.Desc = "";
            pickupMissionData.CanBeAbandonedByPlayers = false;
            pickupMissionData.MissionID = 8000007;
            pickupMissionData.CanBeBlockedByOtherPickupMissions = false;
            pickupMissionData.LongRangeDialogueActorID = "";
            pickupMissionData.LongRangeDialogueDisplayName = "";
            pickupMissionData.LongRangeDialogueDisplayNameOriginal = "";
            pickupMissionData.FailureRequirements_ALLMUSTMATCH = false;
            pickupMissionData.Hidden = true;
            pickupMissionData.Rarity = -1f;

            List<ObjectiveData> objectiveDatas = new List<ObjectiveData>()
                {
                    new ObjectiveData
                    {
                        ObjType = 2,
                        Data = new Dictionary<string, string>()
                        {
                            {
                                "ScriptName",
                                "ExGal_FF_Hidden_A"
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
                                "False"
                            },
                            {
                                "CustomText",
                                ""
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
                        RewardDataB = MissionShipComponentModManager.Instance.GetMissionShipComponentIDFromName("Frequency Scanner"),
                        RewardAmount = 0,
                    },
                };

            List<RequirementData> failureRequirements = new List<RequirementData>()
                {
                    new RequirementData()
                    {
                        ReqType = 18,
                        Data = new Dictionary<string, string>()
                        {
                            {
                                "MissionIDInProgress_ID",
                                "8000007"
                            }
                        }
                    }
                };

            pickupMissionData.Objectives.AddRange(objectiveDatas);
            pickupMissionData.StartingRewards.AddRange(rewardDatas);
            pickupMissionData.FailureRequirements.AddRange(failureRequirements);

            return pickupMissionData;
        }
    }
}

