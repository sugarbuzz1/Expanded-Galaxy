using HarmonyLib;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLLocalize), "Localize", new System.Type[] { typeof(string), typeof(string), typeof(bool) })]
    internal class SkipLocalization
    {
        private static void Postfix(ref string __result, string value)
        {
            if (!(__result == string.Empty))
                return;
            __result = value;
        }
    }

}

