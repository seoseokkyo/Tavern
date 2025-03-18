using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class CustomerScript : MonoBehaviour
{
    public GameObject selfObj;

    List<ItemData> orderItems = new List<ItemData>();
    public GameObject menuObj;
    private MenuManager menuManager;

    public GameObject orderUIObject;
    private OrderCanvasScript_TestSSK orderUI;

    private float time = 0f;
    public bool findSeat = false;
    public bool isOrdered = false;

    TableScript table;
    SeatData seat;
    void Start()
    {
        menuManager = GameObject.FindWithTag("MenuManager").GetComponent<MenuManager>();
    }

    void Update()
    {
        
        if(findSeat == false)
        {
            time += Time.deltaTime;
            if (time > 10)
            {
                time = 0;
                findSeat = true;
            }
        }
        if (findSeat == true && isOrdered == false)
        {
            time += Time.deltaTime;
            if(time > 5)
            {
                time = 0;
                checkTable();
            }
        }
        
    }

    void checkTable()
    {
        table = TableManager.instance.FindRandomAvailableTable();
        if(table != null)
        {
            seat = table.GetAvailableSeat(); // 여기서 isSitting 체크까지 하고 옴
            if (seat != null)
            {
                selfObj.transform.position = seat.chair.transform.position;
                Initialize();
                DecideOrder();
                isOrdered = true;
            }
            else
            {
                Debug.Log("No Seat!");
            }
        }
        else
        {
            Debug.Log("No Table!");
        }
    }

    void Initialize()
    {
        menuManager = GameObject.FindWithTag("MenuManager").GetComponent<MenuManager>();
        if (menuManager != null)
        {
            orderItems.Clear();
        }
        
        orderUI = orderUIObject.GetComponent<OrderCanvasScript_TestSSK>();
        if(orderUI != null)
        {
            orderUI.enabled = true;
        }
    }

    private List<ItemData> GetMenuFromManager()
    {
        if(menuManager != null)
        {
            return menuManager.GetMenuList();
        }

        return null;
    }

    private void DecideOrder()
     {
        orderItems.Clear();

        List<ItemData> menu = GetMenuFromManager();
        if(menu != null)
        {
            int maxOrderCount = Random.Range(0, 2);
            for(int i = 0; i < maxOrderCount + 1; i++)
            {
                int random = Random.Range(0, menu.Count);
                ItemData temp = menu[random];
                orderItems.Add(temp);
                orderUI.SetOrderUI(orderItems);
            }
        }
    }

    public bool CheckOrder(ItemData food)
    {
        foreach(ItemData cur in orderItems)
        {
            if(cur.itemID == food.itemID)
            {
                RemoveOrder(cur);
                table.SetFood(food, seat);
                return true;
            }
        }

        return false;
    }

    public void RemoveOrder(ItemData food)
    {
        foreach (ItemData cur in orderItems)
        {
            if (cur.itemID == food.itemID)
            {
                orderItems.Remove(cur);
                orderUI.RemoveOrderUI(cur);
                break;
            }
        }
    }
    public void Leave()
    {
        table.RemoveFood(seat);
        table.ReleaseSeat(seat);
    }
}
