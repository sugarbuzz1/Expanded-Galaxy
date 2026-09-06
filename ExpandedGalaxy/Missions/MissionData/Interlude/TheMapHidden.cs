using PulsarModLoader.Content.Items;
using System.Collections.Generic;

namespace ExpandedGalaxy
{
    public class TheMapHidden
    {
        public static PickupMissionData MissionData => CreateData();

        public static PickupMissionData CreateData()
        {
            PickupMissionData pickupMissionData = new PickupMissionData();
            pickupMissionData.Name = "ExGal_TheMap_Hidden";
            pickupMissionData.Desc = "";
            pickupMissionData.CanBeAbandonedByPlayers = false;
            pickupMissionData.MissionID = 8000019;
            pickupMissionData.CanBeBlockedByOtherPickupMissions = false;
            pickupMissionData.LongRangeDialogueActorID = "";
            pickupMissionData.CallOnStart = false;
            pickupMissionData.BlocksOtherPickupMissionStarts = false;
            pickupMissionData.IsRepeatable = false;
            pickupMissionData.RecCrewLevel = 0;
            pickupMissionData.Rarity = -1f;
            pickupMissionData.Hidden = true;

            pickupMissionData.OriginalName = "ExGal_TheMap_Hidden";
            pickupMissionData.OriginalDesc = "";

            List<ObjectiveData> objectiveDatas = new List<ObjectiveData>()
                {
                    new ObjectiveData
                    {
                        ObjType = 13,
                        Data = new Dictionary<string, string>()
                        {
                            {
                                "ScriptName",
                                "ExGal_TheMap_Hidden_UnlockDoor"
                            },
                            {
                                "DPO_Name",
                                ""
                            },
                            {
                                "DPO_AmountNeeded",
                                "1"
                            },
                            {
                                "CustomText",
                                ""
                            },
                        }
                    },
                    new ObjectiveData
                    {
                        ObjType = 0,
                        Data = new Dictionary<string, string>()
                        {
                            {
                                "ScriptName",
                                "ExGal_TheMap_Hidden_TalkCaravan"
                            },
                            {
                                "CustomText",
                                ""
                            }
                        }
                    },
                    new ObjectiveData
                    {
                        ObjType = 2,
                        Data = new Dictionary<string, string>()
                        {
                            {
                                "ScriptName",
                                "ExGal_TheMap_Hidden_GetCube"
                            },
                            {
                                "CustomText",
                                ""
                            },
                            {
                                "PC_ComponentType",
                                "E_COMP_MISSION_COMPONENT"
                            },
                            {
                                "PC_SubType",
                                "8"
                            },
                            {
                                "PC_CompName",
                                "Junk Cube"
                            },
                            {
                                "PC_AmountNeeded",
                                "1"
                            },
                            {
                                "PC_RemoveComponents",
                                "false"
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
                                "ExGal_TheMap_Hidden_ReturnCube"
                            },
                            {
                                "CustomText",
                                ""
                            }
                        }
                    },
                };
            pickupMissionData.Objectives.AddRange(objectiveDatas);
            ItemModManager.Instance.GetItemIDsFromName("Warp Key", out int MainType, out int Subtype);
            List<RewardData> rewards = new List<RewardData>()
                {
                    new RewardData()
                    {
                        RwdType = 2,
                        RewardDataA = MainType,
                        RewardDataB = Subtype,
                        RewardAmount = 0,
                    }
                };
            pickupMissionData.SuccessRewards.AddRange(rewards);
            return pickupMissionData;
        }
    }
}

