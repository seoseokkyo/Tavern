using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class CustomerScript : Interactable
{
    public GameObject selfObj;
    private CustomerAnim animScript;

    List<ItemData> orderItems = new List<ItemData>();
    public GameObject menuObj;
    private MenuManager menuManager;

    public GameObject orderUIObject;
    private OrderCanvasScript_TestSSK orderUI;

    private float time = 0f;
    public bool findSeat = false;
    public bool isOrdered = false;
    private bool getOrdered = false;

    TableScript table;
    SeatData seat;

    public Transform startLoc;

    public override string GetInteractingDescription()
    {
        return "Press [E] to Serve";
    }

    public override void Interact()
    {
        if(interactPlayer != null && interactPlayer.CurrentEquipmentItem != null)
        {
            ItemBase equip = interactPlayer.CurrentEquipmentItem;
            if (CheckOrder(equip.CurrentItemData))
            {
                interactPlayer.CurrentEquipmentItem.CurrentItemData.itemCount -= 1;
            }
            else
            {
                Debug.Log("WHAT?");
            }
        }
    }

    void Start()
    {
        menuManager = GameObject.FindWithTag("MenuManager").GetComponent<MenuManager>();
        animScript = GetComponent<CustomerAnim>();
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
        if(getOrdered == true)
        {
            time += Time.deltaTime;
            if(time > 3)
            {
                getOrdered = false;
                time = 0;
                Leave();
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
                animScript.MoveToLocation(seat.chair.transform);
                //selfObj.transform.position = seat.chair.transform.position;
                //Initialize();
                //DecideOrder();
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

    public void Initialize()
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

    public void DecideOrder()
     {
        orderItems.Clear();

        List<ItemData> menu = GetMenuFromManager();
        if(menu != null)
        {
            int maxOrderCount = Random.Range(1, 2);
            while (true)
            {
                int random = Random.Range(0, menu.Count);
                ItemData temp = menu[random];
                orderItems.Add(temp);
                orderUI.SetOrderUI(orderItems);
                maxOrderCount--;

                if (maxOrderCount == 0)
                    break;
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
                animScript.Check(true);

                if (orderItems.Count == 0)
                {
                    getOrdered = true;
                    animScript.Eat();
                   // Leave();
                    return true;

                }
                return true;
            }
        }
        animScript.Check(false);
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
        //selfObj.transform.position = startLoc.transform.position;
        animScript.Leave();
        table.ReleaseSeat(seat);
    }

}
