using HarmonyLib;
using PulsarModLoader.Content.Components.WarpDrive;

namespace ExpandedGalaxy
{
    public class DestroyedDrive : WarpDriveMod
    {
        public override string Name => "Broken Warp Drive";

        public override string Description => "A warp drive that has seemingly been modified to have warp capabilities using a method long lost to the sands of time. Unfortunately, most of it is beyond salvageable. The drive's interface contains some recognizeable components that could be repaired with enough scrap...";

        public override bool CanBeDroppedOnShipDeath => true;

        public override float ChargeSpeed => 0f;

        public override float WarpRange => 0f;

        public override float EnergySignature => 2f;

        public override int NumberOfChargesPerFuel => 0;

        public override float MaxPowerUsage_Watts => 1f;

        private void UpdateDescription(PLShipComponent InComp)
        {
            if (InComp.Level == 0)
                InComp.Desc = "A warp drive that has seemingly been modified to have warp capabilities using a method long lost to the sands of time. Unfortunatly, most of it is beyond salvageable. The drive's interface contains some recognizeable components that could be repaired with enough scrap...";
            else
                InComp.Desc = "A warp drive that has seemingly been modified to have warp capabilities using a method long lost to the sands of time. Unfortunatly, most of it is beyond salvageable. The drive's interface has been restored.";
        }

        public override void Tick(PLShipComponent InComp)
        {
            UpdateDescription(InComp);
            PLWarpDrive pLWarpDrive = InComp as PLWarpDrive;
            if (!pLWarpDrive.ImportantItem)
                pLWarpDrive.ImportantItem = true;
        }

        [HarmonyPatch(typeof(PLShipInfo), "GetMatCostForComp")]
        private class BrokenDriveRepairCost
        {
            private static void Postfix(PLShipComponent shipComp, ref int __result)
            {
                if (shipComp.SlotType == ESlotType.E_COMP_WARP && shipComp.SubType == WarpDriveModManager.Instance.GetWarpDriveIDFromName("Broken Warp Drive"))
                {
                    if (shipComp.Level == 0)
                        __result = 10;
                    else
                        __result = 999;
                }
            }
        }
    }
}
