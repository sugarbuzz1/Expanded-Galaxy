using PulsarModLoader.Content.Items;
using System.Collections.Generic;

namespace ExpandedGalaxy
{
    public class CUFriendlyFavorB
    {
        public static PickupMissionData MissionData => CreateData();

        public static PickupMissionData CreateData()
        {
            PickupMissionData pickupMissionData = new PickupMissionData();
            pickupMissionData.Name = "Friendly Favor";
            pickupMissionData.Desc = "I've set up a deal for a frequency scanner so I can intercept transmissions to the command center. Transport it for me and I'll make sure you are well rewarded.";
            pickupMissionData.CanBeAbandonedByPlayers = true;
            pickupMissionData.MissionID = 8000006;
            pickupMissionData.CanBeBlockedByOtherPickupMissions = false;
            pickupMissionData.LongRangeDialogueActorID = "";
            pickupMissionData.LongRangeDialogueDisplayName = "";
            pickupMissionData.LongRangeDialogueDisplayNameOriginal = "";
            pickupMissionData.Rarity = -1f;

            PickupSectorData pickupSectorData = new PickupSectorData();
            pickupSectorData.SectorType = 146;
            pickupSectorData.UniqueType = true;
            pickupSectorData.Distance = 16f;
            pickupSectorData.SpawnRegularShipsToo = false;
            pickupSectorData.FactionID = 1;
            pickupSectorData.Name = "";

            PickupShipData pickupShipData = new PickupShipData();
            pickupShipData.Name = "The Milano";
            pickupShipData.RandomizeName = true;
            pickupShipData.DropDefaultCredits = false;
            pickupShipData.CreditsToDrop = 0;
            pickupShipData.DialogueActorID = "ExGal_ContactFF";
            pickupShipData.Flagged = false;
            pickupShipData.ForceHostileAgainstPlayerShip = false;
            pickupShipData.ShipType = (int)EShipType.E_STARGAZER;
            pickupShipData.ImmediatelyStartDialogue = true;
            pickupShipData.ForceHostileAgainstAll = false;
            pickupShipData.ForceHostileAgainstName = "";

            pickupSectorData.Ships.Add(pickupShipData);

            pickupMissionData.Sectors.Add(pickupSectorData);

            List<ObjectiveData> objectiveDatas = new List<ObjectiveData>()
                {
                    new ObjectiveData
                    {
                        ObjType = 1,
                        Data = new Dictionary<string, string>()
                        {
                            {
                                "RST_SectorTypeValue",
                                "146"
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
                                "Reach the marked sector"
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
                                "ExGal_MeetContactFF"
                            },
                            {
                                "CustomText",
                                "Meet with the contact"
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
                                "ExGal_ReturnFF"
                            },
                            {
                                "CustomText",
                                "Return to Ena Sekra in the apartments at Outpost 448"
                            }
                        }
                    }
                };
            ItemModManager.Instance.GetItemIDsFromName("Auto Rifle", out int MainType, out int SubType);
            List<RewardData> rewardDatas = new List<RewardData>()
                {
                    new RewardData
                    {
                        RwdType = 2,
                        RewardDataA = MainType,
                        RewardDataB = SubType,
                        RewardAmount = 2,
                    },
                    new RewardData
                    {
                        RwdType = 2,
                        RewardDataA = MainType,
                        RewardDataB = SubType,
                        RewardAmount = 2,
                    },
                    new RewardData
                    {
                        RwdType = 4,
                        RewardAmount = 2
                    },
                    new RewardData
                    {
                        RwdType = 1,
                        RewardAmount = 2000
                    },
                };

            pickupMissionData.Objectives.AddRange(objectiveDatas);
            pickupMissionData.SuccessRewards.AddRange(rewardDatas);

            return pickupMissionData;
        }
    }
}

