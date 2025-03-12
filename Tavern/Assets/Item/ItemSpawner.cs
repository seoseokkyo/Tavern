using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class ItemSpawner : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public List<string> SpawnTargetItemsName = new List<string>();
    private List<WorldItem> SpawnWorldItem = new List<WorldItem>();
    public int SpawnGridNum = 0;

    public GameObject StartPointObject;

    private List<ItemBase> CreateItemBases = new List<ItemBase>();
    private List<Vector3> Positions = new List<Vector3>();

    void Start()
    {
        if(StartPointObject)
        {

        }

        SpawnWorldItem.Clear();

        Vector3 CurrentSpawnPos = transform.position;

        for (int i = 0; i < SpawnGridNum; i++)
        {
            for (int j = 0; j < SpawnGridNum; j++)
            {
                RaycastHit hit;

                if (GroundCheck(CurrentSpawnPos, out hit))
                {
                    CurrentSpawnPos.y = hit.point.y;
                    CurrentSpawnPos.y++;
                }
                else
                {
                    continue;
                }

                int Rand = Random.Range(0, SpawnTargetItemsName.Count);
                string TartgetItemName = SpawnTargetItemsName[Rand];

                ItemData TargetItemData = ItemManager.Instance.GetItemDataByName(TartgetItemName);

                var CreatedItemBase = ItemManager.Instance.CreateItemBase(TargetItemData);

                CreateItemBases.Add(CreatedItemBase);
                Positions.Add(CurrentSpawnPos);

                CurrentSpawnPos.x++;
            }

            CurrentSpawnPos.z++;
        }

        Invoke("SpawnItems", 1);
    }

    // Update is called once per frame
    void Update()
    {

    }

    bool GroundCheck(Vector3 Position, out RaycastHit hit)
    {
        Ray ray = new Ray(Position, new Vector3(0, -1, 0));

        return Physics.Raycast(ray, out hit, 10);
    }

    public void SpawnItems()
    {
        int Count = 0;
        for(int i = 0; i < SpawnGridNum; i++)
        {
            for (int j = 0; j < SpawnGridNum; j++)
            {
                WorldItem WorldItemTemp = ItemManager.Instance.ItemSpawn(CreateItemBases[Count], Positions[Count], Quaternion.identity);

                SpawnWorldItem.Add(WorldItemTemp);

                Count++;
            }
        }
    }
}
