using System.Collections.Generic;
using UnityEngine;

namespace ExpandedGalaxy
{
    public class MiningDroneFlag : MissionComponentFlag
    {
        private float LastVirusTime = float.MinValue;
        private GameObject currentAsteroid;
        private static bool GaveMission = false;
        public static List<MiningDroneFlag> AllMiningDroneFlags = new List<MiningDroneFlag>();
        public MiningDroneFlag(int inLevel = 0) : base(0, inLevel)
        {
            this.Name = "ExGal_MiningDrone_Flag";
            this.CanBeDroppedOnShipDeath = false;
        }

        public override void Tick()
        {
            base.Tick();
            if (!(this.IsEquipped && this.ShipStats != null && this.ShipStats.Ship != null && this.ShipStats.Ship.IsDrone))
                return;
            if (this.ShipStats.Ship.MyShieldGenerator != null && this.ShipStats.Ship.MyShieldGenerator.CanBeDroppedOnShipDeath)
                this.ShipStats.Ship.MyShieldGenerator.CanBeDroppedOnShipDeath = false;
            this.ShipStats.Ship.ClearModifiers();
            if (PhotonNetwork.isMasterClient && !GaveMission && PLServer.Instance != null && !PLEncounterManager.Instance.PlayerShip.InWarp && (double)PLEncounterManager.Instance.GetCPEI().GetTimePlayerInEncounter() > 10.0)
            {
                if (!PLServer.Instance.HasActiveMissionWithID(8000015) && !PLServer.Instance.HasCompletedMissionWithID(8000015) && PLServer.Instance.HasYetToStartMissionWithID(8000015))
                    PLServer.Instance.AttemptStartMissionOfTypeID(8000015, true, new PhotonMessageInfo());
                GaveMission = true;
            }
            if (this.Level == 0)
            {
                this.ShipStats.Ship.ShipNameValue = "Mining Drone";
                this.ShipStats.Ship.GX_ID = "Mining Drone";
                if (PhotonNetwork.isMasterClient && !PLEncounterManager.Instance.PlayerShip.InWarp && MiningDroneQuest.GXData < 1)
                {
                    MiningDroneQuest.GXData = 1;
                    PLServer.Instance.photonView.RPC("AddCrewWarning", PhotonTargets.All, new object[4]
                    {
                            "New GX Entry Added!",
                            Color.white,
                            0,
                            "GX"
                    });
                }
            }
            else if (this.Level < 4)
            {
                this.ShipStats.Ship.ShipNameValue = "Escort Drone";
                this.ShipStats.Ship.GX_ID = "Escort Drone";
                if (PhotonNetwork.isMasterClient && !PLEncounterManager.Instance.PlayerShip.InWarp && MiningDroneQuest.GXData < 1)
                {
                    MiningDroneQuest.GXData = 1;
                    PLServer.Instance.photonView.RPC("AddCrewWarning", PhotonTargets.All, new object[4]
                    {
                            "New GX Entry Added!",
                            Color.white,
                            0,
                            "GX"
                    });
                }
            }
            else
            {
                this.ShipStats.Ship.ShipNameValue = "Guardian Drone";
                this.ShipStats.Ship.GX_ID = "Guardian Drone";
                if (PhotonNetwork.isMasterClient && !PLEncounterManager.Instance.PlayerShip.InWarp && MiningDroneQuest.GXData < 2)
                {
                    MiningDroneQuest.GXData = 2;
                    PLServer.Instance.photonView.RPC("AddCrewWarning", PhotonTargets.All, new object[4]
                    {
                            "New GX Entry Added!",
                            Color.white,
                            0,
                            "GX"
                    });

                }
            }
            if (MiningDroneQuest.dronesActive)
            {
                this.ShipStats.Ship.CanFireProbes = true;
                this.ShipStats.Ship.SetAbandoned(false);
                /*if (PhotonNetwork.isMasterClient && this.ShipStats.Ship.LastTookDamageTime() == float.MinValue && !this.ShipStats.Ship.PersistantShipInfo.ForcedHostile && this.Level == 0)
                {
                    if (this.IsEquipped && this.ShipStats.Ship.IsDrone && this.ShipStats.Ship.AlertLevel == 0 && this.currentAsteroid == null && PLEncounterManager.Instance.PlayerShip != null && !PLEncounterManager.Instance.PlayerShip.InWarp)
                    {
                        if (PLEncounterManager.Instance.GetCPEI() != null)
                        {
                            foreach (GameObject levelObject in PLEncounterManager.Instance.GetCPEI().LevelObjects)
                            {
                                if (levelObject.name.ToLower().Contains("asteroid"))
                                {
                                    if (levelObject.TryGetComponent(typeof(PLRandomChildItem), out Component child))
                                    {
                                        for (int i = 0; i < levelObject.transform.childCount; i++)
                                        {
                                            GameObject asteroidChoice = levelObject.transform.GetChild(i).gameObject;
                                            if (asteroidChoice.activeSelf)
                                            {
                                                if (currentAsteroid == null)
                                                    currentAsteroid = asteroidChoice;
                                                else
                                                {
                                                    if (Vector3.SqrMagnitude(currentAsteroid.transform.position - this.ShipStats.Ship.Exterior.transform.position) > Vector3.SqrMagnitude(asteroidChoice.transform.position - this.ShipStats.Ship.Exterior.transform.position))
                                                        currentAsteroid = asteroidChoice;
                                                }
                                                break;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (currentAsteroid == null)
                                            currentAsteroid = levelObject;
                                        else
                                        {
                                            if (Vector3.SqrMagnitude(currentAsteroid.transform.position - this.ShipStats.Ship.Exterior.transform.position) > Vector3.SqrMagnitude(levelObject.transform.position - this.ShipStats.Ship.Exterior.transform.position))
                                                currentAsteroid = levelObject;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    if (currentAsteroid != null)
                    {
                        if (this.ShipStats.Ship.TargetSpaceTarget == null && this.ShipStats.Ship.AlertLevel < 2)
                        {
                            if (currentAsteroid.TryGetComponent<PLDamageableSpaceObject>(out PLDamageableSpaceObject component) || currentAsteroid.GetComponentInChildren<PLDamageableSpaceObject>() != null)
                            {
                                if (component != null)
                                    this.ShipStats.Ship.Captain_SetTargetShip(component.SpaceTargetID);
                                else
                                    this.ShipStats.Ship.Captain_SetTargetShip(currentAsteroid.GetComponentInChildren<PLDamageableSpaceObject>().SpaceTargetID);
                                this.ShipStats.Ship.AlertLevel = 1;
                                this.ShipStats.Ship.ShipTypeID = EShipType.E_ACADEMY;
                            }
                            else
                            {
                                GameObject attachPoint;
                                List<MeshCollider> colliders = new List<MeshCollider>();
                                if (currentAsteroid.TryGetComponent<MeshCollider>(out MeshCollider meshCollider))
                                {
                                    attachPoint = currentAsteroid;
                                    colliders.Add(meshCollider);
                                }
                                else
                                {
                                    attachPoint = currentAsteroid.GetComponentInChildren<MeshCollider>().gameObject;
                                    colliders.AddRange(currentAsteroid.GetComponentsInChildren<MeshCollider>());
                                }
                                if (attachPoint == null)
                                {
                                    currentAsteroid = null;
                                    return;
                                }
                                PLDamageableSpaceObject spaceObject = attachPoint.AddComponent<PLDamageableSpaceObject>();
                                spaceObject.MaxHealth = 1000000f;
                                spaceObject.Health = 1000000f;
                                spaceObject.SlowHealth = 1000000;
                                spaceObject.ShowDamageVisualEffectOnHit = false;
                                spaceObject.Visuals = new GameObject[1];
                                spaceObject.Visuals[0] = attachPoint;
                                spaceObject.Destroyable = false;
                                spaceObject.DisplayName = "Asteroid";

                                foreach (MeshCollider collider1 in colliders)
                                {
                                    PLDamageableSpaceObject_Collider DSO_Collider = collider1.gameObject.AddComponent<PLDamageableSpaceObject_Collider>();
                                    DSO_Collider.MyDSO = spaceObject;
                                }

                                AsteroidInfo info = attachPoint.AddComponent<AsteroidInfo>();
                                PLSensorObjectShip objectShip = attachPoint.AddComponent<PLSensorObjectShip>();
                                objectShip.ManualSensorStrings = new PLSensorObjectString[0];
                                info.MySensorObjectShip = objectShip;
                                objectShip.MyShipInfo = info;
                                info.TeamID = 1;
                                info.Exterior = attachPoint;
                                info.SetShipID(PLServer.ServerSpaceTargetIDCounter++);
                                info.SetupShipStats();
                                if (PLEncounterManager.Instance != null && PLEncounterManager.Instance.GetCPEI() != null)
                                    PLEncounterManager.Instance.GetCPEI().MyCreatedShipInfos.Add(info);
                                spaceObject.Ship = info;

                            }
                        }
                    }
                }
                if (this.ShipStats.Ship.TargetSpaceTarget != null)
                {
                    this.ShipStats.Ship.PowerPercent_SysIntConduits[5] = 0.1f;
                    this.ShipStats.Ship.PowerPercent_SysIntConduits[6] = 0.1f;
                    this.ShipStats.Ship.PowerPercent_SysIntConduits[7] = 0.1f;
                    for (int i = 0; i < 2; i++)
                    {
                        PLTurret turret = this.ShipStats.GetRegularTurretAtIndex(i);
                        if (!(turret is Turrets.AuxTurrets.MiningLaser))
                            turret.ChargeAmount = 0f;
                    }
                }*/
                if (PhotonNetwork.isMasterClient && this.ShipStats.Ship.AlertLevel > 1 && PLEncounterManager.Instance.GetCPEI() != null)
                {
                    this.ShipStats.Ship.PersistantShipInfo.ForcedHostile = true;
                    foreach (PLShipInfoBase plShipInfoBase in PLEncounterManager.Instance.GetCPEI().MyCreatedShipInfos)
                    {
                        if (plShipInfoBase != null && plShipInfoBase.PersistantShipInfo != null && !plShipInfoBase.PersistantShipInfo.ForcedHostile)
                        {
                            if (plShipInfoBase.ShipTypeID == EShipType.E_WDDRONE2)
                            {
                                foreach (PLShipComponent component in plShipInfoBase.MyStats.GetComponentsOfType(ESlotType.E_COMP_REAC_COOLING))
                                {
                                    if (component is MiningDroneFlag)
                                    {
                                        plShipInfoBase.PersistantShipInfo.ForcedHostile = true;
                                        if (this.ShipStats.Ship.TargetShip != null)
                                            plShipInfoBase.Captain_SetTargetShip(this.ShipStats.Ship.TargetShip.ShipID);
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
                if (!(this.Level > 0 && PhotonNetwork.isMasterClient && this.ShipStats.Ship.AlertLevel > 1 && this.ShipStats.Ship.TargetShip != null))
                    return;
                double cooldown;
                EVirusType virusType;
                if (this.Level < 4)
                {
                    switch (this.Level)
                    {
                        case 1:
                            virusType = EVirusType.WARP_DISABLE;
                            cooldown = 120.0;
                            break;
                        case 2:
                            virusType = EVirusType.ARMOR_FLAW;
                            cooldown = 60.0;
                            break;
                        default:
                            virusType = EVirusType.PHALANX;
                            cooldown = 150.0;
                            break;
                    }
                }
                else
                {
                    switch (UnityEngine.Random.Range(0, 3))
                    {
                        case 1:
                            virusType = EVirusType.SYBER_SHIELD;
                            cooldown = 100.0;
                            break;
                        case 2:
                            virusType = EVirusType.ARMOR_FLAW;
                            cooldown = 60.0;
                            break;
                        default:
                            virusType = EVirusType.PHALANX;
                            cooldown = 150.0;
                            break;
                    }
                }
                if (PLEncounterManager.Instance.PlayerShip != null && !this.ShipStats.Ship.SendQueueContainsVirusOfType(virusType))
                {
                    if ((double)(Time.time - LastVirusTime) > cooldown)
                    {
                        LastVirusTime = Time.time;
                        PLServer.Instance.photonView.RPC("AddToSendQueue", PhotonTargets.All, new object[4]
                        {
                                this.ShipStats.Ship.ShipID,
                                this.ShipStats.Ship.VirusSendQueueCounter++,
                                (int)virusType,
                                PLServer.Instance.GetEstimatedServerMs()
                        });
                    }
                }
            }
            else
            {
                this.ShipStats.Ship.CanFireProbes = false;
                this.ShipStats.Ship.SetAbandoned(true);
                this.ShipStats.Ship.AlertLevel = 0;
                this.ShipStats.Ship.Captain_SetTargetShip(-1);
                if (PhotonNetwork.isMasterClient)
                    this.ShipStats.Ship.PersistantShipInfo.ForcedHostile = false;
            }
        }

        public override void FinalLateAddStats(PLShipStats inStats)
        {
            base.FinalLateAddStats(inStats);
            if (!(this.IsEquipped && this.ShipStats != null && this.ShipStats.Ship != null && this.ShipStats.Ship.IsDrone))
                return;
            if (!MiningDroneQuest.dronesActive)
            {
                this.ShipStats.ShieldsCurrent = 0f;
                this.ShipStats.ShieldsMax = 0f;
                this.ShipStats.ShieldsChargeRate = 0f;
                this.ShipStats.ThrustOutputMax = 0f;
                this.ShipStats.ThrustOutputCurrent = 0f;
                this.ShipStats.ManeuverThrustOutputMax = 0f;
                this.ShipStats.ManeuverThrustOutputCurrent = 0f;
                this.ShipStats.InertiaThrustOutputMax = 0f;
                this.ShipStats.InertiaThrustOutputCurrent = 0f;
            }
        }

        public override void Equip()
        {
            base.Equip();
            AllMiningDroneFlags.Add(this);
        }

        public override void Unequip()
        {
            base.Unequip();
            if (AllMiningDroneFlags.Contains(this))
                AllMiningDroneFlags.Remove(this);
        }
    }
}

