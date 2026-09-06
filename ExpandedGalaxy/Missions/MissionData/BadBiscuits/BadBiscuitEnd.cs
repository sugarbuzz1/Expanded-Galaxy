using HarmonyLib;
using PulsarModLoader.Content.Components.MegaTurret;
using PulsarModLoader.Content.Components.Shield;
using PulsarModLoader.Content.Components.Turret;
using PulsarModLoader.Content.Components.WarpDriveProgram;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLPickupMissionBase), "OnMissionSuccess")]
    internal class BadBiscuitEnd
    {
        private static void Postfix(PLPickupMissionBase __instance, ref PickupMissionData ___pickupMissionData)
        {
            if (___pickupMissionData == null || ___pickupMissionData.MissionID != 8000012)
                return;
            if (!PhotonNetwork.isMasterClient || PLGlobal.Instance.Galaxy == null)
                return;
            if (PLGlobal.Instance.Galaxy.GetSectorOfVisualIndication(ESectorVisualIndication.FLUFFY_FACTORY_ABANDONED) == null)
            {
                PulsarModLoader.Utilities.Logger.Info("Couldn't find sector of type FLUFFY_FACTORY_ABANDONED!");
                return;
            }
            PLGlobal.Instance.Galaxy.GetSectorOfVisualIndication(ESectorVisualIndication.FLUFFY_FACTORY_ABANDONED).MissionSpecificID = -1;
            PLPersistantShipInfo shipInfo = new PLPersistantShipInfo(EShipType.E_CARRIER, 3, PLGlobal.Instance.Galaxy.GetSectorOfVisualIndication(ESectorVisualIndication.FLUFFY_FACTORY_ABANDONED));
            shipInfo.SelectedActorID = "ExGal_FBCarrier";
            TraderPersistantDataEntry traderPersistantDataEntry = new TraderPersistantDataEntry();
            traderPersistantDataEntry.ServerAddWare(PLShipComponent.CreateShipComponentFromHash((int)PLShipComponent.createHashFromInfo((int)ESlotType.E_COMP_SHLD, (int)EShieldGeneratorType.E_GRIMCUTLASS_SHIELDS, 0, 0, (int)ESlotType.E_COMP_CARGO)));
            traderPersistantDataEntry.ServerAddWare(PLShipComponent.CreateShipComponentFromHash((int)PLShipComponent.createHashFromInfo((int)ESlotType.E_COMP_SHLD, (int)ShieldModManager.Instance.GetShieldIDFromName("Reflector Shield Generator"), 1, 0, (int)ESlotType.E_COMP_CARGO)));
            traderPersistantDataEntry.ServerAddWare(PLShipComponent.CreateShipComponentFromHash((int)PLShipComponent.createHashFromInfo((int)ESlotType.E_COMP_WARP, (int)EWarpDriveType.E_OLDWARS_SUPER_JUMPER, 0, 0, (int)ESlotType.E_COMP_CARGO)));
            traderPersistantDataEntry.ServerAddWare(PLShipComponent.CreateShipComponentFromHash((int)PLShipComponent.createHashFromInfo((int)ESlotType.E_COMP_TURRET, (int)TurretModManager.Instance.GetTurretIDFromName("Particle Lance"), 0, 0, (int)ESlotType.E_COMP_CARGO)));
            traderPersistantDataEntry.ServerAddWare(PLShipComponent.CreateShipComponentFromHash((int)PLShipComponent.createHashFromInfo((int)ESlotType.E_COMP_TURRET, (int)TurretModManager.Instance.GetTurretIDFromName("Seeker Turret"), 0, 0, (int)ESlotType.E_COMP_CARGO)));
            traderPersistantDataEntry.ServerAddWare(PLShipComponent.CreateShipComponentFromHash((int)PLShipComponent.createHashFromInfo((int)ESlotType.E_COMP_MAINTURRET, (int)MegaTurretModManager.Instance.GetMegaTurretIDFromName("WD Long Range"), 0, 0, (int)ESlotType.E_COMP_CARGO)));
            traderPersistantDataEntry.ServerAddWare(PLShipComponent.CreateShipComponentFromHash((int)PLShipComponent.createHashFromInfo((int)ESlotType.E_COMP_SALVAGE_SYSTEM, (int)EExtractorType.E_PT_EXTRACTOR, 0, 0, (int)ESlotType.E_COMP_CARGO)));
            traderPersistantDataEntry.ServerAddWare(PLShipComponent.CreateShipComponentFromHash((int)PLShipComponent.createHashFromInfo((int)ESlotType.E_COMP_PROGRAM, (int)WarpDriveProgramModManager.Instance.GetWarpDriveProgramIDFromName("Special Training [VIRUS]"), 0, 0, (int)ESlotType.E_COMP_CARGO)));
            traderPersistantDataEntry.ServerAddWare(PLShipComponent.CreateShipComponentFromHash((int)PLShipComponent.createHashFromInfo((int)ESlotType.E_COMP_FB_RECIPE, (int)FBRecipe.E_SPICY, 0, 0, (int)ESlotType.E_COMP_CARGO)));
            traderPersistantDataEntry.ServerAddWare(PLShipComponent.CreateShipComponentFromHash((int)PLShipComponent.createHashFromInfo((int)ESlotType.E_COMP_FB_RECIPE, (int)FBRecipe.E_GARLIC, 0, 0, (int)ESlotType.E_COMP_CARGO)));
            traderPersistantDataEntry.ServerAddWare(PLShipComponent.CreateShipComponentFromHash((int)PLShipComponent.createHashFromInfo((int)ESlotType.E_COMP_FB_RECIPE, (int)FBRecipe.E_LUCKY, 0, 0, (int)ESlotType.E_COMP_CARGO)));
            traderPersistantDataEntry.ServerAddWare(PLShipComponent.CreateShipComponentFromHash((int)PLShipComponent.createHashFromInfo((int)ESlotType.E_COMP_FB_RECIPE, (int)FBRecipe.E_SUGAR, 0, 0, (int)ESlotType.E_COMP_CARGO)));
            traderPersistantDataEntry.ServerAddWare(PLShipComponent.CreateShipComponentFromHash((int)PLShipComponent.createHashFromInfo((int)ESlotType.E_COMP_TRACKERMISSILE, (int)ETrackerMissileType.FB_MISSILE, 0, 0, (int)ESlotType.E_COMP_CARGO)));
            traderPersistantDataEntry.ServerAddWare(RelicCaravan.GetSpecialOffer());
            traderPersistantDataEntry.ServerAddWare(RelicCaravan.GetSpecialOffer());
            for (int i = 0; i < 5; i++)
                traderPersistantDataEntry.ServerAddWare(PLShipComponent.CreateRandom());
            shipInfo.OptionalTPDE = traderPersistantDataEntry;
            BadBiscuits.carrierData = traderPersistantDataEntry;
            PLServer.Instance.AllPSIs.Add(shipInfo);
        }
    }

}

