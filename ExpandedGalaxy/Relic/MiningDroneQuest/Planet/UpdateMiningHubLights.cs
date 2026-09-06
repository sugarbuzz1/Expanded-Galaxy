using HarmonyLib;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLPersistantPlanetEncounterInstance), "Update")]
    internal class UpdateMiningHubLights
    {
        private static void Postfix(PLPersistantPlanetEncounterInstance __instance)
        {
            if (PLEncounterManager.Instance == null || PLEncounterManager.Instance.PlayerShip == null || PLEncounterManager.Instance.PlayerShip.Get_IsInWarpMode())
                return;
            if (!(__instance is PLLavaPlanet2Encounter))
                return;
            if (PLNetworkManager.Instance.CurrentGame == null || !__instance.GameInitWithHubID || PLEncounterManager.Instance.GetCPEI() != __instance)
                return;
            bool flag = false;
            foreach (MiningDroneBossFlag miningDroneBossFlag in MiningDroneBossFlag.AllMiningDroneBossFlags)
            {
                PLShipInfoBase plShipInfoBase = miningDroneBossFlag.ShipStats.Ship;
                if (plShipInfoBase == null)
                    continue;
                if (plShipInfoBase.ShipTypeID == EShipType.E_WDDRONE2)
                {
                    flag = true;
                    break;
                }
            }
            if (flag && !PLMusic.Instance.SpecialMusicPlaying)
                PLMusic.Instance.PlayMusic("mx_infected_commander", true, false, true, true);
            if (!flag && PLMusic.Instance.SpecialMusicPlaying)
                PLMusic.Instance.StopCurrentMusic();
            PLGamePlanet currentGame = PLNetworkManager.Instance.CurrentGame as PLGamePlanet;
            PLInterior interior = currentGame.PlanetRoot.GetComponentInChildren<PLInterior>();
            if (interior == null)
                return;
            for (int i = 5; i < interior.Lights.Length; i++)
            {
                if (i == 5 && interior.Lights[5].gameObject != null)
                    interior.Lights[5].gameObject.SetActive(MiningDroneQuest.dronesActive);
                if (i != 6 && i != 7)
                {
                    if (interior.Lights[i].gameObject != null && interior.Lights[i].transform != null)
                    {
                        if (interior.Lights[i].transform.parent != null && interior.Lights[i].transform.parent.name.Contains("Carrier_Exterior_Light") && interior.Lights[i].name == "Point light")
                        {
                            interior.Lights[i].gameObject.SetActive(MiningDroneQuest.dronesActive);
                        }
                    }
                }
            }
            interior.RootObj.transform.GetChild(0).GetChild(163).gameObject.SetActive(MiningDroneQuest.dronesActive);
            if (MiningDroneQuest.dronesActive && interior.AmbienceSFX == "")
            {
                interior.AmbienceSFX = "sx_planet_terra_station_hum";
                if (PLNetworkManager.Instance.LocalPlayer.CurrentInterior == interior)
                    PLMusic.PostEvent("play_" + interior.AmbienceSFX, interior.gameObject);

            }
            else if (!MiningDroneQuest.dronesActive && interior.AmbienceSFX != "")
            {
                if (PLNetworkManager.Instance.LocalPlayer.CurrentInterior == interior)
                    PLMusic.PostEvent("stop_" + interior.AmbienceSFX, interior.gameObject);
                interior.AmbienceSFX = "";
            }
        }
    }
}
