using CodeStage.AntiCheat.ObscuredTypes;
using PulsarModLoader.Content.Components.Turret;
using UnityEngine;

namespace ExpandedGalaxy
{
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

        public override string GetTurretPrefabPath() => "NetworkPrefabs/Component_Prefabs/Turret";
    
    }
}
