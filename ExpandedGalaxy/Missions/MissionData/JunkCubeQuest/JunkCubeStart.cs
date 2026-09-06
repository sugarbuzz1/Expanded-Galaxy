using System.Collections.Generic;

namespace ExpandedGalaxy
{
    public class JunkCubeStart
    {
        public static PickupMissionData MissionData => CreateData();
        private static PickupMissionData CreateData()
        {
            PickupMissionData pickupMissionData = new PickupMissionData();
            pickupMissionData.Name = "\"Special\" Offer";
            pickupMissionData.Desc = "I'm looking for something. On the outside it looks like a worthless hunk of garbage, but it's actully considered a high deity to some denizens of the galaxy. If you can find it and bring it to me, I will make it worth your while.";
            pickupMissionData.CanBeAbandonedByPlayers = false;
            pickupMissionData.MissionID = 8000000;
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
                        ObjType = 2,
                        Data = new Dictionary<string, string>()
                        {
                            {
                                "ScriptName",
                                "ExGal_GotJunkCube1"
                            },
                            {
                                "CustomText",
                                "Find the Junk Cube"
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
                                "true"
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
                                "ExGal_DeliverJunkCube1"
                            },
                            {
                                "CustomText",
                                "Deliver the Junk Cube to the Caravan"
                            }
                        }
                    },
                };
            pickupMissionData.Objectives.AddRange(objectiveDatas);
            return pickupMissionData;
        }
    }
}

