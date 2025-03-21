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
    private Interact_OpenCloseButton openCloseButton;

    bool isVisited = false;

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
                animScript.Check(true);
                interactPlayer.CurrentEquipmentItem.CurrentItemData.itemCount -= 1;

                if (orderItems.Count == 0)
                {
                    ResetTimer();
                    StartCoroutine(LeaveTimer());
                }
            }
            else
            {
                animScript.Check(false);
            }
        }
    }

    void Start()
    {
        menuManager = GameObject.FindWithTag("MenuManager").GetComponent<MenuManager>();
        animScript = GetComponent<CustomerAnim>();
        openCloseButton = GameObject.FindWithTag("Store").GetComponent<Interact_OpenCloseButton>();
    }

    void Update()
    {
        if(openCloseButton.isOpend == true)
        {
            if (!isVisited)
            {
                isVisited = true;
                StartCoroutine(FindTableTimer());
            }
        }
    }

    void checkTable()
    {
        table = TableManager.instance.FindRandomAvailableTable();
        if(table != null)
        {
            seat = table.GetAvailableSeat();
            if (seat != null)
            {
                animScript.MoveToLocation(seat.chair.transform);
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
        animScript.Leave();
        table.ReleaseSeat(seat);
    }

    private System.Collections.IEnumerator FindTableTimer()
    {
        while(GetTime() < 3f)
        {
            IncreaseTimer();
            yield return null;
        }

        ResetTimer();
        checkTable();
    }

    private System.Collections.IEnumerator LeaveTimer()
    {
        while (GetTime() < 2f)
        {
            IncreaseTimer();
            yield return null;
        }

        ResetTimer();
        Leave();
    }

    private void ResetTimer() => time = 0f;
    private float GetTime() => time;
    private void IncreaseTimer() => time += Time.deltaTime;
}
