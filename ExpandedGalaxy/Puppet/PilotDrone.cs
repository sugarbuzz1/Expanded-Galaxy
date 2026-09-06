using PulsarModLoader;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using UnityEngine;

namespace ExpandedGalaxy
{
    public class PilotDrone : ModMessage
    {
        public override void HandleRPC(object[] arguments, PhotonMessageInfo sender)
        {
            if (sender.sender != PhotonNetwork.masterClient)
                return;
            DelayedPilotDrone((int)arguments[0]);
        }

        private static async void DelayedPilotDrone(int shipID)
        {
            float Timer = Time.time;
            PLShipInfoBase ship = PLEncounterManager.Instance.GetShipFromID(shipID);
            YieldAwaitable yieldAwaitable;
            while ((UnityEngine.Object)ship == (UnityEngine.Object)null && (double)Time.time - (double)Timer < 15.0)
            {
                ship = PLEncounterManager.Instance.GetShipFromID(shipID);
                yieldAwaitable = Task.Yield();
                await yieldAwaitable;
            }
            if ((double)Time.time - (double)Timer >= 15.0 && (UnityEngine.Object)ship == (UnityEngine.Object)null)
            {
                ship = (PLShipInfoBase)null;
            }
            else
            {
                if ((UnityEngine.Object)ship.PilotingSystem == (UnityEngine.Object)null)
                {
                    ship.PilotingSystem = ship.gameObject.AddComponent<PLPilotingSystem>();
                    ship.PilotingSystem.MyShipInfo = ship;
                }
                if ((UnityEngine.Object)ship.PilotingHUD == (UnityEngine.Object)null)
                {
                    ship.PilotingHUD = ship.gameObject.AddComponent<PLPilotingHUD>();
                    ship.PilotingHUD.MyShipInfo = ship;
                }
                ship.OrbitCameraMaxDistance = 40f;
                ship.OrbitCameraMinDistance = 7f;
                yieldAwaitable = Task.Yield();
                await yieldAwaitable;
                ship.photonView.RPC("NewShipController", PhotonTargets.All, (object)(int)PLNetworkManager.Instance.LocalPlayerID);
            }
        }
    }
}

