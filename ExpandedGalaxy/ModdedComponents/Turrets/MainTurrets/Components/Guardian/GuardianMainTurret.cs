using CodeStage.AntiCheat.ObscuredTypes;
using PulsarModLoader.Content.Components.MegaTurret;
using UnityEngine;

namespace ExpandedGalaxy
{
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

        public override string GetDamageTypeString() => "PHASE (BEAM)";
    }
}
