using HarmonyLib;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLPawnItem_Food), "OnAction")]
    internal class PawnItemFoodAction
    {
        private static bool Prefix(PLPawnItem_Food __instance, string inAction)
        {
            if (__instance.MySetupPawn == null)
                return true;
            if (inAction == "Eat")
            {
                if (PLServer.Instance != null)
                {
                    PawnStatusEffect pawnStatusEffect = null;
                    if (__instance.SubType == (int)EFoodType.SUGAR_BISCUIT)
                    {
                        pawnStatusEffect = new PawnStatusEffect(PLServer.Instance.GetEstimatedServerMs(), (EPawnStatusEffectType)16, __instance.GenericStatMultiplierBasedOnCookedLevel());
                    }
                    if (pawnStatusEffect != null)
                    {
                        bool flag = false;
                        foreach (PawnStatusEffect statusEffect in __instance.MySetupPawn.MyStatusEffects)
                        {
                            if (statusEffect.Type == pawnStatusEffect.Type)
                            {
                                flag = true;
                                statusEffect.EndTime += pawnStatusEffect.EndTime - pawnStatusEffect.StartTime;
                                break;
                            }
                        }
                        if (flag)
                            return true;
                        __instance.MySetupPawn.MyStatusEffects.Add(pawnStatusEffect);
                    }
                }
            }
            return true;
        }
    }
}
