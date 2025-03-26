using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class CustomerScript : Interactable, IPunObservable
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

    private PhotonView photonView;

    private void Awake()
    {
        photonView = GetComponent<PhotonView>();

    }

    void Spawn()
    {
        GameObject obj = PhotonNetwork.Instantiate("Customer1", startLoc.position, Quaternion.identity);
        CustomerScript customer= obj.GetComponent<CustomerScript>();
        customer.startLoc = startLoc;
    }

    public override string GetInteractingDescription()
    {
        return "Press [E] to Serve";
    }

    public override void Interact()
    {
        if (interactPlayer != null && interactPlayer.CurrentEquipmentItem != null)
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
        photonView = GetComponent<PhotonView>();
        menuManager = GameObject.FindWithTag("MenuManager").GetComponent<MenuManager>();
        animScript = GetComponent<CustomerAnim>();
        openCloseButton = GameObject.FindWithTag("Store").GetComponent<Interact_OpenCloseButton>();
    }

    void Update()
    {
        if (openCloseButton.isOpend == true)
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
        if (!PhotonNetwork.IsMasterClient) return;

        table = TableManager.instance.FindRandomAvailableTable();
        if (table != null)
        {
            seat = table.GetAvailableSeat();
            if (seat != null)
            {
                int tableID = TableManager.instance.GetTableID(table);
                photonView.RPC("MoveToSeat", RpcTarget.All, tableID, seat.seatID);
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

    [PunRPC]
    void MoveToSeat(int table, int seat)
    {
        TableScript targetTable = TableManager.instance.GetTableByID(table);
        SeatData targetSeat = targetTable.GetSeatByID(seat);
        if (targetSeat != null)
        {
            animScript.MoveToLocation(targetSeat.chair.transform);
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
        if (orderUI != null)
        {
            orderUI.enabled = true;
        }
    }

    private List<ItemData> GetMenuFromManager()
    {
        if (menuManager != null)
        {
            return menuManager.GetMenuList();
        }

        return null;
    }

    public void DecideOrder()
    {
        if (!PhotonNetwork.IsMasterClient) return;

        orderItems.Clear();

        List<ItemData> menu = GetMenuFromManager();
        if (menu != null)
        {
            int maxOrderCount = Random.Range(1, 3);
            List<string> itemNames = new List<string>();
            while (maxOrderCount > 0)
            {
                int random = Random.Range(0, menu.Count);
                ItemData temp = menu[random];
                orderItems.Add(temp);
                itemNames.Add(temp.itemName);
                orderUI.SetOrderUI(orderItems);
                maxOrderCount--;
            }

            photonView.RPC("SyncOrder", RpcTarget.Others, itemNames.ToArray());

        }
    }

    [PunRPC]
    void SyncOrder(string[] orderIDs)
    {
        orderItems.Clear();

        foreach (string item in orderIDs)
        {
            ItemData cur = ItemManager.Instance.GetItemDataByName(item);
            orderItems.Add(cur);
        }
    }

    public bool CheckOrder(ItemData food)
    {
        if (!PhotonNetwork.IsMasterClient) return false;

        foreach (ItemData cur in orderItems)
        {
            if (cur.itemID == food.itemID)
            {
                photonView.RPC("RemoveOrder", RpcTarget.All, cur.itemID);
                table.SetFood(food, seat);
                return true;
            }
        }
        return false;
    }

    [PunRPC]
    public void RemoveOrder(int food)
    {
        /*
               foreach (ItemData cur in orderItems)
               {
                   if (cur.itemID == food.itemID)
                   {
                       orderItems.Remove(cur);
                       orderUI.RemoveOrderUI(cur);
                       break;
                   }
               }
               */
        ItemData target = orderItems.Find(item => item.itemID == food);

        orderItems.RemoveAll(item => item.itemID == food);
        orderUI.RemoveOrderUI(orderItems.Find(item => item.itemID == food));
        orderItems.Remove(target);
        orderUI.RemoveOrderUI(target);
    }
    public void Leave()
    {
        if (!PhotonNetwork.IsMasterClient) return;

        photonView.RPC("LeaveRPC", RpcTarget.All);
    }

    [PunRPC]
    void LeaveRPC()
    {
        animScript.Leave();
        table.ReleaseSeat(seat);
    }

    private System.Collections.IEnumerator FindTableTimer()
    {
        while (GetTime() < 3f)
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

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(findSeat);
            stream.SendNext(isOrdered);
        }
        else
        {
            findSeat = (bool)stream.ReceiveNext();
            isOrdered = (bool)stream.ReceiveNext();
        }
    }
}