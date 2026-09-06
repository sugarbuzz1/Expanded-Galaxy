using PulsarModLoader.Content.Components.MissionShipComponent;
using System.Collections.Generic;

namespace ExpandedGalaxy
{
    public class TreasureFleet
    {
        public static PickupMissionData MissionData => CreateData();

        public static PickupMissionData CreateData()
        {
            PickupMissionData pickupMissionData = new PickupMissionData();
            pickupMissionData.Name = "Treasure Fleet";
            pickupMissionData.Desc = "";
            pickupMissionData.CanBeAbandonedByPlayers = false;
            pickupMissionData.MissionID = 8000008;
            pickupMissionData.CanBeBlockedByOtherPickupMissions = false;
            pickupMissionData.LongRangeDialogueActorID = "";
            pickupMissionData.LongRangeDialogueDisplayName = "";
            pickupMissionData.LongRangeDialogueDisplayNameOriginal = "";
            pickupMissionData.Rarity = -1f;

            List<ObjectiveData> objectiveDatas = new List<ObjectiveData>()
                {
                    new ObjectiveData
                    {
                        ObjType = 0,
                        Data = new Dictionary<string, string>()
                        {
                            {
                                "ScriptName",
                                "ExGal_TreasureFleet_Intercept"
                            },
                            {
                                "CustomText",
                                "Intercept W.D. Fleet"
                            },
                            {
                                "ExGal_NPC_SectorCurrent",
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
                                "ExGal_TreasureFleet_Cargo"
                            },
                            {
                                "PC_ComponentType",
                                "E_COMP_MISSION_COMPONENT"
                            },
                            {
                                "PC_SubType",
                                MissionShipComponentModManager.Instance.GetMissionShipComponentIDFromName("Irradiated Cargo").ToString()
                            },
                            {
                                "PC_AmountNeeded",
                                "16"
                            },
                            {
                                "PC_RemoveComponents",
                                "True"
                            },
                            {
                                "CustomText",
                                "Board and retrieve cargo"
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
                                "ExGal_TreasureFleet_Deliver"
                            },
                            {
                                "CustomText",
                                "Bring the shipment to Kadew Rufara in the cargo hold of the Estate"
                            },
                        }
                    },
                };

            pickupMissionData.Objectives.AddRange(objectiveDatas);

            return pickupMissionData;
        }
    }
}

