using CodeStage.AntiCheat.ObscuredTypes;
using PulsarModLoader.Content.Items;
using System.Collections;
using UnityEngine;

namespace ExpandedGalaxy
{
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
            if ((UnityEngine.Object)Items.JetpackCanisterPrefab != (UnityEngine.Object)null)
                return Items.JetpackCanisterPrefab;
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
}
