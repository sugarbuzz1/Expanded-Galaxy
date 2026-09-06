using HarmonyLib;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLWarpStation), "GetPriceForSectorID")]
    internal class IHateHardmode_WarpStationFix
    {
        private static bool Prefix(PLWarpStation __instance, int inSectorID, ref int __result)
        {
            if (inSectorID != -1 && PLGlobal.Instance.Galaxy.AllSectorInfos.ContainsKey(inSectorID) && PLGlobal.Instance.Galaxy.AllSectorInfos[inSectorID].MySPI != null && PLGlobal.Instance.Galaxy.AllSectorInfos[inSectorID].MySPI.Faction == 6)
            {
                __result = 0;
                return false;
            }
            return true;
        }
    }
}
