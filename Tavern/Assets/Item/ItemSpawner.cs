using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

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
                }
                else
                {
                    continue;
                }

                var CreatedItemBase = ItemManager.Instance.CreateItemBase(ItemManager.Instance.GetItemDataByName(SpawnTargetItemsName[Random.Range(0, SpawnTargetItemsName.Count)]));

                WorldItem WorldItemTemp = ItemManager.Instance.ItemSpawn(CreatedItemBase, CurrentSpawnPos, Quaternion.identity);

                SpawnWorldItem.Add(WorldItemTemp);

                CurrentSpawnPos.x++;
            }

            CurrentSpawnPos.z++;
        }
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
}
