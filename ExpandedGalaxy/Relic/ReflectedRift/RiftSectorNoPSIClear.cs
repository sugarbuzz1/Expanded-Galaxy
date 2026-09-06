using CodeStage.AntiCheat.ObscuredTypes;
using HarmonyLib;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLPersistantEncounterInstance), "PlayerEnter")]
    internal class RiftSectorNoPSIClear
    {
        private static bool Prefix(PLPersistantEncounterInstance __instance, int inHubID, ref ObscuredBool ___PlayerInEncounter)
        {
            if (ReflectedRift.inRift)
            {
                PLServer.Instance.LongRangeCommsDisabled = (ObscuredBool)true;
                ___PlayerInEncounter = (ObscuredBool)true;
                return false;
            }
            return true;
        }
    }
}
