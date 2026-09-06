using PulsarModLoader;
using UnityEngine;

namespace ExpandedGalaxy
{
    internal class AncientSensorDish : PLSensorDish
    {
        public GameObject beam;
        public AncientSensorDish(ESensorDishType inType, int inLevel) : base(inType, inLevel)
        {
            this.SubType = (int)inType;
            this.Level = inLevel;
            this.SubTypeData = 0;
            this.Name = "Ancient Sensor Dish";
            this.Desc = "A sensor dish more focused on combat capabilaties rather than assisting in ship detection.";
            this.CalculatedMaxPowerUsage_Watts = 6000f;
            this.CargoVisualPrefabID = 45;
        }

        public override void Tick()
        {
            base.Tick();
        }

        public override void OnWarp()
        {
            base.OnWarp();
            this.SubTypeData = 0;
            if (PhotonNetwork.isMasterClient)
                ModMessage.SendRPC("sugarbuzz1.ExpandedGalaxy", "ExpandedGalaxy.UpdateSubTypeData", PhotonTargets.Others, new object[3]
                {
                        (object) this.ShipStats.Ship.ShipID,
                        (object) this.NetID,
                        (object) this.SubTypeData,
                });
        }
    }
}
