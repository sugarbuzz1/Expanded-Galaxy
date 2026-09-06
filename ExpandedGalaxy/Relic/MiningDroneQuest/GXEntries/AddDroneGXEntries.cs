using HarmonyLib;
using System.Collections.Generic;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLGlobal), "LoadGXFile")]
    internal class AddDroneGXEntries
    {
        private static void Postfix(PLGlobal __instance)
        {
            __instance.AllGeneralInfos.Add(new GeneralInfo
            {
                Name = "Mining Drone",
                Name_lower = "mining drone",
                ID = 29,
                Desc = "Drone used for extracting minerals from space asteroids.\n\nThey appear to be communicating with some uncharted hub sector within this galaxy. They are equipped with powerful laser technology.\n\nMining drones are not hostile unless provoked.",
                MyStats = new List<GeneralInfoStat> { new GeneralInfoStat
                {
                    Left = "Type",
                    Right = "Unmanned Drone"
                },
                new GeneralInfoStat
                {
                    Left = "Faction",
                    Right = "Unknown"
                },
                new GeneralInfoStat
                {
                    Left = "Service",
                    Right = "Resource Extraction"
                }
                }
            });
            __instance.AllGeneralInfos.Add(new GeneralInfo
            {
                Name = "Escort Drone",
                Name_lower = "escort drone",
                ID = 30,
                Desc = "Drone used to protect mining drone operations.\n\nThey appear to be communicating with some uncharted hub sector within this galaxy. They reinforce other mining and escort drones in distress.\n\nEscort drones are not hostile unless a drone is provoked.",
                MyStats = new List<GeneralInfoStat> { new GeneralInfoStat
                {
                    Left = "Type",
                    Right = "Unmanned Drone"
                },
                new GeneralInfoStat
                {
                    Left = "Faction",
                    Right = "Unknown"
                },
                new GeneralInfoStat
                {
                    Left = "Service",
                    Right = "Asset Protection"
                }
                }
            });
            __instance.AllGeneralInfos.Add(new GeneralInfo
            {
                Name = "Guardian Drone",
                Name_lower = "guardian drone",
                ID = 31,
                Desc = "Drone seen protecting the mining drone hub world in this galaxy.\n\nThey are outfitted with powerful weaponry designed to debilitate attacking ships. Each drone is equipped with a signal jammer that blocks teleportation to thier world's surface.\n\nGuardian drones are always hostile.",
                MyStats = new List<GeneralInfoStat> { new GeneralInfoStat
                {
                    Left = "Type",
                    Right = "Unmanned Drone"
                },
                new GeneralInfoStat
                {
                    Left = "Faction",
                    Right = "Unknown"
                },
                new GeneralInfoStat
                {
                    Left = "Service",
                    Right = "Hub World Protector"
                }
                }
            });
        }
    }
}
