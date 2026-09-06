using CodeStage.AntiCheat.ObscuredTypes;

namespace ExpandedGalaxy
{
    public class ReflectedRiftShipFlag : MissionComponentFlag
    {
        public ReflectedRiftShipFlag(int inLevel = 0) : base(4, inLevel)
        {
            this.Name = "ExGal_ReflectedRiftShip_Flag";
            this.CanBeDroppedOnShipDeath = false;
            this.SubTypeData = 0;
        }

        public override void Tick()
        {
            base.Tick();
            if (!this.ShipStats.Ship.IsAbandoned())
                this.ShipStats.Ship.SetAbandoned(true);
            if (PhotonNetwork.isMasterClient && this.SubTypeData == 0)
            {
                this.ShipStats.Ship.FactionID = 6;             
                this.ShipStats.Ship.AuxConfig = 0;
                this.ShipStats.Ship.EngineeringSystem.Health = (ObscuredFloat)0f;
                this.ShipStats.Ship.WeaponsSystem.Health = (ObscuredFloat)0f;
                this.ShipStats.Ship.ComputerSystem.Health = (ObscuredFloat)0f;
                this.ShipStats.Ship.LifeSupportSystem.Health = (ObscuredFloat)0f;
                this.ShipStats.HullCurrent = this.ShipStats.HullMax * 0.13f;
                ((PLShipInfo)this.ShipStats.Ship).CrewControlEnabled = false;
                this.ShipStats.Ship.MyStats.OxygenLevel = 0f;
                this.ShipStats.Ship.MyStats.RemoveShipComponentByNetID(this.ShipStats.Ship.MyReactor.NetID);
                foreach (PLShipComponent component in this.ShipStats.AllComponents)
                {
                    if (component != null && !(component is PLWarpDrive))
                        component.CanBeDroppedOnShipDeath = false;
                }
                ((PLShipInfo)this.ShipStats.Ship).StartupSwitchBoard.SetStatus(0, false);
                ++this.SubTypeData;
            }
        }
    }

}

