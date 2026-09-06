using HarmonyLib;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLServer), "OnGameOver")]
    internal class EnsureGuidebookOnGameOver
    {
        private static void Postfix(PLServer __instance, bool backToMainMenu)
        {
            if (!backToMainMenu)
                NPCData.ServerStartGuidebookCoroutine(__instance);
        }
    }
}
