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

    public void RandDataSet()
    {
        int Size = ItemManager.Instance.items.Count;

        int RandNum = UnityEngine.Random.Range(0, Size);

        CurrentItemData = ItemManager.Instance.items[RandNum];

        //// WaterBucket 제외 -> 테스트용
        //if(RandNum != 10)
        //    CurrentItemData = ItemManager.Instance.items[RandNum];
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
