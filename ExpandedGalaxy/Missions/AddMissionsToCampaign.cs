using HarmonyLib;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLCampaignIO), "ReadCampaign")]
    internal class AddMissionsToCampaign
    {
        private static void Postfix()
        {
            PLCampaignIO.Instance.GetAllMissionData().Add(ReflectedRiftStart.MissionData);
            PLCampaignIO.Instance.GetAllPickupMissionData().Add(ReflectedRiftStart.MissionData);

            PLCampaignIO.Instance.GetAllMissionData().Add(JunkCubeStart.MissionData);
            PLCampaignIO.Instance.GetAllPickupMissionData().Add(JunkCubeStart.MissionData);
            PLCampaignIO.Instance.GetAllMissionData().Add(JunkCubeWait1.MissionData);
            PLCampaignIO.Instance.GetAllPickupMissionData().Add(JunkCubeWait1.MissionData);
            PLCampaignIO.Instance.GetAllMissionData().Add(JunkCubeRetrieval.MissionData);
            PLCampaignIO.Instance.GetAllPickupMissionData().Add(JunkCubeRetrieval.MissionData);
            PLCampaignIO.Instance.GetAllMissionData().Add(JunkCubeWait2.MissionData);
            PLCampaignIO.Instance.GetAllPickupMissionData().Add(JunkCubeWait2.MissionData);
            PLCampaignIO.Instance.GetAllMissionData().Add(CUFriendlyFavorA.MissionData);
            PLCampaignIO.Instance.GetAllPickupMissionData().Add(CUFriendlyFavorA.MissionData);
            PLCampaignIO.Instance.GetAllMissionData().Add(CUFriendlyFavorB.MissionData);
            PLCampaignIO.Instance.GetAllPickupMissionData().Add(CUFriendlyFavorB.MissionData);
            PLCampaignIO.Instance.GetAllMissionData().Add(CUFriendlyFavorAHidden.MissionData);
            PLCampaignIO.Instance.GetAllPickupMissionData().Add(CUFriendlyFavorAHidden.MissionData);
            PLCampaignIO.Instance.GetAllMissionData().Add(WDHuntProjVulcanus.MissionData);
            PLCampaignIO.Instance.GetAllPickupMissionData().Add(WDHuntProjVulcanus.MissionData);
            PLCampaignIO.Instance.GetAllMissionData().Add(TreasureFleet.MissionData);
            PLCampaignIO.Instance.GetAllPickupMissionData().Add(TreasureFleet.MissionData);
            PLCampaignIO.Instance.GetAllMissionData().Add(TreasureFleetRewardA.MissionData);
            PLCampaignIO.Instance.GetAllPickupMissionData().Add(TreasureFleetRewardA.MissionData);
            PLCampaignIO.Instance.GetAllMissionData().Add(TreasureFleetRewardB.MissionData);
            PLCampaignIO.Instance.GetAllPickupMissionData().Add(TreasureFleetRewardB.MissionData);
            PLCampaignIO.Instance.GetAllMissionData().Add(TreasureFleetKilledFriendly.MissionData);
            PLCampaignIO.Instance.GetAllPickupMissionData().Add(TreasureFleetKilledFriendly.MissionData);
            PLCampaignIO.Instance.GetAllMissionData().Add(BadBiscuits.MissionData);
            PLCampaignIO.Instance.GetAllPickupMissionData().Add(BadBiscuits.MissionData);
            PLCampaignIO.Instance.GetAllMissionData().Add(MiningDroneQuestMission.MissionData);
            PLCampaignIO.Instance.GetAllPickupMissionData().Add(MiningDroneQuestMission.MissionData);
            PLCampaignIO.Instance.GetAllMissionData().Add(JunkCubeWait3.MissionData);
            PLCampaignIO.Instance.GetAllPickupMissionData().Add(JunkCubeWait3.MissionData);
            PLCampaignIO.Instance.GetAllMissionData().Add(InterludeStart.MissionData);
            PLCampaignIO.Instance.GetAllPickupMissionData().Add(InterludeStart.MissionData);
            PLCampaignIO.Instance.GetAllMissionData().Add(TheMap.MissionData);
            PLCampaignIO.Instance.GetAllPickupMissionData().Add(TheMap.MissionData);
            PLCampaignIO.Instance.GetAllMissionData().Add(TheMapHidden.MissionData);
            PLCampaignIO.Instance.GetAllPickupMissionData().Add(TheMapHidden.MissionData);
            PLCampaignIO.Instance.GetAllMissionData().Add(RecompilerMaxDifficulty.MissionData);
            PLCampaignIO.Instance.GetAllPickupMissionData().Add(RecompilerMaxDifficulty.MissionData);
            PLCampaignIO.Instance.GetAllMissionData().Add(RecompilerPolytech.MissionData);
            PLCampaignIO.Instance.GetAllPickupMissionData().Add(RecompilerPolytech.MissionData);

        }
    }

}

