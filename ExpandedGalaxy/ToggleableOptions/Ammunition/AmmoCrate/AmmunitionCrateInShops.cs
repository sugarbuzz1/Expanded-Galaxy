using HarmonyLib;
using PulsarModLoader.Content.Components.MissionShipComponent;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLShop), "Update")]
    internal class AmmunitionCrateInShops
    {
        private static void Postfix(PLShop __instance)
        {
            if (!Ammunition.DynamicAmmunition || !PhotonNetwork.isMasterClient)
                return;
            if (__instance.SpaceTrader && __instance.MyPDE != null && __instance is PLShop_General)
            {
                bool flag = false;
                foreach (PLWare ware in __instance.MyPDE.Wares.Values)
                {
                    if (ware is PLShipComponent)
                    {
                        if (ware is PLMissionShipComponent && (ware as PLMissionShipComponent).SubType == MissionShipComponentModManager.Instance.GetMissionShipComponentIDFromName("Ammunition Cache"))
                        {
                            flag = true;
                            break;
                        }
                    }
                }
                if (!flag)
                {
                    PLMissionShipComponent component = MissionShipComponentModManager.CreateMissionShipComponent(MissionShipComponentModManager.Instance.GetMissionShipComponentIDFromName("Ammunition Cache"), UnityEngine.Random.Range(0, 3));
                    __instance.MyPDE.ServerAddWare(component);
                }
            }
        }
    }
}
