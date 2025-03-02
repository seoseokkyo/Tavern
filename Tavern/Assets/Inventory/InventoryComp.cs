using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEditor.Timeline.Actions.MenuPriority;
using System;

public class InventoryComp : MonoBehaviour
{
    private List<ItemBase> inventory = new List<ItemBase>();

    public delegate void OnInventoryChanged();
    public OnInventoryChanged OnChanged;

    public InventoryUI InventoryUI_prefab;
    private InventoryUI InventoryUI;

    void Start()
    {
        InventoryUI = Instantiate(InventoryUI_prefab);

        InventoryUI.SetOwnerInventory(this);

        Canvas canvas = FindFirstObjectByType<Canvas>(); // 씬에 Canvas가 하나만 있을 경우

        InventoryUI.transform.SetParent(canvas.transform, false); // Canvas의 자식으로 설정 (worldPositionStays = false)

        // RectTransform을 사용하여 UI 위치 및 크기 설정 (선택 사항)
        RectTransform rectTransform = InventoryUI.GetComponent<RectTransform>();

        OnChanged();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void InventoryInitialize(int inventorySize)
    {
        // 이후 네트워크 붙으면 Server에서 이니셜
        inventory = Enumerable.Repeat<ItemBase>(null, inventorySize).ToList();
    }

    public void CheckInventory()
    {
        string strInventory = "";
        for (int i = 0; i < inventory.Count; i++)
        {
            string strTemp = "";
            if (CheckItem(i) != null)
            {
                strTemp = $"{i} : {CheckItem(i).CurrentItemData.itemName}, ";
            }
            else
            {
                strTemp = $"{i} : Empty, ";
            }

            strInventory += strTemp;
        }
        Debug.Log(strInventory);
    }

    public bool AddItem(ref ItemBase addItem)
    {
        if (addItem == null)
            return false;

        bool bCheck = false;

        for (int i = 0; i < inventory.Count; i++)
        {
            if (inventory[i] == null)
                continue;

            if (inventory[i].CurrentItemData.itemName == addItem.CurrentItemData.itemName &&
                inventory[i].CurrentItemData.itemCount < inventory[i].CurrentItemData.itemCountLimit)
            {
                int val = Math.Min(addItem.CurrentItemData.itemCount, inventory[i].CurrentItemData.itemCountLimit - inventory[i].CurrentItemData.itemCount);
                inventory[i].CurrentItemData.itemCount = inventory[i].CurrentItemData.itemCount + val;
                addItem.CurrentItemData.itemCount = addItem.CurrentItemData.itemCount - val;
                bCheck = true;

                if (addItem.CurrentItemData.itemCount == 0)
                    break;
            }
        }

        for (int i = 0; i < inventory.Count; i++)
        {
            if (addItem.CurrentItemData.itemCount == 0)
                break;

            if (inventory[i] == null)
            {
                inventory[i] = addItem;
                bCheck = true;
                break;
            }
        }

        CheckInventory();

        OnChanged();

        return bCheck;
    }

    public ItemBase SwapItem(ref ItemBase currentItem, int index)
    {
        ItemBase itemCheck = inventory[index];

        inventory[index] = currentItem;

        return itemCheck;
    }

    public void SwapItemByIndex(int SwapTargetIndexX, int SwapTargetIndexY)
    {
        ItemBase SwapItem = inventory[SwapTargetIndexX];

        inventory[SwapTargetIndexX] = inventory[SwapTargetIndexY];
        inventory[SwapTargetIndexY] = SwapItem;

        OnChanged();
    }

    public ItemBase CheckItem(int index)
    {
        return inventory[index];
    }

    public ItemBase PopItem(int index)
    {
        ItemBase itemCheck = inventory[index];

        inventory[index] = null;

        OnChanged();

        return itemCheck;
    }

    public int GetInventorySize()
    {
        return inventory.Count;
    }
}
