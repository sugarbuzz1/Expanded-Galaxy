namespace ExpandedGalaxy
{
    internal class PLFactionInfo_Unknown : PLFactionInfo
    {
        public override void Setup(PLGalaxy inGalaxy)
        {
            base.Setup(inGalaxy);
            this.FactionID = 6;
        }

        public override void CreateStartingPoints(int inSeed)
        {
        }
    }
}

