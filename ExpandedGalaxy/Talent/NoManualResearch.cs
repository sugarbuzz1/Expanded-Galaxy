using HarmonyLib;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLServer), "ServerManualProgramCharge")]
    internal class NoManualResearch
    {
        private static bool Prefix(PLServer __instance, int inShipID, PhotonMessageInfo pmi, out bool __state)
        {
            __state = false;
            PLShipInfoBase shipFromId = PLEncounterManager.Instance.GetShipFromID(inShipID);
            if (!((UnityEngine.Object)shipFromId != (UnityEngine.Object)null))
                return true;
            __state = shipFromId.WarpCapsuleIsLoaded;
            return true;
        }
        private static void Postfix(PLServer __instance, int inShipID, PhotonMessageInfo pmi, bool __state)
        {
            PLShipInfoBase shipFromId = PLEncounterManager.Instance.GetShipFromID(inShipID);
            if (!((UnityEngine.Object)shipFromId != (UnityEngine.Object)null) || !__state || shipFromId.NumberOfFuelCapsules < 0)
                return;
            if (shipFromId.MyWarpDrive != null && __instance.TalentToResearch != ETalents.MAX && (int)__instance.JumpsNeededToResearchTalent >= 0)
                ++__instance.JumpsNeededToResearchTalent;

        }
    }
}
