using HarmonyLib;
using System;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLFocusLaserTurret), MethodType.Constructor, new Type[2] { typeof(int), typeof(int) })]
    internal class FocusLaserDamageNerf
    {
        private static void Postfix(PLFocusLaserTurret __instance)
        {
            __instance.m_Damage = 80f;
        }
    }
}
