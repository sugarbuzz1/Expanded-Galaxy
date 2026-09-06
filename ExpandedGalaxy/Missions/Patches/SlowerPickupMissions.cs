using HarmonyLib;
using System.Collections.Generic;
using System.Text;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLServer), "AttemptToAddPickupMission")]
    internal class SlowerPickupMissions
    {
        private static bool Prefix(PLServer __instance)
        {
            if (Missions.slowMissionPickups)
            {
                if (Missions.pickupMissionDelay == 0)
                {
                    Missions.pickupMissionDelay = UnityEngine.Random.Range(1, 4);
                    return true;
                }
                else
                {
                    bool flag = false;
                    List<string> blockingMissions = new List<string>();
                    foreach (PLMissionBase allMission in __instance.AllMissions)
                    {
                        if (allMission != null && !allMission.Ended && !allMission.Abandoned && allMission.IsPickupMission && allMission.MyMissionData != null && allMission.MyMissionData is PickupMissionData missionData && missionData.BlocksOtherPickupMissionStarts)
                        {
                            flag = true;
                            blockingMissions.Add(missionData.Name);
                        }
                    }
                    if (!flag)
                        --Missions.pickupMissionDelay;
                    else
                    {
                        StringBuilder stringBuilder = new StringBuilder();
                        for (int i = 0; i < blockingMissions.Count; i++)
                            stringBuilder.Append(blockingMissions[i] + ", ");
                        PulsarModLoader.Utilities.Logger.Info("Found Blocking Missions: " + stringBuilder.ToString());
                    }
                    return false;
                }
            }
            return true;
        }
    }

}

