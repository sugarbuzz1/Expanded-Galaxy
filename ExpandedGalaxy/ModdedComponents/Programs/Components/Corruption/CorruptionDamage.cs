using HarmonyLib;
using UnityEngine;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLVirus), "CorruptionUpdate")]
    internal class CorruptionDamage
    {
        private static void Postfix(PLVirus __instance, PLShipStats stats, ref float ___LastSiphenAttemptTime)
        {
            if (!PhotonNetwork.isMasterClient)
                return;
            if ((double)Time.time - (double)___LastSiphenAttemptTime <= 6.0)
                return;
            ___LastSiphenAttemptTime = Time.time;
            if (__instance.ShipStats.Ship != null)
            {
                PLMainSystem system = __instance.ShipStats.Ship.GetSystemFromID(UnityEngine.Random.Range(0, 4));
                if (system != null)
                {
                    system.TakeDamage((float)system.MaxHealth * UnityEngine.Random.Range(0.05f, 0.2f));
                }
            }
        }
    }
}

