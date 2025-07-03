using HarmonyLib;
using PulsarModLoader;
using PulsarModLoader.Content.Components.PolytechModule;
using UnityEngine;

namespace ExpandedGalaxy
{
    internal class PTModules
    {
        private class BossPT1 : PolytechModuleMod
        {
            public override string Name => "P.T. Module: Recompiler 1";

            public override string Description => "If you're reading this, I fucked up :(";

            public override int MarketPrice => 9999999;

            public override float MaxPowerUsage_Watts => 1f;

            public override bool Experimental => false;

            public override bool Contraband => true;

            public override bool CanBeDroppedOnShipDeath => false;

            public override void Tick(PLShipComponent InComp)
            {
                if (PhotonNetwork.isMasterClient)
                {
                    if (InComp.SubTypeData < 0)
                        InComp.SubTypeData = 300;
                    if (InComp.IsEquipped && InComp.SubTypeData < 300)
                        InComp.SubTypeData += 10;
                    ModMessage.SendRPC("sugarbuzz1.ExpandedGalaxy", "ExpandedGalaxy.UpdateSubTypeData", PhotonTargets.Others, new object[3]
                        {
                        (object) InComp.ShipStats.Ship.ShipID,
                        (object) InComp.NetID,
                        (object) InComp.SubTypeData,
                        });
                }
                if (InComp.IsEquipped && InComp.ShipStats.Ship.ShipID == PLEncounterManager.Instance.PlayerShip.ShipID)
                {
                    InComp.FlagForSelfDestruction();
                    return;
                }
            }

            public override void AddStats(PLShipComponent InComp)
            {
                base.AddStats(InComp);
                PLShipStats shipStats = InComp.ShipStats;
                shipStats.HullArmor += InComp.SubTypeData / 250f;

            }

            public override void LateAddStats(PLShipComponent InComp)
            {
                PLShipStats shipStats = InComp.ShipStats;
                shipStats.ShieldsChargeRate += Mathf.Clamp((1f - (shipStats.HullCurrent / shipStats.HullMax)) * 75f, 0f, 50f);
                shipStats.ShieldsChargeRateMax += Mathf.Clamp((1f - (shipStats.HullCurrent / shipStats.HullMax)) * 75f, 0f, 50f); ;
            }
        }

        [HarmonyPatch(typeof(PLShipStats), "TakeHullDamage")]
        internal class AdaptiveArmorPT
        {
            private static void Postfix(PLShipStats __instance, float inDmg, EDamageType inDmgType, PLShipInfoBase attackingShip, PLTurret turret, ref float __result)
            {
                if (__instance.Ship != null)
                {
                    if (__instance.Ship.ShipTypeID == EShipType.E_POLYTECH_SHIP)
                    {
                        foreach (PLShipComponent component in __instance.GetComponentsOfType(ESlotType.E_COMP_POLYTECH_MODULE))
                        {
                            PLPolytechModule module = component as PLPolytechModule;
                            if (module != null)
                            {
                                if (component.SubType == PolytechModuleModManager.Instance.GetPolytechModuleIDFromName("P.T. Module: Recompiler 1"))
                                {
                                    if (PhotonNetwork.isMasterClient)
                                    {
                                        component.SubTypeData = 0;
                                        ModMessage.SendRPC("sugarbuzz1.ExpandedGalaxy", "ExpandedGalaxy.UpdateSubTypeData", PhotonTargets.Others, new object[3]
                                            {
                                            (object) __instance.Ship.ShipID,
                                            (object) component.NetID,
                                            (object) component.SubTypeData,
                                            });
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        private class BossPT3 : PolytechModuleMod
        {
            private float LastProgramTickTime;
            public override string Name => "P.T. Module: Recompiler 3";

            public override string Description => "If you're reading this, I fucked up :(";

            public override int MarketPrice => 9999999;

            public override float MaxPowerUsage_Watts => 1f;

            public override bool Experimental => false;

            public override bool Contraband => true;

            public override bool CanBeDroppedOnShipDeath => false;

            public override void Tick(PLShipComponent InComp)
            {
                //Program Recharge + Decloaker
                if (!PhotonNetwork.isMasterClient || !InComp.IsEquipped)
                    return;
                if (InComp.ShipStats == null || InComp.ShipStats.Ship == null)
                    return;
                PLPolytechShipInfo shipInfo = InComp.ShipStats.Ship as PLPolytechShipInfo;
                if (!shipInfo.StartupSwitchBoard.GetStatus(0))
                {
                    this.LastProgramTickTime = Time.time;
                    return;
                }
                if ((double)Time.time - (double)this.LastProgramTickTime > 15.0)
                {
                    this.LastProgramTickTime = Time.time;
                    InComp.ShipStats.Ship.MyWarpDrive.ChargePrograms(overrideChargeCount: 4);
                }
                PLShipInfo ship = PLEncounterManager.Instance.PlayerShip;
                if (ship.GetIsCloakingSystemActive() && InComp.ShipStats.Ship.PersistantShipInfo.MyCurrentSector.ID == ship.PersistantShipInfo.MyCurrentSector.ID && !ship.Get_IsInWarpMode())
                    ship.TakeDamage(500f, false, EDamageType.E_ENERGY, 1f, -1, InComp.ShipStats.Ship, -1);
            }
        }

        internal class BossPT4 : PolytechModuleMod
        {
            private float LastProgramTickTime;
            public static float LastEMPTime = -1f;
            public override string Name => "P.T. Module: Recompiler 4";

            public override string Description => "If you're reading this, I fucked up :(";

            public override int MarketPrice => 9999999;

            public override float MaxPowerUsage_Watts => 1f;

            public override bool Experimental => false;

            public override bool Contraband => true;

            public override bool CanBeDroppedOnShipDeath => false;

            public override void Tick(PLShipComponent InComp)
            {
                //Add EMP
                if (!PhotonNetwork.isMasterClient || !InComp.IsEquipped)
                    return;
                if (InComp.SubTypeData < 0 || InComp.SubTypeData > 1)
                    InComp.SubTypeData = 0;
                if (InComp.ShipStats == null || InComp.ShipStats.Ship == null)
                    return;
                if (InComp.IsEquipped && InComp.ShipStats.Ship.ShipID == PLEncounterManager.Instance.PlayerShip.ShipID)
                {
                    InComp.FlagForSelfDestruction();
                    return;
                }
                PLPolytechShipInfo shipInfo = InComp.ShipStats.Ship as PLPolytechShipInfo;
                if (!shipInfo.StartupSwitchBoard.GetStatus(0))
                {
                    this.LastProgramTickTime = Time.time;
                    LastEMPTime = Time.time;
                    InComp.SubTypeData = 0;
                    return;
                }
                if ((double)Time.time - (double)this.LastProgramTickTime > 15.0)
                {
                    this.LastProgramTickTime = Time.time;
                    InComp.ShipStats.Ship.MyWarpDrive.ChargePrograms(overrideChargeCount: 4);
                }
                PLShipInfo ship = PLEncounterManager.Instance.PlayerShip;
                if (ship.GetIsCloakingSystemActive() && InComp.ShipStats.Ship.PersistantShipInfo.MyCurrentSector.ID == ship.PersistantShipInfo.MyCurrentSector.ID && !ship.Get_IsInWarpMode())
                    ship.TakeDamage(500f, false, EDamageType.E_ENERGY, 1f, -1, InComp.ShipStats.Ship, -1);

                if (LastEMPTime == -1f && !PLEncounterManager.Instance.PlayerShip.Get_IsInWarpMode())
                {
                    LastEMPTime = Time.time;
                    InComp.SubTypeData = 0;
                }
                if ((double)Time.time - (double)LastEMPTime > 20.0)
                {
                    InComp.SubTypeData = 1;
                    if ((double)Time.time - (double)LastEMPTime > 30.0)
                    {
                        LastEMPTime = Time.time;
                        InComp.SubTypeData = 0;
                        if (PhotonNetwork.isMasterClient)
                        {
                            ModMessage.SendRPC("sugarbuzz1.ExpandedGalaxy", "ExpandedGalaxy.EMPPulse", PhotonTargets.All, new object[2]
                            {
                            (object) InComp.ShipStats.Ship.ShipID,
                            (object) 5000f
                            });
                        }
                    }
                }
                ModMessage.SendRPC("sugarbuzz1.ExpandedGalaxy", "ExpandedGalaxy.UpdateSubTypeData", PhotonTargets.Others, new object[3]
                        {
                        (object) InComp.ShipStats.Ship.ShipID,
                        (object) InComp.NetID,
                        (object) InComp.SubTypeData,
                        });
            }

            public override void FinalLateAddStats(PLShipComponent InComp)
            {
                if (InComp.SubTypeData == 1)
                    InComp.ShipStats.Ship.DischargeAmount = 0.9f;
                else
                    InComp.ShipStats.Ship.DischargeAmount = 0.0f;
            }
        }

        internal class BossPT5 : PolytechModuleMod
        {
            private float LastProgramTickTime;
            public static float LastEMPTime = -1f;
            public override string Name => "P.T. Module: Recompiler 5";

            public override string Description => "If you're reading this, I fucked up :(";

            public override int MarketPrice => 9999999;

            public override float MaxPowerUsage_Watts => 1f;

            public override bool Experimental => false;

            public override bool Contraband => true;

            public override bool CanBeDroppedOnShipDeath => false;

            public override void Tick(PLShipComponent InComp)
            {
                //Add Anti-Virus
                if (!PhotonNetwork.isMasterClient || !InComp.IsEquipped)
                    return;
                if (InComp.ShipStats == null || InComp.ShipStats.Ship == null)
                    return;
                if (InComp.IsEquipped && InComp.ShipStats.Ship.ShipID == PLEncounterManager.Instance.PlayerShip.ShipID)
                {
                    InComp.FlagForSelfDestruction();
                    return;
                }
                PLPolytechShipInfo shipInfo = InComp.ShipStats.Ship as PLPolytechShipInfo;
                if (!shipInfo.StartupSwitchBoard.GetStatus(0))
                {
                    this.LastProgramTickTime = Time.time;
                    LastEMPTime = Time.time;
                    InComp.SubTypeData = 0;
                    return;
                }
                if ((double)Time.time - (double)this.LastProgramTickTime > 20.0)
                {
                    this.LastProgramTickTime = Time.time;
                    InComp.ShipStats.Ship.MyWarpDrive.ChargePrograms(overrideChargeCount: 3);
                }
                PLShipInfo ship = PLEncounterManager.Instance.PlayerShip;
                if (ship.GetIsCloakingSystemActive() && InComp.ShipStats.Ship.PersistantShipInfo.MyCurrentSector.ID == ship.PersistantShipInfo.MyCurrentSector.ID && !ship.Get_IsInWarpMode())
                    ship.TakeDamage(500f, false, EDamageType.E_ENERGY, 1f, -1, InComp.ShipStats.Ship, -1);

                if (LastEMPTime == -1f && !PLEncounterManager.Instance.PlayerShip.Get_IsInWarpMode())
                {
                    LastEMPTime = Time.time;
                    InComp.SubTypeData = 0;
                }
                if ((double)Time.time - (double)LastEMPTime > 20.0)
                {
                    InComp.SubTypeData = 1;
                    if ((double)Time.time - (double)LastEMPTime > 30.0)
                    {
                        LastEMPTime = Time.time;
                        InComp.SubTypeData = 0;
                        if (PhotonNetwork.isMasterClient)
                        {
                            ModMessage.SendRPC("sugarbuzz1.ExpandedGalaxy", "ExpandedGalaxy.EMPPulse", PhotonTargets.All, new object[2]
                            {
                            (object) InComp.ShipStats.Ship.ShipID,
                            (object) 5000f
                            });
                        }
                    }
                }
                ModMessage.SendRPC("sugarbuzz1.ExpandedGalaxy", "ExpandedGalaxy.UpdateSubTypeData", PhotonTargets.Others, new object[3]
                        {
                        (object) InComp.ShipStats.Ship.ShipID,
                        (object) InComp.NetID,
                        (object) InComp.SubTypeData,
                        });
            }

            public override void FinalLateAddStats(PLShipComponent InComp)
            {
                if (InComp.ShipStats.Ship.IsSupershieldActive())
                    InComp.ShipStats.CyberDefenseRating += 10f;
                if (InComp.SubTypeData == 1)
                    InComp.ShipStats.Ship.DischargeAmount = 0.9f;
                else
                    InComp.ShipStats.Ship.DischargeAmount = 0.0f;
            }
        }
    }
}
