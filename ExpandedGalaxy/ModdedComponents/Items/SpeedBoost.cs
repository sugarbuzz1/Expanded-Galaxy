using HarmonyLib;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLPlayerController), "HandleMovement")]
    internal class SpeedBoost
    {
        private static void Prefix(PLPlayerController __instance)
        {
            if (!(bool)(UnityEngine.Object)__instance.MyPawn || !(bool)(UnityEngine.Object)__instance.MyPawn.GetPlayer())
                return;
            bool speedFlag = false;
            foreach (PawnStatusEffect statusEffect in __instance.MyPawn.MyStatusEffects)
            {
                if (statusEffect != null && (int)statusEffect.Type == 16)
                {
                    speedFlag = true;
                    break;
                }
            }
            __instance.DefaultPawnSpeed = speedFlag ? 6f : 4f;
            __instance.SprintPawnSpeed = speedFlag ? 9f : 6f;
        }
    }
}
