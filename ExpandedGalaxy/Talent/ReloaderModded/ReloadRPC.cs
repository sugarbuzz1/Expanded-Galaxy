using PulsarModLoader;
using UnityEngine;

namespace ExpandedGalaxy
{
    internal class ReloadRPC : ModMessage
    {
        public override void HandleRPC(object[] arguments, PhotonMessageInfo sender)
        {
            int[] playerIDs = (int[])arguments[1];
            int refillRate = (int)arguments[2];
            foreach (int playerID in playerIDs)
            {
                PLPlayer player = PLServer.Instance.GetPlayerFromPlayerID(playerID);
                if (player == null || player.MyInventory == null)
                    continue;
                foreach (PLPawnItem item in player.MyInventory.AllItems)
                {
                    if (item != null && item.UsesAmmo)
                    {
                        if (item.AmmoCurrent < item.AmmoMax)
                        {
                            int num0 = Mathf.CeilToInt(item.AmmoMax * (0.05f * refillRate));
                            if (!(num0 > 0))
                                num0 = 1;
                            PLMusic.PostEvent("play_sx_player_item_grenadelauncher_ammorefill", player.GetPawn().gameObject);
                            item.AmmoCurrent += num0;
                            item.AmmoCurrent = Mathf.Min(item.AmmoCurrent, item.AmmoMax);
                            if (player == PLNetworkManager.Instance.LocalPlayer)
                            {
                                PLPawn pawn = player.GetPawn();
                                ++pawn.LerpedVignetteAmount;
                                pawn.LerpedVignetteAmount = Mathf.Clamp(pawn.LerpedVignetteAmount, 0f, 5f);
                                pawn.LerpedVignetteSize = 1f;
                                pawn.LerpedVignetteColor = new Color(0.75f, 0.225f, 0f, 0.75f);
                            }
                        }
                    }
                }
            }
        }
    }
}
