using HarmonyLib;
using PulsarModLoader.Content.Components.Hull;

namespace ExpandedGalaxy
{
    internal class Hull
    {
        private class JuggernautHullMod : HullMod
        {
            public override string Name => "Juggernaut Hull";

            public override string Description => "This hull boasts formidable strength and boosted collision damage. However, it isn't compatible with shields.";

            public override int MarketPrice => 38000;

            public override float HullMax => 1500f;

            public override float Armor => 0.8f;

            public override int CargoVisualID => 4;

            public override string GetStatLineLeft(PLShipComponent InComp) => PLLocalize.Localize("Integrity") + "\n" + PLLocalize.Localize("Armor") + "\n" + PLLocalize.Localize("Mass");

            public override string GetStatLineRight(PLShipComponent InComp)
            {
                PLHull plHull = InComp as PLHull;
                return (plHull.Max * InComp.LevelMultiplier(0.2f)).ToString("0") + "\n" + (plHull.Armor * 250f * InComp.LevelMultiplier(0.15f)).ToString("0") + "\n" + (200f + (40f * InComp.Level)).ToString("0") + "\n";
            }

            public override void AddStats(PLShipComponent InComp)
            {
                InComp.ShipStats.Mass += 200f + (40f * InComp.Level) - GetMassDiscountForShip(InComp.ShipStats.Ship.ShipTypeID);
            }

            public override void FinalLateAddStats(PLShipComponent InComp)
            {
                InComp.ShipStats.ShieldsCurrent = 0f;
                InComp.ShipStats.ShieldsMax = 0f;
                InComp.ShipStats.ShieldsChargeRate = 0f;
            }
        }

        [HarmonyPatch(typeof(PLShipStats), "TakeHullDamage")]
        internal class JuggernautDamage
        {
            private static bool Prefix(PLShipStats __instance, ref float inDmg, EDamageType inDmgType, PLShipInfoBase attackingShip, PLTurret turret)
            {
                if (__instance.GetShipComponent<PLHull>(ESlotType.E_COMP_HULL) != null)
                {
                    PLHull hull = __instance.GetShipComponent<PLHull>(ESlotType.E_COMP_HULL);
                    if (hull.SubType == HullModManager.Instance.GetHullIDFromName("Juggernaut Hull"))
                    {
                        inDmg *= 0.8f;
                    }
                }

                if (inDmgType == EDamageType.E_COLLISION)
                {
                    if ((UnityEngine.Object)attackingShip != (UnityEngine.Object)null)
                    {
                        if (attackingShip.MyHull != null)
                        {
                            PLHull enemyHull = attackingShip.MyHull;
                            if (enemyHull.SubType == HullModManager.Instance.GetHullIDFromName("Juggernaut Hull"))
                            {
                                inDmg *= 2.0f;
                            }
                        }
                    }
                }
                return true;
            }
        }

        private class MiningDroneHullMod : HullMod
        {
            public override string Name => "Extraction Drone Hull";

            public override string Description => "Hull built for the ancient mining drones. Lightweight yet strong.";

            public override int MarketPrice => 21000;

            public override float HullMax => 867.5f;

            public override float Armor => 0.34f;

            public override int CargoVisualID => 4;

            public override bool CanBeDroppedOnShipDeath => false;

            public override string GetStatLineLeft(PLShipComponent InComp) => PLLocalize.Localize("Integrity") + "\n" + PLLocalize.Localize("Armor") + "\n" + PLLocalize.Localize("Mass");

            public override string GetStatLineRight(PLShipComponent InComp)
            {
                PLHull plHull = InComp as PLHull;
                return (plHull.Max * InComp.LevelMultiplier(0.2f)).ToString("0") + "\n" + (plHull.Armor * 250f * InComp.LevelMultiplier(0.15f)).ToString("0") + "\n" + (90f + (7f * InComp.Level)).ToString("0") + "\n";
            }

            public override void AddStats(PLShipComponent InComp)
            {
                InComp.ShipStats.Mass += 90f + (7f * InComp.Level) - GetMassDiscountForShip(InComp.ShipStats.Ship.ShipTypeID);
            }
        }

        private class EscortDroneHullMod : HullMod
        {
            public override string Name => "Escort Drone Hull";

            public override string Description => "Hull built for the escort drones. Lightweight yet strong.";

            public override int MarketPrice => 23000;

            public override float HullMax => 1000f;

            public override float Armor => 0.8f;

            public override int CargoVisualID => 4;

            public override bool CanBeDroppedOnShipDeath => false;

            public override string GetStatLineLeft(PLShipComponent InComp) => PLLocalize.Localize("Integrity") + "\n" + PLLocalize.Localize("Armor") + "\n" + PLLocalize.Localize("Mass");

            public override string GetStatLineRight(PLShipComponent InComp)
            {
                PLHull plHull = InComp as PLHull;
                return (plHull.Max * InComp.LevelMultiplier(0.2f)).ToString("0") + "\n" + (plHull.Armor * 250f * InComp.LevelMultiplier(0.15f)).ToString("0") + "\n" + (108f + (9f * InComp.Level)).ToString("0") + "\n";
            }

            public override void AddStats(PLShipComponent InComp)
            {
                InComp.ShipStats.Mass += 108f + (9f * InComp.Level) - GetMassDiscountForShip(InComp.ShipStats.Ship.ShipTypeID);
            }
        }

        private class GuardianDroneHullMod : HullMod
        {
            public override string Name => "Guardian Drone Hull";

            public override string Description => "Hull built for Guardian Protector Drones. Heavy, yet resiliant.";

            public override int MarketPrice => 25000;

            public override float HullMax => 3250f;

            public override float Armor => 1f;

            public override int CargoVisualID => 4;

            public override bool CanBeDroppedOnShipDeath => false;

            public override string GetStatLineLeft(PLShipComponent InComp) => PLLocalize.Localize("Integrity") + "\n" + PLLocalize.Localize("Armor") + "\n" + PLLocalize.Localize("Mass");

            public override string GetStatLineRight(PLShipComponent InComp)
            {
                PLHull plHull = InComp as PLHull;
                return (plHull.Max * InComp.LevelMultiplier(0.2f)).ToString("0") + "\n" + (plHull.Armor * 250f * InComp.LevelMultiplier(0.15f)).ToString("0") + "\n" + (228f + (11f * InComp.Level)).ToString("0") + "\n";
            }

            public override void AddStats(PLShipComponent InComp)
            {
                InComp.ShipStats.Mass += 228f + (11f * InComp.Level) - GetMassDiscountForShip(InComp.ShipStats.Ship.ShipTypeID);
            }
        }

        [HarmonyPatch(typeof(PLHull), "AddStats")]
        internal class MassForBaseHulls
        {
            private static bool Prefix(PLHull __instance, PLShipStats inStats)
            {
                float mass = 0f;

                mass = GetBaseHullMass(__instance.SubType, __instance.Level);
                if (mass > 0f)
                {
                    mass -= GetMassDiscountForShip(inStats.Ship.ShipTypeID);
                }
                inStats.Mass += mass;
                return true;
            }
        }

        [HarmonyPatch(typeof(PLHull), "GetStatLineLeft")]
        internal class MassLineForBaseHullsLeft
        {
            private static bool Prefix(PLHull __instance, ref string __result)
            {
                if (__instance.SubType < HullModManager.Instance.VanillaHullMaxType)
                {
                    if (GetBaseHullMass(__instance.SubType, __instance.Level) == 0f)
                        return true;
                    __result = PLLocalize.Localize("Integrity") + "\n" + PLLocalize.Localize("Armor") + "\n" + PLLocalize.Localize("Mass") + "\n";
                    return false;
                }
                return true;
            }
        }

        [HarmonyPatch(typeof(PLHull), "GetStatLineRight")]
        internal class MassLineForBaseHullsRight
        {
            private static bool Prefix(PLHull __instance, ref string __result)
            {
                if (__instance.SubType < HullModManager.Instance.VanillaHullMaxType)
                {
                    if (GetBaseHullMass(__instance.SubType, __instance.Level) == 0f)
                        return true;
                    __result = (__instance.Max * __instance.LevelMultiplier(0.2f)).ToString("0") + "\n" + (__instance.Armor * 250f * __instance.LevelMultiplier(0.15f)).ToString("0") + "\n" + (GetBaseHullMass(__instance.SubType, __instance.Level)).ToString("0") + "\n";
                    return false;
                }
                return true;
            }
        }

        [HarmonyPatch(typeof(PLShipComponent), "GetExtraLineLeft")]
        internal class LayeredArmorMaxLine
        {
            private static bool Prefix(PLShipComponent __instance, ref string __result)
            {
                PLHull hull = __instance as PLHull;
                if (hull == null)
                {
                    return true;
                }
                else
                {

                    if (hull.SubType == 9)
                    {
                        __result = "-\n" + PLLocalize.Localize("Armor (Max)") + "\n";
                        return false;
                    }
                    if (hull.SubType == HullModManager.Instance.GetHullIDFromName("Juggernaut Hull"))
                    {
                        __result = "-\n" + PLLocalize.Localize("Incoming Dmg") + "\n";
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
            }
        }

        [HarmonyPatch(typeof(PLShipComponent), "GetExtraLineRight")]
        internal class LayeredArmorMaxNumberLine
        {
            private static bool Prefix(PLShipComponent __instance, ref string __result)
            {
                PLHull hull = __instance as PLHull;
                if (hull == null)
                {
                    return true;
                }
                else
                {
                    if (hull.SubType == 9)
                    {
                        __result = "Level " + (hull.Level + 1).ToString() + "\n" + (500f * __instance.LevelMultiplier(0.15f)).ToString("0") + "\n";
                        return false;
                    }
                    if (hull.SubType == HullModManager.Instance.GetHullIDFromName("Juggernaut Hull"))
                    {
                        __result = "Level " + (hull.Level + 1).ToString() + "\n" + "-20%" + "\n";
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
            }
        }

        [HarmonyPatch(typeof(PLShipStats), "ClearStats")]
        internal class ClearMass
        {
            private static void Postfix(PLShipStats __instance)
            {
                switch (__instance.Ship.ShipTypeID)
                {
                    case EShipType.E_INTREPID:
                    case EShipType.E_INFECTED_CARRIER:
                    case EShipType.E_ACADEMY:
                    case EShipType.E_CIVILIAN_STARTING_SHIP:
                        __instance.Mass = 500f;
                        break;
                    case EShipType.E_WDDRONE1:
                    case EShipType.E_MATRIX_DRONE:
                    case EShipType.E_INFECTED_FIGHTER:
                    case EShipType.E_INFECTED_FIGHTER_HEAVY:
                    case EShipType.E_REPAIR_DRONE:
                    case EShipType.E_SELF_DESTRUCT_DRONE:
                    case EShipType.E_BOUNTY_HUNTER_01:
                    case EShipType.E_BEACON:
                        __instance.Mass = 480f;
                        break;
                    case EShipType.E_WDCRUISER:
                        __instance.Mass = 520f;
                        break;
                    case EShipType.E_STARGAZER:
                        __instance.Mass = 540f;
                        break;
                    case EShipType.E_ROLAND:
                        __instance.Mass = 1020f;
                        break;
                    case EShipType.E_DESTROYER:
                        __instance.Mass = 750f;
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
                    case EShipType.E_OUTRIDER:
                    case EShipType.E_FLUFFY_DELIVERY:
                    case EShipType.E_POLYTECH_SHIP:
                    case EShipType.E_PTDRONE:
                        __instance.Mass = 600f;
                        break;
                    case EShipType.E_ALIEN_TENTACLE_EYE:
                        __instance.Mass = 12931f;
                        break;
                    case EShipType.E_ANNIHILATOR:
                        __instance.Mass = 655f;
                        break;
                    case EShipType.E_DEATHSEEKER_DRONE_SC:
                        __instance.Mass = 300f;
                        break;
                    case EShipType.OLDWARS_HUMAN:
                        __instance.Mass = 680f;
                        break;
                    case EShipType.E_GUARDIAN:
                        __instance.Mass = 3600f;
                        break;
                    case EShipType.E_UNSEEN_FIGHTER:
                        __instance.Mass = 700f;
                        break;
                    default:
                        __instance.Mass = 380f;
                        break;
                }
            }
        }

        public static float GetBaseHullMass(int Subtype, int level = 0)
        {
            float mass = 0f;
            switch (Subtype)
            {
                case (int)EHullType.E_CCG_LIGHT_HULL:
                    mass = 80f + (8f * level);
                    break;

                case (int)EHullType.E_CCG_HULL:
                    mass = 100f + (12f * level);
                    break;

                case (int)EHullType.E_CCG_MILITARYGRADE:
                    mass = 150f + (30f * level);
                    break;

                case (int)EHullType.E_DESTROYER_HULL:
                    mass = 200f + (40f * level);
                    break;

                case (int)EHullType.E_HUMAN_OLDWARS_HULL:
                    mass = 175f + (10f * level);
                    break;

                case (int)EHullType.E_LAYERED_HULL:
                    mass = 135f + (26f * level);
                    break;

                case (int)EHullType.E_NANO_ACTIVE_HULL:
                    mass = 80f + (8f * level);
                    break;

                case (int)EHullType.E_OBSOLETE_HULL:
                    mass = 40f + (1f * level);
                    break;

                case (int)EHullType.E_POLYTECH_HULL:
                    mass = 90f + (10f * level);
                    break;

                default:
                    break;
            }
            return mass;
        }

        public static float GetMassDiscountForShip(EShipType shipType)
        {
            float discount = 0f;

            switch (shipType)
            {
                case EShipType.E_INTREPID:
                case EShipType.E_STARGAZER:
                case EShipType.E_OUTRIDER:
                case EShipType.E_FLUFFY_TWO:
                    discount = 100f;
                    break;

                case EShipType.E_ROLAND:
                case EShipType.OLDWARS_SYLVASSI:
                    discount = 112f;
                    break;

                case EShipType.E_CIVILIAN_STARTING_SHIP:
                case EShipType.E_FLUFFY_DELIVERY:
                    discount = 80f;
                    break;

                case EShipType.E_CARRIER:
                    discount = 88;
                    break;

                case EShipType.E_WDCRUISER:
                    discount = 150f;
                    break;

                case EShipType.E_DESTROYER:
                    discount = 240;
                    break;

                case EShipType.OLDWARS_HUMAN:
                    discount = 195;
                    break;

                case EShipType.E_ANNIHILATOR:
                    discount = 135f;
                    break;

                case EShipType.E_POLYTECH_SHIP:
                    discount = 90f;
                    break;

                default:
                    break;
            }
            return discount;
        }
    }
}
