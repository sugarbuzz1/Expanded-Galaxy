namespace ExpandedGalaxy.SensorDish
{
    public class VanillaSensorDishMod : SensorDishMod
    {
        public override string Name => "Sensor Dish";

        public VanillaSensorDishMod()
        {
            SensorDishAbilities[0] = new CyberDefenseFlaw();
            SensorDishAbilities[1] = new WeakenShields();
            SensorDishAbilities[2] = new ReactorProtocol();
        }

        public override int AbilityLogicForBots(PLBot inBot, PLShipInfo shipContext)
        {
            if (inBot.PlayerOwner.GetPriorityLevel(20) > 0 && shipContext.MyStats.CyberAttackRating > shipContext.TargetShip.MyStats.CyberDefenseRating * 0.6f && HasUseableVirus(shipContext))
            {
                return 3;
            }
            else if (!SensorDishModManager.IsSensorWeaknessActiveModded(shipContext.TargetShip, 5, 0) && shipContext.TargetShip.ReactorCoolingEnabled ? shipContext.TargetShip.MyStats.ReactorTempCurrent / shipContext.TargetShip.MyStats.ReactorTempMax > 0.9f : shipContext.TargetShip.CoreInstability < 0.3f && UnityEngine.Random.Range(0, 1000) > 600)
            {
                return 5;
            }
            else if (shipContext.TargetShip.MyShieldGenerator != null && shipContext.TargetShip.MyShieldGenerator.MinIntegrityAfterDamageScaled < shipContext.TargetShip.MyShieldGenerator.Current && (double)shipContext.TargetShip.MyStats.ShieldsMax != 0.0)
            {
                return 4;
            }
            else if (shipContext.TargetShip.GetCombatLevel() > shipContext.GetCombatLevel() && (float)shipContext.TargetShip.WeaponsSystem.Health > 5f)
            {
                return 1;
            }
            else if ((float)shipContext.TargetShip.EngineeringSystem.Health > 5f)
            {
                return 0;
            }
            else if ((float)shipContext.TargetShip.WeaponsSystem.Health > 5f)
            {
                return 1;
            }
            else
            {
                return 2;
            }
        }

        internal static bool HasUseableVirus(PLShipInfoBase inShipInfo)
        {
            bool flag = false;
            foreach (PLWarpDriveProgram program in inShipInfo.MyStats.GetComponentsOfType(ESlotType.E_COMP_PROGRAM))
            {
                if (program.IsVirus && program.Level >= program.MaxLevelCharges)
                {
                    flag = true;
                    break;
                }
            }
            return flag;
        }
    }
}
