using CodeStage.AntiCheat.ObscuredTypes;
using PulsarModLoader.Content.Components.Turret;
using UnityEngine;

namespace ExpandedGalaxy
{
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

        private void UpdateBaseDamage()
        {
            PLTrackerMissile missile = this.ShipStats.Ship.SelectedMissileLauncher;
            if (missile != null)
            {
                this.m_Damage = missile.Damage;
                return;
            }
            this.m_Damage = 0f;
        }

        public override string GetDamageTypeString()
        {
            PLTrackerMissile missile = this.ShipStats.Ship.SelectedMissileLauncher;
            if (missile != null)
            {
                switch (missile.DamageType)
                {
                    case EDamageType.E_ENERGY:
                        return "ENERGY";
                    case EDamageType.E_PHYSICAL:
                    case EDamageType.E_COLLISION:
                    case EDamageType.E_ARMOR_PIERCE_PHYS:
                        return "PHYSICAL";
                    case EDamageType.E_SHIELD_PIERCE_PHYS:
                    case EDamageType.E_REACTOR_TARGETED_PHYS:
                        return "PHYSICAL (TGT)";
                    case EDamageType.E_INFECTED:
                        return "INFECTED";
                    case EDamageType.E_LIGHTNING:
                        return "ENERGY (LTNG)";
                    case EDamageType.E_PHASE:
                    case EDamageType.E_ALL_SYSTEM_DMG:
                        return "ENERGY (PHASE)";
                    case EDamageType.E_SYSTEM_DAMAGE:
                        return "PHYSICAL (PHASE)";
                    case EDamageType.E_FIRE:
                        return "PHYSICAL (FIRE)";
                    case EDamageType.E_BIOHAZARD:
                        return "PHYSICAL (ACID)";
                }
            }
            return "NONE";
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

        public override string GetStatLineRight()
        {
            float num1 = this.m_Damage;
            float num2 = this.FireDelay / (this.ShipStats != null ? this.ShipStats.TurretChargeFactor : 1f);
            return num1.ToString("0") + "\n" + num2.ToString("0.0") + "\n" + this.GetDamageTypeString() + "\n";
        }

        public override void OnTurretInstanceCreated()
        {
            base.OnTurretInstanceCreated();
            this.TurretInstance.OptionalSecondaryFiringLoc = this.TurretInstance.FiringLoc;
        }

        public override bool ShouldAIFire(bool operatedByBot, float heatOffset, float heatGeneratedOnFire)
        {
            return currentTargetShip != null && base.ShouldAIFire(operatedByBot, heatOffset, heatGeneratedOnFire);
        }
        public override string GetTurretPrefabPath() => "NetworkPrefabs/Component_Prefabs/Turret";
    }
}
