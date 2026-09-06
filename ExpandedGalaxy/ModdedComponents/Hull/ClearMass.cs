using HarmonyLib;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLShipStats), "ClearStats")]
    internal class ClearMass
    {
        private static void Postfix(PLShipStats __instance)
        {
            switch (__instance.Ship.ShipTypeID)
            {
                case EShipType.E_DEATHSEEKER_DRONE_SC:
                    __instance.Mass = 300f;
                    break;
                case EShipType.E_ABYSS_FIGHTER:
                case EShipType.E_ABYSS_HEAVY_FIGHTER:
                    __instance.Mass = 400f;
                    break;
                case EShipType.E_STARGAZER:
                case EShipType.E_MATRIX_DRONE:
                case EShipType.E_INFECTED_FIGHTER:
                case EShipType.E_INFECTED_FIGHTER_HEAVY:
                case EShipType.E_REPAIR_DRONE:
                case EShipType.E_SELF_DESTRUCT_DRONE:
                case EShipType.E_BOUNTY_HUNTER_01:
                case EShipType.E_BEACON:
                    __instance.Mass = 480f;
                    break;
                case EShipType.E_INTREPID:
                case EShipType.E_INFECTED_CARRIER:
                case EShipType.E_ACADEMY:
                case EShipType.E_CIVILIAN_STARTING_SHIP:
                    __instance.Mass = 500f;
                    break;
                case EShipType.E_WDCRUISER:
                case EShipType.E_WDDRONE3:
                    __instance.Mass = 520f;
                    break;
                case EShipType.E_OUTRIDER:
                case EShipType.E_FLUFFY_DELIVERY:
                case EShipType.E_POLYTECH_SHIP:
                case EShipType.E_PTDRONE:
                case EShipType.E_WDDRONE1:
                    __instance.Mass = 600f;
                    break;
                case EShipType.E_ANNIHILATOR:
                case EShipType.E_WDDRONE2:
                    __instance.Mass = 655f;
                    break;
                case EShipType.OLDWARS_HUMAN:
                    __instance.Mass = 680f;
                    break;
                case EShipType.E_UNSEEN_FIGHTER:
                    __instance.Mass = 700f;
                    break;
                case EShipType.E_DESTROYER:
                    __instance.Mass = 750f;
                    break;
                case EShipType.E_ROLAND:
                    __instance.Mass = 1020f;
                    break;
                case EShipType.E_DEATHSEEKER_DRONE:
                    __instance.Mass = 1200f;
                    break;
                case EShipType.E_SHOCK_DRONE:
                    __instance.Mass = 1800f;
                    break;
                case EShipType.E_SWARM_KEEPER:
                    __instance.Mass = 2000f;
                    break;
                case EShipType.E_ALIEN_TENTACLE_EYE:
                    __instance.Mass = 12931f;
                    break;
                case EShipType.E_GUARDIAN:
                    __instance.Mass = 3600f;
                    break;
                default:
                    __instance.Mass = 380f;
                    break;
            }
        }
    }
}
