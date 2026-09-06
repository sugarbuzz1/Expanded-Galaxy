using HarmonyLib;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLServer), "GetCoolantBasePrice")]
    internal class CoolantChaosEventPrice
    {
        private static void Postfix(ref int __result)
        {
            if (PLServer.Instance == null)
                return;
            if (PLServer.Instance.IsChaosEventActive(EChaosEvent.E_COOLANT_SHORTAGE))
                __result *= 10;
        }
    }
}
