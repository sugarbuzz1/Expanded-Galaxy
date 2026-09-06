namespace ExpandedGalaxy.SensorDish
{
    public abstract class SensorDishMod
    {
        public virtual string Name => "";

        public virtual PLShipComponent SensorDish => (PLShipComponent)new PLSensorDish(0, 0);

        public SensorDishAbility[] SensorDishAbilities = new SensorDishAbility[3];

        //Dictates the logic for the bots when using sensor weaknesses
        public virtual int AbilityLogicForBots(PLBot inBot, PLShipInfo shipContext)
        {
            return -1;
        }
    }
}
