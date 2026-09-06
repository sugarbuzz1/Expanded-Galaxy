namespace ExpandedGalaxy
{
    public class PLMissionObjective_CompleteAfterJumpCount : PLMissionObjective
    {
        public PLMissionObjective_CompleteAfterJumpCount(int inJumpCount)
        {
            this.AmountNeeded = inJumpCount;
            this.AmountCompleted = 0;
            if (this.AmountNeeded <= 1)
                return;
            this.m_ObjectiveText = PLLocalize.Localize("Complete mission after ") + this.AmountNeeded.ToString() + " " + PLLocalize.Localize("jumps.");
        }

        public override void CheckIfCompleted() => this.IsCompleted = this.AmountCompleted >= this.AmountNeeded;

        public override string CustomPostString() => " (" + this.AmountCompleted.ToString() + "/" + this.AmountNeeded.ToString() + ")";

        public static void OnShipWarp()
        {
            foreach (PLMissionObjective missionObjective in PLMissionObjective.AllMissionObjectives)
            {
                if (missionObjective != null && !missionObjective.MyMissionIsCompleted && missionObjective is PLMissionObjective_CompleteAfterJumpCount completeWithinJumpCount)
                    ++completeWithinJumpCount.AmountCompleted;
            }
        }
    }

}

