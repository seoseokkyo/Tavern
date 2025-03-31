using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using UnityEngine.UI;
using TMPro;

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


    private bool waitingForInteraction = false;
    private bool interacted = false;

    private float waitTimer = 0f;
    [SerializeField] private float waitTimeLimit = 10f;
    [SerializeField] private float waitServeLimit = 60f;


    private UnityEngine.UI.Slider waitingTimerSlider;
    private TextMeshProUGUI customerStateText;
    
    
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
        if (interactPlayer != null && interactPlayer.CurrentEquipmentItem != null && isOrdered && !getOrdered)
        {
            ItemBase equip = interactPlayer.CurrentEquipmentItem;
            if (CheckOrder(equip.CurrentItemData))
            {
                animScript.Check(true);
                photonView.RPC("OnServingSuccess", RpcTarget.AllBuffered);
                if (orderItems.Count == 0)
                {
                    getOrdered = true;
                }
            }
            else
            {
                animScript.Check(false);
                photonView.RPC("OnServingFailed", RpcTarget.AllBuffered);
            }
        }

        if (waitingForInteraction && !interacted)
        {
            photonView.RPC("OnInteractedRPC", RpcTarget.MasterClient);
            return;
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
                photonView.RPC("MoveToSeat", RpcTarget.AllBuffered, tableID, seat.seatID);
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
        SetStateText("Moving to Table");
        TableScript targetTable = TableManager.instance.GetTableByID(table);
        SeatData targetSeat = targetTable.GetSeatByID(seat);
        if (targetSeat != null)
        {
            animScript.MoveToLocation(targetSeat.chair.transform);
        }
    }

    public void SetStateText(string state)
    {
        photonView.RPC("UpdateCustomerText", RpcTarget.AllBuffered, state);
    }

    public IEnumerator WaitForInteraction()
    {
        photonView.RPC("ShowWaitingUI", RpcTarget.AllBuffered);

        waitingForInteraction = true;
        interacted = false;

        waitTimer = 0f;

        while (waitTimer < waitTimeLimit)
        {
            if (interacted)
            {
                waitingForInteraction = false;
                photonView.RPC("HideWaitingUI", RpcTarget.AllBuffered);
                photonView.RPC("OnInteractionSuccess", RpcTarget.AllBuffered);
                yield break;
            }

            waitTimer += Time.deltaTime;

            float ratio = waitTimer / waitTimeLimit;
            photonView.RPC("UpdateWaitingSlider", RpcTarget.AllBuffered, ratio);

            yield return null;
        }

        waitingForInteraction = false;
        photonView.RPC("HideWaitingUI", RpcTarget.AllBuffered);
        photonView.RPC("OnInteractionFailed", RpcTarget.AllBuffered);
    }

    public IEnumerator WaitForServe()
    {
        isOrdered = true;
        getOrdered = false;

        waitTimer = 0f;

        while (waitTimer < waitServeLimit)
        {
            if(getOrdered)
            {
                ResetTimer();
                photonView.RPC("HideWaitingUI", RpcTarget.AllBuffered);
                SetStateText("Thanks! bye");
                StartCoroutine(LeaveTimer());

                yield break;
            }

            waitTimer += Time.deltaTime;

            float ratio = waitTimer / waitServeLimit;
            photonView.RPC("UpdateWaitingSlider", RpcTarget.AllBuffered, ratio);

            yield return null;
        }

        waitingForInteraction = false;
        photonView.RPC("HideWaitingUI", RpcTarget.AllBuffered);
        photonView.RPC("OnInteractionFailed", RpcTarget.AllBuffered);
    }

    [PunRPC]
    void OnInteractedRPC()
    {
        if (!PhotonNetwork.IsMasterClient) return;

        interacted = true;
    }

    [PunRPC]
    void OnInteractionSuccess()
    {
        if(photonView.IsMine && PhotonNetwork.IsMasterClient)
        {
            DecideOrder();
        }
    }

    [PunRPC]
    void OnServingSuccess()
    {
        // 별점, 가격 측정

    }

    [PunRPC]
    void OnServingFailed()
    {
        // 별점, 가격 측정
    }

    [PunRPC]
    void OnInteractionFailed()
    {
        // 별점, 가격 측정

        photonView.RPC("UpdateCustomerText", RpcTarget.AllBuffered, "what the FAQ");
        Leave();
    }

    [PunRPC]
    void HideWaitingUI()
    {
        if (waitingTimerSlider != null)
            waitingTimerSlider.gameObject.SetActive(false);
    }

    [PunRPC]
    void ShowWaitingUI()
    {
        if (waitingTimerSlider != null)
            waitingTimerSlider.gameObject.SetActive(true);
    }

    [PunRPC]
    void UpdateWaitingSlider(float ratio)
    {
        if (waitingTimerSlider != null)
            waitingTimerSlider.value = ratio;
    }

    [PunRPC]
    void UpdateCustomerText(string text)
    {
        if (customerStateText != null)
        {
            customerStateText.text = text;
            customerStateText.gameObject.SetActive(true);
        }
    }

    public void Initialize()
    {
        photonView.RPC("InitializeRPC", RpcTarget.AllBuffered);
    }

    [PunRPC]
    void InitializeRPC()
    {
        menuManager = GameObject.FindWithTag("MenuManager")?.GetComponent<MenuManager>();
        orderUI = orderUIObject?.GetComponent<OrderCanvasScript_TestSSK>();
        
        if (orderUI != null)
        {
            orderUI.enabled = true;

            waitingTimerSlider = orderUI.waitingTimerSlider;
            customerStateText = orderUI.customerStateText;

            if (waitingTimerSlider != null)
            {
                waitingTimerSlider.maxValue = 1f;
                waitingTimerSlider.value = 0f;
                waitingTimerSlider.gameObject.SetActive(false);
            }
            else
            {
                Debug.Log("WatingTimerSlider is NULL");
            }

            if (customerStateText != null)
            {
                customerStateText.text = "";
                customerStateText.gameObject.SetActive(true);
            }
            else
            {
                Debug.Log("customerStateText is NULL");
            }
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
                maxOrderCount--;
            }

            if(itemNames.Count > 1)
            {
                SetStateText("It's my order");
                photonView.RPC("SyncOrder_Double", RpcTarget.AllBuffered, itemNames.ToArray());
            }
            else 
            {
                string name = itemNames[0];
                SetStateText("It's my order");
                photonView.RPC("SyncOrder_Single", RpcTarget.AllBuffered, name);
            }

        }
    }

    [PunRPC]
    void SyncOrder_Single(string orderIDs)
    {
        orderItems.Clear();

        ItemData cur = ItemManager.Instance.GetItemDataByName(orderIDs);
        orderItems.Add(cur);

        if (orderUI != null)
        {
            orderUI.SetOrderUI(orderItems);
        }

        if (photonView.IsMine && PhotonNetwork.IsMasterClient)
        {
            photonView.RPC("ShowWaitingUI", RpcTarget.AllBuffered);
            StartCoroutine(WaitForServe());
        }
    }

    [PunRPC]
    void SyncOrder_Double(string[] orderIDs)
    {
        orderItems.Clear();
        foreach (string item in orderIDs)
        {
            ItemData cur = ItemManager.Instance.GetItemDataByName(item);
            orderItems.Add(cur);
        }

        if (orderUI != null)
        {
            orderUI.SetOrderUI(orderItems);
        }

        if(photonView.IsMine && PhotonNetwork.IsMasterClient)
        {
            photonView.RPC("ShowWaitingUI", RpcTarget.AllBuffered);
            StartCoroutine(WaitForServe());
        }
    }

    public bool CheckOrder(ItemData food)
    {
        if (!PhotonNetwork.IsMasterClient) return false;

        foreach (ItemData cur in orderItems)
        {
            if (cur.itemID == food.itemID)
            {
                photonView.RPC("RemoveOrder", RpcTarget.AllBuffered, cur.itemID);
                table.SetFood(food, seat);
                return true;
            }
        }
        return false;
    }

    [PunRPC]
    public void RemoveOrder(int food)
    {
        ItemData target = orderItems.Find(item => item.itemID == food);
        orderItems.Remove(target);
        orderUI.RemoveOrderUI(target);
    }
    public void Leave()
    {
        if (!PhotonNetwork.IsMasterClient) return;

        photonView.RPC("LeaveRPC", RpcTarget.AllBuffered);
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