using System.Collections.Generic;

namespace ExpandedGalaxy
{
    public class MiningDroneQuestMission
    {
        public static PickupMissionData MissionData => CreateData();

        public static PickupMissionData CreateData()
        {
            PickupMissionData pickupMissionData = new PickupMissionData();
            pickupMissionData.Name = "Ancient Drones";
            pickupMissionData.Desc = "The galaxy is littered with squadrons of extraction vessels, each recieving and relaying data to... somewhere. Leave them be and they will pay you no mind. Though finding their base of operations could prove useful...";
            pickupMissionData.CanBeAbandonedByPlayers = false;
            pickupMissionData.MissionID = 8000015;
            pickupMissionData.CanBeBlockedByOtherPickupMissions = false;
            pickupMissionData.LongRangeDialogueActorID = "";
            pickupMissionData.CallOnStart = false;
            pickupMissionData.BlocksOtherPickupMissionStarts = false;
            pickupMissionData.IsRepeatable = false;
            pickupMissionData.RecCrewLevel = 0;
            pickupMissionData.Rarity = -1f;

            pickupMissionData.OriginalName = "Ancient Drones";
            pickupMissionData.OriginalDesc = "The galaxy is littered with squadrons of extraction vessels, each recieving and relaying data to... somewhere. Leave them be and they will pay you no mind. Though finding their base of operations could prove useful...";

            RequirementData requirementData = new RequirementData();
            requirementData.ReqType = 16;

            List<ObjectiveData> objectiveDatas = new List<ObjectiveData>()
                {
                    new ObjectiveData
                    {
                        ObjType = 1,
                        Data = new Dictionary<string, string>()
                        {
                            {
                                "RST_SectorTypeValue",
                                "LAVA2"
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
                                "Locate the mining drone hub world"
                            }
                        }
                    },
                    new ObjectiveData
                    {
                        ObjType = 9,
                        Data = new Dictionary<string, string>()
                        {
                            {
                                "ScriptName",
                                "ExGal_MiningDrone_Finish"
                            },
                            {
                                "CustomText",
                                "Investigate the source of the drones"
                            },
                            {
                                "RVON_Name",
                                "ExGal_MiningDrone_Volume"
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
                        RewardDataA = 2,
                    }
                };
            pickupMissionData.SuccessRewards.AddRange(rewards);

            return pickupMissionData;
        }
    }
}

