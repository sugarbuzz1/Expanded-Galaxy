using CodeStage.AntiCheat.ObscuredTypes;
using PulsarModLoader.Content.Components.MegaTurret;
using UnityEngine;

namespace ExpandedGalaxy
{
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

        public override float GetDPS() => base.GetDPS() * ProjectilesPerShot;

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

        public override void InnerCheckFire()
        {
            PLServer.Instance.photonView.RPC("TurretFire", PhotonTargets.All, (object)this.ShipStats.Ship.ShipID, (object)this.TurretID, (object)PLServer.Instance.ServerProjIDCounter, (object)Vector3.zero);
            PLServer.Instance.ServerProjIDCounter += this.ProjectilesPerShot;
        }
    }
}
