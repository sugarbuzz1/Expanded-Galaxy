using Talents.Framework;

namespace ExpandedGalaxy
{
    public class ProbeLocator : TalentMod
    {
        public override string Name => "Probe Specialty: Locator";

        public override string Description => "Probe spots are highlighted in the sensor dish view";

        public override int MaxRank => 1;

        public override int ClassID => (int)TalentModManager.CharacterClass.Scientist;

        public override int[] ResearchCost => new int[6]
        {
                0,
                0,
                1,
                0,
                2,
                1,
        };

        public override bool NeedsToBeResearched => true;
    }
}
