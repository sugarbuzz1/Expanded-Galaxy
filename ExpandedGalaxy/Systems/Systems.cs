using CodeStage.AntiCheat.ObscuredTypes;
using PulsarModLoader.Content.Components.MissionShipComponent;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace ExpandedGalaxy
{
    public class Systems
    {
        public static async void TickDamage(
          int ticks,
          PLSpaceTarget target,
          float dmg,
          bool bottomHit,
          EDamageType dmgType,
          int SystemTargetID,
          PLShipInfoBase attackingShip,
          int turretID)
        {
            int i;
            if (target is PLShipInfoBase)
            {
                PLShipInfoBase targetShip = (PLShipInfoBase)target;
                for (i = ticks; i > 0; --i)
                {
                    if ((!((UnityEngine.Object)targetShip != (UnityEngine.Object)null) || targetShip.MyStats == null || !((UnityEngine.Object)targetShip.MyStats.Ship != (UnityEngine.Object)null)) && !targetShip.HasBeenDestroyed)
                        return;
                    await Task.Delay(1000);
                    double damage = (double)targetShip.TakeDamage(dmg, bottomHit, dmgType, 1f, SystemTargetID, attackingShip, turretID);
                }
                targetShip = (PLShipInfoBase)null;
            }
            else
            {
                for (i = ticks; i > 0 && (UnityEngine.Object)target != (UnityEngine.Object)null && (double)target.GetHPAlphaCurrent() > 0.0; --i)
                {
                    await Task.Delay(1000);
                    target.TakeDamage(dmg);
                }
            }
        }


        public static bool IsPlayerPiloting(int playerID)
        {
            if (playerID == -1)
                return false;
            foreach (PLShipInfoBase infoBase in UnityEngine.Object.FindObjectsOfType<PLShipInfoBase>())
            {
                if (infoBase.GetCurrentShipControllerPlayerID() == playerID)
                    return true;
            }
            return false;
        }

        internal static bool IsCargoScrappable(CargoObjectDisplay cargo)
        {
            if (cargo.DisplayedItem.ActualSlotType == ESlotType.E_COMP_MISSION_COMPONENT && (cargo.DisplayedItem.SubType == MissionShipComponentModManager.Instance.GetMissionShipComponentIDFromName("Ammunition Cache") || cargo.DisplayedItem.SubType == MissionShipComponentModManager.Instance.GetMissionShipComponentIDFromName("Reward") || cargo.DisplayedItem.SubType == MissionShipComponentModManager.Instance.GetMissionShipComponentIDFromName("Data Cache")))
                return true;
            if (cargo.DisplayedItem.ActualSlotType == ESlotType.E_COMP_SCRAP)
                return true;
            if (PLServer.GetCurrentSector() != null && PLServer.GetCurrentSector().VisualIndication == ESectorVisualIndication.SPACE_SCRAPYARD)
            {
                if (PLServer.Instance.CurrentCrewCredits >= (cargo.DisplayedItem.Level + 1) * 800)
                    return !cargo.DisplayedItem.ImportantItem && !cargo.DisplayedItem.Contraband && !Relic.GetIsRelic(cargo.DisplayedItem);
            }
            return false;
        }

        public static PLPlayer GetPlayerFromPhotonPlayer(PhotonPlayer photon)
        {
            foreach (PLPlayer allPlayer in PLServer.Instance.AllPlayers)
            {
                if ((UnityEngine.Object)allPlayer != (UnityEngine.Object)null && allPlayer.GetPhotonPlayer() != null && allPlayer.GetPhotonPlayer().Equals(photon))
                    return allPlayer;
            }
            return (PLPlayer)null;
        }

        internal static PLShipInfoBase TurretTargetingUI(PLTurret turret)
        {
            if (PLUIOutsideWorldUI.Instance != null && PLCameraSystem.Instance.GetModeString() == "Turret")
            {
                float greatestDot = 0f;
                PLShipInfoBase shipInfo = null;
                foreach (PLShipInfoBase pLShipInfoBase in UnityEngine.Object.FindObjectsOfType<PLShipInfoBase>())
                {
                    if (pLShipInfoBase != turret.ShipStats.Ship)
                    {
                        Vector3 dir = (pLShipInfoBase.Exterior.transform.position - turret.ShipStats.Ship.Exterior.transform.position).normalized;
                        float dot = Vector3.Dot(dir, turret.TurretInstance.RefJoint.forward.normalized);
                        if (dot > 0.85f && dot > greatestDot)
                        {
                            greatestDot = dot;
                            shipInfo = pLShipInfoBase;
                        }
                    }
                }
                if (shipInfo != null)
                {
                    PLUIOutsideWorldUI.Instance.RequestKeenUIElement(shipInfo.Exterior.transform, "Target");
                    return shipInfo;
                }
            }
            return null;
        }

        public static async void PhaseAway(PLShipInfo ship)
        {
            if (!((UnityEngine.Object)ship != (UnityEngine.Object)null))
            {
                ship = (PLShipInfo)null;
            }
            else
            {
                float StartedPhasing = Time.time;
                UnityEngine.Object.Instantiate<GameObject>(PLGlobal.Instance.PhasePS, ship.Exterior.transform.position, Quaternion.identity);
                GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(PLGlobal.Instance.PhaseTrailPS, ship.Exterior.transform.position, Quaternion.identity);
                if ((UnityEngine.Object)gameObject != (UnityEngine.Object)null)
                {
                    PLPhaseTrail component = gameObject.GetComponent<PLPhaseTrail>();
                    if ((UnityEngine.Object)component != (UnityEngine.Object)null)
                    {
                        component.StartPos = ship.Exterior.transform.position;
                        component.End = ship.Exterior.transform;
                    }
                    component = (PLPhaseTrail)null;
                }
                foreach (PLShipInfoBase plshipInfoBase in PLEncounterManager.Instance.AllShips.Values)
                {
                    if ((UnityEngine.Object)plshipInfoBase != (UnityEngine.Object)null && (UnityEngine.Object)plshipInfoBase.MySensorObjectShip != (UnityEngine.Object)null)
                    {
                        PLSensorObjectCacheData plsensorObjectCacheData = plshipInfoBase.MySensorObjectShip.IsDetectedBy_CachedInfo((PLShipInfoBase)ship);
                        if (plsensorObjectCacheData != null)
                        {
                            plsensorObjectCacheData.LastDetectedCheckTime = 0.0f;
                            plsensorObjectCacheData.IsDetected = false;
                        }
                        plsensorObjectCacheData = (PLSensorObjectCacheData)null;
                    }
                }
                Systems.DelayedEndPhasePS(ship);
                List<MeshRenderer> exteriorRenderers = ship.ExteriorRenderers;
                MeshRenderer[] hullplanting = ship.HullPlatingRenderers;
                List<PLShipComponent> componentsOfType = ship.MyStats.GetComponentsOfType(ESlotType.E_COMP_TURRET);
                componentsOfType.AddRange((IEnumerable<PLShipComponent>)ship.MyStats.GetComponentsOfType(ESlotType.E_COMP_MAINTURRET));
                componentsOfType.AddRange((IEnumerable<PLShipComponent>)ship.MyStats.GetComponentsOfType(ESlotType.E_COMP_AUTO_TURRET));
                ship.Exterior.transform.position = ship.Exterior.transform.position + ship.Exterior.transform.forward * 200f;
                PLMusic.PostEvent("play_sx_ship_enemy_phasedrone_warp", ship.Exterior);
                while ((double)Time.time - (double)StartedPhasing < 1.0)
                {
                    ship.MyStats.EMSignature = 0.0f;
                    ship.MyStats.CanBeDetected = (ObscuredBool)false;
                    foreach (Renderer rend in exteriorRenderers)
                    {
                        if ((UnityEngine.Object)rend != (UnityEngine.Object)null)
                            rend.enabled = false;
                    }
                    MeshRenderer[] meshRendererArray = hullplanting;
                    for (int index = 0; index < meshRendererArray.Length; ++index)
                    {
                        Renderer rend = (Renderer)meshRendererArray[index];
                        if ((UnityEngine.Object)rend != (UnityEngine.Object)null)
                            rend.enabled = false;
                        rend = (Renderer)null;
                    }
                    meshRendererArray = (MeshRenderer[])null;
                    foreach (PLShipComponent comp in componentsOfType)
                    {
                        PLTurret turret = comp as PLTurret;
                        if (turret != null && (UnityEngine.Object)turret.TurretInstance != (UnityEngine.Object)null)
                        {
                            Renderer[] rendererArray = turret.TurretInstance.MyMainRenderers;
                            for (int index = 0; index < rendererArray.Length; ++index)
                            {
                                Renderer rend = rendererArray[index];
                                rend.enabled = false;
                                rend = (Renderer)null;
                            }
                            rendererArray = (Renderer[])null;
                        }
                        turret = (PLTurret)null;
                    }
                    if ((ship is PLFluffyShipInfo || ship is PLFluffyShipInfo2) && (UnityEngine.Object)(ship as PLFluffyShipInfo).MyVisibleBomb != (UnityEngine.Object)null)
                        (ship as PLFluffyShipInfo).MyVisibleBomb.gameObject.SetActive(false);
                    ship.MyStats.ThrustOutputCurrent = 0.0f;
                    ship.MyStats.ManeuverThrustOutputCurrent = 0.0f;
                    ship.MyStats.InertiaThrustOutputCurrent = 0.0f;
                    if ((UnityEngine.Object)ship.GetExteriorMeshCollider() != (UnityEngine.Object)null)
                        ship.GetExteriorMeshCollider().enabled = false;
                    await Task.Yield();
                }
                foreach (Renderer rend in exteriorRenderers)
                {
                    if ((UnityEngine.Object)rend != (UnityEngine.Object)null)
                        rend.enabled = true;
                }
                MeshRenderer[] meshRendererArray1 = hullplanting;
                for (int index = 0; index < meshRendererArray1.Length; ++index)
                {
                    Renderer rend = (Renderer)meshRendererArray1[index];
                    if ((UnityEngine.Object)rend != (UnityEngine.Object)null)
                        rend.enabled = true;
                    rend = (Renderer)null;
                }
                meshRendererArray1 = (MeshRenderer[])null;
                componentsOfType = ship.MyStats.GetComponentsOfType(ESlotType.E_COMP_TURRET);
                componentsOfType.AddRange((IEnumerable<PLShipComponent>)ship.MyStats.GetComponentsOfType(ESlotType.E_COMP_MAINTURRET));
                componentsOfType.AddRange((IEnumerable<PLShipComponent>)ship.MyStats.GetComponentsOfType(ESlotType.E_COMP_AUTO_TURRET));
                foreach (PLShipComponent comp in componentsOfType)
                {
                    PLTurret turret = comp as PLTurret;
                    if (turret != null && (UnityEngine.Object)turret.TurretInstance != (UnityEngine.Object)null)
                    {
                        Renderer[] rendererArray = turret.TurretInstance.MyMainRenderers;
                        for (int index = 0; index < rendererArray.Length; ++index)
                        {
                            Renderer rend = rendererArray[index];
                            rend.enabled = true;
                            rend = (Renderer)null;
                        }
                        rendererArray = (Renderer[])null;
                    }
                    turret = (PLTurret)null;
                }
                if ((ship is PLFluffyShipInfo || ship is PLFluffyShipInfo2) && (UnityEngine.Object)(ship as PLFluffyShipInfo).MyVisibleBomb != (UnityEngine.Object)null)
                    (ship as PLFluffyShipInfo).MyVisibleBomb.gameObject.SetActive(true);
                if ((UnityEngine.Object)ship.GetExteriorMeshCollider() != (UnityEngine.Object)null)
                    ship.GetExteriorMeshCollider().enabled = true;
                ship.MyStats.CanBeDetected = (ObscuredBool)true;
                PLMusic.PostEvent("stop_sx_ship_enemy_phasedrone_warp", ship.Exterior);
                gameObject = (GameObject)null;
                exteriorRenderers = (List<MeshRenderer>)null;
                hullplanting = (MeshRenderer[])null;
                componentsOfType = (List<PLShipComponent>)null;
                ship = (PLShipInfo)null;
            }
        }

        private static async void DelayedEndPhasePS(PLShipInfo ship)
        {
            await Task.Delay(1000);
            UnityEngine.Object.Instantiate<GameObject>(PLGlobal.Instance.PhasePS, ship.Exterior.transform.position, Quaternion.identity);
        }

        public static bool IsWanderingNPCShip(PLPersistantShipInfo persistantShipInfo)
        {
            if (persistantShipInfo != null)
            {
                switch (persistantShipInfo.SelectedActorID)
                {
                    case "ExGal_RelicCaravan":
                    case "ExGal_TreasureFleet_Cruiser":
                    case "ExGal_TreasureFleet_Friend":
                        return true;
                    default:
                        return false;
                }
            }
            return false;
        }
    }
}

