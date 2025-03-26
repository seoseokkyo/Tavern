using NUnit.Framework.Interfaces;
using System.Collections.Generic;
using TMPro;
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

    private ItemData currentRecipe;
    private ItemBase currentResultItem;

    public float cookingTime;
    public UnityEngine.UI.Slider cookingTimerSlider;
    public TextMeshProUGUI cookingStatus;

    public GameObject cookingPotObj;

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
        if (foodSelected == true)
        {
            CanCook();
            // 버튼 활성화
            if(canCook == true)
            {
                cookButton.interactable = true;
            }
            else
            {
                cookButton.interactable = false;
            }
        }
    }

    public void GetInventoryFromController(PlayerController pc)
    {
        if(pc != null)
        {
            controller = pc;
            //inventory = controller.PlayerInventory;
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
        int amount = 0;
        if (inventory != null)
        {
            for(int i = 0; i < inventory.GetInventorySize(); i++)
            {
                ItemBase temp = inventory.CheckItem(i);
                if(temp != null && temp.CurrentItemData.itemName == item.itemName)
                {
                    amount += temp.CurrentItemData.itemCount;
                }
            }
        }

        return amount;
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
                            // 플레이어 인벤토리에 해당 재료가 있는지 확인 후 전달
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

    private void SaveResultItemData(string result)
    {
        for(int i = 0; i < itemDatas.items.Count; i++)
        {

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
                currentRecipe = temp;
                GameObject prefab = Instantiate(resultItemView);
                RecipeUI tempRecipeUI = prefab.GetComponent<RecipeUI>();
                prefab.transform.SetParent(resultTransform, false);
                prefab.transform.SetLocalPositionAndRotation(new Vector3(0, 0), new Quaternion(0, 0, 0, 0));
                prefab.transform.localScale = new Vector3(0.7f, 0.6f, 0.6f);
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
            for(int i = 0; i < inventory.GetInventorySize(); i++)
            {
                ItemBase temp = inventory.CheckItem(i);
                if (temp != null && temp.CurrentItemData.itemName == idg.itemName)
                {
                    // 인벤토리 모든 슬롯에서 같은 아이템 수량 확인
                    int havingCount = CheckIngredientFromPlayerInventory(temp.CurrentItemData);
                    if(idg.amount - havingCount <= 0)
                    {
                        canCook = true;
                        return i;
                    }
                }
            }
        }

        canCook = false;
        return -1;
    }
    
    public ItemData FindData(string name)
    {
        for(int i = 0; i < itemDatas.items.Count; i++)
        {
            if (itemDatas.items[i].itemName == name)
            {
                return itemDatas.items[i];
            }
        }

        // 아무거나 반환..
        return itemDatas.items[0];
    }

    public void Cook()
    {
        if (cookingStatus != null)
        {
            cookingStatus.text = "cooking..";
            cookingStatus.enabled = true;
        }

        foreach (IngredientAmount idg in selectedRecipe)
        {
            int reqAmount = idg.amount;

            while(reqAmount > 0)
            {
                for (int i = 0; i < inventory.GetInventorySize(); i++)
                {
                    ItemBase temp = inventory.CheckItem(i);
                    if (temp != null && temp.CurrentItemData.itemName == idg.itemName)
                    {
                        // 인벤토리 모든 슬롯에서 같은 아이템 수량 확인
                        int havingCount = temp.CurrentItemData.itemCount;
                        if (reqAmount - havingCount < 0)
                        {
                            ItemBase backup = temp;
                            backup.CurrentItemData.itemCount -= havingCount - reqAmount;
                            reqAmount -= havingCount;
                            // 갯수 차감한 아이템으로 갈아끼워줌
                            inventory.PopItem(i);
                            inventory.AddItem(ref backup);
                        }
                        else
                        {
                            reqAmount -= havingCount;
                            inventory.PopItem(i);
                            break;
                        }
                    }
                }
            }
        }

        // 레시피 재료 상태 업데이트
        OnSelect(currentRecipe.itemName, inventory);
        // 타이머 Start
        StartTimer();
    }

    private void StartTimer()
    {
        cookButton.interactable = false;
        canCook = false;
        foodSelected = false;
        if (cookingTimerSlider != null)
        {
            ResetTimer();
            cookingTimerSlider.value = GetCookingTime();
            StartCoroutine(CookingTimerCoroutine());
        }
    }

    private System.Collections.IEnumerator CookingTimerCoroutine()
    {
        while(GetCookingTime() < currentRecipe.recipe.cookingTime)
        {
            IncreaseTimer();
            cookingTimerSlider.value = GetCookingTime() / currentRecipe.recipe.cookingTime;
            yield return null;
        }

        ResetTimer();
        string result = $"{currentRecipe.itemName} is Cooked! ";
        Debug.Log(result);
        cookingStatus.text = "Done!";
        cookingTimerSlider.value = GetCookingTime() / currentRecipe.recipe.cookingTime;

        Interact_CookPot pot = cookingPotObj.GetComponent<Interact_CookPot>();
        pot.SetSettingCooked(currentRecipe,5);
    }

    private void ResetTimer() => cookingTime = 0f;
    private float GetCookingTime() => cookingTime;
    private void IncreaseTimer() => cookingTime += Time.deltaTime;

    void OnClickCookButton()
    {
        if(canCook)
        {
            Cook();
        }
    }
}
