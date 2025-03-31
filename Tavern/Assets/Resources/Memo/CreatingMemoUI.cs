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
                prefab.transform.SetParent(foodsContentTransform, false);
                FoodSelect tempUI = prefab.GetComponent<FoodSelect>();
                if(tempUI != null)
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
        MemoData memoData = new MemoData();
        foreach(GameObject cur in foods)
        {
            FoodSelect f = cur.GetComponent<FoodSelect>();
            if(f != null)
            {
                if(f.isSelected)
                {
                    memoData.foods.Add(f);
                }
            }
        }
        memoData.extraNote = extraNotesInput.text;

        string json = JsonUtility.ToJson(memoData);
        photonView.RPC("RPC_CreateMemoItem", RpcTarget.AllBuffered, json);

        CloseUI();
    }

    [PunRPC]
    void RPC_CreateMemoItem(string json)
    {
        MemoData data = JsonUtility.FromJson<MemoData>(json);

        GameObject memoItem = Instantiate(Resources.Load<GameObject>("Memo"));
        MenoScript memo = memoItem.GetComponent<MenoScript>();

        if (memo != null)
        {
            memo.Initialize(data.foods, data.extraNote);
            Vector3 loc = spawnLoc.transform.up * 5;
            memo.transform.position = loc;
        }
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

        gameObject.SetActive(false);
    }
}
