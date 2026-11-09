using CodeStage.AntiCheat.ObscuredTypes;
using PulsarModLoader.Content.Components.MegaTurret;
using UnityEngine;

namespace ExpandedGalaxy
{
    internal class AcidTurret : PLMegaTurretCU
    {
        private bool ColorCorrected;
        public AcidTurret(int inLevel = 0, int inSubTypeData = 1) : base(inLevel, inSubTypeData)
        {
            this.Name = "Caustic Collider";
            this.Desc = "An impressive beam weapon that was designed for use in the Colonial Union Fleet. This one has had illegal acid-based modifications installed. It has a max range of 12 km.";
            this.m_Damage = 550f;
            this.SubType = MegaTurretModManager.Instance.GetMegaTurretIDFromName(this.Name);
            this.TurretRange = 12000f;
            this.BeamColor = new Color(0.1f, 1.5f, 0.1f);
            this.Contraband = true;
            this.TrackerMissileReloadTime = 9f;
            this.CanBeDroppedOnShipDeath = false;
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
            if (this.IsEquipped && !this.ColorCorrected)
                this.CorrectColors();
        }
    }
}
