namespace ExpandedGalaxy
{
    public class MiningDroneSignal : PLDistressSignal
    {

        public MiningDroneSignal(int inType, int inLevel = 0) : base(EDistressSignalType.UNION, inLevel)
        {
            this.SubType = 4;
            this.Name = "368.6 MHz";
            this.Desc = "A distress signal recovered from the wreckage of a mining drone.\n\nPerhaps using it can give insight on where they originated from...";
            this.CanBeDroppedOnShipDeath = false;
            this.Level = inLevel;
        }

        private void UpdateDescription()
        {
            if (this.IsEquipped)
                this.Desc = "Distress Signal: Mining Drone";
            else
                this.Desc = "A distress signal recovered from the wreckage of a mining drone.\nPerhaps using it can give insight on where they originated from...";
        }

        public override void Tick()
        {
            base.Tick();
            this.UpdateDescription();
        }
    }
}
