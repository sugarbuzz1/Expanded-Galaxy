using PulsarModLoader;
using PulsarModLoader.Utilities;

namespace ExpandedGalaxy
{
    internal class Achievements
    {
        public static bool HasUnlockedAchievement(int index)
        {
            return (SaveValues.AchievementData.Value & (1U << index)) > 0;
        }

        internal static string GetAchievement(int index, out string desc)
        {
            switch (index)
            {
                case 1:
                    desc = "Defeat the Recompiler as a Polytech Federation crew";
                    return "Apostasy Denied";
                case 2:
                    desc = "Successfully locate the Mining Drone Hub World";
                    return "Echoes of Industry";
                case 3:
                    desc = "Unravel the anomoly that is the Reflected Rift";
                    return "Riftwalker";
                case 4:
                    desc = "Defeat a certain wandering boss fight";
                    return "Reclamation";
                default:
                    desc = "Defeat the Recompiler at maximum chaos on expert settings without warping away";
                    return "Full Rebuild";
            }
        }
    }
}
