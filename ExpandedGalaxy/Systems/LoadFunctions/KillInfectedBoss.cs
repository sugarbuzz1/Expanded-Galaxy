using HarmonyLib;
using UnityEngine;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLInfectedBoss_WDFlagship), "Start")]
    internal class KillInfectedBoss
    {
        private static void Postfix(PLInfectedBoss_WDFlagship __instance)
        {
            if (!(PLServer.Instance != null))
                GameObject.DestroyImmediate(__instance.gameObject);
        }
    }
}
