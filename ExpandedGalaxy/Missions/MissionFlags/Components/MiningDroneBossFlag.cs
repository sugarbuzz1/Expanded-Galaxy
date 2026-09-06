using System.Collections.Generic;

namespace ExpandedGalaxy
{
    public class MiningDroneBossFlag : BossFlag
    {
        public static List<MiningDroneBossFlag> AllMiningDroneBossFlags = new List<MiningDroneBossFlag>();
        public MiningDroneBossFlag(int inLevel = 0) : base(8, inLevel)
        {
            this.Name = "ExGal_BossFlag_MiningDrone";
            this.CanBeDroppedOnShipDeath = false;
            this.SubTypeData = 0;
        }
        public override string GetBossName()
        {
            return "Guardian Drones";
        }

        public override void Equip()
        {
            base.Equip();
            AllMiningDroneBossFlags.Add(this);
        }

        public override void Unequip()
        {
            base.Unequip();
            if (AllMiningDroneBossFlags.Contains(this))
                AllMiningDroneBossFlags.Remove(this);
        }
    }
}

