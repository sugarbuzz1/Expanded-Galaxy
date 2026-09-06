using PulsarModLoader;
using PulsarModLoader.Utilities;

namespace ExpandedGalaxy
{
    internal class ClientUnlockAchievement : ModMessage
    {
        public override void HandleRPC(object[] arguments, PhotonMessageInfo sender)
        {
            int index = (int)arguments[0];
            if (Achievements.HasUnlockedAchievement(index))
                return;
            SaveValues.AchievementData.Value = SaveValues.AchievementData.Value | (1U << index);
            if (PLServer.Instance != null)
            {
                PLServer.Instance.AddCrewWarning("ACHIEVEMENT UNLOCKED", Relic.GetRelicColor(), 0, "ACV");
                Messaging.Echo(PLNetworkManager.Instance.LocalPlayer, string.Format("<color=#5500FF>[ExGal]</color> Achievement Unlocked: {0}!", Achievements.GetAchievement(index, out string desc)));
            }
        }
    }
}
