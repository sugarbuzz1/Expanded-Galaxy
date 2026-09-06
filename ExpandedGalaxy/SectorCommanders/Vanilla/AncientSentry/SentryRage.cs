using HarmonyLib;
using System;
using UnityEngine;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLCorruptedDroneShipInfo), "Update")]
    internal class SentryRage
    {
        private static void Postfix(PLCorruptedDroneShipInfo __instance, ref Material ___CorruptedGreenMaterial, ref float ___Server_LastMissleFireTime, ref float ___Server_LastEMPBlastTime)
        {
            if (__instance.AlertLevel > 0)
            {
                if (Time.time - ___Server_LastEMPBlastTime > 30f && PhotonNetwork.isMasterClient)
                {
                    __instance.photonView.RPC("EMPBlast", PhotonTargets.All);
                    ___Server_LastEMPBlastTime = Time.time;
                }
                if (__instance.MyStats.HullCurrent / __instance.MyStats.HullMax < 0.3f)
                {

                    foreach (Light light in __instance.GreenLights)
                    {
                        light.color = Color.red;
                        light.intensity = (float)Math.Sin((double)Time.time) * 25f;
                    }
                    if (PhotonNetwork.isMasterClient)
                    {
                        if ((double)Time.time - (double)___Server_LastMissleFireTime > 30.0)
                            ___Server_LastMissleFireTime -= 30f;
                    }
                }
            }
        }
    }
}
