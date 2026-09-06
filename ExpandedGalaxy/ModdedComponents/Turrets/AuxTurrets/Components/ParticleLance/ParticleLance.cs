using CodeStage.AntiCheat.ObscuredTypes;
using PulsarModLoader.Content.Components.Turret;
using UnityEngine;

namespace ExpandedGalaxy
{
    internal class ParticleLance : PLTurret
    {
        private ParticleSystem beamPS;
        private ParticleSystem flarePS;
        private ParticleSystem flarePS2;
        private ParticleSystem flarePS3;
        private ParticleSystem beamPS2;
        private ParticleSystem beamPS3;
        private ParticleSystem.EmissionModule flarePS2_emmModule;
        private bool ColorCorrected = false;

        public ParticleLance(int inLevel = 0, int inSubTypeData = 0)
        {
            this.Name = "Particle Lance";
            this.Desc = "Fires a beam that deals significant damage to both hull and shields. Deals more damage as the target gets closer.";
            this.m_Damage = 450f;
            this.SubType = TurretModManager.Instance.GetTurretIDFromName(this.Name);
            this.m_MarketPrice = (ObscuredInt)9700;
            this.m_ProjSpeed = 550f;
            this.FireDelay = 8.4f;
            this.HeatGeneratedOnFire = 0.6f;
            this.TurretRange = 5500f;
            this.CargoVisualPrefabID = 3;
            this.Level = inLevel;
            this.FireTurretSoundSFX = "play_ship_generic_external_weapon_phaseturret_shoot";
            this.CanHitMissiles = true;
            this.UpdateMaxPowerUsageWatts();
            this.ColorCorrected = false;
        }

        protected virtual void CorrectColors()
        {
            if (!(this.TurretInstance != null && this.TurretInstance.OptionalGameObjects.Length > 0))
                return;
            this.TurretInstance.OptionalGameObjects[0].GetComponent<ParticleSystem>().startColor = new Color(1f, 0f, 0f, 1f);
            this.TurretInstance.OptionalGameObjects[1].GetComponent<ParticleSystem>().startColor = new Color(0f, 0f, 0f, 0f);
            this.TurretInstance.OptionalGameObjects[2].GetComponent<ParticleSystem>().startColor = new Color(1f, 0f, 0f, 0.4392f);
            this.TurretInstance.OptionalGameObjects[4].GetComponent<ParticleSystem>().startColor = new Color(1f, 0f, 0f, 1f);
            this.ColorCorrected = true;
        }

        public void UpdateMaxPowerUsageWatts() => this.CalculatedMaxPowerUsage_Watts = 8000f * this.LevelMultiplier(0.1f);

        private float RangeDamageScalar(Vector3 targetPosition)
        {
            if ((targetPosition - this.ShipStats.Ship.GetCurrentSensorPosition()).magnitude < 1500f / 5f)
                return 1f;
            else
                return (((this.TurretRange - 1500f) / 5f) - ((targetPosition - this.ShipStats.Ship.GetCurrentSensorPosition()).magnitude - (1500 / 5f))) / ((this.TurretRange - 1500f) / 5f);
        }

        public override string GetStatLineLeft() => PLLocalize.Localize("Damage") + "\n" + PLLocalize.Localize("Damage (Max)") + "\n" + PLLocalize.Localize("Charge Time") + "\n" + PLLocalize.Localize("Dmg Type") + "\n";

        public override string GetStatLineRight()
        {
            float num1 = (float)((double)this.m_Damage * (double)this.LevelMultiplier(0.15f) * (this.ShipStats != null ? (double)this.ShipStats.TurretDamageFactor : 1.0));
            float num2 = this.FireDelay / (this.ShipStats != null ? this.ShipStats.TurretChargeFactor : 1f);
            float num3 = num1 / 10f;
            return num3.ToString("0") + "\n" + num1.ToString("0") + "\n" + num2.ToString("0.0") + "\n" + this.GetDamageTypeString() + "\n";
        }

        public override string GetTurretPrefabPath() => "NetworkPrefabs/Component_Prefabs/PhaseTurret";

        public override string GetDamageTypeString() => "PHYSICAL (BEAM)";

        public override bool ApplyLeadingToAutoAimShot() => false;

        public override void Tick()
        {
            base.Tick();
            if (!(this.TurretInstance != null))
                return;
            if (this.IsEquipped && !this.ColorCorrected)
                this.CorrectColors();
            this.UpdateMaxPowerUsageWatts();
            if (this.beamPS == null || this.beamPS2 == null)
            {
                this.beamPS = this.TurretInstance.BeamObject.GetComponent<ParticleSystem>();
                this.flarePS = this.TurretInstance.OptionalGameObjects[0].GetComponent<ParticleSystem>();
                this.flarePS2 = this.TurretInstance.OptionalGameObjects[1].GetComponent<ParticleSystem>();
                this.beamPS2 = this.TurretInstance.OptionalGameObjects[2].GetComponent<ParticleSystem>();
                this.flarePS3 = this.TurretInstance.OptionalGameObjects[3].GetComponent<ParticleSystem>();
                this.beamPS3 = this.TurretInstance.OptionalGameObjects[4].GetComponent<ParticleSystem>();
                this.flarePS2_emmModule = this.flarePS2.emission;
            }
            if (!(this.flarePS2 != null))
                return;
            float num = (float)PLXMLOptionsIO.Instance.CurrentOptions.GetStringValueAsInt("QualityLevel") * 200f;
            this.flarePS2_emmModule.rateOverTime = (ParticleSystem.MinMaxCurve)((double)Time.time - (double)this.LastFireTime < 0.30000001192092896 ? num : 0.0f);
        }

        public override void Fire(int inProjID, Vector3 dir)
        {
            this.ChargeAmount = 0.0f;
            this.LastFireTime = Time.time;
            if (this.FireTurretSoundSFX != "")
                PLMusic.PostEvent(this.FireTurretSoundSFX, this.TurretInstance.gameObject);
            if (this.beamPS != null)
                this.beamPS.Emit(40);
            if (this.flarePS != null)
                this.flarePS.Emit(2);
            if (this.beamPS2 != null)
                this.beamPS2.Emit(8);
            if (this.beamPS3 != null)
                this.beamPS3.Emit(8);
            if (this.flarePS3 != null)
                this.flarePS3.Emit(12);
            this.Heat += this.HeatGeneratedOnFire;
            if ((double)Time.time - (double)this.ShipStats.Ship.LastCloakingSystemActivatedTime > 2.0)
                this.ShipStats.Ship.SetIsCloakingSystemActive(false);
            float dmg = this.m_Damage * this.LevelMultiplier(0.15f) * this.ShipStats.TurretDamageFactor;
            Ray ray = new Ray(this.TurretInstance.FiringLoc.position, this.TurretInstance.FiringLoc.forward);
            int layerMask = 524289;
            int num1 = 0;
            if (this.ShipStats.Ship.GetExteriorMeshCollider() != null)
            {
                num1 = this.ShipStats.Ship.GetExteriorMeshCollider().gameObject.layer;
                this.ShipStats.Ship.GetExteriorMeshCollider().gameObject.layer = 31;
            }
            float laserDist = 0.0f;
            float maxDistance = this.TurretRange / 5f;
            try
            {
                UnityEngine.RaycastHit hitInfo;
                if (Physics.SphereCast(ray, 3f, out hitInfo, maxDistance, layerMask))
                {
                    PLMusic.PostEvent("play_ship_generic_external_weapon_phaseturret_impact", hitInfo.collider.gameObject);
                    PLShipInfoBase plShipInfoBase = (PLShipInfoBase)null;
                    if (hitInfo.collider != null)
                        plShipInfoBase = hitInfo.collider.GetComponentInParent<PLShipInfoBase>();
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
                    if (plProximityMine != null)
                        PLServer.Instance.photonView.RPC("ProximityMineExplode", PhotonTargets.All, (object)plProximityMine.EncounterNetID);
                    else if (plShipInfoBase != null)
                    {
                        if (plShipInfoBase != this.ShipStats.Ship)
                            PLServer.Instance.photonView.RPC("ServerTakeDamageProjectile", PhotonTargets.MasterClient, (object)plShipInfoBase.SpaceTargetID, (object)(dmg * RangeDamageScalar(plShipInfoBase.GetCurrentSensorPosition())), (object)false, (object)inProjID, (object)(int)EDamageType.E_PHYSICAL, (object)-1, (object)this.ShipStats.Ship.ShipID, (object)this.TurretID);
                    }
                    else if (plSpaceTarget != null)
                    {
                        if (plSpaceTarget != this.ShipStats.Ship)
                            PLServer.Instance.photonView.RPC("ServerTakeDamageProjectile", PhotonTargets.MasterClient, (object)plShipInfoBase.SpaceTargetID, (object)(dmg * RangeDamageScalar(plSpaceTarget.GetCurrentSensorPosition())), (object)false, (object)inProjID, (object)(int)EDamageType.E_PHYSICAL, (object)-1, (object)this.ShipStats.Ship.ShipID, (object)this.TurretID);
                    }
                    else if (hitInfo.collider != null && hitInfo.collider.CompareTag("Projectile"))
                    {
                        PLProjectile component = hitInfo.collider.gameObject.GetComponent<PLProjectile>();
                        if (component != null && component.LaserCanCauseExplosion && component.OwnerShipID != -1 && component.OwnerShipID != this.ShipStats.Ship.ShipID)
                            component.TakeDamage(dmg * RangeDamageScalar(component.transform.position), hitInfo.point, hitInfo.normal, this.GetCurrentOperator());
                        flag = true;
                    }
                    laserDist = flag ? maxDistance : (hitInfo.point - this.TurretInstance.FiringLoc.position).magnitude;
                }
                Vector3 hitLoc;
                PLSwarmCollider hitSwarmCollider = PLTurret.GetHitSwarmCollider(laserDist, out hitLoc, out int _, this.TurretInstance);
                if (hitSwarmCollider != null)
                {
                    double num2 = (double)UnityEngine.Random.Range(0.0f, 1f);
                    PLMusic.PostEvent("play_ship_generic_external_weapon_phaseturret_impact", hitSwarmCollider.gameObject);
                    hitSwarmCollider.MyShipInfo.HandleSwarmColliderHitVisuals(hitSwarmCollider, hitLoc);
                    PLServer.Instance.photonView.RPC("ServerTakeDamageProjectile", PhotonTargets.MasterClient, (object)hitSwarmCollider.MyShipInfo.SpaceTargetID, (object)(dmg * RangeDamageScalar(hitSwarmCollider.transform.position)), (object)false, (object)inProjID, (object)(int)EDamageType.E_PHYSICAL, (object)-1, (object)this.ShipStats.Ship.ShipID, (object)this.TurretID);
                }
            }
            catch
            {
            }
            if (!(this.ShipStats.Ship.GetExteriorMeshCollider() != null))
                return;
            this.ShipStats.Ship.GetExteriorMeshCollider().gameObject.layer = num1;
        }

        public override void Unequip()
        {
            this.ColorCorrected = false;
            base.Unequip();
        }
    }
}
