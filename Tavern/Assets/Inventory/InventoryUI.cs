using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    public Transform ContentTransform;
    public GameObject ItemView;

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
        Debug.Log($"ItemViewList Size : {ItemViewList.Count - 1}");

        for (int i = ItemViewList.Count - 1; i >= 0; i--)
        {
            GameObject temp = ItemViewList[i];

            ItemViewList.RemoveAt(i);
            Destroy(temp);
        }

        int iCount = ContentTransform.childCount;
        for (int i = iCount - 1; i >= 0; i--)
        {
            var temp = ContentTransform.GetChild(i);

            if (temp != null)
            {
                var ItemUITemp = temp.GetComponent<ItemUI>();

                if (ItemUITemp != null)
                {
                    Destroy(temp.gameObject);
                }
            }
        }
    }

    void RefreshInventory()
    {
        ClearList();

        Debug.Log($"Inventory Size : {PlayerInventory.GetInventorySize()}");

        for (int i = 0; i < PlayerInventory.GetInventorySize(); i++)
        {
            GameObject prefab = Instantiate(ItemView);

            ItemUI TempItemView = prefab.GetComponent<ItemUI>();

            if (TempItemView != null)
            {
                if (PlayerInventory.CheckItem(i) != null)
                {
                    TempItemView.InitData(PlayerInventory.CheckItem(i).CurrentItemData.itemIcon, ContentTransform, PlayerInventory.CheckItem(i).CurrentItemData.itemCount);
                }
                else
                {
                    TempItemView.InitData(EmptyIcon, ContentTransform, 0);
                }
            }

            ItemViewList.Add(prefab);
        }
    }
}
