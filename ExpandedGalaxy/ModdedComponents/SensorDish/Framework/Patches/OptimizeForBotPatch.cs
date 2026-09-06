using HarmonyLib;
using PulsarModLoader;
using UnityEngine;

namespace ExpandedGalaxy.SensorDish
{
    [HarmonyPatch(typeof(PLScientistSensorScreen), "OptimizeForBot")]
    internal class OptimizeForBotPatch
    {
        private static bool Prefix(PLScientistSensorScreen __instance, PLBot inBot)
        {
            if (!(inBot != null) || !(inBot.PlayerOwner != null) || !(inBot.PlayerOwner.GetPawn() != null) || !(inBot.PlayerOwner.GetPawn().CurrentShip != null) || !(__instance.MyScreenHubBase.OptionalShipInfo != null))
                return false;
            if (__instance.MyScreenHubBase.OptionalShipInfo.MySensorDish != null)
                inBot.LastInRangeForSensorDishTime = Time.time;
            if ((double)Time.time - (double)__instance.LastTimeBot_Activated_SensorSweep > 8.0 && (double)Time.time - (double)__instance.LastTimeBot_TriedToActivate_SensorSweep > 1.0)
            {
                __instance.LastTimeBot_TriedToActivate_SensorSweep = Time.time;
                int num = inBot.PlayerOwner.GetPriorityLevel(18);
                if (inBot.PlayerOwner.ActiveSubPriority != null && inBot.PlayerOwner.ActiveSubPriority.PriID == 62)
                    num = 5;
                if (UnityEngine.Random.Range(0, 100) < num * num)
                {
                    __instance.LastTimeBot_Activated_SensorSweep = Time.time;
                    PLServer.Instance.photonView.RPC("StartActiveScan", PhotonTargets.All, __instance.MyScreenHubBase.OptionalShipInfo.ShipID, 0);
                }
            }
            int subType = 0;
            if (__instance.MyScreenHubBase.OptionalShipInfo.MySensorDish != null)
                subType = __instance.MyScreenHubBase.OptionalShipInfo.MySensorDish.SubType;
            if (!(__instance.MyScreenHubBase.OptionalShipInfo.TargetShip != null) || (double)__instance.MyScreenHubBase.OptionalShipInfo.TargetShip.MySensorObjectShip.GetDetectionSignal(Vector3.SqrMagnitude(__instance.MyScreenHubBase.OptionalShipInfo.TargetShip.Exterior.transform.position - __instance.MyScreenHubBase.OptionalShipInfo.Exterior.transform.position), __instance.MyScreenHubBase.OptionalShipInfo.TargetShip.MyStats.EMSignature, __instance.MyScreenHubBase.OptionalShipInfo.MyStats.EMDetection, (PLShipInfoBase)__instance.MyScreenHubBase.OptionalShipInfo, (PLSensorObject)__instance.MyScreenHubBase.OptionalShipInfo.TargetShip.MySensorObjectShip) < 18.0 || !(__instance.MyScreenHubBase.OptionalShipInfo.TargetShip != __instance.MyScreenHubBase.OptionalShipInfo) || ((double)Time.time - (double)__instance.MyScreenHubBase.OptionalShipInfo.TargetShip.LastRecievedWeaknessTime <= 32.0 || (Mathf.RoundToInt((float)((double)Time.time - (double)__instance.MyScreenHubBase.OptionalShipInfo.TargetShip.LastRecievedWeaknessTime)) % 4 == 0 && DoesActiveWeaknessStillMakeSense(__instance.MyScreenHubBase.OptionalShipInfo, __instance.MyScreenHubBase.OptionalShipInfo.TargetShip))))
                return false;
            int abilityToActivate = SensorDishModManager.Instance.SensorDishTypes[subType].AbilityLogicForBots(inBot, __instance.MyScreenHubBase.OptionalShipInfo);
            if (abilityToActivate != -1)
            {
                int weaknessID = 0;
                int data = (__instance.MyScreenHubBase.OptionalShipInfo.ShipID << 16);
                switch (abilityToActivate)
                {
                    case 0:
                        weaknessID = 0;
                        break;
                    case 1:
                        weaknessID = 0;
                        data |= 1;
                        break;
                    case 2:
                        weaknessID = 0;
                        data |= 3;
                        break;
                    case 3:
                        weaknessID = 3;
                        data |= subType;
                        break;
                    case 4:
                        weaknessID = 2;
                        data |= subType;
                        break;
                    case 5:
                        weaknessID = 5;
                        data |= subType;
                        break;

                }
                __instance.TargetShip.photonView.RPC("AddSensorWeakness", PhotonTargets.All, weaknessID, PLServer.Instance.GetEstimatedServerMs(), data);
            }
            if (subType == SensorDishModManager.Instance.GetSensorDishIDFromName("Ancient Sensor Dish"))
            {
                AncientSensorDishMod.lastToInteract = inBot.PlayerOwner;
                ModMessage.SendRPC("sugarbuzz1.ExpandedGalaxy", "ExpandedGalaxy.CachePlayerRPC", PhotonTargets.Others, new object[1]
                {
                     inBot.PlayerOwner.GetPlayerID(),
                });
            }
            return false;
        }

        private static bool DoesActiveWeaknessStillMakeSense(PLShipInfoBase inHomeShipInfo, PLShipInfoBase inTargetShipInfo)
        {
            if (inTargetShipInfo.IsSensorWeaknessActive(ESensorWeakness.CYBERDEF_WEAKNESS))
                return SensorDishModManager.Instance.SensorDishTypes[inTargetShipInfo.SensorWeaknessData[3] & 65535].SensorDishAbilities[0].DoesWeaknessStillMakeSense(inHomeShipInfo, inTargetShipInfo);
            else if (inTargetShipInfo.IsSensorWeaknessActive(ESensorWeakness.SHLD_WEAKPOINT))
                return SensorDishModManager.Instance.SensorDishTypes[inTargetShipInfo.SensorWeaknessData[2] & 65535].SensorDishAbilities[1].DoesWeaknessStillMakeSense(inHomeShipInfo, inTargetShipInfo);
            else if (inTargetShipInfo.IsSensorWeaknessActive(ESensorWeakness.REACTOR_WEAKNESS))
                return SensorDishModManager.Instance.SensorDishTypes[inTargetShipInfo.SensorWeaknessData[5] & 65535].SensorDishAbilities[2].DoesWeaknessStillMakeSense(inHomeShipInfo, inTargetShipInfo);
            return false;
        }
    }
}
