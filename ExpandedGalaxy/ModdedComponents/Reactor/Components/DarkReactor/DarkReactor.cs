using PulsarModLoader.Content.Components.Reactor;
using UnityEngine;

namespace ExpandedGalaxy
{
    public class DarkReactor : ReactorMod
    {
        public override string Name => "Dark-Matter Reactor";

        public override string Description => "This reactor creates power at a scale unlike any other. Interestingly, it seems to have no reaction to ANY form of coolant.";

        public override int MarketPrice => 50500;

        public override float EnergyOutputMax => 46000f;

        public override float MaxTemp => 4050f;

        public override float EmergencyCooldownTime => 300f;

        public override float EnergySignatureAmount => 9f;

        public override float HeatOutput => 1f;

        public override void Tick(PLShipComponent InComp)
        {
            if (!InComp.IsEquipped || InComp.ShipStats == null || !(InComp.ShipStats.Ship is PLShipInfo) || !((UnityEngine.Object)(InComp.ShipStats.Ship as PLShipInfo).ReactorInstance != (UnityEngine.Object)null))
                return;
            PLReactor plReactor = InComp as PLReactor;
            float num = Mathf.Clamp01(plReactor.ShipStats.ReactorTempCurrent * plReactor.ShipStats.ReactorTotalUsagePercent / plReactor.ShipStats.ReactorTempMax);
            float num1 = Mathf.Clamp01(plReactor.ShipStats.ReactorTotalUsagePercent / 0.75f);
            float num2 = 1f - Mathf.Clamp01(0.5f * num + 0.5f * num1);
            Color color = new Color(85f * num2 / 255f, 0f, 255f * num2 / 255f);
            Color color1 = new Color((35f + 50f * num2) / 255f, 0f, 1f);

            if (InComp.ShipStats.Ship is PLShipInfo)
            {
                foreach (Light componentsInChild in (InComp.ShipStats.Ship as PLShipInfo).ReactorInstance.GetComponentsInChildren<Light>())
                    componentsInChild.color = color1;
                foreach (ParticleSystem componentsInChild in (InComp.ShipStats.Ship as PLShipInfo).ReactorInstance.GetComponentsInChildren<ParticleSystem>())
                {
                    if (componentsInChild != (InComp.ShipStats.Ship as PLShipInfo).ReactorInstance.CenterParticles && componentsInChild != (InComp.ShipStats.Ship as PLShipInfo).ReactorInstance.WhiteParticles && componentsInChild != (InComp.ShipStats.Ship as PLShipInfo).ReactorInstance.FlareAltParticles && componentsInChild != (InComp.ShipStats.Ship as PLShipInfo).ReactorInstance.FlareParticles && componentsInChild != (InComp.ShipStats.Ship as PLShipInfo).ReactorInstance.VerticalFlareParticles)
                        componentsInChild.startColor = color;
                }

            }
        }
    }
}
