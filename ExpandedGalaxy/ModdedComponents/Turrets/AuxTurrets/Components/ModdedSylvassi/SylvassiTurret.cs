using CodeStage.AntiCheat.ObscuredTypes;
using PulsarModLoader.Content.Components.Turret;
using UnityEngine;

namespace ExpandedGalaxy
{
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
}
