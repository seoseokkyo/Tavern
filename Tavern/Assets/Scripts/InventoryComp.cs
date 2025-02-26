using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEditor.Timeline.Actions.MenuPriority;

public class InventoryComp : MonoBehaviour
{
    private List<ItemBase> inventory = new List<ItemBase>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

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
            if (inventory[i] != null)
            {
                strTemp = $"{i} : {inventory[i].CurrentItemData.itemName}, ";
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
            {
                inventory[i] = addItem;
                bCheck = true;
                break;
            }
        }

        CheckInventory();

        return bCheck;
    }

    public ItemBase SwapItem(ref ItemBase currentItem, int index)
    {
        ItemBase itemCheck = inventory[index];

        inventory[index] = currentItem;

        return itemCheck;
    }

    public ItemBase CheckItem(int index)
    {
        return inventory[index];
    }

    public ItemBase PopItem(int index)
    {
        ItemBase itemCheck = inventory[index];

        inventory[index] = null;

        return itemCheck;
    }

    public int GetInventorySize()
    {
        return inventory.Count;
    }
}
