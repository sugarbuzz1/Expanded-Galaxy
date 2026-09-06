using HarmonyLib;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLNetworkManager), "JoinRoom")]
    internal class JoinRoomPatch
    {
        private static bool Prefix()
        {
            ResetFlags.OnNewGame();
            return true;
        }
    }
}
