using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UI;


public class FaDMenuUI : MonoBehaviour
{
    public GameObject player;

    public ItemDatas itemDatas;

    public GameObject foodMenuUI;
    public Transform foodMenuContentTransform;
    List<GameObject> foodViewList = new List<GameObject>();

    public GameObject drinkMenuUI;
    public Transform drinkMenuContentTransform;
    List<GameObject> drinkViewList = new List<GameObject>();

    public GameObject menuManager;
    private MenuManager menuManagerScript;

    public UnityEngine.UI.Button SetMenuButton;
    private List<ItemData> savedMenuList = new List<ItemData>();

    public ModeController modeController;
    public ClickEventTest clickEventTestScript;

    void Start()
    {
        foodViewList.Clear();
        drinkViewList.Clear();
        SetMenuButton.onClick.AddListener(OnSetMenuButtonClick);
    }
    void Update()
    {

    }

    void OnSetMenuButtonClick()
    {
        if (menuManagerScript != null)
        {
            savedMenuList = menuManagerScript.GetMenuList();
            if(modeController != null & clickEventTestScript != null)
            {
                modeController.SetMode(false);
                clickEventTestScript.SetUIActivated(false);
                clickEventTestScript.popUI.SetActive(false);
            }
        }
    }

    public void SetMenuList()
    {
        ClearList();
        SetFoodList();
        SetDrinkList();
        SetMenuManager();
    }

    private void GetSavedMenuFromManager()
    {
        savedMenuList = menuManagerScript.GetMenuList();
    }

    private void SetMenuManager()
    {
        menuManagerScript = menuManager.GetComponent<MenuManager>();
        if(menuManagerScript != null)
        {
            // foodS
            for(int i = 0; i < foodMenuContentTransform.childCount; i++)
            {
                var temp = foodMenuContentTransform.GetChild(i);
                if(temp != null)
                {
                    var tempUI = temp.GetComponent<MenuPanelUI>();
                    if(tempUI != null)
                    {
                        tempUI.menuManagerScript = menuManagerScript;
                    }
                }
            }
            // drinks
            for (int i = 0; i < drinkMenuContentTransform.childCount; i++)
            {
                var temp = foodMenuContentTransform.GetChild(i);
                if (temp != null)
                {
                    var tempUI = temp.GetComponent<MenuPanelUI>();
                    if (tempUI != null)
                    {
                        tempUI.menuManagerScript = menuManagerScript;
                    }
                }
            }
        }

        SetSavedMenuList();
    }

    private void SetSavedMenuList()
    {
        GetSavedMenuFromManager();
        if(savedMenuList.Count == 0)
           return;

        // foods
        for(int i = 0; i < foodMenuContentTransform.childCount; i++)
        {
            var temp = foodMenuContentTransform.GetChild(i);
            if(temp != null)
            {
                var item = temp.GetComponent<MenuPanelUI>();
                ItemData itemData = item.GetCurrentItem();
                for (int j = 0; j < savedMenuList.Count; j++)
                {
                    var menuTemp = savedMenuList[j];
                    if(itemData.itemName == menuTemp.itemName)
                    {
                        item.SetIsSelectedBefore();
                        break;
                    }
                }
            }
        }
        // drinks
        for (int i = 0; i < drinkMenuContentTransform.childCount; i++)
        {
            var temp = drinkMenuContentTransform.GetChild(i);
            if (temp != null)
            {
                var item = temp.GetComponent<MenuPanelUI>();
                ItemData itemData = item.GetCurrentItem();
                for (int j = 0; j < savedMenuList.Count; j++)
                {
                    var menuTemp = savedMenuList[j];
                    if (itemData.itemName == menuTemp.itemName)
                    {
                        item.SetIsSelectedBefore();
                        break;
                    }
                }
            }
        }
    }

    private void ClearList()
    {
        // food
        int fCount = foodMenuContentTransform.childCount;
        for(int i = fCount - 1; i >= 0; i--)
        {
            var temp = foodMenuContentTransform.GetChild(i);
            if (temp != null)
            {
                var foodTemp = temp.GetComponent<MenuPanelUI>();
                if (foodTemp != null)
                {
                    Destroy(temp.gameObject);
                }
            }
        }

        for (int i = foodViewList.Count - 1; i >= 0; i--)
        {
            GameObject temp = foodViewList[i];
            foodViewList.RemoveAt(i);
            Destroy(temp);
        }

        // drink
        int dCount = drinkMenuContentTransform.childCount;
        for (int i = dCount - 1; i >= 0; i--)
        {
            var temp = drinkMenuContentTransform.GetChild(i);
            if (temp != null)
            {
                var drinkTemp = temp.GetComponent<MenuPanelUI>();
                if (drinkTemp != null)
                {
                    Destroy(temp.gameObject);
                }
            }
        }

        for (int i = drinkViewList.Count - 1; i >= 0; i--)
        {
            GameObject temp = drinkViewList[i];
            drinkViewList.RemoveAt(i);
            Destroy(temp);
        }
    }

    private void SetFoodList()
    {
        foreach(CreateRecipe item in itemDatas.createRecipes)
        {
            if(item.CreateItemData.CreateItemType == CreateItemType.Cooking)
            {
                string name = item.CreateItemData.ItemName;
                GameObject prefab = Instantiate(foodMenuUI);
                prefab.transform.SetParent(foodMenuContentTransform, false);
                MenuPanelUI tempMenuUI = prefab.GetComponent<MenuPanelUI>();
                if(tempMenuUI != null)
                {
                    tempMenuUI.SetMenuInfo(name);
                    tempMenuUI.isSelected = false;
                }
                prefab.SetActive(true);
                foodViewList.Add(prefab);
            }
        }
    }

    private void SetDrinkList()
    {
        foreach (CreateRecipe item in itemDatas.createRecipes)
        {
            if (item.CreateItemData.CreateItemType == CreateItemType.Brewing)
            {
                string name = item.CreateItemData.ItemName;
                GameObject prefab = Instantiate(foodMenuUI);
                prefab.transform.SetParent(drinkMenuContentTransform, false);
                MenuPanelUI tempMenuUI = prefab.GetComponent<MenuPanelUI>();
                if (tempMenuUI != null)
                {
                    tempMenuUI.SetMenuInfo(name);
                    tempMenuUI.isSelected = false;
                }
                prefab.SetActive(true);
                drinkViewList.Add(prefab);
            }
        }
    }
}
