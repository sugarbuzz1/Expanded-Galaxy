using Talents.Framework;

namespace ExpandedGalaxy
{
    public class CustomJetpackFuel2 : TalentMod
    {
        public override string Name => "Jetpack Fuel Reserve";

        public override string Description => "Increases your jetpack's fuel reserve by 25% per rank";

        public override int MaxRank => 4;

        public override int[] ResearchCost => new int[6]
        {
                2,
                0,
                0,
                0,
                0,
                0,
        };

        public override int WarpsToResearch => 2;

        public override bool NeedsToBeResearched => true;
    }
}
