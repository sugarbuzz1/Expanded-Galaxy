using HarmonyLib;
using PulsarModLoader;
using System.Collections.Generic;
using static ExpandedGalaxy.Nukes;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLShipInfoBase), "AboutToBeDestroyed")]
    internal class AboutToBeDestroyedyEffect
    {
        private static bool Prefix(PLShipInfoBase __instance)
        {
            if (!PhotonNetwork.isMasterClient || __instance == null)
                return true;
            if (AllStatusDatas.ContainsKey(__instance.SpaceTargetID))
            {
                AllStatusDatas.Remove(__instance.SpaceTargetID);

                List<object> sendData = new List<object>();
                sendData.Add(AllStatusDatas.Count);
                foreach (int key in AllStatusDatas.Keys)
                {
                    ShipStatusEffectData data = AllStatusDatas[key];
                    sendData.Add(key);
                    sendData.Add(data.EffectId);
                    sendData.Add(data.EffectStrength);
                }
                ModMessage.SendRPC("sugarbuzz1.ExpandedGalaxy", "ExpandedGalaxy.UpdateStatusDatas", PhotonTargets.Others, sendData.ToArray());
            }
            return true;
        }
    }
}
