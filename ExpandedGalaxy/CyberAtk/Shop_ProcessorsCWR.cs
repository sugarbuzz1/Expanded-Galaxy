using HarmonyLib;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLShop_Processors), "CreateInitialWares")]
    internal class Shop_ProcessorsCWR
    {
        private static void Postfix(PLShop_Processors __instance, TraderPersistantDataEntry inPDE)
        {
            if (inPDE == null)
                return;
            inPDE.ServerAddWare((PLWare)new PLCPU(ECPUClass.CYBERWARFARE_MODULE, 0));
            inPDE.ServerAddWare((PLWare)new PLCPU(ECPUClass.CYBERWARFARE_MODULE, 0));
        }
    }
}
