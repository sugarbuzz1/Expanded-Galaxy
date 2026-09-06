using HarmonyLib;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;

namespace ExpandedGalaxy
{
    //i made this with chatgpt because i am lazy
    [HarmonyPatch]
    public static class Patch_UpdateMainItems_ReplaceString
    {
        static MethodBase TargetMethod()
        {
            // Get the nested compiler-generated state machine for UpdateMainItems
            var outerType = typeof(PLItemShopMenu);
            foreach (var nested in outerType.GetNestedTypes(BindingFlags.NonPublic | BindingFlags.Instance))
            {
                if (nested.Name.Contains("UpdateMainItems"))
                {
                    return AccessTools.Method(nested, "MoveNext");
                }
            }
            throw new System.Exception("Failed to locate state machine for UpdateMainItems");
        }

        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            foreach (var instruction in instructions)
            {
                if (instruction.opcode == OpCodes.Ldstr && instruction.operand is string str && str == " / 200)")
                {
                    instruction.operand = " / 50)";
                }
                yield return instruction;
            }
        }
    }
}
