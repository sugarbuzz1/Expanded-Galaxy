using HarmonyLib;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLCorruptedDroneShipInfo), "SetupShipStats")]
    internal class MoreSentryComps
    {
        private static void Postfix(PLCorruptedDroneShipInfo __instance)
        {
            if (PhotonNetwork.isMasterClient)
            {
                PLShipStats stats = __instance.MyStats;
                for (int i = 0; i < 5; i++)
                {
                    stats.AddShipComponent(PLShipComponent.CreateShipComponentFromHash((int)PLShipComponent.createHashFromInfo((int)ESlotType.E_COMP_CPU, (int)ECPUClass.E_CPUTYPE_TURRETCHARGE_BACKUP, 5, 0, (int)ESlotType.E_COMP_CPU)));
                }
            }
        }
    }
}
