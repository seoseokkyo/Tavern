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
        if (!PhotonNetwork.IsMasterClient) return;

        menuList = selectedMenu;
        List<string> ids = new List<string>();
        foreach (ItemData item in selectedMenu)
        {
            ids.Add(item.itemName);
        }

        photonView.RPC("SyncMenuList", RpcTarget.Others, ids.ToArray());
    }

    [PunRPC]
    void SyncMenuList(string[] ids)
    {
        menuList.Clear();

        foreach (string id in ids)
        {
            ItemData data = ItemManager.Instance.GetItemDataByName(id);
            menuList.Add(data);
        }
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