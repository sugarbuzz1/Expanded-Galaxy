using PulsarModLoader.Content.Components.Extractor;
using System.Collections.Generic;

namespace ExpandedGalaxy
{
    public class TreasureFleetRewardB
    {
        public static PickupMissionData MissionData => CreateData();

        public static PickupMissionData CreateData()
        {
            PickupMissionData pickupMissionData = new PickupMissionData();
            pickupMissionData.Name = "ExGal_TreasureFleet_Hidden_B";
            pickupMissionData.Desc = "";
            pickupMissionData.CanBeAbandonedByPlayers = false;
            pickupMissionData.MissionID = 8000010;
            pickupMissionData.CanBeBlockedByOtherPickupMissions = false;
            pickupMissionData.LongRangeDialogueActorID = "";
            pickupMissionData.LongRangeDialogueDisplayName = "";
            pickupMissionData.LongRangeDialogueDisplayNameOriginal = "";
            pickupMissionData.FailureRequirements_ALLMUSTMATCH = false;
            pickupMissionData.Hidden = true;
            pickupMissionData.Rarity = -1f;

            List<RewardData> rewardDatas = new List<RewardData>()
                {
                    new RewardData
                    {
                        RwdType = 3,
                        RewardDataA = (int)ESlotType.E_COMP_SALVAGE_SYSTEM,
                        RewardDataB = ExtractorModManager.Instance.GetExtractorIDFromName("P.T. Extractor Prototype"),
                        RewardAmount = 0,
                    },
                    new RewardData
                    {
                        RwdType = 6,
                        RewardDataA = 1,
                        RewardAmount = 1,
                    }
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
                                "8000010"
                            }
                        }
                    }
                };

            pickupMissionData.SuccessRewards.AddRange(rewardDatas);
            pickupMissionData.FailureRequirements.AddRange(failureRequirements);

            return pickupMissionData;
        }
    }
}

