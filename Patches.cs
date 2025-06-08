using Il2Cpp;
using HarmonyLib;

namespace Boxing
{
    internal class Patches
    {
        [HarmonyPatch(typeof(Panel_Inventory), nameof(Panel_Inventory.Initialize))]
        internal class BoxingInitialization
        {
            private static void Postfix(Panel_Inventory __instance)
            {
                BoxingUtils.inventory = __instance;
                BoxingFunctionalities.InitializeMTB(__instance.m_ItemDescriptionPage);
            }
        }
        [HarmonyPatch(typeof(ItemDescriptionPage), nameof(ItemDescriptionPage.UpdateGearItemDescription))]
        internal class UpdateInventoryButton
        {
            private static void Postfix(ItemDescriptionPage __instance, GearItem gi)
            {
                if (__instance != InterfaceManager.GetPanel<Panel_Inventory>()?.m_ItemDescriptionPage) return;
                BoxingFunctionalities.pileItem = gi?.GetComponent<GearItem>();
                BoxingFunctionalities.unPileItem = gi?.GetComponent<GearItem>();
                if (gi != null && BoxingUtils.IsItemPileable(gi.name) == true)
                {
                    BoxingFunctionalities.SetPileActive(true);
                }
                else
                {
                    BoxingFunctionalities.SetPileActive(false);
                }

                if (gi != null && BoxingUtils.IsItemUnPileable(gi.name) == true)
                {
                    BoxingFunctionalities.SetUnPileActive(true);
                }
                else
                {
                    BoxingFunctionalities.SetUnPileActive(false);
                }
            }
        }
    }
}
    