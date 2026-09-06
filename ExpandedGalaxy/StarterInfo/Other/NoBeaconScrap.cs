using HarmonyLib;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLBeaconInfo), "SetupShipStats")]
    internal class NoBeaconScrap
    {
        private static void Postfix(PLBeaconInfo __instance, bool previewStats, bool startingPlayerShip)
        {
            if (__instance == null || __instance.MyStats == null)
                return;
            if (!__instance.ShouldCreateDefaultComponents || !(PhotonNetwork.isMasterClient | previewStats))
                return;
            __instance.DropScrap = false;
        }
    }
}
