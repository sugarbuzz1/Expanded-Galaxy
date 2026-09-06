using PulsarModLoader;
using PulsarModLoader.Content.Components.MegaTurret;
using UnityEngine;

namespace ExpandedGalaxy
{
    internal class PhysicalTurret : PLMegaTurret_Proj
    {
        private float LastSwitchedFiringMode;

        public PhysicalTurret(int inLevel = 0, int inSubTypeData = 1) : base(inLevel, inSubTypeData)
        {
            this.Name = "Coil Artillery";
            this.Desc = "A projectile-based main turret with two modes of fire. Despite being develpoed before the widespread use of laser weaponry, it is still commonly found on modern starships.";
            this.m_Damage = 80f;
            this.FireDelay = 2f;
            this.SubType = MegaTurretModManager.Instance.GetMegaTurretIDFromName(this.Name);
            this.SubTypeData = 0;
            this.TurretRange = 10000f;
            this.m_MaxPowerUsage_Watts = 11000f;
            this.AbyssProjDisplay = true;
            this.HasPulseLaser = false;
            this.CanHitMissiles = false;
        }

        private void UpdateFiringMode()
        {
            if (this.SubTypeData == 0)
            {
                this.m_Damage = 80f;
                this.FireDelay = 2f;
                this.ProjSpeed = 6000f * 0.7f;
                this.TurretRange = 10000f;
                this.HeatGeneratedOnFire = 0.23f;
            }
            else
            {
                this.m_Damage = 280f;
                this.FireDelay = 6f;
                this.ProjSpeed = 1500f * 0.7f;
                this.TurretRange = 5000f;
                this.HeatGeneratedOnFire = 0.48f;
            }
        }

        public override void Tick()
        {
            base.Tick();
            if (this.IsEquipped)
            {
                if (this.ShipStats.Ship.GetCurrentTurretControllerPlayerID(this.TurretID) == PLNetworkManager.Instance.LocalPlayerID)
                {
                    if ((double)Time.time - (double)this.LastSwitchedFiringMode > 1.0 && PLInput.Instance.GetButtonUp(PLInputBase.EInputActionName.right_click) && (double)PLInput.Instance.GetHeldDownTime(PLInputBase.EInputActionName.right_click) < 0.15000000596046448 + (double)Time.deltaTime)
                    {
                        int data = 0;
                        if (this.SubTypeData == 0)
                        {
                            this.SubTypeData = 1;
                            data = 1;
                            PLMusic.PostEvent("play_sx_ship_changetransmission_manual", this.TurretInstance.gameObject);
                        }
                        else
                        {
                            this.SubTypeData = 0;
                            PLMusic.PostEvent("play_sx_ship_changetransmission_auto", this.TurretInstance.gameObject);
                        }
                        this.LastSwitchedFiringMode = Time.time;
                        this.ChargeAmount = 0f;
                        ModMessage.SendRPC("sugarbuzz1.ExpandedGalaxy", "ExpandedGalaxy.SwitchTurretModeRPC", PhotonTargets.Others, new object[3]
                        {
                                (object) this.ShipStats.Ship.ShipID,
                                (object) this.NetID,
                                (object) (short)data,
                        });
                    }
                }
                else if (this.GetCurrentOperator() != null && this.GetCurrentOperator().IsBot)
                {
                    if ((double)Time.time - (double)this.LastSwitchedFiringMode > 5.0)
                    {
                        if (this.ShipStats.Ship.TargetSpaceTarget != null && (this.ShipStats.Ship.TargetSpaceTarget.GetCurrentSensorPosition() - this.ShipStats.Ship.GetCurrentSensorPosition()).magnitude < 1000f)
                        {
                            this.SubTypeData = 1;
                            ModMessage.SendRPC("sugarbuzz1.ExpandedGalaxy", "ExpandedGalaxy.SwitchTurretModeRPC", PhotonTargets.Others, new object[3]
                        {
                                (object) this.ShipStats.Ship.ShipID,
                                (object) this.NetID,
                                (object) this.SubTypeData,
                        });
                        }
                        else if (this.SubTypeData == (short)1)
                        {
                            this.SubTypeData = 0;
                            ModMessage.SendRPC("sugarbuzz1.ExpandedGalaxy", "ExpandedGalaxy.SwitchTurretModeRPC", PhotonTargets.Others, new object[3]
                        {
                                (object) this.ShipStats.Ship.ShipID,
                                (object) this.NetID,
                                (object) this.SubTypeData,
                        });
                        }
                    }
                }
                this.UpdateFiringMode();
            }
        }

        public override string GetInfoString() => base.GetInfoString() + " " + ((this.SubTypeData == 0) ? "(Long Range)" : "(Short Range)");

        public override string GetDamageTypeString() => (this.SubTypeData == 0) ? "PHYSICAL" : "PHYS (EXPLD)";

    }
}
