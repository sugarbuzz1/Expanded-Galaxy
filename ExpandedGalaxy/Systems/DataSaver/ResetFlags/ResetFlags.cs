namespace ExpandedGalaxy
{
    internal class ResetFlags
    {
        internal static void OnNewGame()
        {
            PFSectorCommander.bossFlag = 0;
            Relic.RelicData = 0U;
            PlanetCompPickups.PickupQueue.Clear();
            MiningDroneQuest.dronesActive = true;
            MiningDroneQuest.GXData = 0;
            SyncCheck.checkedPlayers.Clear();
            RelicCaravan.CaravanCurrentSector = -1;
            RelicCaravan.CaravanTargetSector = -1;
            RelicCaravan.CaravanUpdateTime = 60000;
            RelicCaravan.ClearCaravanPath();
            ReflectedRift.riftData = 0;
            Missions.pickupMissionDelay = 0;
            PersistantScrapManager.Instance.ClearData();
            CrewLogManager.Instance.OnNewGame();
            PlanetObjectSetupManager.Instance.OnNewGame();
        }
    }
}
