using PulsarModLoader.Content.Components.Missile;

namespace ExpandedGalaxy
{
    public class RelicMissile : MissileMod
    {
        public override string Name => "GKS-Ex27 \"Soul Siphon\"";

        public override string Description => "Tracker missile that utilizes alien technology to dematerialize a target's hull to repair its wielder.";

        public override int MarketPrice => 20000;

        public override float Damage => 295f;

        public override float Speed => 8f;

        public override EDamageType DamageType => (EDamageType)17;

        public override int MissileRefillPrice => 900;

        public override int AmmoCapacity => 7;

        public override int PrefabID => 7;

        public override int CargoVisualID => 43;
    }
}
