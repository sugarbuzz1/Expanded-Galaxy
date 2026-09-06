namespace ExpandedGalaxy
{
    public class Shop_Caravan : PLShop
    {
        public bool HasPDEBeenSetup;
        public override void Start()
        {
            base.Start();
            this.Name = "Wandering Caravan";
            this.Desc = "The best exotic ware seller around!";
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
