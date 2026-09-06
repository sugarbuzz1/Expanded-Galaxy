using HarmonyLib;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLServer), "OnChaosEventActivate")]
    internal class OnChaosEventActivatePatch
    {
        private static bool Prefix(PLServer __instance, EChaosEvent chaosEvent)
        {
            bool flag = false;
            switch (chaosEvent)
            {
                case EChaosEvent.E_INFECTION_BOOST:
                case EChaosEvent.E_WD_OFFENSIVE:
                    flag = true;
                    break;
                case EChaosEvent.E_LONG_RANGE_PRICE_HIKE:
                    foreach (PLSectorInfo pLSectorInfo in PLGlobal.Instance.Galaxy.AllSectorInfos.Values)
                    {
                        if (pLSectorInfo.VisualIndication == ESectorVisualIndication.WARP_NETWORK_STATION && pLSectorInfo.MySPI.Faction == 0 && pLSectorInfo.IsPartOfLongRangeWarpNetwork)
                        {
                            pLSectorInfo.MySPI.Faction = 1;
                            pLSectorInfo.IsPartOfLongRangeWarpNetwork = false;
                        }
                        else if ((pLSectorInfo.VisualIndication == ESectorVisualIndication.COLONIAL_HUB || pLSectorInfo.VisualIndication == ESectorVisualIndication.FLUFFY_FACTORY_01 || pLSectorInfo.VisualIndication == ESectorVisualIndication.FLUFFY_FACTORY_02 || pLSectorInfo.VisualIndication == ESectorVisualIndication.FLUFFY_FACTORY_03) && pLSectorInfo.IsPartOfLongRangeWarpNetwork)
                            pLSectorInfo.IsPartOfLongRangeWarpNetwork = false;
                    }
                    break;
            }
            return flag;
        }
    }
}
