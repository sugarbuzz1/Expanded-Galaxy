using System.Collections.Generic;

namespace ExpandedGalaxy
{
    public class JunkCubeWait3
    {
        public static PickupMissionData MissionData => CreateData();
        private static PickupMissionData CreateData()
        {
            PickupMissionData pickupMissionData = new PickupMissionData();
            pickupMissionData.Name = "ExGal_JunkCube_WaitHidden";
            pickupMissionData.Desc = "";
            pickupMissionData.CanBeAbandonedByPlayers = false;
            pickupMissionData.MissionID = 8000016;
            pickupMissionData.CanBeBlockedByOtherPickupMissions = false;
            pickupMissionData.LongRangeDialogueActorID = "";
            pickupMissionData.LongRangeDialogueDisplayName = "";
            pickupMissionData.LongRangeDialogueDisplayNameOriginal = "";
            pickupMissionData.RecCrewLevel = 0;
            pickupMissionData.Rarity = -1f;
            pickupMissionData.Hidden = true;
            List<ObjectiveData> objectiveDatas = new List<ObjectiveData>()
                {
                    new ObjectiveData
                    {
                        ObjType = 21,
                        Data = new Dictionary<string, string>()
                        {
                            {
                                "ScriptName",
                                "ExGal_WaitJunkCube3"
                            },
                            {
                                "CMAJC_Value",
                                "10"
                            }
                        }
                    }
                };
            pickupMissionData.Objectives.AddRange(objectiveDatas);
            return pickupMissionData;
        }
    }
}

