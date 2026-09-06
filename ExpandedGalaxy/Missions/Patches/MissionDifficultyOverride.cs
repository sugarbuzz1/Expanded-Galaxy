using HarmonyLib;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLTabMenu), "GetDifficultyForMission")]
    internal class MissionDifficultyOverride
    {
        private static void Postfix(PLTabMenu __instance, PLTabMenu.MissionDisplay md, ref int __result)
        {
            if (PLServer.Instance == null || md.Mission == null)
                return;
            switch (md.Mission.MyMissionData.MissionID)
            {
                case 8000000:
                case 8000001:
                case 8000002:
                case 8000003:
                case 8000014:
                case 8000015:
                case 8000017:
                case 8000018:
                    __result = 4;
                    break;
            }
        }
    }

}

