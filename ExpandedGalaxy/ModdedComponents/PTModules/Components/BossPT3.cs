using PulsarModLoader.Content.Components.PolytechModule;
using UnityEngine;

namespace ExpandedGalaxy
{
    public class BossPT3 : PolytechModuleMod
    {
        private float LastProgramTickTime;
        public override string Name => "P.T. Module: Recompiler 3";

        public override string Description => "If you're reading this, I fucked up :(";

        public override int MarketPrice => 9999999;

        public override float MaxPowerUsage_Watts => 1f;

        public override bool Experimental => false;

        public override bool Contraband => true;

        public override bool CanBeDroppedOnShipDeath => false;

        public override void Tick(PLShipComponent InComp)
        {
            //Program Recharge + Decloaker
            if (!PhotonNetwork.isMasterClient || !InComp.IsEquipped)
                return;
            if (InComp.ShipStats == null || InComp.ShipStats.Ship == null)
                return;
            PLPolytechShipInfo shipInfo = InComp.ShipStats.Ship as PLPolytechShipInfo;
            if (!shipInfo.StartupSwitchBoard.GetStatus(0))
            {
                this.LastProgramTickTime = Time.time;
                return;
            }
            if ((double)Time.time - (double)this.LastProgramTickTime > 15.0)
            {
                this.LastProgramTickTime = Time.time;
                InComp.ShipStats.Ship.MyWarpDrive.ChargePrograms(overrideChargeCount: 4);
            }
            PLShipInfo ship = PLEncounterManager.Instance.PlayerShip;
            if (ship.GetIsCloakingSystemActive() && InComp.ShipStats.Ship.PersistantShipInfo.MyCurrentSector.ID == ship.PersistantShipInfo.MyCurrentSector.ID && !ship.Get_IsInWarpMode())
                ship.TakeDamage(500f, false, EDamageType.E_ENERGY, 1f, -1, InComp.ShipStats.Ship, -1);
        }
    }

}
