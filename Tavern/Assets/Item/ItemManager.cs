using NUnit.Framework.Interfaces;
using System;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Analytics.IAnalytic;

public class ItemManager : MonoBehaviour
{
    public static ItemManager Instance { get; private set; } // 전역 접근을 위한 싱글톤

    [SerializeField] public List<ItemData> items; // 아이템 리스트
    [SerializeField] public List<CreateRecipe> createRecipes; // 제작 아이템 레시피
    Dictionary<string, ItemData> ItemsDictionary = new Dictionary<string, ItemData>();
    Dictionary<string, Tuple<CreateTargetRecipeData, List<CreateResources>>> CreateRecipesDictionary = new Dictionary<string, Tuple<CreateTargetRecipeData, List<CreateResources>>>();

    public WorldItem itemPrefab;

    public bool bReady = false;

    public Texture2D EmptyImage;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject); // 기존 객체가 있다면 삭제
        }

        // 싱글톤 패턴 적용
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // 씬이 바뀌어도 유지
        }


        items = Resources.Load<ItemDatas>("ItemDatas").items;
        InitCreateItemsDictionary();

        createRecipes = Resources.Load<ItemDatas>("ItemDatas").createRecipes;
        InitCreateRecipeDictionary();


        if (items != null)
        {
            bReady = true;
        }
        else
        {

        }
    }

    public WorldItem ItemSpawn(ItemBase SpawnItem, Vector3 SpawnPos, Quaternion SpawnRotation)
    {
        WorldItem SpawnWorldItem = Instantiate(itemPrefab, SpawnPos, SpawnRotation);
        SpawnWorldItem.bRandSet = false;
        SpawnWorldItem.InitItemName = SpawnItem.CurrentItemData.itemName;

        SpawnWorldItem.transform.localScale = new Vector3(10f, 10f, 10f);

        return SpawnWorldItem;
    }

    void InitCreateRecipeDictionary()
    {
        foreach (var pair in createRecipes)
        {
            if (!CreateRecipesDictionary.ContainsKey(pair.CreateItemData.ItemName))
            {
                CreateRecipesDictionary[pair.CreateItemData.ItemName] = new Tuple<CreateTargetRecipeData, List<CreateResources>>(pair.CreateItemData, pair.Resources);
            }
            else
            {
                Debug.LogWarning($"중복 키 발견: {pair.CreateItemData.ItemName}, 기존 값이 유지됩니다.");
            }
        }
    }

    void InitCreateItemsDictionary()
    {
        foreach (var item in items)
        {
            if (!ItemsDictionary.ContainsKey(item.itemName))
            {
                ItemsDictionary[item.itemName] = item;
            }
            else
            {
                Debug.LogWarning($"중복 키 발견: {item}, 기존 값이 유지됩니다.");
            }
        }
    }

    public ItemData GetItemDataByName(string ItemName)
    {
        if(!ItemsDictionary.ContainsKey(ItemName))
        {
            ItemData EmptyItemData = new ItemData();
            return EmptyItemData;
        }

        return ItemsDictionary[ItemName];
    }

    public Texture2D GetItemTextureByName(string ItemName)
    {
        return ItemsDictionary[ItemName].itemIcon;
    }

    public Sprite GetItemSpriteByName(string ItemName)
    {
        if (ItemName == "" || !ItemsDictionary.ContainsKey(ItemName))
        {
            Rect EmptyRect = new Rect(0, 0, Mathf.Min(EmptyImage.width, 500), Mathf.Min(EmptyImage.height, 500));

            return Sprite.Create(EmptyImage, EmptyRect, new Vector2(0.5f, 0.5f));
        }
        else
        {
            Rect rect = new Rect(0, 0, Mathf.Min(ItemsDictionary[ItemName].itemIcon.width, 500), Mathf.Min(ItemsDictionary[ItemName].itemIcon.height, 500));

            return Sprite.Create(ItemsDictionary[ItemName].itemIcon, rect, new Vector2(0.5f, 0.5f));
        }
    }

    public List<string> GetCreateRecipeAllList()
    {
        List<string> CreateItemList = new List<string>();

        foreach (var rcp in createRecipes)
        {
            CreateItemList.Add(rcp.CreateItemData.ItemName);
        }

        return CreateItemList;
    }

    public List<string> GetCreateRecipeListByType(CreateItemType createType)
    {
        List<string> CreateItemList = new List<string>();

        foreach (var rcp in createRecipes)
        {
            if (rcp.CreateItemData.CreateItemType == createType)
            {
                CreateItemList.Add(rcp.CreateItemData.ItemName);
            }
        }

        return CreateItemList;
    }

    public List<CreateResources> GetCreateItemResourcesListByName(string ItemName)
    {
        return CreateRecipesDictionary[ItemName].Item2;
    }

    public CreateTargetRecipeData GetRecipeDataByName(string ItemName)
    {
        return CreateRecipesDictionary[ItemName].Item1;
    }

    public ItemBase CastItemType(ItemBase CurrentItemBase)
    {
        ItemBase NewItemBase = null;

        if (CurrentItemBase.CurrentItemData.ItemType == EItemType.UseAble)
        {
            NewItemBase = new UseableItem();
            NewItemBase.SetItemData(CurrentItemBase.CurrentItemData);

            Debug.Log($"{CurrentItemBase.CurrentItemData.itemName} Was Changed To UseableItem");
        }
        else if (CurrentItemBase.CurrentItemData.ItemType == EItemType.Equipment)
        {
            NewItemBase = new EquipmentItem();
            NewItemBase.SetItemData(CurrentItemBase.CurrentItemData);

            Debug.Log($"{CurrentItemBase.CurrentItemData.itemName} Was Changed To EquipmentItem");
        }

        return NewItemBase ?? CurrentItemBase;
    }
}
