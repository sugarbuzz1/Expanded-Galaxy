using HarmonyLib;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLSpecialEncounterNetObject), "InitNewObject")]
    internal class InitExtraScrapData
    {
        private static void Postfix(PLSpecialEncounterNetObject inObject, PLPersistantEncounterInstance inPersistantEncounterInstance)
        {
            if (!PhotonNetwork.isMasterClient)
                return;
            if (inObject is PLSpaceScrap)
            {
                PLSpaceScrap scrap = (PLSpaceScrap)inObject;
                PersistantScrapManager.Instance.AddScrap(scrap, PLServer.GetCurrentSector().ID);
            }
        }
    }
}
