using PulsarModLoader.Utilities;

namespace ExpandedGalaxy
{
    internal class Programs
    {
        public static bool HasActiveProgramOfType(PLShipInfoBase shipInfo, int programType)
        {
            foreach (PLWarpDriveProgram program in shipInfo.MyStats.GetComponentsOfType(ESlotType.E_COMP_PROGRAM))
            {
                if (program.SubType == programType)
                    if ((double)program.GetActiveTimerAlpha() > 0.0 && (double)program.GetActiveTimerAlpha() < 1.0)
                        return true;
            }
            return false;
        }
    }
}

