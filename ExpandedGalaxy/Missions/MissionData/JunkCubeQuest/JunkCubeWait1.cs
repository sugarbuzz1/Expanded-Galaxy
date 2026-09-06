using System.Collections.Generic;

namespace ExpandedGalaxy
{
    public class JunkCubeWait1
    {
        public static PickupMissionData MissionData => CreateData();
        private static PickupMissionData CreateData()
        {
            PickupMissionData pickupMissionData = new PickupMissionData();
            pickupMissionData.Name = "Junk Processing";
            pickupMissionData.Desc = "Incredible! I'm suprised you were able to find it in a galaxy so vast... assuming what you've brought me is authentic that is! I'll need some time to make sure what you brought me isn't actually junk. Come find me later and I'll give you your reward.";
            pickupMissionData.CanBeAbandonedByPlayers = false;
            pickupMissionData.MissionID = 8000001;
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
                        ObjType = 21,
                        Data = new Dictionary<string, string>()
                        {
                            {
                                "ScriptName",
                                "ExGal_WaitJunkCube1"
                            },
                            {
                                "CMAJC_Value",
                                "10"
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
                                "ExGal_WaitJunkCubeReturn1"
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

