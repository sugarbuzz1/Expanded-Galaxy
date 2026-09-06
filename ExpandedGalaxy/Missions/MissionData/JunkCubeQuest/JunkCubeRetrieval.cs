using System.Collections.Generic;

namespace ExpandedGalaxy
{
    public class JunkCubeRetrieval
    {
        public static PickupMissionData MissionData => JunkCubeRetrieval.CreateData();
        private static PickupMissionData CreateData()
        {
            PickupMissionData pickupMissionData = new PickupMissionData();
            pickupMissionData.Name = "Recover Junk Cube";
            pickupMissionData.Desc = "I was wondering when you'd return for that. Unfortunatly some bandits took off with it while we were stopped at the Burrow. They kept shouting \"Praise be the Cube,\" whatever that means. It's safe to say I am never taking my business there again. I've tracked them to this sector, bring it back to me so I can finish looking it over and get you your reward.";
            pickupMissionData.CanBeAbandonedByPlayers = false;
            pickupMissionData.MissionID = 8000002;
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
                        ObjType = 1,
                        Data = new Dictionary<string, string>()
                        {
                            {
                                "CustomText",
                                "Reach the marked sector"
                            },
                            {
                                "RST_SectorTypeValue",
                                "16"
                            },
                            {
                                "RST_MustKillAll",
                                "0"
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
                                "ExGal_GotJunkCube2"
                            },
                            {
                                "CustomText",
                                "Retrieve the Junk Cube"
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
                                "ExGal_DeliverJunkCube2"
                            },
                            {
                                "CustomText",
                                "Deliver the Junk Cube back to the Caravan"
                            }
                        }
                    },
                };
            PickupSectorData sectorData = new PickupSectorData();
            sectorData.SectorType = (int)ESectorVisualIndication.CANYON;
            sectorData.Distance = 36f;
            sectorData.SpawnRegularShipsToo = false;
            sectorData.UniqueType = false;
            sectorData.Name = "";
            pickupMissionData.Sectors.Add(sectorData);
            pickupMissionData.Objectives.AddRange(objectiveDatas);
            return pickupMissionData;
        }
    }
}

