using HarmonyLib;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PawnStatusEffect), "GetTypeAsString")]
    internal class PawnStatusEffectText
    {
        private static void Postfix(PawnStatusEffect __instance, ref string __result)
        {
            if ((int)__instance.Type < 16)
                return;
            switch ((int)__instance.Type)
            {
                case 16:
                    __result = "Speed Boost";
                    break;
            }
        }
    }
}
