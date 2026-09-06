using PulsarModLoader.Content.Components.Shield;

namespace ExpandedGalaxy
{
    public class MiningDroneShield : ShieldMod
    {
        public override string Name => "Layered Surface Projector";

        public override string Description => "A precursor to the Tetragonal Surface Projector. Reduces incoming damage of all damage types by 10%.";

        public override int MarketPrice => 20000;

        public override float ShieldMax => 750f;

        public override float ChargeRateMax => 18f;

        public override float RecoveryRate => 2f;

        public override float MinIntegrityPercentForQuantumShield => 0.4f;

        public override float MaxPowerUsage_Watts => 17500f;

        public override int MinIntegrityAfterDamage => 40;

        public override bool CanBeDroppedOnShipDeath => false;
    }
}
