using HarmonyLib;
using PulsarModLoader.Patches;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLShipInfo), "CreateShipInstUIs")]
    internal class WeaponSysFix
    {
        internal static int HelperMethod(PLSlot slot)
        {
            int n = slot.MaxItems;
            if (n > 3)
                n = 3;
            return n;
        }

        private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
        {
            List<CodeInstruction> list = instructions.ToList();

            List<CodeInstruction> targetSequence = new List<CodeInstruction>()
            {
                new CodeInstruction(OpCodes.Callvirt, AccessTools.Method(typeof(PLSlot), "get_MaxItems")),
            };
            List<CodeInstruction> patchSequence = new List<CodeInstruction>()
                {
                    CodeInstruction.Call(typeof(ExpandedGalaxy.WeaponSysFix), "HelperMethod", new Type[1]
                    {
                        typeof(PLSlot)
                    }),
                };
            return HarmonyHelpers.PatchBySequence(list.AsEnumerable<CodeInstruction>(), targetSequence, patchSequence, HarmonyHelpers.PatchMode.REPLACE, HarmonyHelpers.CheckMode.NONNULL, false);
        }

        private static void Postfix(PLShipInfo __instance)
        {
            if (__instance.MyStats.GetSlot(ESlotType.E_COMP_TURRET).MaxItems > 2)
            {
                foreach (PLSysInstPowerBarUI ui in __instance.AllSysIntPowerBars)
                {
                    if (ui.ID == 13)
                    {
                        ui.Name.text = "Other Turrets";
                    }
                }
            }
        }
    }
}
