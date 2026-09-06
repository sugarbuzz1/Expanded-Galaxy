using CodeStage.AntiCheat.ObscuredTypes;
using PulsarModLoader.Content.Components.MegaTurret;
using UnityEngine;

namespace ExpandedGalaxy
{
    internal class Disassembler : HeldLaserTurret
    {
        protected Color BeamColor = Color.red;
        public Disassembler(int inLevel = 0, int inSubTypeData = 1)
            : base(inLevel, inSubTypeData)
        {
            this.Name = "The Disassembler";
            this.Desc = "Powerful beam weapon that uses its target's hull debris to repair its own ship.";
            this.m_Damage = 100f;
            this.SubType = MegaTurretModManager.Instance.GetMegaTurretIDFromName("The Disassembler");
            this.m_MarketPrice = (ObscuredInt)9200;
            this.CargoVisualPrefabID = 5;
            this.FireDelay = 1f;
            this.m_SlotType = ESlotType.E_COMP_MAINTURRET;
            this.HeatGeneratedOnFire = 0.2f;
            this.DamageChecksPerSecond = 2f;
            this.m_IconTexture = (Texture2D)Resources.Load("Icons/8_Weapons");
            this.HasTrackingMissileCapability = true;
            this.TrackerMissileReloadTime = 9f;
            this.TurretRange = 15000f;
            this.m_AutoAimMinDotPrd = 0.99f;
            this.m_Laser_xzScale = 300f;
            this.m_Laser_yScale = 100f;
            this.AutoAimEnabled = true;
            this.IsMainTurret = true;
            this.BeamColor = new Color(1f, 1f, 0.05f);
            this.HitComboCountMax = 1;
            this.HitComboMultiplier = 0.8f;
        }

        public override void UpdateMaxPowerUsageWatts() => this.CalculatedMaxPowerUsage_Watts = 11800f;

        protected override void CorrectColors()
        {
            if (!((UnityEngine.Object)this.TurretInstance != (UnityEngine.Object)null) || !((UnityEngine.Object)this.TurretInstance.BeamObjectRenderer != (UnityEngine.Object)null))
                return;
            if ((UnityEngine.Object)this.TurretInstance.BeamObjectRenderer == (UnityEngine.Object)null)
                return;
            this.TurretInstance.BeamObjectRenderer.material.SetColor("_LaserColor", this.BeamColor);
            if (this.TurretInstance.OptionalGameObjects[0] == null)
                return;
            this.TurretInstance.OptionalGameObjects[0].transform.Find("Charge (1)").GetComponent<ParticleSystem>().startColor = this.BeamColor;
            this.TurretInstance.OptionalGameObjects[0].transform.Find("Point light (1)").GetComponent<Light>().color = this.BeamColor;
            base.CorrectColors();
        }

        public override bool AutoAimTrackingIsEnabled()
        {
            PLPlayer playerFromPlayerId = PLServer.Instance.GetPlayerFromPlayerID(this.ShipStats.Ship.GetCurrentTurretControllerPlayerID(this.TurretID));
            bool flag = false;
            if (playerFromPlayerId != null && playerFromPlayerId.IsBot)
                flag = true;
            return flag || this.ShipStats.Ship.IsDrone;
        }

        public override string GetTurretPrefabPath() => "NetworkPrefabs/Component_Prefabs/MegaTurret3";
    }
}
