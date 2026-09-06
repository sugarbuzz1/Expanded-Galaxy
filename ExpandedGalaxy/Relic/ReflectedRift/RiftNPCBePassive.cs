using HarmonyLib;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLShipInfoBase), "ShouldBeHostileToShip")]
    internal class RiftNPCBePassive
    {
        private static void Postfix(PLShipInfoBase __instance, PLShipInfoBase inShip, ref bool __result)
        {
            if (__instance.SelectedActorID == "ExGal_ReflectedRift_NPC" || inShip.SelectedActorID == "ExGal_ReflectedRift_NPC")
                __result = false;
        }
    }
}
