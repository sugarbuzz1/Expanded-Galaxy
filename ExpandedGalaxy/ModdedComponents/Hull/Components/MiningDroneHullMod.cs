using PulsarModLoader.Content.Components.Hull;

namespace ExpandedGalaxy
{
    internal class MiningDroneHullMod : HullMod
    {
        public override string Name => "Extraction Drone Hull";

        public override string Description => "Hull built for the ancient mining drones. Lightweight yet strong.";

        public override int MarketPrice => 21000;

        public override float HullMax => 867.5f;

        public override float Armor => 0.34f;

        public override int CargoVisualID => 4;

        public override bool CanBeDroppedOnShipDeath => false;

        public override string GetStatLineLeft(PLShipComponent InComp) => PLLocalize.Localize("Integrity") + "\n" + PLLocalize.Localize("Armor") + "\n" + PLLocalize.Localize("Mass");

        public override string GetStatLineRight(PLShipComponent InComp)
        {
            PLHull plHull = InComp as PLHull;
            return (plHull.Max * InComp.LevelMultiplier(0.2f)).ToString("0") + "\n" + (plHull.Armor * 250f * InComp.LevelMultiplier(0.15f)).ToString("0") + "\n" + (90f + (7f * InComp.Level)).ToString("0") + "\n";
        }

        public override void AddStats(PLShipComponent InComp)
        {
            InComp.ShipStats.Mass += 90f + (7f * InComp.Level) - Hull.GetMassDiscountForShip(InComp.ShipStats.Ship.ShipTypeID);
        }
    }
}
