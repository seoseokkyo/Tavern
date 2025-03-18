using System.Collections.Generic;
using UnityEngine;

public class TableManager : MonoBehaviour
{
    public static TableManager instance;
    private List<TableScript> tables = new List<TableScript>();

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        tables.AddRange(FindObjectsOfType<TableScript>());
    }

    void Update()
    {
        
    }

    public TableScript FindRandomAvailableTable()
    {
        List<TableScript> availables = new List<TableScript>();
        foreach(var table in tables)
        {
            if(table.HasAvailableSeat())
            {
                availables.Add(table);
            }
        }

        if (availables.Count == 0) return null;

        TableScript selectedTable = availables[Random.Range(0, availables.Count)];
        return selectedTable;
    }
}
