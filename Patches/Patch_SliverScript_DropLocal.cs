using HarmonyLib;
using GadgetCore.API;

namespace GadgetCore.Patches
{
    [HarmonyPatch(typeof(SliverScript))]
    [HarmonyPatch("DropLocal")]
    static class Patch_SliverScript_DropLocal
    {
        [HarmonyPostfix]
        public static void Postfix(SliverScript __instance)
        {
            LootTables.DropLoot("entity:" + __instance.name.Split(' ', '(')[0], __instance.transform.position);
        }
    }
}