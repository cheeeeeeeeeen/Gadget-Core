using HarmonyLib;
using GadgetCore.API;
using GadgetCore;

namespace GadgetCore.Patches
{
    [HarmonyPatch(typeof(GameScript))]
    [HarmonyPatch("SwapCombatChip")]
    static class Patch_GameScript_SwapCombatChip
    {
        [HarmonyPrefix]
        public static void Prefix(GameScript __instance, int slot, int[] ___combatChips)
        {
            int chipID = ___combatChips[slot];
            if (ChipRegistry.GetSingleton().HasEntry(chipID))
            {
                ChipRegistry.GetSingleton().GetEntry(chipID).InvokeOnDequip(slot);
            }
        }

        [HarmonyPostfix]
        public static void Postfix(GameScript __instance, int slot, int[] ___combatChips)
        {
            int chipID = ___combatChips[slot];
            if (ChipRegistry.GetSingleton().HasEntry(chipID))
            {
                ChipRegistry.GetSingleton().GetEntry(chipID).InvokeOnEquip(slot);
            }
        }
    }
}