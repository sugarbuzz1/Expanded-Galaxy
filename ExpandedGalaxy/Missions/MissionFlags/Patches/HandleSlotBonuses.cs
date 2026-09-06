using HarmonyLib;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLShipStats), "AddShipComponent")]
    internal class HandleSlotBonuses
    {
        private static bool Prefix(PLShipStats __instance, PLShipComponent inComponent, int inNetID, ESlotType visualSlot)
        {
            if (inComponent is MissionAddOneCPUSlotComp && __instance.GetSlot(ESlotType.E_COMP_CPU).MaxItems < 8)
                ++__instance.GetSlot(ESlotType.E_COMP_CPU).MaxItems;
            return true;
        }

        private static void Postfix(PLShipStats __instance, PLShipComponent inComponent, int inNetID, ESlotType visualSlot)
        {
            if (!(inComponent is MissionAddOneCPUSlotComp))
                return;
            if (!__instance.AllComponents.Contains(inComponent))
                return;
            __instance.AllComponents.Remove(inComponent);
            __instance.AllComponents.Insert(0, inComponent);
        }
    }

}

