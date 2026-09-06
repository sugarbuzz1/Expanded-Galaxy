using HarmonyLib;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLWarpDrive), "ChargePrograms")]
    internal class ChargePrograms
    {
        private static bool Prefix(PLWarpDrive __instance, ref bool chargeToFull, int overrideChargeCount)
        {
            if (!__instance.ShipStats.Ship.GetIsPlayerShip() && overrideChargeCount == -1) // Full charge programs
                chargeToFull = true;
            if ((overrideChargeCount != -1 || chargeToFull) && (int)PLServer.Instance.JumpsNeededToResearchTalent > 0) // Capacitor doesn't advance research
                ++PLServer.Instance.JumpsNeededToResearchTalent;
            return true;
        }
    }
}
