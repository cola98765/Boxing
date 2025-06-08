using Il2Cpp;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Boxing
{
    internal class BoxingFunctionalities
    {
        internal static string pileText;
        private static GameObject pileButton;
        internal static GearItem pileItem;
        internal static string unPileText;
        private static GameObject unPileButton;
        internal static GearItem unPileItem;
        internal static int pileindex;
        internal static float totalhp;

        internal static void InitializeMTB(ItemDescriptionPage itemDescriptionPage)
        {
            pileText = "Pile";
            unPileText = "Unpile";

            GameObject equipButton = itemDescriptionPage.m_MouseButtonEquip;
            
            pileButton = UnityEngine.Object.Instantiate<GameObject>(equipButton, equipButton.transform.parent, true);
            pileButton.transform.Translate(0.36f, -0.1f, 0);
            Utils.GetComponentInChildren<UILabel>(pileButton).text = pileText;

            unPileButton = UnityEngine.Object.Instantiate<GameObject>(equipButton, equipButton.transform.parent, true);
            unPileButton.transform.Translate(0.72f, -0.1f, 0);
            Utils.GetComponentInChildren<UILabel>(unPileButton).text = unPileText;

            AddAction(pileButton, new System.Action(OnPile));
            AddAction(unPileButton, new System.Action(OnUnPile));

            SetPileActive(false);
            SetUnPileActive(false);

        }
        private static void AddAction(GameObject button, System.Action action)
        {
            Il2CppSystem.Collections.Generic.List<EventDelegate> placeHolderList = new Il2CppSystem.Collections.Generic.List<EventDelegate>();
            placeHolderList.Add(new EventDelegate(action));
            Utils.GetComponentInChildren<UIButton>(button).onClick = placeHolderList;
        }


        internal static void SetPileActive(bool active)
        {
            NGUITools.SetActive(pileButton, active);
        }

        internal static void SetUnPileActive(bool active)
        {
            NGUITools.SetActive(unPileButton, active);
        }

        private static void OnPile()
        {
            var thisGearItem = pileItem;
            if (thisGearItem == null) return;
            for (int j = 0; j < BoxingUtils.pilable.Count; j++)
            {
                if (BoxingUtils.pilable[j] == thisGearItem.name)
                {
                    float[] hp = new float[BoxingUtils.pilesize[j]];
                    int i = 0;
                    totalhp = 0;
                    for (i = 0; i < BoxingUtils.pilesize[j]; i++)
                    {
                        GearItem can = GameManager.GetInventoryComponent().GetHighestConditionGearThatMatchesName(BoxingUtils.pilable[j]);
                        if (can == null) break;
                        hp[i] = can.m_CurrentHP;
                        totalhp += can.GetNormalizedCondition();
                        if (can.m_StackableItem != null)
                        {
                            can.m_StackableItem.m_Units--;
                            if (can.m_StackableItem.m_Units == 0) GameManager.GetInventoryComponent().RemoveGear(can.gameObject);
                        }
                        else GameManager.GetInventoryComponent().RemoveGear(can.gameObject);

                    }
                    if (i == BoxingUtils.pilesize[j])
                    {
                        pileindex = j;
                        totalhp /= BoxingUtils.pilesize[j];
                        InterfaceManager.GetPanel<Panel_GenericProgressBar>().Launch("Piling...", 1f, 0f, 0f,
                                        "PLAY_CRAFTINGACORNSSHELLING", null, false, true, new System.Action<bool, bool, float>(OnPileFinished));
                    }
                    else
                    {
                        HUDMessage.AddMessage("Not enought to pile, you need " + BoxingUtils.pilesize[j]);
                        i--;
                        for (; i >= 0; i--)
                        {
                            GameManager.GetPlayerManagerComponent().InstantiateItemInPlayerInventory(thisGearItem).m_CurrentHP = hp[i];
                        }
                    }
                    break;
                }
            }
        }
        private static void OnPileFinished(bool success, bool playerCancel, float progress)
        {
            GearItem toadd = Addressables.LoadAssetAsync<GameObject>(BoxingUtils.unpilable[pileindex]).WaitForCompletion().GetComponent<GearItem>();
            GameManager.GetPlayerManagerComponent().InstantiateItemInPlayerInventory(toadd, 1, totalhp);
        }

        private static void OnUnPile()
        {

            var thisGearItem = unPileItem;
            if (thisGearItem == null) return;
            for (int j = 0; j < BoxingUtils.unpilable.Count; j++)
            {
                if (BoxingUtils.unpilable[j] == thisGearItem.name)
                {
                    GearItem can = GameManager.GetInventoryComponent().GetHighestConditionGearThatMatchesName(BoxingUtils.unpilable[j]);
                    if (can == null) break;
                    totalhp = can.m_CurrentHP;
                    if (can.m_StackableItem != null)
                    {
                        can.m_StackableItem.m_Units--;
                        if (can.m_StackableItem.m_Units == 0) GameManager.GetInventoryComponent().RemoveGear(can.gameObject);
                    }
                    else GameManager.GetInventoryComponent().RemoveGear(can.gameObject);

                    pileindex = j;
                    InterfaceManager.GetPanel<Panel_GenericProgressBar>().Launch("Unpiling...", 1f, 0f, 0f,
                                "PLAY_CRAFTINGACORNSSHELLING", null, false, true, new System.Action<bool, bool, float>(OnUnPileFinished));

                    break;
                }
            }
        }
        private static void OnUnPileFinished(bool success, bool playerCancel, float progress)
        {
            GearItem toadd = Addressables.LoadAssetAsync<GameObject>(BoxingUtils.pilable[pileindex]).WaitForCompletion().GetComponent<GearItem>();
            for (int i = 0; i < BoxingUtils.pilesize[pileindex]; i++)
            {
                GameManager.GetPlayerManagerComponent().InstantiateItemInPlayerInventory(toadd, 1, totalhp);
            }
        }
    }
}
