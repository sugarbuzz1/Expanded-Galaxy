using CodeStage.AntiCheat.ObscuredTypes;
using HarmonyLib;
using PulsarModLoader.Content.Components.MegaTurret;
using UnityEngine;

namespace ExpandedGalaxy
{
    internal class WDStandard : PLMegaTurret
    {
        protected int ShotsMax = 3;
        private bool ColorCorrected;
        private double lastUpdateTime = double.MinValue;

        public WDStandard(int inLevel = 0, int inSubTypeData = 0)
        : base(inLevel)
        {
            this.Name = "WD Standard";
            this.Desc = "A main turret equipped with a battery array allowing it to fire multiple times before needing to recharge. It has a max range of 7 km.";
            this.m_Damage = 250f;
            this.SubType = MegaTurretModManager.Instance.GetMegaTurretIDFromName("WDStandard");
            this.m_MarketPrice = (ObscuredInt)20000;
            this.FireDelay = 20f;
            this.m_MaxPowerUsage_Watts = 14150f;
            this.CargoVisualPrefabID = 5;
            this.TurretRange = 7000f;
            this.BeamColor = new Color(1f, 0.3f, 0f);
            this.MegaTurretExplosionID = 0;
            this.Level = inLevel;
            this.m_KickbackForceMultiplier = 0.67f;
            this.m_AutoAimMinDotPrd = 0.98f;
            this.HeatGeneratedOnFire = 0.23f;
            this.AutoAimEnabled = true;
            this.IsMainTurret = true;
            this.HasTrackingMissileCapability = true;
            this.TrackerMissileReloadTime = 5f;
            this.HasPulseLaser = true;
            Traverse.Create((PLMegaTurret)this).Field("turretChargeSpeed_ToVisualChargeSpeed").SetValue(0.2625f);
            this.ColorCorrected = false;
            this.SubTypeData = 3;
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

        public override void Fire(int inProjID, Vector3 dir)
        {
            bool flag = this.IsCharging;
            base.Fire(inProjID, dir);
            if (flag)
                return;
            if ((int)this.SubTypeData < this.ShotsMax - 1)
                this.ChargeAmount = 1f;
        }
        public override void ChargeComplete(int inProjID, Vector3 dir)
        {
            base.ChargeComplete(inProjID, dir);
            ++this.SubTypeData;
        }

        public override string GetInfoString() => "    " + (this.ShotsMax - this.SubTypeData).ToString() + "/" + this.ShotsMax.ToString();

        public override void Tick()
        {
            if (this.SubTypeData >= this.ShotsMax && (double)this.ChargeAmount > 0.99f)
                this.SubTypeData = 0;
            base.Tick();
            this.m_MaxPowerUsage_Watts = 14150f * this.LevelMultiplier(0.2f);
            if (this.IsEquipped && !this.ColorCorrected)
                this.CorrectColors();
            /*if (this.IsEquipped && PhotonNetwork.isMasterClient && (double)Time.time - this.lastUpdateTime > 4.0)
            {
                ModMessage.SendRPC("sugarbuzz1.ExpandedGalaxy", "ExpandedGalaxy.UpdateSubTypeData", PhotonTargets.Others, new object[3]
            {
                (object) this.ShipStats.Ship.ShipID,
                (object) this.NetID,
                (object) this.SubTypeData,
            });
                this.lastUpdateTime = (double)Time.time;
            }*/
        }

        public override void UpdatePowerUsage(PLPlayer currentOperator)
        {
            base.UpdatePowerUsage(currentOperator);
            if ((double)this.ChargeAmount >= 1.0 && (double)this.Heat > 0.0)
            {
                this.m_RequestPowerUsage_Percent = this.ShipStats.Ship.WeaponsSystem.GetHealthRatio() * 0.33f;
                this.IsPowerActive = true;
            }

        }

        public override bool ShouldAIFire(bool operatedByBot, float heatOffset, float heatGeneratedOnFire)
        {
            return operatedByBot && this.Heat + heatGeneratedOnFire < 0.90f && this.SubTypeData < 3;
        }
    }
}
