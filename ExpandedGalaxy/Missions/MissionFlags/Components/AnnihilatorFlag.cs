using UnityEngine;

namespace ExpandedGalaxy
{
    internal class AnnihilatorFlag : MissionComponentFlag
    {
        public AnnihilatorFlag(int inType = 10, int inLevel = 0) : base(10, inLevel)
        {
            this.Name = "ExGal_AnnihilatorFlag";
            this.CanBeDroppedOnShipDeath = false;
        }

        public override void AddStats(PLShipStats inStats)
        {
            if (this.ShipStats == null)
                return;
            PLReactor reactor = this.ShipStats.Ship.MyReactor;
            if (reactor != null && reactor.SubType == (int)EReactorType.E_REAC_STRONGPOINT)
            {
                this.ShipStats.TurretDamageFactor *= 1.1f;
                this.ShipStats.ReactorOutputFactor *= 1.1f;
            }
        }

        public override void Tick()
        {
            base.Tick();
            if (!PhotonNetwork.isMasterClient)
                return;
            float num = PLReactor.GetPowerLevelOfType(this.ShipStats, 0);
            float num1 = PLReactor.GetPowerLevelOfType(this.ShipStats, 1);
            float num2 = PLReactor.GetPowerLevelOfType(this.ShipStats, 3);
            float num3 = (this.ShipStats.Ship.IsAuxSystemActive(1) ? 0.3f : 0f) + (this.ShipStats.Ship.IsAuxSystemActive(2) ? 0.2f : 0f);

            this.ShipStats.Ship.EngineeringSystem?.TakeDamage(Time.deltaTime * num);
            this.ShipStats.Ship.ComputerSystem?.TakeDamage(Time.deltaTime * num1);
            this.ShipStats.Ship.WeaponsSystem?.TakeDamage(Time.deltaTime * num2);
            this.ShipStats.Ship.LifeSupportSystem?.TakeDamage(Time.deltaTime * num3);
        }
    }
}
