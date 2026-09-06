using HarmonyLib;
using PulsarModLoader.Content.Components.WarpDrive;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLShipComponent), "Tick")]
    internal class WarpDriveTickPatch
    {
        private static void Postfix(PLShipComponent __instance)
        {
            PLWarpDrive drive = __instance as PLWarpDrive;
            if (drive == null)
                return;
            int index = drive.SubType - WarpDriveModManager.Instance.VanillaWarpDriveMaxType;
            if (index <= -1 || index >= WarpDriveModManager.Instance.WarpDriveTypes.Count || drive.ShipStats == null)
                return;
            WarpDriveModManager.Instance.WarpDriveTypes[index].Tick((PLShipComponent)drive);
        }
    }
}
