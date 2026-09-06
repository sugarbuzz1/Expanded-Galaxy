using System.Collections.Generic;
using UnityEngine;

namespace ExpandedGalaxy
{
    public class BossFlag : MissionComponentFlag
    {
        private static int totalBossShips;
        private static float maxHullHp;
        private static List<BossFlag> allBossFlags = new List<BossFlag>();

        public BossFlag(int inType = 7, int inLevel = 0) : base(inType, inLevel)
        {
            this.Name = "ExGal_BossFlag";
            this.CanBeDroppedOnShipDeath = false;
            this.SubTypeData = 0;
        }

        public override void Equip()
        {
            base.Equip();
            allBossFlags.Add(this);
            totalBossShips += 1;
        }

        public override void Unequip()
        {
            base.Unequip();
            allBossFlags.Remove(this);
            if (AllBossFlags.Count < 1)
            {
                maxHullHp = 0;
                totalBossShips = 0;
            }
        }

        public override void Tick()
        {
            base.Tick();
            if (this.SubTypeData < 1 && this.ShipStats != null)
            {
                maxHullHp += this.ShipStats.HullMax;
                ++this.SubTypeData;
            }
        }

        public virtual string GetBossName()
        {
            return this.ShipStats.Ship.GetNameForUI();
        }

        public static float GetMaxHP()
        {
            return maxHullHp;
        }

        public static float GetCurrentHP()
        {
            float hp = 0;
            foreach (BossFlag flag in allBossFlags)
            {
                if (flag.ShipStats != null)
                    hp += flag.ShipStats.HullCurrent;
            }
            return hp;
        }

        public static float GetCurrentHPAlpha()
        {
            return GetCurrentHP() / GetMaxHP();
        }

        public static float GetLerpedHPAlpha()
        {
            float totalAlpha = 0f;
            foreach (BossFlag flag in allBossFlags)
            {
                if (flag.ShipStats != null && flag.ShipStats.Ship != null)
                {
                    totalAlpha += flag.ShipStats.Ship.GetSlowHPAlphaCurrent();
                }
            }
            return (float)totalAlpha / totalBossShips;
        }

        public static float LastTookDamageTime()
        {
            float lastDamageTime = float.MinValue;
            {
                foreach (BossFlag flag in allBossFlags)
                {
                    if (flag.ShipStats != null && flag.ShipStats.Ship != null)
                    {
                        lastDamageTime = Mathf.Max(lastDamageTime, flag.ShipStats.Ship.LastDamageRecievedTime);
                    }
                }
            }
            return lastDamageTime;
        }

        public static List<BossFlag> AllBossFlags
        {
            get
            {
                return allBossFlags;
            }
        }
    }

}

