using PulsarModLoader.Content.Components.Shield;

namespace ExpandedGalaxy
{
    public class ReflectorShield : ShieldMod
    {
        public override string Name => "Reflector Shield Generator";

        public override string Description => "A shield generator that reflects all incoming damage back to the dealer. Due to its potential in harming police vessels it has been declared contraband by the Colonial Union.";

        public override int MarketPrice => 18645;

        public override float ShieldMax => 650f;

        public override float ChargeRateMax => 9f;

        public override float RecoveryRate => 2f;

        public override float MinIntegrityPercentForQuantumShield => 0.4f;

        public override float MaxPowerUsage_Watts => 16000f;

        public override int MinIntegrityAfterDamage => 90;

        public override bool Contraband => true;
    }
}
