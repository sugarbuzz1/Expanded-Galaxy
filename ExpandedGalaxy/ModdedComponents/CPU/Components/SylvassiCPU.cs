using PulsarModLoader;
using PulsarModLoader.Content.Components.CPU;
using UnityEngine;

namespace ExpandedGalaxy
{
    public class SylvassiCPU : CPUMod
    {
        public override string Name => "Sylvassi Shield Charger";

        public override string Description => "Shield co-processor that greatly boosts shield charge rate at the cost of a massive power draw.";

        public override int MarketPrice => 20000;

        public override Texture2D IconTexture => (Texture2D)Resources.Load("Icons/63_Processer");

        public override int CargoVisualID => 37;

        public override float MaxPowerUsage_Watts => 1f;

        public override int SysInstConduit => 2;

        public override string GetStatLineLeft(PLShipComponent InComp) => "Shield Recharge" + "\n" + "Status" + "\n";

        public override string GetStatLineRight(PLShipComponent InComp)
        {
            PLCPU plcpu = InComp as PLCPU;

            if (!plcpu.IsEquipped || !plcpu.IsPowered)
            {
                return "+" + (0).ToString() + "/" + (100 + plcpu.Level * 10).ToString() + "\n" + "INACTIVE" + "\n";
            }
            if (plcpu.SubTypeData == 0)
            {
                return "+" + (0).ToString() + "/" + (100 + plcpu.Level * 10).ToString() + "\n" + "DISABLED" + "\n";
            }
            else
            {
                return "+" + ((100 + plcpu.Level * 10) * plcpu.GetPowerPercentInput()).ToString("0") + "/" + (100 + plcpu.Level * 10).ToString() + "\n" + "ENABLED" + "\n";
            }
        }

        public override void OnWarp(PLShipComponent InComp)
        {

        }

        public override void FinalLateAddStats(PLShipComponent InComp)
        {
            PLCPU plcpu = InComp as PLCPU;
            PLShipStats shipStats = InComp.ShipStats;
            if (InComp.ShipStats.ShieldsCurrent < InComp.ShipStats.ShieldsMax)
            {
                if (InComp.SubTypeData > 0)
                {
                    shipStats.ShieldsChargeRate += plcpu.GetPowerPercentInput() * (100f + plcpu.Level * 10f);
                    shipStats.ShieldsChargeRateMax += (100f + plcpu.Level * 10f);
                }
            }
        }

        public override void Tick(PLShipComponent InComp)
        {
            base.Tick(InComp);
            PLCPU plcpu = InComp as PLCPU;
            if (plcpu.SubTypeData < 0 || plcpu.SubTypeData > 1)
            {
                plcpu.SubTypeData = 0;
                if (PhotonNetwork.isMasterClient)
                    ModMessage.SendRPC("sugarbuzz1.ExpandedGalaxy", "ExpandedGalaxy.UpdateSubTypeData", PhotonTargets.Others, new object[3]
                    {
                        (object) InComp.ShipStats.Ship.ShipID,
                        (object) InComp.NetID,
                        (object) InComp.SubTypeData,
                    });
            }
            if (!plcpu.IsEquipped && plcpu.SubTypeData != 0)
            {
                plcpu.SubTypeData = 0;
                if (PhotonNetwork.isMasterClient)
                    ModMessage.SendRPC("sugarbuzz1.ExpandedGalaxy", "ExpandedGalaxy.UpdateSubTypeData", PhotonTargets.Others, new object[3]
                    {
                        (object) InComp.ShipStats.Ship.ShipID,
                        (object) InComp.NetID,
                        (object) InComp.SubTypeData,
                    });
                return;
            }
            if (plcpu.SubTypeData == 0)
            {
                plcpu.CalculatedMaxPowerUsage_Watts = 1f;
            }
            else
            {
                plcpu.CalculatedMaxPowerUsage_Watts = (40000f + 4000f * plcpu.Level);
            }
            if (PLNetworkManager.Instance.MyLocalPawn != null && PLNetworkManager.Instance.MyLocalPawn.CurrentShip != null && PLNetworkManager.Instance.MyLocalPawn.CurrentShip == plcpu.ShipStats.Ship && (PLNetworkManager.Instance.LocalPlayer.GetClassID() == 0 || PLNetworkManager.Instance.LocalPlayer.GetClassID() == 2) && PLInput.Instance.GetButtonUp("ExpandedGalaxy.Ability") && !PLNetworkManager.Instance.LocalPlayer.IsSittingInCaptainsChair() && PLCameraSystem.Instance.CurrentCameraMode != null && PLCameraSystem.Instance.CurrentCameraMode.GetModeString() == "LocalPawn")
            {
                ModMessage.SendRPC("sugarbuzz1.ExpandedGalaxy", "ExpandedGalaxy.UpdateSylvassiCPU", PhotonTargets.All, new object[1]
                {
                        (object)PLNetworkManager.Instance.MyLocalPawn.CurrentShip.ShipID
                });
            }
        }
    }
}
