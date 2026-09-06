using CodeStage.AntiCheat.ObscuredTypes;
using HarmonyLib;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLShipInfo), "CreateDefaultItemsForEnemyBotPlayer")]
    internal class CrewReloaderTalent
    {
        private static void Postfix(PLShipInfo __instance, PLPlayer inPlayer)
        {
            if (Ammunition.DynamicAmmunition)
                inPlayer.Talents[49] = (ObscuredInt)50;
            inPlayer.gameObject.name += " ExGal";
        }
    }
}
