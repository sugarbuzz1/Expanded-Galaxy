using HarmonyLib;
using UnityEngine;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLShipStats), "CalculateStats")]
    internal class EMSignatureDecreaser
    {
        private static void Postfix(PLShipStats __instance)
        {
            PLCloakingSystem cloakingSystem = __instance.Ship.MyCloakingSystem;
            if (cloakingSystem != null && __instance.Ship.GetIsCloakingSystemActive())
            {
                float EMReduction = 0f;
                switch (cloakingSystem.SubType)
                {
                    case (int)ECloakingSystemType.E_SYVASSI:
                        EMReduction = 4f * cloakingSystem.GetPowerPercentInput(); break;
                    default:
                        EMReduction = 3f * cloakingSystem.GetPowerPercentInput(); break;
                }
                __instance.EMSignature = Mathf.Clamp(__instance.EMSignature - EMReduction, 0f, __instance.EMSignature);
                __instance.LFSignature = 0f;
            }
        }
    }
}
