using HarmonyLib;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLGameStatic), "Update")]
    internal class NoUpdateWhenLoading
    {
        private static bool Prefix()
        {
            return CustomLoad.CustomLoadComplete;
        }
    }
}
