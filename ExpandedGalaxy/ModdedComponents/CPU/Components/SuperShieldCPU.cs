using PulsarModLoader.Content.Components.CPU;
using UnityEngine;

namespace ExpandedGalaxy
{
    public class SuperShieldCPU : CPUMod
    {
        public override string Name => "Super Shield";

        public override string Description => "Overclocks the ship's shields to make them impervious to any attack. This component is particularly vunerable to electromagnetic pulses...";

        public override int MarketPrice => 9999999;

        public override bool CanBeDroppedOnShipDeath => false;

        public override bool Contraband => true;

        public override Texture2D IconTexture => (Texture2D)Resources.Load("Icons/51_Processer");

        public override float MaxPowerUsage_Watts => 100f;

        public override string GetStatLineLeft(PLShipComponent InComp) => "Activating in: " + "\n";

        public override string GetStatLineRight(PLShipComponent InComp)
        {
            if (!InComp.IsEquipped)
                return "INACTIVE" + "\n";
            int num = InComp.SubTypeData;
            if (num > 0)
                return (num).ToString("0") + "\n";
            else
                return "ACTIVE" + "\n";
        }

        public override void Tick(PLShipComponent InComp)
        {
            base.Tick(InComp);
            if (PhotonNetwork.isMasterClient)
            {
                if (InComp.SubTypeData < 0 || InComp.SubTypeData > 35 * 80)
                    InComp.SubTypeData = 0;
                if (!InComp.IsEquipped)
                {
                    InComp.SubTypeData = 30 * 80;
                    return;
                }
                PLShipInfo info = InComp.ShipStats.Ship as PLShipInfo;
                if (info == null)
                    return;
                if (info.StartupSwitchBoard.GetLateStatus(0))
                {
                    if (InComp.SubTypeData > 0)
                        InComp.SubTypeData--;
                }
                else
                {
                    InComp.SubTypeData = 30 * 80;
                }
            }
            if (InComp.IsEquipped && InComp.ShipStats.Ship.ShipID == PLEncounterManager.Instance.PlayerShip.ShipID)
            {
                InComp.FlagForSelfDestruction();
                return;
            }
            if (InComp.SubTypeData <= 0 && InComp.IsEquipped)
            {
                InComp.ShipStats.Ship.SuperShieldActivateStartTime = PLServer.Instance.GetEstimatedServerMs() - 2000;
            }
        }
    }
}
