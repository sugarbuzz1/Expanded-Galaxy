using PulsarModLoader.Content.Components.Missile;

namespace ExpandedGalaxy
{
    public class SwarmerMissile : MissileMod
    {
        public override string Name => "Seeker Missile";

        public override string Description => "Extremely fast missile designed to disable the shields of even the most agile ship.";

        public override int MarketPrice => 6800;

        public override bool Experimental => true;

        public override float Damage => 40f;

        public override float Speed => 20f;

        public override EDamageType DamageType => EDamageType.E_SHIELD_PIERCE_PHYS;

        public override int MissileRefillPrice => 600;

        public override int AmmoCapacity => 10;

        public override int PrefabID => 2;

        public override int CargoVisualID => 30;
    }
}
