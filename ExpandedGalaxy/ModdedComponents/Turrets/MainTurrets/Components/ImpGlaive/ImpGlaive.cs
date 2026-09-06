using CodeStage.AntiCheat.ObscuredTypes;
using HarmonyLib;
using PulsarModLoader.Content.Components.MegaTurret;
using UnityEngine;

namespace ExpandedGalaxy
{
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
}
