using CodeStage.AntiCheat.ObscuredTypes;
using PulsarModLoader;
using System.Collections.Generic;
using UnityEngine;

namespace ExpandedGalaxy
{
    public class HeldLaserTurret : PLTurret
    {
        protected bool IsBeamActive;
        protected float BeamActiveTime = 1f;
        protected float DamageChecksPerSecond = 3f;
        protected float LastDamageCheckTime;
        protected float m_Laser_xzScale = 1f;
        protected float m_Laser_yScale = 1f;
        public float LaserDist = 20000f;
        protected float LaserBaseRadius = 0.045f;
        public int laserTurretExplosionID;
        private bool beamSFXActive;
        private int ProjIDLast = int.MinValue;
        private int BeamTickCount = int.MinValue;
        private Dictionary<int, ProjBeamCounter> ProjBeamCounters = new Dictionary<int, ProjBeamCounter>();
        private float LastClearProjBeamCountersTime = float.MinValue;
        public EDamageType LaserDamageType = EDamageType.E_BEAM;
        protected string PlayShootSFX = "";
        protected string StopShootSFX = "";
        protected string PlayProjSFX = "";
        protected string StopProjSFX = "";
        protected bool HitCombo = false;
        protected int HitComboCount = 0;
        protected int HitComboCountMax = 0;
        protected float HitComboMultiplier = 1f;
        private bool ColorCorrected;
        protected bool isMiningLaser = false;

        public ProjBeamCounter GetCounterForProjID(int inProjID)
        {
            if (this.ProjBeamCounters.ContainsKey(inProjID))
                return this.ProjBeamCounters[inProjID];
            ProjBeamCounter counterForProjId = new ProjBeamCounter(inProjID);
            this.ProjBeamCounters.Add(inProjID, counterForProjId);
            return counterForProjId;
        }

        public override string GetDamageTypeString() => "ENERGY (BEAM)";
        public HeldLaserTurret(int inLevel = 0, int inSubTypeData = 0)
        {
            this.Name = "Held Laser Turret";
            this.Desc = "If you're reading this, I fucked up :(";
            this.m_Damage = 50f;
            this.FireDelay = 1f;
            this.m_MarketPrice = (ObscuredInt)6000;
            this.Level = inLevel;
            this.SubTypeData = (short)inSubTypeData;
            this.TurretRange = 8000f;
            this.LaserDist = this.TurretRange / 5f;
            this.HeatGeneratedOnFire = 0.35f;
            this.CargoVisualPrefabID = 3;
            this.CanHitMissiles = true;
            this.PlayShootSFX = "";
            this.StopShootSFX = "";
            this.PlayProjSFX = "play_ship_generic_external_weapon_laser_projectile";
            this.StopProjSFX = "stop_ship_generic_external_weapon_laser_projectile";
            this.ColorCorrected = false;
            this.UpdateMaxPowerUsageWatts();
        }

        protected virtual void CorrectColors()
        {
            this.ColorCorrected = true;
        }

        public virtual float HitComboDamage(float inDamage)
        {
            return inDamage * (1 + this.HitComboCount) * this.HitComboMultiplier;
        }
        public virtual void UpdateMaxPowerUsageWatts() => this.CalculatedMaxPowerUsage_Watts = 7800f * this.LevelMultiplier(0.2f);

        /*public override void UpdatePowerUsage(PLPlayer currentOperator)
        {
            if ((double)this.Heat > 0.0)
            {
                this.m_RequestPowerUsage_Percent = 1f;
                this.IsPowerActive = true;
            }
            else
            {
                this.m_RequestPowerUsage_Percent = 0.0f;
                this.IsPowerActive = false;
            }
        }*/

        public override float GetDPS() => base.GetDPS() * this.DamageChecksPerSecond;

        public override void CheckFire()
        {
            if (!this.IsFiring || (double)this.ChargeAmount <= 0.99000000953674316 || !PhotonNetwork.isMasterClient || this.IsBeamActive || this.IsOverheated)
                return;
            PLServer.Instance.photonView.RPC("TurretFire", PhotonTargets.All, (object)this.ShipStats.Ship.ShipID, (object)this.TurretID, (object)PLServer.Instance.ServerProjIDCounter, (object)Vector3.zero);
            ++PLServer.Instance.ServerProjIDCounter;
        }

        public override void Tick()
        {
            if (PLEncounterManager.Instance == null || PLInput.Instance == null)
                return;
            base.Tick();
            this.UpdateMaxPowerUsageWatts();
            if (this.IsEquipped && !this.ColorCorrected)
                this.CorrectColors();
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
            if (!(this.TurretInstance != null) || this.ShipStats == null || !(this.ShipStats.Ship != null))
                return;
            if (this.IsFiring)
            {
                this.IsBeamActive = true;
                this.Heat += Time.deltaTime * this.HeatGeneratedOnFire;
            }
            else
            {
                this.IsBeamActive = false;
                this.HitComboCount = 0;
            }
            if (this.IsBeamActive && this.TurretInstance != null && !this.IsOverheated && (double)Time.time - (double)this.LastDamageCheckTime > 1.0 / (double)this.DamageChecksPerSecond)
            {
                ++this.BeamTickCount;
                this.LastDamageCheckTime = Time.time;
                float num1 = this.m_Damage * this.LevelMultiplier(0.15f) * this.ShipStats.TurretDamageFactor;
                if (this.HitCombo)
                {
                    num1 = this.HitComboDamage(num1);
                }
                Ray ray = new Ray(this.TurretInstance.FiringLoc.position, this.TurretInstance.FiringLoc.forward);
                int layerMask = 524289;
                int num2 = 0;
                if (this.ShipStats.Ship.GetExteriorMeshCollider() != null)
                {
                    num2 = this.ShipStats.Ship.GetExteriorMeshCollider().gameObject.layer;
                    this.ShipStats.Ship.GetExteriorMeshCollider().gameObject.layer = 31;
                }
                float maxDistance = this.TurretRange / 5f;
                bool flag1 = false;
                try
                {
                    UnityEngine.RaycastHit hitInfo;
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
                        if (plShipInfoBase != null && plShipInfoBase.Hull_Virtual_MeshCollider != null && !plShipInfoBase.CollisionShieldShouldBeActive() && !plShipInfoBase.Hull_Virtual_MeshCollider.Raycast(ray, out UnityEngine.RaycastHit _, maxDistance))
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
                        {
                            spaceObjectCollider.MyDSO.TakeDamage_Location(num1, hitInfo.point, hitInfo.normal);
                            this.HitComboCount++;
                            flag1 = true;
                        }
                        else if (plProximityMine != null)
                        {
                            PLServer.Instance.photonView.RPC("ProximityMineExplode", PhotonTargets.All, (object)plProximityMine.EncounterNetID);
                            flag1 = true;
                        }
                        else if (plShipInfoBase != null)
                        {
                            if (plShipInfoBase != this.ShipStats.Ship)
                            {
                                float randomNum = UnityEngine.Random.Range(0.0f, 1f);
                                if (!PhotonNetwork.isMasterClient)
                                    ModMessage.SendRPC("sugarbuzz1.ExpandedGalaxy", "ExpandedGalaxy.RecieveHeldLaserDamage", PhotonTargets.MasterClient, new object[9]
                                    {
                                            plShipInfoBase.ShipID,
                                            this.ShipStats.Ship.ShipID,
                                            num1,
                                            randomNum,
                                            plShipInfoBase.Exterior.transform.InverseTransformPoint(hitInfo.point),
                                            this.TurretID,
                                            this.ProjIDLast,
                                            this.BeamTickCount,
                                            this.TurretID
                                    });
                                Turrets.HeldLaserTurretDamage(plShipInfoBase.ShipID, this.ShipStats.Ship.ShipID, num1, randomNum, plShipInfoBase.Exterior.transform.InverseTransformPoint(hitInfo.point), this.TurretID, this.ProjIDLast, this.BeamTickCount, this.TurretID);
                                this.HitComboCount++;
                                flag1 = true;
                            }
                        }
                        else if (plSpaceTarget != null)
                        {
                            if (plSpaceTarget != this.ShipStats.Ship)
                            {
                                float randomNum = UnityEngine.Random.Range(0.0f, 1f);
                                if (!PhotonNetwork.isMasterClient)
                                    ModMessage.SendRPC("sugarbuzz1.ExpandedGalaxy", "ExpandedGalaxy.RecieveHeldLaserDamage", PhotonTargets.MasterClient, new object[9]
                                    {
                                            plSpaceTarget.SpaceTargetID,
                                            this.ShipStats.Ship.ShipID,
                                            num1,
                                            randomNum,
                                            plSpaceTarget.transform.InverseTransformPoint(hitInfo.point),
                                            this.TurretID,
                                            this.ProjIDLast,
                                            this.BeamTickCount,
                                            this.TurretID
                                    });
                                Turrets.HeldLaserTurretDamage(plSpaceTarget.SpaceTargetID, this.ShipStats.Ship.ShipID, num1, randomNum, plSpaceTarget.transform.InverseTransformPoint(hitInfo.point), this.TurretID, this.ProjIDLast, this.BeamTickCount, this.TurretID);
                                this.HitComboCount++;
                                flag1 = true;
                            }
                        }
                        else if (hitInfo.collider != null && hitInfo.collider.CompareTag("Projectile"))
                        {
                            PLProjectile component = hitInfo.collider.gameObject.GetComponent<PLProjectile>();
                            if (component != null && component.LaserCanCauseExplosion && component.OwnerShipID != -1 && component.OwnerShipID != this.ShipStats.Ship.ShipID)
                            {
                                component.TakeDamage(num1, hitInfo.point, hitInfo.normal, this.GetCurrentOperator());
                                flag1 = true;
                            }
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
                            ModMessage.SendRPC("sugarbuzz1.ExpandedGalaxy", "ExpandedGalaxy.RecieveHeldLaserDamage", PhotonTargets.MasterClient, new object[9]
                                    {
                                            hitSwarmCollider.MyShipInfo.SpaceTargetID,
                                            this.ShipStats.Ship.ShipID,
                                            (float)((double)num1 * (double)numHit),
                                            randomNum,
                                            hitLoc,
                                            this.TurretID,
                                            this.ProjIDLast,
                                            this.BeamTickCount,
                                            this.TurretID
                                    });
                        Turrets.HeldLaserTurretDamage(hitSwarmCollider.MyShipInfo.SpaceTargetID, this.ShipStats.Ship.ShipID, (float)((double)num1 * (double)numHit), randomNum, hitLoc, this.TurretID, this.ProjIDLast, this.BeamTickCount, this.TurretID);
                        this.HitComboCount++;
                        flag1 = true;
                    }
                }
                catch
                {
                }
                if (this.ShipStats.Ship.GetExteriorMeshCollider() != null)
                    this.ShipStats.Ship.GetExteriorMeshCollider().gameObject.layer = num2;
                if (!flag1)
                    this.HitComboCount = 0;
                if (this.HitComboCount > this.HitComboCountMax)
                    this.HitComboCount = this.HitComboCountMax;
            }
            if (!(this.TurretInstance != null) || !(this.TurretInstance.BeamObject != null))
                return;
            this.TurretInstance.BeamObject.transform.localPosition = new Vector3(0.0f, 0.0f, this.LaserDist * 0.5f) * (1f / this.TurretInstance.transform.lossyScale.x);
            float num = Mathf.Abs(Mathf.Sin((float)((double)Time.time * 5.0 % 3.1415927410125732 * 2.0)));
            Vector3 vector3 = new Vector3(this.LaserBaseRadius + 0.01f * num, this.LaserDist * 0.5f, this.LaserBaseRadius + 0.01f * num) * (1f / this.TurretInstance.transform.lossyScale.x);
            this.TurretInstance.BeamObject.transform.localScale = new Vector3(vector3.x * this.m_Laser_xzScale, vector3.y * this.m_Laser_yScale, vector3.z * this.m_Laser_xzScale);
            if (this.TurretInstance.BeamObject.activeSelf != this.IsBeamActive)
                this.TurretInstance.BeamObject.SetActive(this.IsBeamActive);
            if (this.TurretInstance.BeamObjectRenderer == null)
                this.TurretInstance.BeamObjectRenderer = this.TurretInstance.BeamObject.GetComponent<Renderer>();
            this.TurretInstance.BeamObjectRenderer.material.SetFloat("_TimeSinceShot", Time.time - this.LastFireTime);
            if (this.IsBeamActive && !this.IsOverheated)
            {
                if (!this.beamSFXActive && this.ShipStats != null && this.ShipStats.Ship != null && !this.ShipStats.Ship.HasBeenDestroyed)
                {
                    this.beamSFXActive = true;
                    if (this.PlayShootSFX != "")
                        PLMusic.PostEvent(this.PlayShootSFX, this.TurretInstance.gameObject);
                    if (this.PlayProjSFX != "")
                        PLMusic.PostEvent(this.PlayProjSFX, this.TurretInstance.gameObject);
                }
                if (this.TurretInstance.OptionalGameObjects[0].activeSelf)
                    return;
                this.TurretInstance.OptionalGameObjects[0].SetActive(true);
            }
            else
            {
                this.StopBeamSFX();
                if (!this.TurretInstance.OptionalGameObjects[0].activeSelf)
                    return;
                this.TurretInstance.OptionalGameObjects[0].SetActive(false);
            }
        }

        private Vector3 ClampTurretLocalRotEulerAngles(Vector3 localRotEuler)
        {
            if ((double)localRotEuler.y < -180.0)
                localRotEuler.y += 359.99f;
            if ((double)localRotEuler.x < -180.0)
                localRotEuler.x += 359.99f;
            if ((double)localRotEuler.y > 180.0)
                localRotEuler.y -= 359.99f;
            if ((double)localRotEuler.x > 180.0)
                localRotEuler.x -= 359.99f;
            localRotEuler.x = Mathf.Clamp(localRotEuler.x, this.minimumY, this.maximumY);
            localRotEuler.y = Mathf.Clamp(localRotEuler.y, this.minimumX, this.maximumX);
            localRotEuler.z = 0.0f;
            return localRotEuler;
        }

        public override void Unequip()
        {
            this.StopBeamSFX();
            this.ColorCorrected = false;
            base.Unequip();
        }
        private void StopBeamSFX()
        {
            if (!this.beamSFXActive)
                return;
            this.beamSFXActive = false;
            if (!((UnityEngine.Object)this.TurretInstance != (UnityEngine.Object)null))
                return;
            if (this.StopShootSFX != "")
                PLMusic.PostEvent(this.StopShootSFX, this.TurretInstance.gameObject);
            if (!(this.StopProjSFX != ""))
                return;
            PLMusic.PostEvent(this.StopProjSFX, this.TurretInstance.gameObject);
        }

        public override void Fire(int inProjID, Vector3 dir)
        {
            this.LastFireTime = Time.time;
            this.IsBeamActive = true;
            this.ProjIDLast = inProjID;
            this.BeamTickCount = 0;
            if ((double)Time.time - (double)this.ShipStats.Ship.LastCloakingSystemActivatedTime <= 3.0)
                return;
            this.ShipStats.Ship.SetIsCloakingSystemActive(false);
        }

        public override bool ApplyLeadingToAutoAimShot() => false;

        public override bool ShouldAIFire(bool operatedByBot, float heatOffset, float heatGenOnFire)
        {
            if (operatedByBot)
            {
                if (this.IsFiring && (double)this.Heat < 1.0)
                    return true;
                return !this.IsFiring && (double)this.Heat < 0.60000002384185791;
            }
            if (this.IsFiring && (double)this.Heat < 0.899999988079071)
                return true;
            return !this.IsFiring && (double)this.Heat < 0.30000001192092896;
        }

        public override string GetTurretPrefabPath() => "NetworkPrefabs/Component_Prefabs/LaserTurret";

    }
}
