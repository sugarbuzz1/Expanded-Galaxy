using HarmonyLib;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLShipComponent), "CreateShipComponentFromHash")]
    internal class CreateMissionFlagFromHash
    {
        private static bool Prefix(int inHash, SaveGameData inData, ref PLShipComponent __result)
        {
            uint num1 = (uint)(inHash & 63);
            uint inSubType = (uint)(inHash >> 6 & 63);
            uint inLevel = (uint)(inHash >> 12 & 31);
            uint inSubTypeData = (uint)(inHash >> 17 & 63);
            uint num2 = (uint)(inHash >> 23 & 63);
            if (inData != null && (int)inData.SaveVerID < 49)
            {
                inLevel = (uint)(inHash >> 12 & 15);
                inSubTypeData = (uint)(inHash >> 16 & 63);
                num2 = (uint)(inHash >> 22 & 63);
            }

            if (num1 == (int)ESlotType.E_COMP_REAC_COOLING)
            {
                PLShipComponent component = MissionComponentFlag.CreateMissionFlagFromHash((int)inSubType, (int)inLevel, (int)inSubTypeData);
                if (component != null)
                {
                    component.VisualSlotType = (ESlotType)num2;
                    if (component.VisualSlotType == ESlotType.E_COMP_NONE)
                        component.VisualSlotType = component.ActualSlotType;
                }
                __result = component;
                return false;
            }
            return true;
        }
    }

}

