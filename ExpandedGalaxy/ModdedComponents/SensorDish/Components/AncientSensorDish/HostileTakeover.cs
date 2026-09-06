using ExpandedGalaxy.SensorDish;
using PulsarModLoader;
using UnityEngine;

namespace ExpandedGalaxy
{
    internal class HostileTakeover : SensorDishAbility
    {
        public override string Name => "Hostile Takeover";

        public override string Description => "Control target drone for 60 seconds\n<color=red>One use per jump</color>";

        public override Texture2D IconTexture => PLGlobal.Instance.ClassIcons[1];

        public override void OnActivate(PLShipStats inStats)
        {
            if (!PhotonNetwork.isMasterClient)
                return;
            if (!(AncientSensorDishMod.lastToInteract != null))
                return;
            bool shouldActivate = false;
            if (AncientSensorDishMod.lastToInteract.GetPawn() != null && AncientSensorDishMod.lastToInteract.GetPawn().CurrentShip != null)
            {
                PLShipComponent component = AncientSensorDishMod.lastToInteract.GetPawn().CurrentShip.MyStats.GetShipComponent<PLShipComponent>(ESlotType.E_COMP_SENSORDISH);
                if (component == null || component.SubTypeData > 0)
                    shouldActivate = true;
            }
            if (shouldActivate)
            {
                bool flag = false;
                bool flag2 = false;
                switch (inStats.Ship.ShipTypeID)
                {
                    case EShipType.E_WDDRONE1:
                    case EShipType.E_WDDRONE2:
                    case EShipType.E_WDDRONE3:
                    case EShipType.E_DEATHSEEKER_DRONE:
                    case EShipType.E_SHOCK_DRONE:
                    case EShipType.E_PHASE_DRONE:
                    case EShipType.E_UNSEEN_FIGHTER:
                        flag = true; break;
                    case EShipType.E_GUARDIAN:
                    case EShipType.E_CORRUPTED_DRONE:
                    case EShipType.E_DEATHSEEKER_DRONE_SC:
                    case EShipType.E_SWARM_KEEPER:
                    case EShipType.E_REPAIR_DRONE:
                    case EShipType.E_PTDRONE:
                        flag2 = true; break;
                }
                if (!flag)
                {
                    if (!flag2)
                    {
                        ModMessage.SendRPC("sugarbuzz1.ExpandedGalaxy", "ExpandedGalaxy.SendWarning", AncientSensorDishMod.lastToInteract.GetPhotonPlayer(), new object[1]
                        {
                                (object) "Target must be a drone!"
                        });
                    }
                    else
                    {
                        ModMessage.SendRPC("sugarbuzz1.ExpandedGalaxy", "ExpandedGalaxy.SendWarning", AncientSensorDishMod.lastToInteract.GetPhotonPlayer(), new object[1]
                        {
                                (object) "Target too strong!"
                        });
                    }
                }
                HostileTakeoverMethod(AncientSensorDishMod.lastToInteract, inStats.Ship);

            }
            else
            {
                if (PhotonNetwork.isMasterClient && !AncientSensorDishMod.lastToInteract.IsBot)
                {
                    ModMessage.SendRPC("sugarbuzz1.ExpandedGalaxy", "ExpandedGalaxy.SendWarning", AncientSensorDishMod.lastToInteract.GetPhotonPlayer(), new object[1]
                    {
                        (object) "Only useable once per jump!"
                    });
                }
            }
        }

        private void HostileTakeoverMethod(PLPlayer player, PLShipInfoBase target)
        {
            if (player == null)
                return;
            if (!player.IsBot)
            {
                if ((UnityEngine.Object)target.PilotingSystem == (UnityEngine.Object)null)
                {
                    target.PilotingSystem = target.gameObject.AddComponent<PLPilotingSystem>();
                    target.PilotingSystem.MyShipInfo = target;
                }
                if ((UnityEngine.Object)target.PilotingHUD == (UnityEngine.Object)null)
                {
                    target.PilotingHUD = target.gameObject.AddComponent<PLPilotingHUD>();
                    target.PilotingHUD.MyShipInfo = target;
                }
                target.OrbitCameraMaxDistance = 40f;
                target.OrbitCameraMinDistance = 7f;
                if (player.GetPhotonPlayer().IsMasterClient)
                {
                    target.photonView.RPC("NewShipController", PhotonTargets.All, (object)player.GetPlayerID());
                }
                else
                {
                    ModMessage.SendRPC("sugarbuzz1.ExpandedGalaxy", "ExpandedGalaxy.PilotDrone", player.GetPhotonPlayer(), new object[1]
                    {
                        target.ShipID
                    });
                }
            }
            ModMessage.SendRPC("sugarbuzz1.ExpandedGalaxy", "ExpandedGalaxy.HostileTakeoverRPC", PhotonTargets.All, new object[2]
            {
                player.GetPawn().CurrentShip.ShipID,
                target.ShipID
            });
        }
    }
}
