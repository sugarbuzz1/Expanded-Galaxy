using PulsarModLoader;
using PulsarModLoader.Content.Components.PolytechModule;
using UnityEngine;

namespace ExpandedGalaxy
{
    internal class BossPT4 : PolytechModuleMod
    {
        private float LastProgramTickTime;
        public static float LastEMPTime = -1f;
        public override string Name => "P.T. Module: Recompiler 4";

        public override string Description => "If you're reading this, I fucked up :(";

        public override int MarketPrice => 9999999;

        public override float MaxPowerUsage_Watts => 1f;

        public override bool Experimental => false;

        public override bool Contraband => true;

        public override bool CanBeDroppedOnShipDeath => false;

        public override void Tick(PLShipComponent InComp)
        {
            if (!InComp.IsEquipped)
                return;
            if (InComp.ShipStats == null || InComp.ShipStats.Ship == null)
                return;
            ModdedCountdown.Instance.Boss = InComp.ShipStats.Ship;
            ModdedCountdown.Instance.BossComponent = InComp;
            if (!PhotonNetwork.isMasterClient)
                return;
            if (InComp.SubTypeData < 0 || InComp.SubTypeData > 1)
                InComp.SubTypeData = 0;
            if (InComp.IsEquipped && InComp.ShipStats.Ship.ShipID == PLEncounterManager.Instance.PlayerShip.ShipID)
            {
                InComp.FlagForSelfDestruction();
                return;
            }
            PLPolytechShipInfo shipInfo = InComp.ShipStats.Ship as PLPolytechShipInfo;
            if (!shipInfo.StartupSwitchBoard.GetStatus(0))
            {
                this.LastProgramTickTime = Time.time;
                LastEMPTime = Time.time;
                InComp.SubTypeData = 0;
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

            if (LastEMPTime == -1f || PLEncounterManager.Instance.PlayerShip.Get_IsInWarpMode())
            {
                LastEMPTime = Time.time;
                InComp.SubTypeData = 0;
            }
            if ((double)Time.time - (double)LastEMPTime > 10.0 && !PLEncounterManager.Instance.PlayerShip.Get_IsInWarpMode())
            {
                if (InComp.SubTypeData == 0)
                {
                    ModMessage.SendRPC("sugarbuzz1.ExpandedGalaxy", "ExpandedGalaxy.TriggerModdedCountdownRPC", PhotonTargets.All, new object[1] { 0 });
                }
                InComp.SubTypeData = 1;
                if ((double)Time.time - (double)LastEMPTime > 30.0)
                {
                    LastEMPTime = Time.time;
                    InComp.SubTypeData = 0;
                    if (PhotonNetwork.isMasterClient)
                    {
                        ModMessage.SendRPC("sugarbuzz1.ExpandedGalaxy", "ExpandedGalaxy.EMPPulse", PhotonTargets.All, new object[2]
                        {
                            (object) InComp.ShipStats.Ship.ShipID,
                            (object) 5000f
                        });
                    }
                }
            }
        }

        public override void FinalLateAddStats(PLShipComponent InComp)
        {
            if (InComp.SubTypeData == 1)
                InComp.ShipStats.Ship.DischargeAmount = 0.9f;
            else
                InComp.ShipStats.Ship.DischargeAmount = 0.0f;
        }
    }

}
