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

    // SSK
    [HideInInspector]
    public MemoDummyScript MemoDummy = null; 

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
                        ItemData tempData = MemoDummy.FindItemData(name);
                        tempUI.Initialize(tempData);
                        tempUI.isSelected = false;
                    }
                    prefab.SetActive(true);
                    foods.Add(prefab);
                }
            }
        }
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
                    foodNames.Add(f.itemData.itemName);
                    Debug.Log("SelectedFood is Set");
                }
            }
        }
        string extraNote = extraNotesInput.text;

        MemoDummy.CreateMemoItem(foodNames.ToArray(), extraNote);

        CloseUI();
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
