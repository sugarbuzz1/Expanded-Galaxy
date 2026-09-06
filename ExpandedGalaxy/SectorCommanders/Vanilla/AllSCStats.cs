using HarmonyLib;
using UnityEngine;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLShipInfoBase), "ShipFinalCalculateStats")]
    internal class AllSCStats
    {
        private static void Postfix(PLShipInfoBase __instance, ref PLShipStats inStats)
        {
            if (inStats.Ship.ComputerSystem == null)
                return;
            if (__instance is PLCorruptedDroneShipInfo)
            {
                inStats.CyberDefenseRating += (2.5f + 0.2f * Mathf.Floor(PLServer.Instance.ChaosLevel / 2) * PLGlobal.Instance.Galaxy.GenerationSettings.EnemyShipPowerScalar) * inStats.Ship.ComputerSystem.GetHealthRatio();
                inStats.CyberAttackRating += (0.5f + 0.1f * Mathf.Floor(PLServer.Instance.ChaosLevel / 3) * PLGlobal.Instance.Galaxy.GenerationSettings.EnemyShipPowerScalar) * inStats.Ship.ComputerSystem.GetHealthRatio();
            }
            else if (__instance is PLWarpGuardian)
            {
                inStats.CyberDefenseRating += (1.5f + 0.1f * Mathf.Floor(PLServer.Instance.ChaosLevel) * PLGlobal.Instance.Galaxy.GenerationSettings.EnemyShipPowerScalar) * inStats.Ship.ComputerSystem.GetHealthRatio();
                inStats.CyberAttackRating += (0f + 0.1f * Mathf.Floor(PLServer.Instance.ChaosLevel) * PLGlobal.Instance.Galaxy.GenerationSettings.EnemyShipPowerScalar) * inStats.Ship.ComputerSystem.GetHealthRatio();
            }
            else if (__instance is PLDeathseekerCommanderDrone)
            {
                inStats.CyberDefenseRating += (3.0f + 0.1f * Mathf.Floor(PLServer.Instance.ChaosLevel) * PLGlobal.Instance.Galaxy.GenerationSettings.EnemyShipPowerScalar) * inStats.Ship.ComputerSystem.GetHealthRatio();
            }
            else if (__instance is PLAlchemistShipInfo && !__instance.GetIsPlayerShip())
            {
                inStats.CyberDefenseRating = Mathf.Clamp(inStats.CyberDefenseRating, 1.5f, float.MaxValue) * inStats.Ship.ComputerSystem.GetHealthRatio();
                inStats.CyberAttackRating += 2.0f * inStats.Ship.ComputerSystem.GetHealthRatio();
            }
            else if (__instance is PLIntrepidCommanderInfo && !__instance.GetIsPlayerShip())
            {
                inStats.CyberDefenseRating = Mathf.Clamp(inStats.CyberDefenseRating, 2f, float.MaxValue) * inStats.Ship.ComputerSystem.GetHealthRatio();
                inStats.CyberAttackRating += 1.0f * inStats.Ship.ComputerSystem.GetHealthRatio();
            }
        }
    }
}
