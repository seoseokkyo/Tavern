using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class InventoryComp : MonoBehaviour
{
    public List<ItemBase> inventory = new List<ItemBase>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }



    void InventoryInitialize(int inventorySize)
    {
        // 이후 네트워크 붙으면 Server에서 이니셜
        inventory = new List<ItemBase>(inventorySize);
    }

    void AddItem(ItemBase addItem)
    {

    }
}
