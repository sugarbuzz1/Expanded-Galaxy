using System.Collections.Generic;

namespace ExpandedGalaxy
{
    public class TheMap
    {
        public static PickupMissionData MissionData => CreateData();

        public static PickupMissionData CreateData()
        {
            PickupMissionData pickupMissionData = new PickupMissionData();
            pickupMissionData.Name = "The Map";
            pickupMissionData.Desc = "Ah yes, the map. We found it in the wreckage of a ship. Poor bastard warped into a sector with some shock drones. I would go investigate it myself but I've got a business to uphold here. Go check it out and come back with your findings.";
            pickupMissionData.CanBeAbandonedByPlayers = false;
            pickupMissionData.MissionID = 8000018;
            pickupMissionData.CanBeBlockedByOtherPickupMissions = false;
            pickupMissionData.LongRangeDialogueActorID = "";
            pickupMissionData.CallOnStart = false;
            pickupMissionData.BlocksOtherPickupMissionStarts = false;
            pickupMissionData.IsRepeatable = false;
            pickupMissionData.RecCrewLevel = 0;
            pickupMissionData.Rarity = -1f;

            pickupMissionData.OriginalName = "The Map";
            pickupMissionData.OriginalDesc = "Ah yes, the map. We found it in the wreckage of a ship. Poor bastard warped into a sector with some shock drones. I would go investigate it myself but I've got a business to uphold here. Go check it out and come back with your findings.";

            List<ObjectiveData> objectiveDatas = new List<ObjectiveData>()
                {
                    new ObjectiveData
                    {
                        ObjType = 0,
                        Data = new Dictionary<string, string>()
                        {
                            {
                                "ScriptName",
                                "ExGal_TheMap_Decipher"
                            },
                            {
                                "CustomText",
                                "Decipher the contents of the map"
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
                                "ExGal_TheMap_Return"
                            },
                            {
                                "CustomText",
                                "Return to the Caravan"
                            }
                        }
                    },
                };
            pickupMissionData.Objectives.AddRange(objectiveDatas);

            List<RewardData> rewards = new List<RewardData>()
                {
                    new RewardData()
                    {
                        RwdType = 9,
                        RewardDataA = 3,
                    }
                };
            pickupMissionData.StartingRewards.AddRange(rewards);

            List<RewardData> rewards2 = new List<RewardData>()
                {
                    new RewardData()
                    {
                        RwdType = 10,
                        RewardDataA = 4,
                    }
                };
            pickupMissionData.SuccessRewards.AddRange(rewards2);

            return pickupMissionData;
        }
    }
}

