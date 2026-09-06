using Talents.Framework;

namespace ExpandedGalaxy
{
    public class EngineerImplant : TalentMod
    {
        public override string Name => "Engineer Eye Implant";

        public override string Description => "Gain system and reactor info UI while aboard home ship";

        public override int MaxRank => 1;

        public override int ClassID => (int)TalentModManager.CharacterClass.Engineer;

        public override int[] ResearchCost => new int[6]
        {
                0,
                0,
                1,
                2,
                0,
                0,
        };

        public override int WarpsToResearch => 4;

        public override bool NeedsToBeResearched => true;
    }
}
