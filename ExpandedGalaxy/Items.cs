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
