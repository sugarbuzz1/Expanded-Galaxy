using HarmonyLib;
using PulsarModLoader;
using UnityEngine;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLRolandInfo), "SetupShipStats")]
    internal class AddCaravanShopComponent
    {
        private static void Postfix(PLRolandInfo __instance, bool previewStats, bool startingPlayerShip)
        {
            if (__instance.PersistantShipInfo == null || !(__instance.PersistantShipInfo.ShipName == "Wandering Caravan" && __instance.PersistantShipInfo.Type == EShipType.E_ROLAND && __instance.PersistantShipInfo.SelectedActorID == "ExGal_RelicCaravan"))
                return;
            if (!startingPlayerShip && !previewStats && __instance.ShipRoot != null)
            {
                if (PhotonNetwork.isMasterClient)
                    ModMessage.SendRPC("sugarbuzz1.ExpandedGalaxy", "ExpandedGalaxy.RecieveShopComponent", PhotonTargets.Others, new object[2] { __instance.ShipID, 0 });
                Shop_Caravan shop = __instance.ShipRoot.AddComponent<Shop_Caravan>();
                shop.OptionalShip = __instance;
                shop.MySensorObject = __instance.MySensorObjectShip;
                __instance.photonView.ObservedComponents.Add((Component)shop);
            }
        }
    }
}
