using CodeStage.AntiCheat.ObscuredTypes;

namespace ExpandedGalaxy
{
    public class MissionComponentFlag : PLShipComponent
    {
        protected bool shouldResort;
        public MissionComponentFlag(int inType, int inLevel = 0) : base(ESlotType.E_COMP_REAC_COOLING)
        {
            this.SubType = inType;
            this.Level = inLevel;
            this.m_MarketPrice = (ObscuredInt)(-1);
            this.Desc = "If you're reading this, I fucked up :(";
            this.shouldResort = false;
        }

        public override string GetShortDesc() => "FLAG";

        public static PLShipComponent CreateMissionFlagFromHash(int inSubType, int inLevel, int inSubTypeData)
        {
            switch (inSubType)
            {
                case 0:
                    return new MiningDroneFlag(inLevel);
                case 1:
                    return new MissionShipFlagComp();
                case 2:
                    return new MissionAddOneCPUSlotComp();
                case 3:
                    return new MissionNoExtractorFlag();
                case 4:
                    return new ReflectedRiftShipFlag();
                case 5:
                    return new NoScalingFlag();
                case 6:
                    return new CaravanFlag();
                case 7:
                    return new BossFlag();
                case 8:
                    return new MiningDroneBossFlag();
                case 9:
                    return new RolandSCFlag();
                default:
                    return null;
            }
        }

        public override void Tick()
        {
            base.Tick();

            if (this.IsEquipped && this.ShipStats != null && shouldResort)
            {
                int currentIndex = this.ShipStats.AllComponents.IndexOf(this);
                if (currentIndex <= 0)
                    return;
                int insertPos = 0;
                for (int i = 0; i < this.ShipStats.AllComponents.Count; i++)
                {
                    if (this.ShipStats.AllComponents[i] != null)
                    {
                        if (this.ShipStats.AllComponents[i] == this)
                            return;
                        if (this.ShipStats.AllComponents[i].ActualSlotType == ESlotType.E_COMP_REAC_COOLING)
                            continue;
                        else
                        {
                            insertPos = i;
                            break;
                        }

                    }
                }
                if (insertPos < currentIndex)
                {
                    this.ShipStats.AllComponents.RemoveAt(currentIndex);
                    this.ShipStats.AllComponents.Insert(insertPos, this);
                }
            }
        }
    }

}

