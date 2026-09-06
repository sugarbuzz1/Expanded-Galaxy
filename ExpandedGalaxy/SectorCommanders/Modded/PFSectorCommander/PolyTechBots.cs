using HarmonyLib;
using UnityEngine;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLGame), "SpawnSimpleCombatBotAtTransformForPlayer")]
    internal class PolyTechBots
    {
        private static bool Prefix(PLGame __instance, Vector3 pos, PLPlayer newPlayer, ref string pawnPrefabString)
        {
            if ((UnityEngine.Object)newPlayer != (UnityEngine.Object)null)
            {
                if ((UnityEngine.Object)newPlayer.StartingShip != (UnityEngine.Object)null)
                {
                    if (newPlayer.StartingShip.ShipTypeID == EShipType.E_POLYTECH_SHIP && (newPlayer.RaceID != 2 || !newPlayer.Gender_IsMale))
                    {
                        newPlayer.SetGender(true);
                        newPlayer.SetRace(2);
                        newPlayer.SetAIGender(true);
                        newPlayer.SetAIRace(2);
                        pawnPrefabString = "PLPawnAndroid";
                    }
                }
            }

            return true;
        }
    }
}
