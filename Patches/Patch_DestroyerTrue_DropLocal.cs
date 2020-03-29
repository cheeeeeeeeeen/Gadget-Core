using HarmonyLib;
using GadgetCore.API;

namespace GadgetCore.Patches
{
    [HarmonyPatch(typeof(DestroyerTrue))]
    [HarmonyPatch("DropLocal")]
    static class Patch_DestroyerTrue_DropLocal
    {
        [HarmonyPostfix]
        public static void Postfix(DestroyerTrue __instance)
        {
            LootTables.DropLoot("entity:" + __instance.name.Split(' ', '(')[0], __instance.transform.position);
        }
    }
}