namespace ExpandedGalaxy
{
    public class NoScalingFlag : MissionComponentFlag
    {
        public NoScalingFlag(int inLevel = 0) : base(5, inLevel)
        {
            this.Name = "ExGal_NoScaling_Flag";
            this.CanBeDroppedOnShipDeath = false;
            this.SubTypeData = 0;
        }

        public override void Tick()
        {
            base.Tick();
            if (this.ShipStats != null && this.ShipStats.Ship != null && this.ShipStats.Ship.GetIsPlayerShip())
                this.FlagForSelfDestruction();
        }
    }

}

