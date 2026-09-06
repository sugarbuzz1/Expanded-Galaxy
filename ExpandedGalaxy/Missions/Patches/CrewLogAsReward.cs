using HarmonyLib;
using PulsarModLoader;
using System.Collections.Generic;
using UnityEngine;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLPickupMissionBase), "ProcessReward")]
    internal class CrewLogAsReward
    {
        private static void Postfix(RewardData inRwd, string humanNameForAction)
        {
            if (!PhotonNetwork.isMasterClient)
                return;
            if (inRwd.RwdType == 9)
            {
                switch (inRwd.RewardDataA)
                {
                    case 3:
                        int sector1 = -1;
                        int sector2 = -1;
                        if (PLGlobal.Instance.Galaxy != null)
                        {
                            foreach (PLSectorInfo info in PLGlobal.Instance.Galaxy.AllSectorInfos.Values)
                            {
                                if (info != null && info.MySPI != null)
                                {
                                    if (info.MySPI.Faction == 6 && info.VisualIndication == ESectorVisualIndication.WARP_NETWORK_STATION && info.Position.x > -10f && info.Position.y > -10f)
                                        sector1 = info.ID;
                                    else if (info.VisualIndication == ESectorVisualIndication.DESERT_HUB)
                                        sector2 = info.ID;
                                }
                                if (sector1 != -1 && sector2 != -1)
                                    break;
                            }
                        }
                        CrewLogData data = new CrewLogData
                        {
                            optionalSectorID = -1,
                            timeStamp = (float)PLServer.Instance.Playtime,
                            optionalColor = new Color(UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f)),
                            Text = sector1.ToString() + ',' + sector2.ToString(),
                            specialData = 3
                        };
                        CrewLogManager.Instance.AddLog(data);
                        List<object> sendArgumentList = new List<object>();
                        int logCount = CrewLogManager.Instance.GetLogs().Count;
                        sendArgumentList.Add(logCount);
                        foreach (CrewLogData sendLogData in CrewLogManager.Instance.GetLogs())
                        {
                            sendArgumentList.Add(sendLogData.Text);
                            sendArgumentList.Add(sendLogData.timeStamp);
                            sendArgumentList.Add(sendLogData.optionalSectorID);
                            sendArgumentList.Add(sendLogData.optionalColor.r);
                            sendArgumentList.Add(sendLogData.optionalColor.g);
                            sendArgumentList.Add(sendLogData.optionalColor.b);
                            sendArgumentList.Add(sendLogData.optionalColor.a);
                            sendArgumentList.Add(sendLogData.specialData);
                        }
                        ModMessage.SendRPC("sugarbuzz1.ExpandedGalaxy", "ExpandedGalaxy.ServerSendLog", PhotonTargets.Others, sendArgumentList.ToArray());
                        PLServer.Instance.photonView.RPC("AddCrewWarning", PhotonTargets.All, new object[4]
                        {
                            "Crew Log Added!",
                            Color.white,
                            0,
                            "LOG"
                        });
                        break;
                }
            }
            else if (inRwd.RwdType == 10 && PhotonNetwork.isMasterClient)
                ModMessage.SendRPC("sugarbuzz1.ExpandedGalaxy", "ExpandedGalaxy.ClientUnlockAchievement", PhotonTargets.All, new object[1] { inRwd.RewardDataA });
        }
    }

}

