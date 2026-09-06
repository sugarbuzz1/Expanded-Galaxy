using System;
using System.Collections.Generic;
using System.Globalization;

namespace ExpandedGalaxy
{
    internal class ChaosEvents
    {
        internal static List<int> DeactivateAllChaosEvents()
        {
            List<int> events = new List<int>();
            for (int i = 0; i < (int)EChaosEvent.MAX; i++)
            {
                if (PLServer.Instance.IsChaosEventActive((EChaosEvent)i))
                {
                    OnChaosEventDeactivate((EChaosEvent)i);
                    events.Add(i);
                }
                    
            }
            PLServer.Instance.ActiveChaosEvents = 0L;
            return events;
        }

        internal static void OnChaosEventDeactivate(EChaosEvent chaosEvent)
        {
            switch(chaosEvent)
            {
                case EChaosEvent.E_INFECTION_BOOST:
                    PLServer.Instance.FactionStrengthLevels[4] /= 4;
                    foreach(PLFactionInfo info in PLGlobal.Instance.Galaxy.AllFactions)
                    {
                        if (info.FactionID == 4)
                        {
                            info.FactionAI_Continuous_GalaxySpreadLimit /= 1.5f;
                            info.FactionAI_Continuous_GalaxySpreadFactor /= 1.85f;
                            break;
                        }
                    }
                    break;
                case EChaosEvent.E_WD_OFFENSIVE:
                    PLServer.Instance.FactionStrengthLevels[2] /= 4;
                    foreach (PLFactionInfo info in PLGlobal.Instance.Galaxy.AllFactions)
                    {
                        if (info.FactionID == 2)
                        {
                            info.FactionAI_Continuous_GalaxySpreadLimit /= 2f;
                            info.FactionAI_Continuous_GalaxySpreadFactor /= 6f;
                            break;
                        }
                    }
                    break;
                case EChaosEvent.E_LONG_RANGE_PRICE_HIKE:
                    foreach (PLSectorInfo pLSectorInfo in PLGlobal.Instance.Galaxy.AllSectorInfos.Values)
                    {
                        if (pLSectorInfo.VisualIndication == ESectorVisualIndication.WARP_NETWORK_STATION && pLSectorInfo.MySPI.Faction == 1 && !pLSectorInfo.IsPartOfLongRangeWarpNetwork)
                        {
                            pLSectorInfo.MySPI.Faction = 0;
                            pLSectorInfo.IsPartOfLongRangeWarpNetwork = true;
                        }
                        else if ((pLSectorInfo.VisualIndication == ESectorVisualIndication.COLONIAL_HUB || pLSectorInfo.VisualIndication == ESectorVisualIndication.FLUFFY_FACTORY_01 || pLSectorInfo.VisualIndication == ESectorVisualIndication.FLUFFY_FACTORY_02 || pLSectorInfo.VisualIndication == ESectorVisualIndication.FLUFFY_FACTORY_03) && !pLSectorInfo.IsPartOfLongRangeWarpNetwork)
                            pLSectorInfo.IsPartOfLongRangeWarpNetwork = true;
                    }
                    break;
            }
        }

        internal static void CreateLRDAForChaosEvent(EChaosEvent chaosEvent)
        {
            string actorName = "";
            switch (chaosEvent)
            {
                case EChaosEvent.E_FUEL_SHORTAGE:
                    actorName = "ExGal_ChaosEvent_FuelShortage";
                    break;
                case EChaosEvent.E_COOLANT_SHORTAGE:
                    actorName = "ExGal_ChaosEvent_CoolantShortage";
                    break;
                case EChaosEvent.E_CALM:
                    actorName = "ExGal_ChaosEvent_Contraband";
                    break;
                case EChaosEvent.E_SHOP_STRIKE:
                    actorName = "ExGal_ChaosEvent_ShopStrike";
                    break;
                case EChaosEvent.E_INFECTION_BOOST:
                    actorName = "ExGal_ChaosEvent_InfectionBoost";
                    break;
                case EChaosEvent.E_WD_OFFENSIVE:
                    actorName = "ExGal_ChaosEvent_WDOffensive";
                    break;
                case EChaosEvent.E_SHOCK_DRONE_INVASION:
                    actorName = "ExGal_ChaosEvent_ShockDrones";
                    break;
                case EChaosEvent.E_DEATHSEEKER_DRONE_INVASION:
                    actorName = "ExGal_ChaosEvent_Deathseekers";
                    break;
                case EChaosEvent.E_PHASE_DRONE_INVASION:
                    actorName = "ExGal_ChaosEvent_PhaseDrones";
                    break;
                case EChaosEvent.E_LONG_RANGE_PRICE_HIKE:
                    actorName = "ExGal_ChaosEvent_LongRangeDisable";
                    break;
            }
            if (actorName != "")
                PLServer.CreateBasicLongRangeDialogueActor(actorName, "CU Information Desk");
        }

        internal static bool CanActivateChaosEvent(EChaosEvent chaosEvent)
        {
            if (PLServer.Instance == null)
                return false;
            switch (chaosEvent)
            {
                case EChaosEvent.E_FUEL_SHORTAGE:
                    return (double)(float)PLServer.Instance.ChaosLevel >= 2.0;
                case EChaosEvent.E_COOLANT_SHORTAGE:
                    return (double)(float)PLServer.Instance.ChaosLevel >= 1.0;
                case EChaosEvent.E_CALM: //Contraband Inspections
                    return (double)(float)PLServer.Instance.ChaosLevel >= 1.0;
                case EChaosEvent.E_SHOP_STRIKE:
                    return (double)(float)PLServer.Instance.ChaosLevel >= 3.0;
                case EChaosEvent.E_INFECTION_BOOST:
                    return (double)(float)PLServer.Instance.ChaosLevel >= 3.0;
                case EChaosEvent.E_WD_OFFENSIVE:
                    return (double)(float)PLServer.Instance.ChaosLevel >= 3.0;
                case EChaosEvent.E_SHOCK_DRONE_INVASION:
                    return (double)(float)PLServer.Instance.ChaosLevel >= 3.0;
                case EChaosEvent.E_DEATHSEEKER_DRONE_INVASION:
                    return (double)(float)PLServer.Instance.ChaosLevel >= 1.0;
                case EChaosEvent.E_PHASE_DRONE_INVASION:
                    return (double)(float)PLServer.Instance.ChaosLevel >= 2.0;
                case EChaosEvent.E_LONG_RANGE_PRICE_HIKE: //Long Range Warp Disable
                    return (double)(float)PLServer.Instance.ChaosLevel >= 2.0;
                default:
                    return false;
            }
        }
    }
}
