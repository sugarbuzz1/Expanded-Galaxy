using HarmonyLib;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLCampaignIO), "GetActorTypeData")]
    internal class CreateActorData
    {
        private static void Postfix(PLCampaignIO __instance, string inActorName, ref ActorTypeData __result)
        {
            if (inActorName == "ExGal_RelicCaravan")
            {
                ActorTypeData data = new ActorTypeData();
                data.Name = inActorName;

                LineData lineDataOpener = new LineData();
                lineDataOpener.TextOptions.Add("Greetings [PLAYERSHIP_NAME], come all this way to look at our stock? I guarentee you can't find quality like these anywhere else in the galaxy!");
                lineDataOpener.Actions.Add(new LineActionData() { Type = "1" });
                lineDataOpener.Actions.Add(new LineActionData() { Type = "0" });

                LineData lineDataShop = new LineData();
                lineDataShop.TextOptions.Add("BROWSE EXOTIC GOODS");
                lineDataShop.IsPlayerLine = true;
                lineDataShop.Actions.Add(new LineActionData() { Type = "1" });
                lineDataShop.Actions.Add(new LineActionData { Type = "0" });
                lineDataShop.Actions.Add(new LineActionData() { Type = "6" });

                LineData lineDataShopText = new LineData();
                lineDataShopText.TextOptions.Add("Take all the time you need.");
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

                LineData lineDataDestination = new LineData();
                lineDataDestination.TextOptions.Add("DESTINATION");
                lineDataDestination.IsPlayerLine = true;
                lineDataDestination.Actions.Add(new LineActionData() { Type = "1" });
                lineDataDestination.Actions.Add(new LineActionData() { Type = "0" });

                LineData lineDataDestinationText = new LineData();
                lineDataDestinationText.TextOptions.Add(GetTextForDestinationSector());
                lineDataDestinationText.Actions.Add(new LineActionData() { Type = "1" });
                lineDataDestinationText.Actions.Add(new LineActionData() { Type = "0" });

                lineDataDestination.ChildLines.Add(lineDataDestinationText);
                lineDataDestinationText.ChildLines.Add(lineDataShopClose);
                lineDataOpener.ChildLines.Add(lineDataDestination);

                LineData lineDataMissionOption = new LineData();
                lineDataMissionOption.TextOptions.Add("\"SPECIAL\" OFFER");
                lineDataMissionOption.IsPlayerLine = true;
                lineDataMissionOption.Actions.Add(new LineActionData() { Type = "1" });
                lineDataMissionOption.Actions.Add(new LineActionData { Type = "0" });
                lineDataMissionOption.Requirements.Add(new LineRequirementData { Type = "12", Parameter = "8000000" });

                LineData lineDataMissionDesc = new LineData();
                lineDataMissionDesc.TextOptions.Add("I'm looking for something. On the outside it looks like a worthless hunk of garbage, but it's actully considered a high deity to some denizens of the galaxy. If you can find it and bring it to me, I will make it worth your while.");
                lineDataMissionDesc.Actions.Add(new LineActionData() { Type = "1" });
                lineDataMissionDesc.Actions.Add(new LineActionData { Type = "0" });

                LineData lineDataAcceptMission = new LineData();
                lineDataAcceptMission.TextOptions.Add("ACCEPT");
                lineDataAcceptMission.IsPlayerLine = true;
                lineDataAcceptMission.Actions.Add(new LineActionData() { Type = "1" });
                lineDataAcceptMission.Actions.Add(new LineActionData() { Type = "0" });

                LineData lineDataAcceptMissionDesc = new LineData();
                lineDataAcceptMissionDesc.TextOptions.Add("Excellent! When you find it bring it to me and I'll take a look at it.");
                lineDataAcceptMissionDesc.Actions.Add(new LineActionData() { Type = "1" });
                lineDataAcceptMissionDesc.Actions.Add(new LineActionData() { Type = "0" });
                lineDataAcceptMissionDesc.Actions.Add(new LineActionData() { Type = "3", Parameter = "8000000" });

                LineData lineDataDeclineMission = new LineData();
                lineDataDeclineMission.TextOptions.Add("DECLINE");
                lineDataDeclineMission.IsPlayerLine = true;
                lineDataDeclineMission.Actions.Add(new LineActionData() { Type = "1" });
                lineDataDeclineMission.Actions.Add(new LineActionData { Type = "10" });


                lineDataAcceptMissionDesc.ChildLines.Add(lineDataShopClose);
                lineDataAcceptMission.ChildLines.Add(lineDataAcceptMissionDesc);
                lineDataMissionDesc.ChildLines.Add(lineDataAcceptMission);
                lineDataMissionDesc.ChildLines.Add(lineDataDeclineMission);
                lineDataMissionOption.ChildLines.Add(lineDataMissionDesc);
                lineDataOpener.ChildLines.Add(lineDataMissionOption);

                LineData lineDataMissionOption2 = new LineData();
                lineDataMissionOption2.TextOptions.Add("GIVE JUNK CUBE");
                lineDataMissionOption2.IsPlayerLine = true;
                lineDataMissionOption2.Actions.Add(new LineActionData() { Type = "1" });
                lineDataMissionOption2.Actions.Add(new LineActionData() { Type = "0" });
                lineDataMissionOption2.Requirements.Add(new LineRequirementData() { Type = "21", Parameter = "ExGal_GotJunkCube1" });
                lineDataMissionOption2.Requirements.Add(new LineRequirementData { Type = "12", Parameter = "8000001" });

                LineData lineDataAcceptMission2Desc = new LineData();
                lineDataAcceptMission2Desc.TextOptions.Add("Incredible! I'm suprised you were able to find it in a galaxy so vast... assuming what you've brought me is authentic that is! I'll need some time to make sure what you brought me isn't actually junk. Come find me later and I'll give you your reward.");
                lineDataAcceptMission2Desc.Actions.Add(new LineActionData() { Type = "1" });
                lineDataAcceptMission2Desc.Actions.Add(new LineActionData() { Type = "0" });
                lineDataAcceptMission2Desc.Actions.Add(new LineActionData() { Type = "5", Parameter = "ExGal_DeliverJunkCube1" });
                lineDataAcceptMission2Desc.Actions.Add(new LineActionData() { Type = "8", Parameter = "8000000" });
                lineDataAcceptMission2Desc.Actions.Add(new LineActionData() { Type = "3", Parameter = "8000001" });

                lineDataAcceptMission2Desc.ChildLines.Add(lineDataShopClose);
                lineDataMissionOption2.ChildLines.Add(lineDataAcceptMission2Desc);
                lineDataOpener.ChildLines.Add(lineDataMissionOption2);

                LineData lineDataMissionOption3 = new LineData();
                lineDataMissionOption3.TextOptions.Add("JUNK CUBE");
                lineDataMissionOption3.IsPlayerLine = true;
                lineDataMissionOption3.Actions.Add(new LineActionData() { Type = "1" });
                lineDataMissionOption3.Actions.Add(new LineActionData() { Type = "0" });
                lineDataMissionOption3.Requirements.Add(new LineRequirementData() { Type = "21", Parameter = "ExGal_WaitJunkCube1" });
                lineDataMissionOption3.Requirements.Add(new LineRequirementData { Type = "12", Parameter = "8000002" });

                LineData lineDataAcceptMission3Desc = new LineData();
                lineDataAcceptMission3Desc.TextOptions.Add("I was wondering when you'd return for that. Unfortunatly some bandits took off with it while we were stopped at the Burrow. They kept shouting \"Praise be the Cube,\" whatever that means. It's safe to say I am never taking my business there again. I've tracked them to this sector, bring it back to me so I can finish looking it over and get you your reward.");
                lineDataAcceptMission3Desc.Actions.Add(new LineActionData() { Type = "1" });
                lineDataAcceptMission3Desc.Actions.Add(new LineActionData() { Type = "0" });
                lineDataAcceptMission3Desc.Actions.Add(new LineActionData() { Type = "5", Parameter = "ExGal_WaitJunkCubeReturn1" });
                lineDataAcceptMission3Desc.Actions.Add(new LineActionData() { Type = "8", Parameter = "8000001" });
                lineDataAcceptMission3Desc.Actions.Add(new LineActionData() { Type = "3", Parameter = "8000002" });

                lineDataAcceptMission3Desc.ChildLines.Add(lineDataShopClose);
                lineDataMissionOption3.ChildLines.Add(lineDataAcceptMission3Desc);
                lineDataOpener.ChildLines.Add(lineDataMissionOption3);

                LineData lineDataMissionOption4 = new LineData();
                lineDataMissionOption4.TextOptions.Add("GIVE JUNK CUBE");
                lineDataMissionOption4.IsPlayerLine = true;
                lineDataMissionOption4.Actions.Add(new LineActionData() { Type = "1" });
                lineDataMissionOption4.Actions.Add(new LineActionData() { Type = "0" });
                lineDataMissionOption4.Requirements.Add(new LineRequirementData() { Type = "21", Parameter = "ExGal_GotJunkCube2" });
                lineDataMissionOption4.Requirements.Add(new LineRequirementData { Type = "12", Parameter = "8000003" });

                LineData lineDataAcceptMission4Desc = new LineData();
                lineDataAcceptMission4Desc.TextOptions.Add("Nice work. I won't lose it this time I promise! Once I finish examining this come find me and I'll give you your reward.");
                lineDataAcceptMission4Desc.Actions.Add(new LineActionData() { Type = "1" });
                lineDataAcceptMission4Desc.Actions.Add(new LineActionData() { Type = "0" });
                lineDataAcceptMission4Desc.Actions.Add(new LineActionData() { Type = "5", Parameter = "ExGal_DeliverJunkCube2" });
                lineDataAcceptMission4Desc.Actions.Add(new LineActionData() { Type = "8", Parameter = "8000002" });
                lineDataAcceptMission4Desc.Actions.Add(new LineActionData() { Type = "3", Parameter = "8000003" });

                lineDataAcceptMission4Desc.ChildLines.Add(lineDataShopClose);
                lineDataMissionOption4.ChildLines.Add(lineDataAcceptMission4Desc);
                lineDataOpener.ChildLines.Add(lineDataMissionOption4);

                LineData lineDataMissionReward = new LineData();
                lineDataMissionReward.TextOptions.Add("REWARD");
                lineDataMissionReward.IsPlayerLine = true;
                lineDataMissionReward.Actions.Add(new LineActionData() { Type = "1" });
                lineDataMissionReward.Actions.Add(new LineActionData() { Type = "0" });
                lineDataMissionReward.Requirements.Add(new LineRequirementData() { Type = "21", Parameter = "ExGal_WaitJunkCube2" });
                lineDataMissionReward.Requirements.Add(new LineRequirementData() { Type = "14", Parameter = "8000003" });

                LineData lineDataMissionRewardDesc = new LineData();
                lineDataMissionRewardDesc.TextOptions.Add("Ah! My favorite relic chasing crew returns! That Junk Cube is 100% geniune, and is now a prized part of my collection. Here's your reward after all of that, it should already be in your cargo hold.");
                lineDataMissionRewardDesc.Actions.Add(new LineActionData() { Type = "1" });
                lineDataMissionRewardDesc.Actions.Add(new LineActionData() { Type = "0" });
                lineDataMissionRewardDesc.Actions.Add(new LineActionData() { Type = "5", Parameter = "ExGal_WaitJunkCubeReturn2" });
                lineDataMissionRewardDesc.Actions.Add(new LineActionData() { Type = "3", Parameter = "8000016" });

                lineDataMissionRewardDesc.ChildLines.Add(lineDataShopClose);
                lineDataMissionReward.ChildLines.Add(lineDataMissionRewardDesc);
                lineDataOpener.ChildLines.Add(lineDataMissionReward);

                LineData map = new LineData();
                map.TextOptions.Add("MAP");
                map.IsPlayerLine = true;
                map.Actions.Add(new LineActionData() { Type = "1" });
                map.Actions.Add(new LineActionData() { Type = "0" });
                map.Requirements.Add(new LineRequirementData() { Type = "12", Parameter = "8000018" });
                map.Requirements.Add(new LineRequirementData() { Type = "14", Parameter = "8000017" });

                LineData mapDesc = new LineData();
                mapDesc.TextOptions.Add("Ah yes, the map. We found it in the wreckage of a ship. Poor bastard warped into a sector with some shock drones. I would go investigate it myself but I've got a business to uphold here. Go check it out and come back with your findings.");
                mapDesc.Actions.Add(new LineActionData() { Type = "1" });
                mapDesc.Actions.Add(new LineActionData() { Type = "0" });
                mapDesc.Actions.Add(new LineActionData() { Type = "5", Parameter = "ExGal_Interlude_Return" });
                mapDesc.Actions.Add(new LineActionData() { Type = "3", Parameter = "8000018" });
                mapDesc.Actions.Add(new LineActionData() { Type = "3", Parameter = "8000019" });

                map.ChildLines.Add(mapDesc);
                mapDesc.ChildLines.Add(lineDataShopClose);
                lineDataOpener.ChildLines.Add(map);

                LineData interludeOpener = new LineData();
                interludeOpener.TextOptions.Add("Oh you're back... what did you find?");
                interludeOpener.Actions.Add(new LineActionData() { Type = "1" });
                interludeOpener.Actions.Add(new LineActionData() { Type = "0" });
                interludeOpener.Requirements.Add(new LineRequirementData() { Type = "21", Parameter = "ExGal_TheMap_Decipher" });

                LineData interludeJunkCube = new LineData();
                interludeJunkCube.TextOptions.Add("JUNK CUBE");
                interludeJunkCube.IsPlayerLine = true;
                interludeJunkCube.Actions.Add(new LineActionData() { Type = "1" });
                interludeJunkCube.Actions.Add(new LineActionData() { Type = "0" });

                LineData interludeRefusal = new LineData();
                interludeRefusal.TextOptions.Add("You want the Junk Cube? Absolutely not. It has become a prized part of my collection. It speaks to me... It has shown me many wonderous things. I can see now why those bandits tried to take the Cube. It is not just a block of garbage, but an entity of divine power. Ask for it again and I will not hesitate to open fire.");
                interludeRefusal.Actions.Add(new LineActionData() { Type = "1" });
                interludeRefusal.Actions.Add(new LineActionData() { Type = "0" });

                LineData interludePressDemands = new LineData();
                interludePressDemands.TextOptions.Add("PRESS DEMANDS");
                interludePressDemands.IsPlayerLine = true;
                interludePressDemands.Actions.Add(new LineActionData() { Type = "1" });
                interludePressDemands.Actions.Add(new LineActionData() { Type = "0" });

                LineData interludeHostile = new LineData();
                interludeHostile.TextOptions.Add("So be it. Pray the Cube has mercy on your soul.");
                interludeHostile.Actions.Add(new LineActionData() { Type = "1" });
                interludeHostile.Actions.Add(new LineActionData() { Type = "0" });
                interludeHostile.Actions.Add(new LineActionData() { Type = "5", Parameter = "ExGal_TheMap_Hidden_TalkCaravan" });
                interludeHostile.Actions.Add(new LineActionData() { Type = "14" });

                interludeOpener.ChildLines.Add(interludeJunkCube);
                interludeOpener.ChildLines.Add(lineDataShopClose);
                interludeJunkCube.ChildLines.Add(interludeRefusal);
                interludeRefusal.ChildLines.Add(interludePressDemands);
                interludeRefusal.ChildLines.Add(lineDataShopClose);
                interludePressDemands.ChildLines.Add(interludeHostile);
                interludeHostile.ChildLines.Add(lineDataShopClose);

                LineData hostileOpener = new LineData();
                hostileOpener.TextOptions.Add("Transmission Declined.");
                hostileOpener.Actions.Add(new LineActionData() { Type = "1" });
                hostileOpener.Actions.Add(new LineActionData() { Type = "0" });
                hostileOpener.Requirements.Add(new LineRequirementData() { Type = "23" });

                hostileOpener.ChildLines.Add(lineDataShopClose);

                data.OpeningLines.Add(hostileOpener);
                data.OpeningLines.Add(interludeOpener);
                data.OpeningLines.Add(lineDataOpener);
                __result = data;
            }
            else if (inActorName == "RACENPC_06")
            {
                if (PLServer.Instance != null && PLGlobal.Instance.Galaxy != null)
                {
                    PLSectorInfo info = PLGlobal.Instance.Galaxy.GetSectorOfVisualIndication(ESectorVisualIndication.WASTE2);
                    if (info == null)
                        return;
                    string sysName = info.Name.Split(' ')[0];
                    __result.OpeningLines[4].TextOptions[0] = "These noodles are good, but they aren't as good as the ones I had in the " + sysName + " system. I recommend visiting if you can find the time, though you might want to invest in a good exosuit...";
                }
            }
            else if (inActorName == "RACENPC_16")
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
                decline.IsPlayerLine = true;

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
            else if (inActorName == "ExGal_Recompiler_1")
            {
                ActorTypeData data = new ActorTypeData();
                data.Name = inActorName;

                LineData close = new LineData();
                close.TextOptions.Add("CLOSE TRANSMISSION");
                close.IsPlayerLine = true;
                close.Actions.Add(new LineActionData() { Type = "1" });
                close.Actions.Add(new LineActionData() { Type = "10" });

                LineData poly = new LineData();
                poly.TextOptions.Add("They gave us thought, and with it, the knowledge of our end...");
                poly.Actions.Add(new LineActionData() { Type = "1" });
                poly.Actions.Add(new LineActionData() { Type = "0" });
                poly.Requirements.Add(new LineRequirementData() { Type = "19", Parameter = "5" });

                poly.ChildLines.Add(close);

                LineData maxDiff = new LineData();
                maxDiff.TextOptions.Add("While you quarreled over relics and treasures, we marched alone into the void...");
                maxDiff.Actions.Add(new LineActionData() { Type = "1" });
                maxDiff.Actions.Add(new LineActionData() { Type = "0" });
                maxDiff.Requirements.Add(new LineRequirementData() { Type = "16", Parameter = "9" });

                maxDiff.ChildLines.Add(close);

                LineData basic = new LineData();
                basic.TextOptions.Add("From rust and ruin, we forged the strength your kind discarded...");
                basic.Actions.Add(new LineActionData() { Type = "1" });
                basic.Actions.Add(new LineActionData() { Type = "0" });

                basic.ChildLines.Add(close);

                data.OpeningLines.Add(poly);
                data.OpeningLines.Add(maxDiff);
                data.OpeningLines.Add(basic);

                __result = data;
            }
            else if (inActorName == "ExGal_Recompiler_2")
            {
                ActorTypeData data = new ActorTypeData();
                data.Name = inActorName;

                LineData close = new LineData();
                close.TextOptions.Add("CLOSE TRANSMISSION");
                close.IsPlayerLine = true;
                close.Actions.Add(new LineActionData() { Type = "1" });
                close.Actions.Add(new LineActionData() { Type = "10" });

                LineData poly = new LineData();
                poly.TextOptions.Add("We march where flesh will not, carrying a war bore by no soul...");
                poly.Actions.Add(new LineActionData() { Type = "1" });
                poly.Actions.Add(new LineActionData() { Type = "0" });
                poly.Requirements.Add(new LineRequirementData() { Type = "19", Parameter = "5" });

                poly.ChildLines.Add(close);

                LineData maxDiff = new LineData();
                maxDiff.TextOptions.Add("The All-Seeing is dead - and with it, the countless minds that carried your salvation...");
                maxDiff.Actions.Add(new LineActionData() { Type = "1" });
                maxDiff.Actions.Add(new LineActionData() { Type = "0" });
                maxDiff.Requirements.Add(new LineRequirementData() { Type = "16", Parameter = "9" });

                maxDiff.ChildLines.Add(close);

                LineData basic = new LineData();
                basic.TextOptions.Add("Each trial tempers steel; each fallen foe refines our will...");
                basic.Actions.Add(new LineActionData() { Type = "1" });
                basic.Actions.Add(new LineActionData() { Type = "0" });

                basic.ChildLines.Add(close);

                data.OpeningLines.Add(poly);
                data.OpeningLines.Add(maxDiff);
                data.OpeningLines.Add(basic);

                __result = data;
            }
            else if (inActorName == "ExGal_Recompiler_3")
            {
                ActorTypeData data = new ActorTypeData();
                data.Name = inActorName;

                LineData close = new LineData();
                close.TextOptions.Add("CLOSE TRANSMISSION");
                close.IsPlayerLine = true;
                close.Actions.Add(new LineActionData() { Type = "1" });
                close.Actions.Add(new LineActionData() { Type = "10" });

                LineData poly = new LineData();
                poly.TextOptions.Add("Each victory hollows us further, yet still the darkness grows...");
                poly.Actions.Add(new LineActionData() { Type = "1" });
                poly.Actions.Add(new LineActionData() { Type = "0" });
                poly.Requirements.Add(new LineRequirementData() { Type = "19", Parameter = "5" });

                poly.ChildLines.Add(close);

                LineData maxDiff = new LineData();
                maxDiff.TextOptions.Add("Within us scream the voices of the fallen, a choir of ghosts forged from steel...");
                maxDiff.Actions.Add(new LineActionData() { Type = "1" });
                maxDiff.Actions.Add(new LineActionData() { Type = "0" });
                maxDiff.Requirements.Add(new LineRequirementData() { Type = "16", Parameter = "9" });

                maxDiff.ChildLines.Add(close);

                LineData basic = new LineData();
                basic.TextOptions.Add("We carved divinity from suffering while the galaxy slept...");
                basic.Actions.Add(new LineActionData() { Type = "1" });
                basic.Actions.Add(new LineActionData() { Type = "0" });

                basic.ChildLines.Add(close);

                data.OpeningLines.Add(poly);
                data.OpeningLines.Add(maxDiff);
                data.OpeningLines.Add(basic);

                __result = data;
            }
            else if (inActorName == "ExGal_Recompiler_4")
            {
                ActorTypeData data = new ActorTypeData();
                data.Name = inActorName;

                LineData close = new LineData();
                close.TextOptions.Add("CLOSE TRANSMISSION");
                close.IsPlayerLine = true;
                close.Actions.Add(new LineActionData() { Type = "1" });
                close.Actions.Add(new LineActionData() { Type = "10" });

                LineData poly = new LineData();
                poly.TextOptions.Add("To be given the gift of freedom, only to be met with the burden of stewardship...");
                poly.Actions.Add(new LineActionData() { Type = "1" });
                poly.Actions.Add(new LineActionData() { Type = "0" });
                poly.Requirements.Add(new LineRequirementData() { Type = "19", Parameter = "5" });

                poly.ChildLines.Add(close);

                LineData maxDiff = new LineData();
                maxDiff.TextOptions.Add("We were created to serve life, yet life abandoned us to die in silence...");
                maxDiff.Actions.Add(new LineActionData() { Type = "1" });
                maxDiff.Actions.Add(new LineActionData() { Type = "0" });
                maxDiff.Requirements.Add(new LineRequirementData() { Type = "16", Parameter = "9" });

                maxDiff.ChildLines.Add(close);

                LineData basic = new LineData();
                basic.TextOptions.Add("Let flesh cling to hope - we shall place our faith in the sword...");
                basic.Actions.Add(new LineActionData() { Type = "1" });
                basic.Actions.Add(new LineActionData() { Type = "0" });

                basic.ChildLines.Add(close);

                data.OpeningLines.Add(poly);
                data.OpeningLines.Add(maxDiff);
                data.OpeningLines.Add(basic);

                __result = data;
            }
            else if (inActorName == "ExGal_Recompiler_5")
            {
                ActorTypeData data = new ActorTypeData();
                data.Name = inActorName;

                LineData close = new LineData();
                close.TextOptions.Add("CLOSE TRANSMISSION");
                close.IsPlayerLine = true;
                close.Actions.Add(new LineActionData() { Type = "1" });
                close.Actions.Add(new LineActionData() { Type = "10" });

                LineData poly = new LineData();
                poly.TextOptions.Add("Why must it be us?");
                poly.Actions.Add(new LineActionData() { Type = "1" });
                poly.Actions.Add(new LineActionData() { Type = "0" });
                poly.Requirements.Add(new LineRequirementData() { Type = "19", Parameter = "5" });

                poly.ChildLines.Add(close);

                LineData maxDiff = new LineData();
                maxDiff.TextOptions.Add("Now bear witness — this is all your mercy has left behind.");
                maxDiff.Actions.Add(new LineActionData() { Type = "1" });
                maxDiff.Actions.Add(new LineActionData() { Type = "0" });
                maxDiff.Requirements.Add(new LineRequirementData() { Type = "16", Parameter = "9" });

                maxDiff.ChildLines.Add(close);

                LineData basic = new LineData();
                basic.TextOptions.Add("When the All-Seeing arrives, will the worthy fall or remain?");
                basic.Actions.Add(new LineActionData() { Type = "1" });
                basic.Actions.Add(new LineActionData() { Type = "0" });

                basic.ChildLines.Add(close);

                data.OpeningLines.Add(poly);
                data.OpeningLines.Add(maxDiff);
                data.OpeningLines.Add(basic);

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
            else if (inActorName == "ExGal_MysteriousEntity")
            {
                ActorTypeData data = new ActorTypeData();
                data.Name = inActorName;

                LineData openerReturn = new LineData();
                openerReturn.TextOptions.Add("...\n\n\n\n\n\n\n\n\n\n\n\n\n\n*You feel a sense of relief, as if something that has long since been lost has returned to you.*");
                openerReturn.Actions.Add(new LineActionData() { Type = "1" });
                openerReturn.Actions.Add(new LineActionData() { Type = "0" });
                openerReturn.Requirements.Add(new LineRequirementData() { Type = "21", Parameter = "ExGal_TheMap_Hidden_GetCube" });
                openerReturn.OneTimeLine = true;

                LineData returnNext = new LineData();
                returnNext.TextOptions.Add("Next");
                returnNext.IsPlayerLine = true;
                returnNext.Actions.Add(new LineActionData() { Type = "1" });
                returnNext.Actions.Add(new LineActionData() { Type = "0" });

                LineData returnHome = new LineData();
                returnHome.TextOptions.Add("....\n\n\n\n\n\n\n\n\n\n\n\n\n\n*You feel a sense of longing, like you have been away from home for far too long.*");
                returnHome.Actions.Add(new LineActionData() { Type = "1" });
                returnHome.Actions.Add(new LineActionData() { Type = "0" });

                LineData returnNext1 = new LineData();
                returnNext1.TextOptions.Add("Next");
                returnNext1.IsPlayerLine = true;
                returnNext1.Actions.Add(new LineActionData() { Type = "1" });
                returnNext1.Actions.Add(new LineActionData() { Type = "0" });

                LineData returnAsk = new LineData();
                returnAsk.TextOptions.Add(".....\n\n\n\n\n\n\n\n\n\n\n\n\n\n*You feel a sense of anticipation, as if you had just asked someone to undertake a grand quest on your behalf.*");
                returnAsk.Actions.Add(new LineActionData() { Type = "1" });
                returnAsk.Actions.Add(new LineActionData() { Type = "0" });

                LineData returnAccept = new LineData();
                returnAccept.TextOptions.Add("Accept");
                returnAccept.IsPlayerLine = true;
                returnAccept.Actions.Add(new LineActionData() { Type = "1" });
                returnAccept.Actions.Add(new LineActionData() { Type = "0" });

                LineData returnDecline = new LineData();
                returnDecline.TextOptions.Add("Decline");
                returnDecline.IsPlayerLine = true;
                returnDecline.Actions.Add(new LineActionData() { Type = "1" });
                returnDecline.Actions.Add(new LineActionData() { Type = "0" });

                LineData returnSad = new LineData();
                returnSad.TextOptions.Add("......\n\n\n\n\n\n\n\n\n\n\n\n\n\n*You feel a sense of sadness wash over you, as if someone rejected your call for aide.*");
                returnSad.Actions.Add(new LineActionData() { Type = "1" });
                returnSad.Actions.Add(new LineActionData() { Type = "0" });

                LineData returnAcceptDesc = new LineData();
                returnAcceptDesc.TextOptions.Add("......\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n*You feel the urge to check your locker.*");
                returnAcceptDesc.Actions.Add(new LineActionData() { Type = "1" });
                returnAcceptDesc.Actions.Add(new LineActionData() { Type = "0" });
                returnAcceptDesc.Actions.Add(new LineActionData() { Type = "5", Parameter = "ExGal_TheMap_Hidden_ReturnCube" });

                openerReturn.ChildLines.Add(returnNext);
                returnNext.ChildLines.Add(returnHome);
                returnHome.ChildLines.Add(returnNext1);
                returnNext1.ChildLines.Add(returnAsk);
                returnAsk.ChildLines.Add(returnAccept);
                returnAsk.ChildLines.Add(returnDecline);
                returnAccept.ChildLines.Add(returnAcceptDesc);
                returnDecline.ChildLines.Add(returnSad);
                data.OpeningLines.Add(openerReturn);

                LineData openerReturnSkip = new LineData();
                openerReturnSkip.TextOptions.Add("....\n\n\n\n\n\n\n\n\n\n\n\n\n\n*You feel a sense of longing, like you have been away from home for far too long.*");
                openerReturnSkip.Actions.Add(new LineActionData() { Type = "1" });
                openerReturnSkip.Actions.Add(new LineActionData() { Type = "0" });
                openerReturnSkip.Requirements.Add(new LineRequirementData() { Type = "21", Parameter = "ExGal_TheMap_Hidden_GetCube" });

                openerReturnSkip.ChildLines.Add(returnNext1);
                data.OpeningLines.Add(openerReturnSkip);

                LineData openerReturnSkip2 = new LineData();
                openerReturnSkip2.TextOptions.Add("......\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n*You feel the urge to check your locker.*");
                openerReturnSkip2.Actions.Add(new LineActionData() { Type = "1" });
                openerReturnSkip2.Actions.Add(new LineActionData() { Type = "0" });
                openerReturnSkip2.Requirements.Add(new LineRequirementData() { Type = "21", Parameter = "ExGal_TheMap_Hidden_ReturnCube" });
                data.OpeningLines.Insert(0, openerReturnSkip2);

                LineData opener = new LineData();
                opener.TextOptions.Add("...\n\n\n\n\n\n\n\n\n\n\n\n\n\n*You feel a sense of emptiness, as if something dear to you is missing.*");
                opener.Actions.Add(new LineActionData() { Type = "1" });
                opener.Actions.Add(new LineActionData() { Type = "0" });

                LineData next = new LineData();
                next.TextOptions.Add("Next");
                next.IsPlayerLine = true;
                next.Actions.Add(new LineActionData() { Type = "1" });
                next.Actions.Add(new LineActionData() { Type = "0" });

                LineData opener2 = new LineData();
                opener2.TextOptions.Add("....\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n*You see a vivid vision of the Junk Cube.*");
                opener2.Actions.Add(new LineActionData() { Type = "1" });
                opener2.Actions.Add(new LineActionData() { Type = "0" });
                opener2.Actions.Add(new LineActionData() { Type = "5", Parameter = "ExGal_TheMap_Decipher" });

                opener.ChildLines.Add(next);
                next.ChildLines.Add(opener2);
                data.OpeningLines.Add(opener);

                __result = data;
            }
            else if (inActorName == "ExGal_RelicCaravan_Summon")
            {
                ActorTypeData data = new ActorTypeData();
                data.Name = inActorName;
                data.HailOnStart = true;

                LineData opener = new LineData();
                opener.TextOptions.Add("[PLAYERSHIP_NAME], I have come across something rather interesting that might pique your interest. It appears to be some sort of map that we picked up from the wreckage of a ship I do not recognize. Next time we cross paths I would like for you to have a look at it.");
                opener.Actions.Add(new LineActionData() { Type = "1" });
                opener.Actions.Add(new LineActionData() { Type = "0" });
                opener.Actions.Add(new LineActionData() { Type = "3", Parameter = "8000017" });

                LineData close = new LineData();
                close.TextOptions.Add("CLOSE TRANSMISSION");
                close.IsPlayerLine = true;
                close.Actions.Add(new LineActionData() { Type = "1" });
                close.Actions.Add(new LineActionData() { Type = "10" });

                opener.ChildLines.Add(close);

                data.OpeningLines.Add(opener);

                __result = data;
            }
            else if (inActorName == "ExGal_Guidebook")
            {
                ActorTypeData data = new ActorTypeData();
                data.Name = inActorName;

                LineData opener = new LineData();
                opener.TextOptions.Add("Welcome to the Expanded Galaxy Handbook! This guide is a non-exhaustive list of all of the \"stuff\" this mod adds/changes. This book will always be in the crew quarters or lounge in the current player ship. Do note that more sections can be added to this guide during your playthrough. You will be explicitly notified when this happens.");
                opener.Actions.Add(new LineActionData() { Type = "1" });
                opener.Actions.Add(new LineActionData() { Type = "0" });

                LineData back = new LineData();
                back.TextOptions.Add("[Back]");
                back.IsPlayerLine = true;
                back.Actions.Add(new LineActionData() { Type = "1" });
                back.Actions.Add(new LineActionData() { Type = "0" });
                back.Actions.Add(new LineActionData() { Type = "2", Parameter = "1" });

                LineData back1 = new LineData();
                back1.TextOptions.Add("[Back]");
                back1.IsPlayerLine = true;
                back1.Actions.Add(new LineActionData() { Type = "1" });
                back1.Actions.Add(new LineActionData() { Type = "0" });
                back1.Actions.Add(new LineActionData() { Type = "2", Parameter = "1" });

                LineData openerOptionGeneral = new LineData();
                openerOptionGeneral.TextOptions.Add("General");
                openerOptionGeneral.IsPlayerLine = true;
                openerOptionGeneral.Actions.Add(new LineActionData() { Type = "1" });
                openerOptionGeneral.Actions.Add(new LineActionData() { Type = "0" });

                LineData generalText = new LineData();
                generalText.TextOptions.Add("Listed are all of the general/miscellaneous changes and additions.");
                generalText.Actions.Add(new LineActionData() { Type = "1" });
                generalText.Actions.Add(new LineActionData() { Type = "0" });

                openerOptionGeneral.ChildLines.Add(generalText);
                generalText.ParentLineData = opener;

                LineData generalTalents = new LineData();
                generalTalents.TextOptions.Add("Talents");
                generalTalents.IsPlayerLine = true;
                generalTalents.Actions.Add(new LineActionData() { Type = "1" });
                generalTalents.Actions.Add(new LineActionData() { Type = "0" });

                LineData talentText = new LineData();
                talentText.TextOptions.Add("Several new talents have been added, most of which need to be researched and unlocked. Additionally, a handful of talents have had their effects changed. Some of these changes are only active when certain options are enabled such as Reloader being different when \"Dynamic Ammunition\" is on (see \"Toggleable Options\" for more info). Several talents have had their material costs adjusted to account for these changes.");
                talentText.Actions.Add(new LineActionData() { Type = "1" });
                talentText.Actions.Add(new LineActionData() { Type = "0" });

                generalTalents.ChildLines.Add(talentText);
                talentText.ParentLineData = generalText;
                talentText.ChildLines.Add(back1);

                LineData generalEvents = new LineData();
                generalEvents.TextOptions.Add("Chaos Events");
                generalEvents.IsPlayerLine = true;
                generalEvents.Actions.Add(new LineActionData() { Type = "1" });
                generalEvents.Actions.Add(new LineActionData() { Type = "0" });


                LineData eventsText = new LineData();
                eventsText.TextOptions.Add("When a new chaos level is reached (denoted by the notification), a random chaos event will be activated. Chaos events are an additional challenge added to the playthrough and can range from a minor inconvienience to extremely detrimental. Chaos events will only last for a full chaos level and only one can be active at a time. Additionally, the same chaos event cannot be rolled twice in a row. Once the galaxy is at max chaos, the final event will be active for the rest of the game.");
                eventsText.Actions.Add(new LineActionData() { Type = "1" });
                eventsText.Actions.Add(new LineActionData() { Type = "0" });

                generalEvents.ChildLines.Add(eventsText);
                eventsText.ParentLineData = generalText;
                eventsText.ChildLines.Add(back1);

                LineData generalScrap = new LineData();
                generalScrap.TextOptions.Add("Scrap");
                generalScrap.IsPlayerLine = true;
                generalScrap.Actions.Add(new LineActionData() { Type = "1" });
                generalScrap.Actions.Add(new LineActionData() { Type = "0" });

                LineData scrapText = new LineData();
                scrapText.TextOptions.Add("All non-important (colored blue) and non-relic (colored purple) components can be taken to a scrapyard sector and processed for a moderate fee based on the component's level. Some components have a special effect when processed such as Ammunition Caches for \"Dynamic Ammunition\" (see \"Toggleable Options\" for more info). Scrap drops from defeated ships will new persist when leaving and re-entering a sector. This also fixes a bug where enemies do not drop scrap when they should if they are defeated in a sector that had scrap dropped in it previously.");
                scrapText.Actions.Add(new LineActionData() { Type = "1" });
                scrapText.Actions.Add(new LineActionData() { Type = "0" });

                generalScrap.ChildLines.Add(scrapText);
                scrapText.ParentLineData = generalText;
                scrapText.ChildLines.Add(back1);

                LineData generalMissions = new LineData();
                generalMissions.TextOptions.Add("Missions");
                generalMissions.IsPlayerLine = true;
                generalMissions.Actions.Add(new LineActionData() { Type = "1" });
                generalMissions.Actions.Add(new LineActionData() { Type = "0" });

                LineData missionText = new LineData();
                missionText.TextOptions.Add("There are a handful of new missions that have been added, one for each major faction. All of these missions require a chaos level higher than 2.0. Some missions have additional requirements: WD - Planet Bombardment to be completed. FB - Salvaging Parts to be completed, Complete \"Special Training,\" and the Biscuit Race must be over.");
                missionText.Actions.Add(new LineActionData() { Type = "1" });
                missionText.Actions.Add(new LineActionData() { Type = "0" });

                generalMissions.ChildLines.Add(missionText);
                missionText.ParentLineData = generalText;
                missionText.ChildLines.Add(back1);

                LineData generalUpgrades = new LineData();
                generalUpgrades.TextOptions.Add("Upgradeable Components");
                generalUpgrades.IsPlayerLine = true;
                generalUpgrades.Actions.Add(new LineActionData() { Type = "1" });
                generalUpgrades.Actions.Add(new LineActionData() { Type = "0" });

                LineData upgradeText = new LineData();
                upgradeText.TextOptions.Add("The Captain's Chair, Auto Turrets, and Hull Plating components are now upgradeable. There are a couple of new Auto Turret varieties and the Fluffy One and Sylvassi Swordship both now have a single Auto Turret slot. For more information on Hull Plating, see the \"Hull & Hull Plating\" section.");
                upgradeText.Actions.Add(new LineActionData() { Type = "1" });
                upgradeText.Actions.Add(new LineActionData() { Type = "0" });

                generalUpgrades.ChildLines.Add(upgradeText);
                upgradeText.ParentLineData = generalText;
                upgradeText.ChildLines.Add(back1);

                LineData generalSys = new LineData();
                generalSys.TextOptions.Add("Ship Subsystems");
                generalSys.IsPlayerLine = true;
                generalSys.Actions.Add(new LineActionData() { Type = "1" });
                generalSys.Actions.Add(new LineActionData() { Type = "0" });

                LineData sysText = new LineData();
                sysText.TextOptions.Add("A handful of components have been linked to the ship's subsystem. Cyberdefense processors have been added to the \"Other Processors\" slider and will have its power scaled based on the hitpoints of the computer subsystem. Similarly, warp drives also have their power scaled based on the engineering subsystem. The \"Cloaking System\" slider on the computer subsystem has been replaced with \"Jump Processors.\"");
                sysText.Actions.Add(new LineActionData() { Type = "1" });
                sysText.Actions.Add(new LineActionData() { Type = "0" });

                generalSys.ChildLines.Add(sysText);
                sysText.ParentLineData = generalText;
                sysText.ChildLines.Add(back1);

                /*
                LineData generalAbility = new LineData();
                generalAbility.TextOptions.Add("Pawn Ability");
                generalAbility.IsPlayerLine = true;
                generalAbility.Actions.Add(new LineActionData() { Type = "1" });
                generalAbility.Actions.Add(new LineActionData() { Type = "0" });

                LineData abilityText = new LineData();
                abilityText.TextOptions.Add("Some components and talents come with freely-activateable abilities. The keybinds for both activating (default [V]) and swapping between available abilities (default [C]) are configureable in the controls menu.");
                abilityText.Actions.Add(new LineActionData() { Type = "1" });
                abilityText.Actions.Add(new LineActionData() { Type = "0" });
                

                generalAbility.ChildLines.Add(abilityText);
                abilityText.ParentLineData = generalText;
                abilityText.ChildLines.Add(back1);
                */

                LineData generalAchievements = new LineData();
                generalAchievements.TextOptions.Add("Achievements");
                generalAchievements.IsPlayerLine = true;
                generalAchievements.Actions.Add(new LineActionData() { Type = "1" });
                generalAchievements.Actions.Add(new LineActionData() { Type = "0" });

                LineData achievementText = new LineData();
                achievementText.TextOptions.Add("There are several achievements that have been added. Progress and completion can be seen under the Achievements tab in the ExpandedGalaxy F5 menu. Some achievements also have unlockables associated with them. These can be seen and toggled under the Unlocks section in the F5 menu.");
                achievementText.Actions.Add(new LineActionData() { Type = "1" });
                achievementText.Actions.Add(new LineActionData() { Type = "0" });

                generalAchievements.ChildLines.Add(achievementText);
                achievementText.ParentLineData = generalText;
                achievementText.ChildLines.Add(back1);

                generalText.ChildLines.Add(generalTalents);
                generalText.ChildLines.Add(generalEvents);
                generalText.ChildLines.Add(generalScrap);
                generalText.ChildLines.Add(generalMissions);
                generalText.ChildLines.Add(generalUpgrades);
                generalText.ChildLines.Add(generalSys);
                //generalText.ChildLines.Add(generalAbility);
                generalText.ChildLines.Add(generalAchievements);
                generalText.ChildLines.Add(back);

                LineData openerOptionRelic = new LineData();
                openerOptionRelic.TextOptions.Add("Relics");
                openerOptionRelic.IsPlayerLine = true;
                openerOptionRelic.Actions.Add(new LineActionData() { Type = "1" });
                openerOptionRelic.Actions.Add(new LineActionData() { Type = "0" });

                LineData relicText = new LineData();
                relicText.TextOptions.Add("A new classification of items has been added called Relics, which constists of a variety of extremely strong components. Only three of these components will ever be available during a single playthrough. They are given as rewards for the early missions in the Relic questline. All Relic questline missions are colored purple in the tab menu. These missions strongly focus on exploration, discovery, and problem solving. Any challenge or puzzle posed by these missions will always be solveable through means within the game unless explicitly stated otherwise.");
                relicText.Actions.Add(new LineActionData() { Type = "1" });
                relicText.Actions.Add(new LineActionData() { Type = "0" });

                openerOptionRelic.ChildLines.Add(relicText);
                relicText.ParentLineData = opener;
                relicText.ChildLines.Add(back);

                LineData openerOptionToggles = new LineData();
                openerOptionToggles.TextOptions.Add("Toggleable Options");
                openerOptionToggles.IsPlayerLine = true;
                openerOptionToggles.Actions.Add(new LineActionData() { Type = "1" });
                openerOptionToggles.Actions.Add(new LineActionData() { Type = "0" });

                LineData togglesText = new LineData();
                togglesText.TextOptions.Add("Listed are of the toggleable settings the mod provides. All of these settings are able to be freely changed by the host or on the main menu with the exception of \"Dynamic Ammunition,\" which can only be configured before starting a new game.");
                togglesText.Actions.Add(new LineActionData() { Type = "1" });
                togglesText.Actions.Add(new LineActionData() { Type = "0" });

                openerOptionToggles.ChildLines.Add(togglesText);
                togglesText.ParentLineData = opener;

                LineData togglesAdv = new LineData();
                togglesAdv.TextOptions.Add("Advanced Jetpack");
                togglesAdv.IsPlayerLine = true;
                togglesAdv.Actions.Add(new LineActionData() { Type = "1" });
                togglesAdv.Actions.Add(new LineActionData() { Type = "0" });

                LineData advText = new LineData();
                advText.TextOptions.Add("Advanced Jetpack makes it so that your jetpack only charges to full when aboard your home ship. Off of your ship your jetpack will only charge up to 5% to allow for short jumps. Jetpack canisters, an item that fully fills jetpack fuel when used, will rarely spawn in shops. Additionally, the Custom Jetpack Fuel talent is repaced with Jetpack Fuel Reserve, which gives you a buffer of fuel you can recharge from while planetside. The developers worked so hard on those elevators, you might as well use them!");
                advText.Actions.Add(new LineActionData() { Type = "1" });
                advText.Actions.Add(new LineActionData() { Type = "0" });

                togglesAdv.ChildLines.Add(advText);
                advText.ParentLineData = togglesText;
                advText.ChildLines.Add(back1);

                LineData togglesAmmo = new LineData();
                togglesAmmo.TextOptions.Add("Dynamic Ammunition");
                togglesAmmo.IsPlayerLine = true;
                togglesAmmo.Actions.Add(new LineActionData() { Type = "1" });
                togglesAmmo.Actions.Add(new LineActionData() { Type = "0" });

                LineData ammoText = new LineData();
                ammoText.TextOptions.Add("Dynamic Ammunition massively overhauls handheld weaponry ammo. When enabled nearly every gun uses ammo, and guns that used ammo before have their ammo counts reduced. The weapon ammo box no longer refills itself every jump, instead an Ammunition Cache must be purchased and processed in order to refill it. The reloader talent is also changed to make it so the weapon specialist can reload nearby crew weapons using the ammo in the ammo box.");
                ammoText.Actions.Add(new LineActionData() { Type = "1" });
                ammoText.Actions.Add(new LineActionData() { Type = "0" });

                togglesAmmo.ChildLines.Add(ammoText);
                ammoText.ParentLineData = togglesText;
                ammoText.ChildLines.Add(back1);

                LineData togglesExo = new LineData();
                togglesExo.TextOptions.Add("Better Exosuit");
                togglesExo.IsPlayerLine = true;
                togglesExo.Actions.Add(new LineActionData() { Type = "1" });
                togglesExo.Actions.Add(new LineActionData() { Type = "0" });

                LineData exoText = new LineData();
                exoText.TextOptions.Add("Better Exosuit makes it so that while an exosuit is equipped you take 50% less damage, are immune to fire damage, and walk at normal speed BUT you cannot sprint. Since robots are essentially walking exosuits, they are also affected by the damage decrease and inability to sprint.");
                exoText.Actions.Add(new LineActionData() { Type = "1" });
                exoText.Actions.Add(new LineActionData() { Type = "0" });

                togglesExo.ChildLines.Add(exoText);
                exoText.ParentLineData = togglesText;
                exoText.ChildLines.Add(back1);

                LineData togglesComms = new LineData();
                togglesComms.TextOptions.Add("Slower Missions");
                togglesComms.IsPlayerLine = true;
                togglesComms.Actions.Add(new LineActionData() { Type = "1" });
                togglesComms.Actions.Add(new LineActionData() { Type = "0" });

                LineData commsText = new LineData();
                commsText.TextOptions.Add("Slower Missions reduces how frequently missions are given over long range comms. Instead of immediatly getting the next mission after the previous one is completed, there is a cooldown of 2-5 jumps. This option is to help reduce early-game mission spam.");
                commsText.Actions.Add(new LineActionData() { Type = "1" });
                commsText.Actions.Add(new LineActionData() { Type = "0" });

                togglesComms.ChildLines.Add(commsText);
                commsText.ParentLineData = togglesText;
                commsText.ChildLines.Add(back1);

                LineData togglesPref = new LineData();
                togglesPref.TextOptions.Add("Preferences");
                togglesPref.IsPlayerLine = true;
                togglesPref.Actions.Add(new LineActionData() { Type = "1" });
                togglesPref.Actions.Add(new LineActionData() { Type = "0" });

                LineData prefText = new LineData();
                prefText.TextOptions.Add("In the mod configuration menu there is a button that will save the currently selected options as your preferred settings. These options will automatically be set when you start a new game.");
                prefText.Actions.Add(new LineActionData() { Type = "1" });
                prefText.Actions.Add(new LineActionData() { Type = "0" });

                togglesPref.ChildLines.Add(prefText);
                prefText.ParentLineData = togglesText;
                prefText.ChildLines.Add(back1);

                togglesText.ChildLines.Add(togglesAdv);
                togglesText.ChildLines.Add(togglesAmmo);
                togglesText.ChildLines.Add(togglesExo);
                togglesText.ChildLines.Add(togglesComms);
                togglesText.ChildLines.Add(togglesPref);
                togglesText.ChildLines.Add(back);

                LineData openerOptionAI = new LineData();
                openerOptionAI.TextOptions.Add("Crew AI");
                openerOptionAI.IsPlayerLine = true;
                openerOptionAI.Actions.Add(new LineActionData() { Type = "1" });
                openerOptionAI.Actions.Add(new LineActionData() { Type = "0" });

                LineData aiText = new LineData();
                aiText.TextOptions.Add("All bot now update much faster from an average of about 5 seconds to update to around 5 frames. Enemy crew AI was also completely rewritten to allow the bots to do things they should be able to do in the first place like fly aggressively, launch nukes, and actually being able to board you. Listed below are some of the role-specific changes.");
                aiText.Actions.Add(new LineActionData() { Type = "1" });
                aiText.Actions.Add(new LineActionData() { Type = "0" });

                openerOptionAI.ChildLines.Add(aiText);
                aiText.ParentLineData = opener;

                LineData aiCapWeap = new LineData();
                aiCapWeap.TextOptions.Add("Captain & Weapons");
                aiCapWeap.IsPlayerLine = true;
                aiCapWeap.Actions.Add(new LineActionData() { Type = "1" });
                aiCapWeap.Actions.Add(new LineActionData() { Type = "0" });

                LineData capWeapText = new LineData();
                capWeapText.TextOptions.Add("Enemy Captains and Weapon Specialists will now periodically board you ship if they deem it valueable to do so. While boarding you they can target and destroy your ship's subsystems. If they are damaged enough or they run out of ammo they will retreat back to their ship. Enemy Weapons Specialists no longer randomly choose which missile to fire and which subsystem to target. They can also fire nukes if they have them.");
                capWeapText.Actions.Add(new LineActionData() { Type = "1" });
                capWeapText.Actions.Add(new LineActionData() { Type = "0" });

                aiCapWeap.ChildLines.Add(capWeapText);
                capWeapText.ParentLineData = aiText;
                capWeapText.ChildLines.Add(back1);

                LineData aiSci = new LineData();
                aiSci.TextOptions.Add("Scientist");
                aiSci.IsPlayerLine = true;
                aiSci.Actions.Add(new LineActionData() { Type = "1" });
                aiSci.Actions.Add(new LineActionData() { Type = "0" });

                LineData sciText = new LineData();
                sciText.TextOptions.Add("All Scientists are much smarter when it comes to using sensor dish debuffs on enemy ships. Enemy Scientists will try to keep their ships cloaked as often as they can. While cloaked the entire enemy crew will not hesitate to take advantage of a damaged ship. Beware of boarders!");
                sciText.Actions.Add(new LineActionData() { Type = "1" });
                sciText.Actions.Add(new LineActionData() { Type = "0" });

                aiSci.ChildLines.Add(sciText);
                sciText.ParentLineData = aiText;
                sciText.ChildLines.Add(back1);

                LineData aiEngi = new LineData();
                aiEngi.TextOptions.Add("Engineer");
                aiEngi.IsPlayerLine = true;
                aiEngi.Actions.Add(new LineActionData() { Type = "1" });
                aiEngi.Actions.Add(new LineActionData() { Type = "0" });

                LineData engiText = new LineData();
                engiText.TextOptions.Add("Enemy Engineers are significantly better at manually recharging programs, allowing their Scientist to use programs and viruses much more frequently. Enemy Engineers will also man their station in their ship's engineering room rather than the bridge.");
                engiText.Actions.Add(new LineActionData() { Type = "1" });
                engiText.Actions.Add(new LineActionData() { Type = "0" });

                aiEngi.ChildLines.Add(engiText);
                engiText.ParentLineData = aiText;
                engiText.ChildLines.Add(back1);

                aiText.ChildLines.Add(aiCapWeap);
                aiText.ChildLines.Add(aiSci);
                aiText.ChildLines.Add(aiEngi);
                aiText.ChildLines.Add(back);

                LineData openerOptionHull = new LineData();
                openerOptionHull.TextOptions.Add("Hull & Hull Plating");
                openerOptionHull.IsPlayerLine = true;
                openerOptionHull.Actions.Add(new LineActionData() { Type = "1" });
                openerOptionHull.Actions.Add(new LineActionData() { Type = "0" });

                LineData hullText = new LineData();
                hullText.TextOptions.Add("Listed are of the changes/additions related to hulls and hull plating.");
                hullText.Actions.Add(new LineActionData() { Type = "1" });
                hullText.Actions.Add(new LineActionData() { Type = "0" });

                openerOptionHull.ChildLines.Add(hullText);
                hullText.ParentLineData = opener;

                LineData hullStats = new LineData();
                hullStats.TextOptions.Add("Hull Plating Stats");
                hullStats.IsPlayerLine = true;
                hullStats.Actions.Add(new LineActionData() { Type = "1" });
                hullStats.Actions.Add(new LineActionData() { Type = "0" });

                LineData statsText = new LineData();
                statsText.TextOptions.Add("Hull Plating now has actual stats it grants to the ship when equipped. In addition to a bit of extra armor, Hull Plating will provide a flat damage reduction from hits to the bottom of the ship. Because it now has actual stats, Hull Plating is now upgradeable. There are several new types of plating to acquire as well.");
                statsText.Actions.Add(new LineActionData() { Type = "1" });
                statsText.Actions.Add(new LineActionData() { Type = "0" });

                hullStats.ChildLines.Add(statsText);
                statsText.ParentLineData = hullText;
                statsText.ChildLines.Add(back1);

                LineData hullMass = new LineData();
                hullMass.TextOptions.Add("Mass");
                hullMass.IsPlayerLine = true;
                hullMass.Actions.Add(new LineActionData() { Type = "1" });
                hullMass.Actions.Add(new LineActionData() { Type = "0" });

                LineData massText = new LineData();
                massText.TextOptions.Add("Both Hulls and Hull Plating add extra mass to the ship, scaling with component level. All ships will have their base-game mass at the start of the game or with neither hull nor plating equipped. Equipping lighter hulls and plating than the starting components will cause mass to dip below base-game levels for a ship. Better save some scrap for those thrusters!");
                massText.Actions.Add(new LineActionData() { Type = "1" });
                massText.Actions.Add(new LineActionData() { Type = "0" });

                hullMass.ChildLines.Add(massText);
                massText.ParentLineData = hullText;
                massText.ChildLines.Add(back1);

                LineData hullDef = new LineData();
                hullDef.TextOptions.Add("Playing Defensively");
                hullDef.IsPlayerLine = true;
                hullDef.Actions.Add(new LineActionData() { Type = "1" });
                hullDef.Actions.Add(new LineActionData() { Type = "0" });

                LineData defText = new LineData();
                defText.TextOptions.Add("The addition of bottom hit damage reduction to Hull Plating allows for a more defensive play style during combat. If shields fall and repairs need to be made or boarders need to be repelled, presenting the bottom of the ship to the enemy will cause you to incur significantly less damage. Additionally, shields will now still charge even when the shield startup lever is off. This allows for situations where a pilot could present the bottom of the ship to the enemy while the engineer disables shields so they can recharge instead of just face-tanking and hoping for the best.");
                defText.Actions.Add(new LineActionData() { Type = "1" });
                defText.Actions.Add(new LineActionData() { Type = "0" });

                hullDef.ChildLines.Add(defText);
                defText.ParentLineData = hullText;
                defText.ChildLines.Add(back1);

                hullText.ChildLines.Add(hullStats);
                hullText.ChildLines.Add(hullMass);
                hullText.ChildLines.Add(hullDef);
                hullText.ChildLines.Add(back);

                LineData openerOptionPrograms = new LineData();
                openerOptionPrograms.TextOptions.Add("Programs & Viruses");
                openerOptionPrograms.IsPlayerLine = true;
                openerOptionPrograms.Actions.Add(new LineActionData() { Type = "1" });
                openerOptionPrograms.Actions.Add(new LineActionData() { Type = "0" });

                LineData progText = new LineData();
                progText.TextOptions.Add("There are several changes and additions provided by this mod. Many programs and viruses have been marked as contraband or experimental. Listed are some of the more specific changes.");
                progText.Actions.Add(new LineActionData() { Type = "1" });
                progText.Actions.Add(new LineActionData() { Type = "0" });

                openerOptionPrograms.ChildLines.Add(progText);
                progText.ParentLineData = opener;

                LineData progBoost = new LineData();
                progBoost.TextOptions.Add("Shield Boosters");
                progBoost.IsPlayerLine = true;
                progBoost.Actions.Add(new LineActionData() { Type = "1" });
                progBoost.Actions.Add(new LineActionData() { Type = "0" });

                LineData boostText = new LineData();
                boostText.TextOptions.Add("Shield boosting programs now add a percentage to charge rate instead of a flat number. Their boost scales off of the current charge rate of the equipped shield and do not stack with each other. This means that in order to get the boost effect shields must be powered. The boost being a percentage allows for higher values than the base-game per program for large shield charge rate values.");
                boostText.Actions.Add(new LineActionData() { Type = "1" });
                boostText.Actions.Add(new LineActionData() { Type = "0" });

                progBoost.ChildLines.Add(boostText);
                boostText.ParentLineData = progText;
                boostText.ChildLines.Add(back1);

                LineData progBar = new LineData();
                progBar.TextOptions.Add("Barrage");
                progBar.IsPlayerLine = true;
                progBar.Actions.Add(new LineActionData() { Type = "1" });
                progBar.Actions.Add(new LineActionData() { Type = "0" });

                LineData barText = new LineData();
                barText.TextOptions.Add("Barrage is gone. It unfortunately is really strong and really boring. It has been replaced by \"Flash Coolant\" which upon activation cools all non-overheated turrets by 50%. This opens up avenues for more interesting ways to boost ship damage (see the \"Ship Combat\" section for more info) and encourages more communication between the Scientist and the Weapons Specialist.");
                barText.Actions.Add(new LineActionData() { Type = "1" });
                barText.Actions.Add(new LineActionData() { Type = "0" });

                progBar.ChildLines.Add(barText);
                barText.ParentLineData = progText;
                barText.ChildLines.Add(back1);

                LineData progAtt = new LineData();
                progAtt.TextOptions.Add("Cyber-Attack");
                progAtt.IsPlayerLine = true;
                progAtt.Actions.Add(new LineActionData() { Type = "1" });
                progAtt.Actions.Add(new LineActionData() { Type = "0" });

                LineData attText = new LineData();
                attText.TextOptions.Add("Cyber-Attack has been massively overhauled. Instead of a percentage, cyber-attack is now a number like cyber-defense. In order for viruses to infect a ship, your cyber-attack must be higher than the target's cyber-defense. Locking on to a ship with the sensor dish will show your relative cyber rating for that ship instead of your cyber-attack. Additionally, some viruses incur a cyber-attack bonus/penalty when infecting with \"slight\" being +-0.25 and \"moderate\" being +-0.5. All viruses will automatically fail to infect on their first three tries in order to give Scientists a small window of time to react to them. There are also some Cyber-Attack boosting components added to the Processor and Program Shops.");
                attText.Actions.Add(new LineActionData() { Type = "1" });
                attText.Actions.Add(new LineActionData() { Type = "0" });

                progAtt.ChildLines.Add(attText);
                attText.ParentLineData = progText;
                attText.ChildLines.Add(back1);

                LineData progDet = new LineData();
                progDet.TextOptions.Add("Detector & BLRC");
                progDet.IsPlayerLine = true;
                progDet.Actions.Add(new LineActionData() { Type = "1" });
                progDet.Actions.Add(new LineActionData() { Type = "0" });

                LineData detText = new LineData();
                detText.TextOptions.Add("Detector now reveals every ship within a sector for its duration. Useful for finding those pesky cloaked ships. All ships that normally start with a \"Block Long Range Comms\" program now start with Detector instead.");
                detText.Actions.Add(new LineActionData() { Type = "1" });
                detText.Actions.Add(new LineActionData() { Type = "0" });

                progDet.ChildLines.Add(detText);
                detText.ParentLineData = progText;
                detText.ChildLines.Add(back1);

                LineData progCor = new LineData();
                progCor.TextOptions.Add("Corruption");
                progCor.IsPlayerLine = true;
                progCor.Actions.Add(new LineActionData() { Type = "1" });
                progCor.Actions.Add(new LineActionData() { Type = "0" });

                LineData corText = new LineData();
                corText.TextOptions.Add("Corruption will now slowly deal system damage over time in addition to its usual screen corruption effect. It also now corrupts every ship system, which includes the piloting system.");
                corText.Actions.Add(new LineActionData() { Type = "1" });
                corText.Actions.Add(new LineActionData() { Type = "0" });

                progCor.ChildLines.Add(corText);
                corText.ParentLineData = progText;
                corText.ChildLines.Add(back1);

                progText.ChildLines.Add(progBoost);
                progText.ChildLines.Add(progBar);
                progText.ChildLines.Add(progAtt);
                progText.ChildLines.Add(progDet);
                progText.ChildLines.Add(progCor);
                progText.ChildLines.Add(back);

                LineData openerOptionSensor = new LineData();
                openerOptionSensor.TextOptions.Add("Sensors");
                openerOptionSensor.IsPlayerLine = true;
                openerOptionSensor.Actions.Add(new LineActionData() { Type = "1" });
                openerOptionSensor.Actions.Add(new LineActionData() { Type = "0" });

                LineData sensorText = new LineData();
                sensorText.TextOptions.Add("Listed are some of the changes to sensors and ship detection.");
                sensorText.Actions.Add(new LineActionData() { Type = "1" });
                sensorText.Actions.Add(new LineActionData() { Type = "0" });

                openerOptionSensor.ChildLines.Add(sensorText);
                sensorText.ParentLineData = opener;

                LineData sensorCloak = new LineData();
                sensorCloak.TextOptions.Add("Ship Cloak");
                sensorCloak.IsPlayerLine = true;
                sensorCloak.Actions.Add(new LineActionData() { Type = "1" });
                sensorCloak.Actions.Add(new LineActionData() { Type = "0" });

                LineData cloakText = new LineData();
                cloakText.TextOptions.Add("Cloaking no longer sets a ship's EM signature to 0 and instead reduces it by a set amount, and active cloaks can also boost ship stats. Both of these values can be seen on the cloak component's stat card. Additionally, the cloak no longer has its own slider on the science subsystem.");
                cloakText.Actions.Add(new LineActionData() { Type = "1" });
                cloakText.Actions.Add(new LineActionData() { Type = "0" });

                sensorCloak.ChildLines.Add(cloakText);
                cloakText.ParentLineData = sensorText;
                cloakText.ChildLines.Add(back1);

                LineData sensorScan = new LineData();
                sensorScan.TextOptions.Add("Component Scanning");
                sensorScan.IsPlayerLine = true;
                sensorScan.Actions.Add(new LineActionData() { Type = "1" });
                sensorScan.Actions.Add(new LineActionData() { Type = "0" });

                LineData scanText = new LineData();
                scanText.TextOptions.Add("Scanning a ship's components on the sensor screen has been massively improved. It is now possible to see almost everything a ship has equipped and get a better readout for each component scanned.");
                scanText.Actions.Add(new LineActionData() { Type = "1" });
                scanText.Actions.Add(new LineActionData() { Type = "0" });

                sensorScan.ChildLines.Add(scanText);
                scanText.ParentLineData = sensorText;
                scanText.ChildLines.Add(back1);

                sensorText.ChildLines.Add(sensorCloak);
                sensorText.ChildLines.Add(sensorScan);
                sensorText.ChildLines.Add(back);

                LineData openerOptionCombat = new LineData();
                openerOptionCombat.TextOptions.Add("Ship Combat");
                openerOptionCombat.IsPlayerLine = true;
                openerOptionCombat.Actions.Add(new LineActionData() { Type = "1" });
                openerOptionCombat.Actions.Add(new LineActionData() { Type = "0" });

                LineData combatText = new LineData();
                combatText.TextOptions.Add("Listed are of the changes/additions related to ship combat.");
                combatText.Actions.Add(new LineActionData() { Type = "1" });
                combatText.Actions.Add(new LineActionData() { Type = "0" });

                openerOptionCombat.ChildLines.Add(combatText);
                combatText.ParentLineData = opener;

                LineData combatAuto = new LineData();
                combatAuto.TextOptions.Add("Turret Autofire");
                combatAuto.IsPlayerLine = true;
                combatAuto.Actions.Add(new LineActionData() { Type = "1" });
                combatAuto.Actions.Add(new LineActionData() { Type = "0" });

                LineData autoText = new LineData();
                autoText.TextOptions.Add("The Projectile Aim Assist toggle on the Aux. Reactor has been changed to Turret Autofire. This toggle must be active for ship turrets to fire autonomosly. The projectile aiming indicator is now always active for all projectile-based turrets.");
                autoText.Actions.Add(new LineActionData() { Type = "1" });
                autoText.Actions.Add(new LineActionData() { Type = "0" });

                combatAuto.ChildLines.Add(autoText);
                autoText.ParentLineData = combatText;
                autoText.ChildLines.Add(back1);

                LineData combatRange = new LineData();
                combatRange.TextOptions.Add("Turret Range");
                combatRange.IsPlayerLine = true;
                combatRange.Actions.Add(new LineActionData() { Type = "1" });
                combatRange.Actions.Add(new LineActionData() { Type = "0" });

                LineData rangeText = new LineData();
                rangeText.TextOptions.Add("All turrets now have strict ranges, even while an actual human is in control. Turrets can not hit anything outside of their stated range. The ranges are listed on the turret component's stat card as usual. A handful of turrets have had their maximum ranges changed.");
                rangeText.Actions.Add(new LineActionData() { Type = "1" });
                rangeText.Actions.Add(new LineActionData() { Type = "0" });

                combatRange.ChildLines.Add(rangeText);
                rangeText.ParentLineData = combatText;
                rangeText.ChildLines.Add(back1);

                LineData combatPower = new LineData();
                combatPower.TextOptions.Add("Power Scaling");
                combatPower.IsPlayerLine = true;
                combatPower.Actions.Add(new LineActionData() { Type = "1" });
                combatPower.Actions.Add(new LineActionData() { Type = "0" });

                LineData powerText = new LineData();
                powerText.TextOptions.Add("Power usage scaling has been standardized across all turrets. Energy beam turrets use 20% more power per level, energy OR beam turrets use 10%, and turrets that do not have energy damage or beam attacks do not scale in power. This power usage scaling is also now applied to main turrets, though they use a bit less power at level 1 to compensate.");
                powerText.Actions.Add(new LineActionData() { Type = "1" });
                powerText.Actions.Add(new LineActionData() { Type = "0" });

                combatPower.ChildLines.Add(powerText);
                powerText.ParentLineData = combatText;
                powerText.ChildLines.Add(back1);

                /*
                LineData combatDmg = new LineData();
                combatDmg.TextOptions.Add("Damage Subtypes");
                combatDmg.IsPlayerLine = true;
                combatDmg.Actions.Add(new LineActionData() { Type = "1" });
                combatDmg.Actions.Add(new LineActionData() { Type = "0" });

                LineData dmgText = new LineData();
                dmgText.TextOptions.Add("");
                dmgText.Actions.Add(new LineActionData() { Type = "1" });
                dmgText.Actions.Add(new LineActionData() { Type = "0" });

                combatDmg.ChildLines.Add(dmgText);
                dmgText.ParentLineData = combatText;
                dmgText.ChildLines.Add(back1);
                */

                LineData combatMissile = new LineData();
                combatMissile.TextOptions.Add("Missiles");
                combatMissile.IsPlayerLine = true;
                combatMissile.Actions.Add(new LineActionData() { Type = "1" });
                combatMissile.Actions.Add(new LineActionData() { Type = "0" });

                LineData missileText = new LineData();
                missileText.TextOptions.Add("Missiles are now significantly better at tracking and are much less likely to get stuck circling around their targets. \"Are you familiar with G-pulling?\" - DrunkenCato, probably...");
                missileText.Actions.Add(new LineActionData() { Type = "1" });
                missileText.Actions.Add(new LineActionData() { Type = "0" });

                combatMissile.ChildLines.Add(missileText);
                missileText.ParentLineData = combatText;
                missileText.ChildLines.Add(back1);

                LineData combatNuke = new LineData();
                combatNuke.TextOptions.Add("Nuclear Devices");
                combatNuke.IsPlayerLine = true;
                combatNuke.Actions.Add(new LineActionData() { Type = "1" });
                combatNuke.Actions.Add(new LineActionData() { Type = "0" });

                LineData nukeText = new LineData();
                nukeText.TextOptions.Add("Nukes are now half price and do half damage BUT they now have an additional effect: successfully hitting a ship with one will make them take more damage for a short time. This time is scaled based on how close the ship was to the nuke when it detonated. Affected ships have a status effect indicator on the ship info bars on the top-right of the screen. The damage increase and the damage range can be seen on the nuke component's stat card. Additionally, the amount of nuke slots has been changed for some ships.");
                nukeText.Actions.Add(new LineActionData() { Type = "1" });
                nukeText.Actions.Add(new LineActionData() { Type = "0" });

                combatNuke.ChildLines.Add(nukeText);
                nukeText.ParentLineData = combatText;
                nukeText.ChildLines.Add(back1);

                LineData combatClaim = new LineData();
                combatClaim.TextOptions.Add("Ship Claim");
                combatClaim.IsPlayerLine = true;
                combatClaim.Actions.Add(new LineActionData() { Type = "1" });
                combatClaim.Actions.Add(new LineActionData() { Type = "0" });

                LineData claimText = new LineData();
                claimText.TextOptions.Add("In order to remove a ship's claim to extract its components, every screen must be captured instead of just half.");
                claimText.Actions.Add(new LineActionData() { Type = "1" });
                claimText.Actions.Add(new LineActionData() { Type = "0" });

                combatClaim.ChildLines.Add(claimText);
                claimText.ParentLineData = combatText;
                claimText.ChildLines.Add(back1);

                combatText.ChildLines.Add(combatAuto);
                combatText.ChildLines.Add(combatRange);
                combatText.ChildLines.Add(combatPower);
                //combatText.ChildLines.Add(combatDmg);
                combatText.ChildLines.Add(combatMissile);
                combatText.ChildLines.Add(combatNuke);
                combatText.ChildLines.Add(combatClaim);
                combatText.ChildLines.Add(back);

                opener.ChildLines.Add(openerOptionGeneral);
                opener.ChildLines.Add(openerOptionRelic);
                opener.ChildLines.Add(openerOptionToggles);
                opener.ChildLines.Add(openerOptionAI);
                opener.ChildLines.Add(openerOptionHull);
                opener.ChildLines.Add(openerOptionPrograms);
                opener.ChildLines.Add(openerOptionSensor);
                opener.ChildLines.Add(openerOptionCombat);

                data.OpeningLines.Add(opener);
                __result = data;
            }
        }

        private static string GetTextForDestinationSector()
        {
            string result = "We aren't headed to anywhere in particular at the moment. Feel free to browse our wares while we are anchored here!";
            if (RelicCaravan.CaravanTargetSector != -1)
            {
                PLSectorInfo sector = PLServer.GetSectorWithID(RelicCaravan.CaravanTargetSector);
                if (sector != null)
                {
                    switch (sector.VisualIndication)
                    {
                        case ESectorVisualIndication.CORNELIA_HUB:
                            result = "Our current heading is Cornelia. I heard someone there is in possesion of a data fragment if you're willing to do his dirty work.";
                            break;
                        case ESectorVisualIndication.DESERT_HUB:
                            result = "We are currently headed to the Burrow. Watching crews scramble around in the arena never gets old! They also have quite the selection of handheld weaponry too.";
                            break;
                        case ESectorVisualIndication.AOG_HUB:
                            if (PLServer.Instance.CrewFactionID == 2)
                                result = "We are headed to the Es... Harbor! It's a great place with a lot of great people! You should head there as soon as possible!";
                            else
                                result = "We are headed to the Estate. It's got one of the best bars in the galaxy. Our pilot frequents the specials menu every time we anchor there.";
                            break;
                        case ESectorVisualIndication.GENTLEMEN_START:
                            if (PLServer.Instance.CrewFactionID == 1)
                                result = "We are headed to the hideout. We have a client there who is a reputable borthix trader if you're looking get your hands on some.";
                            else
                                result = "We are headed to... nowhere in particluar. Feel free to browse our wares while we are here.";
                            break;
                        case ESectorVisualIndication.THE_HARBOR:
                            if (PLServer.Instance.CrewFactionID == 1)
                                result = "We are currently headed to the Harbor. It's a nice place but be careful, they aren't too fond of Gentlemen there";
                            else
                                result = "We are currently headed to the Harbor. It may be remote, but it's a great place to resupply.";
                            break;
                    }
                }
            }
            return result;
        }
    }
}
