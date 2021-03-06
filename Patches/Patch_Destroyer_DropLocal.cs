using HarmonyLib;
using GadgetCore.API;
using GadgetCore;
using UnityEngine;
using System.Reflection;

namespace GadgetCore.Patches
{
    [HarmonyPatch(typeof(Destroyer))]
    [HarmonyPatch("DropLocal")]
    static class Patch_Destroyer_DropLocal
    {
        [HarmonyPostfix]
        public static void Postfix(Destroyer __instance)
        {
            LootTables.DropLoot("entity:" + __instance.name.Split(' ', '(')[0], __instance.transform.position);
        }
    }
}