using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEditor.Timeline.Actions.MenuPriority;
using System;
using System.Threading;
using static UnityEditor.Progress;

public class InventoryComp : MonoBehaviour
{
    private List<ItemBase> inventory = new List<ItemBase>();

    public delegate void OnInventoryChanged();
    public OnInventoryChanged OnChanged;

    void Start()
    {
        //OnChanged();
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
                inventory[i].OwnerInventory = this;
                bCheck = true;
                break;
            }
        }

        //CheckInventory();

        OnChanged();

        return bCheck;
    }

    public ItemBase SwapItem(ref ItemBase currentItem, int index)
    {
        ItemBase itemCheck = inventory[index];

        inventory[index] = currentItem;

        return itemCheck;
    }

    public void SwapItemByIndex(ref InventoryComp InventoryX, ref InventoryComp InventoryY, int SwapTargetIndexX, int SwapTargetIndexY)
    {
        if (null != InventoryX && null != InventoryY)
        {
            var TestX = InventoryX;
            var TestY = InventoryY;

            var SwapItem = InventoryX.inventory[SwapTargetIndexX];
            if (null != SwapItem)
            {
                SwapItem.OwnerInventory = InventoryY;
            }

            InventoryX.inventory[SwapTargetIndexX] = InventoryY.inventory[SwapTargetIndexY];
            InventoryY.inventory[SwapTargetIndexY] = SwapItem;

            if (null != TestX.inventory[SwapTargetIndexX])
            {
                TestX.inventory[SwapTargetIndexX].OwnerInventory = TestX;
            }

            if (null != TestY.inventory[SwapTargetIndexY])
            {
                TestY.inventory[SwapTargetIndexY].OwnerInventory = TestY;
            }

            if (TestX != TestY)
            {
                TestX.OnChanged();
                TestY.OnChanged();
            }
            else
            {
                OnChanged();
            }
        }
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

    public int CountItemByName(string ItemName)
    {
        int InventorySize = GetInventorySize();

        int ItemCount = 0;

        for (int i = 0; i < InventorySize; i++)
        {
            if (null != inventory[i])
            {
                if (inventory[i].CurrentItemData.itemName == ItemName)
                {
                    ItemCount += inventory[i].CurrentItemData.itemCount;
                }
            }
        }

        return ItemCount;
    }

    public bool ConsumeItem(string ItemName, int ConsumeNum)
    {
        if (CountItemByName(ItemName) < ConsumeNum)
        {
            return false;
        }

        int InventorySize = GetInventorySize();

        for (int i = 0; i < InventorySize; i++)
        {
            if (null != inventory[i] && inventory[i].CurrentItemData.itemName == ItemName)
            {
                int val = Math.Min(inventory[i].CurrentItemData.itemCount, ConsumeNum);
                inventory[i].CurrentItemData.itemCount -= val;
                ConsumeNum -= val;

                if (inventory[i].CurrentItemData.itemCount <= 0)
                {
                    inventory[i] = null;
                }

                if (ConsumeNum <= 0)
                {
                    break;
                }
            }
        }

        OnChanged();

        return true;
    }

    public void UseItemByIndex(PlayerController playerController, int UseItemIndex)
    {
        int InventorySize = GetInventorySize();

        if (0 > UseItemIndex || UseItemIndex >= InventorySize || null == inventory[UseItemIndex])
        {
            return;
        }

        inventory[UseItemIndex].UseItem(playerController);
    }

    public void ConsumeItem(ItemBase itemBase)
    {
        if(1 > CountItemByName(itemBase.CurrentItemData.itemName))
        {
            Debug.Log($"ConsumeItem(ItemBase itemBase) Failed");
            return;
        }

        int Size = GetInventorySize();
        for(int i = 0; i < Size; i++)
        {
            if (inventory[i] == itemBase)
            {
                inventory[i].CurrentItemData.itemCount--;

                if (inventory[i].CurrentItemData.itemCount <= 0)
                {
                    inventory[i] = null;
                }

                OnChanged();
            }
        }
    }
}
