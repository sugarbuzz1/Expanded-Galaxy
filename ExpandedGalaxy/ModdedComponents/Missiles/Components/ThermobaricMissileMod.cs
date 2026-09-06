using PulsarModLoader.Content.Components.Missile;

namespace ExpandedGalaxy
{
    public class ThermobaricMissileMod : MissileMod
    {
        public override string Name => "Thermobaric Missile";

        public override string Description => "Tracker missile that deals fire damage.";

        public override int MarketPrice => 2000;

        public override bool Contraband => true;

        public override float Damage => 405f;

        public override float Speed => 4f;

        public override EDamageType DamageType => EDamageType.E_FIRE;

        public override int MissileRefillPrice => 200;

        public override int AmmoCapacity => 16;

        public override int PrefabID => 1;

        public override bool CanBeDroppedOnShipDeath => false;

        public override int CargoVisualID => 29;
    }
}
