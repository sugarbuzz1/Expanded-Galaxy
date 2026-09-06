using HarmonyLib;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLScientistComputerScreen), "SetupGeneralInfoResult")]
    internal class HideDroneInfos
    {
        private static bool Prefix(ref string ___CurrentSearchString)
        {
            int result = -1;
            if (!int.TryParse(___CurrentSearchString, out result) || PLGlobal.Instance.AllGeneralInfos == null || result < 0 || PLGlobal.Instance.AllGeneralInfos.Count <= result)
                return false;
            if ((PLGlobal.Instance.AllGeneralInfos[result].Name == "Mining Drone" || PLGlobal.Instance.AllGeneralInfos[result].Name == "Escort Drone") && MiningDroneQuest.GXData < 1)
                return false;
            else if (PLGlobal.Instance.AllGeneralInfos[result].Name == "Guardian Drone" && MiningDroneQuest.GXData < 2)
                return false;
            return true;
        }
    }
}
