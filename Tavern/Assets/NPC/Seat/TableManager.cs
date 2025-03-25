using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class TableManager : MonoBehaviourPunCallbacks
{
    public static TableManager instance;
    private List<TableScript> tables = new List<TableScript>();

    private Dictionary<int, TableScript> tableDict = new Dictionary<int, TableScript>();

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        tables.AddRange(FindObjectsOfType<TableScript>());
        for(int i = 0; i < tables.Count; i++)
        {
            tableDict[i] = tables[i];
            tables[i].tableID = i;
        }
    }

    public int GetTableID(TableScript table)
    {
        foreach(var pair in tableDict)
        {
            if (pair.Value == table) return pair.Key;
        }

        return -1;
    }

    public TableScript GetTableByID(int id)
    {
        if(tableDict.ContainsKey(id)) return tableDict[id];
        return null;
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
