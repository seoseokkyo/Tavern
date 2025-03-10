using NUnit.Framework.Interfaces;
using System;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Analytics.IAnalytic;

public class ItemManager : MonoBehaviour
{
    public static ItemManager Instance { get; private set; } // ���� ������ ���� �̱���

    [SerializeField] public List<ItemData> items; // ������ ����Ʈ
    [SerializeField] public List<CreateRecipe> createRecipes; // ���� ������ ������
    Dictionary<string, ItemData> ItemsDictionary = new Dictionary<string, ItemData>();
    Dictionary<string, Tuple<CreateTargetRecipeData, List<CreateResources>>> CreateRecipesDictionary = new Dictionary<string, Tuple<CreateTargetRecipeData, List<CreateResources>>>();

    public WorldItem itemPrefab;

    public bool bReady = false;

    public Texture2D EmptyImage;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject); // ���� ��ü�� �ִٸ� ����
        }

        // �̱��� ���� ����
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // ���� �ٲ� ����
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

    public void ItemSpawn(ItemBase SpawnItem, Vector3 SpawnPos, Quaternion SpawnRotation)
    {
        WorldItem SpawnWorldItem = Instantiate(itemPrefab, SpawnPos, SpawnRotation);

        SpawnItem.ItemType = ItemBase.EItemType.NoUseAble;
        SpawnWorldItem.SetItem(SpawnItem);
        SpawnWorldItem.bRandSet = false;
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
                Debug.LogWarning($"�ߺ� Ű �߰�: {pair.CreateItemData.ItemName}, ���� ���� �����˴ϴ�.");
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
                Debug.LogWarning($"�ߺ� Ű �߰�: {item}, ���� ���� �����˴ϴ�.");
            }
        }
    }

    public ItemData GetItemDataByName(string ItemName)
    {
        return ItemsDictionary[ItemName];
    }

    public Texture2D GetItemTextureByName(string ItemName)
    {
        return ItemsDictionary[ItemName].itemIcon;
    }

    public Sprite GetItemSpriteByName(string ItemName)
    {
        if (ItemName == "" || !CreateRecipesDictionary.ContainsKey(ItemName))
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

    public List<string> GetCreateRecipeList()
    {
        List<string> CreateItemList = new List<string>();

        foreach (var rcp in createRecipes)
        {
            CreateItemList.Add(rcp.CreateItemData.ItemName);
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
}
