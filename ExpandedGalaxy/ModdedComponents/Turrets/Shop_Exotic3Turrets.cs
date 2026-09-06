using HarmonyLib;
using PulsarModLoader.Content.Components.Turret;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLShop_Exotic3), "CreateInitialWares")]
    internal class Shop_Exotic3Turrets
    {
        private static void Postfix(PLShop_Exotic3 __instance, TraderPersistantDataEntry inPDE)
        {
            if (inPDE == null)
                return;
            PLShipComponent componentFromHash = PLShipComponent.CreateShipComponentFromHash((int)PLShipComponent.createHashFromInfo((int)ESlotType.E_COMP_TURRET, (int)TurretModManager.Instance.GetTurretIDFromName("Missile Turret Mk. II"), 0, 0, 12));
            componentFromHash.NetID = inPDE.ServerWareIDCounter;
            inPDE.Wares.Add(inPDE.ServerWareIDCounter, (PLWare)componentFromHash);
            ++inPDE.ServerWareIDCounter;
        }
    }

}
