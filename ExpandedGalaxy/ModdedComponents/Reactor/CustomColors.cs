using HarmonyLib;
using PulsarModLoader.Content.Components.Reactor;
using UnityEngine;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLReactorInstance), "Update")]
    internal class CustomColors
    {
        private static void Postfix(PLReactorInstance __instance, ref float ___SmoothTotalUsagePercent)
        {
            if (!((UnityEngine.Object)__instance.MyShipInfo != (UnityEngine.Object)null) || !((UnityEngine.Object)PLServer.Instance != (UnityEngine.Object)null))
                return;
            if (!(__instance.MyShipInfo.MyStats.GetShipComponent<PLReactor>(ESlotType.E_COMP_REACTOR) != null && __instance.MyShipInfo.MyStats.GetShipComponent<PLReactor>(ESlotType.E_COMP_REACTOR).SubType == ReactorModManager.Instance.GetReactorIDFromName("Dark-Matter Reactor")))
                return;
            float num = Mathf.Clamp01(__instance.MyShipInfo.CoreInstability / 0.75f);
            float num1 = Mathf.Clamp01(__instance.MyShipInfo.MyStats.ReactorTotalUsagePercent / 0.75f);
            float num2 = Mathf.Clamp01(0.5f * num + 0.5f * num1);
            if ((UnityEngine.Object)__instance.WhiteParticles != (UnityEngine.Object)null)
            {
                __instance.WhiteParticles.emissionRate = Mathf.Clamp01(___SmoothTotalUsagePercent) * 150f;
            }
            if ((UnityEngine.Object)__instance.FlareParticles != (UnityEngine.Object)null)
            {
                __instance.FlareParticles.emissionRate = (float)(5.0 + (double)Mathf.Clamp01(num2 - 0.2f) * 8.0);
                __instance.FlareParticles.startColor = new Color((35f + 50f * num2) / 255f, 0f, 1f);
                __instance.FlareParticles.startSize = Mathf.Clamp01(num2 - 0.2f) * 14f;
            }
            if ((UnityEngine.Object)__instance.FlareAltParticles != (UnityEngine.Object)null)
            {
                __instance.FlareAltParticles.emissionRate = (float)(5.0 + (double)Mathf.Clamp01(num2 - 0.2f) * 4.0);
                __instance.FlareAltParticles.startColor = new Color(35f / 255f, 0f, 1f);
                __instance.FlareAltParticles.startSize = Mathf.Clamp01(num2 - 0.2f) * 18f;
            }
            if ((UnityEngine.Object)__instance.VerticalFlareParticles != (UnityEngine.Object)null)
            {
                __instance.VerticalFlareParticles.emissionRate = (float)(5.0 + (double)Mathf.Clamp01(num2 - 0.5f) * 12.0);
                __instance.VerticalFlareParticles.startColor = new Color(45f / 255f, 0f, 1f);
                __instance.VerticalFlareParticles.startSize = Mathf.Clamp01(num2 - 0.5f) * 12f;
            }
            if (!((UnityEngine.Object)__instance.CenterParticles != (UnityEngine.Object)null))
                return;
            __instance.CenterParticles.startColor = new Color(45f / 255f, 0f, 1f, num2 + 0.2f);
            __instance.CenterParticles.emissionRate = (float)(50.0 + (double)Mathf.Clamp01(num2 - 0.2f) * 150.0);
            __instance.CenterParticles.startSize = (float)(0.15000000596046448 + (double)num2 * 0.20000000298023224);
        }
    }
}
