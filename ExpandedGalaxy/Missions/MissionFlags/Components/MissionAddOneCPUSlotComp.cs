namespace ExpandedGalaxy
{
    public class MissionAddOneCPUSlotComp : MissionComponentFlag
    {
        public MissionAddOneCPUSlotComp(int inLevel = 0) : base(2, inLevel)
        {
            this.Name = "ExGal_AddOneCPUSlot";
            this.Desc = "If you're reading this, I fucked up :(";
            this.CanBeDroppedOnShipDeath = false;
        }

        public override void Tick()
        {
            base.Tick();
            if (this.ShipStats == null)
                return;
            if (this.ShipStats.AllComponents[0] != this)
            {
                this.ShipStats.AllComponents.Remove(this);
                this.ShipStats.AllComponents.Insert(0, this);
            }
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
        }
    }

}

