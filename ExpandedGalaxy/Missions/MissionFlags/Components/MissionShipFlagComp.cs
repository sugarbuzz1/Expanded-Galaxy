using UnityEngine;

namespace ExpandedGalaxy
{
    public class MissionShipFlagComp : MissionComponentFlag
    {
        public MissionShipFlagComp(int inLevel = 0) : base(1, inLevel)
        {
            this.Name = "ExGal_ProjVulcanus_Flag";
            this.CanBeDroppedOnShipDeath = false;
            this.SubTypeData = 0;
        }

        public override void Tick()
        {
            base.Tick();
            if (!PhotonNetwork.isMasterClient)
                return;
            if (this.ShipStats != null && this.VisualSlotType != ESlotType.E_COMP_REAC_COOLING)
            {
                this.Equip();
                PLServer.Instance.photonView.RPC("CaptainChangeItemVisualSlot", PhotonTargets.All, new object[3]
                {
                        this.ShipStats.Ship.ShipID,
                        this.NetID,
                        (int)ESlotType.E_COMP_REAC_COOLING
                });
            }
            foreach (PLSlot slot in this.ShipStats.GetAllSlots())
            {
                if (slot.Type != ESlotType.E_COMP_CARGO && slot.Type != ESlotType.E_COMP_HIDDENCARGO && slot.Type != ESlotType.E_COMP_REAC_COOLING && !slot.Locked)
                    slot.Locked = true;
            }
            if (PLServer.Instance.HasCompletedMissionWithID(8000004) && PLEncounterManager.Instance.GetCPEI() != null && !this.ShipStats.Ship.IsAbandoned())
            {
                foreach (PLShipInfoBase pLShipInfoBase in PLEncounterManager.Instance.GetCPEI().MyCreatedShipInfos)
                {
                    bool flag = false;
                    foreach (PLShipComponent pLShipComponent in pLShipInfoBase.MyStats.AllComponents)
                    {
                        if (pLShipComponent is MissionAddOneCPUSlotComp)
                        {
                            flag = true;
                            break;
                        }
                    }
                    if (flag)
                    {
                        this.ShipStats.Ship.SetAbandoned(true);
                        pLShipInfoBase.SetAbandoned(false);
                        if (pLShipInfoBase.MyStats.GetShipComponent<PLShipComponent>(ESlotType.E_COMP_REAC_COOLING) != null)
                            pLShipInfoBase.MyStats.RemoveShipComponent(pLShipInfoBase.MyStats.GetShipComponent<PLShipComponent>(ESlotType.E_COMP_REAC_COOLING));
                        PLServer.Instance.photonView.RPC("ClaimShip", PhotonTargets.All, pLShipInfoBase.ShipID);
                        foreach (PLPlayer allPlayer in PLServer.Instance.AllPlayers)
                        {
                            if (allPlayer != null)
                            {
                                if (allPlayer.MyCurrentTLI != null && allPlayer.MyCurrentTLI.MyShipInfo != null && allPlayer.MyCurrentTLI.MyShipInfo.ShipID == this.ShipStats.Ship.ShipID)
                                {
                                    allPlayer.SetSubHubAndTTIID(pLShipInfoBase.MyTLI.SubHubID, 0);
                                    allPlayer.CurrentlyInLiarsDiceGame = (PLLiarsDiceGame)null;
                                }
                            }
                        }
                        PhotonNetwork.Destroy(this.ShipStats.Ship.ShipRoot);
                    }
                }
            }
        }

        public override void FinalLateAddStats(PLShipStats inStats)
        {
            inStats.TurretDamageFactor *= Mathf.Clamp(1.2f - 0.05f * this.SubTypeData, 0.5f, 1.2f);
            inStats.QuantumShieldDefensesActive = inStats.ShieldsCurrent / inStats.ShieldsMax > Mathf.Clamp01(0.1f + 0.2f * this.SubTypeData);
            inStats.ReactorOutputFactor *= Mathf.Clamp(1.2f - 0.05f * this.SubTypeData, 0.5f, 1.2f);
            inStats.HullArmor += Mathf.Clamp(100f - 20f * this.SubTypeData, 0f, 100f) / 250f;
        }

        public override void OnWarp()
        {
            base.OnWarp();
            this.SubTypeData++;
        }
    }

}

