using HarmonyLib;
using PulsarModLoader.Content.Components.Shield;
using UnityEngine;

namespace ExpandedGalaxy
{
    internal class Shields
    {
        public class SuspensionField : ShieldMod
        {
            internal static float cachedDamage = 0f;
            public override string Name => "Anti-Matter Suspension Field";

            public override string Description => "A unique shield that lessens the effects of larger blows by sending the damage to be absorbed at a later point in time. It is not advised to let the shields fail while still absorbing damage";

            public override int MarketPrice => 16000;

            public override float ShieldMax => 1050f;

            public override float ChargeRateMax => 13f;

            public override float RecoveryRate => 1f;

            public override float MinIntegrityPercentForQuantumShield => 0.45f;

            public override float MaxPowerUsage_Watts => 10200f;

            public override int MinIntegrityAfterDamage => 105;

            public override void Tick(PLShipComponent InComp)
            {
                if (!PhotonNetwork.isMasterClient)
                    return;
                PLShieldGenerator pLShieldGenerator = InComp as PLShieldGenerator;
                if (!pLShieldGenerator.IsEquipped || pLShieldGenerator.ShipStats == null || !((UnityEngine.Object)pLShieldGenerator.ShipStats.Ship != (UnityEngine.Object)null))
                    return;
                if (pLShieldGenerator.Current > pLShieldGenerator.ShipStats.ShieldsMax * 0.1f && cachedDamage > 0f)
                {
                    pLShieldGenerator.Current -= (cachedDamage / 10f) * Time.deltaTime;
                    cachedDamage -= (cachedDamage / 10f) * Time.deltaTime;
                    pLShieldGenerator.Current = Mathf.Clamp(pLShieldGenerator.Current, 0f, pLShieldGenerator.Current + 1f);
                    cachedDamage = Mathf.Clamp(cachedDamage, 0f, cachedDamage + 1f);
                }
                else
                {
                    if (cachedDamage > 0f)
                    {
                        pLShieldGenerator.Current = 0f;
                        pLShieldGenerator.ShipStats.Ship.TakeDamage(cachedDamage * 1.3f, false, EDamageType.E_ENERGY, 1f, -1, null, -1);
                        cachedDamage = 0f;
                    }
                }
            }
        }

        [HarmonyPatch(typeof(PLShipStats), "TakeShieldDamage")]
        internal class relicShield
        {
            private static bool Prefix(PLShipStats __instance, ref float inDmg, EDamageType dmgType, float DT_ShieldBoost, float shieldDamageMod, PLTurret turret)
            {
                if (__instance.GetShipComponent<PLShieldGenerator>(ESlotType.E_COMP_SHLD) != null && __instance.GetShipComponent<PLShieldGenerator>(ESlotType.E_COMP_SHLD).SubType == ShieldModManager.Instance.GetShieldIDFromName("Anti-Matter Suspension Field"))
                {
                    PLShieldGenerator shipComponent = __instance.GetShipComponent<PLShieldGenerator>(ESlotType.E_COMP_SHLD);
                    if (shipComponent.Current < shipComponent.ShipStats.ShieldsMax * 0.101f)
                        return true;
                    float num1 = 1f;
                    float num2 = num1 * (float)(1.0 / (double)DT_ShieldBoost * (1.0 / (double)shieldDamageMod));
                    float num3 = Mathf.Min(inDmg, shipComponent.Current * num2);
                    float num4 = num3 / num2;
                    SuspensionField.cachedDamage += num4;
                    inDmg = 0f;
                }
                return true;
            }
        }

        public class ReflectorShield : ShieldMod
        {
            public override string Name => "Reflector Shield Generator";

            public override string Description => "A shield generator that reflects 20% of incoming damage back to the dealer. Due to its potential in harming police vessels it has been declared contraband by the Colonial Union.";

            public override int MarketPrice => 18645;

            public override float ShieldMax => 800f;

            public override float ChargeRateMax => 12f;

            public override float RecoveryRate => 2f;

            public override float MinIntegrityPercentForQuantumShield => 0.4f;

            public override float MaxPowerUsage_Watts => 16000f;

            public override int MinIntegrityAfterDamage => 125;

            public override bool Contraband => true;
        }

        [HarmonyPatch(typeof(PLShipStats), "TakeShieldDamage")]
        internal class ReflectorShieldDamage
        {
            private static bool Prefix(PLShipStats __instance, ref float inDmg, EDamageType dmgType, float DT_ShieldBoost, float shieldDamageMod, PLTurret turret)
            {
                if (__instance.GetShipComponent<PLShieldGenerator>(ESlotType.E_COMP_SHLD) != null && __instance.GetShipComponent<PLShieldGenerator>(ESlotType.E_COMP_SHLD).SubType == ShieldModManager.Instance.GetShieldIDFromName("Reflector Shield Generator"))
                {
                    if (turret != null && turret.ShipStats != null && turret.ShipStats.Ship != null && turret.ShipStats != __instance)
                    {
                        turret.ShipStats.Ship.TakeDamage(inDmg * 0.2f, false, dmgType, 1f, -1, __instance.Ship, -1);
                    }
                    inDmg *= 0.8f;
                }
                return true;
            }
        }

        [HarmonyPatch(typeof(PLShieldGenerator), "Tick")]
        internal class ChargeShieldWhenOff
        {
            private static void Postfix(PLShieldGenerator __instance)
            {
                if (!__instance.IsEquipped)
                    return;
                PLShipInfo ship = __instance.ShipStats.Ship as PLShipInfo;
                if ((double)__instance.Current < (double)__instance.ShipStats.ShieldsMax && ((Object)ship == (Object)null || ((Object)ship.StartupSwitchBoard != (Object)null && !ship.StartupSwitchBoard.GetLateStatus(2))))
                {
                    __instance.RequestPowerUsage_Percent = 1f;
                    __instance.IsPowerActive = true;
                }
            }
        }

        [HarmonyPatch(typeof(PLShipInfo), "OnCalculateStatsFinal")]
        internal class ChargeShieldFix
        {
            private static bool Prefix(PLShipInfo __instance, PLShipStats inStats)
            {
                if (__instance.MyShieldGenerator != null && !__instance.MyShieldGenerator.IsPowerActive)
                    inStats.ShieldsChargeRate = 0f;
                if (!__instance.InWarp)
                    return false;
                inStats.ShieldsChargeRate *= 10f;
                inStats.ShieldsChargeRateMax *= 10f;
                return false;
            }
        }
    }
}
