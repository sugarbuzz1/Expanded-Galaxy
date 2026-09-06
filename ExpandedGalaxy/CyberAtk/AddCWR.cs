using HarmonyLib;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLShipInfo), "AddCrewShipCompVariety")]
    internal class AddCWR
    {
        private static bool Prefix(PLShipInfo __instance, int offset)
        {
            PLRand plRand = new PLRand(offset);
            __instance.MyStats.AddShipComponent(new PLCPU(ECPUClass.CYBERWARFARE_MODULE, __instance.GetChaosBoost(plRand.Next() % 50)));
            return true;
        }
    }
}
