namespace ExpandedGalaxy
{
    public class MissionNoExtractorFlag : MissionComponentFlag
    {
        public MissionNoExtractorFlag(int inLevel = 0) : base(3, inLevel)
        {
            this.Name = "ExGal_NoExtractor_Flag";
            this.CanBeDroppedOnShipDeath = false;
        }

        public override void Tick()
        {
            base.Tick();
            if (this.ShipStats == null)
                return;
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

