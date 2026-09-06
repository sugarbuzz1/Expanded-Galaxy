using CodeStage.AntiCheat.ObscuredTypes;
using PulsarModLoader.Content.Components.Turret;
using UnityEngine;

namespace ExpandedGalaxy
{
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
            this.isMiningLaser = true;
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
                this.HeatGeneratedOnFire = 0f;
        }
    }
}
