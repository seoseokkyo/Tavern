using UnityEngine;
using Photon.Pun;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine.XR;
using UnityEngine.InputSystem;

public class TableScript : WorldItem
{
    private BuildableComponent buildableComponent;

    public int tableID;

    public List<SeatData> seats = new List<SeatData>();
    public GameObject foodPrefab;

    public ItemDatas itemDatas;
    private PhotonView photonView;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if(rb != null)
        {
            rb.isKinematic = true;
            rb.useGravity = false;
        }

        photonView = GetComponent<PhotonView>(); 
        buildableComponent = GetComponent<BuildableComponent>();
        if (buildableComponent != null)
        {
            //buildableComponent.StopBuilding();
            buildableComponent.photonView = photonView;
            buildableComponent.originFurniture = this.gameObject;
        }

        for (int i = 0; i < seats.Count; i++)
        {
            seats[i].seatID = i;
        }

        // 이건 GetUsedToolItem함수 결과 확인용
        ItemData usedTool = GetUsedToolItem(ERequiredTool.Bowl);
        //Debug.Log($"usedTool : {usedTool.itemName}");

        usedTool = GetUsedToolItem(ERequiredTool.Plate);
        //Debug.Log($"usedTool : {usedTool.itemName}");

        usedTool = GetUsedToolItem(ERequiredTool.Cup);
        //Debug.Log($"usedTool : {usedTool.itemName}");
    }

    public override void Interact()
    {
        if (buildableComponent != null)
        {
            if (!buildableComponent.IsBuilding())
            {
                buildableComponent.StartBuilding(); 
            }
        }
    }

    private void Update()
    {
        if (buildableComponent != null && buildableComponent.IsBuilding())
        {
            buildableComponent.UpdatePreviewMeshPosition();
            if (UnityEngine.Input.GetKey(KeyCode.C) && buildableComponent.IsPlacementValid())
            {
                buildableComponent.PlaceFurniture(buildableComponent.previewMesh.transform.position, buildableComponent.previewMesh.transform.rotation);
            }
        }
    }

    public void AddSelfToManager()
    {
        GameObject manager = GameObject.FindWithTag("TableManager");
        if(manager != null)
        {
            manager.GetComponent<TableManager>().Initialize();
        }
    }

    public bool HasAvailableSeat()
    {
        foreach (SeatData seat in seats)
        {
            if (!seat.isSitting)
                return true;
        }
        return false;
    }

    public SeatData GetSeatByID(int id)
    {
        foreach (SeatData seat in seats)
        {
            if (seat.seatID == id) return seat;
        }
        return null;
    }

    public SeatData GetAvailableSeat()
    {
        foreach (SeatData seat in seats)
        {
            if (!seat.isSitting)
            {
                photonView.RPC("SetSeatOccupied", RpcTarget.All, seat.seatID);
                return seat;
            }
        }
        return null;
    }

    [PunRPC]
    void SetSeatOccupied(int id)
    {
        foreach (SeatData seat in seats)
        {
            if (seat.seatID == id)
            {
                seat.isSitting = true;
                break;
            }
        }
    }

    public void ReleaseSeat(SeatData seat)
    {
        if (seat != null)
        {
            photonView.RPC("ReleaseSeatRPC", RpcTarget.All, seat.seatID);
            seat.isSitting = false;
        }

        RemoveFood(seat);
    }

    [PunRPC]
    void ReleaseSeatRPC(int id)
    {
        foreach (SeatData seat in seats)
        {
            if (seat.seatID == id)
            {
                seat.isSitting = false;
                break;
            }
        }
    }


    public void SetFood(ItemData food, SeatData seat)
    {
        if (seat != null)
        {
            photonView.RPC("SetFoodRPC", RpcTarget.All, seat.seatID, food.itemName);
        }
    }

    [PunRPC]
    void SetFoodRPC(int seatID, string itemName)
    {
        SeatData seat = GetSeatByID(seatID);
        ItemData food = ItemManager.Instance.GetItemDataByName(itemName);

        var createdItemBase = ItemBase.ItemBaseCreator.CreateItemBase(food);
        WorldItem worldItem = ItemManager.Instance.ItemSpawn(createdItemBase, seat.foodPositionLeft.position, Quaternion.identity);
        if (seat.foodLeft == null)
        {
            worldItem.transform.SetParent(seat.foodPositionLeft);
            seat.foodLeft = worldItem;
        }
        else
        {
            worldItem.transform.SetParent(seat.foodPositionRight);
            seat.foodRight = worldItem;
        }

        worldItem.transform.localPosition = Vector3.zero;
        worldItem.transform.localRotation = Quaternion.identity;
        worldItem.transform.localScale = Vector3.one;
    }

    public void RemoveFood(SeatData seat)
    {
        if (seat != null)
        {
            photonView.RPC("RemoveFoodRPC", RpcTarget.All, seat.seatID);
        }
    }

    [PunRPC]
    void RemoveFoodRPC(int seatID)
    {
        SeatData seat = GetSeatByID(seatID);

        if (seat.foodLeft != null)
        {
            ItemData foodData = seat.foodLeft.item.CurrentItemData;
            ItemData usedTool = GetUsedToolItem(foodData.requireToolType);

            var toolItemBase = ItemBase.ItemBaseCreator.CreateItemBase(usedTool);
            WorldItem toolItem = ItemManager.Instance.ItemSpawn(toolItemBase, seat.foodPositionLeft.position, Quaternion.identity);

            toolItem.transform.SetParent(seat.foodPositionLeft);
            toolItem.transform.localPosition = Vector3.zero;
            toolItem.transform.localRotation = Quaternion.identity;
            toolItem.transform.localScale = Vector3.one;

            Destroy(seat.foodLeft.gameObject);
            seat.foodLeft = toolItem;
        }

        if (seat.foodRight != null)
        {
            ItemData foodData = seat.foodRight.item.CurrentItemData;
            ItemData usedTool = GetUsedToolItem(foodData.requireToolType);

            var toolItemBase = ItemBase.ItemBaseCreator.CreateItemBase(usedTool);
            WorldItem toolItem = ItemManager.Instance.ItemSpawn(toolItemBase, seat.foodPositionRight.position, Quaternion.identity);

            toolItem.transform.SetParent(seat.foodPositionRight);
            toolItem.transform.localPosition = Vector3.zero;
            toolItem.transform.localRotation = Quaternion.identity;
            toolItem.transform.localScale = Vector3.one;

            Destroy(seat.foodRight.gameObject);
            seat.foodRight = toolItem;
        }
    }

    private ItemData GetUsedToolItem(ERequiredTool type)
    {
        return ItemManager.Instance.GetItemDataByName(type.ToString());
    }
}
