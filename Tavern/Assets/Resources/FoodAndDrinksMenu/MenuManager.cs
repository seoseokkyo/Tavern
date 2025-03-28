using NUnit.Framework;
using Photon.Pun;
using System;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviourPun
{
    public static MenuManager Instance;
    public List<ItemData> menuList = new List<ItemData>();

    public event Action OnMenuUpdated; 

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
    }

    void Update()
    {


    }

    public void SetMenu(List<ItemData> selectedMenu)
    {
        /*
        if (!PhotonNetwork.IsMasterClient) return;

        menuList = selectedMenu;
        */
        menuList = selectedMenu;

        List<string> ids = new List<string>();
        foreach (ItemData item in selectedMenu)
        {
            ids.Add(item.itemName);
        }

        if (ids.Count > 1)
        {
            photonView.RPC("SyncMenuList_Multiple", RpcTarget.AllBuffered, ids.ToArray());
        }
        else if (ids.Count == 1)
        {
            string id = ids[0];
            photonView.RPC("SyncMenuList_Single", RpcTarget.AllBuffered, id);
        }
        else if (ids.Count == 0)
        {
            photonView.RPC("SyncMenuList_None", RpcTarget.AllBuffered);
        }
    }

    [PunRPC]
    void SyncMenuList_Multiple(string[] ids)
    {
        menuList.Clear();

        foreach (string id in ids)
        {
            ItemData data = ItemManager.Instance.GetItemDataByName(id);
            menuList.Add(data);
        }

        OnMenuUpdated?.Invoke();
    }

    [PunRPC]
    void SyncMenuList_Single(string id)
    {
        menuList.Clear();

        ItemData data = ItemManager.Instance.GetItemDataByName(id);
        menuList.Add(data);

        OnMenuUpdated?.Invoke();
    }

    [PunRPC]
    void SyncMenuList_None()
    {
        menuList.Clear();
        OnMenuUpdated?.Invoke();
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
        if (menuList.Contains(item))
            if (menuList.Contains(item))
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