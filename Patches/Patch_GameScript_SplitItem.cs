using HarmonyLib;
using GadgetCore.API;
using GadgetCore;
using UnityEngine;
using System.Collections;
using System.Reflection;

namespace GadgetCore.Patches
{
    [HarmonyPatch(typeof(GameScript))]
    [HarmonyPatch("SplitItem")]
    static class Patch_GameScript_SplitItem
    {
        public static readonly MethodInfo SelectItem = typeof(GameScript).GetMethod("SelectItem", BindingFlags.NonPublic | BindingFlags.Instance);

        [HarmonyPrefix]
        public static bool Prefix(GameScript __instance, int slot, ref Item ___holdingItem)
        {
            ItemInfo itemInfo = ItemRegistry.GetSingleton().GetEntry(___holdingItem.id);
            ItemType holdingItemType = itemInfo != null ? (itemInfo.Type & (ItemType.BASIC_MASK | ItemType.TYPE_MASK)) : ItemRegistry.GetDefaultTypeByID(___holdingItem.id);
            if ((holdingItemType & ItemType.NONSTACKING) == ItemType.NONSTACKING)
            {
                SelectItem.Invoke(__instance, new object[] { slot });
                return false;
            }
            return true;
        }
    }
}