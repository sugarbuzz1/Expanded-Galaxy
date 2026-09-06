using HarmonyLib;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLServer), "GetFuelBasePrice")]
    internal class FuelChaosEventPrice
    {
        private static void Postfix(ref int __result)
        {
            if (PLServer.Instance == null)
                return;
            if (PLServer.Instance.IsChaosEventActive(EChaosEvent.E_FUEL_SHORTAGE))
                __result *= 5;
        }
    }
}
