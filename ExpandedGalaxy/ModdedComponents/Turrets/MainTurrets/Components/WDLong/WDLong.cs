using CodeStage.AntiCheat.ObscuredTypes;
using PulsarModLoader.Content.Components.MegaTurret;
using UnityEngine;

namespace ExpandedGalaxy
{
    internal class WDLong : PLMegaTurretCU
    {
        private bool ColorCorrected;
        public WDLong(int inLevel = 0, int inSubTypeData = 1) : base(inLevel, inSubTypeData)
        {
            this.Name = "WD Long Range";
            this.Desc = "The Corporation's own long-range was made to outclass its C.U. counterpart. This model was stolen in development and has not yet been fitted with missile capabilities.";
            this.m_Damage = 900f;
            this.SubType = MegaTurretModManager.Instance.GetMegaTurretIDFromName(this.Name);
            this.m_MarketPrice = (ObscuredInt)20000;
            this.FireDelay = 20f;
            this.m_MaxPowerUsage_Watts = 11600f;
            this.TurretRange = 16000f;
            this.BeamColor = new Color(1.5f, 0.45f, 0f);
            this.m_KickbackForceMultiplier = 1.2f;
            this.HasTrackingMissileCapability = false;
            this.HasPulseLaser = false;
            this.CanHitMissiles = false;
            this.Experimental = true;
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
            this.m_MaxPowerUsage_Watts = 11600f * this.LevelMultiplier(0.2f);
            if (this.IsEquipped && !this.ColorCorrected)
                this.CorrectColors();
        }
    }
}
