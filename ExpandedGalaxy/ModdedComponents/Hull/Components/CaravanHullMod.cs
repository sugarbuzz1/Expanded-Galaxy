using PulsarModLoader.Content.Components.Hull;

namespace ExpandedGalaxy
{
    internal class CaravanHullMod : HullMod
    {
        public override string Name => "Caravaneer Hull";

        public override string Description => "This hull is exceptionally strong";

        public override int MarketPrice => 0;

        public override float HullMax => 3750f;

        public override float Armor => 1.05f;

        public override int CargoVisualID => 4;

        public override bool CanBeDroppedOnShipDeath => false;

        public override string GetStatLineLeft(PLShipComponent InComp) => PLLocalize.Localize("Integrity") + "\n" + PLLocalize.Localize("Armor") + "\n" + PLLocalize.Localize("Mass");

        public override string GetStatLineRight(PLShipComponent InComp)
        {
            PLHull plHull = InComp as PLHull;
            return (plHull.Max * InComp.LevelMultiplier(0.2f)).ToString("0") + "\n" + (plHull.Armor * 250f * InComp.LevelMultiplier(0.15f)).ToString("0") + "\n" + (470f + (35f * InComp.Level)).ToString("0") + "\n";
        }

        public override void AddStats(PLShipComponent InComp)
        {
            InComp.ShipStats.Mass += 470f + (35f * InComp.Level) - Hull.GetMassDiscountForShip(InComp.ShipStats.Ship.ShipTypeID);
        }
    }
}
