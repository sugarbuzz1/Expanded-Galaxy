using HarmonyLib;
using UnityEngine;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLShipComponent), "CreateRandom")]
    internal class RandomMegaTurretInShop
    {
        private static bool Prefix(ref PLShipComponent __result)
        {
            int num = UnityEngine.Random.Range(0, 5000000) % 1000;
            if (num < 20)
            {
                switch (num % 5)
                {
                    case 0:
                        __result = new PLMegaTurret_RapidFire();
                        break;
                    case 1:
                        __result = new PhysicalTurret();
                        break;
                    default:
                        __result = new PLMegaTurret();
                        break;
                }
                __result.Level += Mathf.RoundToInt((UnityEngine.Object)PLServer.Instance != (UnityEngine.Object)null ? (float)PLServer.Instance.ChaosLevel * 0.5f : 0.0f);
                return false;
            }
            return true;
        }
    }

}
