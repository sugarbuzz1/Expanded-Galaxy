using CodeStage.AntiCheat.ObscuredTypes;
using HarmonyLib;
using UnityEngine;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLShipInfo), "CreateDefaultItemsForEnemyBotPlayer")]
    internal class CaravanCrewSpawn
    {
        private static void Postfix(PLPlayer inPlayer)
        {
            if (!((UnityEngine.Object)inPlayer != (UnityEngine.Object)null) || !((UnityEngine.Object)inPlayer.StartingShip != (UnityEngine.Object)null))
                return;
            if (inPlayer.StartingShip.PersistantShipInfo == null || !(inPlayer.StartingShip.PersistantShipInfo.ShipName == "Wandering Caravan" && inPlayer.StartingShip.PersistantShipInfo.Type == EShipType.E_ROLAND && inPlayer.StartingShip.PersistantShipInfo.SelectedActorID == "ExGal_RelicCaravan"))
                return;
            inPlayer.Talents[(int)ETalents.HEALTH_BOOST] = (ObscuredInt)5;
            inPlayer.Talents[(int)ETalents.PISTOL_DMG_BOOST] = (ObscuredInt)3;
            inPlayer.Talents[(int)ETalents.QUICK_RESPAWN] = (ObscuredInt)4;
            inPlayer.Talents[(int)ETalents.ARMOR_BOOST] = (ObscuredInt)5;
            inPlayer.Talents[(int)ETalents.WPN_AMMO_BOOST] = (ObscuredInt)50;
            switch (inPlayer.GetClassID())
            {
                case 0: //CAP
                    inPlayer.Talents[(int)ETalents.CAP_CREW_SPEED_BOOST] = (ObscuredInt)5;
                    inPlayer.Talents[(int)ETalents.CAP_ARMOR_BOOST] = (ObscuredInt)0 + Mathf.RoundToInt(Mathf.Floor(PLServer.Instance.ChaosLevel / 3) * PLGlobal.Instance.Galaxy.GenerationSettings.EnemyShipPowerScalar);
                    inPlayer.Talents[(int)ETalents.CAP_SCREEN_DEFENSE] = (ObscuredInt)5 + Mathf.RoundToInt(Mathf.Floor(PLServer.Instance.ChaosLevel / 3) * PLGlobal.Instance.Galaxy.GenerationSettings.EnemyShipPowerScalar);
                    inPlayer.Talents[(int)ETalents.CAP_SCREEN_SAFETY] = (ObscuredInt)5 + Mathf.RoundToInt(Mathf.Floor(PLServer.Instance.ChaosLevel / 3) * PLGlobal.Instance.Galaxy.GenerationSettings.EnemyShipPowerScalar);
                    break;
                case 1:  //PI
                    inPlayer.Talents[(int)ETalents.PIL_SHIP_SPEED] = (ObscuredInt)5;
                    inPlayer.Talents[(int)ETalents.PIL_SHIP_TURNING] = (ObscuredInt)5;
                    inPlayer.Talents[(int)ETalents.PIL_REDUCE_SYS_DAMAGE] = (ObscuredInt)18;
                    inPlayer.Talents[(int)ETalents.PIL_REDUCE_HULL_DAMAGE] = (ObscuredInt)2 + Mathf.RoundToInt(Mathf.Floor(PLServer.Instance.ChaosLevel / 3) * PLGlobal.Instance.Galaxy.GenerationSettings.EnemyShipPowerScalar);
                    break;
                case 2: //SCI
                    inPlayer.Talents[(int)ETalents.SCI_SENSOR_BOOST] = (ObscuredInt)5;
                    inPlayer.Talents[(int)ETalents.SCI_SENSOR_HIDE] = (ObscuredInt)1 + Mathf.RoundToInt(Mathf.Floor(PLServer.Instance.ChaosLevel / 3) * PLGlobal.Instance.Galaxy.GenerationSettings.EnemyShipPowerScalar);
                    inPlayer.Talents[(int)ETalents.SCI_HEAL_NEARBY] = (ObscuredInt)5;
                    break;
                case 3: //WEAP
                    inPlayer.Talents[(int)ETalents.WPNS_MISSILE_EXPERT] = (ObscuredInt)1 + Mathf.RoundToInt(Mathf.Floor(PLServer.Instance.ChaosLevel / 3) * PLGlobal.Instance.Galaxy.GenerationSettings.EnemyShipPowerScalar);
                    inPlayer.Talents[(int)ETalents.WPNS_COOLING] = (ObscuredInt)5;
                    inPlayer.Talents[(int)ETalents.WPNS_REDUCE_PAWN_DMG] = (ObscuredInt)2 + Mathf.RoundToInt(Mathf.Floor(PLServer.Instance.ChaosLevel / 3) * PLGlobal.Instance.Galaxy.GenerationSettings.EnemyShipPowerScalar);
                    inPlayer.Talents[(int)ETalents.WPNS_BOOST_CREW_TURRET_CHARGE] = (ObscuredInt)2 + Mathf.RoundToInt(Mathf.Floor(PLServer.Instance.ChaosLevel / 3) * PLGlobal.Instance.Galaxy.GenerationSettings.EnemyShipPowerScalar);
                    inPlayer.Talents[(int)ETalents.WPNS_BOOST_CREW_TURRET_DAMAGE] = (ObscuredInt)2 + Mathf.RoundToInt(Mathf.Floor(PLServer.Instance.ChaosLevel / 3) * PLGlobal.Instance.Galaxy.GenerationSettings.EnemyShipPowerScalar);
                    inPlayer.Talents[(int)ETalents.WPN_SCREEN_HACKER] = (ObscuredInt)2 + Mathf.RoundToInt(Mathf.Floor(PLServer.Instance.ChaosLevel / 3) * PLGlobal.Instance.Galaxy.GenerationSettings.EnemyShipPowerScalar);
                    inPlayer.Talents[(int)ETalents.E_TURRET_COOLING_CREW_WEAPONS] = (ObscuredInt)5;
                    break;
                case 4: //ENG
                    inPlayer.Talents[(int)ETalents.ENG_FIRE_REDUCTION] = (ObscuredInt)5;
                    inPlayer.Talents[(int)ETalents.ENG_COOLANT_MIX_CUSTOM] = (ObscuredInt)5;
                    inPlayer.Talents[(int)ETalents.ENG_REPAIR_DRONES] = (ObscuredInt)5 + Mathf.RoundToInt(Mathf.Floor(PLServer.Instance.ChaosLevel / 3) * PLGlobal.Instance.Galaxy.GenerationSettings.EnemyShipPowerScalar);
                    inPlayer.Talents[(int)ETalents.ENG_COREPOWERBOOST] = (ObscuredInt)50;
                    inPlayer.Talents[(int)ETalents.ENG_CORECOOLINGBOOST] = (ObscuredInt)2 + Mathf.RoundToInt(Mathf.Floor(PLServer.Instance.ChaosLevel / 3) * PLGlobal.Instance.Galaxy.GenerationSettings.EnemyShipPowerScalar);
                    inPlayer.Talents[(int)ETalents.E_TURRET_COOLING_CREW_ENGINEER] = (ObscuredInt)5;
                    break;
            }
            inPlayer.MyInventory.Clear();
            switch (inPlayer.GetClassID())
            {
                case 0:
                    inPlayer.MyInventory.UpdateItem(PLServer.Instance.PawnInvItemIDCounter++, 9, 0, 5, 1);
                    break;
                case 1:
                    inPlayer.MyInventory.UpdateItem(PLServer.Instance.PawnInvItemIDCounter++, 10, 0, 5, 1);
                    break;
                case 2:
                    inPlayer.MyInventory.UpdateItem(PLServer.Instance.PawnInvItemIDCounter++, 11, 0, 5, 1);
                    break;
                case 3:
                    inPlayer.MyInventory.UpdateItem(PLServer.Instance.PawnInvItemIDCounter++, 12, 0, 5, 1);
                    break;
                case 4:
                    inPlayer.MyInventory.UpdateItem(PLServer.Instance.PawnInvItemIDCounter++, 2, 0, 5, 1);
                    break;
            }

            if (inPlayer.GetClassID() != 4)
            {
                int inNetID1 = PLServer.Instance.PawnInvItemIDCounter++;
                inPlayer.MyInventory.UpdateItem(inNetID1, 3, 0, 5, 2);
            }
            else
            {
                int inNetID3 = PLServer.Instance.PawnInvItemIDCounter++;
                inPlayer.MyInventory.UpdateItem(inNetID3, 23, 0, 5, 2);
            }
            int inNetID2 = PLServer.Instance.PawnInvItemIDCounter++;
            inPlayer.MyInventory.UpdateItem(inNetID2, 4, 0, 5, 3);
            if (inPlayer.GetClassID() == 0)
            {
                inPlayer.MyInventory.UpdateItem(PLServer.Instance.PawnInvItemIDCounter++, 13, 0, 0, 4);
            }
            inPlayer.gameObject.name += " ExGal";
        }
    }
}
