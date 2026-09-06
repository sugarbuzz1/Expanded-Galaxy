using CodeStage.AntiCheat.ObscuredTypes;
using HarmonyLib;
using PulsarModLoader;
using Talents.Framework;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLShipInfoBase), "Update")]
    internal class ShipUpdatePatch
    {
        private static void Postfix(PLShipInfoBase __instance)
        {
            if (__instance == null || !__instance.GetIsPlayerShip())
                return;
            if (Jetpack.AdvancedJetPack)
            {
                TalentModManager.Instance.HideTalent((int)ETalents.INC_JETPACK);
                TalentModManager.Instance.UnHideTalent((int)TalentModManager.Instance.GetTalentIDFromName("Jetpack Fuel Reserve"));
            }
            else
            {
                TalentModManager.Instance.UnHideTalent((int)ETalents.INC_JETPACK);
                TalentModManager.Instance.HideTalent((int)TalentModManager.Instance.GetTalentIDFromName("Jetpack Fuel Reserve"));
            }
            if (Ammunition.DynamicAmmunition)
            {
                TalentModManager.Instance.HideTalent((int)ETalents.WPN_AMMO_BOOST);
                TalentModManager.Instance.UnHideTalent((int)TalentModManager.Instance.GetTalentIDFromName("Reloader"));
            }
            else
            {
                TalentModManager.Instance.UnHideTalent((int)ETalents.WPN_AMMO_BOOST);
                TalentModManager.Instance.HideTalent((int)TalentModManager.Instance.GetTalentIDFromName("Reloader"));
            }
            TalentModManager.Instance.HideTalent((int)ETalents.SCI_RESEARCH_SPECIALTY);
            if (PLNetworkManager.Instance.LocalPlayer != null)
            {
                foreach (PLPlayer player in PLServer.Instance.AllPlayers)
                {
                    if (player != null && player.TeamID == PLNetworkManager.Instance.LocalPlayer.TeamID)
                    {
                        if (Jetpack.AdvancedJetPack && player.Talents[(int)ETalents.INC_JETPACK] > 0)
                        {
                            int num = (int)player.Talents[(int)ETalents.INC_JETPACK];
                            player.Talents[TalentModManager.Instance.GetTalentIDFromName("Jetpack Fuel Reserve")] = (ObscuredInt)num;
                            player.Talents[(int)ETalents.INC_JETPACK] = (ObscuredInt)0;
                        }
                        else if (!Jetpack.AdvancedJetPack && player.Talents[TalentModManager.Instance.GetTalentIDFromName("Jetpack Fuel Reserve")] > 0)
                        {
                            int num = (int)player.Talents[TalentModManager.Instance.GetTalentIDFromName("Jetpack Fuel Reserve")];
                            player.Talents[(int)ETalents.INC_JETPACK] = (ObscuredInt)num;
                            player.Talents[(int)TalentModManager.Instance.GetTalentIDFromName("Jetpack Fuel Reserve")] = (ObscuredInt)0;
                        }
                    }
                }
            }
            if (!PhotonNetwork.isMasterClient)
                return;
            if (PLServer.Instance == null)
                return;
            foreach (PLPlayer player in PLServer.Instance.AllPlayers)
            {
                if (player != null && !player.IsBot && player.GetPhotonPlayer() != null && !SyncCheck.checkedPlayers.Contains(player.GetPhotonPlayer()))
                {
                    ModMessage.SendRPC("sugarbuzz1.ExpandedGalaxy", "ExpandedGalaxy.RequestSyncCheck", player.GetPhotonPlayer(), new object[0]);
                    SyncCheck.checkedPlayers.Add(player.GetPhotonPlayer());
                }
            }
        }
    }
}
