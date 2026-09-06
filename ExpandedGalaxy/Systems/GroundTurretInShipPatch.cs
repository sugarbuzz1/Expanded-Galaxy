using CodeStage.AntiCheat.ObscuredTypes;
using HarmonyLib;
using UnityEngine;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLGroundTurret), "Start")]
    internal class GroundTurretInShipPatch
    {
        private static void Postfix(PLGroundTurret __instance)
        {
            if (__instance.CurrentShip != null)
            {
                __instance.gameObject.layer = 11;
                foreach (Transform transform in __instance.gameObject.GetComponentsInChildren<Transform>(true))
                {
                    transform.gameObject.layer = 11;
                }
                __instance.transform.GetChild(1).gameObject.SetActive(false);

                if (__instance.CurrentShip.PersistantShipInfo != null && __instance.CurrentShip.PersistantShipInfo.SelectedActorID == "ExGal_RelicCaravan")
                {
                    __instance.MaxHealth = (ObscuredFloat)250f;
                    __instance.Health = (ObscuredFloat)250f;
                }
            }
        }
    }
}

