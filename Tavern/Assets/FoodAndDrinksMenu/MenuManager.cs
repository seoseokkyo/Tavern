using NUnit.Framework;
using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviourPun
{
    public static MenuManager Instance;
    public List<ItemData> menuList = new List<ItemData>();

    private void Awake()
    {
    }

    void Start()
    {
    }

    void Update()
    {
        
    }


    public void AddMenu(ItemData item)
    {
        if (!menuList.Contains(item))
        {
            menuList.Add(item);
        }
    }

    public void RemoveMenu(ItemData item)
    {
        if(menuList.Contains(item))
        {
            menuList.Remove(item);
        }
    }

    public List<ItemData> GetMenuList()
    {
        return menuList;
    }

    public void ClearMenuList()
    {
        menuList.Clear();
    }
}
