using HarmonyLib;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLPersistantEncounterInstance), "PlayMusicBasedOnShipType")]
    internal class RiftMusic
    {
        private static bool Prefix(PLPersistantEncounterInstance __instance, EShipType inType, bool combat)
        {
            if (ReflectedRift.inRift && !combat)
            {
                PLMusic.Instance.PlayMusic("mx_ivm_tooquiet01", false, false, isLoopingTrack: true);
                return false;
            }
            return true;
        }
    }
}
