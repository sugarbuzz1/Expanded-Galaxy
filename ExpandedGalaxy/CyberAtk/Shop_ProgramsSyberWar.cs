using HarmonyLib;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLShop_Programs), "CreateInitialWares")]
    internal class Shop_ProgramsSyberWar
    {
        private static void Postfix(PLShop_Programs __instance, TraderPersistantDataEntry inPDE)
        {
            if (inPDE == null)
                return;
            inPDE.ServerAddWare((PLWare)new PLWarpDriveProgram(EWarpDriveProgramType.VIRUS_BOOSTER));
            inPDE.ServerAddWare((PLWare)new PLWarpDriveProgram(EWarpDriveProgramType.VIRUS_BOOSTER));
        }
    }
}
