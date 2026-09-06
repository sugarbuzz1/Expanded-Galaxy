using HarmonyLib;
using PulsarModLoader;
using System.Collections.Generic;
using UnityEngine;
using static ExpandedGalaxy.Nukes;

namespace ExpandedGalaxy
{
    [HarmonyPatch(typeof(PLNuke), "DamageFunction")]
    internal class NukeDamageEffect
    {
        private static void Postfix(PLNuke __instance, Vector3 pos)
        {
            if (!PhotonNetwork.isMasterClient)
                return;
            if (__instance.OwnerShip == null || __instance.OwnerShip.LoadedNuclearDevice == null)
                return;
            int effect = -1;
            float strength = 1f;
            switch (__instance.OwnerShip.LoadedNuclearDevice.SubType)
            {
                case 0:
                    effect = 0;
                    strength = 1.35f;
                    break;
                case 1:
                    effect = 0;
                    strength = 1.4f;
                    break;
                case 2:
                    effect = 0;
                    strength = 1.3f;
                    break;
                case 4:
                    effect = 0;
                    strength = 1.45f;
                    break;
                case 6:
                    effect = 0;
                    strength = 1.25f;
                    break;
            }
            if (__instance.OwnerShip.LoadedNuclearDevice is PLBiscuitBombComponent)
                effect = -1;
            if (effect == -1)
                return;
            foreach (PLShipInfoBase plShipInfoBase in PLEncounterManager.Instance.AllShips.Values)
            {
                if (plShipInfoBase != null && plShipInfoBase.ExteriorRigidbody != null)
                {
                    float distance = (plShipInfoBase.ExteriorRigidbody.transform.position - pos).magnitude * 5f;
                    if (distance > __instance.Range)
                        continue;
                    float lengthMod = distance / __instance.Range;
                    if (distance < 1000f)
                        lengthMod = 1f;
                    if (AllStatusDatas.ContainsKey(plShipInfoBase.SpaceTargetID))
                    {
                        ShipStatusEffectData data = AllStatusDatas[plShipInfoBase.SpaceTargetID];
                        data.EffectStrength = strength;
                        data.RemoveTime = Time.time + 90f * lengthMod;
                    }
                    else
                    {
                        ShipStatusEffectData data = new ShipStatusEffectData();
                        data.EffectId = effect;
                        data.EffectStrength = strength;
                        data.RemoveTime = Time.time + 90f * lengthMod;
                        AllStatusDatas.Add(plShipInfoBase.SpaceTargetID, data);
                    }
                }
            }
            foreach (PLDamageableSpaceObject damageableSpaceObject in PLDamageableSpaceObject.All)
            {
                if (damageableSpaceObject != null && damageableSpaceObject.Damageable)
                {
                    float distance = (damageableSpaceObject.transform.position - pos).magnitude * 5f;
                    if (distance > __instance.Range)
                        continue;
                    float lengthMod = distance / __instance.Range;
                    if (distance < 1000f)
                        lengthMod = 1f;
                    if (AllStatusDatas.ContainsKey(damageableSpaceObject.SpaceTargetID))
                    {
                        ShipStatusEffectData data = AllStatusDatas[damageableSpaceObject.SpaceTargetID];
                        data.EffectStrength = strength;
                        data.RemoveTime = Time.time + 90f * lengthMod;
                    }
                    else
                    {
                        ShipStatusEffectData data = new ShipStatusEffectData();
                        data.EffectId = effect;
                        data.EffectStrength = strength;
                        data.RemoveTime = Time.time + 90f * lengthMod;
                        AllStatusDatas.Add(damageableSpaceObject.SpaceTargetID, data);
                    }
                }
            }
            List<object> sendData = new List<object>();
            sendData.Add(AllStatusDatas.Count);
            foreach (int key in AllStatusDatas.Keys)
            {
                ShipStatusEffectData data = AllStatusDatas[key];
                sendData.Add(key);
                sendData.Add(data.EffectId);
                sendData.Add(data.EffectStrength);
            }
            ModMessage.SendRPC("sugarbuzz1.ExpandedGalaxy", "ExpandedGalaxy.UpdateStatusDatas", PhotonTargets.Others, sendData.ToArray());
        }
    }
}
