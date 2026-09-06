using CodeStage.AntiCheat.ObscuredTypes;
using PulsarModLoader.Content.Components.HullPlating;
using UnityEngine;

namespace ExpandedGalaxy
{
    internal class BonePlating : PLHullPlating
    {
        private float lastUpdateTime = float.MinValue;
        public BonePlating(EHullPlatingType inType, int inLevel) : base(inType, inLevel)
        {
            this.SubType = HullPlatingModManager.Instance.GetHullPlatingIDFromName("Bone Plating");
            this.Level = inLevel;
            this.Name = "Bone Plating";
            this.Desc = "Hull plating made from the ever-growing exoskeleton of an infected carrier. It can block anything, but will take some time to regrow...";
            this.m_MarketPrice = (ObscuredInt)7600;
            this.Contraband = true;
            this.SubTypeData = 0;
        }

        public override void Update()
        {
            base.Update();
            if (this.IsEquipped)
            {
                if (this.SubTypeData > 0 && Time.time - this.lastUpdateTime > 1f)
                {
                    this.lastUpdateTime = Time.time;
                    --this.SubTypeData;
                }
            }
            else
            {
                this.SubTypeData = (short)Mathf.Clamp(21 - this.Level, 8f, float.MaxValue);
            }
        }

        public override string GetStatLineLeft() => "Status:" + "\n" + "Regrow Time:" + "\n";

        public override string GetStatLineRight()
        {
            string line;
            if (!this.IsEquipped)
                line = "INACTIVE" + "\n";
            int num = this.SubTypeData;
            if (num > 0)
                line = (num).ToString("0") + "s\n";
            else
                line = "ACTIVE" + "\n";
            return line + Mathf.Clamp(20 - this.Level, 7f, float.MaxValue).ToString("0") + "s\n";
        }
    }
}
