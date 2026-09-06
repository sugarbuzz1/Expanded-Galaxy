using System.Collections.Generic;

namespace ExpandedGalaxy
{
    public class TreasureFleetRewardA
    {
        public static PickupMissionData MissionData => CreateData();

        public static PickupMissionData CreateData()
        {
            PickupMissionData pickupMissionData = new PickupMissionData();
            pickupMissionData.Name = "ExGal_TreasureFleet_Hidden_A";
            pickupMissionData.Desc = "";
            pickupMissionData.CanBeAbandonedByPlayers = false;
            pickupMissionData.MissionID = 8000009;
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
                        RwdType = 1,
                        RewardAmount = 20000,
                    },
                    new RewardData
                    {
                        RwdType = 6,
                        RewardDataA = 1,
                        RewardAmount = 1,
                    }
                };

            pickupMissionData.SuccessRewards.AddRange(rewardDatas);

            return pickupMissionData;
        }
    }
}

