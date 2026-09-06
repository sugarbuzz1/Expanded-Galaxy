using HarmonyLib;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLWarpDriveProgram), "MakesSenseToRun")]
    internal class MakesSenseToRunPatch
    {
        private static bool Prefix(PLWarpDriveProgram __instance, PLBot inBot, int numVirusTargets, int numVirusesIHave, ref bool __result)
        {
            if (__instance.SubType == 17)
            {
                __result = false;
                if (inBot.PlayerOwner != null && inBot.PlayerOwner.StartingShip != null && inBot.PlayerOwner.StartingShip.MyStats != null)
                {
                    PLTurret mainTurret = inBot.PlayerOwner.StartingShip.MyStats.GetMainTurret();
                    if (mainTurret != null)
                    {
                        __result = !mainTurret.IsOverheated && (double)mainTurret.Heat > 0.7;
                    }
                    else
                    {
                        int num = 0;
                        float totalHeat = 0f;
                        foreach (PLTurret turret in inBot.PlayerOwner.StartingShip.MyStats.GetComponentsOfType(ESlotType.E_COMP_TURRET))
                        {
                            if (!turret.IsOverheated)
                            {
                                totalHeat += turret.Heat;
                                ++num;
                            }
                        }
                        __result = (double)(totalHeat / num) > 0.5;
                    }
                }
                return false;
            }
            return true;
        }
    }
}

