using HarmonyLib;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLTeleportationLocationInstance), "ShouldBeUsable")]
    internal class DisableHubTeleporter
    {
        private static bool TeleporterActive(PLSectorInfo sectorInfo)
        {
            if (sectorInfo.VisualIndication == ESectorVisualIndication.LAVA2 && PLEncounterManager.Instance != null && PLEncounterManager.Instance.PlayerShip != null && !PLEncounterManager.Instance.PlayerShip.InWarp && PLEncounterManager.Instance.GetCPEI() != null)
            {
                bool flag = false;
                foreach (MiningDroneBossFlag miningDroneBossFlag in MiningDroneBossFlag.AllMiningDroneBossFlags)
                {
                    PLShipInfoBase plShipInfoBase = miningDroneBossFlag.ShipStats.Ship;
                    if (plShipInfoBase == null || plShipInfoBase.HasBeenDestroyed)
                    {
                        continue;
                    }
                    flag = true;
                    break;
                }
                return !flag;
            }
            return true;
        }

        private static bool Prefix(PLTeleportationLocationInstance __instance, ref bool __result)
        {
            if (PLEncounterManager.Instance != null && PLEncounterManager.Instance.PlayerShip != null && !PLEncounterManager.Instance.PlayerShip.InWarp)
            {
                if (__instance.MyShipInfo != null)
                    return true;
                if (PLServer.Instance != null)
                {
                    if (TeleporterActive(PLServer.GetCurrentSector()))
                        return true;
                    else
                    {
                        __result = false;
                        return false;
                    }
                }
            }
            return true;
        }
    }
}
