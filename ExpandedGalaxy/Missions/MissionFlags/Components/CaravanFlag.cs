using CodeStage.AntiCheat.ObscuredTypes;
using System.Collections.Generic;
using UnityEngine;

namespace ExpandedGalaxy
{
    public class CaravanFlag : MissionComponentFlag
    {
        public CaravanFlag(int inLevel = 0) : base(6, inLevel)
        {
            this.Name = "ExGal_Caravan_Flag";
            this.CanBeDroppedOnShipDeath = false;
            this.SubTypeData = 0;
        }

        public override void Tick()
        {
            base.Tick();
            if (this.ShipStats != null && this.ShipStats.Ship != null)
            {
                this.ShipStats.Ship.AuxConfig &= (byte)191;
                if (this.SubTypeData != 1)
                {
                    this.ShipStats.Ship.WeaponsSystem.MaxHealth = (ObscuredFloat)40f;
                    this.ShipStats.Ship.WeaponsSystem.Health = (ObscuredFloat)40f;
                    this.ShipStats.Ship.ComputerSystem.MaxHealth = (ObscuredFloat)30f;
                    this.ShipStats.Ship.ComputerSystem.Health = (ObscuredFloat)30f;

                    if (this.ShipStats.Ship is PLShipInfo && ((PLShipInfo)this.ShipStats.Ship).InteriorStatic != null && PLEncounterManager.Instance.GetCPEI() != null)
                    {
                        GameObject gameObject = new GameObject("GroundTurretSpawner");
                        gameObject.transform.SetParent(((PLShipInfo)this.ShipStats.Ship).InteriorStatic.transform);
                        gameObject.layer = 11;
                        gameObject.transform.localPosition = Vector3.zero;
                        if (PhotonNetwork.isMasterClient)
                        {
                            for (int i = 0; i < 27; i++)
                            {
                                GameObject turretSpawner = new GameObject("Spawner");
                                turretSpawner.transform.SetParent(gameObject.transform);
                                Vector3 localPos = Vector3.zero;
                                switch (i)
                                {
                                    case 0:     //Forward Bridge
                                        localPos = new Vector3(-0.1f, 23.4f, 36.6f);
                                        break;
                                    case 1:
                                        localPos = new Vector3(-0.1f, 23.4f, 32);
                                        break;
                                    case 2:     //Rear Bridge
                                        localPos = new Vector3(-2.15f, 25.5f, 10.1f);
                                        break;
                                    case 3:
                                        localPos = new Vector3(1.85f, 25.5f, 10.1f);
                                        break;
                                    case 4:
                                        localPos = new Vector3(-2.15f, 25.5f, 1.45f);
                                        break;
                                    case 5:
                                        localPos = new Vector3(1.85f, 25.5f, 1.45f);
                                        break;
                                    case 6:     //Forward Hallway
                                        localPos = new Vector3(-0.2f, 15.7f, 34.95f);
                                        break;
                                    case 7:
                                        localPos = new Vector3(-0.2f, 15.7f, 25.1f);
                                        break;
                                    case 8:
                                        localPos = new Vector3(-0.2f, 15.7f, 16.05f);
                                        break;
                                    case 9:     //Mid Hallway
                                        localPos = new Vector3(-19.9f, 14.8f, 0.25f);
                                        break;
                                    case 10:
                                        localPos = new Vector3(19.5f, 14.8f, 0.25f);
                                        break;
                                    case 11:
                                        localPos = new Vector3(-19.9f, 14.8f, -29.6f);
                                        break;
                                    case 12:
                                        localPos = new Vector3(19.5f, 14.8f, -29.6f);
                                        break;
                                    case 13:    //Back Hallway
                                        localPos = new Vector3(-6.5f, 16, -44.9f);
                                        break;
                                    case 14:
                                        localPos = new Vector3(6.1f, 16, -44.9f);
                                        break;
                                    case 15:    //Engineering
                                        localPos = new Vector3(-9, 15.8f, -57.2f);
                                        break;
                                    case 16:
                                        localPos = new Vector3(8.6f, 15.8f, -57.2f);
                                        break;
                                    case 17:
                                        localPos = new Vector3(-9, 15.8f, -76);
                                        break;
                                    case 18:
                                        localPos = new Vector3(8.6f, 15.8f, -76);
                                        break;
                                    case 19:    //Cargo
                                        localPos = new Vector3(-9.7f, 3.3f, -2.4f);
                                        break;
                                    case 20:
                                        localPos = new Vector3(9.5f, 3.3f, -2.4f);
                                        break;
                                    case 21:
                                        localPos = new Vector3(-9.7f, 3.3f, -13.8f);
                                        break;
                                    case 22:
                                        localPos = new Vector3(9.5f, 3.3f, -13.8f);
                                        break;
                                    case 23:
                                        localPos = new Vector3(-9.7f, 3.3f, -25.1f);
                                        break;
                                    case 24:
                                        localPos = new Vector3(9.5f, 3.3f, -25.1f);
                                        break;
                                    case 25:
                                        localPos = new Vector3(-9.7f, 3.3f, -36.4f);
                                        break;
                                    case 26:
                                        localPos = new Vector3(9.5f, 3.3f, -36.4f);
                                        break;
                                }
                                turretSpawner.transform.localPosition = localPos;
                                PLSpawner spawner = turretSpawner.AddComponent<PLSpawner>();
                                spawner.Spawn = "GroundTurret";
                                spawner.Parameters = new List<SpawnParameter> { new SpawnParameter() { Name = "TargetingType", Value = "AllPlayers" } };
                                spawner.SpawnedEnemyRespawnTime_Seconds = float.MaxValue;
                            }
                        }
                    }

                    this.ShipStats.Ship.FactionID = 6;
                    if (PhotonNetwork.isMasterClient)
                    {
                        if (PLMissionObjective.IDIsCompleted("ExGal_TheMap_Decipher"))
                            this.ShipStats.AddShipComponent(new PLMissionShipComponent(8));
                        foreach (PLPlayer player in PLServer.Instance.AllPlayers)
                        {
                            if (player != null && player.StartingShip == this.ShipStats.Ship)
                                player.FactionID = (ObscuredInt)6;
                        }
                    }
                    this.SubTypeData = 1;
                }
                if (PLEncounterManager.Instance.GetCPEI() != null && (double)PLEncounterManager.Instance.GetCPEI().GetTimePlayerInEncounterAfterWarp() > 5.0 && this.ShipStats.Ship.TeamID == -1 || this.ShipStats.Ship.TeamID == 0)
                    this.ShipStats.Ship.TakeDamage(500000f, false, EDamageType.E_PHYSICAL, 1f, -1, null, -1);
                if (true || PLMissionObjective.IDIsCompleted("ExGal_TheMap_Hidden_TalkCaravan"))
                {
                    if (this.ShipStats.Ship.PersistantShipInfo != null)
                        this.ShipStats.Ship.PersistantShipInfo.ForcedHostile = true;
                    if (PLEncounterManager.Instance.PlayerShip != null && !PLEncounterManager.Instance.PlayerShip.InWarp && this.ShipStats.Ship.AlertLevel > 0 && PLMusic.Instance.CurrentPlayingMusicEventString != "mx_drone_commander_lp")
                        PLMusic.Instance.PlayMusic("mx_drone_commander_lp", true, false, true, false);
                }
            }
        }

        public override void Unequip()
        {
            if (this.ShipStats != null && this.ShipStats.HullCurrent < 1f && this.ShipStats.Ship != null && this.ShipStats.Ship.PersistantShipInfo != null)
            {
                this.ShipStats.Ship.PersistantShipInfo.IsShipDestroyed = true;
                if (PLMusic.Instance.CurrentPlayingMusicEventString == "mx_drone_commander_lp")
                    PLMusic.Instance.StopCurrentMusic();
                if (PLMissionObjective.IDIsCompleted("ExGal_TheMap_Hidden_TalkCaravan"))
                    PLMissionObjective_Custom.OnCustomObjEvent("ExGal_TheMap_Return");
            }
            base.Unequip();
        }

        public override void Equip()
        {
            this.ShipStats?.SetSlotLimit(ESlotType.E_COMP_TURRET, 6);
            List<Transform> transforms = new List<Transform>();
            transforms.AddRange(this.ShipStats.Ship.RegularTurretPoints);
            for (int i = 0; i < 4; i++)
            {
                GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.ShipStats.Ship.RegularTurretPoints[0].gameObject);
                gameObject.transform.SetParent(this.ShipStats.Ship.Exterior.transform);
                Vector3 localPos;
                Quaternion localRot;
                switch (i)
                {
                    case 1:
                        localPos = new Vector3(27.091f, 24.6881f, -2.5418f);
                        localRot = Quaternion.Euler(0, 0, 315);
                        break;
                    case 2:
                        localPos = new Vector3(-34.2565f, 12.3871f, 42.0181f);
                        localRot = Quaternion.Euler(0, 0, 90);
                        break;
                    case 3:
                        localPos = new Vector3(32.9565f, 12.1371f, 41.9181f);
                        localRot = Quaternion.Euler(0, 0, 270);
                        break;
                    default:
                        localPos = new Vector3(-27.091f, 24.6881f, -2.5418f);
                        localRot = Quaternion.Euler(0, 0, 45);
                        break;
                }
                gameObject.transform.localPosition = localPos;
                gameObject.transform.localRotation = localRot;
                transforms.Add(gameObject.transform);
            }
            this.ShipStats.Ship.RegularTurretPoints = transforms.ToArray();
            base.Equip();
        }
    }

}

