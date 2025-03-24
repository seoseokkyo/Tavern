using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class WorldItem : Interactable
{
    // Scene등에서 플레이어와 상호작용이 가능한 아이템의 형태
    public ItemBase item;

    public MeshFilter WorldItemMeshFilter;
    public MeshRenderer WorldItemMesh;

    public string InitItemName = "";

    void Start()
    {
        WorldItemMeshFilter = GetComponent<MeshFilter>();
        WorldItemMesh = GetComponent<MeshRenderer>();

        //if (InitItemName == "")
        //{
        //    RequestServerData();
        //}
        //else
        //{
        //    item = ItemBase.ItemBaseCreator.CreateItemBase(ItemManager.Instance.GetItemDataByName(InitItemName));
        //    SetItem(item);
        //}

        if (!PhotonNetwork.IsMasterClient)
        {
            RequestServerData();
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public override string GetInteractingDescription() { return item.CurrentItemData.itemDescription; }

    public override void Interact()
    {
        if (interactPlayer)
        {
            Debug.Log($"itemCount : {item.CurrentItemData.itemCount}");

            if (interactPlayer.PlayerInventory.AddItem(ref item))
            {
                item = null;

                RequestDestroy();
            }
        }
    }

    public void SetItem(ItemBase inputItem)
    {
        item = inputItem;
        InitItemName = inputItem.CurrentItemData.itemName;

        if (item.CurrentItemData.ItemPrefab)
        {
            GameObject child = Instantiate(item.CurrentItemData.ItemPrefab);
            child.transform.SetParent(transform, false);

            child.SetActive(true);
        }
        else
        {
            WorldItemMeshFilter.sharedMesh = item.CurrentItemData.itemMeshFilter.sharedMesh;
            WorldItemMesh.sharedMaterials = item.CurrentItemData.itemMesh.sharedMaterials;
        }
    }

    public void RequestServerData()
    {
        photonView.RPC("ClientToServerRequestItemData", RpcTarget.MasterClient, PhotonNetwork.LocalPlayer);
    }

    [PunRPC]
    public void ServerToClientReceiveItemData(string ItemDataName, int CurrentItemCount)
    {
        SetItem(ItemBase.ItemBaseCreator.CreateItemBase(ItemManager.Instance.GetItemDataByName(ItemDataName)));

        item.CurrentItemData.itemCount = CurrentItemCount;

        Debug.Log($"ServerToClientReceiveItemData_CurrentItemCount : {CurrentItemCount}");
    }

    [PunRPC]
    public void ClientToServerRequestItemData(Player requester)
    {
        photonView.RPC("ServerToClientReceiveItemData", requester, item.CurrentItemData.itemName, item.CurrentItemData.itemCount);

        Debug.Log($"ClientToServerRequestItemData_CurrentItemCount : {item.CurrentItemData.itemCount}");
    }

    public void RequestDestroy()
    {
        photonView.RPC("DestroyFromServer", photonView.Owner);
    }

    [PunRPC]
    public void DestroyFromServer()
    {
        PhotonNetwork.Destroy(gameObject);
    }

    public void ClientToAllItemDataSync()
    {
        photonView.RPC("ClientToAllRequestItemDataSync", RpcTarget.All, item.CurrentItemData.itemName, item.CurrentItemData.itemCount);
    }

    [PunRPC]
    public void ClientToAllRequestItemDataSync(string ItemDataName, int CurrentItemCount)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            photonView.RequestOwnership();
        }

        SetItem(ItemBase.ItemBaseCreator.CreateItemBase(ItemManager.Instance.GetItemDataByName(ItemDataName)));

        item.CurrentItemData.itemCount = CurrentItemCount;

        Debug.Log($"ClientToAllRequestItemDataSync_CurrentItemCount : {item.CurrentItemData.itemCount}");
    }
}
