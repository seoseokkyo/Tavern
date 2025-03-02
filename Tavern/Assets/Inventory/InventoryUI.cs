using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    public Transform ContentTransform;
    public GameObject ItemSlotUI;
    public GameObject ItemUI;

    public Texture2D EmptyIcon;

    List<GameObject> ItemViewList = new List<GameObject>();

    private InventoryComp PlayerInventory;


    void Start()
    {
        ItemViewList.Clear();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetOwnerInventory(InventoryComp Inventory)
    {
        PlayerInventory = Inventory;

        PlayerInventory.OnChanged += RefreshInventory;
    }

    void ClearList()
    {
        int iCount = ContentTransform.childCount;
        for (int i = iCount - 1; i >= 0; i--)
        {
            var temp = ContentTransform.GetChild(i);

            if (temp != null)
            {
                var ItemSlotUITemp = temp.GetComponent<ItemSlotUI>();
                if (ItemSlotUITemp != null)
                {
                    Destroy(temp.gameObject);
                }

                //var ItemUITemp = temp.GetComponent<ItemUI>();

                //if (ItemUITemp != null)
                //{
                //    Destroy(temp.gameObject);
                //}
            }
        }
    }

    void RefreshInventory()
    {
        ClearList();

        for (int i = 0; i < PlayerInventory.GetInventorySize(); i++)
        {
            GameObject instItemSlotUI = Instantiate(ItemSlotUI);
            instItemSlotUI.transform.SetParent(ContentTransform);
            ItemSlotUI TempItemSlotUI = instItemSlotUI.GetComponent<ItemSlotUI>();
            TempItemSlotUI.SlotIndex = i;

            GameObject instItemUI = Instantiate(ItemUI);
            ItemUI TempItemView = instItemUI.GetComponent<ItemUI>();
            

            if (TempItemView != null)
            {
                if (PlayerInventory.CheckItem(i) != null)
                {
                    TempItemView.InitData(PlayerInventory.CheckItem(i).CurrentItemData.itemIcon, instItemSlotUI.transform, PlayerInventory.CheckItem(i).CurrentItemData.itemCount, i);
                }
                //else
                //{
                //    TempItemView.InitData(EmptyIcon, instItemSlotUI.transform, 0);
                //}
            }

            ItemViewList.Add(instItemUI);
        }
    }
    public void SwapItemFromUI(int SwapTargetIndexX, int SwapTargetIndexY)
    {
        // 나중에 슬롯이랑 아이템UI 완성 되면 실제 아이템이 있는 슬롯인지 확인하는거 추가
        PlayerInventory.SwapItemByIndex(SwapTargetIndexX, SwapTargetIndexY);
    }
}
