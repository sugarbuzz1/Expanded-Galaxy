using HarmonyLib;
using System;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLCPU), MethodType.Constructor, new Type[2] { typeof(ECPUClass), typeof(int) })]
    internal class CPUConstructorPatch
    {
        private static void Postfix(PLCPU __instance, ECPUClass inClass, int inLevel, ref float ___m_Speed)
        {
            if (inClass == ECPUClass.E_CPUTYPE_SHIELD_COPROCESSOR)
                ___m_Speed = 10f;

            switch (inClass)
            {
                case ECPUClass.IMPROVED_DEFENSES:
                    __instance.SysInstConduit = 2;
                    break;
                case ECPUClass.CYBERWARFARE_MODULE:
                    __instance.SysInstConduit = 3;
                    break;
                case ECPUClass.E_CPUTYPE_JUMP_PROCESSOR:
                case ECPUClass.COMBO:
                    __instance.SysInstConduit = 4;
                    break;
            }
        }
    }
}
