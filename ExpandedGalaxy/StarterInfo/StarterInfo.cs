using System.Collections;
using UnityEngine;

namespace ExpandedGalaxy
{
    internal class StarterInfo
    {
        internal static bool RolandSC = false;

        /*
        public class AsteroidInfo : PLShipInfoBase
        {
            public override void SetupShipStats(bool previewStats = false, bool startingPlayerShip = false)
            {
                base.SetupShipStats(previewStats, startingPlayerShip);
                this.ShipTypeID = (EShipType)69;
                this.IsDrone = true;
                this.MyStats.SetSlotLimit(ESlotType.E_COMP_HULL, 1);
                this.MyStats.Mass = 1000f;
                this.m_VisibleToPlayersInUI = true;
                this.DropScrap = false;
                this.CanFireProbes = false;
                this.ShipNameValue = "Asteroid";
                this.Thrusters = new GameObject[0];
                this.ThrusterRenderers = new GameObject[0];
                this.ReverseThrusters = new GameObject[0];
                this.ReverseThrusterRenderers = new GameObject[0];
                this.AmbientThrusters = new GameObject[0];
                this.DirectionalThrusters = new GameObject[0];
                this.ExteriorCollider = this.Exterior.GetComponent<MeshCollider>();
                this.ExteriorMeshCollider = this.Exterior.GetComponent<MeshCollider>();
                if (!PhotonNetwork.isMasterClient)
                    return;
                this.MyStats.AddShipComponent(PLHull.CreateHullFromHash((int)EHullType.E_WARP_GUARDIAN_HULL, 10, 0));
            }

            public override void ShipFinalCalculateStats(ref PLShipStats inStats)
            {
                base.ShipFinalCalculateStats(ref inStats);
                inStats.EMSignature += 100f;
                this.DischargeAmount = 0f;
            }

            public override void TakeDamage(float damage)
            {
            }
        }
        */

        internal static IEnumerator DelayedSetupShipStatsPostfix(int index, PLShipInfoBase __instance, bool previewStats, bool startingPlayerShip)
        {
            yield return new WaitForSecondsRealtime(1);
            switch (index)
            {
                case 0:
                    CruiserInfoMod.Postfix((PLCruiserInfo)__instance, previewStats, startingPlayerShip);
                    break;
                case 1:
                    DestroyerInfoMod.Postfix((PLWDDestroyerInfo)__instance, previewStats, startingPlayerShip);
                    break;
                case 2:
                    StargazerInfoMod.Postfix((PLStarGazerInfo)__instance, previewStats, startingPlayerShip);
                    break;
                case 3:
                    CarrierInfoMod.Postfix((PLCarrierInfo)__instance, previewStats, startingPlayerShip);
                    break;
            }
        }
    }
}
