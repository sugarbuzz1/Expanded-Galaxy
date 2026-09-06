using CodeStage.AntiCheat.ObscuredTypes;
using System.Collections.Generic;
using UnityEngine;

namespace ExpandedGalaxy
{
    public class RolandSCFlag : MissionComponentFlag
    {
        public RolandSCFlag(int inLevel = 0) : base(9, inLevel)
        {
            this.Name = "ExGal_RolandSC_Flag";
            this.CanBeDroppedOnShipDeath = false;
            this.shouldResort = true;
            this.SubTypeData = 0;
        }

        public override void Tick()
        {
            base.Tick();
            if (this.ShipStats != null && this.ShipStats.Ship != null)
            {
                if (this.SubTypeData != 1)
                {
                    this.ShipStats.Ship.WeaponsSystem.MaxHealth = (ObscuredFloat)40f;
                    this.ShipStats.Ship.WeaponsSystem.Health = (ObscuredFloat)40f;
                    this.ShipStats.Ship.ComputerSystem.MaxHealth = (ObscuredFloat)30f;
                    this.ShipStats.Ship.ComputerSystem.Health = (ObscuredFloat)30f;

                    GameObject turretStation1 = this.ShipStats.Ship.WeaponsSystem.RegularTurretStationTransforms[0].gameObject;
                    GameObject turretStation2 = this.ShipStats.Ship.WeaponsSystem.RegularTurretStationTransforms[1].gameObject;

                    GameObject turretStation3 = GameObject.Instantiate(turretStation1);
                    GameObject turretStation4 = GameObject.Instantiate(turretStation1);
                    GameObject turretStation5 = GameObject.Instantiate(turretStation1);
                    GameObject turretStation6 = GameObject.Instantiate(turretStation1);

                    GameObject.DontDestroyOnLoad(turretStation3);
                    GameObject.DontDestroyOnLoad(turretStation4);
                    GameObject.DontDestroyOnLoad(turretStation5);
                    GameObject.DontDestroyOnLoad(turretStation6);

                    turretStation3.transform.SetParent(turretStation1.transform.parent);
                    turretStation4.transform.SetParent(turretStation1.transform.parent);
                    turretStation5.transform.SetParent(turretStation1.transform.parent);
                    turretStation6.transform.SetParent(turretStation1.transform.parent);

                    turretStation1.transform.localPosition = new Vector3(13.1f, 7, -7.35f);
                    turretStation1.transform.localRotation = Quaternion.identity;

                    turretStation2.transform.localPosition = new Vector3(-13.4f, 7, -7.35f);
                    turretStation2.transform.localRotation = Quaternion.Euler(0, 180, 0);

                    turretStation3.transform.localPosition = new Vector3(-13.4f, 7, -0.9f);
                    turretStation3.transform.localRotation = Quaternion.Euler(0, 180, 0);

                    turretStation4.transform.localPosition = new Vector3(13.1f, 7, -0.9f);
                    turretStation4.transform.localRotation = Quaternion.identity;

                    turretStation5.transform.localPosition = new Vector3(-13.4f, 7, 5.55f);
                    turretStation5.transform.localRotation = Quaternion.Euler(0, 180, 0);

                    turretStation6.transform.localPosition = new Vector3(13.1f, 7, 5.55f);
                    turretStation6.transform.localRotation = Quaternion.identity;

                    Transform[] newTurretStations = new Transform[6] { turretStation2.transform, turretStation1.transform, turretStation3.transform, turretStation4.transform, turretStation5.transform, turretStation6.transform };
                    this.ShipStats.Ship.WeaponsSystem.RegularTurretStationTransforms = newTurretStations;

                    this.ShipStats.Ship.CurrentTurretControllerPlayerID = new int[7] { -1, -1, -1, -1, -1, -1, -1 };

                    this.SubTypeData = 1;
                }
            }
        }

        public override void Equip()
        {
            this.ShipStats?.SetSlotLimit(ESlotType.E_COMP_TURRET, 6);
            base.Equip();
            if (this.ShipStats == null || this.ShipStats.Ship == null)
                return;
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
        }

        public override void FinalLateAddStats(PLShipStats inStats)
        {
            inStats.ReactorOutputFactor *= 1.5f;
            inStats.OxygenRefillRate *= 2f;
        }
    }

}

