using CodeStage.AntiCheat.ObscuredTypes;
using PulsarModLoader;
using PulsarModLoader.Content.Components.AutoTurret;
using System.Collections.Generic;
using UnityEngine;

namespace ExpandedGalaxy
{
    internal class AutoLaser : PLTurret
    {
        protected bool IsBeamActive;
        protected float BeamActiveTime = 1f;
        protected float BeamDelayTime = 0f;
        protected bool ShowDelayBeam = false;
        protected float DamageChecksPerSecond = 3f;
        protected float LastDamageCheckTime;
        protected float m_Laser_xzScale = 1f;
        protected float m_Laser_yScale = 1f;
        public float LaserDist = 20000f;
        protected float LaserBaseRadius = 0.045f;
        public int laserTurretExplosionID;
        private int ProjIDLast = int.MinValue;
        private int BeamTickCount = int.MinValue;
        private Dictionary<int, ProjBeamCounter> ProjBeamCounters = new Dictionary<int, ProjBeamCounter>();
        private float LastClearProjBeamCountersTime = float.MinValue;
        public EDamageType LaserDamageType = EDamageType.E_BEAM;
        protected float basePowerUsage;
        protected string PlayShootSFX = "";
        protected string StopShootSFX = "";
        protected string PlayProjSFX = "";
        protected string StopProjSFX = "";
        private bool beamSFXActive;
        protected float beamColorScalar = 1;
        protected Color beamColor = Color.red;

        public AutoLaser(int inLevel = 0, int inSubTypeData = 0)
        {
            this.Name = "Auto Laser Turret";
            this.Desc = "A fully automated laser turret system designed to fire without direct crew control.";
            this.m_Damage = 30f;
            this.FireDelay = 4f;
            this.SubType = AutoTurretModManager.Instance.GetAutoTurretIDFromName(this.Name);
            this.ActualSlotType = ESlotType.E_COMP_AUTO_TURRET;
            this.m_MarketPrice = (ObscuredInt)3700;
            this.Level = inLevel;
            this.SubTypeData = (short)inSubTypeData;
            this.TurretRange = 8000f;
            this.CargoVisualPrefabID = 3;
            this.CanHitMissiles = true;
            this.AutoAim_CanTargetMissiles = true;
            this.HeatGeneratedOnFire = 0.15f;
            this.PlayShootSFX = "play_ship_generic_external_weapon_laser_shoot";
            this.StopShootSFX = "";
            this.PlayProjSFX = "play_ship_generic_external_weapon_laser_projectile";
            this.StopProjSFX = "stop_ship_generic_external_weapon_laser_projectile";
            this.basePowerUsage = 4000f;
            this.UpdateMaxPowerUsageWatts();
        }

        public ProjBeamCounter GetCounterForProjID(int inProjID)
        {
            if (this.ProjBeamCounters.ContainsKey(inProjID))
                return this.ProjBeamCounters[inProjID];
            ProjBeamCounter counterForProjId = new ProjBeamCounter(inProjID);
            this.ProjBeamCounters.Add(inProjID, counterForProjId);
            return counterForProjId;
        }

        public override void CheckFire()
        {
            if (!this.IsFiring || (double)this.ChargeAmount <= 0.99000000953674316 || !PhotonNetwork.isMasterClient || this.IsBeamActive || this.IsOverheated)
                return;
            PLServer.Instance.photonView.RPC("AutoTurretFire", PhotonTargets.All, (object)this.ShipStats.Ship.ShipID, (object)this.AutoTurretID, (object)PLServer.Instance.ServerProjIDCounter, (object)Vector3.zero);
            ++PLServer.Instance.ServerProjIDCounter;
        }

        public override void Unequip()
        {
            this.StopBeamSFX();
            base.Unequip();
        }

        public virtual void UpdateMaxPowerUsageWatts() => this.CalculatedMaxPowerUsage_Watts = basePowerUsage * this.LevelMultiplier(0.2f);

        public override void Tick()
        {
            if (PLEncounterManager.Instance == null || PLInput.Instance == null)
                return;
            base.Tick();
            this.UpdateMaxPowerUsageWatts();
            if ((double)Time.time - (double)this.LastClearProjBeamCountersTime > 5.0)
            {
                this.LastClearProjBeamCountersTime = Time.time;
                List<ProjBeamCounter> projBeamCounterList = new List<ProjBeamCounter>((IEnumerable<ProjBeamCounter>)this.ProjBeamCounters.Values);
                for (int index = 0; index < projBeamCounterList.Count; ++index)
                {
                    if (projBeamCounterList[index] != null && projBeamCounterList[index].ProjID < this.ProjIDLast - 50)
                        this.ProjBeamCounters.Remove(projBeamCounterList[index].ProjID);
                }
            }
            if (this.IsBeamActive && (double)Time.time - (double)this.LastFireTime > (double)this.BeamActiveTime + (double)this.BeamDelayTime)
                this.IsBeamActive = false;
            if ((double)this.BeamDelayTime > 0.0 && this.LastFireTime != float.MinValue)
            {
                this.ShowDelayBeam = false;
                if ((double)Time.time - (double)this.LastFireTime > (double)this.BeamDelayTime && !((double)Time.time - (double)this.LastFireTime > (double)this.BeamDelayTime + (double)this.BeamActiveTime))
                    this.IsBeamActive = true;
                if (!((double)Time.time - (double)this.LastFireTime > (double)this.BeamDelayTime))
                    this.ShowDelayBeam = true;
            }
            if (this.IsBeamActive && this.TurretInstance != null && !this.IsOverheated && (double)Time.time - (double)this.LastDamageCheckTime > 1.0 / (double)this.DamageChecksPerSecond)
            {
                ++this.BeamTickCount;
                this.LastDamageCheckTime = Time.time;
                this.Heat += this.HeatGeneratedOnFire;
                float num1 = this.m_Damage * this.LevelMultiplier(0.15f) * this.ShipStats.TurretDamageFactor;
                Ray ray = new Ray(this.TurretInstance.FiringLoc.position, this.TurretInstance.FiringLoc.forward);
                int layerMask = 524289;
                int num2 = 0;
                if (this.ShipStats.Ship.GetExteriorMeshCollider() != null)
                {
                    num2 = this.ShipStats.Ship.GetExteriorMeshCollider().gameObject.layer;
                    this.ShipStats.Ship.GetExteriorMeshCollider().gameObject.layer = 31;
                }
                float maxDistance = this.TurretRange / 5f;
                try
                {
                    RaycastHit hitInfo;
                    if (Physics.SphereCast(ray, 3f, out hitInfo, maxDistance, layerMask))
                    {
                        PLShipInfoBase plShipInfoBase = (PLShipInfoBase)null;
                        PLDamageableSpaceObject_Collider spaceObjectCollider = (PLDamageableSpaceObject_Collider)null;
                        if (hitInfo.collider != null)
                        {
                            plShipInfoBase = hitInfo.collider.GetComponentInParent<PLShipInfoBase>();
                            spaceObjectCollider = hitInfo.collider.GetComponent<PLDamageableSpaceObject_Collider>();
                        }
                        bool flag = false;
                        if (plShipInfoBase != null && plShipInfoBase.Hull_Virtual_MeshCollider != null && !plShipInfoBase.CollisionShieldShouldBeActive() && !plShipInfoBase.Hull_Virtual_MeshCollider.Raycast(ray, out RaycastHit _, maxDistance))
                        {
                            flag = true;
                            plShipInfoBase = (PLShipInfoBase)null;
                        }
                        PLProximityMine plProximityMine = (PLProximityMine)null;
                        if (plShipInfoBase == null)
                            plProximityMine = hitInfo.collider.GetComponentInParent<PLProximityMine>();
                        if (hitInfo.collider.gameObject.name.StartsWith("MatrixPoint"))
                        {
                            PLMatrixPoint component = hitInfo.collider.GetComponent<PLMatrixPoint>();
                            if (component != null && component.IsActiveAndBlocking())
                                component.OnHit(this.ShipStats.Ship);
                        }
                        PLSpaceTarget plSpaceTarget = (PLSpaceTarget)null;
                        if (hitInfo.transform != null)
                            plSpaceTarget = hitInfo.transform.GetComponentInParent<PLSpaceTarget>();
                        if (spaceObjectCollider != null)
                            spaceObjectCollider.MyDSO.TakeDamage_Location(num1, hitInfo.point, hitInfo.normal);
                        else if (plProximityMine != null)
                            PLServer.Instance.photonView.RPC("ProximityMineExplode", PhotonTargets.All, (object)plProximityMine.EncounterNetID);
                        else if (plShipInfoBase != null)
                        {
                            if (plShipInfoBase != this.ShipStats.Ship)
                            {
                                float randomNum = UnityEngine.Random.Range(0.0f, 1f);
                                if (!PhotonNetwork.isMasterClient)
                                    ModMessage.SendRPC("sugarbuzz1.ExpandedGalaxy", "ExpandedGalaxy.ReciveAutoLaserDamage", PhotonTargets.MasterClient, new object[9]
                                    {
                    (object) plShipInfoBase.ShipID,
                    (object) this.ShipStats.Ship.ShipID,
                    (object) num1,
                    (object) randomNum,
                    (object) plShipInfoBase.Exterior.transform.InverseTransformPoint(hitInfo.point),
                    (object) this.AutoTurretID,
                    (object) this.ProjIDLast,
                    (object) this.BeamTickCount,
                    (object) this.AutoTurretID
                                    });
                                Turrets.LaserAutoTurretDamage(plShipInfoBase.ShipID, this.ShipStats.Ship.ShipID, num1, randomNum, plShipInfoBase.Exterior.transform.InverseTransformPoint(hitInfo.point), this.AutoTurretID, this.ProjIDLast, this.BeamTickCount, this.AutoTurretID);
                            }
                        }
                        else if (plSpaceTarget != null)
                        {
                            if (plSpaceTarget != this.ShipStats.Ship)
                            {
                                float randomNum = UnityEngine.Random.Range(0.0f, 1f);
                                if (!PhotonNetwork.isMasterClient)
                                    ModMessage.SendRPC("sugarbuzz1.ExpandedGalaxy", "ExpandedGalaxy.ReciveAutoLaserDamage", PhotonTargets.MasterClient, new object[9]
                                    {
                    (object) plSpaceTarget.SpaceTargetID,
                    (object) this.ShipStats.Ship.ShipID,
                    (object) num1,
                    (object) randomNum,
                    (object) plSpaceTarget.transform.InverseTransformPoint(hitInfo.point),
                    (object) this.AutoTurretID,
                    (object) this.ProjIDLast,
                    (object) this.BeamTickCount,
                    (object) this.AutoTurretID
                                    });
                                Turrets.LaserAutoTurretDamage(plSpaceTarget.SpaceTargetID, this.ShipStats.Ship.ShipID, num1, randomNum, plSpaceTarget.transform.InverseTransformPoint(hitInfo.point), this.AutoTurretID, this.ProjIDLast, this.BeamTickCount, this.AutoTurretID);
                            }
                        }
                        else if (hitInfo.collider != null && hitInfo.collider.CompareTag("Projectile"))
                        {
                            PLProjectile component = hitInfo.collider.gameObject.GetComponent<PLProjectile>();
                            if (component != null && component.LaserCanCauseExplosion && component.OwnerShipID != -1 && component.OwnerShipID != this.ShipStats.Ship.ShipID)
                                component.TakeDamage(num1, hitInfo.point, hitInfo.normal, this.GetCurrentOperator());
                            flag = true;
                        }
                        else if (!flag)
                        {
                            PLServer.Instance.photonView.RPC("LaserTurretExplosion", PhotonTargets.Others, (object)hitInfo.point, (object)-1, (object)this.laserTurretExplosionID);
                            PLServer.Instance.LaserTurretExplosion(hitInfo.point, -1, this.laserTurretExplosionID);
                        }
                        this.LaserDist = flag ? maxDistance : (hitInfo.point - this.TurretInstance.FiringLoc.position).magnitude;
                    }
                    Vector3 hitLoc;
                    int numHit;
                    PLSwarmCollider hitSwarmCollider = PLTurret.GetHitSwarmCollider(this.LaserDist, out hitLoc, out numHit, this.TurretInstance);
                    if (hitSwarmCollider != null)
                    {
                        float randomNum = UnityEngine.Random.Range(0.0f, 1f);
                        hitSwarmCollider.MyShipInfo.HandleSwarmColliderHitVisuals(hitSwarmCollider, hitLoc);
                        if (!PhotonNetwork.isMasterClient)
                            PLServer.Instance.photonView.RPC("LaserTurretDamage", PhotonTargets.MasterClient, (object)hitSwarmCollider.MyShipInfo.SpaceTargetID, (object)this.ShipStats.Ship.ShipID, (object)(float)((double)num1 * (double)numHit), (object)randomNum, (object)hitLoc, (object)this.AutoTurretID, (object)this.ProjIDLast, (object)this.BeamTickCount, (object)this.AutoTurretID);
                        PLServer.Instance.LaserTurretDamage(hitSwarmCollider.MyShipInfo.SpaceTargetID, this.ShipStats.Ship.ShipID, num1 * (float)numHit, randomNum, hitLoc, this.AutoTurretID, this.ProjIDLast, this.BeamTickCount, this.AutoTurretID);
                    }
                }
                catch
                {
                }
                if (this.ShipStats.Ship.GetExteriorMeshCollider() != null)
                    this.ShipStats.Ship.GetExteriorMeshCollider().gameObject.layer = num2;
            }
            if (!(this.TurretInstance != null) || !(this.TurretInstance.BeamObject != null))
                return;
            this.TurretInstance.BeamObject.transform.localPosition = new Vector3(0.0f, 0.0f, this.LaserDist * 0.5f) * (1f / this.TurretInstance.transform.lossyScale.x);
            float num = Mathf.Abs(Mathf.Sin((float)((double)Time.time * 5.0 % 3.1415927410125732 * 2.0)));
            if (this.ShowDelayBeam)
                num = 0f;
            Vector3 vector3 = new Vector3(this.LaserBaseRadius + 0.01f * num, this.LaserDist * 0.5f, this.LaserBaseRadius + 0.01f * num) * (1f / this.TurretInstance.transform.lossyScale.x);
            this.TurretInstance.BeamObject.transform.localScale = new Vector3(vector3.x * this.m_Laser_xzScale, vector3.y * this.m_Laser_yScale, vector3.z * this.m_Laser_xzScale);
            if (this.TurretInstance.BeamObject.activeSelf != this.IsBeamActive || this.ShowDelayBeam)
                this.TurretInstance.BeamObject.SetActive(this.IsBeamActive || this.ShowDelayBeam);
            if (this.TurretInstance.BeamObjectRenderer == null)
                this.TurretInstance.BeamObjectRenderer = this.TurretInstance.BeamObject.GetComponent<Renderer>();
            this.TurretInstance.BeamObjectRenderer.material.SetFloat("_TimeSinceShot", Time.time - this.LastFireTime);
            Color color = this.beamColor;
            if (this.ShowDelayBeam)
            {
                color.a = 0.05f / this.beamColorScalar;
                this.TurretInstance.BeamObjectRenderer.material.SetColor("_Color", color * this.beamColorScalar);
            }
            else
            {
                color.a = 1f;
                this.TurretInstance.BeamObjectRenderer.material.SetColor("_Color", color * this.beamColorScalar);
            }
            if ((this.IsBeamActive) && !this.IsOverheated)
            {
                if (!this.beamSFXActive)
                {
                    this.beamSFXActive = true;
                    if (this.PlayShootSFX != "")
                        PLMusic.PostEvent(this.PlayShootSFX, this.TurretInstance.gameObject);
                    if (this.PlayProjSFX != "")
                        PLMusic.PostEvent(this.PlayProjSFX, this.TurretInstance.gameObject);
                }
                if (!this.TurretInstance.OptionalGameObjects[0].activeSelf)
                    this.TurretInstance.OptionalGameObjects[0].SetActive(true);
            }
            else
            {
                this.StopBeamSFX();
                if (this.TurretInstance.OptionalGameObjects[0].activeSelf)
                    this.TurretInstance.OptionalGameObjects[0].SetActive(false);
            }
        }

        private void StopBeamSFX()
        {
            if (!this.beamSFXActive)
                return;
            this.beamSFXActive = false;
            if (this.TurretInstance != null)
            {
                if (this.StopShootSFX != "")
                    PLMusic.PostEvent(this.StopShootSFX, this.TurretInstance.gameObject);
                if (this.StopProjSFX != "")
                    PLMusic.PostEvent(this.StopProjSFX, this.TurretInstance.gameObject);
            }
        }

        public override void Fire(int inProjID, Vector3 dir)
        {
            this.ChargeAmount = 0.0f;
            this.LastFireTime = Time.time;
            if (!((double)this.BeamDelayTime > 0.0))
                this.IsBeamActive = true;
            this.ProjIDLast = inProjID;
            this.BeamTickCount = 0;
            if ((double)Time.time - (double)this.ShipStats.Ship.LastCloakingSystemActivatedTime <= 3.0)
                return;
            this.ShipStats.Ship.SetIsCloakingSystemActive(false);
        }

        public override bool ApplyLeadingToAutoAimShot() => false;

        public override string GetTurretPrefabPath() => "NetworkPrefabs/Component_Prefabs/LaserTurret";
    }
}
