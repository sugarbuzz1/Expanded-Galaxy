using CodeStage.AntiCheat.ObscuredTypes;
using HarmonyLib;

namespace ExpandedGalaxy
{
    internal class Senor
    {
        [HarmonyPatch(typeof(PLSensor), "CreateSensorFromHash")]
        internal class CreateSensorFix
        {
            private static bool Prefix(int inSubType, int inLevel, int inSubTypeData, ref PLShipComponent __result)
            {
                switch (inSubType)
                {
                    case 1:
                        __result = new Sensor_LF(inLevel);
                        break;
                    case 2:
                        __result = new Sensor_GV(inLevel);
                        break;
                    case 3:
                        __result = new Sensor_NT(inLevel);
                        break;
                    case 4:
                        __result = new Sensor_PT(inLevel);
                        break;
                    case 5:
                        __result = new Sensor_DC(inLevel);
                        break;
                    default:
                        __result = new PLSensor_EM(inLevel);
                        break;
                }
                return false;
            }
        }

        public class Sensor_LF : PLSensor
        {
            public Sensor_LF(int inLevel = 0)
            {
                this.Name = "Life Form Sensor";
                this.SensorShortName = "Lifeform";
                this.SensorLogName = "LF";
                this.Desc = "Provides information on the life forms abord the target ship.";
                this.Detection = 1f;
                this.Level = inLevel;
                this.m_MarketPrice = (ObscuredInt)1400;
                this.SubType = 1;
                this.SysInstConduit = 0;
            }

            private void UpdateMaxPowerWatts() => this.m_MaxPowerUsage_Watts = 1200f * this.LevelMultiplier(0.5f);

            public override void Tick()
            {
                this.UpdateMaxPowerWatts();
                base.Tick();
            }

            public override void AddStats(PLShipStats inStats)
            {
                inStats.LFDetection += this.Detection * this.GetPowerPercentInput() * this.LevelMultiplier(0.2f);
            }
        }

        public class Sensor_GV : PLSensor
        {
            public Sensor_GV(int inLevel = 0)
            {
                this.Name = "Gravitic Sensor";
                this.SensorShortName = "Gravitational";
                this.SensorLogName = "GV";
                this.Desc = "Detects the gravitational fields of heavy ships within a moderate range.";
                this.Detection = 1f;
                this.Level = inLevel;
                this.m_MarketPrice = (ObscuredInt)2200;
                this.SubType = 2;
                this.SysInstConduit = 0;
            }

            private void UpdateMaxPowerWatts() => this.m_MaxPowerUsage_Watts = 1800f * this.LevelMultiplier(0.5f);

            public override void Tick()
            {
                this.UpdateMaxPowerWatts();
                base.Tick();
            }

            public override void AddStats(PLShipStats inStats)
            {
                inStats.QTDetection += this.Detection * this.GetPowerPercentInput() * this.LevelMultiplier(0.2f);
            }
        }

        public class Sensor_NT : PLSensor
        {
            public Sensor_NT(int inLevel = 0)
            {
                this.Name = "Neutrino Sensor";
                this.SensorShortName = "Neutrino";
                this.SensorLogName = "NT";
                this.Desc = "Detects the neutrino particles emmited by a ship's reactor and thrusters.";
                this.Detection = 1f;
                this.Level = inLevel;
                this.m_MarketPrice = (ObscuredInt)1700;
                this.SubType = 3;
                this.SysInstConduit = 0;
            }

            private void UpdateMaxPowerWatts() => this.m_MaxPowerUsage_Watts = 1600f * this.LevelMultiplier(0.5f);

            public override void Tick()
            {
                this.UpdateMaxPowerWatts();
                base.Tick();
            }

            public override void AddStats(PLShipStats inStats)
            {
                inStats.RADetection += this.Detection * this.GetPowerPercentInput() * this.LevelMultiplier(0.2f);
            }
        }

        public class Sensor_PT : PLSensor
        {
            public Sensor_PT(int inLevel = 0)
            {
                this.Name = "Polytech Sensor";
                this.SensorShortName = "Polytech";
                this.SensorLogName = "PT";
                this.Desc = "A finely tuned device that combines the effects of electromagnetic, gravitic, and neutrino sensors.";
                this.Experimental = true;
                this.Detection = 0.8f;
                this.Level = inLevel;
                this.m_MarketPrice = (ObscuredInt)1700;
                this.SubType = 4;
                this.SysInstConduit = 0;
            }

            private void UpdateMaxPowerWatts() => this.m_MaxPowerUsage_Watts = 1600f * this.LevelMultiplier(0.5f);

            public override void Tick()
            {
                this.UpdateMaxPowerWatts();
                base.Tick();
            }

            public override void AddStats(PLShipStats inStats)
            {
                inStats.EMDetection += this.Detection * this.GetPowerPercentInput() * this.LevelMultiplier(0.2f);
                inStats.QTDetection += this.Detection * this.GetPowerPercentInput() * this.LevelMultiplier(0.2f);
                inStats.RADetection += this.Detection * this.GetPowerPercentInput() * this.LevelMultiplier(0.2f);
            }
        }

        public class Sensor_DC : PLSensor
        {
            public Sensor_DC(int inLevel = 0)
            {
                this.Name = "Dark-Matter Detection Cell";
                this.SensorShortName = "Dark-Matter";
                this.SensorLogName = "DC";
                this.Desc = "A complex sensor that reveals all ships and boosts targeting systems against them within the current sector upon completion of a sensor sweep. They can run, but they cannot hide...";
                this.Detection = 0f;
                this.Level = inLevel;
                this.m_MarketPrice = (ObscuredInt)10000;
                this.SubType = 5;
                this.SysInstConduit = 0;
            }

            private void UpdateMaxPowerWatts() => this.m_MaxPowerUsage_Watts = 2000f * this.LevelMultiplier(0.5f);

            public override void Tick()
            {
                this.UpdateMaxPowerWatts();
                base.Tick();
            }

            public override void AddStats(PLShipStats inStats)
            {
            }
        }
    }
}
