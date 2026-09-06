using HarmonyLib;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLMissionBase), "CreateObjectiveFromData")]
    internal class AddModdedObjectives
    {
        private static bool Prefix(PLMissionBase __instance, PLMissionBase inMission, ObjectiveData inObjData)
        {
            PLMissionObjective missionObjective = null;
            if (inObjData.ObjType == 21)
            {
                missionObjective = (PLMissionObjective)new PLMissionObjective_CompleteAfterJumpCount(int.Parse(inObjData.GetValueFromKey("CMAJC_Value")));
            }
            else
                return true;
            if (missionObjective == null)
                return true;
            missionObjective.Init();
            missionObjective.RawCustomText = inObjData.GetValueFromKey("CustomText");
            missionObjective.ScriptName = inObjData.GetValueFromKey("ScriptName");
            inMission.Objectives.Add(missionObjective);
            return false;
        }
    }

}

