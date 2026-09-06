using PulsarModLoader.Content.Components.Hull;

namespace ExpandedGalaxy
{
    internal class GuardianDroneHullMod : HullMod
    {
        public override string Name => "Guardian Drone Hull";

        public override string Description => "Hull built for Guardian Protector Drones. Heavy, yet resiliant.";

        public override int MarketPrice => 25000;

        public override float HullMax => 1100f;

        public override float Armor => 1f;

        public override int CargoVisualID => 4;

        public override bool CanBeDroppedOnShipDeath => false;

        public override string GetStatLineLeft(PLShipComponent InComp) => PLLocalize.Localize("Integrity") + "\n" + PLLocalize.Localize("Armor") + "\n" + PLLocalize.Localize("Mass");

        public override string GetStatLineRight(PLShipComponent InComp)
        {
            PLHull plHull = InComp as PLHull;
            return (plHull.Max * InComp.LevelMultiplier(0.2f)).ToString("0") + "\n" + (plHull.Armor * 250f * InComp.LevelMultiplier(0.15f)).ToString("0") + "\n" + (228f + (11f * InComp.Level)).ToString("0") + "\n";
        }

        public override void AddStats(PLShipComponent InComp)
        {
            InComp.ShipStats.Mass += 228f + (11f * InComp.Level) - Hull.GetMassDiscountForShip(InComp.ShipStats.Ship.ShipTypeID);
        }
    }
}
