using System.Collections.Generic;

namespace ExpandedGalaxy
{
    public class InterludeStart
    {
        public static PickupMissionData MissionData => CreateData();

        public static PickupMissionData CreateData()
        {
            PickupMissionData pickupMissionData = new PickupMissionData();
            pickupMissionData.Name = "The Summon";
            pickupMissionData.Desc = "[PLAYERSHIP_NAME], I have come across something rather interesting that might pique your interest. It appears to be some sort of map that we picked up from the wreckage of a ship I do not recognize. Next time we cross paths I would like for you to have a look at it.";
            pickupMissionData.CanBeAbandonedByPlayers = false;
            pickupMissionData.MissionID = 8000017;
            pickupMissionData.CanBeBlockedByOtherPickupMissions = false;
            pickupMissionData.LongRangeDialogueActorID = "ExGal_RelicCaravan_Summon";
            pickupMissionData.LongRangeDialogueDisplayName = "Wandering Caravan";
            pickupMissionData.LongRangeDialogueDisplayNameOriginal = "Wandering Caravan";
            pickupMissionData.CallOnStart = true;
            pickupMissionData.BlocksOtherPickupMissionStarts = false;
            pickupMissionData.IsRepeatable = false;
            pickupMissionData.RecCrewLevel = 0;
            pickupMissionData.Rarity = -1f;

            pickupMissionData.OriginalName = "The Summon";
            pickupMissionData.OriginalDesc = "[PLAYERSHIP_NAME], I have come across something rather interesting that might pique your interest. It appears to be some sort of map that we picked up from the wreckage of a ship I do not recognize. Next time we cross paths I would like for you to have a look at it.";

            pickupMissionData.StartingRequirements = new List<RequirementData>()
                {
                    new RequirementData()
                    {
                        ReqType = 5,
                        Data = new Dictionary<string, string>()
                        {
                            {
                                "MissionIDCompleted_ID",
                                "8000016"
                            }
                        }
                    },
                    new RequirementData()
                    {
                        ReqType = 5,
                        Data = new Dictionary<string, string>()
                        {
                            {
                                "MissionIDCompleted_ID",
                                "8000014"
                            }
                        }
                    },
                    new RequirementData()
                    {
                        ReqType = 5,
                        Data = new Dictionary<string, string>()
                        {
                            {
                                "MissionIDCompleted_ID",
                                "8000015"
                            }
                        }
                    }
                };
            pickupMissionData.StartingRequirements_ALLMUSTMATCH = true;

            List<ObjectiveData> objectiveDatas = new List<ObjectiveData>()
                {
                    new ObjectiveData
                    {
                        ObjType = 0,
                        Data = new Dictionary<string, string>()
                        {
                            {
                                "ScriptName",
                                "ExGal_Interlude_Return"
                            },
                            {
                                "CustomText",
                                "Return to the Caravan"
                            }
                        }
                    },
                };
            pickupMissionData.Objectives.AddRange(objectiveDatas);

            return pickupMissionData;
        }
    }
}

