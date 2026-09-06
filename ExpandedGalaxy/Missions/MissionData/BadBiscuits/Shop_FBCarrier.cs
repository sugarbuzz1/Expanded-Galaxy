namespace ExpandedGalaxy
{
    public class Shop_FBCarrier : PLShop
    {
        public bool HasPDEBeenSetup;
        public override void Start()
        {
            base.Start();
            this.Name = "Unknown.";
            this.Desc = "Buy or leave.";
            this.ContrabandDealer = true;
            if (!PhotonNetwork.isMasterClient)
                this.MyPDE = new TraderPersistantDataEntry();
        }

        public override void CreateSpecials(TraderPersistantDataEntry inPDE)
        {
        }

        public override float GetWareBuyPriceScalar() => 1f;
    }

}

