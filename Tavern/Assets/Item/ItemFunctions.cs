using System.Collections.Generic;
using System;
using UnityEngine;
using static ItemFunctionParam;

[System.Serializable]
public class ItemFunctionParam
{
    [System.Serializable]
    public class ItemFunctionData
    {
        [Tooltip("ü��, ������� ����� float��")]
        public float fValue1;
        public float fValue2;
        public float fValue3;

        [Tooltip("������ ���� ����� ����� float��")]
        public int iValue1;
        public int iValue2;
        public int iValue3;

        [Tooltip("������ ���� �̸�� ����� float��")]
        public string strValue1;
        public string strValue2;
        public string strValue3;

        ItemFunctionData()
        {
            fValue1 = 0f;
            fValue2 = 0f;
            fValue3 = 0f;
            iValue1 = 0;
            iValue2 = 0;
            iValue3 = 0;
            strValue1 = "";
            strValue2 = "";
            strValue3 = "";
        }
    }

    public ItemFunctions.EItemFunctionsList eFunc;
    public ItemFunctionData tData;
}

public class ItemFunctionArgs
{
    public PlayerController arg1;
    public ItemBase arg2;
    public ItemFunctionParam.ItemFunctionData arg3;
}

[System.Serializable]
public class ItemFunction
{
    public Action<ItemFunctionArgs> action;
    public ItemFunctionArgs args;
}

public class ItemFunctions : MonoBehaviour
{
    public enum EItemFunctionsList
    {
        AccumulateHP,
        AddItem,
        ConsumeItem,
        EItemFunctionsListMax
    }

    Dictionary<string, Action<ItemFunctionArgs>> ItemFunctionDictionary = new Dictionary<string, Action<ItemFunctionArgs>>();

    private void Awake()
    {
        ItemFunctionDictionary.Add("AccumulateHP", AccumulateHP);
        ItemFunctionDictionary.Add("AddItem", AddItem);
        ItemFunctionDictionary.Add("ConsumeItem", ConsumeItem);
    }


    public Action<ItemFunctionArgs> GetItemFunctionFromDictionary(string FunctionName)
    {
        if (!ItemFunctionDictionary.ContainsKey(FunctionName))
        {
            return null;
        }

        return ItemFunctionDictionary[FunctionName];
    }

    void AccumulateHP(ItemFunctionArgs FunctionArgs)
    {
        // fValue1�� ü�������� ���
        FunctionArgs.arg1.CurrentPlayer.ChangeHP(FunctionArgs.arg3.fValue1);
    }

    void AddItem(ItemFunctionArgs FunctionArgs)
    {
        // iValue1�� ������ ���� ������ ���
        // strValue1�� ������ Ư���� ���
        var CreatedItemData = ItemManager.Instance.GetItemDataByName(FunctionArgs.arg3.strValue1);
        CreatedItemData.itemCount = FunctionArgs.arg3.iValue1;

        var CreatedItemBase = ItemBase.ItemBaseCreator.CreateItemBase(CreatedItemData);

        FunctionArgs.arg1.PlayerInventory.AddItem(ref CreatedItemBase);
    }

    void ConsumeItem(ItemFunctionArgs FunctionArgs)
    {
        // ���� ���� �÷��̾� Hand�� ����ٸ��ѵ��� �����ٵ� �ϴ� ���⼭
        bool bCheck = false;
        if (FunctionArgs.arg2.CurrentItemData.ItemType == EItemType.Equipment)
        {
            int RemainNum = FunctionArgs.arg2.CurrentItemData.itemCount;
            if (RemainNum <= 1)
            {
                bCheck = true;
            }
        }

        FunctionArgs.arg2.OwnerInventory.ConsumeItem(FunctionArgs.arg2);

        if(bCheck)
        {
            FunctionArgs.arg1.CurrentPlayer.OnChanged(null);
        }
    }
}
