using Talents.Framework;

namespace ExpandedGalaxy
{
    public class Reloader2 : TalentMod
    {
        public override string Name => "Reloader";

        public override string Description => "Reloads nearby crew weapons over time";

        public override int MaxRank => 5;

        public override int ClassID => (int)TalentModManager.CharacterClass.Weapons;

        public override int[] ResearchCost => new int[6]
        {
                0,
                0,
                0,
                0,
                0,
                1,
        };

        public override int WarpsToResearch => 2;

        public override bool NeedsToBeResearched => true;
    }
}
