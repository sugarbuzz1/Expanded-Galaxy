using HarmonyLib;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLStarmap), "Update")]
    internal class CaravanIcon
    {
        private static void Postfix(PLStarmap __instance)
        {
            if (__instance.IsActive && RelicCaravan.CaravanCurrentSector != -1)
            {
                if (PLServer.Instance != null)
                {
                    PLSectorInfo sectorWithId = PLServer.GetSectorWithID(RelicCaravan.CaravanCurrentSector);
                    if (sectorWithId != null)
                    {
                        bool flag = sectorWithId.IsThisSectorWithinPlayerWarpRange() || (bool)PLNetworkManager.Instance.IsInternalBuild;
                        if (flag)
                        {
                            int sectorId;
                            CrewLogManager.Instance.GetPinOfName("CARAVAN", out sectorId);
                            if (sectorId == -1)
                                CrewLogManager.Instance.AddPin("CARAVAN", RelicCaravan.CaravanCurrentSector, Relic.GetRelicColor(), 1);
                            else if (sectorId != RelicCaravan.CaravanCurrentSector)
                                CrewLogManager.Instance.MovePin("CARAVAN", sectorId, RelicCaravan.CaravanCurrentSector);
                            return;
                        }
                    }

                }
            }
            CrewLogManager.Instance.RemovePinOfName("CARAVAN");
        }
    }
}
