using CodeStage.AntiCheat.ObscuredTypes;
using PulsarModLoader.Content.Components.HullPlating;
using UnityEngine;

namespace ExpandedGalaxy
{
    internal class NanoActivePlating : PLHullPlating
    {
        internal float armor = 200f;
        public NanoActivePlating(EHullPlatingType inType, int inLevel) : base(inType, inLevel)
        {
            this.SubType = HullPlatingModManager.Instance.GetHullPlatingIDFromName("NanoActivePlating");
            this.Level = inLevel;
            this.Name = "Nano-Active Hull Plating";
            this.Desc = "Utilizing nanite technology this plating reinforces itself, getting stronger over time";
            this.m_MarketPrice = (ObscuredInt)8000;
            this.Experimental = true;
            this.UpdateArmorValue();
        }

        private void UpdateArmorValue()
        {
            armor = 200f + 40f * this.Level;
        }

        internal float MaxArmor()
        {
            return 200f + 40f * this.Level;
        }

        public override void AddStats(PLShipStats inStats)
        {
            base.AddStats(inStats);
            inStats.HullArmor += armor / 250f;
            inStats.Mass += 10f + (25f + 10f * this.Level) * (armor / this.MaxArmor());
        }

        public override void Tick()
        {
            base.Tick();
            if (!this.IsEquipped)
                return;
            armor += 30f * Time.deltaTime;
            armor = Mathf.Clamp(armor, 0f, this.MaxArmor());
        }

        public override string GetStatLineLeft() => PLLocalize.Localize("Armor") + "\n" + PLLocalize.Localize("Armor (Max)") + "\n" + PLLocalize.Localize("Mass") + "\n";

        public override string GetStatLineRight() => (this.armor).ToString("0") + "\n" + (this.MaxArmor()).ToString("0") + "\n" + (10f + (75f + 10f * this.Level) * (armor / this.MaxArmor())).ToString("0") + "\n";
    }
}
