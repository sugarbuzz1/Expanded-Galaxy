
using CodeStage.AntiCheat.ObscuredTypes;
using HarmonyLib;
using PulsarModLoader.Content.Components.NuclearDevice;
using System;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLNuclearDevice), MethodType.Constructor, new Type[2] { typeof(ENuclearDeviceType), typeof(int) })]
    internal class NuclearDevicePatch
    {
        private static void Postfix(PLNuclearDevice __instance, ENuclearDeviceType inType, int inLevel, ref ObscuredInt ___m_MarketPrice)
        {
            if (__instance.SubType < NuclearDeviceModManager.Instance.VanillaNuclearDeviceMaxType && __instance.SubType != 5 && __instance.SubType != 3 && !(__instance is PLBiscuitBombComponent))
            {
                __instance.MaxDamage /= 2f;
                ___m_MarketPrice = (ObscuredInt)((int)___m_MarketPrice / 2);
                switch (__instance.SubType)
                {
                    case 1:
                        __instance.Range = 4500f;
                        break;
                    case 2:
                        __instance.Range = 2500f;
                        break;
                    case 4:
                        __instance.Range = 3500f;
                        break;
                    case 6:
                        __instance.Range = 2000f;
                        break;
                    default:
                        __instance.Range = 3000f;
                        break;
                }
            }
        }
    }
}
