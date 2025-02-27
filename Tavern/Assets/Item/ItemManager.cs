using NUnit.Framework.Interfaces;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public static ItemManager Instance { get; private set; } // ���� ������ ���� �̱���

    [SerializeField] public List<ItemData> items; // ������ ����Ʈ
    public WorldItem itemPrefab;

    public bool bReady = false;

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

        if (items != null)
        {
            // �ε� ����
            Debug.Log("ItemDatas �ε� ����!");

            bReady = true;
        }
        else
        {
            // �ε� ����
            Debug.LogError("ItemDatas �ε� ����!");
        }
    }

    public void ItemSpawn(ItemBase SpawnItem, Vector3 SpawnPos, Quaternion SpawnRotation)
    {
        WorldItem SpawnWorldItem = Instantiate(itemPrefab, SpawnPos, SpawnRotation);

        SpawnItem.ItemType = ItemBase.EItemType.NoUseAble;
        SpawnWorldItem.SetItem(SpawnItem);
        SpawnWorldItem.bRandSet = false;
    }
}
