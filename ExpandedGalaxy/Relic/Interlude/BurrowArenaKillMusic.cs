using HarmonyLib;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLBurrowArena), "Update")]
    internal class BurrowArenaKillMusic
    {
        private static void Postfix(PLBurrowArena __instance)
        {
            if (PLNetworkManager.Instance.LocalPlayer != null && PLNetworkManager.Instance.LocalPlayer.GetPawn() != null)
            {
                if (__instance.ArenaIsActive && PLNetworkManager.Instance.LocalPlayer.OnPlanet && PLNetworkManager.Instance.LocalPlayer.GetPawn().SpawnedInArena && PLMusic.Instance.CurrentPlayingMusicEventString != "")
                    PLMusic.Instance.StopCurrentMusic();
            }
        }
    }
}

