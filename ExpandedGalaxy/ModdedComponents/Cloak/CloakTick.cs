using HarmonyLib;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLCloakingSystem), "Tick")]
    internal class CloakTick
    {
        private static void Postfix(PLCloakingSystem __instance)
        {
            if (PhotonNetwork.isMasterClient)
            {
                if (__instance.SubTypeData < 0)
                    __instance.SubTypeData = 0;
                if (__instance.ShipStats.Ship.GetIsCloakingSystemActive())
                {
                    if (__instance.SubTypeData < (5000 + 625 * __instance.Level))
                        __instance.SubTypeData += 10;
                }
                else
                {
                    if (__instance.SubTypeData > 0)
                        __instance.SubTypeData -= 10;
                }
            }
        }
    }
}
