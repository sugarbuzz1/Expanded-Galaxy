using HarmonyLib;
using PulsarModLoader.Content.Components.Missile;
using PulsarModLoader.Patches;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;

namespace ExpandedGalaxy
{
    internal class Missiles
    {
        private class SwarmerMissile : MissileMod
        {
            public override string Name => "Seeker Missile";

            public override string Description => "Extremely fast missile designed to disable the shields of even the most agile ship.";

            public override int MarketPrice => 6800;

            public override bool Experimental => true;

            public override float Damage => 40f;

            public override float Speed => 20f;

            public override EDamageType DamageType => EDamageType.E_SHIELD_PIERCE_PHYS;

            public override int MissileRefillPrice => 600;

            public override int AmmoCapacity => 10;

            public override int PrefabID => 2;
        }

        [HarmonyPatch(typeof(PLTurret), "ServerFireMissile")]
        internal class MissileTag
        {
            private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
            {
                Label failed = generator.DefineLabel();
                List<CodeInstruction> list = instructions.ToList();

                List<CodeInstruction> targetSequence = new List<CodeInstruction>() {
                    new CodeInstruction(OpCodes.Ldloc_0),
                    new CodeInstruction(OpCodes.Ldc_R4, 30f),
                    new CodeInstruction(OpCodes.Ldarg_1),
                    new CodeInstruction(OpCodes.Ldfld),
                    new CodeInstruction(OpCodes.Mul),
                    new CodeInstruction(OpCodes.Stfld)
                };
                List<CodeInstruction> patchSequence = new List<CodeInstruction>()
                {
                    new CodeInstruction(OpCodes.Ldarg_1),
                    CodeInstruction.Call(typeof(PLWare), "get_Name"),
                    new CodeInstruction(OpCodes.Ldstr, "Seeker Missile"),
                    CodeInstruction.Call(typeof(System.String), "op_Equality", new Type[2] {
                        typeof(string),
                        typeof(string)
                    }),
                    new CodeInstruction(OpCodes.Brfalse, failed),
                    new CodeInstruction(OpCodes.Ldloc_0),
                    new CodeInstruction(OpCodes.Ldloc_0),
                    CodeInstruction.Call(typeof(UnityEngine.Object), "get_name"),
                    new CodeInstruction(OpCodes.Ldstr, " (seeker)"),
                    CodeInstruction.Call(typeof(string), "Concat", new Type[2]
                    {
                        typeof(string),
                        typeof(string)
                    }),
                    CodeInstruction.Call(typeof(UnityEngine.Object), "set_name", new Type[1]
                    {
                        typeof (string),
                    }),
                };
                list[55].labels.Add(failed);
                return HarmonyHelpers.PatchBySequence(list.AsEnumerable<CodeInstruction>(), targetSequence, patchSequence, HarmonyHelpers.PatchMode.AFTER, HarmonyHelpers.CheckMode.NONNULL, false);
            }
        }

        [HarmonyPatch(typeof(PLServer), "ServerTakeDamageProjectile")]
        internal class SeekerDamage
        {
            private static bool Prefix(PLServer __instance, int inShipID, float dmg, bool bottomHit, int inProjID, int dmgType, int SystemTargetID, int attackingShipID, int turretID, ref List<int> ___ProcessedProjIDs)
            {
                PLShipInfoBase shipFromId = PLEncounterManager.Instance.GetShipFromID(inShipID);
                PLSpaceTarget spaceTargetFromId = PLEncounterManager.Instance.GetSpaceTargetFromID(inShipID);
                if ((UnityEngine.Object)shipFromId != (UnityEngine.Object)null)
                {
                    if (___ProcessedProjIDs.Contains(inProjID))
                        return false;
                    bool flag = false;
                    foreach (PLProjectile projectile in __instance.m_ActiveProjectiles)
                    {
                        if (projectile != null && projectile.ProjID == inProjID && projectile.name.Contains("(seeker)"))
                        {
                            flag = true;
                            break;
                        }

                    }
                    if (!flag)
                        return true;
                    Systems.TickDamage(10, shipFromId, dmg, false, EDamageType.E_ARMOR_PIERCE_PHYS, -1, PLEncounterManager.Instance.GetShipFromID(attackingShipID), -1);
                    ___ProcessedProjIDs.Add(inProjID);
                    return false;
                }
                else
                {
                    if ((UnityEngine.Object)spaceTargetFromId == (UnityEngine.Object)null || ___ProcessedProjIDs.Contains(inProjID))
                        return false;
                    bool flag = false;
                    foreach (PLProjectile projectile in __instance.m_ActiveProjectiles)
                    {
                        if (projectile != null && projectile.ProjID == inProjID && projectile.name.Contains("(seeker)"))
                        {
                            flag = true;
                            break;
                        }

                    }
                    if (!flag)
                        return true;
                    ___ProcessedProjIDs.Add(inProjID);
                    Systems.TickDamage(10, spaceTargetFromId, dmg, false, EDamageType.E_ARMOR_PIERCE_PHYS, -1, null, -1);
                }
                return true;
            }
        }

        private class ThermobaricMissileMod : MissileMod
        {
            public override string Name => "Thermobaric Missile";

            public override string Description => "Tracker missile that deals fire damage.";

            public override int MarketPrice => 2000;

            public override bool Contraband => true;

            public override float Damage => 405f;

            public override float Speed => 4f;

            public override EDamageType DamageType => EDamageType.E_FIRE;

            public override int MissileRefillPrice => 200;

            public override int AmmoCapacity => 16;

            public override int PrefabID => 1;

            public override bool CanBeDroppedOnShipDeath => false;
        }
    }
}
