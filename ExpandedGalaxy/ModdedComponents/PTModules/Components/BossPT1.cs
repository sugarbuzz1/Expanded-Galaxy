using PulsarModLoader.Content.Components.PolytechModule;
using UnityEngine;

namespace ExpandedGalaxy
{
    public class BossPT1 : PolytechModuleMod
    {
        public override string Name => "P.T. Module: Recompiler 1";

        public override string Description => "If you're reading this, I fucked up :(";

        public override int MarketPrice => 9999999;

        public override float MaxPowerUsage_Watts => 1f;

        public override bool Experimental => false;

        public override bool Contraband => true;

        public override bool CanBeDroppedOnShipDeath => false;

        public override void Tick(PLShipComponent InComp)
        {
            if (!PhotonNetwork.isMasterClient)
                return;
            if (!InComp.ShipStats.Ship.ShipNameValue.Contains("The Recompiler: Config. " + PFSectorCommander.bossFlag.ToString()))
                InComp.ShipStats.Ship.ShipNameValue = "The Recompiler: Config. " + PFSectorCommander.bossFlag.ToString();
            if (InComp.SubTypeData < 0)
                InComp.SubTypeData = 300;
            if (InComp.IsEquipped && InComp.SubTypeData < 300)
                InComp.SubTypeData += 10;
            if (InComp.IsEquipped && InComp.ShipStats.Ship.ShipID == PLEncounterManager.Instance.PlayerShip.ShipID)
            {
                InComp.FlagForSelfDestruction();
                return;
            }
            if (PFSectorCommander.bossFlag< 2)
            {
                PLShipComponent component = InComp.ShipStats.GetShipComponent<PLShipComponent>(ESlotType.E_COMP_MAINTURRET);
                if (component != null)
                    InComp.ShipStats.RemoveShipComponent(component);
            }
        }

        public override void AddStats(PLShipComponent InComp)
        {
            base.AddStats(InComp);
            PLShipStats shipStats = InComp.ShipStats;
            shipStats.HullArmor += InComp.SubTypeData / 250f;
        }

        public override void LateAddStats(PLShipComponent InComp)
        {
            PLShipStats shipStats = InComp.ShipStats;
            shipStats.ShieldsChargeRate += Mathf.Clamp((1f - (shipStats.HullCurrent / shipStats.HullMax)) * 75f, 0f, 50f);
            shipStats.ShieldsChargeRateMax += Mathf.Clamp((1f - (shipStats.HullCurrent / shipStats.HullMax)) * 75f, 0f, 50f); ;
        }
    }

}
