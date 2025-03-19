using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine.XR;


public class TableScript : MonoBehaviour
{
    public List<SeatData> seats = new List<SeatData>();
    public GameObject foodPrefab;

    public ItemDatas itemDatas;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public bool HasAvailableSeat()
    {
        foreach(SeatData seat in seats)
        {
            if (!seat.isSitting)
                return true;
        }
        return false;
    }

    public SeatData GetAvailableSeat()
    {
        foreach (SeatData seat in seats)
        {
            if (!seat.isSitting)
            {
                seat.isSitting = true;
                return seat;
            }
        }
        return null;
    }

    public void ReleaseSeat(SeatData seat)
    {
        if(seat != null)
        {
            seat.isSitting = false;
        }

        RemoveFood(seat);
    }

    public void SetFood(ItemData food, SeatData seat)
    {
        if (seat != null)
        {
            var CreatedItemBase = ItemBase.ItemBaseCreator.CreateItemBase(food);
            WorldItem WorldItemTemp = ItemManager.Instance.ItemSpawn(CreatedItemBase, seat.foodPositionLeft.position, Quaternion.identity);
            if(seat.foodLeft == null)
            {
                WorldItemTemp.transform.SetParent(seat.foodPositionLeft);
                WorldItemTemp.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
                WorldItemTemp.transform.localScale = Vector3.one;
                seat.foodLeft = WorldItemTemp;
            }
            else if(seat.foodLeft != null)
            {
                WorldItemTemp.transform.SetParent(seat.foodPositionRight);
                WorldItemTemp.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
                WorldItemTemp.transform.localScale = Vector3.one;
                seat.foodRight = WorldItemTemp;
            }
        }
    }

    public void RemoveFood(SeatData seat)
    {
        if(seat.foodLeft != null)
        {
            ItemData temp = seat.foodLeft.item.CurrentItemData;
            ItemData usedTool = GetUsedToolItem(temp.requireToolType);
            var CreatedItemBase = ItemBase.ItemBaseCreator.CreateItemBase(usedTool);
            WorldItem WorldItemTemp = ItemManager.Instance.ItemSpawn(CreatedItemBase, seat.foodPositionLeft.position, Quaternion.identity); 

            WorldItemTemp.transform.SetParent(seat.foodPositionLeft);
            WorldItemTemp.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
            WorldItemTemp.transform.localScale = Vector3.one;

            Destroy(seat.foodLeft.gameObject);
            seat.foodLeft = null;
            seat.foodLeft = WorldItemTemp;
        }

        if (seat.foodRight != null)
        {
            ItemData temp = seat.foodRight.item.CurrentItemData;
            ItemData usedTool = GetUsedToolItem(temp.requireToolType);
            var CreatedItemBase = ItemBase.ItemBaseCreator.CreateItemBase(usedTool);
            WorldItem WorldItemTemp = ItemManager.Instance.ItemSpawn(CreatedItemBase, seat.foodPositionRight.position, Quaternion.identity); Destroy(seat.foodLeft);

            WorldItemTemp.transform.SetParent(seat.foodPositionLeft);
            WorldItemTemp.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
            WorldItemTemp.transform.localScale = Vector3.one;

            Destroy(seat.foodRight.gameObject);
            seat.foodRight = null;
            seat.foodRight = WorldItemTemp;
        }
    }

    private ItemData GetUsedToolItem(ERequiredTool type)
    {
        ItemData temp = itemDatas.items[0];
        for(int i = 0; i < itemDatas.items.Count; i++)
        {
            temp = itemDatas.items[i];
            if(type == ERequiredTool.Plate && temp.itemName == "Plate")
            {
                return temp;
            }
            else if(type == ERequiredTool.Bowl && temp.itemName == "Bowl")
            {
                return temp;
            }
            else if(type == ERequiredTool.Cup && temp.itemName == "Cup")
            {
                return temp;
            }
        }

        return temp;
    }
}
