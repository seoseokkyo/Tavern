using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

public class CreateToolUI : MonoBehaviour
{
    public TMP_Dropdown RecipeDropDown;

    public Button ApplyButton;

    public Transform ImageContentTransform;

    public GameObject ProductSlot;

    public GameObject ItemUI;

    public string CurrentSelectedRecipe;

    public TMP_InputField TimerText;

    public CreateItemType ToolType = CreateItemType.Tool;

    public GameObject itemInfoUIInstance;
    public Transform itemInfoTransform;
    SelectedItemInfoUI itemInfoUI;

    void Start()
    {
        RecipeDropDown = GetComponentInChildren<TMP_Dropdown>();
        RecipeDropDown.ClearOptions();

        var CreateList = ItemManager.Instance.GetCreateRecipeAllList();

        List<TMP_Dropdown.OptionData> OptionsWithSprite = new List<TMP_Dropdown.OptionData>();

        foreach(var targetName in CreateList)
        {
            var targetData = ItemManager.Instance.GetRecipeDataByName(targetName);
            if(targetData.CreateItemType != ToolType)
            {
                continue;
            }

            var temp = new TMP_Dropdown.OptionData();
            temp.text = targetName;
            temp.image = ItemManager.Instance.GetItemSpriteByName(targetName);

            OptionsWithSprite.Add(temp);
        }

        RecipeDropDown.AddOptions(OptionsWithSprite);

        RecipeDropDown.onValueChanged.AddListener(OnDropdownValueChanged);

        OnDropdownValueChanged(0);

        ProductSlot.GetComponent<ItemSlotUI>().bNoUseDrop = true;
    }
    void OnDropdownValueChanged(int index)
    {
        CurrentSelectedRecipe = RecipeDropDown.options[index].text;
        RefreshItemView(CurrentSelectedRecipe);

        SetResultItemInfo(CurrentSelectedRecipe);
    }

    void SetResultItemInfo(string itemName)
    {
        GameObject temp = Instantiate(itemInfoUIInstance);
        itemInfoUI = temp.GetComponent<SelectedItemInfoUI>();
        if (itemInfoUI != null)
        {
            itemInfoUI.transform.SetParent(itemInfoTransform);
            itemInfoUI.transform.SetLocalPositionAndRotation(new Vector3(0, 0), new Quaternion(0, 0, 0, 0));
            ItemData data = ItemManager.Instance.GetItemDataByName(itemName);
            itemInfoUI.SetResultItem(data);
        }
    }


    void Update()
    {

    }

    void ClearList()
    {
        int iCount = ImageContentTransform.childCount;
        for (int i = iCount - 1; i >= 0; i--)
        {
            var temp = ImageContentTransform.GetChild(i);

            if (temp != null)
            {
                var ItemSlotUITemp = temp.GetComponent<ItemSlotUI>();
                if (ItemSlotUITemp != null)
                {
                    Destroy(temp.gameObject);
                }

                var ItemUITemp = temp.GetComponent<ItemUI>();

                if (ItemUITemp != null)
                {
                    Destroy(temp.gameObject);
                }
            }
        }

        if(itemInfoUI != null)
        {
            Destroy(itemInfoUI.gameObject);
        }

        //iCount = ItemViewList.Count;
        //for (int i = iCount - 1; i >= 0; i--)
        //{
        //    var ItemUITemp = ItemViewList[i];

        //    if (ItemUITemp != null && ItemUITemp != ItemDrag.beingDraggedIcon)
        //    {
        //        Destroy(ItemUITemp.gameObject);
        //    }
        //}
    }

    public void RefreshItemView(string TargetItemName)
    {
        ClearList();

        var list = ItemManager.Instance.GetCreateItemResourcesListByName(TargetItemName);

        for (int i = 0; i < list.Count; i++)
        {
            GameObject instItemUI = Instantiate(ItemUI);
            instItemUI.transform.SetParent(ImageContentTransform);
            ItemUI TempItemView = instItemUI.GetComponent<ItemUI>();

            if (TempItemView != null)
            {
                ItemData tempData = ItemManager.Instance.GetItemDataByName(list[i].ItemName);
                tempData.itemCount = list[i].NeedNumber;

                TempItemView.InitData(tempData, ImageContentTransform, i);

                foreach (var graphic in TempItemView.gameObject.GetComponentsInChildren<UnityEngine.UI.Graphic>())
                {
                    graphic.raycastTarget = false;
                }
            }
        }
    }

}
