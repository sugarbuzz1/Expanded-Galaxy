using HarmonyLib;
using PulsarModLoader.Patches;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using UnityEngine;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLInGameUI), "Update")]
    internal class PLInGameUIUpdatePatch
    {
        internal static void HandleBossUI(PLInGameUI pLInGameUI)
        {
            if (BossFlag.AllBossFlags.Count < 1)
            {
                PLGlobal.SafeGameObjectSetActive(pLInGameUI.BossUIRoot, true);
                if ((double)pLInGameUI.LastDisplayedBossUITime == 0.0)
                {
                    pLInGameUI.LastDisplayedBossUITime = Time.time;
                    PLMusic.PostEvent("play_sx_playermenu_creworder", pLInGameUI.gameObject);
                }
                bool flag = (UnityEngine.Object)pLInGameUI.BossUI_SpaceTarget != (UnityEngine.Object)null && (UnityEngine.Object)PLAbyssShipInfo.Instance != (UnityEngine.Object)null;
                if (flag && pLInGameUI.BossTitle_AlreadyShownShips.ContainsKey(pLInGameUI.BossUI_SpaceTarget.ShipID))
                    flag = false;
                if (flag && (double)Time.time - (double)pLInGameUI.LastDisplayedBossUITime > 4.0)
                    pLInGameUI.BossTitle_AlreadyShownShips.Add(pLInGameUI.BossUI_SpaceTarget.ShipID, pLInGameUI.BossUI_SpaceTarget);
                if (flag)
                {
                    PLGlobal.SafeGameObjectSetActive(pLInGameUI.Boss_HealthbarRoot, false);
                    PLGlobal.SafeGameObjectSetActive(pLInGameUI.Boss_TitleRoot, true);
                    pLInGameUI.BossUI_TitleTop.text = PLLocalize.Localize(pLInGameUI.BossUI_SpaceTarget.GetNameForUI()).ToUpper();
                    pLInGameUI.BossUI_TitleBottom.text = PLLocalize.Localize(pLInGameUI.BossUI_SpaceTarget.GetShipShortDesc()).ToUpper();
                }
                else
                {
                    PLGlobal.SafeGameObjectSetActive(pLInGameUI.Boss_HealthbarRoot, true);
                    PLGlobal.SafeGameObjectSetActive(pLInGameUI.Boss_TitleRoot, false);
                    pLInGameUI.BossUI_Name.text = PLLocalize.Localize(pLInGameUI.BossUI_SpaceTarget.GetNameForUI());
                    if (pLInGameUI.BossUI_SpaceTarget.IsRelicHunter)
                    {
                        pLInGameUI.BossUI_Title.text = PLLocalize.Localize("RELIC HUNTER");
                        pLInGameUI.BossUI_Title.color = PLGlobal.Instance.ClassColors[4];
                    }
                    else if (pLInGameUI.BossUI_SpaceTarget.IsBountyHunter)
                    {
                        pLInGameUI.BossUI_Title.text = PLLocalize.Localize("BOUNTY HUNTER");
                        pLInGameUI.BossUI_Title.color = PLGlobal.Instance.ClassColors[3];
                    }
                    else if (pLInGameUI.BossUI_SpaceTarget.IsSectorCommander)
                    {
                        pLInGameUI.BossUI_Title.text = PLLocalize.Localize("SECTOR COMMANDER");
                        pLInGameUI.BossUI_Title.color = PLGlobal.Instance.ClassColors[1];
                    }
                    else
                        pLInGameUI.BossUI_Title.text = string.Empty;
                    if ((double)pLInGameUI.BossUI_SpaceTarget.GetHPAlphaCurrent() <= 1.0 / 1000.0)
                        pLInGameUI.BossUI_HP.text = PLLocalize.Localize("Defeated!");
                    else if (pLInGameUI.BossUI_SpaceTarget.MyStats != null)
                    {
                        UnityEngine.UI.Text bossUiHp = pLInGameUI.BossUI_HP;
                        string str1 = Mathf.CeilToInt(pLInGameUI.BossUI_SpaceTarget.MyStats.HullCurrent).ToString("N0");
                        int num1 = Mathf.CeilToInt(pLInGameUI.BossUI_SpaceTarget.MyStats.HullMax);
                        string str2 = num1.ToString("N0");
                        string str3 = str1 + " / " + str2;
                        bossUiHp.text = str3;
                    }
                    else
                        pLInGameUI.BossUI_HP.text = (pLInGameUI.BossUI_SpaceTarget.GetHPAlphaCurrent() * 100f).ToString("0") + "%";
                    pLInGameUI.BossUI_Fill.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 1000f * Mathf.Clamp01(pLInGameUI.BossUI_SpaceTarget.GetHPAlphaCurrent()));
                    pLInGameUI.BossUI_SlowFill.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 999f * Mathf.Clamp01(pLInGameUI.BossUI_SpaceTarget.GetSlowHPAlphaCurrent()));
                    if ((double)Time.time - (double)pLInGameUI.BossUI_SpaceTarget.LastTookDamageTime() < 0.15000000596046448)
                        pLInGameUI.BossUI_Outline.color = (double)Time.time % 0.10000000149011612 < 0.05000000074505806 ? Color.red : Color.black;
                    else
                        pLInGameUI.BossUI_Outline.color = Color.black;
                    if ((double)Time.time - (double)pLInGameUI.BossUI_SpaceTarget.LastTookDamageTime() < 0.15000000596046448)
                        pLInGameUI.BossUI_Name.color = (double)Time.time % 0.10000000149011612 > 0.05000000074505806 ? Color.red : Color.white;
                    else
                        pLInGameUI.BossUI_Name.color = Color.white;
                }
            }
            else
            {
                PLGlobal.SafeGameObjectSetActive(pLInGameUI.BossUIRoot, true);
                if ((double)pLInGameUI.LastDisplayedBossUITime == 0.0)
                {
                    pLInGameUI.LastDisplayedBossUITime = Time.time;
                    PLMusic.PostEvent("play_sx_playermenu_creworder", pLInGameUI.gameObject);
                }
                else
                {
                    PLGlobal.SafeGameObjectSetActive(pLInGameUI.Boss_HealthbarRoot, true);
                    PLGlobal.SafeGameObjectSetActive(pLInGameUI.Boss_TitleRoot, false);
                    pLInGameUI.BossUI_Name.text = BossFlag.AllBossFlags[0].GetBossName();
                    if (pLInGameUI.BossUI_SpaceTarget.IsRelicHunter)
                    {
                        pLInGameUI.BossUI_Title.text = PLLocalize.Localize("RELIC HUNTER");
                        pLInGameUI.BossUI_Title.color = PLGlobal.Instance.ClassColors[4];
                    }
                    else if (pLInGameUI.BossUI_SpaceTarget.IsBountyHunter)
                    {
                        pLInGameUI.BossUI_Title.text = PLLocalize.Localize("BOUNTY HUNTER");
                        pLInGameUI.BossUI_Title.color = PLGlobal.Instance.ClassColors[3];
                    }
                    else if (pLInGameUI.BossUI_SpaceTarget.IsSectorCommander)
                    {
                        pLInGameUI.BossUI_Title.text = PLLocalize.Localize("SECTOR COMMANDER");
                        pLInGameUI.BossUI_Title.color = PLGlobal.Instance.ClassColors[1];
                    }
                    else
                        pLInGameUI.BossUI_Title.text = string.Empty;
                    if ((double)BossFlag.GetCurrentHPAlpha() <= 1.0 / 1000.0)
                        pLInGameUI.BossUI_HP.text = PLLocalize.Localize("Defeated!");
                    else if (pLInGameUI.BossUI_SpaceTarget.MyStats != null)
                    {
                        UnityEngine.UI.Text bossUiHp = pLInGameUI.BossUI_HP;
                        string str1 = Mathf.CeilToInt(BossFlag.GetCurrentHP()).ToString("N0");
                        int num1 = Mathf.CeilToInt(BossFlag.GetMaxHP());
                        string str2 = num1.ToString("N0");
                        string str3 = str1 + " / " + str2;
                        bossUiHp.text = str3;
                    }
                    else
                        pLInGameUI.BossUI_HP.text = (BossFlag.GetCurrentHPAlpha() * 100f).ToString("0") + "%";
                    pLInGameUI.BossUI_Fill.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 1000f * Mathf.Clamp01(BossFlag.GetCurrentHPAlpha()));
                    pLInGameUI.BossUI_SlowFill.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 999f * Mathf.Clamp01(BossFlag.GetLerpedHPAlpha()));
                    if ((double)Time.time - (double)BossFlag.LastTookDamageTime() < 0.15000000596046448)
                        pLInGameUI.BossUI_Outline.color = (double)Time.time % 0.10000000149011612 < 0.05000000074505806 ? Color.red : Color.black;
                    else
                        pLInGameUI.BossUI_Outline.color = Color.black;
                    if ((double)Time.time - (double)BossFlag.LastTookDamageTime() < 0.15000000596046448)
                        pLInGameUI.BossUI_Name.color = (double)Time.time % 0.10000000149011612 > 0.05000000074505806 ? Color.red : Color.white;
                    else
                        pLInGameUI.BossUI_Name.color = Color.white;
                }
            }
        }
        private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            int num = 0;
            int num1 = 0;
            List<CodeInstruction> list = instructions.ToList();

            foreach (CodeInstruction instruction in list)
            {
                if (instruction.opcode == OpCodes.Br)
                {
                    if (num1 < 4)
                        ++num1;
                    else
                        break;
                }
                ++num;
            }

            List<CodeInstruction> targetSequence = new List<CodeInstruction>()
                    {
                        new CodeInstruction(OpCodes.Ldarg_0),
                        new CodeInstruction(OpCodes.Ldfld),
                        new CodeInstruction(OpCodes.Ldnull),
                        new CodeInstruction(OpCodes.Call),
                        new CodeInstruction(OpCodes.Brfalse),

                        new CodeInstruction(OpCodes.Ldsfld),
                        new CodeInstruction(OpCodes.Ldnull),
                        new CodeInstruction(OpCodes.Call),
                        new CodeInstruction(OpCodes.Brfalse),

                        new CodeInstruction(OpCodes.Ldsfld),
                        new CodeInstruction(OpCodes.Ldfld),
                        new CodeInstruction(OpCodes.Call),
                        new CodeInstruction(OpCodes.Brtrue),

                        new CodeInstruction(OpCodes.Ldsfld),
                        new CodeInstruction(OpCodes.Callvirt),
                        new CodeInstruction(OpCodes.Brtrue),

                        new CodeInstruction(OpCodes.Ldsfld),
                        new CodeInstruction(OpCodes.Ldfld),
                        new CodeInstruction(OpCodes.Ldnull),
                        new CodeInstruction(OpCodes.Call),
                        new CodeInstruction(OpCodes.Brfalse),

                        new CodeInstruction(OpCodes.Ldsfld),
                        new CodeInstruction(OpCodes.Ldfld),
                        new CodeInstruction(OpCodes.Ldfld),
                        new CodeInstruction(OpCodes.Ldnull),
                        new CodeInstruction(OpCodes.Call),
                        new CodeInstruction(OpCodes.Brfalse),

                        new CodeInstruction(OpCodes.Ldsfld),
                        new CodeInstruction(OpCodes.Ldfld),
                        new CodeInstruction(OpCodes.Ldfld),
                        new CodeInstruction(OpCodes.Ldfld),
                        new CodeInstruction(OpCodes.Ldarg_0),
                        new CodeInstruction(OpCodes.Ldfld),
                        new CodeInstruction(OpCodes.Callvirt),
                        new CodeInstruction(OpCodes.Brfalse),
                    };
            List<CodeInstruction> patchSequence = new List<CodeInstruction>()
                    {
                        new CodeInstruction(OpCodes.Ldarg_0),
                        new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(PLInGameUIUpdatePatch), "HandleBossUI", new Type[1] {typeof(PLInGameUI)})),
                        new CodeInstruction(OpCodes.Br, (Label)list[num].operand),
                    };

            return HarmonyHelpers.PatchBySequence(list.AsEnumerable<CodeInstruction>(), targetSequence, patchSequence, HarmonyHelpers.PatchMode.AFTER, HarmonyHelpers.CheckMode.NONNULL, false);
        }
    }

}

