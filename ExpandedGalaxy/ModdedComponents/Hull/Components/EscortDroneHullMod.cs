using PulsarModLoader.Content.Components.Hull;

namespace ExpandedGalaxy
{
    internal class EscortDroneHullMod : HullMod
    {
        public override string Name => "Escort Drone Hull";

        public override string Description => "Hull built for the escort drones. Lightweight yet strong.";

        public override int MarketPrice => 23000;

        public override float HullMax => 1000f;

        public override float Armor => 0.8f;

        public override int CargoVisualID => 4;

        public override bool CanBeDroppedOnShipDeath => false;

        public override string GetStatLineLeft(PLShipComponent InComp) => PLLocalize.Localize("Integrity") + "\n" + PLLocalize.Localize("Armor") + "\n" + PLLocalize.Localize("Mass");

        public override string GetStatLineRight(PLShipComponent InComp)
        {
            PLHull plHull = InComp as PLHull;
            return (plHull.Max * InComp.LevelMultiplier(0.2f)).ToString("0") + "\n" + (plHull.Armor * 250f * InComp.LevelMultiplier(0.15f)).ToString("0") + "\n" + (108f + (9f * InComp.Level)).ToString("0") + "\n";
        }

        public override void AddStats(PLShipComponent InComp)
        {
            InComp.ShipStats.Mass += 108f + (9f * InComp.Level) - Hull.GetMassDiscountForShip(InComp.ShipStats.Ship.ShipTypeID);
        }
    }
}
