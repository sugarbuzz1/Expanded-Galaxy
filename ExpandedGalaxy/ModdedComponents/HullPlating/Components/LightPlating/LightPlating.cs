using CodeStage.AntiCheat.ObscuredTypes;
using PulsarModLoader.Content.Components.HullPlating;

namespace ExpandedGalaxy
{
    internal class LightPlating : PLHullPlating
    {
        public LightPlating(EHullPlatingType inType, int inLevel) : base(inType, inLevel)
        {
            this.SubType = HullPlatingModManager.Instance.GetHullPlatingIDFromName("LightHullPlating");
            this.Level = inLevel;
            this.Name = "Light Hull Plating";
            this.Desc = "A lighter version of the standard hull plating that boosts dodging capabilities";
            this.m_MarketPrice = (ObscuredInt)9000;
        }

        public override void AddStats(PLShipStats inStats)
        {
            base.AddStats(inStats);
            inStats.HullArmor += (30f + (6f * this.Level)) / 250f;
            inStats.Mass += (-15f + (4f * this.Level));
        }

        public override string GetStatLineLeft() => PLLocalize.Localize("Armor") + "\n" + PLLocalize.Localize("Bottom Hit Dmg") + "\n" + PLLocalize.Localize("Mass") + "\n";

        public override string GetStatLineRight() => (30f + (6f * this.Level)).ToString("0") + "\n-" + (20f + (2f * this.Level)).ToString("0") + "\n" + (35f + (4f * this.Level)).ToString("0") + "\n";
    }
}
