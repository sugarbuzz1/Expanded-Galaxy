using HarmonyLib;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLWarpStationScreen), "Update")]
    internal class DisableWarpStationScreen
    {
        private static void Postfix(PLWarpStationScreen __instance, ref UISprite ___WarpPanel)
        {
            if (__instance.gameObject == null || PLServer.Instance == null)
                return;
            if (PLServer.GetCurrentSector() == null || PLServer.GetCurrentSector().MySPI == null || PLServer.GetCurrentSector().MySPI.Faction == 6)
                return;
            bool flag2 = !PLServer.Instance.IsChaosEventActive(EChaosEvent.E_LONG_RANGE_PRICE_HIKE);
            if (___WarpPanel.gameObject.activeSelf != flag2)
                ___WarpPanel.gameObject.SetActive(flag2);
        }
    }
}
