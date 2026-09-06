using PulsarModLoader;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;


namespace ExpandedGalaxy.SensorDish
{
    internal class SensorDishModManager
    {
        public readonly int VanillaSensorDishMaxType;
        private static SensorDishModManager m_instance;
        public readonly List<SensorDishMod> SensorDishTypes = new List<SensorDishMod>();
        internal readonly Dictionary<int, int> ScreenInfoData = new Dictionary<int, int>();

        internal static void Init()
        {
            SensorDishModManager.m_instance = new SensorDishModManager();
        }

        public static SensorDishModManager Instance
        {
            get
            {
                if (SensorDishModManager.m_instance == null)
                    SensorDishModManager.m_instance = new SensorDishModManager();
                return SensorDishModManager.m_instance;
            }
        }

        private SensorDishModManager()
        {
            this.VanillaSensorDishMaxType = Enum.GetValues(typeof(ESensorDishType)).Length;
            PulsarModLoader.Utilities.Logger.Info(string.Format("MaxTypeint = {0}", (object)(this.VanillaSensorDishMaxType - 1)));
            this.SensorDishTypes.Add(new VanillaSensorDishMod());
            foreach (PulsarMod allMod in ModManager.Instance.GetAllMods())
            {
                Assembly assembly = allMod.GetType().Assembly;
                System.Type type1 = typeof(SensorDishMod);
                foreach (System.Type type2 in assembly.GetTypes())
                {
                    if (type1.IsAssignableFrom(type2) && !type2.IsInterface && !type2.IsAbstract)
                    {
                        PulsarModLoader.Utilities.Logger.Info("Loading Sensor Dish from assembly");
                        SensorDishMod instance = (SensorDishMod)Activator.CreateInstance(type2);
                        if (this.GetSensorDishIDFromName(instance.Name) == -1)
                        {
                            this.SensorDishTypes.Add(instance);
                            PulsarModLoader.Utilities.Logger.Info(string.Format("Added Sensor Dish: '{0}' with ID '{1}'", (object)instance.Name, (object)this.GetSensorDishIDFromName(instance.Name)));
                        }
                        else
                            PulsarModLoader.Utilities.Logger.Info("Could not add Sensor Dish from " + allMod.Name + " with the duplicate name of '" + instance.Name + "'");
                    }
                }
            }
        }

        public int GetSensorDishIDFromName(string SensorDishName)
        {
            for (int index = 0; index < this.SensorDishTypes.Count; ++index)
            {
                if (this.SensorDishTypes[index].Name == SensorDishName)
                    return index;
            }
            return -1;
        }

        public static bool IsSensorWeaknessActiveModded(PLShipInfoBase shipInfo, int weaknessID, int subTypeID = -1)
        {
            int[] SensorWeaknessActiveStartTime = shipInfo.SensorWeaknessActiveStartTime;
            int[] SensorWeaknessData = shipInfo.SensorWeaknessData;
            if (SensorWeaknessActiveStartTime == null)
                return false;
            if (SensorWeaknessData == null)
                return false;
            return weaknessID >= 0 && SensorWeaknessActiveStartTime.Length > weaknessID && SensorWeaknessData[weaknessID] == subTypeID && PLGlobal.WithinTimeLimit(PLServer.Instance.GetEstimatedServerMs(), SensorWeaknessActiveStartTime[weaknessID], PLShipInfoBase.GetSensorWeaknessTimeInSeconds((ESensorWeakness)weaknessID) * 1000);
        }

        internal UITexture[] CreateButtonsFromSubType(PLScientistSensorScreen screen, int subType)
        {
            UITexture[] uITextures = new UITexture[6];
            Transform WeaknessRootTransform = screen.ShipInfoScreen_WeaknessRoot.transform;
            float num = 33f;
            if (subType >= this.SensorDishTypes.Count)
                subType = 0;
            SensorDishMod mod = this.SensorDishTypes[subType];
            uITextures[0] = screen.CreateTexture(mod.SensorDishAbilities[0].IconTexture, new Vector3(175f, -70f), new Vector2(num, num), Color.black, WeaknessRootTransform, UIWidget.Pivot.TopLeft);
            uITextures[1] = screen.CreateTexture(mod.SensorDishAbilities[1].IconTexture, new Vector3(175f, -115f), new Vector2(num, num), Color.black, WeaknessRootTransform, UIWidget.Pivot.TopLeft);
            uITextures[2] = screen.CreateTexture(mod.SensorDishAbilities[2].IconTexture, new Vector3(175f, -160f), new Vector2(num, num), Color.black, WeaknessRootTransform, UIWidget.Pivot.TopLeft);
            uITextures[3] = screen.CreateButtonEditable("weaknessScanCyberDef", mod.SensorDishAbilities[0].IconTexture, new Vector3(175f, -70f), new Vector2(num, num), Color.white, WeaknessRootTransform, UIWidget.Pivot.TopLeft);
            uITextures[4] = screen.CreateButtonEditable("weaknessScanShieldWeakPoint", mod.SensorDishAbilities[1].IconTexture, new Vector3(175f, -115f), new Vector2(num, num), Color.white, WeaknessRootTransform, UIWidget.Pivot.TopLeft);
            uITextures[5] = screen.CreateButtonEditable("weaknessScanReactor", mod.SensorDishAbilities[2].IconTexture, new Vector3(175f, -160f), new Vector2(num, num), Color.white, WeaknessRootTransform, UIWidget.Pivot.TopLeft);

            uITextures[3].type = UISprite.Type.Filled;
            uITextures[4].type = UISprite.Type.Filled;
            uITextures[5].type = UISprite.Type.Filled;
            return uITextures;
        }
    }
}
