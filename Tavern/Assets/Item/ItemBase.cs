using System.Collections.Generic;
using System;
using UnityEngine;

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

    private object _ownerInventoryLock = new object();
    public InventoryComp OwnerInventory = null;

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
    }

    public virtual void UseItem(PlayerController playerController)
    {
        Debug.Log("This is ItemBase");
    }
}
