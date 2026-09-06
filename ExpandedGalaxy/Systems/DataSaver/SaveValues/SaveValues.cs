using PulsarModLoader;

namespace ExpandedGalaxy
{
    internal class SaveValues
    {
        internal static SaveValue<bool> AdvancedJetpack = new SaveValue<bool>(nameof(AdvancedJetpack), true);
        internal static SaveValue<bool> DynamicAmmunition = new SaveValue<bool>(nameof(DynamicAmmunition), true);
        internal static SaveValue<bool> BetterExosuit = new SaveValue<bool>(nameof(BetterExosuit), true);
        internal static SaveValue<bool> SlowMissionPickups = new SaveValue<bool>(nameof(SlowMissionPickups), true);

        internal static SaveValue<ulong> CosmeticData = new SaveValue<ulong>(nameof(CosmeticData), ulong.MinValue);
        internal static SaveValue<ulong> AchievementData = new SaveValue<ulong>(nameof(AchievementData), ulong.MinValue);

        internal static void SavePreferences()
        {
            AdvancedJetpack.Value = Jetpack.AdvancedJetPack;
            DynamicAmmunition.Value = Ammunition.DynamicAmmunition;
            BetterExosuit.Value = Exosuit.BetterExosuit;
            SlowMissionPickups.Value = Missions.slowMissionPickups;
        }

        internal static void LoadPreferences()
        {
            Jetpack.AdvancedJetPack = AdvancedJetpack.Value;
            Ammunition.DynamicAmmunition = DynamicAmmunition.Value;
            Exosuit.BetterExosuit = BetterExosuit.Value;
            Missions.slowMissionPickups = SlowMissionPickups.Value;
        }
    }
}
