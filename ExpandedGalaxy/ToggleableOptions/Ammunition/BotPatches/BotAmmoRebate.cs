using HarmonyLib;
using System.Collections;
using UnityEngine;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLPawnItem_Gun), "OnFire_ChanceForAmmo")]
    internal class BotAmmoRebate
    {
        private static bool Prefix(PLPawnItem_Gun __instance, ref PLPawnItemInstance ___MyGunInstance, ref PLPawn ___MySetupPawn)
        {
            if (__instance.UsesAmmo && ___MyGunInstance != null && ___MySetupPawn != null && ___MySetupPawn != PLNetworkManager.Instance.MyLocalPawn && ___MySetupPawn.GetPlayer() != null && ___MySetupPawn.GetPlayer() != PLNetworkManager.Instance.LocalPlayer)
            {
                float num = (float)(int)___MySetupPawn.GetPlayer().Talents[49] * 0.01f;
                if ((double)num > 0.0 && (double)UnityEngine.Random.value < (double)num)
                    ___MyGunInstance.StartCoroutine(LateExtraAmmoReloadBot(__instance));

            }
            return true;
        }

        private static IEnumerator LateExtraAmmoReloadBot(PLPawnItem_Gun plPawnItemGun)
        {
            yield return (object)new WaitForSeconds(0.25f);
            plPawnItemGun.AmmoCurrent += Mathf.CeilToInt((float)plPawnItemGun.AmmoMax * 0.2f);
            plPawnItemGun.AmmoCurrent = Mathf.Clamp(plPawnItemGun.AmmoCurrent, 0, plPawnItemGun.AmmoMax);
        }
    }
}
