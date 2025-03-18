using UnityEngine;
using System.Collections.Generic;


public class TableScript : MonoBehaviour
{
    public List<SeatData> seats = new List<SeatData>();
    public GameObject foodPrefab;
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
            GameObject obj = Instantiate(foodPrefab);
            WorldItem item = obj.GetComponent<WorldItem>();
            ItemBase tempBase = ItemBase.ItemBaseCreator.CreateItemBase(food);
            item.SetItem(tempBase);
            if(seat.foodLeft == null)
            {
                obj.transform.SetParent(seat.foodPositionLeft);
                obj.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
                obj.transform.localScale = Vector3.one;
                seat.foodLeft = obj;
            }
            else if(seat.foodLeft != null)
            {
                obj.transform.SetParent(seat.foodPositionRight);
                obj.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
                obj.transform.localScale = Vector3.one;
                seat.foodRight = obj;
            }
        }
    }

    public void RemoveFood(SeatData seat)
    {
        if(seat.foodLeft != null)
        {
            Destroy(seat.foodLeft);
            seat.foodLeft = null;
        }

        if (seat.foodRight != null)
        {
            Destroy(seat.foodRight);
            seat.foodRight= null;
        }
    }
}
