using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class WorldItem : Interactable
{
    // Scene등에서 플레이어와 상호작용이 가능한 아이템의 형태

    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public ItemBase item;

    public MeshFilter WorldItemMeshFilter;
    public MeshRenderer WorldItemMesh;

    // 나중에 메쉬 콜리더 사이즈 어떻게 할 지 생각해볼것

    public bool bRandSet = true;
    public bool bInit = false;

    public string InitItemName;

    void Start()
    {
        if (!PhotonNetwork.OfflineMode && !PhotonNetwork.IsMasterClient)
        {
            PhotonManager.Instance.OnJoinedRoomEndDelegate += RequestInit;

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

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(item);
            stream.SendNext(transform.localScale);
        }
        else
        {
            SetItem((ItemBase)stream.ReceiveNext());
            transform.localScale = (Vector3)stream.ReceiveNext();
        }
    }

    public void RequestInit()
    {
        //photonView.RequestOwnership();
        photonView.RPC("RequestItemData", RpcTarget.MasterClient, PhotonNetwork.LocalPlayer);

        Debug.Log($"Req_photonView ID : {photonView.InstantiationId}");
    }

    [PunRPC]
    public void ReceiveItemData(ItemBase data)
    {
        SetItem(data);

        Debug.Log($"Rec_photonView ID : {photonView.InstantiationId}");
    }

    [PunRPC]
    public void RequestItemData(Player requester)
    {
        photonView.RPC("ReceiveItemData", requester, item);

        Debug.Log($"Res_photonView ID : {photonView.InstantiationId}");
    }
}
