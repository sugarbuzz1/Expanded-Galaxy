using PulsarModLoader.Content.Components.Hull;

namespace ExpandedGalaxy
{
    internal class JuggernautHullMod : HullMod
    {
        public override string Name => "Juggernaut Hull";

        public override string Description => "This hull boasts formidable strength and boosted collision damage. However, it isn't compatible with shields.";

        public override int MarketPrice => 38000;

        public override float HullMax => 1500f;

        public override float Armor => 0.8f;

        public override int CargoVisualID => 4;

        public override string GetStatLineLeft(PLShipComponent InComp) => PLLocalize.Localize("Integrity") + "\n" + PLLocalize.Localize("Armor") + "\n" + PLLocalize.Localize("Mass");

        public override string GetStatLineRight(PLShipComponent InComp)
        {
            PLHull plHull = InComp as PLHull;
            return (plHull.Max * InComp.LevelMultiplier(0.2f)).ToString("0") + "\n" + (plHull.Armor * 250f * InComp.LevelMultiplier(0.15f)).ToString("0") + "\n" + (200f + (40f * InComp.Level)).ToString("0") + "\n";
        }

        public override void AddStats(PLShipComponent InComp)
        {
            InComp.ShipStats.Mass += 200f + (40f * InComp.Level) - Hull.GetMassDiscountForShip(InComp.ShipStats.Ship.ShipTypeID);
        }

        public override void FinalLateAddStats(PLShipComponent InComp)
        {
            InComp.ShipStats.ShieldsCurrent = 0f;
            InComp.ShipStats.ShieldsMax = 0f;
            InComp.ShipStats.ShieldsChargeRate = 0f;
        }
    }
}
