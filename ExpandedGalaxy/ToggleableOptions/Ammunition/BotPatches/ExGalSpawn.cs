using CodeStage.AntiCheat.ObscuredTypes;
using HarmonyLib;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLSpawner), "DoSpawnStatic")]
    internal class ExGalSpawn
    {
        private static bool Prefix(PLPersistantEncounterInstance pei, string spawnType, Transform childTransform, PLSpawner spawner, PLTeleportationLocationInstance optionalTLI, PLInterior optionalInterior, PLCombatTarget combatTargetWhoSpawnedMe)
        {
            if (pei != null && PLServer.Instance != null && ((pei is PLLavaPlanet2Encounter && string.Equals(spawnType, "BanditSpawn", StringComparison.OrdinalIgnoreCase) && PLServer.Instance.HasCompletedMissionWithID(8000015)) || (pei is PLWarpStationEncounter && PLServer.GetSectorWithID(pei.GetSectorID()).MySPI.Faction == 6)))
                return false;
            return true;
        }
        private static void Postfix(PLPersistantEncounterInstance pei, string spawnType, Transform childTransform, PLSpawner spawner, PLTeleportationLocationInstance optionalTLI, PLInterior optionalInterior, PLCombatTarget combatTargetWhoSpawnedMe)
        {
            if (pei == null)
                return;
            foreach (GameObject allPlayerObject in pei.MyCreatedPlayers)
            {
                if (allPlayerObject == null)
                    continue;
                PLPlayer allPlayer = allPlayerObject.GetComponent<PLPlayer>();
                if (allPlayer != null && !allPlayer.gameObject.name.Contains("ExGal"))
                {
                    allPlayer.Talents[(int)ETalents.WPN_AMMO_BOOST] = (ObscuredInt)50;
                    allPlayer.gameObject.name += " ExGal";

                    if (pei is PLLavaPlanet2Encounter && !allPlayer.gameObject.name.Contains("Lava2"))
                    {
                        int netID = PLServer.Instance.PawnInvItemIDCounter++;
                        int num = UnityEngine.Random.Range(0, 2);
                        allPlayer.MyInventory.Clear();
                        if (num == 0)
                            allPlayer.MyInventory.UpdateItem(netID, 9, 0, 4, 1);
                        else
                            allPlayer.MyInventory.UpdateItem(netID, 12, 0, 4, 1);
                        allPlayer.Talents[(int)ETalents.HEALTH_BOOST] = (ObscuredInt)15;
                        allPlayer.Talents[(int)ETalents.ARMOR_BOOST] = (ObscuredInt)12;
                        allPlayer.Talents[(int)ETalents.PISTOL_DMG_BOOST] = (ObscuredInt)5;
                        allPlayer.gameObject.name += " Lava2";
                        allPlayer.SetPlayerName("Guard");
                    }
                    Dictionary<int, int> talentMap = new Dictionary<int, int>();
                    for (int i = 0; i < allPlayer.Talents.Length; i++)
                    {
                        if ((int)allPlayer.Talents[i] > 0)
                            talentMap.Add(i, (int)allPlayer.Talents[i]);
                    }
                    if (talentMap.Keys.Count > 0)
                        allPlayer.StartCoroutine(EnsureCorrectTalents(allPlayer, talentMap));
                }
            }
        }

        private static IEnumerator EnsureCorrectTalents(PLPlayer player, Dictionary<int, int> talentMap)
        {
            bool corrected = false;
            int tries = 0;

            yield return new WaitForEndOfFrame();
            do
            {
                foreach (int talentID in talentMap.Keys)
                {
                    if ((int)player.Talents[talentID] != talentMap[talentID])
                    {
                        player.Talents[talentID] = (ObscuredInt)talentMap[talentID];
                        corrected = true;
                    }
                }
                if (corrected)
                    ++tries;
                yield return new WaitForEndOfFrame();
            }
            while (!corrected);
            Debug.Log(String.Format("[ExGal] Player {0} talents corrected in {1} tries.", player.GetPlayerID(), tries));
        }
    }
}
