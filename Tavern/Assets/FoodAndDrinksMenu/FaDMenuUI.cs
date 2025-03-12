using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;


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

    List<GameObject> selectedFoodList = new List<GameObject>();
    List<GameObject> selectedDrinkList = new List<GameObject>();
    void Start()
    {
        foodViewList.Clear();
        drinkViewList.Clear();
        selectedFoodList.Clear();
        selectedDrinkList.Clear();

    }
    void Update()
    {

    }

    public void SetMenuList()
    {
        ClearList();
        SetFoodList();
        SetDrinkList();
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
                }
                prefab.SetActive(true);
                drinkViewList.Add(prefab);
            }
        }
    }
}
