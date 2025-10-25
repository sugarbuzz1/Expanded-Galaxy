using CodeStage.AntiCheat.ObscuredTypes;
using HarmonyLib;
using PulsarModLoader.Content.Items;
using System.Collections;
using UnityEngine;

namespace ExpandedGalaxy
{
    internal class Items
    {
        public static GameObject JetpackCanisterPrefab = null;
        public class JetpackCanisterMod : ItemMod
        {
            public override string Name => "Jetpack Canister";

            public override PLPawnItem PLPawnItem => new PLPawnItem_JetpackCanister();
        }

        public class AutoRifleMod : ItemMod
        {
            public override string Name => "Auto Rifle";

            public override PLPawnItem PLPawnItem => new PLPawnItem_AutoRifle();
        }
        public class PLPawnItem_JetpackCanister : PLPawnItem
        {
            public PLPawnItem_JetpackCanister() : base(EPawnItemType.E_HANDS)
            {
                ItemModManager.Instance.GetItemIDsFromName("Jetpack Canister", out int MainType, out int Subtype);
                this.Name = "Jetpack Canister";
                this.PawnItemType = (EPawnItemType)MainType;
                this.SubType = Subtype;
                this.CanBeEquipped = false;
                this.Weight = 3f;
                this.Desc = "Fully refills jetpack fuel";
                this.m_MarketPrice = (ObscuredInt)1200;
                this.AddAvailableAction("Use");
            }

            public override string GetItemName(bool skipLocalization = false) => "Jetpack Canister";

            public override string GetTypeString() => "Jetpack Fuel";

            public override GameObject GetVisualPrefab()
            {
                if ((UnityEngine.Object)JetpackCanisterPrefab != (UnityEngine.Object)null)
                    return JetpackCanisterPrefab;
                foreach (GameObject gameObject in UnityEngine.Object.FindObjectsOfType<GameObject>())
                {
                    if (gameObject.name == "Lab_Bottle_01")
                    {
                        JetpackCanisterPrefab = UnityEngine.Object.Instantiate(gameObject);
                        JetpackCanisterPrefab.layer = 0;
                        JetpackCanisterPrefab.transform.position = Vector3.zero;
                        return JetpackCanisterPrefab;
                    }
                }
                return base.GetVisualPrefab();
            }

            public override bool OnAction(string inAction)
            {
                if ((Object)this.MySetupPawn == (Object)null || (Object)PLServer.Instance == (Object)null)
                    return base.OnAction(inAction);
                if (inAction == "Use")
                {
                    PLTabMenu.Instance.ShouldRecreateLocalInventory = true;
                    if (this.MySetupPawn.MyPlayer != null && this.MySetupPawn.MyController != null)
                    {
                        this.MySetupPawn.StartCoroutine(this.LateJetpackRefill());
                    }
                    this.MyInventory.RemoveItem(this);
                }
                return base.OnAction(inAction);
            }

            private IEnumerator LateJetpackRefill()
            {
                PLPawnItem_JetpackCanister pLPawnItem_JetpackCanister = this;
                yield return (object)new WaitForEndOfFrame();
                yield return (object)new WaitForEndOfFrame();
                if (pLPawnItem_JetpackCanister.MySetupPawn != null && !pLPawnItem_JetpackCanister.MySetupPawn.IsDead && pLPawnItem_JetpackCanister.MySetupPawn.MyController != null)
                {
                    pLPawnItem_JetpackCanister.MySetupPawn.MyController.JetpackFuel = 1f;
                }
            }
        }

        public class PLPawnItem_AutoRifle : PLPawnItem_PhasePistol
        {
            public PLPawnItem_AutoRifle()
            {
                ItemModManager.Instance.GetItemIDsFromName("Auto Rifle", out int MainType, out int Subtype);
                this.GunFireEvent = "play_sx_player_item_handcannon_shoot";
                this.Desc = "A modified burst rifle that is no longer limited to 3-round volleys.";
                this.m_MarketPrice = (ObscuredInt)4000;
                this.MinAutoFireDelay = 0.1f;
                this.PawnWalkSpeed = (ObscuredFloat)3.5f;
                this.PawnItemType = (EPawnItemType)MainType;
                this.SubType = Subtype;
                this.Accuracy = 1.8f;
                this.AnimName_Fire = "";
                this.m_AnimID = 4;
                this.UsesAmmo = true;
                this.AmmoMax = 60;
                this.AmmoCurrent = this.AmmoMax;
                this.invCameraFOVMultipler = 1.6f;
            }

            public override void FireShot(
                Vector3 aimAtPoint,
                Vector3 destNormal,
                int newBoltID,
                Collider hitCollider)
            {
                base.FireShot(aimAtPoint, destNormal, newBoltID, hitCollider);
                this.Heat -= 0.06f;
                this.MySetupPawn.VerticalMouseLook.RotationY_AddOverTime += 1.1f;
                this.MySetupPawn.VerticalMouseLook.RotationY += 0.35f;
            }

            protected override GameObject CreateBoltGO() => Object.Instantiate<GameObject>(PLGlobal.Instance.HeavyPistolBoltPrefab, this.GetBoltSpawnPos(), Quaternion.identity);

            protected override float CalcDamageDone() => (float)(14.0 + 6.0 * (double)this.Level);

            public override string GetItemName(bool skipLocalization = false) => PLLocalize.Localize("Auto Rifle", skipLocalization) + this.GetItemEnd();

            protected override GameObject GetGunPrefab() => PLGlobal.Instance.BurstRiflePrefab;
        }


        [HarmonyPatch(typeof(PLShopkeeper_Random), "CreateInitialWares")]
        internal class CanisterInShops
        {
            private static void Postfix(PLShopkeeper_Random __instance, TraderPersistantDataEntry inPDE)
            {
                if (inPDE == null)
                    return;
                PLRand rand = new PLRand(PLGlobal.Instance.Galaxy.Seed + __instance.MyPEI.GetSectorID());
                for (int i = 0; i < 3; i++)
                {
                    if (rand.Next(100) > 75)
                    {
                        ItemModManager.Instance.GetItemIDsFromName("Jetpack Canister", out int MainType, out int Subtype);
                        PLPawnItem item = ItemModManager.CreatePawnItem(MainType, Subtype, 0);
                        if (item != null)
                            inPDE.ServerAddWare(item);
                    }
                }
            }
        }

        [HarmonyPatch(typeof(PawnStatusEffect), "GetTypeAsString")]
        internal class PawnStatusEffectText
        {
            private static void Postfix(PawnStatusEffect __instance, ref string __result)
            {
                if ((int)__instance.Type < 16)
                    return;
                switch ((int)__instance.Type)
                {
                    case 16:
                        __result = "Speed Boost";
                        break;
                }
            }
        }

        [HarmonyPatch(typeof(PLPawnItem_Food), "OnAction")]
        internal class PawnItemFoodAction
        {
            private static bool Prefix(PLPawnItem_Food __instance, string inAction)
            {
                Traverse traverse = Traverse.Create(__instance);
                if (traverse.Field("MySetupPawn").GetValue<PLPawn>() == null)
                    return true;
                if (inAction == "Eat")
                {
                    if (PLServer.Instance != null)
                    {
                        PawnStatusEffect pawnStatusEffect = null;
                        if (__instance.SubType == (int)EFoodType.SUGAR_BISCUIT)
                        {
                            pawnStatusEffect = new PawnStatusEffect(PLServer.Instance.GetEstimatedServerMs(), (EPawnStatusEffectType)16, traverse.Method("GenericStatMultiplierBasedOnCookedLevel").GetValue<float>());
                        }
                        if (pawnStatusEffect != null)
                        {
                            bool flag = false;
                            foreach (PawnStatusEffect statusEffect in traverse.Field("MySetupPawn").GetValue<PLPawn>().MyStatusEffects)
                            {
                                if (statusEffect.Type == pawnStatusEffect.Type)
                                {
                                    flag = true;
                                    statusEffect.EndTime += pawnStatusEffect.EndTime - pawnStatusEffect.StartTime;
                                    break;
                                }
                            }
                            if (flag)
                                return true;
                            traverse.Field("MySetupPawn").GetValue<PLPawn>().MyStatusEffects.Add(pawnStatusEffect);
                        }
                    }
                }
                return true;
            }
        }

        [HarmonyPatch(typeof(PLPlayerController), "HandleMovement")]
        internal class SpeedBoost
        {
            private static void Prefix(PLPlayerController __instance)
            {
                if (!(bool)(UnityEngine.Object)__instance.MyPawn || !(bool)(UnityEngine.Object)__instance.MyPawn.GetPlayer())
                    return;
                bool speedFlag = false;
                foreach (PawnStatusEffect statusEffect in __instance.MyPawn.MyStatusEffects)
                {
                    if (statusEffect != null && (int)statusEffect.Type == 16)
                    {
                        speedFlag = true;
                        break;
                    }
                }
                __instance.DefaultPawnSpeed = speedFlag ? 6f : 4f;
                __instance.SprintPawnSpeed = speedFlag ? 9f : 6f;
            }
        }

        [HarmonyPatch(typeof(PLPawnItem_HeldBeamPistol_WithHealing), MethodType.Constructor)]
        internal class HealBeamNotWeapon
        {
            private static void Postfix(PLPawnItem_HeldBeamPistol_WithHealing __instance)
            {
                __instance.MyUtilityType = EItemUtilityType.E_NONE;
            }
        }
    }
}
