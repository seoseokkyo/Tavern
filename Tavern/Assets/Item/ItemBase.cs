using System.Collections.Generic;
using System;
using UnityEngine;
using static ItemFunctions;

public enum EItemType
{
    NoUseAble,
    UseAble,
    Equipment, // �ϴ� �����Կ��� ����ϴ� ���·�
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

    [Tooltip("�������� �÷��̾��� �տ� ���� �� ����� Offset")]
    public Vector3 AttachLocation;
    [Tooltip("�������� �÷��̾��� �տ� ���� �� ����� Rotation")]
    public Quaternion AttachRotation;
    [Tooltip("�������� ����� �� ����Ǵ� ��ɵ�")]
    public List<ItemFunctionParam> ItemFunctionList;
    [Tooltip("��� �������� ��� �������� ���")]
    public float fOptionValue1;
    [Tooltip("����")]
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
    // ���� �������� ����

    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public ItemData CurrentItemData;

    public InventoryComp OwnerInventory = null;

    /// <summary>
    ///  ������ ������ ȿ������ �����ִ� ����ü�� ����Ʈ
    /// </summary>
    /// 
    protected List<ItemFunction> CurrentItemFunctions = new List<ItemFunction>();


    // �����ۺ��̽� ������ ���ټ��� ����
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
