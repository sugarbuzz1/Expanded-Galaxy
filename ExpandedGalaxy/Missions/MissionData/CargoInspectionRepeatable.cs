using System.Collections.Generic;

namespace ExpandedGalaxy
{
    public class CargoInspectionRepeatable
    {
        public static PickupMissionData MissionData => CreateData();

        public static PickupMissionData CreateData()
        {
            PickupMissionData pickupMissionData = new PickupMissionData();
            pickupMissionData.Name = "Cargo Inspection";
            pickupMissionData.Desc = "You have been selected for a cargo inspection. Please redirect your course to the nearest inspection station, and you will be compensated for your time. Noncompliance is considered to be a criminal offense and will be reported to the Outpost 448 Command Center. We apologize for the inconvenience and thank you for your cooperation.";
            pickupMissionData.CanBeAbandonedByPlayers = true;
            pickupMissionData.MissionID = 8000013;
            pickupMissionData.CanBeBlockedByOtherPickupMissions = false;
            pickupMissionData.LongRangeDialogueActorID = "ExGal_Inspection_Comms";
            pickupMissionData.LongRangeDialogueDisplayName = "Inspection Station";
            pickupMissionData.LongRangeDialogueDisplayNameOriginal = "Inspection Station";
            pickupMissionData.CallOnStart = true;
            pickupMissionData.BlocksOtherPickupMissionStarts = true;
            pickupMissionData.FailureRequirements_ALLMUSTMATCH = true;
            pickupMissionData.IsRepeatable = true;
            pickupMissionData.Rarity = -1f;

            PickupSectorData pickupSectorData = new PickupSectorData();
            pickupSectorData.SectorType = 78;
            pickupSectorData.UniqueType = false;
            pickupSectorData.Distance = 1f;
            pickupSectorData.SpawnRegularShipsToo = false;
            pickupSectorData.FactionID = 0;
            pickupSectorData.Name = "";

            pickupMissionData.Sectors.Add(pickupSectorData);

            RequirementData requirementData = new RequirementData();
            requirementData.ReqType = 26;
            pickupMissionData.StartingRequirements.Add(requirementData);

            List<ObjectiveData> objectiveDatas = new List<ObjectiveData>()
                {
                    new ObjectiveData
                    {
                        ObjType = 1,
                        Data = new Dictionary<string, string>()
                        {
                            {
                                "RST_SectorTypeValue",
                                "CONTRABAND_STATION"
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
                                "Go to the inspection station"
                            }
                        }
                    },
                    new ObjectiveData
                    {
                        ObjType = 10,
                        Data = new Dictionary<string, string>()
                        {
                            {
                                "CMIJC_Value",
                                "2"
                            },
                        }
                    },
                };
            pickupMissionData.Objectives.AddRange(objectiveDatas);

            List<RewardData> rewardDatasSucceed = new List<RewardData>()
                {
                    new RewardData
                    {
                        RwdType = 1,
                        RewardAmount = 1000,
                    },
                };
            pickupMissionData.SuccessRewards.AddRange(rewardDatasSucceed);

            List<RewardData> rewardDatasFailure = new List<RewardData>()
                {
                    new RewardData
                    {
                        RwdType = 6,
                        RewardAmount = -3,
                        RewardDataA = 0,
                    },
                    new RewardData
                    {
                        RwdType = 5,
                        RewardAmount = 1,
                        RewardDataD = 0.1f
                    },
                    new RewardData
                    {
                        RwdType = 7,
                        RewardAmount = 1
                    }
                };
            pickupMissionData.FailureRewards.AddRange(rewardDatasFailure);

            return pickupMissionData;
        }
    }
}

