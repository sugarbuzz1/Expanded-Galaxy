using HarmonyLib;
using PulsarModLoader.Content.Components.Turret;
using System.Collections.Generic;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLShipInfoBase), "LeaveExtraScrap")]
    internal class DropMiningDroneLoot
    {
        private static void Postfix(PLShipInfoBase __instance, ref List<PLShipComponent> droppedShipComponents)
        {
            if (__instance.GetIsPlayerShip())
                return;
            if (__instance.ShipTypeID == EShipType.E_WDDRONE1)
            {
                bool flag = false;
                foreach (PLShipComponent component in __instance.MyStats.GetComponentsOfType(ESlotType.E_COMP_REAC_COOLING))
                {
                    if (component is MiningDroneFlag)
                    {
                        flag = true;
                        break;
                    }
                }
                if (!flag)
                    return;
                bool flag1 = false;
                foreach (PLShipComponent component in PLEncounterManager.Instance.PlayerShip.MyStats.GetComponentsOfType(ESlotType.E_COMP_DISTRESS_SIGNAL, true))
                {
                    if (component is MiningDroneSignal)
                    {
                        flag1 = true;
                        break;
                    }
                }
                int compHash = -1;
                if (!flag1 && UnityEngine.Random.Range(0f, 1f) > 0.66f)
                {
                    compHash = (int)PLShipComponent.createHashFromInfo(22, 4, 0, 0, (int)ESlotType.E_COMP_NONE);
                    PLServer.Instance.photonView.RPC("CreateSpecificShipScrapAtLocation", PhotonTargets.All, __instance.Exterior.transform.position + UnityEngine.Random.onUnitSphere * 20f, __instance.Exterior.transform.position, (int)compHash, true);
                }
                else if (UnityEngine.Random.Range(0f, 1f) > 0.9f)
                {
                    compHash = (int)PLShipComponent.createHashFromInfo(10, TurretModManager.Instance.GetTurretIDFromName("Mining Laser"), 0, 0, (int)ESlotType.E_COMP_NONE);
                    PLServer.Instance.photonView.RPC("CreateSpecificShipScrapAtLocation", PhotonTargets.All, __instance.Exterior.transform.position + UnityEngine.Random.onUnitSphere * 20f, __instance.Exterior.transform.position, (int)compHash, true);
                }
            }
        }
    }
}
