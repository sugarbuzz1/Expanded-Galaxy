using PulsarModLoader.Content.Components.WarpDriveProgram;
using System;
using System.Collections.Generic;

namespace ExpandedGalaxy
{
    public class DigQuake : WarpDriveProgramMod
    {
        public override string Name => "Digital Earthquake";

        public override string Description => "An ancient code that causes all ships infected with a virus in the system to disintegrate. Its power scales with the number of unique viruses";

        public override int MarketPrice => 12000;

        public override string ShortName => "DQ";

        public override float ActiveTime => 0.1f;

        public override int MaxLevelCharges => 5;

        public override void Execute(PLWarpDriveProgram InWarpDriveProgram)
        {
            if (PLServer.GetCurrentSector() == null || !PhotonNetwork.isMasterClient)
                return;
            List<int> virusIDs = new List<int>();
            foreach (PLShipInfoBase plShipInfoBase in UnityEngine.Object.FindObjectsOfType(typeof(PLShipInfoBase)))
            {
                virusIDs.Clear();
                foreach (PLVirus virus in plShipInfoBase.MyStats.GetComponentsOfType(ESlotType.E_COMP_VIRUS))
                {
                    if (!virusIDs.Contains(virus.SubType))
                        virusIDs.Add(virus.SubType);
                }
                if (virusIDs.Count > 0)
                {
                    plShipInfoBase.MyStats.TakeHullDamage((float)(200.0 * Math.Pow(1.5, virusIDs.Count - 1)), EDamageType.E_ARMOR_PIERCE_PHYS, InWarpDriveProgram.ShipStats.Ship, null);
                    plShipInfoBase.RemoveHostileViruses();
                    plShipInfoBase.DestroySelfIfDead(InWarpDriveProgram.ShipStats.Ship);
                }
            }
        }
    }
}

