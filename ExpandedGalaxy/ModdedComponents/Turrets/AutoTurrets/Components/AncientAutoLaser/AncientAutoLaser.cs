using CodeStage.AntiCheat.ObscuredTypes;
using PulsarModLoader.Content.Components.AutoTurret;
using UnityEngine;

namespace ExpandedGalaxy
{
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
}
