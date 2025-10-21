using CodeStage.AntiCheat.ObscuredTypes;
using HarmonyLib;
using PulsarModLoader;
using PulsarModLoader.Content.Components.AutoTurret;
using PulsarModLoader.Content.Components.MegaTurret;
using PulsarModLoader.Content.Components.Turret;
using PulsarModLoader.Patches;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using UnityEngine;

namespace ExpandedGalaxy
{
    internal class Turrets
    {
        private class NaniteRailgunMod : TurretMod
        {
            public override string Name => "Nanite Railgun";

            public override PLShipComponent PLTurret => (PLShipComponent)new AuxTurrets.NaniteRailgun();
        }

        private class ParticleLanceMod : TurretMod
        {
            public override string Name => "Particle Lance";

            public override PLShipComponent PLTurret => (PLShipComponent)new AuxTurrets.ParticleLance();
        }

        private class MiningLaserMod : TurretMod
        {
            public override string Name => "Mining Laser";

            public override PLShipComponent PLTurret => (PLShipComponent)new AuxTurrets.MiningLaser();
        }

        private class SylvassiTurretMod : TurretMod
        {
            public override string Name => "Sylvassi Turret";

            public override PLShipComponent PLTurret => (PLShipComponent)new AuxTurrets.SylvassiTurret();
        }

        private class SeekerTurretMod : TurretMod
        {
            public override string Name => "Seeker Turret";

            public override PLShipComponent PLTurret => new AuxTurrets.SeekerTurret();
        }

        private class MissileTurretMk2Mod : TurretMod
        {
            public override string Name => "Missile Turret Mk. II";

            public override PLShipComponent PLTurret => new AuxTurrets.MissileTurretMk2();
        }

        private class DisassemblerMod : MegaTurretMod
        {
            public override string Name => "The Disassembler";

            public override PLShipComponent PLMegaTurret => (PLShipComponent)new MainTurrets.Disassembler();
        }

        private class ImpGlaiveMod : MegaTurretMod
        {
            public override string Name => "Imperial Glaive";

            public override PLShipComponent PLMegaTurret => (PLShipComponent)new MainTurrets.ImpGlaive();
        }

        private class WDStandardMod : MegaTurretMod
        {
            public override string Name => "WD Standard";

            public override PLShipComponent PLMegaTurret => (PLShipComponent)new MainTurrets.WDStandard();
        }

        private class GuardianMainTurretMod : MegaTurretMod
        {
            public override string Name => "GuardianMainTurret";

            public override PLShipComponent PLMegaTurret => (PLShipComponent)new MainTurrets.GuardianMainTurret();
        }

        private class RipperTurretMod : MegaTurretMod
        {
            public override string Name => "Ripper Turret";

            public override PLShipComponent PLMegaTurret => new MainTurrets.RipperTurrret();
        }

        private class PhysicalTurretMod : MegaTurretMod
        {
            public override string Name => "Physical Main Turret";

            public override PLShipComponent PLMegaTurret => new MainTurrets.PhysicalTurret();
        }

        private class AutoLaserMod : AutoTurretMod
        {
            public override string Name => "Auto Laser Turret";

            public override PLShipComponent PLAutoTurret => (PLShipComponent)new AutoTurrets.AutoLaser();
        }

        private class AncientAutoLaserMod : AutoTurretMod
        {
            public override string Name => "Ancient Auto Laser Turret";

            public override PLShipComponent PLAutoTurret => (PLShipComponent)new AutoTurrets.AncientAutoLaser();
        }

        internal class AuxTurrets
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
                    this.m_MaxPowerUsage_Watts = 7000f;
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

                protected override string GetDamageTypeString() => "PHYSICAL";

                public override void Tick()
                {
                    base.Tick();
                    if (this.IsEquipped && !this.ColorCorrected)
                        this.CorrectColors();
                    if (!((UnityEngine.Object)this.TurretInstance != (UnityEngine.Object)null))
                    {
                        return;
                    }

                    this.CalculatedMaxPowerUsage_Watts = 7000f * this.LevelMultiplier(0.1f);
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

                protected override string GetTurretPrefabPath() => "NetworkPrefabs/Component_Prefabs/PhaseTurret";

                protected override string GetDamageTypeString() => "PHYSICAL (BEAM)";

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

            internal class SylvassiTurret : PLLaserTurret
            {
                protected bool ColorCorrected;
                public SylvassiTurret(int inLevel = 0, int inSubTypeData = 0) : base()
                {
                    this.Name = "Sylvassi Turret";
                    this.Desc = "Long range laser weapon tuned to a frequency that can ignore enemy shields.";
                    this.m_Damage = 35f;
                    this.FireDelay = 4.5f;
                    this.SubType = TurretModManager.Instance.GetTurretIDFromName(this.Name);
                    this.m_MarketPrice = (ObscuredInt)10800;
                    this.Level = inLevel;
                    this.SubTypeData = (short)inSubTypeData;
                    this.TurretRange = 8500f;
                    this.CargoVisualPrefabID = 3;
                    this.AutoAimPowerUsageRequest = 0.5f;
                    this.HeatGeneratedOnFire = 0.15f;
                    this.PlayShootSFX = "play_ship_generic_external_weapon_laser_shoot";
                    this.StopShootSFX = "";
                    this.PlayProjSFX = "play_ship_generic_external_weapon_laser_projectile";
                    this.StopProjSFX = "stop_ship_generic_external_weapon_laser_projectile";
                    this.UpdateMaxPowerUsageWatts();
                    this.AutoAim_CanTargetMissiles = true;
                    this.ColorCorrected = false;
                }

                protected virtual void CorrectColors()
                {
                    if (!(this.TurretInstance != null && this.TurretInstance.BeamObjectRenderer != null && this.TurretInstance.OptionalGameObjects.Length > 0))
                        return;
                    this.TurretInstance.BeamObjectRenderer.material.SetColor("_Color", new Color(0f, 0f, 14f, 1f));
                    this.TurretInstance.BeamObjectRenderer.material.SetColor("_EmissionColor", new Color(0.7279f, 0.7279f, 99f, 1f));
                    ParticleSystem particleSystem = this.TurretInstance.OptionalGameObjects[0].GetComponentInChildren<ParticleSystem>();
                    if (particleSystem != null)
                        particleSystem.startColor = new Color(0f, 0f, 0.7961f, 0.5176f);
                    this.ColorCorrected = true;
                }

                public override void Tick()
                {
                    base.Tick();
                    if (this.IsEquipped && !this.ColorCorrected)
                        this.CorrectColors();
            }

                public override void Unequip()
                {
                    this.ColorCorrected = false;
                    base.Unequip();
                }
            }

            [HarmonyPatch(typeof(PLShipStats), "TakeShieldDamage")]
            internal class SylvassiTurretDamage
            {
                private static bool Prefix(PLShipStats __instance, float inDmg, EDamageType dmgType, float DT_ShieldBoost, float shieldDamageMod, PLTurret turret, ref float __result)
                {
                    if (turret != null && turret.SubType == TurretModManager.Instance.GetTurretIDFromName("Sylvassi Turret") && __instance.Ship != null && !__instance.Ship.IsSupershieldActive())
                    {
                        __result = inDmg;
                        return false;
                    }
                    return true;
                }
            }

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

                public ProjBeamCounter GetCounterForProjID(int inProjID)
                {
                    if (this.ProjBeamCounters.ContainsKey(inProjID))
                        return this.ProjBeamCounters[inProjID];
                    ProjBeamCounter counterForProjId = new ProjBeamCounter(inProjID);
                    this.ProjBeamCounters.Add(inProjID, counterForProjId);
                    return counterForProjId;
                }

                protected override string GetDamageTypeString() => "ENERGY (BEAM)";
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

                /*protected override void UpdatePowerUsage(PLPlayer currentOperator)
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

                protected override float GetDPS() => base.GetDPS() * this.DamageChecksPerSecond;

                protected override void CheckFire()
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

                protected override bool ShouldAIFire(bool operatedByBot, float heatOffset, float heatGenOnFire)
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

                protected override string GetTurretPrefabPath() => "NetworkPrefabs/Component_Prefabs/LaserTurret";

            }

            internal class MiningLaser : HeldLaserTurret
            {
                public MiningLaser(int inLevel = 0, int inSubTypeData = 0) : base(inLevel, inSubTypeData)
                {
                    this.Name = "Mining Laser";
                    this.Desc = "High-Powered laser designed to extract resources from asteroids. It is also effective against ship hulls and missiles.";
                    this.m_Damage = 37f;
                    this.SubType = TurretModManager.Instance.GetTurretIDFromName(this.Name);
                    this.m_MarketPrice = (ObscuredInt)11000;
                    this.HeatGeneratedOnFire = 0.35f;
                    this.HitCombo = true;
                    this.HitComboCountMax = 5;
                    this.HitComboMultiplier = 0.2f;
                    this.CanBeDroppedOnShipDeath = false;
                }

                protected override void CorrectColors()
                {
                    if (!(this.TurretInstance != null && this.TurretInstance.BeamObjectRenderer != null && this.TurretInstance.OptionalGameObjects.Length > 0))
                        return;
                    this.TurretInstance.BeamObjectRenderer.material.SetColor("_Color", new Color(14f, 4.2f, 0f, 1f));
                    this.TurretInstance.BeamObjectRenderer.material.SetColor("_EmissionColor", new Color(99f, 27.7f, 0.7279f, 1f));
                    ParticleSystem particleSystem = this.TurretInstance.OptionalGameObjects[0].GetComponentInChildren<ParticleSystem>();
                    if (particleSystem != null)
                        particleSystem.startColor = new Color(0.7961f, 0.2388f, 0f, 0.5176f);
                    base.CorrectColors();
                }

                public override void Tick()
                {
                    base.Tick();
                    if (this.ShipStats.Ship.IsDrone)
                        this.HeatGeneratedOnFire = 0.2f;
                }
            }

            internal class SeekerTurret : PLTurret
            {
                private PLShipInfoBase currentTargetShip;
                public SeekerTurret(int inLevel = 0, int inSubTypeData = 0)
                {
                    this.Name = "Seeker Turret";
                    this.Desc = "A turret that shoots tracking projectiles that can hit even the most evasive ships.";
                    this.m_Damage = 20f;
                    this.FireDelay = 0.7f;
                    this.MinFireDelay = 0.4f;
                    this.SubType = TurretModManager.Instance.GetTurretIDFromName(this.Name);
                    this.m_MarketPrice = (ObscuredInt)9000;
                    this.ProjSpeed = 600f;
                    this.TurretRange = 6000f;
                    this.CargoVisualPrefabID = 3;
                    this.Level = inLevel;
                    this.HeatGeneratedOnFire = 0.17f;
                    this.m_MaxPowerUsage_Watts = 5500f;
                }

                public override void Fire(int inProjID, Vector3 dir)
                {
                    this.LastFireTime = Time.time;
                    this.ChargeAmount = 0.0f;
                    PLMusic.PostEvent("play_abyss_simple_shot_sub", this.TurretInstance.gameObject);
                    if ((double)Time.time - (double)this.ShipStats.Ship.LastCloakingSystemActivatedTime > 2.0)
                        this.ShipStats.Ship.SetIsCloakingSystemActive(false);
                    GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.TurretInstance.Proj, this.TurretInstance.FiringLoc.transform.position, this.TurretInstance.FiringLoc.transform.rotation);
                    gameObject.AddComponent(typeof(PLMissle));
                    PLMissle component1 = gameObject.GetComponent<PLMissle>();
                    PLProjectile component2 = gameObject.GetComponent<PLProjectile>();
                    component1.AccelerationFactor = 0.1f;
                    component1.MissileFlag = true;
                    component2.MissileFlag = false;
                    component1.ExplosionPrefab = component2.ExplosionPrefab;
                    component1.DmgRadius = 0f;
                    gameObject.GetComponent<Rigidbody>().velocity = this.ShipStats.Ship.Exterior.GetComponent<Rigidbody>().velocity + dir * this.m_ProjSpeed;
                    component2.ProjID = inProjID;
                    component1.MaxDamage = this.m_Damage * this.LevelMultiplier(0.15f) * this.ShipStats.TurretDamageFactor;
                    component1.Damage = this.m_Damage * this.LevelMultiplier(0.15f) * this.ShipStats.TurretDamageFactor;
                    component2.MaxLifetime = 8f;
                    component1.MaxLifetime = 8f;
                    component2.OwnerShipID = this.ShipStats.Ship.ShipID;
                    component1.TurnFactor = 1f;
                    if (currentTargetShip != null)
                        component1.TargetShipID = currentTargetShip.ShipID;
                    component2.TurretID = this.TurretID;
                    component1.TargetShip = this.currentTargetShip;
                    component2.ExplodeOnMaxLifetime = false;
                    component2.MyDamageType = EDamageType.E_PHYSICAL;
                    component1.TrackingDelay = 0f;
                    component1.Speed = this.ProjSpeed;
                    component1.SetShouldLeadTargetShip(true);
                    component1.SetLerpedSpeed(this.ShipStats.Ship.ExteriorRigidbody.velocity.magnitude * 1.5f);
                    Physics.IgnoreCollision(this.ShipStats.Ship.Exterior.GetComponent<Collider>(), gameObject.GetComponent<Collider>());
                    PLServer.Instance.m_ActiveProjectiles.Add(component2);
                    this.Heat += this.HeatGeneratedOnFire;
                    this.TurretInstance.GetComponent<Animation>().Play(this.TurretInstance.FireAnimationName);
                    foreach (AnimationState animationState in this.TurretInstance.GetComponent<Animation>())
                        animationState.speed = this.FireDelay * 0.95f;
                }

                public override void Tick()
                {
                    base.Tick();
                    if (!this.IsEquipped)
                        return;
                    PLShipInfoBase potentialTarget = null;
                    if (this.GetCurrentOperator() != null && !this.GetCurrentOperator().IsBot && PLNetworkManager.Instance.LocalPlayer != null && this.GetCurrentOperator() == PLNetworkManager.Instance.LocalPlayer)
                        potentialTarget = Systems.TurretTargetingUI(this);
                    if (potentialTarget != null)
                        currentTargetShip = potentialTarget;
                    else
                        currentTargetShip = this.ShipStats.Ship.TargetShip;
                }

                protected override string GetTurretPrefabPath() => "NetworkPrefabs/Component_Prefabs/Turret";
            }

            public static bool IsSeekerProj(PLProjectile projectile)
            {
                if (projectile.OwnerShipID != -1 && projectile.TurretID != -1)
                {
                    PLShipInfoBase ship = PLEncounterManager.Instance.GetShipFromID(projectile.OwnerShipID);
                    PLTurret turret = ship.GetTurretAtID(projectile.TurretID);
                    if (turret is SeekerTurret)
                        return true;
                }
                return false;
            }

            [HarmonyPatch(typeof(PLMissle), "FixedUpdate")]
            internal class SeekerProjFix
            {
                private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
                {
                    Label failed = generator.DefineLabel();
                    Label succeed = generator.DefineLabel();

                    List<CodeInstruction> list = instructions.ToList();

                    List<CodeInstruction> targetSequence = new List<CodeInstruction>() {
                    new CodeInstruction(OpCodes.Ldc_R4, 15f),
                };
                    List<CodeInstruction> patchSequence = new List<CodeInstruction>()
                {
                    new CodeInstruction(OpCodes.Ldarg_0),
                    CodeInstruction.Call(typeof(ExpandedGalaxy.Turrets.AuxTurrets), "IsSeekerProj", new Type[1] {typeof(PLProjectile)}),
                    new CodeInstruction(OpCodes.Brfalse_S, failed),
                    new CodeInstruction(OpCodes.Ldc_R4, 100f),
                    new CodeInstruction(OpCodes.Br_S, succeed),
                    new CodeInstruction(OpCodes.Ldc_R4, 15f),
                    new CodeInstruction(OpCodes.Nop)
                };
                    patchSequence[patchSequence.Count - 1].labels.Add(succeed);
                    patchSequence[patchSequence.Count - 2].labels.Add(failed);
                    return HarmonyHelpers.PatchBySequence(list.AsEnumerable<CodeInstruction>(), targetSequence, patchSequence, HarmonyHelpers.PatchMode.REPLACE, HarmonyHelpers.CheckMode.NONNULL, false);
                }
            }

            internal class MissileTurretMk2 : PLTurret
            {
                private PLShipInfoBase currentTargetShip = null;

                public MissileTurretMk2(int inLevel = 0, int inSubTypeData = 0)
                {
                    this.Name = "Missile Turret Mk. II";
                    this.Desc = "This iteration of the missile turret has an upgraded targeting system that allows for autonomous missile firing.";
                    this.Level = inLevel;
                    this.m_Damage = 0f;
                    this.SetFireDelay();
                    this.SubType = TurretModManager.Instance.GetTurretIDFromName(this.Name);
                    this.m_MarketPrice = (ObscuredInt)12300;
                    this.TurretRange = 8000f;
                    this.CargoVisualPrefabID = 3;
                    this.HeatGeneratedOnFire = 0f;
                    this.HasTrackingMissileCapability = true;
                    this.TrackerMissileReloadTime = 0f;
                }

                private float UpdateBaseDamage()
                {
                    PLTrackerMissile missile = this.ShipStats.GetComponentFromNetID<PLTrackerMissile>(this.ShipStats.Ship.SelectedMissileLauncher.NetID);
                    if (missile != null)
                    {
                        return missile.Damage;
                    }
                    return 0f;
                }

                private void SetFireDelay()
                {
                    this.FireDelay = 20f - 0.8f * this.Level;
                    this.FireDelay = Mathf.Clamp(this.FireDelay, 10f, float.MaxValue);
                }

                public override void Fire(int inProjID, Vector3 dir)
                {
                    this.LastFireTime = Time.time;
                    if ((double)Time.time - (double)this.ShipStats.Ship.LastCloakingSystemActivatedTime > 2.0)
                        this.ShipStats.Ship.SetIsCloakingSystemActive(false);
                    bool flag = currentTargetShip != null;
                    PLTrackerMissile missile = this.ShipStats.GetComponentFromNetID<PLTrackerMissile>(this.ShipStats.Ship.SelectedMissileLauncher.NetID);
                    if (missile != null)
                        flag &= missile.SubTypeData > (short)0;
                    else
                        flag = false;
                    if (flag)
                    {
                        this.ChargeAmount = 0f;
                        PLMusic.PostEvent("play_sx_ship_generic_external_weapon_rocket_shoot", this.TurretInstance.gameObject);
                        if (PhotonNetwork.isMasterClient)
                        {
                            PLServer.Instance.photonView.RPC("MegaTurret_MissileLaunch", PhotonTargets.MasterClient, (object)this.ShipStats.Ship.ShipID, (object)this.NetID, (object)this.ShipStats.Ship.SelectedMissileLauncher.NetID, (object)currentTargetShip.ShipID);
                            this.LastFireMissileTime = Time.time;
                        }
                    }
                    else
                    {
                        this.ChargeAmount = 0.9f;
                        PLMusic.PostEvent("play_sx_ship_generic_external_weapon_rocket_failed", this.TurretInstance.gameObject);
                    }                    
                    this.TurretInstance.GetComponent<Animation>().Play(this.TurretInstance.FireAnimationName);
                    foreach (AnimationState animationState in this.TurretInstance.GetComponent<Animation>())
                        animationState.speed = this.FireDelay * 0.95f;
                }

                public override void Tick()
                {
                    UpdateBaseDamage();
                    SetFireDelay();
                    base.Tick();
                    if (!this.IsEquipped)
                        return;
                    PLShipInfoBase potentialTarget = null;
                    if (this.GetCurrentOperator() != null && !this.GetCurrentOperator().IsBot && PLNetworkManager.Instance.LocalPlayer != null && this.GetCurrentOperator() == PLNetworkManager.Instance.LocalPlayer)
                        potentialTarget = Systems.TurretTargetingUI(this);
                    if (potentialTarget != null)
                        currentTargetShip = potentialTarget;
                    else
                        currentTargetShip = this.ShipStats.Ship.TargetShip;
                    if (this.LockedOnAmount > 0f)
                        this.LockedOnAmount = 0f;
                }

                protected override void OnTurretInstanceCreated()
                {
                    base.OnTurretInstanceCreated();
                    this.TurretInstance.OptionalSecondaryFiringLoc = this.TurretInstance.FiringLoc;
                }

                protected override bool ShouldAIFire(bool operatedByBot, float heatOffset, float heatGeneratedOnFire)
                {
                    return currentTargetShip != null && base.ShouldAIFire(operatedByBot, heatOffset, heatGeneratedOnFire);
                }
                protected override string GetTurretPrefabPath() => "NetworkPrefabs/Component_Prefabs/Turret";
            }

            [HarmonyPatch(typeof(PLRailgunTurret), MethodType.Constructor, new Type[2] {typeof(int), typeof(int)})]
            internal class RailgunTurretFix
            {
                private static void Postfix(PLRailgunTurret __instance, int inLevel, int inSubTypeData)
                {
                    __instance.ProjSpeed = 3600f;
                    __instance.m_Damage = 55f;
                }
            }

            [HarmonyPatch(typeof(PLBasicTurret), MethodType.Constructor, new Type[2] { typeof(int), typeof(int) })]
            internal class PlasmaTurretFix
            {
                private static void Postfix(PLRailgunTurret __instance, int inLevel, int inSubTypeData)
                {
                    __instance.m_Damage = 150f;
                    Traverse traverse = Traverse.Create(__instance);
                    traverse.Field("FireDelay").SetValue(5f);
                    traverse.Field("TurretRange").SetValue(4500f);
                    traverse.Field("HeatGeneratedOnFire").SetValue(0.45f);
                    __instance.Desc = "A standard armament that can be found on many ships. It fires bolts of volatile plasma that detonates on contact or when it reaches max range.";
                }
            }

            [HarmonyPatch(typeof(PLTurret), "GetDamageTypeString")]
            internal class ExplosionDamageString
            {
                private static void Postfix(PLTurret __instance, ref string __result)
                {
                    if (__instance.SubType == (int)ETurretType.PLASMA || __instance.SubType == (int)ETurretType.BURST || __instance.SubType == (int)ETurretType.DEFENDER)
                        __result = "PHYS (EXPLD)";
                }
            }

            [HarmonyPatch(typeof(PLFocusLaserTurret), MethodType.Constructor, new Type[2] { typeof(int), typeof(int) })]
            internal class FocusLaserDamageNerf
            {
                private static void Postfix(PLFocusLaserTurret __instance)
                {
                    __instance.m_Damage = 80f;
                }
            }

            [HarmonyPatch(typeof(PLLightningTurret), MethodType.Constructor, new Type[2] { typeof(int), typeof(int) })]
            internal class LightningDamageNerf
            {
                private static void Postfix(PLFocusLaserTurret __instance)
                {
                    __instance.m_Damage = 30f;
                }
            }

            [HarmonyPatch(typeof(PLLightningTurret), "Tick")]
            internal class LightningDamageShockDrone
            {
                private static void Postfix(PLLightningTurret __instance)
                {
                    if (__instance.IsEquipped && __instance.m_Damage != 50f && __instance.ShipStats.Ship != null && __instance.ShipStats.Ship.ShipTypeID == EShipType.E_SHOCK_DRONE)
                        __instance.m_Damage = 50f;
                }
            }

            [HarmonyPatch(typeof(PLDefenderTurret), "UpdateMaxPowerUsageWatts")]
            internal class DefenderPowerFix
            {
                private static bool Prefix(PLDefenderTurret __instance)
                {
                    __instance.CalculatedMaxPowerUsage_Watts = 7600f;
                    return false;
                }
            }
        }

        internal class MainTurrets
        {
            internal class Disassembler : AuxTurrets.HeldLaserTurret
            {
                protected Color BeamColor = Color.red;
                public Disassembler(int inLevel = 0, int inSubTypeData = 1)
        : base(inLevel, inSubTypeData)
                {
                    this.Name = "The Disassembler";
                    this.Desc = "Powerful beam weapon that uses its target's hull debris to repair its own ship.";
                    this.m_Damage = 100f;
                    this.SubType = MegaTurretModManager.Instance.GetMegaTurretIDFromName("The Disassembler");
                    this.m_MarketPrice = (ObscuredInt)9200;
                    this.CargoVisualPrefabID = 5;
                    this.FireDelay = 1f;
                    this.m_SlotType = ESlotType.E_COMP_MAINTURRET;
                    this.HeatGeneratedOnFire = 0.2f;
                    this.DamageChecksPerSecond = 2f;
                    this.m_IconTexture = (Texture2D)Resources.Load("Icons/8_Weapons");
                    this.HasTrackingMissileCapability = true;
                    this.TrackerMissileReloadTime = 9f;
                    this.TurretRange = 15000f;
                    this.m_AutoAimMinDotPrd = 0.99f;
                    this.m_Laser_xzScale = 300f;
                    this.m_Laser_yScale = 100f;
                    this.AutoAimEnabled = true;
                    this.IsMainTurret = true;
                    this.BeamColor = new Color(1f, 1f, 0.05f);
                    this.HitComboCountMax = 1;
                    this.HitComboMultiplier = 0.8f;
                }

                public override void UpdateMaxPowerUsageWatts() => this.CalculatedMaxPowerUsage_Watts = 11800f;

                protected override void CorrectColors()
                {
                    if (!((UnityEngine.Object)this.TurretInstance != (UnityEngine.Object)null) || !((UnityEngine.Object)this.TurretInstance.BeamObjectRenderer != (UnityEngine.Object)null))
                        return;
                    if ((UnityEngine.Object)this.TurretInstance.BeamObjectRenderer == (UnityEngine.Object)null)
                        return;
                    this.TurretInstance.BeamObjectRenderer.material.SetColor("_LaserColor", this.BeamColor);
                    if (this.TurretInstance.OptionalGameObjects[0] == null)
                        return;
                    this.TurretInstance.OptionalGameObjects[0].transform.Find("Charge (1)").GetComponent<ParticleSystem>().startColor = this.BeamColor;
                    this.TurretInstance.OptionalGameObjects[0].transform.Find("Point light (1)").GetComponent<Light>().color = this.BeamColor;
                    base.CorrectColors();
                }

                protected override bool AutoAimTrackingIsEnabled()
                {
                    PLPlayer playerFromPlayerId = PLServer.Instance.GetPlayerFromPlayerID(this.ShipStats.Ship.GetCurrentTurretControllerPlayerID(this.TurretID));
                    bool flag = false;
                    if (playerFromPlayerId != null && playerFromPlayerId.IsBot)
                        flag = true;
                    return flag || this.ShipStats.Ship.IsDrone;
                }

                protected override string GetTurretPrefabPath() => "NetworkPrefabs/Component_Prefabs/MegaTurret3";
            }

            [HarmonyPatch(typeof(PLShipStats), "TakeHullDamage")]
            internal class DisassemblerHeal
            {
                private static void Postfix(PLShipStats __instance, PLShipInfoBase attackingShip, PLTurret turret, ref float __result)
                {
                    if (!PhotonNetwork.isMasterClient)
                        return;
                    if (attackingShip != null && turret != null && turret.SubType == MegaTurretModManager.Instance.GetMegaTurretIDFromName("The Disassembler"))
                    {
                        float healNum = __result * 0.2f;
                        PLHull hull = attackingShip.MyHull;
                        if (hull != null)
                        {
                            if (hull.SubType == (int)EHullType.E_NANO_ACTIVE_HULL || hull.SubType == (int)EHullType.E_POLYTECH_HULL)
                                healNum *= 4f;
                            PLServer.Instance.ClientRepairHull(attackingShip.ShipID, (int)healNum, 0);
                            PLServer.Instance.photonView.RPC("ClientRepairHull", PhotonTargets.Others, attackingShip.ShipID, (int)healNum, 0);
                        }
                    }
                }
            }

            internal class ImpGlaive : PLMegaTurret
            {
                internal bool tickCharge;
                private bool ColorCorrected;
                public ImpGlaive(int inLevel = 0, int inSubTypeData = 0) : base(inLevel)
                {
                    this.Name = "Imperial Glaive";
                    this.Desc = "Turret with tremendous firepower that charges quicker when the hull takes damage. It was the foundational building block of a long-fallen empire.";
                    this.m_Damage = 650f;
                    this.SubType = MegaTurretModManager.Instance.GetMegaTurretIDFromName("Imperial Glaive");
                    this.m_MarketPrice = (ObscuredInt)40000;
                    this.FireDelay = 32f;
                    this.m_MaxPowerUsage_Watts = 10800f;
                    this.TurretRange = 13000f;
                    this.BeamColor = new Color(58f / 255f, 0f, 175f / 255f);
                    this.m_KickbackForceMultiplier = 1.2f;
                    this.HeatGeneratedOnFire = 0.6f;
                    this.CoolingRateModifier *= 2f;
                    Traverse.Create((PLMegaTurret)this).Field("turretChargeSpeed_ToVisualChargeSpeed").SetValue(0.234375f);
                }

                protected virtual void CorrectColors()
                {
                    if (!(this.TurretInstance != null))
                        return;
                    foreach (Light light in this.TurretInstance.GetComponentsInChildren<Light>(true))
                    {
                        if (light.gameObject != null && light.gameObject.name != "TurretLight")
                        {
                            Color color = this.BeamColor;
                            color.a = light.color.a;
                            light.color = color;
                        }
                    }
                    foreach (ParticleSystem particleSystem in this.TurretInstance.GetComponentsInChildren<ParticleSystem>(true))
                    {
                        Color color = this.BeamColor;
                        color.a = particleSystem.startColor.a;
                        particleSystem.startColor = color;
                    }
                    this.TurretInstance.GetComponentInChildren<PLIdleSound>(true).enabled = false;
                    this.ColorCorrected = true;
                }

                public override void Unequip()
                {
                    this.ColorCorrected = false;
                    base.Unequip();
                }

                public override void Tick()
                {
                    base.Tick();
                    this.m_MaxPowerUsage_Watts = 10800f * this.LevelMultiplier(0.2f);
                    if (this.IsEquipped && !this.ColorCorrected)
                        this.CorrectColors();
                    if (tickCharge)
                    {
                        tickCharge = false;
                        if (this.ChargeAmount < 1f)
                        {
                            this.ChargeAmount = Mathf.Clamp(this.ChargeAmount + 0.33f, 0f, 1f);
                        }
                    }
                }
            }

            [HarmonyPatch(typeof(PLShipStats), "TakeHullDamage")]
            internal class ImpGlaiveCharge
            {
                private static void Postfix(PLShipStats __instance, ref float __result)
                {
                    if (!PhotonNetwork.isMasterClient)
                        return;
                    if (__instance.Ship != null)
                    {
                        if ((double)__result > 0.0)
                        {
                            PLMegaTurret megaTurret = __instance.GetShipComponent<PLMegaTurret>(ESlotType.E_COMP_MAINTURRET);
                            if (!((object)megaTurret != (object)null && megaTurret.SubType == MegaTurretModManager.Instance.GetMegaTurretIDFromName("Imperial Glaive")))
                                return;
                            ModMessage.SendRPC("sugarbuzz1.ExpandedGalaxy", "ExpandedGalaxy.TickCharge", PhotonTargets.All, new object[2]
                            {
                            __instance.Ship.ShipID,
                            megaTurret.NetID
                            });
                        }
                    }
                }
            }

            private class TickCharge : ModMessage
            {
                public override void HandleRPC(object[] arguments, PhotonMessageInfo sender)
                {
                    if (!sender.sender.IsMasterClient)
                        return;
                    PLShipInfoBase shipInfo = PLEncounterManager.Instance.GetShipFromID((int)arguments[0]);
                    if (shipInfo.MyStats != null)
                    {
                        (shipInfo.MyStats.GetComponentFromNetID((int)arguments[1]) as ImpGlaive).tickCharge = true;
                    }
                }
            }

            internal class WDStandard : PLMegaTurret
            {
                protected int ShotsMax = 3;
                private bool ColorCorrected;
                private double lastUpdateTime = double.MinValue;

                public WDStandard(int inLevel = 0, int inSubTypeData = 0)
                : base(inLevel)
                {
                    this.Name = "WD Standard";
                    this.Desc = "A main turret equipped with a battery array allowing it to fire multiple times before needing to recharge. It has a max range of 7 km.";
                    this.m_Damage = 250f;
                    this.SubType = MegaTurretModManager.Instance.GetMegaTurretIDFromName("WDStandard");
                    this.m_MarketPrice = (ObscuredInt)20000;
                    this.FireDelay = 20f;
                    this.m_MaxPowerUsage_Watts = 14150f;
                    this.CargoVisualPrefabID = 5;
                    this.TurretRange = 7000f;
                    this.BeamColor = new Color(1f, 0.3f, 0f);
                    this.MegaTurretExplosionID = 0;
                    this.Level = inLevel;
                    this.m_KickbackForceMultiplier = 0.67f;
                    this.m_AutoAimMinDotPrd = 0.98f;
                    this.HeatGeneratedOnFire = 0.23f;
                    this.AutoAimEnabled = true;
                    this.IsMainTurret = true;
                    this.HasTrackingMissileCapability = true;
                    this.TrackerMissileReloadTime = 5f;
                    this.HasPulseLaser = true;
                    Traverse.Create((PLMegaTurret)this).Field("turretChargeSpeed_ToVisualChargeSpeed").SetValue(0.2625f);
                    this.ColorCorrected = false;
                    this.SubTypeData = 3;
                }

                protected virtual void CorrectColors()
                {
                    if (!(this.TurretInstance != null))
                        return;
                    foreach (Light light in this.TurretInstance.GetComponentsInChildren<Light>(true))
                    {
                        if (light.gameObject != null && light.gameObject.name != "TurretLight")
                        {
                            Color color = this.BeamColor;
                            color.a = light.color.a;
                            light.color = color;
                        }
                    }
                    foreach (ParticleSystem particleSystem in this.TurretInstance.GetComponentsInChildren<ParticleSystem>(true))
                    {
                        Color color = this.BeamColor;
                        color.a = particleSystem.startColor.a;
                        particleSystem.startColor = color;
                    }
                    this.TurretInstance.GetComponentInChildren<PLIdleSound>(true).enabled = false;
                    this.ColorCorrected = true;
                }

                public override void Unequip()
                {
                    this.ColorCorrected = false;
                    base.Unequip();
                }

                public override void Fire(int inProjID, Vector3 dir)
                {
                    bool flag = this.IsCharging;
                    base.Fire(inProjID, dir);
                    if (flag)
                        return;
                    if ((int)this.SubTypeData < this.ShotsMax - 1)
                        this.ChargeAmount = 1f;
                }
                protected override void ChargeComplete(int inProjID, Vector3 dir)
                {
                    base.ChargeComplete(inProjID, dir);
                    ++this.SubTypeData;
                }

                protected override string GetInfoString() => "    " + (this.ShotsMax - this.SubTypeData).ToString() + "/" + this.ShotsMax.ToString();

                public override void Tick()
                {
                    if (this.SubTypeData >= this.ShotsMax && (double)this.ChargeAmount > 0.99f)
                        this.SubTypeData = 0;
                    base.Tick();
                    this.m_MaxPowerUsage_Watts = 14150f * this.LevelMultiplier(0.2f);
                    if (this.IsEquipped && !this.ColorCorrected)
                        this.CorrectColors();
                    /*if (this.IsEquipped && PhotonNetwork.isMasterClient && (double)Time.time - this.lastUpdateTime > 4.0)
                    {
                        ModMessage.SendRPC("sugarbuzz1.ExpandedGalaxy", "ExpandedGalaxy.UpdateSubTypeData", PhotonTargets.Others, new object[3]
                    {
                        (object) this.ShipStats.Ship.ShipID,
                        (object) this.NetID,
                        (object) this.SubTypeData,
                    });
                        this.lastUpdateTime = (double)Time.time;
                    }*/
                }

                protected override void UpdatePowerUsage(PLPlayer currentOperator)
                {
                    base.UpdatePowerUsage(currentOperator);
                    if ((double)this.ChargeAmount >= 1.0 && (double)this.Heat > 0.0)
                    {
                        this.m_RequestPowerUsage_Percent = this.ShipStats.Ship.WeaponsSystem.GetHealthRatio() * 0.33f;
                        this.IsPowerActive = true;
                    }

                }

                protected override bool ShouldAIFire(bool operatedByBot, float heatOffset, float heatGeneratedOnFire)
                {
                    return operatedByBot && this.Heat + heatGeneratedOnFire < 0.90f && this.SubTypeData < 3;
                }
            }

            internal class GuardianMainTurret : PLMegaTurret
            {
                private bool ColorCorrected;
                public GuardianMainTurret(int inLevel = 0, int inSubTypeData = 0) : base(inLevel)
                {
                    this.Name = "Guardian Main Turret";
                    this.Desc = "The main turret built for the Guardian Protection Drone that prioritizes disabling enemy ships. It has a max range of 8 km.";
                    this.m_Damage = 305f;
                    this.DamageType = EDamageType.E_PHASE;
                    this.SubType = MegaTurretModManager.Instance.GetMegaTurretIDFromName("GuardianMainTurret");
                    this.m_MarketPrice = (ObscuredInt)19200;
                    this.FireDelay = 9f;
                    this.m_MaxPowerUsage_Watts = 8300f;
                    this.CargoVisualPrefabID = 5;
                    this.TurretRange = 8000f;
                    this.BeamColor = Color.green;
                    this.MegaTurretExplosionID = 0;
                    this.Level = inLevel;
                    this.m_AutoAimMinDotPrd = 0.99f;
                    this.HeatGeneratedOnFire = 0.55f;
                    this.AutoAimEnabled = true;
                    this.IsMainTurret = true;
                    this.HasTrackingMissileCapability = true;
                    this.TrackerMissileReloadTime = 9f;
                    this.HasPulseLaser = true;
                    this.ColorCorrected = false;
                }

                protected virtual void CorrectColors()
                {
                    if (!(this.TurretInstance != null))
                        return;
                    foreach (Light light in this.TurretInstance.GetComponentsInChildren<Light>(true))
                    {
                        if (light.gameObject != null && light.gameObject.name != "TurretLight")
                        {
                            Color color = this.BeamColor;
                            color.a = light.color.a;
                            light.color = color;
                        }
                    }
                    foreach (ParticleSystem particleSystem in this.TurretInstance.GetComponentsInChildren<ParticleSystem>(true))
                    {
                        Color color = this.BeamColor;
                        color.a = particleSystem.startColor.a;
                        particleSystem.startColor = color;
                    }
                    this.TurretInstance.GetComponentInChildren<PLIdleSound>(true).enabled = false;
                    this.ColorCorrected = true;
                }

                public override void Tick()
                {
                    base.Tick();
                    this.m_MaxPowerUsage_Watts = 8300f * this.LevelMultiplier(0.2f);
                    if (this.IsEquipped && !this.ColorCorrected)
                        this.CorrectColors();
                }

                public override void Unequip()
                {
                    this.ColorCorrected = false;
                    base.Unequip();
                }

                protected override string GetDamageTypeString() => "PHASE (BEAM)";
            }

            internal class RipperTurrret : PLMegaTurret_Proj
            {
                private int ProjectilesPerShot = 8;
                public RipperTurrret(int inLevel = 0, int inSubTypeData = 1) : base(inLevel, inSubTypeData)
                {
                    this.Name = "Ripper Turret";
                    this.Desc = "Main turret that shoots several projectiles that tear through both hull and shields alike.";
                    this.m_Damage = 42f;
                    this.FireDelay = 0.9f;
                    this.MinFireDelay = 0.5f;
                    this.HeatGeneratedOnFire = 0.28f;
                    this.m_MaxPowerUsage_Watts = 12000f;
                    this.SubType = MegaTurretModManager.Instance.GetMegaTurretIDFromName(this.Name);
                    this.m_MarketPrice = (ObscuredInt)17800;
                    this.TurretRange = 4700f;
                }

                protected override float GetDPS() => base.GetDPS() * ProjectilesPerShot;

                public override void Fire(int inProjID, Vector3 dir)
                {
                    this.ChargeAmount = 0.0f;
                    this.LastFireTime = Time.time;
                    PLMusic.PostEvent("play_ship_generic_external_weapon_scattergun_shoot_first_second", this.TurretInstance.gameObject);
                    this.Heat += this.HeatGeneratedOnFire;
                    PLRand plRand = new PLRand(inProjID);
                    Collider[] colliderArray = new Collider[this.ProjectilesPerShot];
                    if (this.TurretInstance != null)
                        this.TurretInstance.GetComponent<Animation>().Play(this.TurretInstance.FireAnimationName);
                    for (int index1 = 0; index1 < this.ProjectilesPerShot; ++index1)
                    {
                        Vector3 normalized = new Vector3(plRand.Next(-1f, 1f), plRand.Next(-1f, 1f), plRand.Next(-1f, 1f)).normalized;
                        GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.TurretInstance.Proj, this.TurretInstance.FiringLoc.transform.position, this.TurretInstance.FiringLoc.transform.rotation);
                        gameObject.GetComponent<Rigidbody>().velocity = this.ShipStats.Ship.Exterior.GetComponent<Rigidbody>().velocity + dir * this.m_ProjSpeed + normalized * this.m_ProjSpeed * 0.1f * (float)plRand.NextDouble() * Mathf.Clamp((float)plRand.NextDouble(), 0.5f, 1f);
                        gameObject.GetComponent<PLProjectile>().ProjID = inProjID + index1;
                        gameObject.GetComponent<PLProjectile>().Damage = this.m_Damage * this.LevelMultiplier(0.15f) * this.ShipStats.TurretDamageFactor;
                        gameObject.GetComponent<PLProjectile>().MaxLifetime = 3f;
                        gameObject.GetComponent<PLProjectile>().OwnerShipID = this.ShipStats.Ship.ShipID;
                        gameObject.GetComponent<PLProjectile>().TurretID = this.TurretID;
                        gameObject.GetComponent<PLProjectile>().ExplodeOnMaxLifetime = false;
                        gameObject.GetComponent<PLProjectile>().MyDamageType = EDamageType.E_PHYSICAL;
                        if (this.ShipStats.Ship.GetExteriorMeshCollider() != null)
                            Physics.IgnoreCollision((Collider)this.ShipStats.Ship.GetExteriorMeshCollider(), gameObject.GetComponent<Collider>());
                        colliderArray[index1] = gameObject.GetComponent<Collider>();
                        for (int index2 = index1 - 1; index2 >= 0; --index2)
                            Physics.IgnoreCollision(colliderArray[index1], colliderArray[index2]);
                        PLServer.Instance.m_ActiveProjectiles.Add(gameObject.GetComponent<PLProjectile>());
                    }
                    ++this.CurrentCameraShake;
                    if ((double)Time.time - (double)this.ShipStats.Ship.LastCloakingSystemActivatedTime <= 2.0)
                        return;
                    this.ShipStats.Ship.SetIsCloakingSystemActive(false);
                }

                protected override void InnerCheckFire()
                {
                    PLServer.Instance.photonView.RPC("TurretFire", PhotonTargets.All, (object)this.ShipStats.Ship.ShipID, (object)this.TurretID, (object)PLServer.Instance.ServerProjIDCounter, (object)Vector3.zero);
                    PLServer.Instance.ServerProjIDCounter += this.ProjectilesPerShot;
                }
            }

            internal class PhysicalTurret : PLMegaTurret_Proj
            {
                private float LastSwitchedFiringMode;

                public PhysicalTurret(int inLevel = 0, int inSubTypeData = 1) : base(inLevel, inSubTypeData)
                {
                    this.Name = "Coil Artillery";
                    this.Desc = "A projectile-based main turret with two modes of fire. Despite being develpoed before the widespread use of laser weaponry, it is still commonly found on modern starships.";
                    this.m_Damage = 80f;
                    this.FireDelay = 2f;
                    this.SubType = MegaTurretModManager.Instance.GetMegaTurretIDFromName(this.Name);
                    this.SubTypeData = 0;
                    this.TurretRange = 10000f;
                    this.m_MaxPowerUsage_Watts = 11000f;
                    this.AbyssProjDisplay = true;
                    this.HasPulseLaser = false;
                    this.CanHitMissiles = false;
                }

                private void UpdateFiringMode()
                {
                    if (this.SubTypeData == 0)
                    {
                        this.m_Damage = 80f;
                        this.FireDelay = 2f;
                        this.ProjSpeed = 6000f * 0.7f;
                        this.TurretRange = 10000f;
                        this.HeatGeneratedOnFire = 0.23f;
                    }
                    else
                    {
                        this.m_Damage = 280f;
                        this.FireDelay = 6f;
                        this.ProjSpeed = 1500f * 0.7f;
                        this.TurretRange = 5000f;
                        this.HeatGeneratedOnFire = 0.48f;
                    }
                }

                public override void Tick()
                {
                    base.Tick();
                    if (this.IsEquipped)
                    {
                        if (this.ShipStats.Ship.GetCurrentTurretControllerPlayerID(this.TurretID) == PLNetworkManager.Instance.LocalPlayerID)
                        {
                            if ((double)Time.time - (double)this.LastSwitchedFiringMode > 1.0 && PLInput.Instance.GetButtonUp(PLInputBase.EInputActionName.right_click) && (double)PLInput.Instance.GetHeldDownTime(PLInputBase.EInputActionName.right_click) < 0.15000000596046448 + (double)Time.deltaTime)
                            {
                                int data = 0;
                                if (this.SubTypeData == 0)
                                {
                                    this.SubTypeData = 1;
                                    data = 1;
                                    PLMusic.PostEvent("play_sx_ship_changetransmission_manual", this.TurretInstance.gameObject);
                                }
                                else
                                {
                                    this.SubTypeData = 0;
                                    PLMusic.PostEvent("play_sx_ship_changetransmission_auto", this.TurretInstance.gameObject);
                                }
                                this.LastSwitchedFiringMode = Time.time;
                                this.ChargeAmount = 0f;
                                ModMessage.SendRPC("sugarbuzz1.ExpandedGalaxy", "ExpandedGalaxy.SwitchTurretModeRPC", PhotonTargets.Others, new object[3]
                                {
                                (object) this.ShipStats.Ship.ShipID,
                                (object) this.NetID,
                                (object) (short)data,
                                });
                            }
                        }
                        else if (this.GetCurrentOperator() != null && this.GetCurrentOperator().IsBot)
                        {
                            if ((double)Time.time - (double)this.LastSwitchedFiringMode > 5.0)
                            {
                                if (this.ShipStats.Ship.TargetSpaceTarget != null && (this.ShipStats.Ship.TargetSpaceTarget.GetCurrentSensorPosition() - this.ShipStats.Ship.GetCurrentSensorPosition()).magnitude < 1000f)
                                {
                                    this.SubTypeData = 1;
                                    ModMessage.SendRPC("sugarbuzz1.ExpandedGalaxy", "ExpandedGalaxy.SwitchTurretModeRPC", PhotonTargets.Others, new object[3]
                                {
                                (object) this.ShipStats.Ship.ShipID,
                                (object) this.NetID,
                                (object) this.SubTypeData,
                                });
                                }
                                else if (this.SubTypeData == (short)1)
                                {
                                    this.SubTypeData = 0;
                                    ModMessage.SendRPC("sugarbuzz1.ExpandedGalaxy", "ExpandedGalaxy.SwitchTurretModeRPC", PhotonTargets.Others, new object[3]
                                {
                                (object) this.ShipStats.Ship.ShipID,
                                (object) this.NetID,
                                (object) this.SubTypeData,
                                });
                                }
                            }
                        }
                        this.UpdateFiringMode();
                    }
                }

                protected override string GetInfoString() => base.GetInfoString() + " " + ((this.SubTypeData == 0) ? "(Long Range)" : "(Short Range)");

                protected override string GetDamageTypeString() => (this.SubTypeData == 0) ? "PHYSICAL" : "PHYS (EXPLD)";

            }

            private class SwitchTurretModeRPC : ModMessage
            {
                public override void HandleRPC(object[] arguments, PhotonMessageInfo sender)
                {
                    PLShipInfoBase shipInfo = PLEncounterManager.Instance.GetShipFromID((int)arguments[0]);
                    if (shipInfo.MyStats != null)
                    {
                        PhysicalTurret turret = (PhysicalTurret)shipInfo.MyStats.GetComponentFromNetID((int)arguments[1]);
                        if (turret != null)
                        {
                            turret.SubTypeData = (short)arguments[2];
                            turret.ChargeAmount = 0f;
                        }
                    }
                }
            }

            internal class WDLong : PLMegaTurretCU
            {
                public WDLong(int inLevel = 0, int inSubTypeData = 1) : base(inLevel, inSubTypeData)
                {

                }
            }

            [HarmonyPatch(typeof(PLMegaTurret), MethodType.Constructor, new Type[1] { typeof(int)})]
            internal class MegaTurretStats
            {
                private static void Postfix(PLMegaTurret __instance, int inLevel, ref float ___m_MaxPowerUsage_Watts)
                {
                    ___m_MaxPowerUsage_Watts = 6800f;
                }
            }

            [HarmonyPatch(typeof(PLMegaTurretCU), MethodType.Constructor, new Type[2] { typeof(int), typeof(int) })]
            internal class CULongStats
            {
                private static void Postfix(PLMegaTurretCU __instance, int inLevel, int inSubTypeData, ref float ___m_MaxPowerUsage_Watts)
                {
                    ___m_MaxPowerUsage_Watts = 11500f;
                }
            }

            [HarmonyPatch(typeof(PLMegaTurretCU_2), MethodType.Constructor, new Type[2] { typeof(int), typeof(int) })]
            internal class CULong2Stats
            {
                private static void Postfix(PLMegaTurretCU_2 __instance, int inLevel, int inSubTypeData, ref float ___m_MaxPowerUsage_Watts, ref float ___TurretRange)
                {
                    ___m_MaxPowerUsage_Watts = 11500f;
                    ___TurretRange = 11000f;
                    __instance.Desc = "An impressive beam weapon that was designed for use in the Colonial Union Fleet. This one has had custom modifications for extra damage. It has a max range of 11 km. ";
                }
            }

            [HarmonyPatch(typeof(PLMegaTurret_RapidFire), MethodType.Constructor, new Type[2] { typeof(int), typeof(int) })]
            internal class RapidfireStats
            {
                private static void Postfix(PLMegaTurret_RapidFire __instance, int inLevel, int inSubTypeData, ref ObscuredInt ___m_MarketPrice, ref float ___m_MaxPowerUsage_Watts)
                {
                    ___m_MarketPrice = (ObscuredInt)19200;
                    ___m_MaxPowerUsage_Watts = 11600f;
                }
            }

            [HarmonyPatch(typeof(PLMegaTurret_WDExperiment), MethodType.Constructor, new Type[2] { typeof(int), typeof(int) })]
            internal class FlashfireStats
            {
                private static void Postfix(PLMegaTurret_WDExperiment __instance, int inLevel, int inSubTypeData, ref float ___m_MaxPowerUsage_Watts)
                {
                    ___m_MaxPowerUsage_Watts = 11600f;
                }
            }

            [HarmonyPatch(typeof(PLMegaTurret), "Tick")]
            internal class TickPowerUsage
            {
                private static void Postfix(PLMegaTurret __instance)
                {
                    switch (__instance.SubType)
                    {
                        case 0: //MAIN TURRET
                            __instance.CalculatedMaxPowerUsage_Watts = 6800f * __instance.LevelMultiplier(0.2f);
                            break;
                        case 1: //CU LONG
                        case 5: //MODIFIED CU LONG
                            __instance.CalculatedMaxPowerUsage_Watts = 11500f * __instance.LevelMultiplier(0.2f);
                            break;
                        case 3: //RAPIDFIRE
                            __instance.CalculatedMaxPowerUsage_Watts = 11600f * __instance.LevelMultiplier(0.2f);
                            break;
                        case 4: //FLASHFIRE
                            __instance.CalculatedMaxPowerUsage_Watts = 11600f * __instance.LevelMultiplier(0.1f);
                            break;
                    }
                }
            }
        }

        internal class AutoTurrets
        {
            public static void LaserAutoTurretDamage(
      int shipID,
      int inAttackingShipID,
      float damage,
      float randomNum,
      Vector3 localPosition,
      int inTurretID,
      int inProjID,
      int inBeamTickCount,
      int turretID)
            {
                PLShipInfoBase shipFromId1 = PLEncounterManager.Instance.GetShipFromID(shipID);
                PLShipInfoBase shipFromId2 = PLEncounterManager.Instance.GetShipFromID(inAttackingShipID);
                PLSpaceTarget spaceTargetFromId = PLEncounterManager.Instance.GetSpaceTargetFromID(shipID);
                if (shipFromId1 != null && shipFromId2 != null)
                {
                    if (!(shipFromId2.GetAutoTurretAtID(inTurretID) is AutoLaser autoTurretAtId) || autoTurretAtId.GetCounterForProjID(inProjID).HasProcessedBeamTickCounter(inBeamTickCount))
                        return;
                    Vector3 inWorldLoc = shipFromId1.Exterior.transform.TransformPoint(localPosition);
                    bool bottomHit = false;
                    if ((double)Vector3.Dot((inWorldLoc - shipFromId1.Exterior.transform.position).normalized, -shipFromId1.Exterior.transform.up) > -0.10000000149011612)
                        bottomHit = true;
                    PLServer.Instance.LaserTurretExplosion(inWorldLoc, shipFromId1.ShipID, autoTurretAtId.laserTurretExplosionID);
                    double damage1 = (double)shipFromId1.TakeDamage(damage, bottomHit, autoTurretAtId.LaserDamageType, randomNum, -1, shipFromId2, turretID);
                }
                else
                {
                    if (!(spaceTargetFromId != null) || !(shipFromId2 != null) || !(shipFromId2.GetAutoTurretAtID(inTurretID) is AutoLaser autoTurretAtId) || autoTurretAtId.GetCounterForProjID(inProjID).HasProcessedBeamTickCounter(inBeamTickCount))
                        return;
                    spaceTargetFromId.TakeDamage(damage);
                }
            }

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

                protected override void CheckFire()
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
                                        AutoTurrets.LaserAutoTurretDamage(plShipInfoBase.ShipID, this.ShipStats.Ship.ShipID, num1, randomNum, plShipInfoBase.Exterior.transform.InverseTransformPoint(hitInfo.point), this.AutoTurretID, this.ProjIDLast, this.BeamTickCount, this.AutoTurretID);
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
                                        AutoTurrets.LaserAutoTurretDamage(plSpaceTarget.SpaceTargetID, this.ShipStats.Ship.ShipID, num1, randomNum, plSpaceTarget.transform.InverseTransformPoint(hitInfo.point), this.AutoTurretID, this.ProjIDLast, this.BeamTickCount, this.AutoTurretID);
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

                protected override string GetTurretPrefabPath() => "NetworkPrefabs/Component_Prefabs/LaserTurret";
            }

            internal class AncientAutoLaser : AutoLaser
            {
                bool ColorCorrected = false;
                public AncientAutoLaser(int inLevel = 0, int inSubTypeData = 0)
                {
                    this.Name = "Ancient Auto Laser Turret";
                    this.Desc = "A turret recovered from the wreckage of a TriCorp sentry drone. It has been retrofitted to work autonomously on a common starship.";
                    this.m_Damage = 170f;
                    this.FireDelay = 13f;
                    this.SubType = AutoTurretModManager.Instance.GetAutoTurretIDFromName(this.Name);
                    this.BeamActiveTime = 1f;
                    this.BeamDelayTime = 2f;
                    this.PlayShootSFX = "play_sx_ship_enemy_ancientsentry_shoot";
                    this.basePowerUsage = 8500f;
                    this.m_MarketPrice = (ObscuredInt)15800;
                    this.TurretRotationLerpSpeed = 15;
                    this.beamColor = new Color(0.15862f, 1f, 0f, 1f);
                    this.beamColorScalar = 5f;
                }

                protected virtual void CorrectColors()
                {
                    if (!(this.TurretInstance != null && this.TurretInstance.BeamObjectRenderer != null && this.TurretInstance.OptionalGameObjects.Length > 0))
                        return;
                    this.TurretInstance.BeamObjectRenderer.material.SetColor("_Color", new Color(0.7931f, 5f, 0f, 5f));
                    this.TurretInstance.BeamObjectRenderer.material.SetColor("_EmissionColor", new Color(0.7931f, 5f, 0f, 5f));
                    ParticleSystem particleSystem = this.TurretInstance.OptionalGameObjects[0].GetComponentInChildren<ParticleSystem>();
                    if (particleSystem != null)
                        particleSystem.startColor = new Color(0.1262f, 0.7961f, 0f, 0.5176f);
                    this.ColorCorrected = true;
                }


                public override void Tick()
                {
                    base.Tick();
                    if (this.IsEquipped && !this.ColorCorrected)
                        this.CorrectColors();
                    if (this.IsBeamActive)
                        this.TurretRotationLerpSpeed = 1f;
                    else
                        this.TurretRotationLerpSpeed = 15f;
                }

                public override void Unequip()
                {
                    this.ColorCorrected = false;
                    base.Unequip();
                }
            }

            private class ReciveAutoLaserDamage : ModMessage
            {
                public override void HandleRPC(object[] arguments, PhotonMessageInfo sender) => AutoTurrets.LaserAutoTurretDamage((int)arguments[0], (int)arguments[1], (float)arguments[2], (float)arguments[3], (Vector3)arguments[4], (int)arguments[5], (int)arguments[6], (int)arguments[7], (int)arguments[8]);
            }

            [HarmonyPatch(typeof(PLShipInfoBase), "GetTurretAtID")]
            private class GetAutoTurret
            {
                private static void Postfix(PLShipInfoBase __instance, ref PLTurret __result, int inID)
                {
                    if (__result != null)
                        return;
                    __result = __instance.GetAutoTurretAtID(inID);
                }
            }
        }

        internal class TurretRange
        {
            [HarmonyPatch(typeof(PLTurret), "UpdateQuickShowVisuals")]
            internal class PulseLaserRange
            {
                private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
                {
                    List<CodeInstruction> list = instructions.ToList();

                    List<CodeInstruction> targetSequence = new List<CodeInstruction>() {
                    new CodeInstruction(OpCodes.Ldc_R4, 20000f),
                    new CodeInstruction(OpCodes.Stloc_S),
                };
                    List<CodeInstruction> patchSequence = new List<CodeInstruction>()
                {
                    new CodeInstruction(OpCodes.Ldarg_0),
                    CodeInstruction.LoadField(typeof(PLTurret), "TurretRange"),
                    new CodeInstruction(OpCodes.Ldc_R4, 5f),
                    new CodeInstruction(OpCodes.Div),
                    new CodeInstruction(OpCodes.Stloc_S, 12)
                };
                    patchSequence[0].labels.Add(list[263].labels[0]);

                    return HarmonyHelpers.PatchBySequence(list.AsEnumerable<CodeInstruction>(), targetSequence, patchSequence, HarmonyHelpers.PatchMode.REPLACE, HarmonyHelpers.CheckMode.NONNULL, false);
                }
            }

            [HarmonyPatch(typeof(PLLaserTurret), "Tick")]
            internal class LaserTurretRange
            {
                private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
                {
                    List<CodeInstruction> list = instructions.ToList();

                    List<CodeInstruction> targetSequence = new List<CodeInstruction>() {
                    new CodeInstruction(OpCodes.Ldc_R4, 20000f),
                    new CodeInstruction(OpCodes.Stloc_S),
                };
                    List<CodeInstruction> patchSequence = new List<CodeInstruction>()
                {
                    new CodeInstruction(OpCodes.Ldarg_0),
                    CodeInstruction.LoadField(typeof(PLTurret), "TurretRange"),
                    new CodeInstruction(OpCodes.Ldc_R4, 5f),
                    new CodeInstruction(OpCodes.Div),
                    new CodeInstruction(OpCodes.Stloc_S, 7)
                };
                    patchSequence[0].labels.Add(list[175].labels[0]);

                    return HarmonyHelpers.PatchBySequence(list.AsEnumerable<CodeInstruction>(), targetSequence, patchSequence, HarmonyHelpers.PatchMode.REPLACE, HarmonyHelpers.CheckMode.NONNULL, false);
                }
            }

            [HarmonyPatch(typeof(PLMegaTurret), "ChargeComplete")]
            internal class MegaTurretRange
            {
                private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
                {
                    List<CodeInstruction> list = instructions.ToList();

                    List<CodeInstruction> targetSequence = new List<CodeInstruction>() {
                    new CodeInstruction(OpCodes.Ldc_R4, 10600f)
                };
                    List<CodeInstruction> patchSequence = new List<CodeInstruction>()
                {
                    new CodeInstruction(OpCodes.Ldarg_0),
                    CodeInstruction.LoadField(typeof(PLTurret), "TurretRange"),
                    new CodeInstruction(OpCodes.Ldc_R4, 5f),
                    new CodeInstruction(OpCodes.Div),
                };
                    return HarmonyHelpers.PatchBySequence(list.AsEnumerable<CodeInstruction>(), targetSequence, patchSequence, HarmonyHelpers.PatchMode.REPLACE, HarmonyHelpers.CheckMode.NONNULL, false);
                }
            }

            [HarmonyPatch(typeof(PLPhaseTurret), "Fire")]
            internal class PhaseTurretRange
            {
                private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
                {
                    List<CodeInstruction> list = instructions.ToList();
                    List<CodeInstruction> patchSequence = new List<CodeInstruction>()
                {
                    new CodeInstruction(OpCodes.Ldarg_0),
                    CodeInstruction.LoadField(typeof(PLTurret), "TurretRange"),
                    new CodeInstruction(OpCodes.Ldc_R4, 5f),
                    new CodeInstruction(OpCodes.Div),
                };
                    list.RemoveAt(131);
                    list.InsertRange(131, patchSequence.AsEnumerable<CodeInstruction>());
                    list.RemoveAt(139);
                    list.InsertRange(139, patchSequence.AsEnumerable<CodeInstruction>());

                    return list.AsEnumerable<CodeInstruction>();
                }
            }
        }

        public static void HeldLaserTurretDamage(
            int shipID,
            int inAttackingShipID,
            float damage,
            float randomNum,
            Vector3 localPosition,
            int inTurretID,
            int inProjID,
            int inBeamTickCount,
            int turretID)
        {
            PLShipInfoBase shipFromId1 = PLEncounterManager.Instance.GetShipFromID(shipID);
            PLShipInfoBase shipFromId2 = PLEncounterManager.Instance.GetShipFromID(inAttackingShipID);
            PLSpaceTarget spaceTargetFromId = PLEncounterManager.Instance.GetSpaceTargetFromID(shipID);
            if (shipFromId1 != null && shipFromId2 != null)
            {
                if (!(shipFromId2.GetTurretAtID(inTurretID) is AuxTurrets.HeldLaserTurret HeldTurretAtId) || HeldTurretAtId.GetCounterForProjID(inProjID).HasProcessedBeamTickCounter(inBeamTickCount))
                    return;
                Vector3 inWorldLoc = shipFromId1.Exterior.transform.TransformPoint(localPosition);
                bool bottomHit = false;
                if ((double)Vector3.Dot((inWorldLoc - shipFromId1.Exterior.transform.position).normalized, -shipFromId1.Exterior.transform.up) > -0.10000000149011612)
                    bottomHit = true;
                PLServer.Instance.LaserTurretExplosion(inWorldLoc, shipFromId1.ShipID, HeldTurretAtId.laserTurretExplosionID);
                double damage1 = (double)shipFromId1.TakeDamage(damage, bottomHit, HeldTurretAtId.LaserDamageType, randomNum, -1, shipFromId2, turretID);
            }
            else
            {
                if (!(spaceTargetFromId != null) || !(shipFromId2 != null) || !(shipFromId2.GetTurretAtID(inTurretID) is AuxTurrets.HeldLaserTurret HeldTurretAtId) || HeldTurretAtId.GetCounterForProjID(inProjID).HasProcessedBeamTickCounter(inBeamTickCount))
                    return;
                spaceTargetFromId.TakeDamage(damage);
            }
        }

        private class RecieveHeldLaserDamage : ModMessage
        {
            public override void HandleRPC(object[] arguments, PhotonMessageInfo sender)
            {
                Turrets.HeldLaserTurretDamage((int)arguments[0], (int)arguments[1], (float)arguments[2], (float)arguments[3], (Vector3)arguments[4], (int)arguments[5], (int)arguments[6], (int)arguments[7], (int)arguments[8]);
            }
        }

        [HarmonyPatch(typeof(PLTurret), "Tick")]
        internal class FixTargetting
        {
            private static void Postfix(PLTurret __instance)
            {
                if (!__instance.IsEquipped || !(__instance.TurretInstance != null))
                    return;
                Vector3 eulerAngles = __instance.targetWorldRot.eulerAngles;
                if (float.IsNaN(eulerAngles.x) || float.IsNaN(eulerAngles.y) || float.IsNaN(eulerAngles.z))
                    __instance.targetWorldRot.eulerAngles = new Vector3(0.0f, 0.0f, 0.0f);
            }
        }

        [HarmonyPatch(typeof(PLProjectile), "Update")]
        internal class StrictProjRange
        {
            private static void Postfix(PLProjectile __instance)
            {
                if (!(__instance != null))
                    return;
                if (!__instance.MissileFlag && __instance.TurretID != -1 && __instance.OwnerShipID != -1)
                {
                    PLShipInfoBase shipFromID = PLEncounterManager.Instance.GetShipFromID(__instance.OwnerShipID);
                    if (shipFromID != null)
                    {
                        PLTurret turret = shipFromID.GetTurretAtID(__instance.TurretID);
                        if (turret == null)
                            turret = shipFromID.GetAutoTurretAtID(__instance.TurretID);
                        if (turret != null && turret.TurretInstance != null)
                        {
                            Traverse travere = Traverse.Create(turret);
                            if ((__instance.transform.position - turret.TurretInstance.RefJoint.position).magnitude > travere.Field("TurretRange").GetValue<float>() / 5f)
                            {
                                if (__instance.ExplodeOnMaxLifetime)
                                {
                                    __instance.EmitExplostionParticleSystem(__instance._transform.position);
                                    ServerExplosiveProjExplode(__instance, null);
                                }
                                UnityEngine.Object.Destroy(__instance.gameObject);
                            }
                            else
                                __instance.MaxLifetime = travere.Field("TurretRange").GetValue<float>() / 1000f * 200f / __instance.Speed + 3f;
                        }
                    }
                }
            }
        }

        [HarmonyPatch(typeof(PLTurret), "Fire")]
        internal class ProjDamageTypePatch
        {
            private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
            {
                List<CodeInstruction> list = instructions.ToList();

                List<CodeInstruction> targetSequence = new List<CodeInstruction>() {
                    new CodeInstruction(OpCodes.Ldc_I4_2),
                    CodeInstruction.StoreField(typeof(PLProjectile), "MyDamageType")
                };
                List<CodeInstruction> patchSequence = new List<CodeInstruction>()
                {
                    new CodeInstruction(OpCodes.Ldarg_0),
                    CodeInstruction.Call(typeof(ExpandedGalaxy.Turrets), "GetTurretDamageType", new Type[1] {typeof(PLTurret)}),
                    CodeInstruction.StoreField(typeof(PLProjectile), "MyDamageType"),
                };
                return HarmonyHelpers.PatchBySequence(list.AsEnumerable<CodeInstruction>(), targetSequence, patchSequence, HarmonyHelpers.PatchMode.REPLACE, HarmonyHelpers.CheckMode.NONNULL, false);
            }
        }

        public static int GetTurretDamageType(PLTurret turret)
        {
            if (turret.SlotType == ESlotType.E_COMP_TURRET)
            {
                switch (turret.SubType)
                {
                    case (int)ETurretType.PLASMA:
                    case (int)ETurretType.BURST:
                        return 16;
                }               
            }
            else if (turret.SlotType == ESlotType.E_COMP_MAINTURRET)
            {
                if (turret.SubType == MegaTurretModManager.Instance.GetMegaTurretIDFromName("Physical Main Turret"))
                    if (turret.SubTypeData == 1)
                        return 16;
            }
            return 2;
        }

        public static bool IsProjectileAOE(PLProjectile projectile) { return projectile.MyDamageType == (EDamageType)16; }


        public static void ServerExplosiveProjExplode(PLProjectile projectile, PLSpaceTarget hitTarget = null)
        {
            if (!PhotonNetwork.isMasterClient)
                return;
            if (projectile != null && projectile.MyDamageType == (EDamageType)16)
            {
                foreach (PLSpaceTarget target in UnityEngine.Object.FindObjectsOfType<PLSpaceTarget>())
                {
                    if (target.SpaceTargetID != (hitTarget != null ? hitTarget.SpaceTargetID : -1))
                    {
                        bool flag = false;
                        PLShipInfoBase inShip = target as PLShipInfoBase;
                        if (inShip != null && inShip.ShipID != projectile.OwnerShipID && (inShip.Exterior.transform.position - projectile.transform.position).magnitude < 100f)
                        {
                            if ((double)Vector3.Dot((projectile.transform.position - inShip.Exterior.transform.position).normalized, -inShip.Exterior.transform.up) > -0.10000000149011612)
                                flag = true;
                            PLServer.Instance.photonView.RPC("ClientShipTakeDamage", PhotonTargets.All, (object)inShip.ShipID, (object)projectile.Damage, (object)flag, (object)(int)projectile.MyDamageType, (object)1f, (object)projectile.TargetShipSystemID, (object)projectile.OwnerShipID, (object)projectile.TurretID);
                        }
                        else if (target != null && (target.transform.position - projectile.transform.position).magnitude < 100f)
                            PLServer.Instance.photonView.RPC("ClientSpaceTargetTakeDamage", PhotonTargets.All, (object)target.SpaceTargetID, (object)projectile.Damage);
                    }
                }
            }
        }

        [HarmonyPatch(typeof(PLProjectile), "Explode")]
        internal class ExplosionAOE
        {
            private static bool Prefix(PLProjectile __instance, PLSpaceTarget target, Vector3 pos, Vector3 norm)
            {
                if (__instance.MyDamageType == (EDamageType)16)
                {
                    ServerExplosiveProjExplode(__instance, target);
                }
                return true;
            }
        }

        [HarmonyPatch(typeof(PLShipComponent), "CreateRandom")]
        internal class RandomMegaTurretInShop
        {
            private static bool Prefix(ref PLShipComponent __result)
            {
                int num = UnityEngine.Random.Range(0, 5000000) % 1000;
                if (num < 20)
                {
                    switch (num % 5)
                    {
                        case 0:
                            __result = new PLMegaTurret_RapidFire();
                            break;
                        case 1:
                            __result = new MainTurrets.RipperTurrret();
                            break;
                        default:
                            __result = new PLMegaTurret();
                            break;
                    }
                    __result.Level += Mathf.RoundToInt((UnityEngine.Object)PLServer.Instance != (UnityEngine.Object)null ? (float)PLServer.Instance.ChaosLevel * 0.5f : 0.0f);
                    return false;
                }
                return true;
            }
        }
    }
}
