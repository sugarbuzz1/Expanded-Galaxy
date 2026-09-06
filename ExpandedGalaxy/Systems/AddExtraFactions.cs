using HarmonyLib;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLGalaxy), "Reset")]
    internal class AddExtraFactions
    {
        private static void Postfix(PLGalaxy __instance, int inSeed)
        {
            __instance.AddFaction(new PLFactionInfo_Polytechnic());
            __instance.AddFaction(new PLFactionInfo_Unknown());
        }
    }
}

