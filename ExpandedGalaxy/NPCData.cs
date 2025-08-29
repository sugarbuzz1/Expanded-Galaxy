
using HarmonyLib;
using PulsarModLoader.Patches;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Reflection;
using System;
using System.Linq;
using PulsarModLoader.Utilities;

namespace ExpandedGalaxy
{
    internal class NPCData
    {
        [HarmonyPatch(typeof(PLCampaignIO), "GetActorTypeData")]
        internal class CreateActorData
        {
            private static void Postfix(PLCampaignIO __instance, string inActorName, ref ActorTypeData __result)
            {
                if (inActorName == "RACENPC_16")
                {
                    if (__result.OpeningLines.Count >= 4)
                        return;
                    LineData postMissionDialogue = new LineData();
                    postMissionDialogue.TextOptions.Add("Thanks again for your help. While you're here you should check out the noodle bar, they have some good stuff.");
                    postMissionDialogue.Actions.Add(new LineActionData() { Type = "1" });
                    postMissionDialogue.Actions.Add(new LineActionData() { Type = "0" });
                    postMissionDialogue.Requirements.Add(new LineRequirementData()
                    {
                        Type = "13",
                        Parameter = "8000004"
                    });

                    LineData handInMission = new LineData();
                    handInMission.TextOptions.Add("Wow, you guys actually pulled through. Nice work. You guys saved us from a lifetime in a corporate brig. Your ship is ready for you whenever. I've taken the liberty of adding an extra processor slot to it as well. I hope that and the credits makes up for all the trouble getting here.");
                    handInMission.Actions.Add(new LineActionData() { Type = "1" });
                    handInMission.Actions.Add(new LineActionData() { Type = "5", Parameter = "ExGal_TalkVulcanus" });
                    handInMission.Actions.Add(new LineActionData() { Type = "4", Parameter = "8000004" });
                    handInMission.Requirements.Add(new LineRequirementData()
                    {
                        Type = "14",
                        Parameter = "8000004"
                    });
                    handInMission.Requirements.Add(new LineRequirementData()
                    {
                        Type = "21",
                        Parameter = "ExGal_DeliverVulcanus"
                    });

                    LineData whyAreYouHereMission = new LineData();
                    whyAreYouHereMission.TextOptions.Add("Are you crew sent by Lors? What are you doing here? Get that ship or corporate will have all of our heads for this. That's right, you're in on this too. Now get going!");
                    whyAreYouHereMission.Actions.Add(new LineActionData() { Type = "1" });
                    whyAreYouHereMission.Actions.Add(new LineActionData() { Type = "0" });
                    whyAreYouHereMission.Requirements.Add(new LineRequirementData()
                    {
                        Type = "14",
                        Parameter = "8000004"
                    });
                    whyAreYouHereMission.Requirements.Add(new LineRequirementData()
                    {
                        Type = "22",
                        Parameter = "ExGal_ClaimVulcanus"
                    });

                    __result.OpeningLines.Insert(0, whyAreYouHereMission);
                    __result.OpeningLines.Insert(0, handInMission);
                    __result.OpeningLines.Insert(0, postMissionDialogue);
                }
                else if (inActorName == "WD_HUBNPC_14")
                {
                    if (__result.OpeningLines.Count >= 6)
                        return;
                    LineData postMission = new LineData();
                    postMission.TextOptions.Add("Thanks again for your help with Vulcanus. She's now safe and sound thanks to you. A few of my coworkers wanted to extend their thanks to you as well. They all got promotions because of your efforts.");
                    postMission.Actions.Add(new LineActionData() { Type = "1" });
                    postMission.Actions.Add(new LineActionData() { Type = "0" });
                    postMission.Requirements.Add(new LineRequirementData()
                    {
                        Type = "13",
                        Parameter = "8000004"
                    });

                    LineData whyAreYouHere = new LineData();
                    whyAreYouHere.TextOptions.Add("Are you out of your mind? Get the ship out of here before someone notices!");
                    whyAreYouHere.Actions.Add(new LineActionData() { Type = "1" });
                    whyAreYouHere.Actions.Add(new LineActionData() { Type = "0" });
                    whyAreYouHere.Requirements.Add(new LineRequirementData()
                    {
                        Type = "14",
                        Parameter = "8000004"
                    });
                    whyAreYouHere.Requirements.Add(new LineRequirementData()
                    {
                        Type = "21",
                        Parameter = "ExGal_ClaimVulcanus"
                    });

                    LineData missionAlreadyStarted = new LineData();
                    missionAlreadyStarted.TextOptions.Add("Will you please hurry? The longer this waits the more likely corporate finds out about this.");
                    missionAlreadyStarted.Actions.Add(new LineActionData() { Type = "1" });
                    missionAlreadyStarted.Actions.Add(new LineActionData() { Type = "0" });
                    missionAlreadyStarted.Requirements.Add(new LineRequirementData()
                    {
                        Type = "14",
                        Parameter = "8000004"
                    });

                    LineData startMission = new LineData();
                    startMission.TextOptions.Add("Hey remember that cloaking prototype you recovered? Well it turns out the theives got their revenge and have stolen the ship it was meant to be installed on. Do you think you can do me a favor off the books and retrieve it for me? I promise I will make it worth your while.");
                    startMission.Actions.Add(new LineActionData() { Type = "1" });
                    startMission.Actions.Add(new LineActionData() { Type = "0" });
                    startMission.Requirements.Add(new LineRequirementData()
                    {
                        Type = "13",
                        Parameter = "62115"
                    });
                    startMission.Requirements.Add(new LineRequirementData()
                    {
                        Type = "16",
                        Parameter = "2"
                    });
                    startMission.Requirements.Add(new LineRequirementData()
                    {
                        Type = "12",
                        Parameter = "8000004"
                    });

                    LineData startMissionPlayerYes = new LineData();
                    startMissionPlayerYes.TextOptions.Add("Accept");
                    startMissionPlayerYes.IsPlayerLine = true;
                    startMissionPlayerYes.Actions.Add(new LineActionData() { Type = "1" });
                    startMissionPlayerYes.Actions.Add(new LineActionData() { Type = "0" });

                    LineData startMissionPlayerYesText = new LineData();
                    startMissionPlayerYesText.TextOptions.Add("Thank you! I've tracked the crew to this sector. Be careful, the hijackers have been reported to have busted open the lockers and are using the prototype weapons that were inside. Take over the ship - DON'T DESTROY IT - and bring it to Maes Argale at Dutain's Garage. I'll have another crew handle your ship.");
                    startMissionPlayerYesText.Actions.Add(new LineActionData() { Type = "1" });
                    startMissionPlayerYesText.Actions.Add(new LineActionData() { Type = "0" });
                    startMissionPlayerYesText.Actions.Add(new LineActionData() { Type = "3", Parameter = "8000004" });

                    startMission.ChildLines.Add(startMissionPlayerYes);
                    startMissionPlayerYes.ChildLines.Add(startMissionPlayerYesText);

                    LineData startMissionPlayerNo = new LineData();
                    startMissionPlayerNo.TextOptions.Add("Decline");
                    startMissionPlayerNo.IsPlayerLine = true;
                    startMissionPlayerNo.Actions.Add(new LineActionData() { Type = "1" });
                    startMissionPlayerNo.Actions.Add(new LineActionData() { Type = "0" });

                    LineData startMissionPlayerNoText = new LineData();
                    startMissionPlayerNoText.TextOptions.Add("Well that's a shame. Let me know if you change your mind. In the meantime I'll just be here trying to figure out how to not end up rotting in prison.");
                    startMissionPlayerNoText.Actions.Add(new LineActionData() { Type = "1" });
                    startMissionPlayerNoText.Actions.Add(new LineActionData() { Type = "0" });

                    startMission.ChildLines.Add(startMissionPlayerNo);
                    startMissionPlayerNo.ChildLines.Add(startMissionPlayerNoText);

                    __result.OpeningLines.Insert(0, startMission);
                    __result.OpeningLines.Insert(0, missionAlreadyStarted);
                    __result.OpeningLines.Insert(0, whyAreYouHere);
                    __result.OpeningLines.Insert(0, postMission);
                }
                else if (inActorName == "HUBNPC_51")
                {
                    if (__result.OpeningLines.Count >= 9)
                        return;
                    LineData postMissionA = new LineData();
                    postMissionA.TextOptions.Add("I'm still listening in on the command center, but I've got nothing on the Wasted Wing. Have they even noticed it's been detatched?");
                    postMissionA.Actions.Add(new LineActionData() { Type = "1" });
                    postMissionA.Actions.Add(new LineActionData() { Type = "0" });
                    postMissionA.Requirements.Add(new LineRequirementData()
                    {
                        Type = "13",
                        Parameter = "8000005"
                    });

                    LineData postMissionB = new LineData();
                    postMissionB.TextOptions.Add("How's your travels been lately? Hopefully you've not been involved in any more shady buisiness. If I get another job for you I'll make sure to triple check the seller.");
                    postMissionB.Actions.Add(new LineActionData() { Type = "1" });
                    postMissionB.Actions.Add(new LineActionData() { Type = "0" });
                    postMissionB.Requirements.Add(new LineRequirementData()
                    {
                        Type = "13",
                        Parameter = "8000006"
                    });

                    LineData turnInMissionA = new LineData();
                    turnInMissionA.TextOptions.Add("Nice! I'm glad everything went well. Here, take these rifles. I need to get rid of them before a patrol bot sniffs them out. In the meantime, if I find anything interesting, I'll be sure your crew is the first to know.");
                    turnInMissionA.Actions.Add(new LineActionData() { Type = "1" });
                    turnInMissionA.Actions.Add(new LineActionData() { Type = "0" });
                    turnInMissionA.Actions.Add(new LineActionData() { Type = "5", Parameter = "ExGal_ReturnFF" });
                    turnInMissionA.Requirements.Add(new LineRequirementData()
                    {
                        Type = "21",
                        Parameter = "ExGal_MeetContactFF"
                    });
                    turnInMissionA.Requirements.Add(new LineRequirementData()
                    {
                        Type = "14",
                        Parameter = "8000005"
                    });

                    LineData turnInMissionB = new LineData();
                    turnInMissionB.TextOptions.Add("The contact was a Union plant? Uhh... sorry about that, I hope it wasn't too much trouble getting back here. Here, take these rifles as well. I need to get rid of them before a patrol bot finds them.");
                    turnInMissionB.Actions.Add(new LineActionData() { Type = "1" });
                    turnInMissionB.Actions.Add(new LineActionData() { Type = "0" });
                    turnInMissionB.Actions.Add(new LineActionData() { Type = "5", Parameter = "ExGal_ReturnFF" });
                    turnInMissionB.Requirements.Add(new LineRequirementData()
                    {
                        Type = "21",
                        Parameter = "ExGal_MeetContactFF"
                    });
                    turnInMissionB.Requirements.Add(new LineRequirementData()
                    {
                        Type = "14",
                        Parameter = "8000006"
                    });

                    LineData missionAlreadyStartedA = new LineData();
                    missionAlreadyStartedA.TextOptions.Add("Any chance you're going to meet the contact soon? I don't want to keep a gentlemen crew waiting...");
                    missionAlreadyStartedA.Actions.Add(new LineActionData() { Type = "1" });
                    missionAlreadyStartedA.Actions.Add(new LineActionData() { Type = "0" });
                    missionAlreadyStartedA.Requirements.Add(new LineRequirementData()
                    {
                        Type = "14",
                        Parameter = "8000005"
                    });

                    LineData missionAlreadyStartedB = new LineData();
                    missionAlreadyStartedB.TextOptions.Add("Any chance you're going to meet the contact soon? I don't want to keep a gentlemen crew waiting...");
                    missionAlreadyStartedB.Actions.Add(new LineActionData() { Type = "1" });
                    missionAlreadyStartedB.Actions.Add(new LineActionData() { Type = "0" });
                    missionAlreadyStartedB.Requirements.Add(new LineRequirementData()
                    {
                        Type = "14",
                        Parameter = "8000006"
                    });

                    LineData startMission = new LineData();
                    startMission.TextOptions.Add("Have you heard anything about the destroyed section of the station? The command center has been keeping quiet about the whole ordeal. I've decided to take things into my own hands, but I need a middle man to transport something. Have room for an extra job?");
                    startMission.Actions.Add(new LineActionData() { Type = "1" });
                    startMission.Actions.Add(new LineActionData() { Type = "0" });
                    startMission.Requirements.Add(new LineRequirementData()
                    {
                        Type = "19",
                        Parameter = "0"
                    });
                    startMission.Requirements.Add(new LineRequirementData()
                    {
                        Type = "16",
                        Parameter = "2"
                    });
                    startMission.Requirements.Add(new LineRequirementData()
                    {
                        Type = "12",
                        Parameter = "8000005"
                    });
                    startMission.Requirements.Add(new LineRequirementData()
                    {
                        Type = "12",
                        Parameter = "8000006"
                    });

                    LineData startMissionPlayerYes = new LineData();
                    startMissionPlayerYes.TextOptions.Add("Accept");
                    startMissionPlayerYes.IsPlayerLine = true;
                    startMissionPlayerYes.Actions.Add(new LineActionData() { Type = "1" });
                    startMissionPlayerYes.Actions.Add(new LineActionData() { Type = "15", Parameter = "30" });

                    LineData startMissionPlayerYesTextA = new LineData();
                    startMissionPlayerYesTextA.TextOptions.Add("Excellent! I've set up a deal for a frequency scanner so I can intercept transmissions to the command center. Transport it for me and I'll make sure you are well rewarded.");
                    startMissionPlayerYesTextA.Actions.Add(new LineActionData() { Type = "1" });
                    startMissionPlayerYesTextA.Actions.Add(new LineActionData() { Type = "0" });
                    startMissionPlayerYesTextA.Actions.Add(new LineActionData() { Type = "3", Parameter = "8000005" });
                    startMissionPlayerYesTextA.LineTag = ELineTag.LT_SUCCESS;

                    LineData startMissionPlayerYesTextB = new LineData();
                    startMissionPlayerYesTextB.TextOptions.Add("Excellent! I've set up a deal for a frequency scanner so I can intercept transmissions to the command center. Transport it for me and I'll make sure you are well rewarded.");
                    startMissionPlayerYesTextB.Actions.Add(new LineActionData() { Type = "1" });
                    startMissionPlayerYesTextB.Actions.Add(new LineActionData() { Type = "0" });
                    startMissionPlayerYesTextB.Actions.Add(new LineActionData() { Type = "3", Parameter = "8000006" });
                    startMissionPlayerYesTextB.LineTag = ELineTag.LT_FAILURE;

                    startMissionPlayerYes.ChildLines.Add(startMissionPlayerYesTextA);
                    startMissionPlayerYes.ChildLines.Add(startMissionPlayerYesTextB);
                    startMission.ChildLines.Add(startMissionPlayerYes);

                    LineData startMissionPlayerNo = new LineData();
                    startMissionPlayerNo.TextOptions.Add("Decline");
                    startMissionPlayerNo.IsPlayerLine = true;
                    startMissionPlayerNo.Actions.Add(new LineActionData() { Type = "1" });
                    startMissionPlayerNo.Actions.Add(new LineActionData() { Type = "0" });

                    LineData startMissionPlayerNoText = new LineData();
                    startMissionPlayerNoText.TextOptions.Add("Well it was worth a shot. I'll be here if you change your mind. If you find another crew that might be willing please send them my way.");
                    startMissionPlayerNoText.Actions.Add(new LineActionData() { Type = "1" });
                    startMissionPlayerNoText.Actions.Add(new LineActionData() { Type = "0" });

                    startMissionPlayerNo.ChildLines.Add(startMissionPlayerNoText);
                    startMission.ChildLines.Add(startMissionPlayerNo);

                    __result.OpeningLines.Insert(0, startMission);
                    __result.OpeningLines.Insert(0, missionAlreadyStartedA);
                    __result.OpeningLines.Insert(0, missionAlreadyStartedB);
                    __result.OpeningLines.Insert(0, turnInMissionA);
                    __result.OpeningLines.Insert(0, turnInMissionB);
                    __result.OpeningLines.Insert(0, postMissionA);
                    __result.OpeningLines.Insert(0, postMissionB);
                }
                else if (inActorName == "ExGal_ContactFF")
                {
                    ActorTypeData data = new ActorTypeData();
                    data.Name = inActorName;

                    LineData close = new LineData();
                    close.TextOptions.Add("CLOSE TRANSMISSION");
                    close.IsPlayerLine = true;
                    close.Actions.Add(new LineActionData() { Type = "1" });
                    close.Actions.Add(new LineActionData() { Type = "10" });

                    LineData opener2 = new LineData();
                    opener2.TextOptions.Add("You got what you came for, now leave.");
                    opener2.Actions.Add(new LineActionData() { Type = "1" });
                    opener2.Actions.Add(new LineActionData() { Type = "0" });
                    opener2.Requirements.Add(new LineRequirementData()
                    {
                        Type = "21",
                        Parameter = "ExGal_MeetContactFF"
                    });

                    opener2.ChildLines.Add(close);

                    LineData opener = new LineData();
                    opener.TextOptions.Add("...you here for the scanner?");
                    opener.Actions.Add(new LineActionData() { Type = "1" });
                    opener.Actions.Add(new LineActionData() { Type = "0" });
                    opener.Requirements.Add(new LineRequirementData()
                    {
                        Type = "22",
                        Parameter = "ExGal_MeetContactFF"
                    });

                    LineData yes = new LineData();
                    yes.TextOptions.Add("YES");
                    yes.IsPlayerLine = true;
                    yes.Actions.Add(new LineActionData() { Type = "1" });
                    yes.Actions.Add(new LineActionData() { Type = "0" });

                    LineData yesTextA = new LineData();
                    yesTextA.TextOptions.Add("Here it is. You never saw us and we never saw you.");
                    yesTextA.Actions.Add(new LineActionData() { Type = "1" });
                    yesTextA.Actions.Add(new LineActionData() { Type = "0" });
                    yesTextA.Actions.Add(new LineActionData() { Type = "32", Parameter = "ExGal_MeetContactFF" });
                    yesTextA.Requirements.Add(new LineRequirementData() { Type = "14", Parameter = "8000005" });

                    LineData yesTextB = new LineData();
                    yesTextB.TextOptions.Add("This ship is affiliated with the Colonial Union contraband interception agency. Your ship has been flagged for attempting to deal with illicit cargo.");
                    yesTextB.Actions.Add(new LineActionData() { Type = "1" });
                    yesTextB.Actions.Add(new LineActionData() { Type = "0" });
                    yesTextB.Actions.Add(new LineActionData() { Type = "14" });
                    yesTextB.Actions.Add(new LineActionData() { Type = "28" });
                    yesTextB.Actions.Add(new LineActionData() { Type = "5", Parameter = "ExGal_MeetContactFF" });
                    yesTextB.Requirements.Add(new LineRequirementData() { Type = "14", Parameter = "8000006" });

                    yesTextA.ChildLines.Add(close);
                    yesTextB.ChildLines.Add(close);
                    yes.ChildLines.Add(yesTextA);
                    yes.ChildLines.Add(yesTextB);
                    opener.ChildLines.Add(yes);

                    LineData no = new LineData();
                    no.TextOptions.Add("NO");
                    no.IsPlayerLine = true;
                    no.Actions.Add(new LineActionData() { Type = "1" });
                    no.Actions.Add(new LineActionData() { Type = "0" });

                    LineData noText = new LineData();
                    noText.TextOptions.Add("Then you have no business here. Leave.");
                    noText.Actions.Add(new LineActionData() { Type = "1" });
                    noText.Actions.Add(new LineActionData() { Type = "0" });

                    noText.ChildLines.Add(close);
                    no.ChildLines.Add(noText);
                    opener.ChildLines.Add(no);
                    opener.ChildLines.Add(close);



                    data.OpeningLines.Add(opener2);
                    data.OpeningLines.Add(opener);

                    __result = data;
                }
            }
        }

        [HarmonyPatch(typeof(PLDialogueActorInstance), "LateExecuteAction")]
        [HarmonyTranspiler]
        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator il)
        {
            var stateMachineType = AccessTools.Inner(typeof(PLDialogueActorInstance), "<LateExecuteAction>d__");
            var actionEnumField = AccessTools.Field(stateMachineType, "<>7__wrap2");
            var authorityField = AccessTools.Field(stateMachineType, "authority");
            var thisField = AccessTools.Field(stateMachineType, "<>4__this");
            var shipDialogueProp = AccessTools.PropertyGetter(typeof(PLDialogueActorInstance), "ShipDialogue");

            // Target sequence: action = enumerator.Current
            var targetSequence = new List<CodeInstruction>
    {
        new CodeInstruction(OpCodes.Ldarg_0),
        new CodeInstruction(OpCodes.Ldflda, actionEnumField),
        new CodeInstruction(OpCodes.Call, AccessTools.PropertyGetter(typeof(List<LineActionData>.Enumerator), "Current"))
    };

            // Label for skip
            var skipLabel = il.DefineLabel();

            // Patch sequence
            var patchSequence = new List<CodeInstruction>
    {
        // duplicate "action" on stack
        new CodeInstruction(OpCodes.Dup),

        // get action.Type
        new CodeInstruction(OpCodes.Callvirt, AccessTools.PropertyGetter(typeof(LineActionData), "Type")),

        // push "32"
        new CodeInstruction(OpCodes.Ldstr, "32"),

        // string.Equals
        new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(string), "op_Equality", new[] { typeof(string), typeof(string) })),

        // if not equal -> skip
        new CodeInstruction(OpCodes.Brfalse_S, skipLabel),

        // load this (state machine)
        new CodeInstruction(OpCodes.Ldarg_0),

        // load authority field
        new CodeInstruction(OpCodes.Ldfld, authorityField),

        // if authority == false -> skip
        new CodeInstruction(OpCodes.Brfalse_S, skipLabel),

        // load this (state machine)
        new CodeInstruction(OpCodes.Ldarg_0),

        // load <>4__this (PLDialogueActorInstance)
        new CodeInstruction(OpCodes.Ldfld, thisField),

        // call ShipDialogue getter
        new CodeInstruction(OpCodes.Callvirt, shipDialogueProp),

        // if ShipDialogue == false -> skip
        new CodeInstruction(OpCodes.Brfalse_S, skipLabel),

        // --- custom logic call ---
        // stack: [action]
        new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(ActionCaseHandler), nameof(ActionCaseHandler.HandleCase32)))
        {
            labels = { skipLabel } // continuation
        }
    };

            return HarmonyHelpers.PatchBySequence(
                instructions,
                targetSequence,
                patchSequence,
                HarmonyHelpers.PatchMode.AFTER,
                HarmonyHelpers.CheckMode.NONNULL,
                showDebugOutput: true
            );
        }

        public static class ActionCaseHandler {
            public static void HandleCase32(LineActionData action)
            {
                if (PLServer.Instance == null || PLEncounterManager.Instance == null || PLEncounterManager.Instance.PlayerShip == null)
                    return;
                UnityEngine.Debug.Log($"[Harmony] Case 32 triggered with param {action.Parameter}");
                bool flag = false;
                foreach (PLMissionBase missionBase in PLServer.Instance.AllMissions)
                {
                    foreach (PLMissionObjective objective in missionBase.Objectives)
                    {
                        if (objective is PLMissionObjective_PickupComponent && objective.ScriptName == action.Parameter)
                        {
                            flag = true;
                            Traverse traverse = Traverse.Create(objective);
                            PLEncounterManager.Instance.PlayerShip.MyStats.AddShipComponent(PLShipComponent.CreateShipComponentFromHash((int)PLShipComponent.createHashFromInfo((int)traverse.Field("CompType").GetValue(), traverse.Field("SubType").GetValue<int>(), 0, 0, 12)));
                            break;
                        }
                    }
                    if (flag)
                        break;
                }
            }
        }
    }
}
