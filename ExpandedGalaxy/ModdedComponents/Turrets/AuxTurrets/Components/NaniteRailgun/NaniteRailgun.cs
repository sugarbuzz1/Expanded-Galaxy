using PulsarModLoader.Content.Components.Turret;
using UnityEngine;

namespace ExpandedGalaxy
{
    internal class NaniteRailgun : PLPhaseTurret
    {
        private bool ColorCorrected = false;
        private ParticleSystem beamPS;
        private ParticleSystem flarePS;
        private ParticleSystem flarePS2;
        private ParticleSystem flarePS3;
        private ParticleSystem beamPS2;
        private ParticleSystem beamPS3;
        private ParticleSystem.EmissionModule flarePS2_emmModule;

        public NaniteRailgun(int inLevel = 0, int inSubTypeData = 0)
        {
            this.Name = "Nanite Railgun";
            this.Desc = "Slow firing railgun that shoots a beam of nanites to inflict damage.";
            this.m_Damage = 64f;
            this.SubType = TurretModManager.Instance.GetTurretIDFromName("Nanite Railgun");
            this.FireDelay = 11f;
            this.TurretRange = 9000f;
            this.Level = inLevel;
            this.CanHitMissiles = false;
            this.m_MaxPowerUsage_Watts = 7000f * this.LevelMultiplier(0.1f); ;
        }

        protected virtual void CorrectColors()
        {
            if (!(this.TurretInstance != null && this.TurretInstance.OptionalGameObjects.Length > 0))
                return;
            this.TurretInstance.OptionalGameObjects[0].GetComponent<ParticleSystem>().startColor = new Color(0f, 0f, 0f, 0f);
            this.TurretInstance.OptionalGameObjects[1].GetComponent<ParticleSystem>().startColor = new Color(0.8f, 0.8f, 0.8f, 0.7451f);
            this.TurretInstance.OptionalGameObjects[2].GetComponent<ParticleSystem>().startColor = new Color(0.2f, 0.2f, 0.2f, 0.4392f);
            this.TurretInstance.OptionalGameObjects[4].GetComponent<ParticleSystem>().startColor = new Color(0.5f, 0.5f, 0.5f, 1f);
            this.ColorCorrected = true;
        }

        public override string GetDamageTypeString() => "PHYSICAL (BEAM)";

        public override void Tick()
        {
            base.Tick();
            this.CalculatedMaxPowerUsage_Watts = 7000f * this.LevelMultiplier(0.1f);
            if (this.IsEquipped && !this.ColorCorrected)
                this.CorrectColors();
            if (!((UnityEngine.Object)this.TurretInstance != (UnityEngine.Object)null))
            {
                return;
            }

            if ((UnityEngine.Object)this.beamPS == (UnityEngine.Object)null || (UnityEngine.Object)this.beamPS2 == (UnityEngine.Object)null)
            {
                this.beamPS = this.TurretInstance.BeamObject.GetComponent<ParticleSystem>();
                this.flarePS = this.TurretInstance.OptionalGameObjects[0].GetComponent<ParticleSystem>();
                this.flarePS2 = this.TurretInstance.OptionalGameObjects[1].GetComponent<ParticleSystem>();
                this.beamPS2 = this.TurretInstance.OptionalGameObjects[2].GetComponent<ParticleSystem>();
                this.flarePS3 = this.TurretInstance.OptionalGameObjects[3].GetComponent<ParticleSystem>();
                this.beamPS3 = this.TurretInstance.OptionalGameObjects[4].GetComponent<ParticleSystem>();
                this.flarePS2_emmModule = this.flarePS2.emission;
            }
            if (!((UnityEngine.Object)this.flarePS2 != (UnityEngine.Object)null))
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
            if ((UnityEngine.Object)this.beamPS != (UnityEngine.Object)null)
                this.beamPS.Emit(40);
            if ((UnityEngine.Object)this.flarePS != (UnityEngine.Object)null)
                this.flarePS.Emit(2);
            if ((UnityEngine.Object)this.beamPS2 != (UnityEngine.Object)null)
                this.beamPS2.Emit(8);
            if ((UnityEngine.Object)this.beamPS3 != (UnityEngine.Object)null)
                this.beamPS3.Emit(8);
            if ((UnityEngine.Object)this.flarePS3 != (UnityEngine.Object)null)
                this.flarePS3.Emit(12);
            this.Heat += this.HeatGeneratedOnFire;
            if ((double)Time.time - (double)this.ShipStats.Ship.LastCloakingSystemActivatedTime > 2.0)
                this.ShipStats.Ship.SetIsCloakingSystemActive(false);
            float dmg = this.m_Damage * this.LevelMultiplier(0.15f) * this.ShipStats.TurretDamageFactor;
            Ray ray = new Ray(this.TurretInstance.FiringLoc.position, this.TurretInstance.FiringLoc.forward);
            int layerMask = 524289;
            int num1 = 0;
            if ((UnityEngine.Object)this.ShipStats.Ship.GetExteriorMeshCollider() != (UnityEngine.Object)null)
            {
                num1 = this.ShipStats.Ship.GetExteriorMeshCollider().gameObject.layer;
                this.ShipStats.Ship.GetExteriorMeshCollider().gameObject.layer = 31;
            }
            float laserDist = 0.0f;
            float maxDistance = this.TurretRange / 5f;
            try
            {
                UnityEngine.RaycastHit hitInfo;
                if (Physics.SphereCast(ray, 3f, out hitInfo, this.TurretRange / 5f, layerMask))
                {
                    PLMusic.PostEvent("play_ship_generic_external_weapon_phaseturret_impact", hitInfo.collider.gameObject);
                    PLShipInfoBase plShipInfoBase = (PLShipInfoBase)null;
                    if ((UnityEngine.Object)hitInfo.collider != (UnityEngine.Object)null)
                        plShipInfoBase = hitInfo.collider.GetComponentInParent<PLShipInfoBase>();
                    bool flag = false;
                    if ((UnityEngine.Object)plShipInfoBase != (UnityEngine.Object)null && (UnityEngine.Object)plShipInfoBase.Hull_Virtual_MeshCollider != (UnityEngine.Object)null && !plShipInfoBase.CollisionShieldShouldBeActive() && !plShipInfoBase.Hull_Virtual_MeshCollider.Raycast(ray, out UnityEngine.RaycastHit _, maxDistance))
                    {
                        flag = true;
                        plShipInfoBase = (PLShipInfoBase)null;
                    }
                    PLProximityMine plProximityMine = (PLProximityMine)null;
                    if ((UnityEngine.Object)plShipInfoBase == (UnityEngine.Object)null)
                        plProximityMine = hitInfo.collider.GetComponentInParent<PLProximityMine>();
                    if (hitInfo.collider.gameObject.name.StartsWith("MatrixPoint"))
                    {
                        PLMatrixPoint component = hitInfo.collider.GetComponent<PLMatrixPoint>();
                        if ((UnityEngine.Object)component != (UnityEngine.Object)null && component.IsActiveAndBlocking())
                            component.OnHit(this.ShipStats.Ship);
                    }
                    PLSpaceTarget plSpaceTarget = (PLSpaceTarget)null;
                    if ((UnityEngine.Object)hitInfo.transform != (UnityEngine.Object)null)
                        plSpaceTarget = hitInfo.transform.GetComponentInParent<PLSpaceTarget>();
                    if ((UnityEngine.Object)plProximityMine != (UnityEngine.Object)null)
                        PLServer.Instance.photonView.RPC("ProximityMineExplode", PhotonTargets.All, (object)plProximityMine.EncounterNetID);
                    else if ((UnityEngine.Object)plShipInfoBase != (UnityEngine.Object)null)
                    {
                        if ((UnityEngine.Object)plShipInfoBase != (UnityEngine.Object)this.ShipStats.Ship)
                        {
                            Systems.TickDamage(10, plShipInfoBase, dmg, false, EDamageType.E_ARMOR_PIERCE_PHYS, -1, this.ShipStats.Ship, this.TurretID);
                        }
                    }
                    else if ((UnityEngine.Object)plSpaceTarget != (UnityEngine.Object)null)
                    {
                        if ((UnityEngine.Object)plSpaceTarget != (UnityEngine.Object)this.ShipStats.Ship)
                        {
                            Systems.TickDamage(10, plSpaceTarget, dmg, false, EDamageType.E_ARMOR_PIERCE_PHYS, -1, this.ShipStats.Ship, this.TurretID);
                        }

                    }
                    else if ((UnityEngine.Object)hitInfo.collider != (UnityEngine.Object)null && hitInfo.collider.CompareTag("Projectile"))
                    {
                        PLProjectile component = hitInfo.collider.gameObject.GetComponent<PLProjectile>();
                        if ((UnityEngine.Object)component != (UnityEngine.Object)null && component.LaserCanCauseExplosion && component.OwnerShipID != -1 && component.OwnerShipID != this.ShipStats.Ship.ShipID)
                            component.TakeDamage(dmg, hitInfo.point, hitInfo.normal, this.GetCurrentOperator());
                        flag = true;
                    }
                    laserDist = flag ? maxDistance : (hitInfo.point - this.TurretInstance.FiringLoc.position).magnitude;
                }
                Vector3 hitLoc;
                PLSwarmCollider hitSwarmCollider = PLTurret.GetHitSwarmCollider(laserDist, out hitLoc, out int _, this.TurretInstance);
                if ((UnityEngine.Object)hitSwarmCollider != (UnityEngine.Object)null)
                {
                    double num2 = (double)UnityEngine.Random.Range(0.0f, 1f);
                    PLMusic.PostEvent("play_ship_generic_external_weapon_phaseturret_impact", hitSwarmCollider.gameObject);
                    hitSwarmCollider.MyShipInfo.HandleSwarmColliderHitVisuals(hitSwarmCollider, hitLoc);
                }
            }
            catch
            {
            }
            if (!((UnityEngine.Object)this.ShipStats.Ship.GetExteriorMeshCollider() != (UnityEngine.Object)null))
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
