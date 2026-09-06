using PulsarModLoader;
using PulsarModLoader.Content.Components.AutoTurret;
using PulsarModLoader.Content.Components.CaptainsChair;
using PulsarModLoader.Content.Components.CPU;
using PulsarModLoader.Content.Components.Extractor;
using PulsarModLoader.Content.Components.FBRecipeModule;
using PulsarModLoader.Content.Components.Hull;
using PulsarModLoader.Content.Components.HullPlating;
using PulsarModLoader.Content.Components.InertiaThruster;
using PulsarModLoader.Content.Components.ManeuverThruster;
using PulsarModLoader.Content.Components.MegaTurret;
using PulsarModLoader.Content.Components.Missile;
using PulsarModLoader.Content.Components.MissionShipComponent;
using PulsarModLoader.Content.Components.NuclearDevice;
using PulsarModLoader.Content.Components.PolytechModule;
using PulsarModLoader.Content.Components.Reactor;
using PulsarModLoader.Content.Components.Shield;
using PulsarModLoader.Content.Components.Thruster;
using PulsarModLoader.Content.Components.Turret;
using PulsarModLoader.Content.Components.Virus;
using PulsarModLoader.Content.Components.WarpDrive;
using PulsarModLoader.Content.Components.WarpDriveProgram;
using PulsarModLoader.Content.Items;

namespace ExpandedGalaxy
{
    internal class RequestSyncCheck : ModMessage
    {
        public override void HandleRPC(object[] arguments, PhotonMessageInfo sender)
        {
            if (sender.sender.IsMasterClient)
            {
                ModMessage.SendRPC("sugarbuzz1.ExpandedGalaxy", "ExpandedGalaxy.RecieveSyncCheck", PhotonTargets.MasterClient, new object[22]
            {
                    (object) AutoTurretModManager.Instance.AutoTurretTypes.Count,
                    (object) CaptainsChairModManager.Instance.CaptainsChairTypes.Count,
                    (object) CPUModManager.Instance.CPUTypes.Count,
                    (object) ExtractorModManager.Instance.ExtractorTypes.Count,
                    (object) FBRecipeModuleModManager.Instance.FBRecipeModuleTypes.Count,
                    (object) HullModManager.Instance.HullTypes.Count,
                    (object) HullPlatingModManager.Instance.HullPlatingTypes.Count,
                    (object) InertiaThrusterModManager.Instance.InertiaThrusterTypes.Count,
                    (object) ManeuverThrusterModManager.Instance.ManeuverThrusterTypes.Count,
                    (object) MegaTurretModManager.Instance.MegaTurretTypes.Count,
                    (object) MissileModManager.Instance.MissileTypes.Count,
                    (object) MissionShipComponentModManager.Instance.MissionShipComponentTypes.Count,
                    (object) NuclearDeviceModManager.Instance.NuclearDeviceTypes.Count,
                    (object) PolytechModuleModManager.Instance.PolytechModuleTypes.Count,
                    (object) ReactorModManager.Instance.ReactorTypes.Count,
                    (object) ShieldModManager.Instance.ShieldTypes.Count,
                    (object) ThrusterModManager.Instance.ThrusterTypes.Count,
                    (object) TurretModManager.Instance.TurretTypes.Count,
                    (object) VirusModManager.Instance.VirusTypes.Count,
                    (object) WarpDriveModManager.Instance.WarpDriveTypes.Count,
                    (object) WarpDriveProgramModManager.Instance.WarpDriveProgramTypes.Count,
                    (object) ItemModManager.Instance.ItemTypes.Count,
            });
            }
        }
    }
}
