using HarmonyLib;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLUICreateGameMenu), "ClickEngage")]
    internal class CreateNewGame
    {
        private static void Postfix() => ResetFlags.OnNewGame();
    }
}
