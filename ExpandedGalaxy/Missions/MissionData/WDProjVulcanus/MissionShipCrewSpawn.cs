using HarmonyLib;
using System;
using static ExpandedGalaxy.Missions;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLShipInfo), "CreateDefaultItemsForEnemyBotPlayer")]
    internal class MissionShipCrewSpawn
    {
        private static Exception Finalizer(Exception __exception, PLPlayer inPlayer)
        {
            if (!(inPlayer != null) || !(inPlayer.StartingShip != null) || inPlayer.StartingShip.IsRelicHunter || inPlayer.StartingShip.IsBountyHunter)
                return __exception;
            bool flag = false;
            foreach (PLShipComponent component in inPlayer.StartingShip.MyStats.AllComponents)
            {
                if (component is MissionShipFlagComp)
                {
                    flag = true;
                    break;
                }
            }
            if (!flag)
                return __exception;
            inPlayer.MyInventory.Clear();
            switch (inPlayer.GetClassID())
            {
                case 0:
                    inPlayer.MyInventory.UpdateItem(PLServer.Instance.PawnInvItemIDCounter++, 30, 0, 6, 1);
                    break;
                case 1:
                    inPlayer.MyInventory.UpdateItem(PLServer.Instance.PawnInvItemIDCounter++, 9, 0, 6, 1);
                    break;
                case 2:
                    inPlayer.MyInventory.UpdateItem(PLServer.Instance.PawnInvItemIDCounter++, 29, 0, 6, 1);
                    inPlayer.MyInventory.UpdateItem(PLServer.Instance.PawnInvItemIDCounter++, 26, 0, 6, 4);
                    break;
                case 3:
                    inPlayer.MyInventory.UpdateItem(PLServer.Instance.PawnInvItemIDCounter++, 30, 0, 6, 1);
                    inPlayer.MyInventory.UpdateItem(PLServer.Instance.PawnInvItemIDCounter++, 23, 0, 6, 4);
                    break;
                case 4:
                    inPlayer.MyInventory.UpdateItem(PLServer.Instance.PawnInvItemIDCounter++, 28, 0, 6, 1);
                    break;
            }
            inPlayer.MyInventory.UpdateItem(PLServer.Instance.PawnInvItemIDCounter++, 3, 0, 6, 2);
            inPlayer.MyInventory.UpdateItem(PLServer.Instance.PawnInvItemIDCounter++, 4, 0, 6, 3);
            inPlayer.gameObject.name += "Vulc";
            return __exception;
        }
    }
}

