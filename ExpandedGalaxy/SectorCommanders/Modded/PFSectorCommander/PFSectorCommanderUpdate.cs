using HarmonyLib;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLPersistantEncounterInstance), "Update")]
    internal class PFSectorCommanderUpdate
    {
        internal static bool bossMusic;
        internal static bool ambientMusic;

        private static void Postfix(PLPersistantEncounterInstance __instance)
        {
            if (!(PLEncounterManager.Instance.GetCurrentPersistantEncounterInstance() != null) || PLServer.GetSectorWithID(__instance.GetSectorID()) == null || PLEncounterManager.Instance.PlayerShip == null)
                return;
            PLSectorInfo sectorInfo = PLServer.GetSectorWithID(__instance.GetSectorID());
            if (!(sectorInfo.VisualIndication == ESectorVisualIndication.ALCHEMIST && sectorInfo.MySPI.Faction == 5))
                return;


            bool playerInEncounter = PLEncounterManager.Instance.GetCurrentPersistantEncounterInstance().GetSectorID() == __instance.GetSectorID();
            if ((bool)playerInEncounter && !PLEncounterManager.Instance.PlayerShip.Get_IsInWarpMode())
            {
                if (PFSectorCommander.bossFlag == 6 && bossMusic)
                {
                    bossMusic = false;
                    PLMusic.Instance.StopCurrentMusic();
                }
                if (!bossMusic && PFSectorCommander.bossFlag != 6)
                {
                    bossMusic = true;
                    PLMusic.Instance.PlayMusic("mx_Polytechnic_Attack", true, false, true, true);
                    return;
                }
                if (PFSectorCommander.bossFlag == 6 && !ambientMusic)
                {
                    ambientMusic = true;
                    PLMusic.Instance.PlayMusic("mx_Polytechnic_Ambient", true, false, true, true);
                    return;
                }
            }
        }
    }
}
