using HarmonyLib;
using System;

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
                    yesTextA.Actions.Add(new LineActionData() { Type = "3", Parameter = "8000007" });
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
                else if (inActorName == "ExGal_TreasureFleet_Cruiser")
                {
                    ActorTypeData data = new ActorTypeData();
                    data.Name = inActorName;
                    data.HostileByDefault = true;

                    LineData opener = new LineData();
                    opener.TextOptions.Add("[SHIP_NAME] is declining transmissions.");
                    opener.Actions.Add(new LineActionData() { Type = "1" });

                    data.OpeningLines.Add(opener);

                    __result = data;
                }
                else if (inActorName == "ExGal_TreasureFleet_Friend")
                {
                    ActorTypeData data = new ActorTypeData();
                    data.Name = inActorName;

                    LineData opener1 = new LineData();
                    opener1.TextOptions.Add("You've got the cargo? Take it to the Estate and you'll get paid. Pleasure working with you Gents.");
                    opener1.Actions.Add(new LineActionData() { Type = "1" });
                    opener1.Requirements.Add(new LineRequirementData() { Type = "21", Parameter = "ExGal_TreasureFleet_Cargo" });

                    LineData opener = new LineData();
                    opener.TextOptions.Add("We've engaged the fleet as a distraction. It's up to you to board them and handle the cargo.");
                    opener.Actions.Add(new LineActionData() { Type = "1" });

                    data.OpeningLines.Add(opener1);
                    data.OpeningLines.Add(opener);

                    __result = data;
                }
                else if (inActorName == "AOG1_NPC_14")
                {
                    if (__result.OpeningLines[0].ChildLines.Count >= 2)
                        return;
                    LineData jobOpener = new LineData();
                    jobOpener.TextOptions.Add("Job Offer");
                    jobOpener.Actions.Add(new LineActionData() { Type = "1" });
                    jobOpener.Actions.Add(new LineActionData() { Type = "0" });
                    jobOpener.IsPlayerLine = true;
                    jobOpener.Requirements.Add(new LineRequirementData() { Type = "12", Parameter = "8000008" });
                    jobOpener.Requirements.Add(new LineRequirementData() { Type = "16", Parameter = "2" });
                    jobOpener.Requirements.Add(new LineRequirementData() { Type = "19", Parameter = "1" });

                    LineData jobText = new LineData();
                    jobText.TextOptions.Add("Looking for a job opportunity? I've got intel on some valueable cargo being moved and I need a crew with an empty cargo hold to intercept it. You won't be alone on the job and you'll be paid well. You in?");
                    jobText.Actions.Add(new LineActionData() { Type = "1" });
                    jobText.Actions.Add(new LineActionData() { Type = "0" });

                    jobOpener.ChildLines.Add(jobText);

                    LineData accept = new LineData();
                    accept.TextOptions.Add("Accept");
                    accept.Actions.Add(new LineActionData() { Type = "1" });
                    accept.Actions.Add(new LineActionData() { Type = "0" });
                    accept.IsPlayerLine = true;

                    LineData acceptText = new LineData();
                    acceptText.TextOptions.Add("Perfect. The target is a part of a W.D. transport fleet. It will no doubt be well-armed and well defended. The cargo in question is highly radioactive materials used in the manufacturing of nukes. You'll need to move fast, once the fleet reaches the Corporation's Headquarters it's as good as gone. The location of the fleet has been marked on your map. Deliver the goods to Kadew Rufara in the cargo hold at the Estate and you'll get paid. Now get going!");
                    acceptText.Actions.Add(new LineActionData() { Type = "1" });
                    acceptText.Actions.Add(new LineActionData() { Type = "0" });
                    acceptText.Actions.Add(new LineActionData() { Type = "3", Parameter = "8000008" });
                    acceptText.Actions.Add(new LineActionData() { Type = "3", Parameter = "8000011" });

                    accept.ChildLines.Add(acceptText);

                    LineData decline = new LineData();
                    decline.TextOptions.Add("Decline");
                    decline.Actions.Add(new LineActionData() { Type = "1" });
                    decline.Actions.Add(new LineActionData() { Type = "0" });
                    decline.IsPlayerLine= true;

                    LineData declineText = new LineData();
                    declineText.TextOptions.Add("That's a shame. If you find a good crew looking for a job send them my way.");
                    declineText.Actions.Add(new LineActionData() { Type = "1" });
                    declineText.Actions.Add(new LineActionData() { Type = "0" });

                    decline.ChildLines.Add(declineText);
                    jobText.ChildLines.Add(accept);
                    jobText.ChildLines.Add(decline);
                    __result.OpeningLines[0].ChildLines.Add(jobOpener);

                }
                else if (inActorName == "ESTATE_34")
                {
                    if (__result.OpeningLines[0].ChildLines.Count >= 2)
                        return;

                    LineData postMissionBad = new LineData();
                    postMissionBad.TextOptions.Add("I don't want anything to do with you. Leave.");
                    postMissionBad.Actions.Add(new LineActionData() { Type = "1" });
                    postMissionBad.Actions.Add(new LineActionData() { Type = "0" });
                    postMissionBad.Requirements.Add(new LineRequirementData() { Type = "13", Parameter = "8000011" });

                    LineData deliverOption = new LineData();
                    deliverOption.TextOptions.Add("Irradiated Cargo");
                    deliverOption.Actions.Add(new LineActionData() { Type = "1" });
                    deliverOption.Actions.Add(new LineActionData() { Type = "0" });
                    deliverOption.IsPlayerLine = true;
                    deliverOption.Requirements.Add(new LineRequirementData() { Type = "14", Parameter = "8000008" });
                    deliverOption.Requirements.Add(new LineRequirementData() { Type = "21", Parameter = "ExGal_TreasureFleet_Cargo" });

                    LineData deliverTextBad = new LineData();
                    deliverTextBad.TextOptions.Add("I'm glad to see the job went without a hitch. Although... the Milano hasn't checked in for a while now. I know you killed them. Take your credits and leave.");
                    deliverTextBad.Actions.Add(new LineActionData() { Type = "1" });
                    deliverTextBad.Actions.Add(new LineActionData() { Type = "0" });
                    deliverTextBad.Actions.Add(new LineActionData() { Type = "5", Parameter = "ExGal_TreasureFleet_Hidden_KilledFriend_Finish" });
                    deliverTextBad.Actions.Add(new LineActionData() { Type = "5", Parameter = "ExGal_TreasureFleet_Deliver" });
                    deliverTextBad.Requirements.Add(new LineRequirementData() { Type = "21", Parameter = "ExGal_TreasureFleet_Hidden_KilledFriend" });

                    deliverOption.ChildLines.Add(deliverTextBad);

                    LineData deliverText = new LineData();
                    deliverText.TextOptions.Add("Good work. I'll have the cargo retrieved from you ship. As for payment I'll give you a choice between 20k credits or this prototype extractor that's been sitting here for ages. I think it's old Polytech technology so I understand if you just take the cash.");
                    deliverText.Actions.Add(new LineActionData() { Type = "1" });
                    deliverText.Actions.Add(new LineActionData() { Type = "0" });

                    LineData rewardOptionCredits = new LineData();
                    rewardOptionCredits.TextOptions.Add("Credits");
                    rewardOptionCredits.Actions.Add(new LineActionData() { Type = "1" });
                    rewardOptionCredits.Actions.Add(new LineActionData() { Type = "0" });
                    rewardOptionCredits.IsPlayerLine = true;

                    LineData rewardOptionCreditsText = new LineData();
                    rewardOptionCreditsText.TextOptions.Add("Understandable. Your payment is being wired to you now.");
                    rewardOptionCreditsText.Actions.Add(new LineActionData() { Type = "1" });
                    rewardOptionCreditsText.Actions.Add(new LineActionData() { Type = "0" });
                    rewardOptionCreditsText.Actions.Add(new LineActionData() { Type = "5", Parameter = "ExGal_TreasureFleet_Deliver" });
                    rewardOptionCreditsText.Actions.Add(new LineActionData() { Type = "3", Parameter = "8000009" });
                    rewardOptionCreditsText.Actions.Add(new LineActionData() { Type = "9", Parameter = "8000011" });

                    rewardOptionCredits.ChildLines.Add(rewardOptionCreditsText);

                    LineData rewardOptionExtractor = new LineData();
                    rewardOptionExtractor.TextOptions.Add("Extractor");
                    rewardOptionExtractor.Actions.Add(new LineActionData() { Type = "1" });
                    rewardOptionExtractor.Actions.Add(new LineActionData() { Type = "0" });
                    rewardOptionExtractor.IsPlayerLine = true;

                    LineData rewardOptionExtractorText = new LineData();
                    rewardOptionExtractorText.TextOptions.Add("It's got a little bit of dust on it, but I know it will serve you well. It should already be in your cargo hold.");
                    rewardOptionExtractorText.Actions.Add(new LineActionData() { Type = "1" });
                    rewardOptionExtractorText.Actions.Add(new LineActionData() { Type = "0" });
                    rewardOptionExtractorText.Actions.Add(new LineActionData() { Type = "5", Parameter = "ExGal_TreasureFleet_Deliver" });
                    rewardOptionExtractorText.Actions.Add(new LineActionData() { Type = "3", Parameter = "8000010" });
                    rewardOptionExtractorText.Actions.Add(new LineActionData() { Type = "9", Parameter = "8000011" });

                    rewardOptionExtractor.ChildLines.Add(rewardOptionExtractorText);

                    deliverText.ChildLines.Add(rewardOptionCredits);
                    deliverText.ChildLines.Add(rewardOptionExtractor);
                    deliverOption.ChildLines.Add(deliverText);

                    __result.OpeningLines[0].ChildLines.Add(deliverOption);
                    __result.OpeningLines.Insert(0, postMissionBad);

                }
                else if (inActorName == "BURROWNPC_17")
                {
                    if (__result.OpeningLines[0].ChildLines.Count >= 2)
                        return;

                    LineData missionOpener = new LineData();
                    missionOpener.TextOptions.Add("You're one of the crews that the company put through special training, huh? I've seen it many times before. I'm part of a small guild of disgruntled former delivery crews that all had the company betray them. I could let you in but there is a... small initiation fee.");
                    missionOpener.Actions.Add(new LineActionData() { Type = "1" });
                    missionOpener.Actions.Add(new LineActionData() { Type = "0" });
                    missionOpener.Requirements.Add(new LineRequirementData() { Type = "12", Parameter = "8000012" });
                    missionOpener.Requirements.Add(new LineRequirementData() { Type = "13", Parameter = "36642" });
                    missionOpener.Requirements.Add(new LineRequirementData() { Type = "16", Parameter = "2" });
                    missionOpener.Requirements.Add(new LineRequirementData() { Type = "19", Parameter = "-1" });

                    LineData accept = new LineData();
                    accept.TextOptions.Add("Accept");
                    accept.Actions.Add(new LineActionData() { Type = "1" });
                    accept.Actions.Add(new LineActionData() { Type = "0" });
                    accept.IsPlayerLine = true;

                    LineData acceptText = new LineData();
                    acceptText.TextOptions.Add("I've sent you the details. Eliminate the target and then return to me. If you get caught this meeting never happened.");
                    acceptText.Actions.Add(new LineActionData() { Type = "1" });
                    acceptText.Actions.Add(new LineActionData() { Type = "0" });
                    acceptText.Actions.Add(new LineActionData() { Type = "3", Parameter = "8000012" });

                    accept.ChildLines.Add(acceptText);

                    LineData decline = new LineData();
                    decline.TextOptions.Add("Decline");
                    decline.Actions.Add(new LineActionData() { Type = "1" });
                    decline.Actions.Add(new LineActionData() { Type = "0" });
                    decline.IsPlayerLine = true;

                    LineData declineText = new LineData();
                    declineText.TextOptions.Add("I guess you're as much as a pushover as the company says you are. I hope you don't mind the bounty hunters the company sends when they find out they failed to kill you.");
                    declineText.Actions.Add(new LineActionData() { Type = "1" });
                    declineText.Actions.Add(new LineActionData() { Type = "0" });

                    decline.ChildLines.Add(declineText);

                    missionOpener.ChildLines.Add(accept);
                    missionOpener.ChildLines.Add(decline);
                    __result.OpeningLines.Insert(0, missionOpener);

                    LineData missionHandIn = new LineData();
                    missionHandIn.TextOptions.Add("Target");
                    missionHandIn.Actions.Add(new LineActionData() { Type = "1" });
                    missionHandIn.Actions.Add(new LineActionData() { Type = "0" });
                    missionHandIn.IsPlayerLine = true;
                    missionHandIn.Requirements.Add(new LineRequirementData() { Type = "21", Parameter = "ExGal_BadBiscuit_Kill" });
                    missionHandIn.Requirements.Add(new LineRequirementData() { Type = "14", Parameter = "8000012" });

                    LineData missionHandInText = new LineData();
                    missionHandInText.TextOptions.Add("Ah, yes. I was informed the target was eliminated by another member before you got here. I suppose that means you're in. I've sent you the location of our hideout. It's a decommissioned biscuit factory; I'm sure you've been there before. You'll meet a ship there that has quite the selection of ship components. Welcome to the cause, brothers.");
                    missionHandInText.Actions.Add(new LineActionData() { Type = "1" });
                    missionHandInText.Actions.Add(new LineActionData() { Type = "0" });
                    missionHandInText.Actions.Add(new LineActionData() { Type = "5", Parameter = "ExGal_BadBiscuit_Return" });

                    missionHandIn.ChildLines.Add(missionHandInText);
                    __result.OpeningLines[1].ChildLines.Add(missionHandIn);
                }
                else if (inActorName == "ExGal_FBCarrier")
                {
                    ActorTypeData data = new ActorTypeData();
                    data.Name = inActorName;

                    LineData lineDataOpener = new LineData();
                    lineDataOpener.TextOptions.Add("Salutations [PLAYERSHIP_NAME], I see you've found your way just fine.");
                    lineDataOpener.Actions.Add(new LineActionData() { Type = "1" });
                    lineDataOpener.Actions.Add(new LineActionData() { Type = "0" });

                    LineData lineDataShop = new LineData();
                    lineDataShop.TextOptions.Add("BROWSE EXOTIC GOODS");
                    lineDataShop.IsPlayerLine = true;
                    lineDataShop.Actions.Add(new LineActionData() { Type = "1" });
                    lineDataShop.Actions.Add(new LineActionData { Type = "0" });
                    lineDataShop.Actions.Add(new LineActionData() { Type = "6" });

                    LineData lineDataShopText = new LineData();
                    lineDataShopText.TextOptions.Add("Take your time.");
                    lineDataShopText.Actions.Add(new LineActionData() { Type = "1" });
                    lineDataShopText.Actions.Add(new LineActionData { Type = "0" });

                    LineData lineDataShop2 = new LineData();
                    lineDataShop2.TextOptions.Add("BROWSE EXOTIC GOODS");
                    lineDataShop2.IsPlayerLine = true;
                    lineDataShop2.Actions.Add(new LineActionData() { Type = "1" });
                    lineDataShop2.Actions.Add(new LineActionData() { Type = "6" });

                    LineData lineDataShopClose = new LineData();
                    lineDataShopClose.TextOptions.Add("CLOSE TRANSMISSION");
                    lineDataShopClose.IsPlayerLine = true;
                    lineDataShopClose.Actions.Add(new LineActionData() { Type = "1" });
                    lineDataShopClose.Actions.Add(new LineActionData() { Type = "10" });

                    lineDataShopText.ChildLines.Add(lineDataShop2);
                    lineDataShopText.ChildLines.Add(lineDataShopClose);
                    lineDataShop.ChildLines.Add(lineDataShopText);
                    lineDataOpener.ChildLines.Add(lineDataShop);
                    data.OpeningLines.Add(lineDataOpener);

                    __result = data;
                }
                else if (inActorName == "ExGal_Inspection_Comms")
                {
                    ActorTypeData data = new ActorTypeData();
                    data.Name = inActorName;
                    data.HailOnStart = true;

                    LineData opener = new LineData();
                    opener.TextOptions.Add("[PLAYERSHIP_NAME]: You have been selected for a cargo inspection. Please redirect your course to the nearest inspection station, and you will be compensated for your time. Noncompliance is considered to be a criminal offense and will be reported to the Outpost 448 Command Center. We apologize for the inconvenience and thank you for your cooperation.");
                    opener.Actions.Add(new LineActionData() { Type = "1" });
                    opener.Actions.Add(new LineActionData() { Type = "0" });
                    opener.Actions.Add(new LineActionData() { Type = "3", Parameter = "8000013" });

                    LineData close = new LineData();
                    close.TextOptions.Add("CLOSE TRANSMISSION");
                    close.IsPlayerLine = true;
                    close.Actions.Add(new LineActionData() { Type = "1" });
                    close.Actions.Add(new LineActionData() { Type = "10" });

                    opener.ChildLines.Add(close);

                    data.OpeningLines.Add(opener);

                    __result = data;
                }
                else if (inActorName == "ExGal_ChaosEvent_FuelShortage")
                {
                    ActorTypeData data = new ActorTypeData();
                    data.Name = inActorName;
                    data.HailOnStart = true;

                    LineData opener = new LineData();
                    opener.TextOptions.Add("According to a study by independant sources, fuel capsules are becoming harder to come by in component shops. All crews are advised to limit the use of manual program charging for the time being. The Outpost 448 Department of Galactic Transportation recommends all ships moving cargo to be outfitted with warp drives that prioritize range.");
                    opener.Actions.Add(new LineActionData() { Type = "1" });
                    opener.Actions.Add(new LineActionData() { Type = "0" });

                    LineData close = new LineData();
                    close.TextOptions.Add("CLOSE TRANSMISSION");
                    close.IsPlayerLine = true;
                    close.Actions.Add(new LineActionData() { Type = "1" });
                    close.Actions.Add(new LineActionData() { Type = "10" });

                    opener.ChildLines.Add(close);

                    data.OpeningLines.Add(opener);

                    __result = data;
                }
                else if (inActorName == "ExGal_ChaosEvent_CoolantShortage")
                {
                    ActorTypeData data = new ActorTypeData();
                    data.Name = inActorName;
                    data.HailOnStart = true;

                    LineData opener = new LineData();
                    opener.TextOptions.Add("Urgent update from the Outpost 448 Command Center: Due to the large demand for coolant for Union research projects all ships are to be advised to limit coolant use in their travels. No information on the aformentioned projects has been cleared for public availibilty by the Command Center at this time.");
                    opener.Actions.Add(new LineActionData() { Type = "1" });
                    opener.Actions.Add(new LineActionData() { Type = "0" });

                    LineData close = new LineData();
                    close.TextOptions.Add("CLOSE TRANSMISSION");
                    close.IsPlayerLine = true;
                    close.Actions.Add(new LineActionData() { Type = "1" });
                    close.Actions.Add(new LineActionData() { Type = "10" });

                    opener.ChildLines.Add(close);

                    data.OpeningLines.Add(opener);

                    __result = data;
                }
                else if (inActorName == "ExGal_ChaosEvent_Contraband")
                {
                    ActorTypeData data = new ActorTypeData();
                    data.Name = inActorName;
                    data.HailOnStart = true;

                    LineData opener = new LineData();
                    opener.TextOptions.Add("Urgent update from the Outpost 448 Command Center: A Union-wide crackdown on contraband items has been instated by the Command Center for the time being. All vessels travelling in Union space must be prepared to receive a cargo inspection at any time. Noncompliance is considered to be a criminal offense and will be reported to the Command Center.");
                    opener.Actions.Add(new LineActionData() { Type = "1" });
                    opener.Actions.Add(new LineActionData() { Type = "0" });

                    LineData close = new LineData();
                    close.TextOptions.Add("CLOSE TRANSMISSION");
                    close.IsPlayerLine = true;
                    close.Actions.Add(new LineActionData() { Type = "1" });
                    close.Actions.Add(new LineActionData() { Type = "10" });

                    opener.ChildLines.Add(close);

                    data.OpeningLines.Add(opener);

                    __result = data;
                }
                else if (inActorName == "ExGal_ChaosEvent_ShopStrike")
                {
                    ActorTypeData data = new ActorTypeData();
                    data.Name = inActorName;
                    data.HailOnStart = true;

                    LineData opener = new LineData();
                    opener.TextOptions.Add("After recent regulations drafted by a joint commission by Outpost 448 and Wolden-Dorf officials, general store vendors have gone on strike until these regulations meet their demands. A report by the Outpost 448 Department of Commerce indicates that prices for all goods have increased and many crews are now struggling to find good deals for the contents of their cargo holds.");
                    opener.Actions.Add(new LineActionData() { Type = "1" });
                    opener.Actions.Add(new LineActionData() { Type = "0" });

                    LineData close = new LineData();
                    close.TextOptions.Add("CLOSE TRANSMISSION");
                    close.IsPlayerLine = true;
                    close.Actions.Add(new LineActionData() { Type = "1" });
                    close.Actions.Add(new LineActionData() { Type = "10" });

                    opener.ChildLines.Add(close);

                    data.OpeningLines.Add(opener);

                    __result = data;
                }
                else if (inActorName == "ExGal_ChaosEvent_InfectionBoost")
                {
                    ActorTypeData data = new ActorTypeData();
                    data.Name = inActorName;
                    data.HailOnStart = true;

                    LineData opener = new LineData();
                    opener.TextOptions.Add("Urgent update from the Outpost 448 Command Center: The Infected has recently shown an unprecedented level of hostility. All crews are advised to avoid sectors bordering Infected space by all means necessary. Do not worry, the Command Center is looking into solutions to quell the Infected. Together we survive.");
                    opener.Actions.Add(new LineActionData() { Type = "1" });
                    opener.Actions.Add(new LineActionData() { Type = "0" });

                    LineData close = new LineData();
                    close.TextOptions.Add("CLOSE TRANSMISSION");
                    close.IsPlayerLine = true;
                    close.Actions.Add(new LineActionData() { Type = "1" });
                    close.Actions.Add(new LineActionData() { Type = "10" });

                    opener.ChildLines.Add(close);

                    data.OpeningLines.Add(opener);

                    __result = data;
                }
                else if (inActorName == "ExGal_ChaosEvent_WDOffensive")
                {
                    ActorTypeData data = new ActorTypeData();
                    data.Name = inActorName;
                    data.HailOnStart = true;

                    LineData opener = new LineData();
                    opener.TextOptions.Add("Urgent update from the Outpost 448 Command Center: The Wolden-Dorf Corporation has declared a new offensive against the Alliance of Gentlemen. All civilian vessels are advised to avoid Corporation controlled space for the time being until the conflict is resolved.");
                    opener.Actions.Add(new LineActionData() { Type = "1" });
                    opener.Actions.Add(new LineActionData() { Type = "0" });

                    LineData close = new LineData();
                    close.TextOptions.Add("CLOSE TRANSMISSION");
                    close.IsPlayerLine = true;
                    close.Actions.Add(new LineActionData() { Type = "1" });
                    close.Actions.Add(new LineActionData() { Type = "10" });

                    opener.ChildLines.Add(close);

                    data.OpeningLines.Add(opener);

                    __result = data;
                }
                else if (inActorName == "ExGal_ChaosEvent_ShockDrones")
                {
                    ActorTypeData data = new ActorTypeData();
                    data.Name = inActorName;
                    data.HailOnStart = true;

                    LineData opener = new LineData();
                    opener.TextOptions.Add("Colonial Union scout vessels has reported an increase in Shock Drone sightings throughout the galaxy. The Command Center advises all crews to use extreme caution when traversing neutral sectors.");
                    opener.Actions.Add(new LineActionData() { Type = "1" });
                    opener.Actions.Add(new LineActionData() { Type = "0" });

                    LineData close = new LineData();
                    close.TextOptions.Add("CLOSE TRANSMISSION");
                    close.IsPlayerLine = true;
                    close.Actions.Add(new LineActionData() { Type = "1" });
                    close.Actions.Add(new LineActionData() { Type = "10" });

                    opener.ChildLines.Add(close);

                    data.OpeningLines.Add(opener);

                    __result = data;
                }
                else if (inActorName == "ExGal_ChaosEvent_Deathseekers")
                {
                    ActorTypeData data = new ActorTypeData();
                    data.Name = inActorName;
                    data.HailOnStart = true;

                    LineData opener = new LineData();
                    opener.TextOptions.Add("Colonial Union scout vessels has reported an increase in Deathseeker sightings throughout the galaxy. The Command Center advises all crews to use extreme caution when traversing neutral sectors and to prioritize the use of high-quality thrusters.");
                    opener.Actions.Add(new LineActionData() { Type = "1" });
                    opener.Actions.Add(new LineActionData() { Type = "0" });

                    LineData close = new LineData();
                    close.TextOptions.Add("CLOSE TRANSMISSION");
                    close.IsPlayerLine = true;
                    close.Actions.Add(new LineActionData() { Type = "1" });
                    close.Actions.Add(new LineActionData() { Type = "10" });

                    opener.ChildLines.Add(close);

                    data.OpeningLines.Add(opener);

                    __result = data;
                }
                else if (inActorName == "ExGal_ChaosEvent_PhaseDrones")
                {
                    ActorTypeData data = new ActorTypeData();
                    data.Name = inActorName;
                    data.HailOnStart = true;

                    LineData opener = new LineData();
                    opener.TextOptions.Add("Colonial Union scout vessels has reported an increase in Phase Drone sightings throughout the galaxy. The Command Center advises all crews to use extreme caution when traversing neutral sectors and to prioritize equipping high-quality repair equipment.");
                    opener.Actions.Add(new LineActionData() { Type = "1" });
                    opener.Actions.Add(new LineActionData() { Type = "0" });

                    LineData close = new LineData();
                    close.TextOptions.Add("CLOSE TRANSMISSION");
                    close.IsPlayerLine = true;
                    close.Actions.Add(new LineActionData() { Type = "1" });
                    close.Actions.Add(new LineActionData() { Type = "10" });

                    opener.ChildLines.Add(close);

                    data.OpeningLines.Add(opener);

                    __result = data;
                }
                else if (inActorName == "ExGal_ChaosEvent_LongRangeDisable")
                {
                    ActorTypeData data = new ActorTypeData();
                    data.Name = inActorName;
                    data.HailOnStart = true;

                    LineData opener = new LineData();
                    opener.TextOptions.Add("Urgent update from the Outpost 448 Command Center: All Long Range Warp Gates have been taken offline due to an unknown network disturbance. The Outpost 448 Department of Galactic Transportation is hard at work to resolve the issue as soon as possible.");
                    opener.Actions.Add(new LineActionData() { Type = "1" });
                    opener.Actions.Add(new LineActionData() { Type = "0" });

                    LineData close = new LineData();
                    close.TextOptions.Add("CLOSE TRANSMISSION");
                    close.IsPlayerLine = true;
                    close.Actions.Add(new LineActionData() { Type = "1" });
                    close.Actions.Add(new LineActionData() { Type = "10" });

                    opener.ChildLines.Add(close);

                    data.OpeningLines.Add(opener);

                    __result = data;
                }
                else if (inActorName == "ExGal_ReflectedRift_Start")
                {
                    ActorTypeData data = new ActorTypeData();
                    data.Name = inActorName;
                    data.HailOnStart = true;

                    LineData opener = new LineData();
                    opener.TextOptions.Add("Urgent message from the Outpost 448 Command Center: [PLAYERSHIP_NAME], your crew has been deemed appropriate for a unique assingment. Please make your way to the provided coordinates as fast as you please. A Union vessel will be waiting there to brief you on the assignment. If you are not the crew of [PLAYERSHIP_NAME], disregard this message.");
                    opener.Actions.Add(new LineActionData() { Type = "1" });
                    opener.Actions.Add(new LineActionData() { Type = "0" });
                    opener.Actions.Add(new LineActionData() { Type = "3", Parameter = "8000014" });

                    LineData close = new LineData();
                    close.TextOptions.Add("CLOSE TRANSMISSION");
                    close.IsPlayerLine = true;
                    close.Actions.Add(new LineActionData() { Type = "1" });
                    close.Actions.Add(new LineActionData() { Type = "10" });

                    opener.ChildLines.Add(close);

                    data.OpeningLines.Add(opener);

                    __result = data;
                }
                else if (inActorName == "ExGal_ReflectedRift_NPC")
                {
                    ActorTypeData data = new ActorTypeData();
                    data.Name = inActorName;
                    data.HailOnStart = true;

                    LineData opener = new LineData();
                    opener.TextOptions.Add("You must be the crew sent by the Command Center. I'm here to welcome you to the only thing the Union and the Corporation seem to want to work together on. You of course are going to be the guinea pigs of this operation and be the first manned ship to enter the rift. If you have any questions, feel free to ask.");
                    opener.Actions.Add(new LineActionData() { Type = "1" });
                    opener.Actions.Add(new LineActionData() { Type = "0" });
                    opener.OneTimeLine = true;

                    LineData landing = new LineData();
                    landing.TextOptions.Add("Anything else you would like to know about?");
                    landing.Actions.Add(new LineActionData() { Type = "1" });
                    landing.Actions.Add(new LineActionData() { Type = "0" });

                    LineData close = new LineData();
                    close.TextOptions.Add("CLOSE TRANSMISSION");
                    close.IsPlayerLine = true;
                    close.Actions.Add(new LineActionData() { Type = "1" });
                    close.Actions.Add(new LineActionData() { Type = "10" });

                    LineData back = new LineData();
                    back.TextOptions.Add("BACK");
                    back.Actions.Add(new LineActionData() { Type = "1" });
                    back.IsPlayerLine = true;
                    back.Actions.Add(new LineActionData() { Type = "0" });
                    back.Actions.Add(new LineActionData() { Type = "2", Parameter = "1" });

                    LineData optionAbout = new LineData();
                    optionAbout.TextOptions.Add("ABOUT THE RIFT");
                    optionAbout.IsPlayerLine = true;
                    optionAbout.Actions.Add(new LineActionData() { Type = "1" });
                    optionAbout.Actions.Add(new LineActionData() { Type = "0" });

                    LineData aboutText = new LineData();
                    aboutText.TextOptions.Add("The rift has been an ongoing project for 3 Union-Standard years, though many experts claim that the rift is older than the Union itself. The rift leads to a cluster of sectors not present in any charts on record. These sectors reside in a reflection of this galaxy... well, the current theory is that it is this galaxy. The rift is rather unstable; all the drones we've sent with equipment to handle the reflection had their components fried and were later found floating in a random sector back in this galaxy. If any of your crew were to be using any form of anti-reflection technology I would advise them to deactivate it while inside the rift.");
                    aboutText.Actions.Add(new LineActionData() { Type = "1" });
                    aboutText.Actions.Add(new LineActionData() { Type = "0" });

                    optionAbout.ChildLines.Add(aboutText);
                    aboutText.ParentLineData = landing;
                    aboutText.ChildLines.Add(back);
                    aboutText.ChildLines.Add(close);

                    LineData optionNavigation = new LineData();
                    optionNavigation.TextOptions.Add("NAVIGATION");
                    optionNavigation.IsPlayerLine = true;
                    optionNavigation.Actions.Add(new LineActionData() { Type = "1" });
                    optionNavigation.Actions.Add(new LineActionData() { Type = "0" });

                    LineData navigationText = new LineData();
                    navigationText.TextOptions.Add("Navigating the rift is very simple; your jump computer should be able to do most of the heavy lifting. You won't have a map to help you find your way so you'll need to explore the old-fashioned way. The main challenge is keeping track of where you've been. I would have something to note what sectors you've visited handy, but that's just me.");
                    navigationText.Actions.Add(new LineActionData() { Type = "1" });
                    navigationText.Actions.Add(new LineActionData() { Type = "0" });

                    optionNavigation.ChildLines.Add(navigationText);
                    navigationText.ParentLineData = landing;
                    navigationText.ChildLines.Add(back);
                    navigationText.ChildLines.Add(close);

                    LineData optionExiting = new LineData();
                    optionExiting.TextOptions.Add("EXITING THE RIFT");
                    optionExiting.IsPlayerLine = true;
                    optionExiting.Actions.Add(new LineActionData() { Type = "1" });
                    optionExiting.Actions.Add(new LineActionData() { Type = "0" });

                    LineData exitingText = new LineData();
                    exitingText.TextOptions.Add("The rift is only a smooth trip going in. To get out of the rift just activate your blind jump and you'll be spit out somewhere in this galaxy. It is very important to keep a close eye on your jump fuel. If you run out of capsules in the rift you will be stuck there forever. Also keep an eye on your hull's integrity, blind jumps are very taxing on the ship and it is your ONLY way out. We'd like you to return in one piece and not several scattered throughout the galaxy.");
                    exitingText.Actions.Add(new LineActionData() { Type = "1" });
                    exitingText.Actions.Add(new LineActionData() { Type = "0" });

                    optionExiting.ChildLines.Add(exitingText);
                    exitingText.ParentLineData = landing;
                    exitingText.ChildLines.Add(back);
                    exitingText.ChildLines.Add(close);

                    LineData optionSignals = new LineData();
                    optionSignals.TextOptions.Add("STRANGE SIGNALS");
                    optionSignals.IsPlayerLine = true;
                    optionSignals.Actions.Add(new LineActionData() { Type = "1" });
                    optionSignals.Actions.Add(new LineActionData() { Type = "0" });

                    LineData signalsText = new LineData();
                    signalsText.TextOptions.Add("While our scout drones were in the rift they picked up some very peculiar signals before their components got fried. We're still unsure of where the signals are originating from but that's what you're here for. There are still some drones in the rift that carry what they could decode from the signals with them. These drones have been corrupted and WILL be hostile. You have the Union's full authority to engage and destroy these drones. Don't worry about destroying the data they have, it'll be stored on a hard-drive in a blast-proof container. Destroy the drones, retrieve the containers, and upload the data to your ship's computer and see if you can make anything of it.");
                    signalsText.Actions.Add(new LineActionData() { Type = "1" });
                    signalsText.Actions.Add(new LineActionData() { Type = "0" });

                    optionSignals.ChildLines.Add(signalsText);
                    signalsText.ParentLineData = landing;
                    signalsText.ChildLines.Add(back);
                    signalsText.ChildLines.Add(close);

                    opener.ChildLines.Add(optionAbout);
                    opener.ChildLines.Add(optionNavigation);
                    opener.ChildLines.Add(optionExiting);
                    opener.ChildLines.Add(optionSignals);
                    opener.ChildLines.Add(close);

                    landing.ChildLines.Add(optionAbout);
                    landing.ChildLines.Add(optionNavigation);
                    landing.ChildLines.Add(optionExiting);
                    landing.ChildLines.Add(optionSignals);
                    landing.ChildLines.Add(close);

                    data.OpeningLines.Add(opener);
                    data.OpeningLines.Add(landing);

                    __result = data;
                }
            }
        }

        [HarmonyPatch(typeof(LineRequirementData), "Passes")]
        internal class ExtraLineReqDataCases
        {
            private static void Postfix(LineRequirementData __instance, ref PLDialogueActorInstance dai, ref PLPersistantShipInfo optionalPSIContext, ref PLFluffyRankingUI optionalFluffyRankingUI, ref bool __result)
            {
                bool flag = true;
                int result;
                if (!int.TryParse(__instance.Parameter, out result))
                {
                    result = -1;
                    flag = false;
                }
                if (!((Object)PLServer.Instance != (Object)null))
                {
                    __result = true;
                    return;
                }
                switch(__instance.Type)
                {
                    case "38":
                        __result = PLServer.Instance.BiscuitContestIsOver.GetDecrypted() && !PLServer.Instance.PlayerCrew_WonFBContest.GetDecrypted();
                        break;
                }
            }
        }
    }
}
