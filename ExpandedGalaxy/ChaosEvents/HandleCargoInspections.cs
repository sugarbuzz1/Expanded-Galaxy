using HarmonyLib;
using System.Collections.Generic;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLServer), "AttemptToAddPickupMission")]
    internal class HandleCargoInspections
    {
        private static bool Prefix(PLServer __instance)
        {
            if (!__instance.IsChaosEventActive(EChaosEvent.E_CALM) || __instance.HasActiveMissionWithID(8000013) || PLEncounterManager.Instance == null || PLEncounterManager.Instance.PlayerShip == null)
                return true;
            PickupMissionData pickupMission = (PickupMissionData)PLCampaignIO.Instance.GetMissionOfTypeID(8000013);
            if (pickupMission == null)
                return true;
            int num4 = 0;
            float num5 = 5f * PLServer.CampaignEditorDistanceToGalaxyDistance;
            float num6 = 50f;
            int num7 = 0;
            int num8 = 0;
            PLSectorInfo plSectorInfo1 = PLServer.GetCurrentSector();
            if ((UnityEngine.Object)PLEncounterManager.Instance.PlayerShip != (UnityEngine.Object)null && PLEncounterManager.Instance.PlayerShip.InWarp && PLEncounterManager.Instance.PlayerShip.WarpTargetID != -1 && PLGlobal.Instance.Galaxy.AllSectorInfos.ContainsKey(PLEncounterManager.Instance.PlayerShip.WarpTargetID))
                plSectorInfo1 = PLGlobal.Instance.Galaxy.AllSectorInfos[PLEncounterManager.Instance.PlayerShip.WarpTargetID];
            if (plSectorInfo1 != null)
            {
                IEnumerator<PLSectorInfo> enumerator = (IEnumerator<PLSectorInfo>)PLGlobal.Instance.Galaxy.AllSectorInfos.Values.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    if (enumerator.Current != null && (double)(plSectorInfo1.Position - enumerator.Current.Position).sqrMagnitude < (double)num5 * (double)num5)
                    {
                        if (enumerator.Current.MySPI.Faction == num4)
                            ++num8;
                        ++num7;
                    }
                }
            }
            if (num7 > 0)
            {
                float num9 = (float)num8 / (float)num7;
                if ((UnityEngine.Object)PLEncounterManager.Instance.PlayerShip == (UnityEngine.Object)null || (double)num9 < (double)num6 * 0.01)
                {
                    return true;
                }
            }
            else
                return true;
            bool flag = false;
            foreach (PLShipComponent component in PLEncounterManager.Instance.PlayerShip.MyStats.AllComponents)
            {
                if (component.Contraband)
                {
                    flag = true;
                    break;
                }
            }
            if (!flag)
            {
                foreach (PLServerClassInfo classInfo in PLServer.Instance.ClassInfos)
                {
                    if (classInfo.ClassLockerInventory == null)
                        continue;
                    foreach (PLPawnItem item in classInfo.ClassLockerInventory.AllItems)
                    {
                        if (item.Contraband)
                        {
                            flag = true;
                            break;
                        }
                    }
                    if (flag)
                        break;
                }
            }
            if (!flag)
            {
                foreach (PLPlayer player in PLServer.Instance.AllPlayers)
                {
                    if (player == null || player.TeamID != 0 || player.MyInventory == null)
                        continue;
                    foreach (PLPawnItem item in player.MyInventory.AllItems)
                    {
                        if (item.Contraband)
                        {
                            flag = true;
                            break;
                        }
                    }
                    if (flag)
                        break;
                }
            }
            bool flag2 = false;
            if (flag)
            {
                if (UnityEngine.Random.Range(0, 1000) > 600)
                {
                    __instance.StartCoroutine(__instance.SafeStartPickupMission(pickupMission));
                    flag2 = true;
                }
            }
            else
            {
                if (UnityEngine.Random.Range(0, 1000) > 900)
                {
                    __instance.StartCoroutine(__instance.SafeStartPickupMission(pickupMission));
                    flag2 = true;
                }
            }
            return !flag2;
        }
    }
}
