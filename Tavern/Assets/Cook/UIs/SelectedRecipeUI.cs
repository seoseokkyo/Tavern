using NUnit.Framework.Interfaces;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SelectedRecipeUI : MonoBehaviour
{

    public Button cookButton;

    public Transform contentTransform;

    public List<IngredientAmount> selectedRecipe;
    public GameObject ingredientView;

    public Texture2D emptyIcon;

    public int ingredientListSize = 4;

    List<GameObject> ingredientViewList = new List<GameObject>();

    public GameObject resultItemView;
    public ItemDatas itemDatas;

    public GameObject player;
    private PlayerController controller;
    InventoryComp inventory;

    private bool canCook = false;
    private bool foodSelected = false;

    void Start()
    {
        //    if (player != null)
        //   {
        //      controller = player.GetComponent<PlayerController>();
        //     inventory = controller.PlayerInventory;
        //    cookButton.onClick.AddListener(OnClickCookButton);
        // }
        OnSelect("WaterMelon", controller.PlayerInventory);
    }

    void Update()
    {
        if(foodSelected == true)
        {
            CanCook();
        }
    }

    public void GetInventoryFromController(PlayerController pc)
    {
        if(pc != null)
        {
            controller = pc;
            inventory = controller.PlayerInventory;
            cookButton.onClick.AddListener(OnClickCookButton);
        }
    }

    void ClearIngredientList()
    {
        foodSelected = false;
        for (int i = ingredientViewList.Count - 1; i >= 0; i--)
        {
            GameObject temp = ingredientViewList[i];
            ingredientViewList.RemoveAt(i);
            Destroy(temp);
        }

        int iCount = contentTransform.childCount;
        for (int i = iCount - 1; i >= 0; i--)
        {
            var temp = contentTransform.GetChild(i);

            if (temp != null)
            {
                var recipeUITemp = temp.GetComponent<ItemUI>();
                if (recipeUITemp != null)
                {
                    Destroy(temp.gameObject);
                }
            }
        }
    }

    public void SetIngredientList(List<IngredientAmount> ingds)
    {
        selectedRecipe = ingds;
        if(selectedRecipe != null)
        {
            for(int i = 0; i < selectedRecipe.Count; i++)
            {
                IngredientAmount current = selectedRecipe[i];
                // check itemdatas
                for (int j = 0; j < itemDatas.items.Count; j++)
                {
                    ItemData currentItem = itemDatas.items[j];
                    if (current.itemName == currentItem.itemName)
                    {
                        GameObject prefab = Instantiate(ingredientView);
                        prefab.transform.SetParent(contentTransform, false);
                        ItemUI tempIngredientView = prefab.GetComponent<ItemUI>();
                        if (tempIngredientView != null)
                        {
//                            Debug.Log($"Setting Recipe UI for {currentItem.itemName}");
                            tempIngredientView.InitData(currentItem.itemIcon, contentTransform);
                        }
                        else
                        {
                            tempIngredientView.InitData(emptyIcon, contentTransform);
                        }

                        prefab.SetActive(true);
                        ingredientViewList.Add(prefab);
                    }
                }
            }
        }
    }

    public void OnSelect(string result, InventoryComp playerInventory)
    {
        inventory = playerInventory;

        ClearIngredientList();

        // recipeUI 
        for(int i = 0; i < itemDatas.items.Count; i++)
        {
            ItemData temp = itemDatas.items[i];

            if(temp.recipe != null && temp.itemName == result)
            {
                GameObject prefab = Instantiate(resultItemView);
                RecipeUI tempRecipeUI = prefab.GetComponent<RecipeUI>();

                if (tempRecipeUI != null)
                {
                    // 완성품 ICON 띄우기
                    tempRecipeUI.AddItemData(temp);
                    // ingredientsList 전달
                    SetIngredientList(temp.recipe.ingredients);
                    foodSelected = true;
                    break;
                }
                else
                {
                    Debug.Log("SELECTED RECIPE LOADING ERROR");
                    foodSelected = false;
                    break;
                }
            }
        }
    }

    public int CanCook()
    {
        foreach (IngredientAmount idg in selectedRecipe)
        {
            // 플레이어 인벤토리 확인
            for (int i = 0; i < inventory.GetInventorySize(); i++)
            {
                ItemBase currentItem = inventory.CheckItem(i);
                // 재료 발견
                if (currentItem != null && currentItem.CurrentItemData.itemName == idg.itemName)
                {
                    // 수량 확인
                    // ..
                    // 플레이어 Inventory 재료 위치한 idx 반환
                    canCook = true;
                    return i;
                }
            }
        }

        canCook = false;
        return -1;
    }
    
    // 필요한 재료 리스트 UI 에서 재료가 플레이어 인벤토리에 없으면 조금 칙칙하게 표현하기 위한 기능 부분
    private void CheckHasIngredients(int ingredientsListIdx)
    {

    }

    public void Cook(int igIdx)
    {
        string strTemp = "";
        strTemp = $"{inventory.CheckItem(CanCook()).CurrentItemData.itemName}, ";
        inventory.PopItem(CanCook());
        Debug.Log(strTemp);

    }

    void OnClickCookButton()
    {
        if(canCook)
        {
            Cook(CanCook());
        }
    }
}
