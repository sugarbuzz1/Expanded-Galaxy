using HarmonyLib;
using PulsarModLoader;
using UnityEngine;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLServer), "Update")]
    internal class EnsureReflectedRift
    {
        private static float lastCheckTime = float.MinValue;
        private static void Postfix(PLServer __instance)
        {
            if (PhotonNetwork.isMasterClient && ReflectedRift.inRift)
            {
                if (PLServer.Instance != null && ReflectedRift.inRift && PLEncounterManager.Instance != null && PLEncounterManager.Instance.PlayerShip != null && !PLEncounterManager.Instance.PlayerShip.InWarp && PLServer.GetCurrentSector() != null && PLServer.GetCurrentSector().VisualIndication != ESectorVisualIndication.DIMENSION_STATION && PLEncounterManager.Instance != null && PLEncounterManager.Instance.GetCPEI() != null && (double)PLEncounterManager.Instance.GetCPEI().GetTimePlayerInEncounterAfterWarp() > 10.0 && Time.time - lastCheckTime > 5f)
                {
                    lastCheckTime = Time.time;
                    ModMessage.SendRPC("sugarbuzz1.ExpandedGalaxy", "ExpandedGalaxy.ServerSendReflectionCheck", PhotonTargets.All, new object[1]
                    {
                            PLServer.Instance.IsReflection.GetDecrypted()
                    });
                }
            }
            else
            {
                lastCheckTime = float.MinValue;
            }
        }
    }
}
