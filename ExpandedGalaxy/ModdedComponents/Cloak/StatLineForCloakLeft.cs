using HarmonyLib;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLWare), "GetStatLineLeft")]
    internal class StatLineForCloakLeft
    {
        private static bool Prefix(PLWare __instance, ref string __result)
        {
            PLCloakingSystem cloakingSystem = __instance as PLCloakingSystem;
            if (cloakingSystem != null)
            {
                if (cloakingSystem.SubType == (int)ECloakingSystemType.E_NORMAL)
                {
                    __result = PLLocalize.Localize("EM Signature") + "\n" + PLLocalize.Localize("Turret Damage") + "\n";
                    return false;
                }
                else
                {
                    __result = PLLocalize.Localize("EM Signature") + "\n" + PLLocalize.Localize("Charge Rate") + "\n";
                    return false;
                }
            }
            return true;
        }
    }
}
