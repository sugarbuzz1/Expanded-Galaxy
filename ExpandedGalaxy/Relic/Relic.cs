using PulsarModLoader.Content.Components.AutoTurret;
using PulsarModLoader.Content.Components.CaptainsChair;
using PulsarModLoader.Content.Components.CPU;
using PulsarModLoader.Content.Components.Hull;
using PulsarModLoader.Content.Components.InertiaThruster;
using PulsarModLoader.Content.Components.MegaTurret;
using PulsarModLoader.Content.Components.Missile;
using PulsarModLoader.Content.Components.Reactor;
using PulsarModLoader.Content.Components.Shield;
using PulsarModLoader.Content.Components.WarpDriveProgram;
using UnityEngine;

namespace ExpandedGalaxy
{
    internal class Relic
    {
        internal static ulong RelicData = 0U;
        static readonly int MaxRelicType = 11;
        private static Color relicColor = new Color(85f / 255f, 0f, 255f / 255f);
    
        public static PLShipComponent GenerateRelic(int seed)
        {
            PLRand rand = new PLRand(seed);
            int type = rand.Next() % MaxRelicType;
            int num = 0;
            while (num < MaxRelicType)
            {
                if (HasRelic(type))
                {
                    type = rand.Next(MaxRelicType);
                    ++num;
                }
                else
                    break;
            }
            if (num == MaxRelicType)
                return new PLScrapCargo();
            PLShipComponent component;
            switch (type)
            {
                case 1:
                    component = PLHull.CreateHullFromHash(HullModManager.Instance.GetHullIDFromName("Juggernaut Hull"), 0, 0);
                    break;
                case 2:
                    component = PLSensorDish.CreateSensorDishFromHash(1, 0, 0);
                    break;
                case 3:
                    component = PLReactor.CreateReactorFromHash(ReactorModManager.Instance.GetReactorIDFromName("Dark-Matter Reactor"), 0, 0);
                    break;
                case 4:
                    component = PLCaptainsChair.CreateCaptainsChairFromHash(CaptainsChairModManager.Instance.GetCaptainsChairIDFromName("Seat of the Surveyor"), 0, 0);
                    component.SubTypeData = 0;
                    break;
                case 5:
                    component = PLMegaTurret.CreateMainTurretFromHash(MegaTurretModManager.Instance.GetMegaTurretIDFromName("Imperial Glaive"), 0, 0);
                    break;
                case 6:
                    component = PLWarpDriveProgram.CreateWarpDriveProgramFromHash(WarpDriveProgramModManager.Instance.GetWarpDriveProgramIDFromName("Digital Earthquake"), 0, 0);
                    break;
                case 7:
                    component = PLWarpDriveProgram.CreateWarpDriveProgramFromHash(WarpDriveProgramModManager.Instance.GetWarpDriveProgramIDFromName("Corruption [VIRUS]"), 0, 0);
                    break;
                case 8:
                    component = PLCPU.CreateCPUFromHash(CPUModManager.Instance.GetCPUIDFromName("Sylvassi Shield Charger"), 0, 0);
                    component.SubTypeData = 0;
                    break;
                case 9:
                    component = PLInertiaThruster.CreateInertiaThrusterFromHash(InertiaThrusterModManager.Instance.GetInertiaThrusterIDFromName("Integrated Stabilizer Thruster"), 0, 0);
                    break;
                case 10:
                    component = PLTrackerMissile.CreateTrackerMissileFromHash(MissileModManager.Instance.GetMissileIDFromName("GKS-Ex27 \"Soul Siphon\""), 0, 0);
                    break;
                default:
                    component = PLShieldGenerator.CreateShieldGeneratorFromHash(ShieldModManager.Instance.GetShieldIDFromName("Anti-Matter Suspension Field"), 0, 0);
                    break;
            }
            RelicData = RelicData | (1U << type);
            return component;
        }

        public static bool HasRelic(int relicType)
        {
            return (RelicData & (1U << relicType)) > 0U;
        }

        public static bool GetIsRelic(PLWare ware)
        {
            PLShieldGenerator shield = ware as PLShieldGenerator;
            if (shield != null)
            {
                if (shield.SubType == ShieldModManager.Instance.GetShieldIDFromName("Anti-Matter Suspension Field"))
                {
                    return true;
                }
            }
            PLHull hull = ware as PLHull;
            if (hull != null)
            {
                if (hull.SubType == HullModManager.Instance.GetHullIDFromName("Juggernaut Hull"))
                {
                    return true;
                }
            }
            PLReactor reactor = ware as PLReactor;
            if (reactor != null)
            {
                if (reactor.SubType == ReactorModManager.Instance.GetReactorIDFromName("Dark-Matter Reactor"))
                {
                    return true;
                }
            }
            PLCaptainsChair chair = ware as PLCaptainsChair;
            if (chair != null)
            {
                if (chair.SubType == CaptainsChairModManager.Instance.GetCaptainsChairIDFromName("Seat of the Surveyor"))
                {
                    return true;
                }
            }
            PLMegaTurret megaTurret = ware as PLMegaTurret;
            if (megaTurret != null)
            {
                if (megaTurret.SubType == MegaTurretModManager.Instance.GetMegaTurretIDFromName("Imperial Glaive"))
                {
                    return true;
                }
            }
            PLTurret turret = ware as PLTurret;
            if (turret != null)
            {
                if (turret.ActualSlotType == ESlotType.E_COMP_AUTO_TURRET && turret.SubType == AutoTurretModManager.Instance.GetAutoTurretIDFromName("Ancient Auto Laser Turret"))
                {
                    return true;
                }
            }
            PLSensorDish sensorDish = ware as PLSensorDish;
            if (sensorDish != null)
            {
                if (sensorDish.SubType == 1)
                {
                    return true;
                }
            }
            PLCPU cpu = ware as PLCPU;
            if (cpu != null)
            {
                if (cpu.SubType == CPUModManager.Instance.GetCPUIDFromName("Sylvassi Shield Charger"))
                {
                    return true;
                }
            }
            PLWarpDriveProgram warpDriveProgram = ware as PLWarpDriveProgram;
            if (warpDriveProgram != null)
            {
                if (warpDriveProgram.SubType == WarpDriveProgramModManager.Instance.GetWarpDriveProgramIDFromName("Digital Earthquake"))
                    return true;
                else if (warpDriveProgram.SubType == WarpDriveProgramModManager.Instance.GetWarpDriveProgramIDFromName("Corruption [VIRUS]"))
                    return true;
                else if (warpDriveProgram.SubType == WarpDriveProgramModManager.Instance.GetWarpDriveProgramIDFromName("Syber Swap"))
                    return true;
            }
            PLInertiaThruster inertiaThruster = ware as PLInertiaThruster;
            if (inertiaThruster != null)
            {
                if (inertiaThruster.SubType == InertiaThrusterModManager.Instance.GetInertiaThrusterIDFromName("Integrated Stabilizer Thruster"))
                    return true;
            }
            PLDistressSignal signal = ware as PLDistressSignal;
            if (signal != null)
            {
                if (signal.SubType == 4)
                {
                    return true;
                }
            }
            PLTrackerMissile missile = ware as PLTrackerMissile;
            if (missile != null)
            {
                if (missile.SubType == MissileModManager.Instance.GetMissileIDFromName("GKS-Ex27 \"Soul Siphon\""))
                {
                    return true;
                }
            }
            return false;
        }

        public static Color GetRelicColor()
        {
            return relicColor;
        }
    }
}
