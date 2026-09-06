using HarmonyLib;
using PulsarModLoader.Content.Components.CaptainsChair;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLCaptainsChair), "LateAddStats")]
    internal class ChairLateAddStats
    {
        private static bool Prefix(PLCaptainsChair __instance, PLShipStats inStats)
        {
            switch (__instance.SubType)
            {
                case 0:
                    inStats.EMDetection *= 1 + ((8 + __instance.Level) / 100f);
                    return false;
                case 1:
                    inStats.ShieldsChargeRate *= 1 + ((8 + __instance.Level) / 100f);
                    inStats.ShieldsChargeRateMax *= 1 + ((8 + __instance.Level) / 100f);
                    return false;
                case 2:
                    inStats.TurretChargeFactor *= 1 + ((8 + __instance.Level) / 100f);
                    return false;
            }
            if (__instance.SubType == CaptainsChairModManager.Instance.GetCaptainsChairIDFromName("W.D. Modern Captain's Chair"))
            {
                inStats.HullArmor *= 1 + ((8 + __instance.Level) / 100f);
                return false;
            }
            return false;
        }
    }
}
