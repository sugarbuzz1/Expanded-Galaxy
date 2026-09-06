using HarmonyLib;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLWare), "GetStatLineRight")]
    internal class StatLineForCloakRight
    {
        private static bool Prefix(PLWare __instance, ref string __result)
        {
            PLCloakingSystem cloakingSystem = __instance as PLCloakingSystem;
            if (cloakingSystem != null)
            {
                if (cloakingSystem.SubType == (int)ECloakingSystemType.E_NORMAL)
                {
                    __result = "-" + (3f * cloakingSystem.GetPowerPercentInput()).ToString("0.0") + "/" + (3f).ToString("0") + "\n" + "+" + Cloak.SubTypeDataParse(cloakingSystem.SubTypeData).ToString("0.0") + "/" + (20f + 2.5f * cloakingSystem.Level).ToString("0.0") + "%" + "\n";
                    return false;
                }
                else
                {
                    __result = "-" + (4f * cloakingSystem.GetPowerPercentInput()).ToString("0.0") + "/" + (4f).ToString("0") + "\n" + "+" + Cloak.SubTypeDataParse(cloakingSystem.SubTypeData).ToString("0.0") + "/" + (20f + 2.5f * cloakingSystem.Level).ToString("0.0") + "%" + "\n";
                    return false;
                }
            }
            return true;
        }
    }
}
