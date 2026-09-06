using Talents.Framework;

namespace ExpandedGalaxy
{
    public class CrewAcademyTraining : TalentMod
    {
        public override string Name => "Crew Academy Training";

        public override string Description => "All other crew members gain additional talent point (+1 per rank)";

        public override int MaxRank => 4;

        public override int ClassID => (int)TalentModManager.CharacterClass.Captain;

        public override int[] ResearchCost => new int[6]
        {
                2,
                2,
                0,
                0,
                0,
                2,
        };

        public override bool NeedsToBeResearched => true;
    }
}
