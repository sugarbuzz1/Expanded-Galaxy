using HarmonyLib;
using PulsarModLoader;
using System.Collections.Generic;
using Talents.Framework;
using UnityEngine;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLPlayer), "Update")]
    internal class ReloaderTalent
    {
        private static void Postfix(PLPlayer __instance, ref float ___LastSciHealPawnFrame, ref bool ___sciHealPawnFrame)
        {
            if (!PhotonNetwork.isMasterClient || __instance.GetPawn() == null)
                return;
            PLAmmoRefill refill;
            if (PLNetworkManager.Instance.CurrentGame != null && __instance.GetClassID() == 3 && __instance.Talents[TalentModManager.Instance.GetTalentIDFromName("Reloader")] > 0)
            {
                if (__instance.StartingShip != null && __instance.StartingShip.MyAmmoRefills.Length > 0)
                {
                    refill = __instance.StartingShip.MyAmmoRefills[0];
                    if (refill == null || !((double)refill.SupplyAmount > 0.0))
                        return;
                }
                else
                {
                    return;
                }
                List<int> playerIDs = new List<int>();
                foreach (PLPawn pawn in PLGameStatic.Instance.AllPawns)
                {
                    if (pawn != null && pawn.GetPlayer() != null && (int)pawn.GetPlayer().TeamID == (int)__instance.TeamID && !pawn.IsDead && (double)(pawn.transform.position - __instance.GetPawn().transform.position).sqrMagnitude < 9.0 && ((double)Time.time - (double)___LastSciHealPawnFrame > 4.0 || ___sciHealPawnFrame))
                    {
                        playerIDs.Add(pawn.GetPlayer().GetPlayerID());
                    }
                }
                if (playerIDs.Count > 0)
                {
                    ___sciHealPawnFrame = true;
                    int refillRate = (int)__instance.Talents[TalentModManager.Instance.GetTalentIDFromName("Reloader")];
                    foreach (int playerID in playerIDs)
                    {
                        PLPlayer player = PLServer.Instance.GetPlayerFromPlayerID(playerID);
                        if (player == null || player.MyInventory == null)
                            continue;
                        foreach (PLPawnItem item in player.MyInventory.AllItems)
                        {
                            if ((double)refill.SupplyAmount > 0.0 && item != null && item.UsesAmmo)
                            {
                                if (item.AmmoCurrent < item.AmmoMax)
                                {
                                    int num0 = Mathf.CeilToInt(item.AmmoMax * (0.05f * refillRate));
                                    if (!(num0 > 0))
                                        num0 = 1;
                                    int num1 = item.AmmoCurrent;
                                    int num3 = item.AmmoCurrent;
                                    num3 += num0;
                                    num3 = Mathf.Min(num3, item.AmmoMax);
                                    float num2 = Mathf.Abs(num3 - num1) * (Ammunition.AmmoRefillPercent() / (float)item.AmmoMax);
                                    refill.SupplyAmount -= num2;
                                }
                            }
                        }
                    }

                    ModMessage.SendRPC("sugarbuzz1.ExpandedGalaxy", "ExpandedGalaxy.ReloadRPC", PhotonTargets.All, new object[3]
                    {
                                __instance.StartingShip.ShipID,
                                playerIDs.ToArray(),
                                (int)__instance.Talents[TalentModManager.Instance.GetTalentIDFromName("Reloader")]
                    });
                    ___LastSciHealPawnFrame = Time.time;
                }
            }
        }
    }
}
