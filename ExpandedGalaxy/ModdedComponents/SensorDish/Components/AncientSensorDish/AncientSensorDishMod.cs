using ExpandedGalaxy.SensorDish;

namespace ExpandedGalaxy
{
    internal class AncientSensorDishMod : SensorDishMod
    {
        internal static PLPlayer lastToInteract;

        public override string Name => "Ancient Sensor Dish";

        public override PLShipComponent SensorDish => new AncientSensorDish(0, 0);

        public AncientSensorDishMod()
        {
            SensorDishAbilities[0] = new HostileTakeover();
            SensorDishAbilities[1] = new WeakenArmor();
            SensorDishAbilities[2] = new MissileLock();
        }

        public override int AbilityLogicForBots(PLBot inBot, PLShipInfo shipContext)
        {
            lastToInteract = inBot.PlayerOwner;
            if (shipContext.MySensorDish.SubTypeData == 0 && HasShipToTakeover(shipContext, out PLShipInfoBase strongestShip))
            {
                strongestShip.photonView.RPC("AddSensorWeakness", PhotonTargets.All, 3, PLServer.Instance.GetEstimatedServerMs(), 1);
                return -1;
            }
            else if (shipContext.TargetShip.MyShieldGenerator != null && shipContext.TargetShip.MyShieldGenerator.MinIntegrityForBubble * 2f <= shipContext.TargetShip.MyShieldGenerator.Current && (double)shipContext.TargetShip.MyStats.ShieldsMax != 0.0)
            {
                return 4;
            }
            else if (UnityEngine.Random.Range(0, 100) < 30)
            {
                return 5;
            }
            else if (shipContext.TargetShip.GetCombatLevel() > shipContext.GetCombatLevel() && (float)shipContext.TargetShip.WeaponsSystem.Health > 5f)
            {
                return 1;
            }
            else if ((float)shipContext.TargetShip.EngineeringSystem.Health > 5f)
            {
                return 0;
            }
            else
            {
                return 2;
            }
        }

        private bool HasShipToTakeover(PLShipInfoBase inHomeShip, out PLShipInfoBase strongestShip)
        {
            strongestShip = null;
            foreach (PLShipInfoBase shipInfoBase in UnityEngine.Object.FindObjectsOfType<PLShipInfoBase>())
            {
                if (shipInfoBase != null && shipInfoBase.ShipID != inHomeShip.ShipID && (inHomeShip.TargetShip == null || inHomeShip.TargetShip.ShipID != shipInfoBase.ShipID) && !shipInfoBase.HasBeenDestroyed && shipInfoBase.IsDrone && inHomeShip.ShouldBeHostileToShip(shipInfoBase, false, false, false))
                {
                    switch (shipInfoBase.ShipTypeID)
                    {
                        case EShipType.E_WDDRONE1:
                        case EShipType.E_WDDRONE2:
                        case EShipType.E_WDDRONE3:
                        case EShipType.E_DEATHSEEKER_DRONE:
                        case EShipType.E_SHOCK_DRONE:
                        case EShipType.E_PHASE_DRONE:
                        case EShipType.E_UNSEEN_FIGHTER:
                            if (strongestShip == null)
                                strongestShip = shipInfoBase;
                            else
                                if (shipInfoBase.GetCombatLevel() > strongestShip.GetCombatLevel())
                                strongestShip = shipInfoBase;
                            break;
                    }
                }
            }
            return strongestShip != null;
        }
    }
}
