using HarmonyLib;
using PulsarModLoader;
using UnityEngine;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLCarrierInfo), "SetupShipStats")]
    internal class AddShopComponent
    {
        private static void Postfix(PLCarrierInfo __instance, bool previewStats, bool startingPlayerShip)
        {
            if (__instance.PersistantShipInfo == null || !(__instance.PersistantShipInfo.Type == EShipType.E_CARRIER && __instance.PersistantShipInfo.SelectedActorID == "ExGal_FBCarrier"))
                return;
            if (!startingPlayerShip && !previewStats && __instance.ShipRoot != null)
            {
                if (PhotonNetwork.isMasterClient)
                    ModMessage.SendRPC("sugarbuzz1.ExpandedGalaxy", "ExpandedGalaxy.RecieveShopComponent", PhotonTargets.Others, new object[2] { __instance.ShipID, 1 });
                Shop_FBCarrier shop = __instance.ShipRoot.AddComponent<Shop_FBCarrier>();
                shop.OptionalShip = __instance;
                shop.MySensorObject = __instance.MySensorObjectShip;
                __instance.photonView.ObservedComponents.Add((Component)shop);
            }
        }
    }

}

