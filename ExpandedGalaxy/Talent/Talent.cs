using Talents.Framework;

namespace ExpandedGalaxy
{
    internal class Talent
    {
        internal static void SetTalentsAsUnhidden()
        {
            TalentModManager.Instance.UnHideTalent((int)ETalents.INC_JETPACK);
            TalentModManager.Instance.UnHideTalent((int)ETalents.SCI_RESEARCH_SPECIALTY);
            TalentModManager.Instance.UnHideTalent((int)ETalents.WPN_AMMO_BOOST);
        }       
    }
}
