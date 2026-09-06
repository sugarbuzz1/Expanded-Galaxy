using HarmonyLib;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLPersistantEncounterInstance), "PlayMusicBasedOnShipType")]
    internal class MiningDroneMusic
    {
        private static bool Prefix(PLPersistantEncounterInstance __instance, EShipType inType, bool combat)
        {
            if (PLServer.GetCurrentSector() == null || !(PLEncounterManager.Instance.PlayerShip != null) || PLEncounterManager.Instance.PlayerShip.InWarp || PLMusic.Instance.SpecialMusicPlaying || !combat)
                return true;
            bool flag = false;
            foreach (MiningDroneFlag miningDroneFlag in MiningDroneFlag.AllMiningDroneFlags)
            {
                PLShipInfoBase ship = miningDroneFlag.ShipStats.Ship;
                if (ship == null)
                    continue;
                if (ship.IsDrone && !ship.HasBeenDestroyed && ship.AlertLevel == 2 && MiningDroneQuest.dronesActive)
                {
                    if (ship.ShipTypeID == EShipType.E_WDDRONE1 || ship.ShipTypeID == EShipType.E_WDDRONE2)
                    {
                        flag = true;
                        break;
                    }
                }
            }
            if (!flag)
                return true;
            PLMusic.Instance.PlayMusic("mx_infected_attack", true, false, false);
            return false;
        }
    }
}
