using HarmonyLib;
using PulsarModLoader.Patches;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLShipInfo), "GetUpgradableComponents")]
    internal class MoreUpgrades
    {
        public static void Patch(PLShipInfo instance, List<PLShipComponent> m_CachedUpgradableComponents)
        {
            m_CachedUpgradableComponents.AddRange((IEnumerable<PLShipComponent>)instance.MyStats.GetComponentsOfType(ESlotType.E_COMP_AUTO_TURRET, false));
            m_CachedUpgradableComponents.AddRange((IEnumerable<PLShipComponent>)instance.MyStats.GetComponentsOfType(ESlotType.E_COMP_HULLPLATING, false));
            m_CachedUpgradableComponents.AddRange((IEnumerable<PLShipComponent>)instance.MyStats.GetComponentsOfType(ESlotType.E_COMP_CAPTAINS_CHAIR, false));
        }
        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            return HarmonyHelpers.PatchBySequence(instructions, new CodeInstruction[]
            {
                new CodeInstruction(OpCodes.Ldarg_0, null),
                new CodeInstruction(OpCodes.Ldfld, AccessTools.Field(typeof(PLShipInfo), "m_CachedUpgradableComponents")),
                new CodeInstruction(OpCodes.Callvirt, null)
            }, new CodeInstruction[]
            {
                new CodeInstruction(OpCodes.Ldarg_0, null),
                new CodeInstruction(OpCodes.Dup, null),
                new CodeInstruction(OpCodes.Ldfld, AccessTools.Field(typeof(PLShipInfo), "m_CachedUpgradableComponents")),
                new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(MoreUpgrades), "Patch", null, null)),
            }, HarmonyHelpers.PatchMode.AFTER, HarmonyHelpers.CheckMode.NONNULL, false);
        }
    }
}

