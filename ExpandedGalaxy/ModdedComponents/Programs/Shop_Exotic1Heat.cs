using HarmonyLib;
using PulsarModLoader.Content.Components.WarpDriveProgram;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLShop_Exotic1), "CreateInitialWares")]
    internal class Shop_Exotic1Heat
    {
        private static void Postfix(PLShop_Exotic1 __instance, TraderPersistantDataEntry inPDE)
        {
            if (inPDE == null)
                return;
            PLShipComponent componentFromHash = PLShipComponent.CreateShipComponentFromHash((int)PLShipComponent.createHashFromInfo((int)ESlotType.E_COMP_PROGRAM, WarpDriveProgramModManager.Instance.GetWarpDriveProgramIDFromName("H.E.A.T."), 0, 0, 12));
            componentFromHash.NetID = inPDE.ServerWareIDCounter;
            inPDE.Wares.Add(inPDE.ServerWareIDCounter, (PLWare)componentFromHash);
            ++inPDE.ServerWareIDCounter;
        }
    }
}

