using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using System.Collections.Generic;

public class CreatingMemoUI : MonoBehaviourPunCallbacks
{
    public Transform foodsContentTransform;
    public TMP_InputField extraNotesInput;
    public Button setButton;
    public Button cancelButton;

    private List<GameObject> foods = new List<GameObject>();
    public GameObject food;
    public ItemDatas itemDatas;

    private PlayerController player;
    private ModeController modeController;

    public Transform spawnLoc;

    public void Init(PlayerController owner)
    {
        player = owner;
        modeController = owner.GetComponent<ModeController>();

        SetFoodList();

        setButton.onClick.AddListener(OnSetButtonClicked);
        cancelButton.onClick.AddListener(OnCancelButtonClicked);

        if(modeController != null)
        {
            modeController.SetMode(true);
        }
    }

    private void SetFoodList()
    {
        foods.Clear();

        foreach (CreateRecipe item in itemDatas.createRecipes)
        {
            if (item.CreateItemData.CreateItemType == CreateItemType.Cooking)
            {
                string name = item.CreateItemData.ItemName;
                GameObject prefab = Instantiate(food);
                if (prefab != null)
                {
                    prefab.transform.SetParent(foodsContentTransform, false);
                    FoodSelect tempUI = prefab.GetComponent<FoodSelect>();
                    if (tempUI != null)
                    {
                        ItemData tempData = FindItemData(name);
                        tempUI.Initialize(tempData);
                        tempUI.isSelected = false;
                    }
                    prefab.SetActive(true);
                    foods.Add(prefab);
                }
            }
        }
    }

    private ItemData FindItemData(string name)
    {
        foreach (ItemData temp in itemDatas.items)
        {
            if (temp.itemName == name)
            {
                return temp;
            }
        }
        return itemDatas.items[0];
    }

    void Start()
    {

    }

    void OnSetButtonClicked()
    {
        Debug.Log("SetButtonClicked");

        List<string> foodNames = new List<string>();
        foreach(GameObject cur in foods)
        {
            FoodSelect f = cur.GetComponent<FoodSelect>();
            if(f != null)
            {
                if(f.isSelected)
                {
                    foodNames.Add(f.name);
                    Debug.Log("SelectedFood is Set");
                }
            }
        }
        string extraNote = extraNotesInput.text;
        Debug.Log("Call RPC");
        photonView.RPC("RPC_CreateMemoItem", RpcTarget.AllBuffered, foodNames.ToArray(), extraNote);

        CloseUI();
    }

    [PunRPC]
    void RPC_CreateMemoItem(string[] foods, string extra)
    {
        Debug.Log("Called CreateMemoItem RPC");

        List<FoodSelect> selected = new List<FoodSelect>();
        foreach (string f in foods)
        {
            ItemData curData = FindItemData(f);
            FoodSelect cur = new FoodSelect();
            cur.Initialize(curData);
            selected.Add(cur);
        }
        
        GameObject memoItem = Resources.Load<GameObject>("Memo/Memo");
        if (memoItem == null)
        {
            Debug.LogError("Memo prefab is null");
            return; 
        }

        GameObject instance = Instantiate(memoItem);
        MenoScript memo = instance.GetComponent<MenoScript>();
        if (memo == null)
        {
            Debug.LogError("MenoScript component is null");
            return;
        }
        memo.Initialize(selected, extra);
        Vector3 loc = spawnLoc.transform.up * 5;
        memo.transform.position = loc;
    }

    void OnCancelButtonClicked()
    {
        CloseUI();
    }

    void CloseUI()
    {
        if (modeController != null)
        {
            modeController.SetMode(false);
        }
        else
        {
            Debug.Log("ModeController is null");
        }

        gameObject.SetActive(false);
    }
}
