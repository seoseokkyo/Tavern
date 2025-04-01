using System.Collections.Generic;
using System;
using UnityEngine;
using static ItemFunctions;

public enum EItemType
{
    NoUseAble,
    UseAble,
    Equipment, // 일단 퀵슬롯에서 사용하는 형태로
    EItemTypeMax
}

public enum ERequiredTool
{
    None,
    Bowl,
    Plate,
    Cup,
}


[System.Serializable]
public struct ItemData
{
    public string itemName;
    public string itemDescription;
    public Texture2D itemIcon;
    public MeshFilter itemMeshFilter;
    public MeshRenderer itemMesh;
    public int itemID;
    public int itemCount;
    public int itemCountLimit;
    public EItemType ItemType;
    public ERequiredTool requireToolType;
    public GameObject ItemPrefab;

    public CookingRecipe recipe;

    [Tooltip("아이템이 플레이어의 손에 붙을 때 사용할 Offset")]
    public Vector3 AttachLocation;
    [Tooltip("아이템이 플레이어의 손에 붙을 때 사용할 Rotation")]
    public Quaternion AttachRotation;
    [Tooltip("아이템을 사용할 때 적용되는 기능들")]
    public List<ItemFunctionParam> ItemFunctionList;
    [Tooltip("장비 아이템일 경우 내구도로 사용")]
    public float fOptionValue1;
    [Tooltip("미정")]
    public float fOptionValue2;
}

[Serializable]
public enum CreateItemType
{
    Tool,
    Cooking,
    Brewing,
    CreateItemTypeMax
}

[Serializable]
public class CreateResources
{
    public string ItemName;
    public int NeedNumber;
}

[Serializable]
public class CreateTargetRecipeData
{
    public string ItemName;
    public CreateItemType CreateItemType;
    public float CreateNeedTime;
    public int CreateNum;
}

[Serializable]
public class CreateRecipe
{
    public CreateTargetRecipeData CreateItemData;
    public List<CreateResources> Resources;
}

[System.Serializable]
public class ItemBase
{
    // 실제 아이템의 형태

    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public ItemData CurrentItemData;

    public InventoryComp OwnerInventory = null;

    /// <summary>
    ///  아이템 사용시의 효과등을 갖고있는 구조체와 리스트
    /// </summary>
    /// 
    protected List<ItemFunction> CurrentItemFunctions = new List<ItemFunction>();


    // 아이템베이스 생성자 접근수준 변경
    protected ItemBase() { }
    protected ItemBase(ItemData ItemData) { CurrentItemData = ItemData; }

    public class ItemBaseCreator
    {
        internal static ItemBase CreateItemBase(ItemData ItemData)
        {
            ItemBase NewItemBase = null;

            if (ItemData.ItemType == EItemType.UseAble)
            {
                NewItemBase = new UseableItem();
            }
            else if (ItemData.ItemType == EItemType.Equipment)
            {
                NewItemBase = new EquipmentItem();
            }
            else
            {
                NewItemBase = new ItemBase();
            }

            NewItemBase.SetItemData(ItemData);

            return NewItemBase;
        }
    }

    public void RandDataSet()
    {
        int Size = ItemManager.Instance.items.Count;

        int RandNum = UnityEngine.Random.Range(0, Size);

        CurrentItemData = ItemManager.Instance.items[RandNum];
    }

    public void SetItemData(ItemData Data)
    {
        CurrentItemData = Data;

        foreach (var funcData in CurrentItemData.ItemFunctionList)
        {
            var func = ItemManager.Instance.GetItemFunctionFromDictionary(funcData.eFunc.ToString());

            if (null != func)
            {
                ItemFunction tempStruct = new ItemFunction();

                ItemFunctionArgs tempArgs = new ItemFunctionArgs();
                tempArgs.arg2 = this;
                tempArgs.arg3 = funcData.tData;

                tempStruct.action = func;
                tempStruct.args = tempArgs;

                CurrentItemFunctions.Add(tempStruct);
            }
        }
    }

    public virtual void UseItem(PlayerController playerController)
    {
        Debug.Log("This is ItemBase");
    }
}
