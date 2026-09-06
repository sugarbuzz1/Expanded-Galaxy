using HarmonyLib;
using PulsarModLoader.Content.Components.WarpDrive;
using UnityEngine;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLUIScreen), "UpdateGlitch")]
    internal class UpdateGlitchBrokenDriveScreen
    {
        private static bool Prefix(
          PLUIScreen __instance,
          ref float ___LastGlitchTime,
          ref float ___GlitchDelay)
        {
            switch (__instance)
            {
                case PLWarpDriveScreen _:
                label_2:
                    if ((UnityEngine.Object)__instance.MyScreenHubBase == (UnityEngine.Object)null || (UnityEngine.Object)__instance.MyScreenHubBase.OptionalShipInfo == (UnityEngine.Object)null || __instance.MyScreenHubBase.OptionalShipInfo.MyWarpDrive == null || __instance.MyScreenHubBase.OptionalShipInfo.MyWarpDrive.SubType != WarpDriveModManager.Instance.GetWarpDriveIDFromName("Broken Warp Drive"))
                        return true;
                    if (!((UnityEngine.Object)__instance.MyRenderer != (UnityEngine.Object)null) || !((UnityEngine.Object)__instance.MyRenderer.material != (UnityEngine.Object)null) || (double)Time.time - (double)___LastGlitchTime <= (double)___GlitchDelay || !((UnityEngine.Object)__instance.MyScreenHubBase != (UnityEngine.Object)null) || !((UnityEngine.Object)__instance.MyScreenHubBase.OptionalShipInfo != (UnityEngine.Object)null) || __instance.MyScreenHubBase.OptionalShipInfo.MyStats == null || !__instance.MyRenderer.enabled)
                        return false;
                    bool flag = __instance.MyScreenHubBase.OptionalShipInfo.MyWarpDrive.Level != 0;
                    ___GlitchDelay = UnityEngine.Random.Range(0.02f, 0.08f);
                    ___LastGlitchTime = Time.time;
                    Vector3 onUnitSphere = UnityEngine.Random.onUnitSphere;
                    __instance.MyRenderer.material.SetVector("_GlitchVector", new Vector4(onUnitSphere.x * (flag ? 1f : 50f), onUnitSphere.y * (flag ? 1f : 50f), onUnitSphere.z * (flag ? 1f : 50f), 0.0f));
                    __instance.MyRenderer.material.SetFloat("_GlitchPercent", flag ? Mathf.Pow(UnityEngine.Random.Range(0.0f, 1f) * UnityEngine.Random.Range(0.0f, 1f), 10f) : 4f);
                    return false;
                case PLClonedScreen _:
                    if (!(((PLClonedScreen)__instance).MyTargetScreen is PLWarpDriveScreen))
                        break;
                    goto label_2;
            }
            return true;
        }
    }
}