using HarmonyLib;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLSensorObjectShip), "GetIsCloaked")]
    internal class Remove0EMSignature
    {
        private static void Postfix(ref bool __result)
        {
            __result = false;
        }
    }
}
