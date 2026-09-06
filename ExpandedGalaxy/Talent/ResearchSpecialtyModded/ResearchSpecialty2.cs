using Talents.Framework;

namespace ExpandedGalaxy
{
    public class ResearchSpecialty2 : TalentMod
    {
        public override string Name => "Research Specialty";

        public override string Description => "Gain a material rebate upon researching a talent";

        public override int MaxRank => 1;

        public override int ClassID => (int)TalentModManager.CharacterClass.Scientist;

        public override int[] ResearchCost => new int[6]
        {
                0,
                0,
                0,
                2,
                0,
                2,
        };

        public override bool NeedsToBeResearched => true;
    }
}
