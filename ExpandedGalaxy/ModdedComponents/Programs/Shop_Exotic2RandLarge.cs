using HarmonyLib;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLShop_Exotic2), "CreateInitialWares")]
    internal class Shop_Exotic2RandLarge
    {
        private static void Postfix(PLShop_Exotic2 __instance, TraderPersistantDataEntry inPDE)
        {
            if (inPDE == null)
                return;
            PLShipComponent componentFromHash = PLShipComponent.CreateShipComponentFromHash((int)PLShipComponent.createHashFromInfo((int)ESlotType.E_COMP_PROGRAM, (int)EWarpDriveProgramType.RAND_LARGE, 0, 0, 12));
            componentFromHash.NetID = inPDE.ServerWareIDCounter;
            inPDE.Wares.Add(inPDE.ServerWareIDCounter, (PLWare)componentFromHash);
            ++inPDE.ServerWareIDCounter;
        }
    }
}

