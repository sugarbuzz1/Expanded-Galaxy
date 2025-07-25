﻿using CodeStage.AntiCheat.ObscuredTypes;
using HarmonyLib;
using PulsarModLoader.Content.Components.HullPlating;
using UnityEngine;

namespace ExpandedGalaxy
{
    internal class HullPlating
    {
        private class HeavyDutyPlatingMod : HullPlatingMod
        {
            public override string Name => "HeavyDutyPlating";

            public override PLShipComponent PLHullPlating => (PLShipComponent)new HullPlating.HeavyDutyPlating(EHullPlatingType.E_HULLPLATING_CCGE, 0);
        }

        internal class HeavyDutyPlating : PLHullPlating
        {
            public HeavyDutyPlating(EHullPlatingType inType, int inLevel) : base(inType, inLevel)
            {
                this.SubType = HullPlatingModManager.Instance.GetHullPlatingIDFromName("HeavyDutyPlating");
                this.Level = inLevel;
                this.Name = "Heavy Duty Plating";
                this.Desc = "This heavyset plating is designed to further reinforce the hull, at the expense of ship maneuverability";
                this.m_MarketPrice = (ObscuredInt)6500;

            }

            public override void AddStats(PLShipStats inStats)
            {
                base.AddStats(inStats);
                inStats.HullArmor += (75f + (15f * this.Level)) / 250f;
                inStats.Mass += (70f + (8f * this.Level));
            }

            public override string GetStatLineLeft() => PLLocalize.Localize("Armor") + "\n" + PLLocalize.Localize("Bottom Hit Dmg") + "\n" + PLLocalize.Localize("Mass") + "\n";

            public override string GetStatLineRight() => (75f + (15f * this.Level)).ToString("0") + "\n-" + (32f + (2f * this.Level)).ToString("0") + "%\n" + (120f + (8f * this.Level)).ToString("0") + "\n";
        }

        private class LightPlatingMod : HullPlatingMod
        {
            public override string Name => "LightHullPlating";

            public override PLShipComponent PLHullPlating => (PLShipComponent)new HullPlating.LightPlating(EHullPlatingType.E_HULLPLATING_CCGE, 0);
        }

        internal class LightPlating : PLHullPlating
        {
            public LightPlating(EHullPlatingType inType, int inLevel) : base(inType, inLevel)
            {
                this.SubType = HullPlatingModManager.Instance.GetHullPlatingIDFromName("LightHullPlating");
                this.Level = inLevel;
                this.Name = "Light Hull Plating";
                this.Desc = "A lighter version of the standard hull plating that boosts dodging capabilities";
                this.m_MarketPrice = (ObscuredInt)9000;
            }

            public override void AddStats(PLShipStats inStats)
            {
                base.AddStats(inStats);
                inStats.HullArmor += (30f + (6f * this.Level)) / 250f;
                inStats.Mass += (-15f + (4f * this.Level));
            }

            public override string GetStatLineLeft() => PLLocalize.Localize("Armor") + "\n" + PLLocalize.Localize("Bottom Hit Dmg") + "\n" + PLLocalize.Localize("Mass") + "\n";

            public override string GetStatLineRight() => (30f + (6f * this.Level)).ToString("0") + "\n-" + (20f + (2f * this.Level)).ToString("0") + "\n" + (35f + (4f * this.Level)).ToString("0") + "\n";
        }

        private class NanoActivePlatingMod : HullPlatingMod
        {
            public override string Name => "NanoActivePlating";

            public override PLShipComponent PLHullPlating => (PLShipComponent)new HullPlating.NanoActivePlating(EHullPlatingType.E_HULLPLATING_CCGE, 0);
        }
        internal class NanoActivePlating : PLHullPlating
        {
            internal float armor = 200f;
            public NanoActivePlating(EHullPlatingType inType, int inLevel) : base(inType, inLevel)
            {
                this.SubType = HullPlatingModManager.Instance.GetHullPlatingIDFromName("NanoActivePlating");
                this.Level = inLevel;
                this.Name = "Nano-Active Hull Plating";
                this.Desc = "Utilizing nanite technology this plating reinforces itself, getting stronger over time";
                this.m_MarketPrice = (ObscuredInt)8000;
                this.Experimental = true;
                this.UpdateArmorValue();
            }

            private void UpdateArmorValue()
            {
                armor = 200f + 40f * this.Level;
            }

            internal float MaxArmor()
            {
                return 200f + 40f * this.Level;
            }

            public override void AddStats(PLShipStats inStats)
            {
                base.AddStats(inStats);
                inStats.HullArmor += armor / 250f;
                inStats.Mass += 10f + (25f + 10f * this.Level) * (armor / this.MaxArmor());
            }

            public override void Tick()
            {
                base.Tick();
                if (!this.IsEquipped)
                    return;
                armor += 30f * Time.deltaTime;
                armor = Mathf.Clamp(armor, 0f, this.MaxArmor());
            }

            public override string GetStatLineLeft() => PLLocalize.Localize("Armor") + "\n" + PLLocalize.Localize("Armor (Max)") + "\n" + PLLocalize.Localize("Mass") + "\n";

            public override string GetStatLineRight() => (this.armor).ToString("0") + "\n" + (this.MaxArmor()).ToString("0") + "\n" + (10f + (75f + 10f * this.Level) * (armor / this.MaxArmor())).ToString("0") + "\n";
        }

        [HarmonyPatch(typeof(PLShipStats), "TakeHullDamage")]
        internal class UpdateNanoArmor
        {
            private static void Postfix(PLShipStats __instance, float inDmg, EDamageType inDmgType, PLShipInfoBase attackingShip, PLTurret turret, ref float __result)
            {
                if (__instance.GetShipComponent<PLHullPlating>(ESlotType.E_COMP_HULLPLATING) == null)
                    return;

                PLHullPlating hullPlating = __instance.GetShipComponent<PLHullPlating>(ESlotType.E_COMP_HULLPLATING);
                if (hullPlating is NanoActivePlating)
                {
                    (hullPlating as NanoActivePlating).armor = 0f;
                }
            }
        }

        [HarmonyPatch(typeof(PLShipComponent), "AddStats")]
        internal class StatsForBaseHullPlating
        {
            private static bool Prefix(PLShipComponent __instance, PLShipStats inStats)
            {
                float mass = 0f;
                PLHullPlating hullPlating = __instance as PLHullPlating;
                if (hullPlating != null && hullPlating.SubType == (int)EHullPlatingType.E_HULLPLATING_CCGE)
                {
                    mass = (5f * hullPlating.Level);
                    inStats.Mass += (ObscuredFloat)mass;
                    inStats.HullArmor += (55f + (12f * hullPlating.Level)) / 250f;
                }
                return true;
            }
        }

        [HarmonyPatch(typeof(PLWare), "GetStatLineLeft")]
        internal class StatLineForBaseHullPlatingLeft
        {
            private static bool Prefix(PLWare __instance, ref string __result)
            {
                PLHullPlating hullPlating = __instance as PLHullPlating;
                if (hullPlating != null && hullPlating.SubType == (int)EHullPlatingType.E_HULLPLATING_CCGE)
                {
                    __result = PLLocalize.Localize("Armor") + "\n" + PLLocalize.Localize("Bottom Hit Dmg") + "\n" + PLLocalize.Localize("Mass") + "\n";
                    return false;
                }
                return true;
            }
        }

        [HarmonyPatch(typeof(PLWare), "GetStatLineRight")]
        internal class StatLineForBaseHullPlatingRight
        {
            private static bool Prefix(PLWare __instance, ref string __result)
            {
                PLHullPlating hullPlating = __instance as PLHullPlating;
                if (hullPlating != null && hullPlating.SubType == (int)EHullPlatingType.E_HULLPLATING_CCGE)
                {
                    __result = (55f + (12f * hullPlating.Level)).ToString("0") + "\n-" + (65f + (7f * hullPlating.Level)).ToString("0") + "\n" + (50f + (5f * hullPlating.Level)).ToString("0") + "\n";
                    return false;
                }
                return true;
            }
        }

        [HarmonyPatch(typeof(PLShipInfoBase), "TakeDamage")]
        internal class HullPlatingDamageReduction
        {
            private static bool Prefix(PLShipInfoBase __instance, ref float dmg, ref bool bottomHit)
            {
                if (!(__instance is PLShipInfo) || (__instance is PLShipInfo && ((PLShipInfo)__instance).StartupSwitchBoard.GetLateStatus(2)))
                {
                    if (__instance.MyStats.GetShipComponent<PLShieldGenerator>(ESlotType.E_COMP_SHLD) != null)
                    {
                        if (__instance.MyStats.ShieldsCurrent > __instance.MyStats.GetShipComponent<PLShieldGenerator>(ESlotType.E_COMP_SHLD).MinIntegrityAfterDamageScaled)
                            return true;
                    }
                }

                if (__instance.MyStats.GetShipComponent<PLHullPlating>(ESlotType.E_COMP_HULLPLATING) == null)
                    return true;

                if (SensorDish.IsSensorWeaknessActiveReal(__instance, 2, 1))
                    return true;

                PLHullPlating hullPlating = __instance.MyStats.GetShipComponent<PLHullPlating>(ESlotType.E_COMP_HULLPLATING);

                if (bottomHit && __instance.MyStats.GetShipComponent<PLHullPlating>(ESlotType.E_COMP_HULLPLATING) != null)
                {
                    if (hullPlating.SubType == (int)EHullPlatingType.E_HULLPLATING_CCGE)
                    {
                        dmg = Mathf.Clamp(dmg - (65f + (7f * hullPlating.Level)) * (1f - __instance.AcidicAtmoBoostAlpha), dmg * 0.1f, dmg);
                    }
                    else if (hullPlating.SubType == HullPlatingModManager.Instance.GetHullPlatingIDFromName("HeavyDutyPlating"))
                    {
                        dmg = Mathf.Clamp(dmg * (1f - ((32f + (2f * hullPlating.Level)) * (1f - __instance.AcidicAtmoBoostAlpha) / 100f)), dmg * 0.1f, dmg);
                    }
                    else if (hullPlating.SubType == HullPlatingModManager.Instance.GetHullPlatingIDFromName("LightHullPlating"))
                    {
                        dmg = Mathf.Clamp(dmg - (20f + (2f * hullPlating.Level)) * (1f - __instance.AcidicAtmoBoostAlpha), dmg * 0.1f, dmg);
                    }
                    else if (hullPlating.SubType == HullPlatingModManager.Instance.GetHullPlatingIDFromName("AdaptiveArmorPlating"))
                    {
                        if (dmg < 10000f)
                        {
                            dmg = dmg - (dmg * (1 - (1000f / (1000f + dmg))) * (1f - __instance.AcidicAtmoBoostAlpha));
                        }
                    }
                }
                return true;
            }
        }
    }
}
