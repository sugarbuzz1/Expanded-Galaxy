using HarmonyLib;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLStarmap), "ShouldShowSector")]
    internal class SectorOnStarMap
    {
        private static void Postfix(PLSectorInfo sectorInfo, ref bool __result)
        {
            if (sectorInfo == null)
                return;
            if (sectorInfo.VisualIndication == ESectorVisualIndication.ALCHEMIST && sectorInfo.MySPI.Faction == 5)
                __result = true;
            if (sectorInfo.VisualIndication == ESectorVisualIndication.LAVA2)
            {
                __result = false;
                if (sectorInfo.Visited || PLServer.Instance.m_ShipCourseGoals.Contains(sectorInfo.ID))
                    __result = true;
                if (PLEncounterManager.Instance != null && PLEncounterManager.Instance.PlayerShip != null && PLEncounterManager.Instance.PlayerShip.DistressSignalActive)
                {
                    PLDistressSignal component = PLEncounterManager.Instance.PlayerShip.MyStats.GetComponentFromNetID<PLDistressSignal>(PLEncounterManager.Instance.PlayerShip.SelectedDistressSignalNetID);
                    if (component != null && component.SubType == 4 && sectorInfo.IsThisSectorWithinPlayerWarpRange())
                    {
                        __result = true;
                        return;
                    }
                }
            }
        }
    }
}
