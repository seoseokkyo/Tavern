using NUnit.Framework.Interfaces;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SelectedRecipeUI : MonoBehaviour
{

    public Button cookButton;

    public Transform contentTransform;
    public Transform resultTransform;

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
        //OnSelect("WaterMelon", controller.PlayerInventory);
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

    public int CheckIngredientFromPlayerInventory(ItemData item)
    {
        if(inventory != null)
        {
            for(int i = 0; i < inventory.GetInventorySize(); i++)
            {
                ItemBase temp = inventory.CheckItem(i);
                if(temp != null && temp.CurrentItemData.itemName == item.itemName)
                {
                    return temp.CurrentItemData.itemCount;
                }
            }
        }

        return 0;
    }

    public int CheckRequiredAmount(ItemData ingredient)
    {
        foreach (IngredientAmount idg in selectedRecipe)
        {
            if (idg.itemName == ingredient.itemName)
                return idg.amount;
        }

        return 0;
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
                        IngredientUI tempIngredientView = prefab.GetComponent<IngredientUI>();
                        if (tempIngredientView != null)
                        {
                            // �÷��̾� �κ��丮�� �ش� ��ᰡ �ִ��� Ȯ�� �� ����
                            int havingCount = CheckIngredientFromPlayerInventory(currentItem);
                            int requiredCount = CheckRequiredAmount(currentItem);
                            tempIngredientView.InitData(itemDatas.items[j], contentTransform, havingCount, requiredCount);
                        }
                        else
                        {
                            tempIngredientView.InitData(itemDatas.items[j], contentTransform, 0,0);
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
                prefab.transform.SetParent(resultTransform, false);
                prefab.transform.SetLocalPositionAndRotation(new Vector3(0, 0), new Quaternion(0, 0, 0, 0));
                prefab.transform.localScale = new Vector3(0.7f, 0.6f, 0.6f);
                if (tempRecipeUI != null)
                {
                    // �ϼ�ǰ ICON ����
                    tempRecipeUI.AddItemData(temp);

                    // ingredientsList ����
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
            // �÷��̾� �κ��丮 Ȯ��
            for (int i = 0; i < inventory.GetInventorySize(); i++)
            {
                ItemBase currentItem = inventory.CheckItem(i);
                // ��� �߰�
                if (currentItem != null && currentItem.CurrentItemData.itemName == idg.itemName)
                {
                    // ���� Ȯ��
                    // ..
                    // �÷��̾� Inventory ��� ��ġ�� idx ��ȯ
                    canCook = true;
                    return i;
                }
            }
        }

        canCook = false;
        return -1;
    }
    
    // �ʿ��� ��� ����Ʈ UI ���� ��ᰡ �÷��̾� �κ��丮�� ������ ���� ĢĢ�ϰ� ǥ���ϱ� ���� ��� �κ�
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
