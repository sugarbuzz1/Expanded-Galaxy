using PulsarModLoader.Content.Components.MissionShipComponent;
using System.Collections.Generic;

namespace ExpandedGalaxy
{
    public class JunkCubeWait2
    {
        public static PickupMissionData MissionData => JunkCubeWait2.CreateData();
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
            pickupMissionData.RecCrewLevel = 0;
            pickupMissionData.Rarity = -1f;
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
                                "10"
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
}

