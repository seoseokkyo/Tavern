using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class WorldItem : Interactable
{
    [HideInInspector]
    public ItemBase item;

    [HideInInspector]
    public MeshFilter WorldItemMeshFilter;

    [HideInInspector]
    public MeshRenderer WorldItemMesh;

    public bool bEditorSetted = false;

    public string InitItemName = "";

    [HideInInspector]
    public Rigidbody ItemRigidbody;

    [HideInInspector]
    public GameObject MeshObj = null;

    [HideInInspector]
    public IngredientComp IngredientComponent = null;

    void Start()
    {
        WorldItemMeshFilter = GetComponent<MeshFilter>();
        WorldItemMesh = GetComponent<MeshRenderer>();

        ItemRigidbody = GetComponent<Rigidbody>();
        ItemRigidbody.isKinematic = false;

        if (bEditorSetted)
        {
            SetItem(ItemBase.ItemBaseCreator.CreateItemBase(ItemManager.Instance.GetItemDataByName(InitItemName)));
        }
        else if (!PhotonNetwork.IsMasterClient && string.IsNullOrEmpty(InitItemName))
        {
            RequestServerData();
        }

        if (item.CurrentItemData.ItemType == EItemType.Equipment)
        {
            IngredientComponent = gameObject.AddComponent<IngredientComp>();

            if (IngredientComponent)
            {
                IngredientComponent.CurrentWorldItem = this;

                var data = ItemManager.Instance.GetIngredientData(item.CurrentItemData.itemName);

                if (null != data)
                {
                    IngredientComponent.SetData(data);
                }
            }

            Debug.Log($"IngredientComponent : {IngredientComponent.name}");
        }
    }

    void Update()
    {
        if(item.CurrentItemData.ItemType != EItemType.Buildable)
        {
            if (ItemRigidbody.isKinematic && ItemRigidbody.linearVelocity.magnitude < 0.1f)
            {
                ItemRigidbody.useGravity = false;
                ItemRigidbody.isKinematic = true;
            }
        }
    }

    public override string GetInteractingDescription() {
        if (item.CurrentItemData.ItemType != EItemType.Buildable)
            return item.CurrentItemData.itemDescription;
        else
            return "Press [B] to Replace";
    }

    public override void Interact()
    {
        if(item.CurrentItemData.ItemType != EItemType.Buildable)
        {
            if (interactPlayer)
            {
                interactPlayer.CurrentPlayer.ItemAttachToRightHand(this);
            }
        }

        // Buildable 은 E 키 상호작용 일단 막아둠. 
    }

    public void SetItem(ItemBase inputItem)
    {
        item = inputItem;
        InitItemName = inputItem.CurrentItemData.itemName;

        if (item.CurrentItemData.ItemPrefab)
        {
            MeshObj = Instantiate(item.CurrentItemData.ItemPrefab);
            MeshObj.transform.SetParent(transform, false);

            MeshObj.SetActive(true);
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

        //Debug.Log($"ServerToClientReceiveItemData_CurrentItemCount : {CurrentItemCount}");
    }

    [PunRPC]
    public void ClientToServerRequestItemData(Player requester)
    {
        photonView.RPC("ServerToClientReceiveItemData", requester, item.CurrentItemData.itemName, item.CurrentItemData.itemCount);

        //Debug.Log($"ClientToServerRequestItemData_CurrentItemCount : {item.CurrentItemData.itemCount}");
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

        //Debug.Log($"ClientToAllRequestItemDataSync_CurrentItemCount : {item.CurrentItemData.itemCount}");
    }
}
