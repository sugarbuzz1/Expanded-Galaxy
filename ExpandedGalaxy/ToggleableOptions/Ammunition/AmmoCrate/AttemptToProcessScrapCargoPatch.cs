using HarmonyLib;
using PulsarModLoader;
using PulsarModLoader.Content.Components.MissionShipComponent;
using System.Collections.Generic;
using UnityEngine;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLPlayer), "AttemptToProcessScrapCargo")]
    internal class AttemptToProcessScrapCargoPatch
    {
        private static bool Prefix(PLPlayer __instance, int inCurrentShipID, int inNetID)
        {
            if (!(PLEncounterManager.Instance != null))
                return false;
            PLShipInfo shipFromId = PLEncounterManager.Instance.GetShipFromID(inCurrentShipID) as PLShipInfo;
            if (!(shipFromId != null))
                return false;
            PLShipComponent component = shipFromId.MyStats.GetComponentFromNetID(inNetID);
            if (component == null)
                return false;
            else if (component is PLMissionShipComponent && component.SubType == MissionShipComponentModManager.Instance.GetMissionShipComponentIDFromName("Ammunition Cache"))
            {
                shipFromId.MyStats.RemoveShipComponentByNetID(inNetID);
                for (int i = 0; i < shipFromId.MyAmmoRefills.Length; i++)
                {
                    shipFromId.MyAmmoRefills[i].SupplyAmount = 1f;
                    shipFromId.MyAmmoRefills[i].ClipAvailable = true;
                }
                PLPlayer friendlyPlayerOfClass = PLServer.Instance.GetCachedFriendlyPlayerOfClass(0);
                if ((int)__instance.TeamID != 0 || !(friendlyPlayerOfClass != null) || !(__instance != friendlyPlayerOfClass))
                    return false;
                PLServer.Instance.photonView.RPC("AddNotificationLocalize", friendlyPlayerOfClass.GetPhotonPlayer(), (object)"[PL] has restocked ammunition", (object)__instance.GetPlayerID(), (object)(PLServer.Instance.GetEstimatedServerMs() + 6000), true);
                return false;
            }
            else if (component is PLMissionShipComponent && component.SubType == MissionShipComponentModManager.Instance.GetMissionShipComponentIDFromName("Reward"))
            {
                shipFromId.MyStats.RemoveShipComponentByNetID(inNetID);
                if (PhotonNetwork.isMasterClient)
                    shipFromId.MyStats.AddShipComponent(Relic.GenerateRelic(PLGlobal.Instance.Galaxy.Seed), visualSlot: ESlotType.E_COMP_CARGO);
                return false;
            }
            else if (component is PLMissionShipComponent && component.SubType == MissionShipComponentModManager.Instance.GetMissionShipComponentIDFromName("Data Cache"))
            {
                shipFromId.MyStats.RemoveShipComponentByNetID(inNetID);
                if (PhotonNetwork.isMasterClient)
                {
                    PLRand rand = new PLRand((int)PLServer.Instance.GalaxySeed);
                    int a = 0;
                    for (int i = 0; i < 200; i++)
                    {
                        a = 1 + rand.Next(5);
                        if (!ReflectedRift.GetRiftData(a))
                            break;
                    }
                    ReflectedRift.SetRiftData(a, true);
                    if ((ReflectedRift.GetRiftData(1) ? 1 : 0) + (ReflectedRift.GetRiftData(2) ? 1 : 0) + (ReflectedRift.GetRiftData(3) ? 1 : 0) + (ReflectedRift.GetRiftData(4) ? 1 : 0) + (ReflectedRift.GetRiftData(5) ? 1 : 0) == 1)
                    {
                        CrewLogData data = new CrewLogData
                        {
                            optionalSectorID = -1,
                            timeStamp = (float)PLServer.Instance.Playtime,
                            optionalColor = new Color(UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f)),
                            Text = string.Empty,
                            specialData = 2
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
                        return false;
                    }
                    PLServer.Instance.photonView.RPC("AddCrewWarning", PhotonTargets.All, new object[4]
                        {
                            "Crew Log Updated!",
                            Color.white,
                            0,
                            "LOG"
                        });
                }
                return false;
            }
            else if (component is PLWarpDriveProgram)
            {
                PLServer.Instance.CurrentUpgradeMats++;
                shipFromId.MyStats.RemoveShipComponentByNetID(inNetID);
                return false;

            }
            else if (component.ActualSlotType != ESlotType.E_COMP_SCRAP)
            {
                PLServer.Instance.CurrentCrewCredits -= (component.Level + 1) * 800;
                if (PLServer.Instance.CurrentCrewCredits < 0)
                    PLServer.Instance.CurrentCrewCredits = 0;
            }
            return true;
        }
    }
}
