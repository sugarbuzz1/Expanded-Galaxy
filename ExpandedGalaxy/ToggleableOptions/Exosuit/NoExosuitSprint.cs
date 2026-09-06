using HarmonyLib;
using System;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLPlayerController), "IsSprinting")]
    internal class NoExosuitSprint
    {
        private static void Postfix(PLPlayerController __instance, ref bool __result)
        {
            if (Exosuit.BetterExosuit)
            {
                if (__instance.MyPawn != null)
                {
                    if (__instance.MyPawn.GetExosuitIsActive())
                        __result = false;
                    else if (__instance.MyPawn.GetPlayer() != null && __instance.MyPawn.GetPlayer().RaceID == 2)
                        __result = false;
                }
            }
        }
    }
}
