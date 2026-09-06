using CodeStage.AntiCheat.ObscuredTypes;
using PulsarModLoader.Content.Components.HullPlating;

namespace ExpandedGalaxy
{
    internal class HeavyDutyPlating : PLHullPlating
    {
        public HeavyDutyPlating(EHullPlatingType inType, int inLevel) : base(inType, inLevel)
        {
            this.SubType = HullPlatingModManager.Instance.GetHullPlatingIDFromName("HeavyDutyPlating");
            this.Level = inLevel;
            this.Name = "Heavy Duty Plating";
            this.Desc = "This heavyset plating is designed to further reinforce the hull, at the expense of ship maneuverability";
            this.m_MarketPrice = (ObscuredInt)6500;

        }

        public override void AddStats(PLShipStats inStats)
        {
            base.AddStats(inStats);
            inStats.HullArmor += (75f + (15f * this.Level)) / 250f;
            inStats.Mass += (70f + (8f * this.Level));
        }

        public override string GetStatLineLeft() => PLLocalize.Localize("Armor") + "\n" + PLLocalize.Localize("Bottom Hit Dmg") + "\n" + PLLocalize.Localize("Mass") + "\n";

        public override string GetStatLineRight() => (75f + (15f * this.Level)).ToString("0") + "\n-" + (32f + (2f * this.Level)).ToString("0") + "%\n" + (120f + (8f * this.Level)).ToString("0") + "\n";
    }
}
