using System.Collections.Generic;

namespace ExpandedGalaxy
{
    public class BadBiscuits
    {
        internal static TraderPersistantDataEntry carrierData = new TraderPersistantDataEntry();

        public static PickupMissionData MissionData => CreateData();

        public static PickupMissionData CreateData()
        {
            PickupMissionData pickupMissionData = new PickupMissionData();
            pickupMissionData.Name = "Bad Biscuits";
            pickupMissionData.Desc = "Eliminate the target and then return to me. If you get caught this meeting never happened.";
            pickupMissionData.CanBeAbandonedByPlayers = true;
            pickupMissionData.MissionID = 8000012;
            pickupMissionData.CanBeBlockedByOtherPickupMissions = false;
            pickupMissionData.LongRangeDialogueActorID = "";
            pickupMissionData.LongRangeDialogueDisplayName = "";
            pickupMissionData.LongRangeDialogueDisplayNameOriginal = "";
            pickupMissionData.Rarity = -1f;

            PickupSectorData pickupSectorData = new PickupSectorData();
            pickupSectorData.SectorType = 0;
            pickupSectorData.UniqueType = false;
            pickupSectorData.Distance = 22f;
            pickupSectorData.SpawnRegularShipsToo = true;
            pickupSectorData.FactionID = 3;
            pickupSectorData.Name = "";

            PickupShipData pickupShipData = new PickupShipData();
            pickupShipData.Name = "Sugary Speeders";
            pickupShipData.RandomizeName = false;
            pickupShipData.DropDefaultCredits = true;
            pickupShipData.CreditsToDrop = 2000;
            pickupShipData.DialogueActorID = "";
            pickupShipData.Flagged = false;
            pickupShipData.ForceHostileAgainstPlayerShip = false;
            pickupShipData.ShipType = (int)EShipType.E_FLUFFY_RIVAL_GENERIC;
            pickupShipData.ImmediatelyStartDialogue = false;
            pickupShipData.ForceHostileAgainstAll = false;
            pickupShipData.ForceHostileAgainstName = "";

            pickupSectorData.Ships.Add(pickupShipData);

            pickupMissionData.Sectors.Add(pickupSectorData);

            List<ObjectiveData> objectives = new List<ObjectiveData>()
                {
                    new ObjectiveData()
                    {
                        ObjType = 8,
                        Data = new Dictionary<string, string>()
                        {
                            {
                            "ScriptName",
                            "ExGal_BadBiscuit_Kill"
                            },
                            {
                                "KSN_Name",
                                "Sugary Speeders"
                            },
                            {
                                "KSN_AmountNeeded",
                                "1"
                            }
                        }
                    },
                    new ObjectiveData()
                    {
                        ObjType = 0,
                        Data = new Dictionary<string, string>()
                        {
                            {
                                "ScriptName",
                                "ExGal_BadBiscuit_Return"
                            },
                            {
                                "CustomText",
                                "Return to Zalsman Sini at the Burrow"
                            }
                        }
                    }
                };

            pickupMissionData.Objectives.AddRange(objectives);

            return pickupMissionData;
        }
    }
}

