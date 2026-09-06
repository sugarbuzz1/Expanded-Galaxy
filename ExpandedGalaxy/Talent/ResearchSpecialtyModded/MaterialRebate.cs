using HarmonyLib;
using Talents.Framework;
using UnityEngine;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLShipInfo), "UpdateResearchTalentChoices")]
    internal class MaterialRebate
    {
        private static bool Prefix(PLShipInfo __instance)
        {
            if (PLServer.Instance != null)
            {
                if (PLServer.Instance.TalentToResearch != ETalents.MAX)
                {
                    if ((int)PLServer.Instance.JumpsNeededToResearchTalent == 0 && PhotonNetwork.isMasterClient)
                    {
                        if (PLServer.Instance.TalentToResearch == ETalents.INC_JETPACK || PLServer.Instance.TalentToResearch == (ETalents)TalentModManager.Instance.GetTalentIDFromName("Jetpack Fuel Reserve"))
                        {
                            PLServer.Instance.SetTalentAsUnlocked((int)ETalents.INC_JETPACK);
                            TalentModManager.Instance.UnlockTalent(TalentModManager.Instance.GetTalentIDFromName("Jetpack Fuel Reserve"));

                        }
                        PLPlayer friendlyPlayerSci = PLServer.Instance.GetCachedFriendlyPlayerOfClass(2);
                        if (friendlyPlayerSci != null)
                        {
                            if ((int)friendlyPlayerSci.Talents[TalentModManager.Instance.GetTalentIDFromName("Research Specialty")] != 0)
                            {
                                TalentInfo info = PLGlobal.GetTalentInfoForTalentType(PLServer.Instance.TalentToResearch);
                                int min = 0;
                                for (int i = 0; i < 6; i++)
                                {
                                    if ((info.ResearchCost[i] > 0 && PLServer.Instance.ResearchMaterials[i] < PLServer.Instance.ResearchMaterials[min]) || info.ResearchCost[min] == 0)
                                        min = i;
                                }
                                if (friendlyPlayerSci.GetPhotonPlayer() != null && !friendlyPlayerSci.IsBot)
                                    PLServer.Instance.photonView.RPC("AddNotification_OneString_LocalizedString", friendlyPlayerSci.GetPhotonPlayer(), (object)"+1 [STR0] to supply (from <Research Specialty> talent)", (object)-1, (object)(PLServer.Instance.GetEstimatedServerMs() + 6000), (object)false, (object)PLPawnItem_ResearchMaterial.GetResearchTypeString(min));
                                PLPlayer friendlyPlayerCap = PLServer.Instance.GetCachedFriendlyPlayerOfClass(0);
                                if (friendlyPlayerCap != null && friendlyPlayerCap.GetPhotonPlayer() != null)
                                    PLServer.Instance.photonView.RPC("AddNotification_OneString_LocalizedString", friendlyPlayerCap.GetPhotonPlayer(), (object)"+1 [STR0] to supply (from <Research Specialty> talent)", (object)-1, (object)(PLServer.Instance.GetEstimatedServerMs() + 6000), (object)false, (object)PLPawnItem_ResearchMaterial.GetResearchTypeString(min));
                                PLServer.Instance.AddToShipLog("CRW", "+1 " + PLPawnItem_ResearchMaterial.GetResearchTypeString(min) + " to supply (from <Research Specialty> talent)", Color.white);
                                ++PLServer.Instance.ResearchMaterials[min];
                            }
                        }
                    }
                }
            }
            return true;
        }
    }
}
