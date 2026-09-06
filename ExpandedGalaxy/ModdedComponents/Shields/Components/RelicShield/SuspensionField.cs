using PulsarModLoader.Content.Components.Shield;
using UnityEngine;

namespace ExpandedGalaxy
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
                    float damage = Mathf.Clamp(cachedDamage * 1.3f, 0f, pLShieldGenerator.ShipStats.HullMax * 0.66f);
                    pLShieldGenerator.Current = 0f;
                    pLShieldGenerator.ShipStats.Ship.TakeDamage(damage, false, EDamageType.E_ENERGY, 1f, -1, null, -1);
                    cachedDamage = 0f;
                }
            }
        }
    }
}
