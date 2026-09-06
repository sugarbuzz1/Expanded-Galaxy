using HarmonyLib;
using System;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLLightningTurret), MethodType.Constructor, new Type[2] { typeof(int), typeof(int) })]
    internal class LightningDamageNerf
    {
        private static void Postfix(PLFocusLaserTurret __instance)
        {
            __instance.m_Damage = 30f;
        }
    }
}
