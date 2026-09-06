namespace ExpandedGalaxy
{
    internal class Hull
    {
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
