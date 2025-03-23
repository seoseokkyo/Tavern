using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class WorldItem : Interactable
{
    // Scene등에서 플레이어와 상호작용이 가능한 아이템의 형태
    public ItemBase item;

    public MeshFilter WorldItemMeshFilter;
    public MeshRenderer WorldItemMesh;

    public bool bRandSet = true;
    public bool bInit = false;

    public string InitItemName;

    void Start()
    {
        if (!PhotonNetwork.OfflineMode && !PhotonNetwork.IsMasterClient)
        {
            RequestInit();

            return;
        }

        WorldItemMeshFilter = GetComponent<MeshFilter>();
        WorldItemMesh = GetComponent<MeshRenderer>();

        if (bRandSet)
        {
            item.RandDataSet();

            item = ItemManager.Instance.CastItemType(item);
        }
        else
        {
            item = ItemBase.ItemBaseCreator.CreateItemBase(ItemManager.Instance.GetItemDataByName(InitItemName));
        }

        SetItem(item);
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
            if (interactPlayer.PlayerInventory.AddItem(ref item))
            {
                item = null;

                PhotonNetwork.Destroy(gameObject);
            }
        }
    }

    public void SetItem(ItemBase inputItem)
    {
        item = inputItem;

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

    public void RequestInit()
    {
        photonView.RPC("RequestItemData", RpcTarget.MasterClient, PhotonNetwork.LocalPlayer);

        //Debug.Log($"Req_photonView ID : {photonView.InstantiationId}");
    }

    [PunRPC]
    public void ReceiveItemData(string ItemDataName)
    {
        SetItem(ItemBase.ItemBaseCreator.CreateItemBase(ItemManager.Instance.GetItemDataByName(ItemDataName)));

        //Debug.Log($"Rec_photonView ID : {photonView.InstantiationId}");
    }

    [PunRPC]
    public void RequestItemData(Player requester)
    {
        photonView.RPC("ReceiveItemData", requester, item.CurrentItemData.itemName);

        //Debug.Log($"Res_photonView ID : {photonView.InstantiationId}");
    }
}
