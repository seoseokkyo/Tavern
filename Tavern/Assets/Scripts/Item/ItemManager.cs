using NUnit.Framework.Interfaces;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public static ItemManager Instance { get; private set; } // 전역 접근을 위한 싱글톤

    [SerializeField] public List<ItemData> items; // 아이템 리스트
    public WorldItem itemPrefab;

    public bool bReady = false;

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

        if (items != null)
        {
            // 로드 성공
            Debug.Log("ItemDatas 로드 성공!");

            bReady = true;
        }
        else
        {
            // 로드 실패
            Debug.LogError("ItemDatas 로드 실패!");
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
