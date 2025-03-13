using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public List<ItemData> menuList = new List<ItemData>();
    
    void Start()
    {
        //ClearMenuList();
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
