using HarmonyLib;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLUICreateGameMenu), "Enter")]
    internal class SetPreferencesWhenCreatingGame
    {
        private static void Postfix()
        {
            SaveValues.LoadPreferences();
        }
    }
}
