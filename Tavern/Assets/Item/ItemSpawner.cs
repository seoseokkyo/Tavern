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

    void Start()
    {
        if(StartPointObject)
        {
            Destroy(StartPointObject);
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
                    CurrentSpawnPos.x++;
                    continue;
                }

                int Rand = Random.Range(0, SpawnTargetItemsName.Count);
                string TartgetItemName = SpawnTargetItemsName[Rand];
                ItemData TargetItemData = ItemManager.Instance.GetItemDataByName(TartgetItemName);
                var CreatedItemBase = ItemManager.Instance.CreateItemBase(TargetItemData);

                WorldItem WorldItemTemp = ItemManager.Instance.ItemSpawn(CreatedItemBase, CurrentSpawnPos, Quaternion.identity);

                SpawnWorldItem.Add(WorldItemTemp);

                CurrentSpawnPos.x++;
            }

            CurrentSpawnPos.x = transform.position.x;
            CurrentSpawnPos.z++;
        }
    }

    bool GroundCheck(Vector3 Position, out RaycastHit hit)
    {
        Ray ray = new Ray(Position, new Vector3(0, -1, 0));

        return Physics.Raycast(ray, out hit, 10);
    }
}
