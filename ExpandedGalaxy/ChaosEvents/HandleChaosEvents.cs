using HarmonyLib;
using System.Collections.Generic;
using UnityEngine;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLServer), "Update")]
    internal class HandleChaosEvents
    {
        private static bool Prefix()
        {
            if (PhotonNetwork.isMasterClient && PLServer.Instance != null && (double)PLServer.Instance.lifetime > 30.0)
                if (Mathf.FloorToInt((float)PLServer.Instance.ChaosLevel) > (int)PLServer.Instance.OldChaosLevel)
                {
                    List<int> activePrevious = ChaosEvents.DeactivateAllChaosEvents();
                    PLRand rand = new PLRand(Mathf.FloorToInt(PLGlobal.Instance.Galaxy.Seed + PLServer.Instance.ChaosLevel + 1));
                    for (int i = 0; i < 100; i++)
                    {
                        EChaosEvent e = (EChaosEvent)rand.Next(0, 10);
                        if (!activePrevious.Contains((int)e) && ChaosEvents.CanActivateChaosEvent(e))
                        {
                            PLServer.Instance.SetChaosEventAsActive((int)e);
                            PLServer.Instance.OnChaosEventActivate(e);
                            ChaosEvents.CreateLRDAForChaosEvent(e);
                            break;
                        }
                    }
                }
            return true;
        }
    }
}
