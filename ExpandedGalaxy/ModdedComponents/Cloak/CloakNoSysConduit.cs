using HarmonyLib;
using System;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLCloakingSystem), MethodType.Constructor, new Type[2] { typeof(ECloakingSystemType), typeof(int) })]
    internal class CloakNoSysConduit
    {
        private static void Postfix(PLCloakingSystem __instance)
        {
            __instance.SysInstConduit = -1;
        }
    }
}
